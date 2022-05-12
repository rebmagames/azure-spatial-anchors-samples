using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField]private string fileName;
    string fullPath;
    [SerializeField]string url;

    public Texture2D mapScreen;

    public void TakeScreenshot()
    {
        fileName = AnchorInfo.Instance.anchorKey +".png";
        StartCoroutine(UploadScreenshot());
    }


    IEnumerator UploadScreenshot()
    {
        yield return new WaitForEndOfFrame();
        //Texture texture = ScreenCapture.CaptureScreenshotAsTexture();
        mapScreen = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        mapScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        mapScreen.Apply();


        WWWForm form = new WWWForm();
        byte[] textureBytes = null;

        //Texture2D imageTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        Texture2D uploadTexture = GetTextureCopy(mapScreen);
        textureBytes = uploadTexture.EncodeToPNG();

        form.AddBinaryData("myimage", textureBytes, fileName, "image/png");
        WWW w = new WWW(url, form);

        yield return w;

        if(w.error != null)
        {
            //error
            Debug.Log("error : " + w.error);
        } else
        {
            //succes
            Debug.Log("done?" + w.text);
        }
        w.Dispose();
    }

    Texture2D GetTextureCopy(Texture2D source)
    {
        //Create a RenderTexture
        RenderTexture rt = RenderTexture.GetTemporary(
                               source.width,
                               source.height,
                               0,
                               RenderTextureFormat.Default,
                               RenderTextureReadWrite.Linear
                           );

        //Copy source texture to the new render (RenderTexture) 
        Graphics.Blit(source, rt);

        //Store the active RenderTexture & activate new created one (rt)
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        //Create new Texture2D and fill its pixels from rt and apply changes.
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        //activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }
}