using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class LoadImage : MonoBehaviour
{

    public string TextureURL = "";
    //public Image image;

    // Start is called before the first frame update
    public void GetImage(string key, Image image)
    {
        TextureURL = "https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/Images/Screenshots/uploaded_images/" + key + ".png";
        StartCoroutine(DownloadImage(TextureURL, image));
    }

 

    IEnumerator DownloadImage(string MediaUrl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            image.sprite = webSprite;
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }


}
