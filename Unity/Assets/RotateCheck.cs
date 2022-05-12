using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class RotateCheck : MonoBehaviour
{
    public GameObject Obj;

    private void Start()
    {
        Debug.Log(Obj.transform.rotation.ToString());
    }
}