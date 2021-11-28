using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _3rdMFD
{
/*
    [HarmonyPatch(typeof(MFDRadarUI), "Update")]
    class RadarUIPatch
    { 
        public static void Postfix(MFDRadarUI __instance)
        {

			if (((__instance.mfdPage && __instance.mfdPage.isOpen) || (__instance.portalPage && __instance.portalPage.pageState != MFDPortalPage.PageStates.Minimized && __instance.portalPage.pageState != MFDPortalPage.PageStates.SubSized)) && __instance.playerRadar.radarEnabled)
			{
                __instance.scanLineTf.localRotation = Quaternion.Euler(0f, 0f,0f);
                float mappedPosition = Main.map(__instance.playerRadar.currentAngle, -60f, 60f, -50f, 50f);
                __instance.scanLineTf.localPosition = new Vector3(mappedPosition, -2.4f, 0);
			}
			
        }
    }

    [HarmonyPatch(typeof(MFDRadarUI), "UpdateActorIcon")]
    class RadarActorUpdatePatch
    {

        public static bool Prefix(MFDRadarUI __instance, MFDRadarUI.UIRadarContact contact, bool resetTime)
        {
            if (resetTime)
            {
                contact.detectedPosition.point = contact.actor.position;
                contact.detectedVelocity = contact.actor.velocity;
                contact.timeFound = Time.time;
            }

            contact.iconObject.transform.localPosition = ContactAngle(contact.detectedPosition.point, __instance);
            contact.iconObject.transform.localRotation = Quaternion.LookRotation(Vector3.forward, __instance.WorldToRadarDirection(contact.detectedPosition.point, contact.detectedVelocity));

            return false;
        }

        private static Vector3 ContactAngle(Vector3 worldPoint, MFDRadarUI radarUi)
        {
            worldPoint.y = radarUi.playerRadar.transform.position.y;
            Vector3 forward = radarUi.playerRadar.transform.forward;
            forward.y = 0f;
            float angle = VectorUtils.SignedAngle(forward, worldPoint - radarUi.playerRadar.transform.position, Vector3.Cross(Vector3.up, forward)) * (radarUi.uiAngle / radarUi.playerRadar.rotationRange);

            float mappedAngle = Main.map(radarUi.playerRadar.currentAngle, -60f, 60f, -50f, 50f);

            float distance = Vector3.Distance(worldPoint, radarUi.playerRadar.transform.position);
            distance *= radarUi.uiHeight / radarUi.viewRanges[radarUi.viewRangeIdx]; ;

            return new Vector3(mappedAngle, distance, 0f);
        }
    }*/
}
