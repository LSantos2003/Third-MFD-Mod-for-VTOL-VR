using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _3rdMFD
{
    
    class LPI : MonoBehaviour
    {
        public Radar radar;

        private void Start()
        {
            radar = base.transform.GetComponentInChildren<Radar>(true);

            radar.OnDetectedActor += LogRadarSignal;
            
        }

        private void LogRadarSignal(Actor a)
        {
           // float ss = Radar.GetRadarSignalStrength(radar.transform.position, a);
            //FlightLogger.Log($"Signal strength of {a.actorName}: {ss} ");
        }

    }
}
