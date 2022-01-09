using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _3rdMFD
{
    class RadarSlew : MonoBehaviour
    {
        public static Radar playerRadar;

        public static float currentOffset = 0;

        public static bool slew { get; private set; }


        private MFDRadarUI radarUI;
        private Traverse radarUITraverse;

        private MFDRadarUI.UIRadarContact contactToSlew;

        private GameObject slewFovTf;

        private void Start()
        {
            playerRadar = base.transform.GetComponentInChildren<Radar>();
            slew = true;
        }


        public void SlewTargetButton(int idx)
        {
            MFDRadarUI.UIRadarContact uiradarContact = radarUI.softLocks[idx];
            if (uiradarContact != null && uiradarContact.active)
            {
               
            }
        }

        private void SelectTargetToSlew(MFDRadarUI.UIRadarContact uiradarContact)
        {
            this.contactToSlew = uiradarContact;
        }

        private void UpdateSlew()
        {
            if (this.contactToSlew != null && this.contactToSlew.active)
            {
                this.slewFovTf.transform.localRotation = Quaternion.LookRotation(Vector3.forward, this.contactToSlew.iconObject.transform.localPosition);
            }
        }




    }
}
