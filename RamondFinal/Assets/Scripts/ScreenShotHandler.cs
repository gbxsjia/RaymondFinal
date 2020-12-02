using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    public static ScreenShotHandler instance;
    private Camera myCamera;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    public event System.Action<Texture2D> ScreenShotCapturedEvent;
    private void OnPostRender()
    {
   
    }
    public void TakeScreenshot(int width, int height)
    {
        StartCoroutine(ScreenshotProcess(width, height));
    }

    private IEnumerator ScreenshotProcess(int resWidth, int resHeight)
    {
        yield return new WaitForEndOfFrame();
        //Texture2D renderResult = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        //renderResult.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //renderResult.Apply();

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        myCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        myCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        screenShot.Apply();
        Destroy(rt);
       


        if (ScreenShotCapturedEvent != null)
        {
            ScreenShotCapturedEvent(screenShot);
        }
    }

}
