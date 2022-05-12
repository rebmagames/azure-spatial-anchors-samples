using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class AnchorParent : MonoBehaviour
{
    public GameObject Anchor;


    private void OnEnable()
    {
        Debug.Log("@@@ onenable");
        AnchorInfo.Instance.GetAnchor(Anchor);
    }
    private void Start()
    {
        AnchorInfo.Instance.GetAnchor(Anchor);
        //testInfo.Instance.GetAnchor(Anchor);
    }


}