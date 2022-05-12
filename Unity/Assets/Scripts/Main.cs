using Microsoft.Azure.SpatialAnchors.Unity.Examples;
using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class Main : MonoBehaviour
{
    public static Main instance;
    public ObjectList ObjectList;
    public DemoScriptBase DemoScriptBase;
    public GetPrefab GetPrefab;

    public string AnchorID;

    private void Start()
    {
        instance = this;
        ObjectList = GetComponent<ObjectList>();
        DemoScriptBase = GetComponent<DemoScriptBase>();
        GetPrefab = GetComponent<GetPrefab>();
    }

    public void SetID(string iD)
    {
        AnchorID = iD;
        GetPrefab.SafeObject();
    }
}