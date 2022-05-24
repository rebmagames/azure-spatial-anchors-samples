using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class objectBelly : MonoBehaviour
{
    public GameObject Cube;

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("BELLY!!!");
        
    }
}