using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VRMaker
{
    class AssetLoader
    {
        public static Shader StereoShader;
        public static GameObject LeftHandBase;
        public static GameObject RightHandBase;

        public AssetLoader()
        {
            var VRathBundle = LoadBundle("vrathbundle");
            StereoShader = VRathBundle.LoadAsset<Shader>("StereoShader");
            LeftHandBase = LoadAsset<GameObject>(VRathBundle, "SteamVR/Prefabs/vr_glove_left_model_slim.prefab");
            RightHandBase = LoadAsset<GameObject>(VRathBundle, "SteamVR/Prefabs/vr_glove_right_model_slim.prefab");

            Logs.WriteInfo("Attempted to load shader: " + StereoShader.name);

        }

        private T LoadAsset<T>(AssetBundle bundle, string prefabName) where T : UnityEngine.Object
        {
            var asset = bundle.LoadAsset<T>($"Assets/{prefabName}");
            if (asset)
                return asset;
            else
            {
                Logs.WriteError($"Failed to load asset {prefabName}");
                return null;
            }
                
        }

        private static AssetBundle LoadBundle(string assetName)
        {
            var myLoadedAssetBundle =
                AssetBundle.LoadFromFile(
                    $"{Plugin.gamePath}/VRath/Assets/{assetName}");
            if (myLoadedAssetBundle == null)
            {
                Logs.WriteError($"Failed to load AssetBundle {assetName}");
                return null;
            }

            return myLoadedAssetBundle;
        }

    }
}
