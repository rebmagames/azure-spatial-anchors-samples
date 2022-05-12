using UnityEngine;
using System;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class AnchorManager : MonoBehaviour
{
    Action<string> _createAnchorCallback;
    public string JsonArrayString;

    public GetAnchors GetAnchors;
    public GameObject Anchor;
    public RoomManagerFind roomManagerFind;
    public AzureSpatialAnchorsFindAnchor azureSpatialAnchorsFindAnchor;

    public Text debugText;

    public LoadImage LoadImage;
    public Image referenceImage;

    private void Start()
    {
        _createAnchorCallback = (JsonArrayString) =>
        {
            StartCoroutine(CreateAnchorRoutine(JsonArrayString));
        };

        CreateAnchor();
    }

    public void CreateAnchor()
    {
        StartCoroutine(GetAnchors.GetAcnhorsIDs(_createAnchorCallback));
    }

    IEnumerator CreateAnchorRoutine( string jsonArrayString)
    {
        //Parsing the JSON array as an  array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

         for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;
            string anchorID = jsonArray[i].AsObject["ID"];
            JSONObject anchorInfoJson = new JSONObject();

            //create a callback to get the information from getanchors.cs
            Action<string> getAnchorInfoCallback = (anchorInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(anchorInfo) as JSONArray;
                anchorInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(GetAnchors.GetItem(anchorID, getAnchorInfoCallback));

            //wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            //instantiate anchor prefab
            GameObject anchorGo = Instantiate(Anchor);
            Anchor anchor = anchorGo.AddComponent<Anchor>();
            anchor.ID = anchorID;
            anchor.anchorName = anchorInfoJson["anchorName"];
            anchor.anchorKey = anchorInfoJson["anchorKey"];

            anchorGo.transform.SetParent(this.transform);
            anchorGo.transform.localScale = Vector3.one;
            anchorGo.transform.localPosition = Vector3.zero;

            Image anchorImage = anchorGo.transform.Find("Image").GetComponent<Image>();
            LoadImage.GetImage(anchorInfoJson["anchorKey"], anchorImage);
            //fill information
            anchorGo.transform.Find("TXT_AnchorName").GetComponent<Text>().text = anchorInfoJson["anchorName"];
            anchorGo.transform.Find("TXT_AnchorKey").GetComponent<Text>().text = anchorInfoJson["anchorKey"];

            anchorGo.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {
                roomManagerFind.SwitchAnchorList(false);

                //send anchorkey to AzureSpatialAnchorsFindAnchor.cs to find the right anchor
                azureSpatialAnchorsFindAnchor.GetAnchorKey(anchorGo.GetComponent<Anchor>().anchorKey);
                AnchorInfo.Instance.anchorID = anchorID;
                debugText.text = AnchorInfo.Instance.anchorID;
                LoadImage.GetImage(anchorInfoJson["anchorKey"], referenceImage);
                //AnchorInfo.instance.ObjectManager.GetObjects();
                //anchorGo.GetComponent<Anchor>().anchorKey;
            });
        }
        yield return null;
    }

}