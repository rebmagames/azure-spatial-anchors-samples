using UnityEngine;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class TestRoomManager : MonoBehaviour
{
    public GameObject AddObjectPanel;

    private void Start()
    {
        SwitchAddObjectPanel(false);
    }

    public void SwitchAddObjectPanel(bool switchy)
    {
        AddObjectPanel.SetActive(switchy);
    }
}