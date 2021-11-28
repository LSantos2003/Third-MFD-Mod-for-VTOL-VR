using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _3rdMFD
{
    [HarmonyPatch(typeof(MFDPage), "Awake")]
    class Patch_RADAR
    {
        public static bool Prefix(MFDPage __instance)
        {

            if (__instance.pageName == "radar")
            {
                ShipRadarManager shipRadar = __instance.transform.root.gameObject.AddComponent<ShipRadarManager>();
                int newSize = __instance.buttons.Length + 2;
                Array.Resize(ref __instance.buttons, newSize);

                MFDPage.MFDButtonInfo gpsButton = new MFDPage.MFDButtonInfo();
                gpsButton.button = MFD.MFDButtons.T4;
                gpsButton.label = "GPS-S";
                gpsButton.toolTip = "GPS Send";
                gpsButton.OnPress = new UnityEvent();
                gpsButton.OnPress.AddListener(shipRadar.SendGPS);

                MFDPage.MFDButtonInfo seaButton = new MFDPage.MFDButtonInfo();
                seaButton.button = MFD.MFDButtons.T3;
                seaButton.label = "SEA";
                seaButton.toolTip = "SEA Radar toggle";
                seaButton.OnPress = new UnityEvent();
                seaButton.OnPress.AddListener(shipRadar.ToggleRadar);

                shipRadar.radarPage = __instance;


                __instance.buttons[newSize - 1] = gpsButton;
                __instance.buttons[newSize - 2] = seaButton;

            }
            return true;
        }
    }
}
