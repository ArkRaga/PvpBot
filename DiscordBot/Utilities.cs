using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DiscordBot
{
    class Utilities
    {

        private static Dictionary<string, string> alerts;
        
        public static ulong TestChan = 616035781437292650;
        public static ulong GenChan = 605871435147247801;
        public static ulong GuildChan = 605871435134664848;

        static Utilities()
        {

            string json = File.ReadAllText("SystemLang/alerts.json");
 
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            alerts = data.ToObject<Dictionary<string, string>>();
            
        }

        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";

        }
       
        public static string GetFormattedAlert(string key, params object[] parameter)
        {

            if (alerts.ContainsKey(key))
            {
                return string.Format(alerts[key], parameter);
            }
            return "";

        }
        public static string GetFormattedAlert(string key, object parameter)
        {

            return GetFormattedAlert(key, new object[] { parameter });

        }
    }
}