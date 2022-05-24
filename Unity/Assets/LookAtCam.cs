using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class LookAtCam : MonoBehaviour
{
    Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
        transform.parent.rotation = transform.rotation;
    }
}