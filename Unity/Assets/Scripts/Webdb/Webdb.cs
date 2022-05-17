using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// connection database
/// </summary>
///-------------------------------------------------------------------------------

public class Webdb : MonoBehaviour
{
    private void Start()
    {
        //StartCoroutine(RegisterUser(AnchorInfo.instance.anchorKey, AnchorInfo.instance.anchorName));
    }

    public void InsertAnchor()
    {
        StartCoroutine(RegisterUser(AnchorInfo.Instance.anchorKey, AnchorInfo.Instance.anchorName));
    }

    public void CallSafeObject(string name, string position, string scale, string text, string anchorID)
    {
        Debug.Log("### name : " + name + " Position : " + position + " Scale : " + scale + " text : " + text + " AnchorID : " + anchorID);
        StartCoroutine(SafeObject(name, position, scale, text ,anchorID));
    }

    public IEnumerator GetItem()
    { 
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/GetKey.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //show result as text
                Debug.Log(www.downloadHandler.text);
                Main.instance.SetID(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterUser(string anchorKey, string anchorName)
    {
        WWWForm form = new WWWForm();
        form.AddField("anchorKey", anchorKey);
        form.AddField("anchorName", anchorName);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/InsertKey.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Main.instance.SetID(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator SafeObject (string name, string position, string scale, string text ,string anchorID )
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("position", position);
        form.AddField("scale", scale);
        form.AddField("text", text);
        form.AddField("anchorID", anchorID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/Objects/InsertObjects.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetObjectsIDs(string anchorID, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/Objects/GetObjectsIDs.php?AnchorID=" + anchorID ))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //show result as text
                Debug.Log(www.downloadHandler.text);
                string jsonArrayString = www.downloadHandler.text;

                //call callback function to pass results
                callback(jsonArrayString);
            }
        }
    }

    public IEnumerator GetObject(string ID, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/Objects/GetObjects.php?ID=" + ID))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //show result as text
                Debug.Log(www.downloadHandler.text);
                string jsonArrayString = www.downloadHandler.text;

                //call callback function to pass resukts
                callback(jsonArrayString);

            }
        }
    }
}