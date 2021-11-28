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



    [HarmonyPatch(typeof(Actor), "Start")]
    class PlayerSpawnPatch
    {
        public static void Postfix(Actor __instance)
        {
            if (VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B || !__instance.isPlayer || __instance.gameObject.GetComponentInChildren<Radar>().radarSymbol == "custom")
            {
                return;
            }
            GameObject go = __instance.gameObject;

            Main.FindSwitchObjects(go);

            UnityMover mover = go.AddComponent<UnityMover>();
            mover.gs = go;
            mover.load(true);

            MFDManager manager = Main.GetChildWithName(__instance.gameObject, "MFD1").GetComponentInChildren<MFDManager>(true);




            manager.gameObject.SetActive(false);
            GameObject mfd2 = manager.mfds[1].gameObject;
            GameObject mfd3 = GameObject.Instantiate(mfd2, mfd2.transform);

            mfd3.transform.SetParent(mfd2.transform.parent);

            mfd3.gameObject.GetComponent<RectTransform>().position = mfd2.gameObject.GetComponent<RectTransform>().position;
            mfd3.gameObject.GetComponent<RectTransform>().localScale = mfd2.gameObject.GetComponent<RectTransform>().localScale;
            mfd3.gameObject.GetComponent<RectTransform>().eulerAngles = mfd2.gameObject.GetComponent<RectTransform>().eulerAngles;
            // mfd3.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -143f, 39.6f);

            ((RectTransform)mfd3.transform).localPosition = new Vector3(0f, -149f, 46f);
            //mfd3.gameObject.transform.localPosition = new Vector3(mfd3.gameObject.transform.localPosition.x, mfd3.gameObject.transform.localPosition.y, 1f);
            //mfd3.AddComponent<Transform>();

            //Transform.Destroy(mfd3.GetComponent<RectTransform>());
            VRInteractable[] interactables = mfd3.GetComponentsInChildren<VRInteractable>();

            foreach (var intact in interactables)
            {

                Vector3 posesize = new Vector3();
                bool resize = false;
                if (intact.poseBounds != null && Main.SEAT_ADJUST_POSE_BOUNDS != null)
                {
                    if (intact.poseBounds.pose != GloveAnimation.Poses.Joystick || intact.poseBounds.pose != GloveAnimation.Poses.Throttle || intact.poseBounds.pose != GloveAnimation.Poses.JetThrottle)
                    {
                        resize = true;
                        posesize = intact.poseBounds.size;
                        GameObject newBounds = GameObject.Instantiate(Main.SEAT_ADJUST_POSE_BOUNDS, go.transform);
                        newBounds.name = "new" + mfd3.transform.parent.name;
                        newBounds.transform.position = intact.transform.parent.gameObject.transform.position;
                        newBounds.transform.eulerAngles = intact.transform.parent.transform.eulerAngles;
                        intact.poseBounds = newBounds.GetComponent<PoseBounds>(); //Assigns bounds for switch
                        if (resize) intact.poseBounds.size = posesize;
                    }


                }




            }

            /*GameObject speedometer = go.GetComponentInChildren<DashSpeedometer>(true).gameObject;
            foreach (Transform tf in speedometer.GetComponentsInChildren<Transform>(true))
            {
                tf.gameObject.SetActive(true);
            }*/

            GameObject dash = Main.GetChildWithName(go, "dash");
            dash.GetComponent<Renderer>().material.color = new Color32(176, 176, 176, 255);

            /* FlightLogger.Log("Trying to get arm swtich");
             GameObject masterArm = Main.GetChildWithName(go, "MasterArmSwitch");
             FlightLogger.Log("tryiung to get panel");
             GameObject panel = Main.GetChildWithName(masterArm, "panelEnd");
             FlightLogger.Log("Trying to make panel");
             GameObject newPanel = GameObject.Instantiate(panel, mfd2.transform.parent);
             FlightLogger.Log("Trying to move panel");
             newPanel.transform.localPosition = new Vector3(3.08658f, 0.09874532f, 0.8590795f);
             newPanel.transform.localEulerAngles = new Vector3(-88.44901f, 180, 0);
             newPanel.transform.localScale = new Vector3(3482.464f, 2314.85f, 2314.853f);
            */
            /*GameObject bound = Main.GetChildWithName(__instance.gameObject, "RightDashPoseBounds");
            bound.transform.localScale = new Vector3(1.9835f, 1.263827f, 1.9835f);
            bound.transform.localPosition = new Vector3(0.185f, 1.007f, 5.815f);
            */


            DashMapDisplay display = go.GetComponentInChildren<DashMapDisplay>(true);

            RectTransform masktf = display.transform.gameObject.GetComponent<RectTransform>();
            //masktf.sizeDelta = new Vector2(masktf.sizeDelta.x * 1.39263f, masktf.sizeDelta.y * 1.39263f);

            //display.gameObject.SetActive(false);
            // display.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, display.gameObject.GetComponent<RectTransform>().rect.width / 3);
            //display.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, display.gameObject.GetComponent<RectTransform>().rect.height / 3);
            //display.gameObject.SetActive(true);
            Debug.Log(manager.mfds[0].gameObject.GetComponent<RectTransform>().anchoredPosition.ToString());

            List<MFD> mfds = new List<MFD>();
            mfds.Add(manager.mfds[0]);
            mfds.Add(mfd3.GetComponentInChildren<MFD>());
            mfds.Add(manager.mfds[1]);


            manager.mfds = mfds;
            foreach (MFD mfd in manager.mfds)
            {
                Debug.Log(mfd.gameObject.name);
            }

            Main.mfd = mfd3;

            //Sets the state of the third mfd power knob to match the second's 
            VRTwistKnobInt mfd1Knob = manager.mfds[0].GetComponentInChildren<VRTwistKnobInt>(true);
            VRTwistKnobInt mfd2Knob = mfd2.GetComponentInChildren<VRTwistKnobInt>(true);
            VRTwistKnobInt mfd3Knob = mfd3.GetComponentInChildren<VRTwistKnobInt>(true);

            VTOLQuickStart.QuickStartComponents.QSKnobInt[] knobs = new VTOLQuickStart.QuickStartComponents.QSKnobInt[3];
            knobs[0] = new VTOLQuickStart.QuickStartComponents.QSKnobInt();
            knobs[0].knob = mfd1Knob;
            knobs[0].state = 1;

            knobs[1] = new VTOLQuickStart.QuickStartComponents.QSKnobInt();
            knobs[1].knob = mfd2Knob;
            knobs[1].state = 1;

            knobs[2] = new VTOLQuickStart.QuickStartComponents.QSKnobInt();
            knobs[2].knob = mfd3Knob;
            knobs[2].state = 1;

            Image mfd3Brightness = null;
            foreach (Image img in mfd3.GetComponentsInChildren<Image>())
            {
                if (img.transform.name.Contains("brightness"))
                {
                    mfd3Brightness = img;
                    break;
                }
            }

            Image[] newBrightImages = new Image[5];

            MFDBrightnessAdjuster mfdBright = go.GetComponentInChildren<MFDBrightnessAdjuster>(true);
            for (int i = 0; i < newBrightImages.Length - 1; i++)
            {
                newBrightImages[i] = mfdBright.images[i];
            }

            newBrightImages[4] = mfd3Brightness;
            mfdBright.images = newBrightImages;

            go.GetComponentInChildren<VTOLQuickStart>(true).quickStartComponents.knobs = knobs;
            manager.gameObject.SetActive(true);

            //Adds le jettison knob
            GameObject knobObj = GameObject.Instantiate(Main.JettisonPrefab, __instance.transform);
            knobObj.transform.localPosition = new Vector3(-0.287f, 0.8324f, 5.772f);
            knobObj.transform.localEulerAngles = new Vector3(-61.94f, 0.188f, 0);
            knobObj.transform.localScale = new Vector3(0.000730152f, 0.000730152f, 0.000730152f);

            VRTwistKnobInt twistKnob = knobObj.GetComponentInChildren<VRTwistKnobInt>(true);
            FaJettisonKnob jettKnob = knobObj.AddComponent<FaJettisonKnob>();
            jettKnob.wm = __instance.weaponManager;
            jettKnob.twistKnob = twistKnob;
            //Gets rid of the jettison console cause it looks cringe
            GameObject.Destroy(Main.GetChildWithName(__instance.gameObject, "CenterConsole"));

            //Adds the damage MMFD page
            GameObject MMFDManager = Main.GetChildWithName(__instance.gameObject, "MMFDManager");


            GameObject rwrButton = Main.FindInteractable(__instance.gameObject, "RWR").gameObject;
            GameObject damageButton = GameObject.Instantiate(rwrButton, rwrButton.transform.parent);
            damageButton.transform.localPosition = new Vector3(-103.8f, -1.9f, 8.4f);
            damageButton.transform.localScale = Vector3.one * 0.9947822f;
            damageButton.transform.localEulerAngles = Vector3.zero;

            VRInteractable damageInteractable = damageButton.GetComponent<VRInteractable>();
            damageInteractable.interactableName = "Damage";
            damageInteractable.OnInteract = new UnityEvent();
            damageInteractable.OnInteract.AddListener(new UnityAction(() =>
            {
                MMFDManager.GetComponent<MFDManager>().mfds[0].SetPage("damage");

            }
            ));

            GameObject rwrText = Main.GetChildWithName(__instance.gameObject, "RWRText");
            GameObject damageText = GameObject.Instantiate(rwrText, rwrText.transform.parent);
            damageText.transform.localPosition = new Vector3(-43.6f, 1.4f, 0);
            damageText.transform.localEulerAngles = Vector3.zero;
            damageText.transform.localScale = rwrText.transform.localScale;

            damageText.GetComponent<Text>().text = "D\nM\nG";

            VRButton dmgButton = damageButton.GetComponent<VRButton>();
            dmgButton.buttonTransform = Main.GetChildWithName(dmgButton.buttonTransform.parent.gameObject, "button.002").transform;



            //Adds master mode buttons
            GameObject masterModeButtons = GameObject.Instantiate(Main.MasterModePrefab, manager.mfds[0].transform.parent);
            masterModeButtons.transform.localPosition = new Vector3(-426.9f, 63.9f, 62.9f);
            masterModeButtons.transform.localEulerAngles = Vector3.zero;
            masterModeButtons.transform.localScale = Vector3.one * 1620.596f;

            masterModeButtons.AddComponent<MasterMode>();

            //Switches the MMFDs around
            if (SettingsManager.settings.switchMMFD)
            {
                FlightLogger.Log("Switching mmfds!");
                GameObject left = Main.GetChildWithName(__instance.gameObject, "MiniMFDLeft");
                GameObject right = Main.GetChildWithName(__instance.gameObject, "MiniMFDRight");

                Vector3 leftAngle = left.transform.localEulerAngles;
                Vector3 leftScale = left.transform.localScale;

                left.transform.localPosition = new Vector3(-248.6f, -65.4f, 50.8f);
                //Adding 180 degrees on the z axis because baha made one of them upsidedown
                left.transform.localEulerAngles = right.transform.localEulerAngles + new Vector3(0, 0, 180);
                left.transform.localScale = right.transform.localScale;

                right.transform.localPosition = new Vector3(371.4f, -50.7f, 61f);
                right.transform.localEulerAngles = leftAngle + new Vector3(0, 0, 180);
                right.transform.localScale = leftScale;

                //Combines the two mmfds ineractables
                VRInteractable[] mmfdInteractables = left.GetComponentsInChildren<VRInteractable>(true).Concat<VRInteractable>(right.GetComponentsInChildren<VRInteractable>(true)).ToArray();
                foreach (var intact in mmfdInteractables)
                {
                    Vector3 posesize = new Vector3();
                    bool resize = false;
                    if (intact.poseBounds != null && Main.SEAT_ADJUST_POSE_BOUNDS != null)
                    {
                        if (intact.poseBounds.pose != GloveAnimation.Poses.Joystick || intact.poseBounds.pose != GloveAnimation.Poses.Throttle || intact.poseBounds.pose != GloveAnimation.Poses.JetThrottle)
                        {
                            resize = true;
                            posesize = intact.poseBounds.size;
                            GameObject newBounds = GameObject.Instantiate(Main.SEAT_ADJUST_POSE_BOUNDS, __instance.gameObject.transform);
                            newBounds.name = "new" + left.transform.parent.name;
                            newBounds.transform.position = intact.transform.parent.gameObject.transform.position;
                            newBounds.transform.eulerAngles = intact.transform.parent.transform.eulerAngles;
                            intact.poseBounds = newBounds.GetComponent<PoseBounds>(); //Assigns bounds for switch
                            if (resize) intact.poseBounds.size = posesize;
                        }


                    }

                }



            }


            //Removes the cover from the master arm switch
            /*GameObject masterArmParent = Main.GetChildWithName(go, "MasterArmSwitch");
            GameObject coverParent = Main.GetChildWithName(masterArmParent, "coverSwitchParent");

            VRInteractable masterArm = Main.FindInteractable(masterArmParent, "Master Arm Switch");

            Main.GetChildWithName(masterArmParent, "coverSwitchBase2").GetComponent<MeshRenderer>().enabled = false;

            Main.GetChildWithName(masterArmParent, "off").transform.localPosition = new Vector3(0, -101.3f, 0.8f);

            masterArm.enabled = true;
            coverParent.SetActive(false);
            */
            //Adds the needles
            /*GameObject vertTick = GameObject.Instantiate(Main.HudTickPrefab);
            GameObject velVec = __instance.gameObject.GetComponentInChildren<HUDVelVector>(true).gameObject;
            vertTick.transform.SetParent(velVec.transform);
            vertTick.transform.localPosition = Vector3.zero;
            vertTick.transform.localEulerAngles = new Vector3(0, 0, 90);
            vertTick.transform.localScale = new Vector3(150.3895f, 150.3895f, 150.3895f);
            GameObject horTick = GameObject.Instantiate(Main.HudTickPrefab);
            horTick.transform.SetParent(velVec.transform);
            horTick.transform.localPosition = Vector3.zero;
            horTick.transform.localEulerAngles = new Vector3(0, 0, 0);
            horTick.transform.localScale = new Vector3(150.3895f, 150.3895f, 150.3895f);
            */


        }


    }
}