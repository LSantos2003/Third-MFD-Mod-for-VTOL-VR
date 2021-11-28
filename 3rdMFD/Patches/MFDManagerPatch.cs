using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _3rdMFD
{
    [HarmonyPatch(typeof(MFDManager), "Awake")]
    class MFDManagerPatch
    {
        public static void Prefix(MFDManager __instance)
        {
            FlightLogger.Log("Running mfd manager patch");
            if (VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B || !__instance.gameObject.name.Contains("MMFDManager") || __instance.transform.root.GetComponentInChildren<Radar>().radarSymbol == "custom") { return; }

            FlightLogger.Log("Found mmfd!");

            GameObject fa26 = __instance.transform.root.gameObject;

            GameObject damageMfd = GameObject.Instantiate(Main.BattleDamagePrefab);

            damageMfd.transform.SetParent(__instance.transform);
            damageMfd.transform.localPosition = Vector3.zero;
            damageMfd.transform.localEulerAngles = Vector3.zero;
            damageMfd.transform.localScale = Vector3.one * 1.002996f;

            GameObject leftWingIcon = Main.GetChildWithName(damageMfd, "26_Left_Wing");
            DamageUI leftWingDamage = leftWingIcon.AddComponent<DamageUI>();
            leftWingDamage.sprite = leftWingIcon.GetComponent<SpriteRenderer>();
            leftWingDamage.healths.Add(Main.GetChildWithName(fa26, "wingTipFlexLeft").GetComponent<Health>());
            leftWingDamage.healths.Add(Main.GetChildWithName(fa26, "wingLeftPart").GetComponent<Health>());

            GameObject rightWingIcon = Main.GetChildWithName(damageMfd, "26_Right_Wing");
            DamageUI rightWingDamage = rightWingIcon.AddComponent<DamageUI>();
            rightWingDamage.sprite = rightWingIcon.GetComponent<SpriteRenderer>();
            rightWingDamage.healths.Add(Main.GetChildWithName(fa26, "wingTipFlexRight").GetComponent<Health>());
            rightWingDamage.healths.Add(Main.GetChildWithName(fa26, "wingRightPart").GetComponent<Health>());

            GameObject mainBodyIcon = Main.GetChildWithName(damageMfd, "26_Whole");
            DamageUI mainBodyDamage = mainBodyIcon.AddComponent<DamageUI>();
            mainBodyDamage.sprite = mainBodyIcon.GetComponent<SpriteRenderer>();
            mainBodyDamage.healths.Add(fa26.GetComponent<Health>());

            GameObject rightElevatorIcon = Main.GetChildWithName(damageMfd, "26_Right_Elevator");
            DamageUI rightElevatorDamage = rightElevatorIcon.AddComponent<DamageUI>();
            rightElevatorDamage.sprite = rightElevatorIcon.GetComponent<SpriteRenderer>();
            rightElevatorDamage.healths.Add(Main.GetChildWithName(fa26, "elevonRightPart").GetComponent<Health>());


            GameObject leftElevatorIcon = Main.GetChildWithName(damageMfd, "26_Left_Elevator");
            DamageUI leftElevatorDamage = leftElevatorIcon.AddComponent<DamageUI>();
            leftElevatorDamage.sprite = leftElevatorIcon.GetComponent<SpriteRenderer>();
            leftElevatorDamage.healths.Add(Main.GetChildWithName(fa26, "elevonLeftPart").GetComponent<Health>());


            GameObject rightEngineIcon = Main.GetChildWithName(damageMfd, "26_Right_Engine");
            DamageUI rightEngineDamage = rightEngineIcon.AddComponent<DamageUI>();
            rightEngineDamage.sprite = rightEngineIcon.GetComponent<SpriteRenderer>();
            rightEngineDamage.healths.Add(Main.GetChildWithName(fa26, "fa26-rightEngine").GetComponent<Health>());

            GameObject leftEngineIcon = Main.GetChildWithName(damageMfd, "26_Left_Engine");
            DamageUI leftEngineDamage = leftEngineIcon.AddComponent<DamageUI>();
            leftEngineDamage.sprite = leftEngineIcon.GetComponent<SpriteRenderer>();
            leftEngineDamage.healths.Add(Main.GetChildWithName(fa26, "fa26-leftEngine").GetComponent<Health>());
            

        }
    }
}
