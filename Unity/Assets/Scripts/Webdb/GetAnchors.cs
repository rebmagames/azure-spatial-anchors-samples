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

public class GetAnchors : MonoBehaviour
{
    //public Text test;
    private void Start()
    {
        //StartCoroutine(GetItem("1"));
        //StartCoroutine(GetAcnhorsIDs());
    }

    public IEnumerator GetAcnhorsIDs(System.Action<string> callback)
    {
       using (UnityWebRequest www = UnityWebRequest.Get("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/GetAnchorID.php"))
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

    public IEnumerator GetItem(string anchorID, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.cross-reality-experts.com/wp-content/uploads/ARDemoPlacement/SpatialAnchors/GetKey.php?ID=" + anchorID))
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