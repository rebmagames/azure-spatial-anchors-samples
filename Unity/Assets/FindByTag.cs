using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class FindByTag : MonoBehaviour
{
    private GameObject anchor;

    public void GetAnchor(GameObject anchorObj)
    {
        anchor = anchorObj;
    }
}