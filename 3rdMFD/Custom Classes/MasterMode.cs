using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _3rdMFD
{
    class MasterMode : MonoBehaviour, IQSVehicleComponent
    {
        public WeaponManager wm;

        private Battery battery;
        private AttackMode currentMode = AttackMode.Disabled;

        public UIImageToggle allToggle;
        public UIImageToggle airToggle;
        public UIImageToggle groundToggle;

        private void Start()
        {
            GameObject fa = base.transform.root.gameObject;
            this.wm = fa.GetComponentInChildren<WeaponManager>(true);

            PoweredAudioPoolSource pool = Main.FindInteractable(fa, "All AP Off").gameObject.GetComponent<PoweredAudioPoolSource>();


            VRInteractable masterAll = Main.FindInteractable(fa, "All Master Mode");
            masterAll.OnInteract.AddListener(SelectAll);
            masterAll.OnInteract.AddListener(pool.PlaySound);

            VRInteractable masterAir = Main.FindInteractable(fa, "Air Master Mode");
            masterAir.OnInteract.AddListener(SelectAir);
            masterAir.OnInteract.AddListener(pool.PlaySound);

            VRInteractable masterGround = Main.FindInteractable(fa, "Ground Master Mode");
            masterGround.OnInteract.AddListener(SelectGround);
            masterGround.OnInteract.AddListener(pool.PlaySound);

            this.allToggle = masterAll.gameObject.GetComponent<UIImageToggle>();
            this.airToggle = masterAir.gameObject.GetComponent<UIImageToggle>();
            this.groundToggle = masterGround.gameObject.GetComponent<UIImageToggle>();

            this.battery = fa.GetComponentInChildren<Battery>();

             
        }

        public virtual void OnQuicksave(ConfigNode qsNode)
        {
            ConfigNode configNode = qsNode.AddNode("MasterMode");
            configNode.SetValue<MasterMode.AttackMode>("attackMode", this.currentMode);
        }

        // Token: 0x06004532 RID: 17714 RVA: 0x00189F04 File Offset: 0x00188104
        public virtual void OnQuickload(ConfigNode qsNode)
        {
            ConfigNode node = qsNode.GetNode("MasterMode");
            if (node != null)
            {
                this.currentMode = node.GetValue<MasterMode.AttackMode>("attackMode");
                switch (this.currentMode)
                {
                    case AttackMode.Disabled:
                        break;
                    case AttackMode.All:
                        this.SelectAll();
                        break;
                    case AttackMode.Air:
                        this.SelectAir();
                        break;
                    case AttackMode.Ground:
                        this.SelectGround();
                        break;   

                }
            }
        }



        private void FixedUpdate()
        {
            if (this.battery && !this.battery.Drain(0))
            {
                this.allToggle.imageEnabled = false;
                this.groundToggle.imageEnabled = false;
                this.airToggle.imageEnabled = false;

                this.currentMode = AttackMode.Disabled;
            }
            else if(this.currentMode == AttackMode.Disabled)
            {
                this.currentMode = AttackMode.All;
                this.SelectAll();
                this.SetAttackMode();
            }
        }
        private void SetAttackMode()
        {
            if (this.wm.isFiring)
            {
                this.wm.EndAllFire();
            }

            switch (this.currentMode)
            {
                case AttackMode.Air:
                    for (int i = 0; i < this.wm.equipCount; i++)
                    {
                        if (this.wm.GetEquip(i) != null)
                        {
                            if (this.wm.GetEquip(i).weaponType == HPEquippable.WeaponTypes.AAM && this.wm.GetEquip(i).armable && this.wm.GetEquip(i).GetCount() > 0)
                            {
                                this.wm.GetEquip(i).armed = true;

                            }
                            else if (this.wm.GetEquip(i).armable && this.wm.GetEquip(i).weaponType != HPEquippable.WeaponTypes.Gun)
                            {
                                this.wm.GetEquip(i).armed = false;

                            }
                            this.wm.RefreshWeapon();
                        }


                    }
                    break;

                case AttackMode.Ground:
                    for (int i = 0; i < this.wm.equipCount; i++)
                    {
                        if (this.wm.GetEquip(i) != null)
                        {
                            if (this.wm.GetEquip(i).weaponType != HPEquippable.WeaponTypes.AAM && this.wm.GetEquip(i).armable && this.wm.GetEquip(i).GetCount() > 0)
                            {
                                this.wm.GetEquip(i).armed = true;

                            }
                            else if (this.wm.GetEquip(i).armable && this.wm.GetEquip(i).weaponType != HPEquippable.WeaponTypes.Gun)
                            {
                                this.wm.GetEquip(i).armed = false;

                            }
                            this.wm.RefreshWeapon();
                        }



                    }
                    break;
                case AttackMode.All:
                    for (int i = 0; i < this.wm.equipCount; i++)
                    {
                        if (this.wm.GetEquip(i) != null)
                        {
                            if (this.wm.GetEquip(i).armable && this.wm.GetEquip(i).GetCount() > 0)
                            {
                                this.wm.GetEquip(i).armed = true;

                            }
                            else if (this.wm.GetEquip(i).armable && this.wm.GetEquip(i).weaponType != HPEquippable.WeaponTypes.Gun)
                            {
                                this.wm.GetEquip(i).armed = false;

                            }
                            this.wm.RefreshWeapon();
                        }



                    }

                    break;

                default:
                    break;
            }
          

        }

        private void SelectAll()
        {
            if(this.currentMode == AttackMode.Disabled)
                return;
            

            this.currentMode = AttackMode.All;

            this.allToggle.imageEnabled = this.currentMode == AttackMode.All;
            this.groundToggle.imageEnabled = this.currentMode != AttackMode.All;
            this.airToggle.imageEnabled = this.currentMode != AttackMode.All;

            this.SetAttackMode();
        }

        private void SelectAir()
        {
            if (this.currentMode == AttackMode.Disabled)
                return;


            this.currentMode = AttackMode.Air;

            this.airToggle.imageEnabled = this.currentMode == AttackMode.Air;
            this.groundToggle.imageEnabled = this.currentMode != AttackMode.Air;
            this.allToggle.imageEnabled = this.currentMode != AttackMode.Air;




            this.SetAttackMode();
        }

        private void SelectGround()
        {
            if (this.currentMode == AttackMode.Disabled)
                return;


            this.currentMode = AttackMode.Ground;

            this.groundToggle.imageEnabled = this.currentMode == AttackMode.Ground;
            this.airToggle.imageEnabled = this.currentMode != AttackMode.Ground;
            this.allToggle.imageEnabled = this.currentMode != AttackMode.Ground;


            this.SetAttackMode();
        }

        public enum AttackMode
        {
            All, 
            Air, 
            Ground,
            Disabled
        }

    }
}
