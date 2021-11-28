using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace _3rdMFD
{
    class ShipRadarManager : MonoBehaviour
    {
        public MFDRadarUI radarUi;
        public WeaponManager wm;

        public MFDPage radarPage;

        private bool seaRadarOn = false;
        private UILineRenderer scanLine;
        private UILineRenderer hardLockLine;

        private static Color greenLine = new Color(0, 0.6323529f, 0.05669371f, 0.09803922f);
        private static Color orangeLine = new Color(1, 0.5591404f, 0, 0.09803922f);

        private static Color airHardLock = new Color(0.5147059f, 1, 0.6184584f, 0.1843137f);
        private static Color seaHardLock = new Color(1, 0.6670619f, 0.3443396f, 0.1843137f);
        private void Awake()
        {
            radarUi = base.transform.root.GetComponentInChildren<MFDRadarUI>();
            wm = base.transform.root.GetComponentInChildren<WeaponManager>();
            scanLine = this.radarUi.scanLineTf.GetComponent<UILineRenderer>();
            hardLockLine = this.radarUi.hardLockLine.GetComponent<UILineRenderer>();
        }

        private void SetSeaRadar(bool state)
        {
            this.seaRadarOn = state;
            this.radarUi.playerRadar.detectShips = state;
            this.radarUi.playerRadar.detectAircraft = !state;

            this.scanLine.color = this.seaRadarOn ? orangeLine : greenLine;
            this.hardLockLine.color = this.seaRadarOn ? seaHardLock : airHardLock;
            //this.radarPage.SetText("seaText", "SEA", this.seaRadarOn ? Color.green : Color.white);

            FlightLogger.Log(this.seaRadarOn ? "Radar in sea mode" : "Radar in air mode");
        }

        public void ToggleRadar()
        {
            this.SetSeaRadar(!this.seaRadarOn);
            this.radarUi.ClearSoftLocks();
        }
        
        public void SendGPS()
        {
            if (this.seaRadarOn && this.radarUi.lockingRadar.IsLocked())
            {
                if (this.wm.gpsSystem.noGroups)
                {
                    this.wm.gpsSystem.CreateCustomGroup();
                }

                float leadTime = Vector3.Distance(wm.actor.position, this.radarUi.lockingRadar.currentLock.actor.position) / 343;
                Vector3 leadPosition = this.radarUi.lockingRadar.currentLock.actor.position + (this.radarUi.lockingRadar.currentLock.actor.velocity * leadTime);
                this.wm.gpsSystem.AddTarget(leadPosition, "RDR");
                return;
            }
        }


    }
}
