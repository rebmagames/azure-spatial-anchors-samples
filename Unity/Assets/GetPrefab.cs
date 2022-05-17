using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class GetPrefab : MonoBehaviour
{
    public List<GameObject> Objects;
    public Vector3 Position;
    private string position;
    private string name;
    private string scale;
    public Webdb Webdb;
    public Text anchorIdText;

    public void AddObject(GameObject obj)
    {
        Objects.Add(obj);
    }

    private void Update()
    {
        //Debug.Log(Main.instance.AnchorID);
    }

    public void SafeObject()
    {
        anchorIdText.text = "Test";
        string anchorID = Main.instance.AnchorID;
        anchorIdText.text = anchorID;
        if (anchorID == "")
        {
            Debug.Log(anchorID);
            Debug.Log("doesn't work!");
        }
        else
        {
            foreach (GameObject item in Objects)
                {
                Debug.Log("anchorID = " + anchorID);
                    position = item.transform.position.ToString();
                    Debug.Log(item.transform.position);
                    name = item.name.Replace("(Clone)", "");
                    scale = item.transform.localScale.ToString();
                    String text = "";
                    if (name == "Text")
                {
                    GameObject textObject = item.transform.GetChild(0).gameObject;
                    TMP_Text textGo = textObject.transform.GetComponent<TMP_Text>();
                    text = textGo.text;
                }
                    Webdb.CallSafeObject(name, position, scale, text, anchorID);
                }
        }
        
        
        
    }

}