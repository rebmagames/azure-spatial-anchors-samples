using UnityEngine;
using System;
using System.Collections;
using SimpleJSON;
using System.Linq;
using UnityEngine.UI;
using TMPro;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ObjectManager : MonoBehaviour
{
    Action<string> _createObjectCallback;
    public string JsonArrayString;
    //public Text ObjectPlaced;

    public Text ObjectName;
    public Text ObjectLocation;
    public Text ObjectScale;

    public Webdb Webdb;
    public GameObject[] prefabs;
    public AnchorInfo AnchorInfo;

    private GameObject parent;
    private bool AnchorParent = false;


    Action<string> _createItemsCallback;
    public string jsonArrayString;

    private void Start()
    {
        ObjectName.text = "GetObjectsTest?";
    }

    public void GetObjects()
    {
        ObjectName.text = "GetObjectsTest";
        _createObjectCallback = (JsonArrayString) =>
        {
            StartCoroutine(CreateObjectRoutine(JsonArrayString));
        };

        CreateAnchor();
    }

    public void CreateAnchor()
    {
        Debug.Log("!!!7 " + AnchorInfo.Instance.anchorID);
        StartCoroutine(Webdb.GetObjectsIDs(AnchorInfo.Instance.anchorID, _createObjectCallback));
    }

    IEnumerator CreateObjectRoutine(string jsonArrayString)
    {
        //Parsing the JSON array as an  array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;
            string ID = jsonArray[i].AsObject["ID"];
            JSONObject InfoJson = new JSONObject();
            int count = 0;
            //create a callback to get the information from getanchors.cs
            Action<string> getAnchorInfoCallback = (anchorInfo) =>
                {
                    isDone = true;
                    JSONArray tempArray = JSON.Parse(anchorInfo) as JSONArray;
                    count = tempArray.Count;
                    InfoJson = tempArray[0].AsObject;
                };
            StartCoroutine(Webdb.GetObject(ID, getAnchorInfoCallback));
            //wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);


                //ObjectPlaced.text = anchorInfoJson["name"].ToString() + " Is the object name";
                //instantiate prefab
                GameObject temp = prefabs.SingleOrDefault(obj => obj.name == InfoJson["name"]);
                GameObject objectGo = Instantiate(temp);

                ObjectName.text = InfoJson["name"];
            Debug.Log("Created obj ===== " + InfoJson["name"] + "Loop int = " + i + "| InfoJson elements = " + InfoJson.Count + " | jsonArray count = " + jsonArray.Count);

            //GameObject parent = AnchorInfo.instance.Anchor;

            //Debug.Log("!!!6  = " + (AnchorInfo == null).ToString());
            StartCoroutine(FindAnchorParent(objectGo, InfoJson));
        }
        
    }

    IEnumerator FindAnchorParent(GameObject objectGo, JSONObject InfoJson)
    {
       // Debug.Log("!!!5 Parent = " + (parent == null).ToString());
        //parent = AnchorInfo.Anchor;
       // Debug.Log("!!!4 parent =" + (AnchorInfo.Anchor == null).ToString());

        while(AnchorInfo.Anchor == null)
            yield return null;

        parent = AnchorInfo.Anchor;
       // Debug.Log("!!!2 parent = " + parent.name + " Parent rotatie = " + parent.transform.rotation.ToString() + " Parent Position = " + parent.transform.position.ToString());
        SetObjectTransformData(objectGo, InfoJson);
    }

    private void SetObjectTransformData(GameObject objectGo, JSONObject InfoJson)
    {
       // Debug.Log("!!!3 Comes here!");
        objectGo.transform.parent = parent.transform;
        Vector3 position = StringToVector3(InfoJson["position"]);
        objectGo.transform.position = position;
        ObjectLocation.text = objectGo.transform.position.ToString();
        Vector3 scale = StringToVector3(InfoJson["scale"]);
        ObjectScale.text = scale.ToString();
        objectGo.transform.localScale = scale;
        Debug.Log("!!! Parent of object = " + parent.name + " Position = " + position.ToString() + " Scale = " + scale.ToString() + " objectname : " + objectGo.name);
        if(objectGo.name == "Text(Clone)")
        {
            Debug.Log(InfoJson["text"] + "####!");
            GameObject textObject = objectGo.transform.GetChild(0).gameObject;
            Debug.Log(textObject.name + "###1");
            TMP_Text textGo = textObject.transform.GetComponent<TMP_Text>();
            Debug.Log(textGo.text + "###2");
            textGo.text = InfoJson["text"];
            Debug.Log(textGo.text + "###3");
        }
    }



    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}