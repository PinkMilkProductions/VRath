using System.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEditor;
using Kingmaker;
using Rewired;
using Valve.VR;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using Unity.XR.OpenVR;

namespace VRMaker
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.HerrFristi.VRMods.VRath";
        public const string PLUGIN_NAME = "VRMaker";
        public const string PLUGIN_VERSION = "0.0.1";

        public static string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
        public static string gamePath = Path.GetDirectoryName(gameExePath);
        public static string HMDModel = "";

        public static UnityEngine.XR.Management.XRManagerSettings managerSettings = null;

        public static List<UnityEngine.XR.XRDisplaySubsystemDescriptor> displaysDescs = new List<UnityEngine.XR.XRDisplaySubsystemDescriptor>();
        public static List<UnityEngine.XR.XRDisplaySubsystem> displays = new List<UnityEngine.XR.XRDisplaySubsystem>();
        public static UnityEngine.XR.XRDisplaySubsystem MyDisplay = null;

        public static GameObject SecondEye;
        public static Camera SecondCam;



        //Create a class that actually inherits from MonoBehaviour
        public class MyStaticMB : MonoBehaviour
        {
        }

        //Variable reference for the class
        public static MyStaticMB myStaticMB;


        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            new AssetLoader();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            //If the instance not exit the first time we call the static class
            if (myStaticMB == null)
            {
                //Create an empty object called MyStatic
                GameObject gameObject = new GameObject("MyStatic");


                //Add this script to the object
                myStaticMB = gameObject.AddComponent<MyStaticMB>();
            }

            myStaticMB.StartCoroutine(InitVRLoader());
            //InitVR();

            //Game.s_Instance.ControllerMode = Game.ControllerModeType.Gamepad;
        }

        private static void InitVR()
        {
            

            //typeof(XRGeneralSettings).GetMethod("AttemptInitializeXRSDKOnLoad", AccessTools.all).Invoke(null, null);
            //typeof(XRGeneralSettings).GetMethod("AttemptStartXRSDKOnBeforeSplashScreen", AccessTools.all).Invoke(null, null);


        }

        public static System.Collections.IEnumerator InitVRLoader()
        {
            
            SteamVR_Actions.PreInitialize();

            var generalSettings = ScriptableObject.CreateInstance<XRGeneralSettings>();
            managerSettings = ScriptableObject.CreateInstance<XRManagerSettings>();
            var xrLoader = ScriptableObject.CreateInstance<OpenVRLoader>();
            

            var settings = OpenVRSettings.GetSettings();
            settings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;
            

            generalSettings.Manager = managerSettings;

            managerSettings.loaders.Clear();
            managerSettings.loaders.Add(xrLoader);

            managerSettings.InitializeLoaderSync(); ;


            XRGeneralSettings.AttemptInitializeXRSDKOnLoad();
            XRGeneralSettings.AttemptStartXRSDKOnBeforeSplashScreen();

            //managerSettings.StartSubsystems();
            //managerSettings.automaticLoading = true;

            SteamVR.Initialize(true);



            //UnityEngine.XR.XRSettings.LoadDeviceByName(UnityEngine.XR.XRSettings.loadedDeviceName);

            SubsystemManager.GetInstances(displays);
            MyDisplay = displays[0];
            MyDisplay.Start();

            Logs.WriteInfo("XRDisplaySubSystem running: ");
            Logs.WriteInfo(MyDisplay.IsRunning());

            //var myVRHelper = myStaticMB.gameObject.AddComponent<StereoRendering>();

            //StereoRendering myVRHelper = new StereoRendering();
            //myVRHelper.Awake();
            //myVRHelper.enabled = true;

            Logs.WriteInfo("Reach end of InitVRLoader(): ");

            yield return null;

        }


    }

}
