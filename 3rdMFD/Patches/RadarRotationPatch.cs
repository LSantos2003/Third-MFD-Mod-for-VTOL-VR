using Harmony;
using UnityEngine;
namespace _3rdMFD.Patches
{
    /*[HarmonyPatch(typeof(Radar), nameof(Radar.UpdateRotation))]
    class RadarRotationPatch
    {
        public static void Postfix(Radar __instance)
        {
            if(__instance == RadarSlew.playerRadar && RadarSlew.slew)
            {
                Traverse radarTraverse = Traverse.Create(__instance);

                if (__instance.radarEnabled || !__instance.allowRotation)
                {
                    float angle = (float)radarTraverse.Field("angle").GetValue();

                    __instance.rotationTransform.localRotation = Quaternion.Euler(0f, angle + RadarSlew.currentOffset, 0f);
                }

            }
           

        }
    }*/
}
