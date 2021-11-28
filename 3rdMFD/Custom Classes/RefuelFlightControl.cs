using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _3rdMFD
{
    class RefuelFlightControl : MonoBehaviour
    {
        VRJoystick[] sticks;
        ModuleEngine[] engines;

        private float originalSensitivity;
        private float originalSpoolRate = 0.6f;
        void Start()
        {
            this.sticks = base.gameObject.GetComponentsInChildren<VRJoystick>(true);
            this.originalSensitivity = this.sticks[0].sensitivity;

            this.engines = base.gameObject.GetComponentsInChildren<ModuleEngine>(true);
        }


        public void OnRefuel(int state)
        {
            
            foreach(VRJoystick stick in this.sticks)
            {
                stick.sensitivity = state == 1 ? 0.6f : this.originalSensitivity;
            }

            foreach(ModuleEngine e in this.engines)
            {
                //e.spoolRate = state == 1 ? 0.4f : 0.6f;
            }

            
        }
    }
}
