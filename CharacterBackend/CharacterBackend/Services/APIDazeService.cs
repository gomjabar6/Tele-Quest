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
        private string apiDazeEndpoint = "https://cpaas-api.voipinnovations.com/68ansd45/calls";
        private string apiDazeEndpointOld = "https://api.apidaze.io/68ansd45/calls";
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

            var url = this.apiDazeEndpointOld + apiSeceret;
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
            //values.Add(new KeyValuePair<string, string>("url", path));

            var url = this.apiDazeEndpoint + "/" + uuid + "/transfer" + apiSeceret;
            var client = new HttpClient();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(values));

            return response;
        }

        public async Task<List<Raid>> RefreshUserRaid()
        {
            var calls = await GetActiveCalls();

            List<Raid> activeRaids = new List<Raid>();

            foreach (var call in calls)
            {
                var userRaid = _context.UserRaids.Where(ur => ur.CallId == call.uuid).Include(ur => ur.Raid).FirstOrDefault();
                
                if (userRaid != null)
                {
                    if (activeRaids.Where(r => r.Id == userRaid.Raid.Id).ToList().Count() == 0)
                    {
                        activeRaids.Add(userRaid.Raid);
                    }

                }
            }

            foreach (var raid in activeRaids)
            {
                var UserRaids = await _context.UserRaids.Where(ur => ur.RaidId == raid.Id).Include(u => u.User).ToListAsync();

                foreach (var userRaid in UserRaids)
                {
                    var callUser = calls.Where(c => c.uuid == userRaid.CallId).FirstOrDefault();

                    if (callUser == null)
                    {
                        _context.UserRaids.Remove(userRaid);
                    }
                }

            }

            await _context.SaveChangesAsync();

            return activeRaids;
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
            var activeRaids = await RefreshUserRaid();

            var calls = await GetActiveCalls();
            calls = calls.Where(c => c.work_tag == "<conference/>").ToList();

            var tasks = new List<Task>();
            foreach (var call in calls)
            {
                tasks.Add(new Task(() => this.TransferCall(call.uuid, $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid")));
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);
        }
    }
}
