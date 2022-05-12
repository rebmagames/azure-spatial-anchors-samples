using System.Collections;
using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class timer : MonoBehaviour
{
    public RoomManager RoomManager;
    

    private void Start()
    {
        StartCoroutine(waiter());
        
    }

    IEnumerator waiter()
    {
        //Wait for 10 seconds
        yield return new WaitForSeconds(5);
        RoomManager.SwitchScanEnvPanel(false);
        AnchorInfo.Instance.azureSpactialAnchors.AdvanceDemo();
        if (AnchorInfo.Instance.azureSpactialAnchors.isdone == false)
        {
            AnchorInfo.Instance.azureSpactialAnchors.StopSessionAsync();
        }
    }

    private async System.Threading.Tasks.Task resetSceneAsync()
    {
        await AnchorInfo.Instance.azureSpactialAnchors.StopSessionAsync();
        Application.LoadLevel(Application.loadedLevel);
    }
}