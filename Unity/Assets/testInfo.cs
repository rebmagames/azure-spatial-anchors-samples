using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class testInfo : MonoBehaviour
{
    public static testInfo Instance;

    private void Start()
    {
        Instance = this;
    }

    
}