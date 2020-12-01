using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    public static ScreenShotHandler instance;
    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    public event System.Action<Texture2D> ScreenShotCapturedEvent;
    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32,false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArry = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArry);

            if (ScreenShotCapturedEvent != null)
            {
                ScreenShotCapturedEvent(renderResult);
            }
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }
    public void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);

        takeScreenshotOnNextFrame = true;
    }

}
