using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DiscordBot
{
    class DataStorarge
    {
        const string enemyListJsonFile = "SystemLang/EnemyList.json";
        const string playerSavedDataFile = "SystemLang/DataStorage.json";
        const string pvpClassListFile = "SystemLang/PvpClasses.json";


        static DataStorarge()
        {
            

            

        }
        public static void LoadData()
        {

            if (!ValidateStorargeFile(playerSavedDataFile)) return;
            string json = File.ReadAllText(playerSavedDataFile);
            Program.PvpPlayers = JsonConvert.DeserializeObject<List<PvpPlayer>>(json);

            if (!ValidateStorargeFile(enemyListJsonFile)) return;
            string enemyListJson = File.ReadAllText(enemyListJsonFile);
            Program.enemyList = JsonConvert.DeserializeObject<List<Enemy>>(enemyListJson);

            if (!ValidateStorargeFile(pvpClassListFile)) return;
            string classListJson = File.ReadAllText(pvpClassListFile);
            Program.playerClasses = JsonConvert.DeserializeObject<List<PvpClass>>(classListJson);
            
        }
        public static PvpPlayer GetStats(string name) => Program.PvpPlayers.FirstOrDefault(o => o.name == name);
        
        public static void SaveData()
        {

            //Save the data
            string json = JsonConvert.SerializeObject(Program.PvpPlayers, Formatting.Indented);
            File.WriteAllText(playerSavedDataFile, json);
        }
        private static bool ValidateStorargeFile(string file)
        {
            if(!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;

            }
            return true;

        }
    }

}
