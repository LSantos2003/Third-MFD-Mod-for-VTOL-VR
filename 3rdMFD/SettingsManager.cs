using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
using System.Reflection;
using Valve.Newtonsoft.Json;
using System.IO;
using UnityEngine.Events;

namespace _3rdMFD
{
    public class ThirdMfdSettings
    {
        public bool switchMMFD = false;
    }

    public static class SettingsManager
    {
        public static VTOLMOD mod;

        public static ThirdMfdSettings settings;

        public static bool settingsChanged;


        public static UnityAction<bool> mmfdChanged;

        public static void SetupSettingsMenu(VTOLMOD mod)
        {
            SettingsManager.mod = mod;

            settings = new ThirdMfdSettings();
            LoadSettingsFromFile();
            Settings modSettings = new Settings(mod);
            modSettings.CreateCustomLabel("Block 3 Settings");
            modSettings.CreateCustomLabel("");

            mmfdChanged += mmfd_Setting;
            modSettings.CreateCustomLabel("Switch MMFD positions");
            modSettings.CreateBoolSetting("(Default = false)", mmfdChanged, settings.switchMMFD);

            VTOLAPI.CreateSettingsMenu(modSettings);
        }

        public static void CheckSave()
        {
            Debug.Log("Checking if settings were changed.");
            if (settingsChanged)
            {
                Debug.Log("Settings were changed, saving changes!");
                SaveSettingsToFile();
            }
        }

        public static void SaveSettingsToFile()
        {
            string address = mod.ModFolder;
            Debug.Log("Checking for: " + address);

            if (Directory.Exists(address))
            {
                Debug.Log("Saving settings!");
                File.WriteAllText(address + @"\settings.json", JsonConvert.SerializeObject(settings));
                settingsChanged = false;
            }
            else
            {
                Debug.Log("Mod folder not found?");
            }
        }

        public static void LoadSettingsFromFile()
        {
            string address = mod.ModFolder;
            Debug.Log("Checking for: " + address);

            if (Directory.Exists(address))
            {
                Debug.Log(address + " exists!");
                try
                {
                    Debug.Log("Checking for: " + address + @"\settings.json");
                    string temp = File.ReadAllText(address + @"\settings.json");

                    settings = JsonConvert.DeserializeObject<ThirdMfdSettings>(temp);
                    settingsChanged = false;
                    Debug.Log("Loaded: " + address + @"\settings.json");
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                    Debug.Log("json not found or invalid, making new one");
                    SaveSettingsToFile();
                }
            }
            else
            {
                Debug.Log("Mod folder not found?");
            }
        }

        public static void mmfd_Setting(bool newval)
        {
            settings.switchMMFD = newval;
            settingsChanged = true;
        }
    }
}
