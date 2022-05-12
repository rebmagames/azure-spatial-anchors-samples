using Microsoft.Azure.SpatialAnchors.Unity.Examples;
using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class AnchorInfo : MonoBehaviour
{
    public RoomManager roomManager;
    public GetAnchors getAnchors;
    public ObjectManager ObjectManager;
    public AzureSpatialAnchorsBasicDemoScript azureSpactialAnchors;
    public string anchorName { get; private set; }

    public string anchorID;
    public string anchorKey { get; private set; }

    public GameObject Anchor;

    private static AnchorInfo _instance;

    public static AnchorInfo Instance { get { return _instance; } }


    private void Awake()
    {
        Debug.Log("@@@ Instance");
        
            _instance = this;
        

        Debug.Log("@@@ Instance 2");
        roomManager = GetComponent<RoomManager>();
        getAnchors = GetComponent<GetAnchors>();
        ObjectManager = GetComponent<ObjectManager>();
    }


    public void SetAnchorName (string name)
    {
        anchorName = name;
    }

    public void SetAnchorKey (string key)
    {
        anchorKey = key;
    }

    public void GetAnchor(GameObject anchor)
    {
        Debug.Log("@@@ Anchor!!!!");
        Anchor = anchor;
        Anchor.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("@@@ Anchor Rotation = " + Anchor.transform.rotation.ToString());
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