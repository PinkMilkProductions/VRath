using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VRMaker
{
    public class StereoRendering : MonoBehaviour
    {
        public static string k_RenderTag = "MyVRRT";
        public static int TempTargetId = 25;
        public static Material stereoMat = null;

        // Creates a private material used to the effect
        public void Awake()
        {
            stereoMat = new Material(AssetLoader.StereoShader);
            Logs.WriteInfo("new StereoRendering Awoken!");
        }

        // Postprocess the image
        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Logs.WriteInfo("Begin of OnRenderImage");

            if (!Kingmaker.Game.GetCamera())
                return;

            if (!Kingmaker.Game.GetCamera().stereoEnabled)
                return;

            UnityEngine.XR.XRDisplaySubsystem.XRRenderPass renderPass;
            if (!Plugin.MyDisplay.Internal_TryGetRenderPass(0, out renderPass))
                return;

            var cmd = UnityEngine.Rendering.CommandBufferPool.Get(k_RenderTag);
            var dest = renderPass.renderTarget;

            //Graphics.Blit(source, dest, stereoMat, 0);

            //int temp = TempTargetId;
            //cmd.GetTemporaryRTWithDescriptor(temp, renderPass.renderTargetDesc, FilterMode.Point);

            cmd.Blit(source, dest, stereoMat, 0);

            //context.ExecuteCommandBuffer(cmd);
            //context.Submit();

            Kingmaker.Game.GetCamera().AddCommandBuffer(UnityEngine.Rendering.CameraEvent.AfterEverything, cmd);

            UnityEngine.Rendering.CommandBufferPool.Release(cmd);

            Logs.WriteInfo("End of OnRenderImage");
        }
    }
}
