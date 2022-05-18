using UnityEngine;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class Test : MonoBehaviour
{
    public AzureSpatialAnchorsBasicDemoScript azureSpatialAnchors;
    public GameObject Text;
    public GameObject Cam;
    public void Update()
    {
        if(Cam.active == false)
        {
           // Debugerror.Log("ERROR cam is not on");
            Debug.LogError("ERROR cam is not on");
        }
    }

    public void GetAnchorKey()
    {
        if (string.IsNullOrEmpty(azureSpatialAnchors._currentAnchorId))
        {
            Debug.Log("currentAnchorId not found");
        }
        else
        {
            Debug.Log("Anchor Key: " + (azureSpatialAnchors._currentAnchorId));
        }
    }

    public void AppStateTest()
    {
        //azureSpatialAnchors.currentappstate 
    }

    public void SafeAnchorKey()
    {
        AnchorInfo.Instance.SetAnchorKey(azureSpatialAnchors._currentAnchorId);
    }

    public void placetext()
    {
        Instantiate(Text);
    }


}