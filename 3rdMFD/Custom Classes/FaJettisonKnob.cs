using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _3rdMFD
{
    class FaJettisonKnob : MonoBehaviour
    {
        private JettisonModes currentJettMode;
        public WeaponManager wm;
        public VRTwistKnobInt twistKnob;

        public enum JettisonModes
        {
            // Token: 0x04004040 RID: 16448
            All,
            Selected,
            Tanks,
            Empty
        }

        private void Start()
        {
            twistKnob.onPushButton.AddListener(JettisonButton);
            twistKnob.OnSetState.AddListener(markForJett);
            this.markForJett(this.twistKnob.currentState);
        }

        private void markForJett(int st)
        {
            if (st == 0)
            {
                this.currentJettMode = JettisonModes.All;
                //FlightLogger.Log("All marked");
            }
            else if (st == 1)
            {
                this.currentJettMode = JettisonModes.Tanks;
                //FlightLogger.Log("Tanks marked");

            }
            else if (st == 2)
            {
                this.currentJettMode = JettisonModes.Empty;
                //FlightLogger.Log("Empty marked");
            }
            else if (st == 3)
            {
                this.currentJettMode = JettisonModes.Selected;
                //FlightLogger.Log("Selected Marked");
            }


        }

        private void JettisonButton()
        {
            switch (this.currentJettMode)
            {
                case JettisonModes.All:
                    for (int i = 0; i < this.wm.equipCount; i++)
                    {
                        this.wm.MarkAllJettison();
                        this.wm.JettisonMarkedItems();
                    }
                    break;
                case JettisonModes.Selected:
                    {
                        this.wm.JettisonMarkedItems();
                        break;
                    }
                case JettisonModes.Tanks:
                    this.wm.MarkDroptanksToJettison();
                    this.wm.JettisonMarkedItems();
                    break;
                case JettisonModes.Empty:
                    this.wm.MarkEmptyToJettison();
                    this.wm.JettisonMarkedItems();
                    break;
            }



        }




    }
}
