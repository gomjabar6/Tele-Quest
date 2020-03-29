using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CharacterBackend.DBContext;
using CharacterBackend.DBContext.Models;
using CharacterBackend.Models.APIDaze;
using CharacterBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharacterBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("text/xml")]
    public class APIDazeRaidController : ControllerBase
    {
        public TeleQuestContext _context { get; }
        public APIDazeService _apiServie { get; }

        public APIDazeRaidController(TeleQuestContext Context, APIDazeService apiServie)
        {
            this._context = Context;
            this._apiServie = apiServie;
        }

        [HttpGet]
        public async Task<ActionResult> GetRaid(
            [FromQuery(Name = "caller_id_number")] string PhoneNumber, 
            [FromQuery(Name = "session_id")] string SessionId, 
            [FromQuery(Name = "AppCurrentRaid")] string RaidId = null, 
            [FromQuery(Name = "Attacking")] Boolean Attacking = false,
            [FromQuery(Name = "Attack")] int Attack = 0,
            [FromQuery(Name = "DoneAttacking")] Boolean DoneAttack = false
        )
        {

            PhoneNumber = PhoneNumber.Replace("+", "");

            var log = new CallLog()
            {
                Id = Guid.NewGuid(),
                PhoneNumber = PhoneNumber,
                UUID = SessionId,
                Time = DateTime.UtcNow,
            };

            _context.CallLog.Add(log);
            await _context.SaveChangesAsync();


            var user = _context.Users.Where(u => u.PhoneNumber == PhoneNumber).FirstOrDefault();

            // Handle no known user
            if (user == null)
            {
                return File(Encoding.UTF8.GetBytes(getUnknownDoc().OuterXml), "text/xml");
            }

            // Check if inside an actual raid
            if (RaidId != null)
            {

                var raidIdGuid = Guid.Empty;

                try {
                    raidIdGuid = Guid.Parse(RaidId);
                } catch
                {
                    return File(Encoding.UTF8.GetBytes(getNoRaid(user).OuterXml), "text/xml");
                }


                var activeRaid = await _context.Raid.FindAsync(raidIdGuid);

                if (activeRaid == null)
                {
                    return File(Encoding.UTF8.GetBytes(getNoRaid(user).OuterXml), "text/xml");
                }

                if (DoneAttack == true)
                {
                    var raiders = await _context.UserRaids.Where(ur => ur.RaidId == activeRaid.Id).ToListAsync();
                    return File(Encoding.UTF8.GetBytes((await getConclueRaidDoc(activeRaid, raiders, user)).OuterXml), "text/xml");
                }

                if (Attacking == true && Attack > 0)
                {
                    return File(Encoding.UTF8.GetBytes((await getConcludeAttackDocAsync(activeRaid, Attack, user)).OuterXml), "text/xml");
                }

                if (Attacking == true)
                {
                    return File(Encoding.UTF8.GetBytes(getStartAttackDoc(activeRaid).OuterXml), "text/xml");
                }

                var activeRaiders = await _apiServie.RefreshUserRaid();
                return File(Encoding.UTF8.GetBytes(getStartRaidDoc(activeRaid, activeRaiders).OuterXml), "text/xml");

            }


            // Get the next raid
            var raid = _context.Raid.Where(r => r.Date >= DateTime.UtcNow).OrderBy(r => r.Date).Take(1).FirstOrDefault();

            if (raid == null)
            {
                return File(Encoding.UTF8.GetBytes(getNoRaid(user).OuterXml), "text/xml");
            }

            var RaidUser = new UserRaid();
            RaidUser.RaidId = raid.Id;
            RaidUser.UserId = user.Id;

            _context.UserRaids.Add(RaidUser);
            await _context.SaveChangesAsync();

            var userRaids = await _apiServie.RefreshUserRaid();

            return File(Encoding.UTF8.GetBytes(getWelcomeDoc(user, raid, userRaids).OuterXml), "text/xml");
        }

        private async Task<XmlDocument> getConclueRaidDoc(Raid raid, List<UserRaid> raiders, User user)
        {

            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));


            work.AppendChild(addSpeak(doc, "The raid has ended"));

            var totalDamage = raiders.Sum(r => r.damage);

            if (totalDamage >= raid.XpLevel)
            {
                user.ExpPoints += raid.XpReward;
                await _context.SaveChangesAsync();

                work.AppendChild(addSpeak(doc, $"The {raid.Name} is destroyed!"));
                work.AppendChild(addSpeak(doc, $"{user.Username} gains {raid.XpReward} x p"));
            } else
            {
                user.ExpPoints -= raid.XpPenalty;
                await _context.SaveChangesAsync();

                work.AppendChild(addSpeak(doc, $"The evil {raid.Name} is victorious!"));
                work.AppendChild(addSpeak(doc, $"{user.Username} looses {raid.XpPenalty} x p"));

            }
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, $"Your party returns to the tavern to discuss the raid"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, "Entering Tavern Now"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addConference(doc, "RaidRoom"));

            return doc;
        }

        private async Task<XmlDocument> getConcludeAttackDocAsync(Raid raid, int Attack, User user)
        {
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));


            var UserRaid = await _context.UserRaids.Where(ur => ur.UserId == user.Id && ur.RaidId == raid.Id).FirstOrDefaultAsync();
            if (UserRaid == null)
            {
                UserRaid = new UserRaid();
                UserRaid.Id = Guid.NewGuid();
                UserRaid.UserId = user.Id;
                UserRaid.RaidId = raid.Id;
                _context.UserRaids.Add(UserRaid);
            }

            var rand = new Random();
            var critAttack = rand.Next(1, 3);

            if (Attack == critAttack)
            {
                UserRaid.damage = user.ExpPoints * 2;
                work.AppendChild(addSpeak(doc, $"Your attack was a critical hit! You contributed {UserRaid.damage} damage to the monster!"));
            } else
            {
                UserRaid.damage = user.ExpPoints;
                work.AppendChild(addSpeak(doc, $"You hit the monster! You contributed {UserRaid.damage} damage to the monster!"));
            }

            work.AppendChild(addSpeak(doc, $"Please wait for the rest of the party to finish attacking the monster"));
            work.AppendChild(addWait(doc, 10));
            work.AppendChild(addDialURL(doc, $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&DoneAttacking=true"));

            return doc;
        }

        private XmlDocument getStartAttackDoc(Raid raid)
        {
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));

            work.AppendChild(addSpeak(doc, $"You approach the {raid.Name}."));

            var bindings = new List<Bindings>();
            bindings.Add(new Bindings() { Choice = 1, URL = $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&Attacking=true&Attack=1" });
            bindings.Add(new Bindings() { Choice = 2, URL = $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&Attacking=true&Attack=2" });
            bindings.Add(new Bindings() { Choice = 3, URL = $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&Attacking=true&Attack=3" });

            work.AppendChild(addSpeak(doc, "Press 1 to direct attack, 2 to feint attack, 3 to range attack", 7000, bindings));
            work.AppendChild(addDialURL(doc, $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&Attacking=true"));

            return doc;
        }

        private XmlDocument getStartRaidDoc(Raid raid, List<UserRaid> raiders)
        {
            var totalXp = raiders.Select(ur => ur.User).Sum(u => u.ExpPoints);

            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));

            work.AppendChild(addSpeak(doc, "The raid begins!"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, $"Your party ventures forth to challenge the mighty {raid.Name}"));
            work.AppendChild(addSpeak(doc, $"The {raid.Name}, requires {raid.XpLevel} x p to defeat, your party has a total of { totalXp } x p "));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, $"Your party approaches the {raid.Name}'s den"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, $"Your party takes aim at the monster"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addDialURL(doc, $"https://telequestbackend.azurewebsites.net/api/APIDazeRaid/GetRaid?AppCurrentRaid={raid.Id}&Attacking=true"));

            return doc;
        }

        private XmlDocument getWelcomeDoc(User user, Raid raid, List<UserRaid> userRaids)
        {

            var totalXp = userRaids.Select(ur => ur.User).Sum(u => u.ExpPoints);
            var timeToRaid = raid.Date - DateTime.Now;


            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));

            work.AppendChild(addSpeak(doc, $"Hello, {user.Username}, welcome to the raid! You have {user.ExpPoints.ToString()} x p"));
            work.AppendChild(addSpeak(doc, $"This raid needs {raid.XpLevel.ToString()} total x p to complete and rewards {raid.XpReward.ToString()} x p"));
            work.AppendChild(addSpeak(doc, $"A failure will result in a loss of {raid.XpPenalty.ToString()} x p"));
            work.AppendChild(addSpeak(doc, $"This raid concludes in {timeToRaid.Hours} hours and {timeToRaid.Minutes} minutes"));
            work.AppendChild(addSpeak(doc, $"There are currently {userRaids.Count().ToString()} adventurers in the tavern with a total of {totalXp.ToString()} x p"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, "Entering Tavern Now"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addConference(doc, "RaidRoom"));

            return doc;
        }

        private XmlDocument getNoRaid(User user)
        {
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));

            work.AppendChild(addSpeak(doc, $"Hello, {user.Username}, There is no active raid right now, try again later!"));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, "Goodbye;"));
            work.AppendChild(addHangup(doc));

            return doc;
        }

        private XmlDocument getUnknownDoc()
        {
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement document = doc.CreateElement(string.Empty, "document", string.Empty);
            doc.AppendChild(document);

            XmlElement work = doc.CreateElement(string.Empty, "work", string.Empty);
            document.AppendChild(work);

            work.AppendChild(doc.CreateElement(string.Empty, "answer", string.Empty));

            work.AppendChild(addSpeak(doc, "Sorry, I don't know who you are yet. Text 970 289 4651 to create your character."));
            work.AppendChild(addWait(doc, 1));
            work.AppendChild(addSpeak(doc, "Goodbye"));
            work.AppendChild(addHangup(doc));

            return doc;
        }

        private XmlElement addSpeak(XmlDocument doc, string Text, int Timeout = 0, List<Bindings> bindings = null)
        {

            XmlElement speak = doc.CreateElement(string.Empty, "speak", string.Empty);
            if (Timeout > 0)
            {
                speak.SetAttribute("input-timeout", Timeout.ToString());
            } 

            XmlText text1 = doc.CreateTextNode(Text);
            speak.AppendChild(text1);

            if (Timeout > 0)
            {
                foreach (var binding in bindings)
                {
                    speak.AppendChild(addBind(doc, binding));
                }
            }
            return speak;
        }

        private XmlElement addDialURL(XmlDocument doc, string URL)
        {
            XmlElement dialEl = doc.CreateElement(string.Empty, "dial", string.Empty);
            XmlElement urlEl = doc.CreateElement(string.Empty, "action", string.Empty);
            XmlText text1 = doc.CreateTextNode(URL);
            urlEl.AppendChild(text1);
            dialEl.AppendChild(urlEl);

            return dialEl;
        }

        private XmlElement addBind(XmlDocument doc, Bindings binding)
        {
            XmlElement bindingEl = doc.CreateElement(string.Empty, $"bind", string.Empty);
            bindingEl.SetAttribute("action", binding.URL);
            XmlText text1 = doc.CreateTextNode((string)binding.Choice.ToString());
            bindingEl.AppendChild(text1);
            return bindingEl;
        }

        private XmlElement addConference(XmlDocument doc, string RoomName)
        {
            XmlElement speak = doc.CreateElement(string.Empty, "conference", string.Empty);
            XmlText text1 = doc.CreateTextNode(RoomName);
            speak.AppendChild(text1);
            return speak;
        }


        private XmlElement addHangup(XmlDocument doc)
        {

            XmlElement hangup = doc.CreateElement(string.Empty, "hangup", string.Empty);
            return hangup;

        }

        private XmlElement addWait(XmlDocument doc, int length)
        {

            XmlElement wait = doc.CreateElement(string.Empty, "wait", string.Empty);
            XmlText text2 = doc.CreateTextNode(length.ToString());
            wait.AppendChild(text2);
            return wait;

        }
    }


}