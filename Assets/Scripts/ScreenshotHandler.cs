using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler _instance;
    private Camera _camera;

    void Awake()
    {
        _instance = this;
        _camera = GetComponent<Camera>();
    }


    private void TakeScreenshot(int width, int height, string filename)
    {
        var renderTexture = new RenderTexture(width, height, 16);

        _camera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        _camera.Render();

        var renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        var rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        renderResult.ReadPixels(rect, 0, 0);

        var byteArray = renderResult.EncodeToPNG();

        System.IO.File.WriteAllBytes(Application.dataPath + "/Dataset/" + filename, byteArray);

        RenderTexture.active = null;
        _camera.targetTexture = null;
    }

    public static void TakeScreenshot_Static(int width, int height, string filename)
    {
        _instance.TakeScreenshot(width, height, filename);
    }
}
