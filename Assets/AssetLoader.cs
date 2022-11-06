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
        public static GameObject Skybox;
        public static GameObject LeftHandBase;
        public static GameObject RightHandBase;

        public AssetLoader()
        {
            var VRathBundle = LoadBundle("vrathbundle");
            Skybox = LoadAsset<GameObject>(VRathBundle, "CustomAssets/SkyboxPrefab.prefab");
            LeftHandBase = LoadAsset<GameObject>(VRathBundle, "SteamVR/Prefabs/vr_glove_left_model_slim.prefab");
            RightHandBase = LoadAsset<GameObject>(VRathBundle, "SteamVR/Prefabs/vr_glove_right_model_slim.prefab");

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
