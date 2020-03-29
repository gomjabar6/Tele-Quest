using CharacterBackend.DBContext;
using CharacterBackend.DBContext.Models;
using CharacterBackend.Models.APIDaze;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharacterBackend.Services
{
    public class APIDazeService
    {
        private string apiDazeEndpoint = "https://api.apidaze.io/68ansd45/calls";
        private string apiSeceret = "?api_secret=08446328b17a983b50536beeacdc85d2";

        public TeleQuestContext _context { get; }

        public APIDazeService(TeleQuestContext context)
        {
            _context = context;
        }

        public async Task PlaceRaidCall(string PhoneNumber)
        {

            var placer = new CallPlacer(PhoneNumber);
            var values = new List<KeyValuePair<string, string>>();

            foreach (var property in placer.GetType().GetProperties())
            {
                values.Add(new KeyValuePair<string, string>(property.Name, property.GetValue(placer, null).ToString()));
            }

            var url = this.apiDazeEndpoint + apiSeceret;
            var client = new HttpClient();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

        }

        public async Task<List<CallList>> GetActiveCalls()
        {
            
            var url = this.apiDazeEndpoint + apiSeceret;
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<CallList>>(jsonString);

            return data;
        }


        public async Task<HttpResponseMessage> EndCall(string uuid)
        {
            var url = this.apiDazeEndpoint + "/" + uuid + apiSeceret;
            var client = new HttpClient();
            var response = await client.DeleteAsync(url);

            return response;
        }

        public async Task<HttpResponseMessage> TransferCall(string uuid, string path)
        {

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("url", path));

            var url = this.apiDazeEndpoint + "/" + uuid + apiSeceret;
            var client = new HttpClient();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

            return response;
        }

        public async Task<List<UserRaid>> RefreshUserRaid()
        {
            var calls = await GetActiveCalls();

            var raid = await _context.Raid.Where(r => r.Date >= DateTime.UtcNow).OrderBy(r => r.Date).Take(1).FirstOrDefaultAsync();

            var UserRaids = await _context.UserRaids.Where(ur => ur.RaidId == raid.Id).Include(u => u.User).ToListAsync();

            foreach (var userRaid in UserRaids)
            {
                var callUser = calls.Where(c => c.cid_num.Replace("+","") == userRaid.User.PhoneNumber).FirstOrDefault();

                if (callUser == null)
                {
                    _context.UserRaids.Remove(userRaid);
                }
            }

            await _context.SaveChangesAsync();

            UserRaids = await _context.UserRaids.Where(ur => ur.RaidId == raid.Id).Include(u => u.User).ToListAsync();

            return UserRaids;
        }

        public async Task EndConference()
        {
            var calls = await GetActiveCalls();

            foreach (var call in calls.Where(c => c.work_tag == "<conference/>"))
            {
                await EndCall(call.uuid);
            }

            return;
        }

        public async Task BeginRaid()
        {
            var calls = await GetActiveCalls();
            calls = calls.Where(c => c.work_tag == "<conference/>").ToList();

            var raid = _context.Raid.Where(r => r.Date >= DateTime.UtcNow).OrderBy(r => r.Date).Take(1).FirstOrDefault();
            if (raid == null)
            {
                throw new Exception("No next raid!");
            }

            foreach (var call in calls){
                await this.TransferCall(call.uuid, $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}");
            }

        }


    }
}
