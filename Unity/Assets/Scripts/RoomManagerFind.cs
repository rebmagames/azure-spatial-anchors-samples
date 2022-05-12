using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class RoomManagerFind : MonoBehaviour
{
    public GameObject AllowPanel;
    public GameObject AnchorList;
    //public GameObject MobileUX;

    private void Start()
    {
        SwitchAnchorList(true);
       // SwitchMobileUX(false);
        SwitchAllowPanel(true);
    }

    public void SwitchAllowPanel(bool switchy)
    {
        AllowPanel.SetActive(switchy);
    }

    public void SwitchAnchorList(bool switchy)
    {
        AnchorList.SetActive(switchy);
    }
}