using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _3rdMFD
{

    public class Main : VTOLMOD
    {
        public static Main instance;

        public static GameObject SEAT_ADJUST_POSE_BOUNDS;
        public static GameObject JettisonPrefab;
        public static GameObject BattleDamagePrefab;
        public static GameObject MasterModePrefab;

        public static GameObject HudTickPrefab;
        public static GameObject mfd;
        public static void FindSwitchObjects(GameObject faObject)
        {


            //TODO start using the seat adjust posebounds
            SEAT_ADJUST_POSE_BOUNDS = Main.GetChildWithName(faObject, ("MasterArmPoseBounds"));
            Debug.Log("pose bound found: " + SEAT_ADJUST_POSE_BOUNDS);

            int i = 5;
        }

        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {
            instance = this;

            string pathToBundle = Path.Combine(instance.ModFolder, "jettisonknob");
            string pathToHudBundle = Path.Combine(instance.ModFolder, "hudelements");
            string pathToDamageBundle = Path.Combine(instance.ModFolder, "battledamage");
            Debug.Log(pathToBundle);
            JettisonPrefab = FileLoader.GetAssetBundleAsGameObject(pathToBundle, "JettisonSwitch");
            Debug.Log("Got the " + JettisonPrefab.name);
            BattleDamagePrefab = FileLoader.GetAssetBundleAsGameObject(pathToDamageBundle, "BattleDamage");
            Debug.Log("Got the " + BattleDamagePrefab.name);
            MasterModePrefab = FileLoader.GetAssetBundleAsGameObject(pathToBundle, "MasterModeButtons");
            Debug.Log("Got the " + MasterModePrefab.name);

            //This is an event the VTOLAPI calls when the game is done loading a scene
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("C-137.MFD");
            harmonyInstance.PatchAll();


            SettingsManager.SetupSettingsMenu(this);

            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();

        }



        private void OnApplicationQuit()
        {
            SettingsManager.CheckSave();
        }

        //This method is called every frame by Unity. Here you'll probably put most of your code
        void Update()
        {


        }

        //This method is like update but it's framerate independent. This means it gets called at a set time interval instead of every frame. This is useful for physics calculations
        void FixedUpdate()
        {

        }


        public static GameObject GetChildWithName(GameObject obj, string name)
        {


            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == name || child.name.Contains(name + "(clone"))
                {
                    return child.gameObject;
                }
            }


            return null;

        }




        public static void disableMesh(GameObject parent, WeaponManager wm, bool nochildren)
        {
            if (!nochildren)
                return;
            MeshRenderer meshes = parent.GetComponent<MeshRenderer>();

            if (meshes)
                meshes.enabled = false;


        }



        public static void disableMesh(GameObject parent)
        {
            MeshRenderer[] meshes = parent.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mesh in meshes)
            {

                mesh.enabled = false;


            }

        }

        public static VRInteractable FindInteractable(GameObject gameObject, string interactableName)
        {
            foreach (VRInteractable interactble in gameObject.GetComponentsInChildren<VRInteractable>(true))
            {
                if (interactble.interactableName == interactableName)
                {
                    return interactble;
                }
            }

            Debug.LogError($"Could not find VRinteractable: {interactableName}");
            return null;
        }

        //This function is called every time a scene is loaded. this behaviour is defined in Awake().
        private void SceneLoaded(VTOLScenes scene)
        {
            //If you want something to happen in only one (or more) scenes, this is where you define it.

            //For example, lets say you're making a mod which only does something in the ready room and the loading scene. This is how your code could look:
            switch (scene)
            {
                case VTOLScenes.ReadyRoom:
                    //Add your ready room code here
                    break;
                case VTOLScenes.LoadingScene:
                    //Add your loading scene code here
                    break;

            }
        }





    }
}