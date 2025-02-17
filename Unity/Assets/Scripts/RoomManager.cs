using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class RoomManager : MonoBehaviour
{
    public GameObject allowPanel;
    //public GameObject AllowBTN;
    //public GameObject advenceButton;
    //public GameObject homeButton;
    public GameObject saveAnchorPanel;
    public GameObject doneButton;
    public InputField anchorNameInput;
    //private string anchorName;
    public GameObject AddObjectPanel;
    public GameObject AddObjects;
    public Text anchorNameText;
    public Text anchorKeyText;
    public GameObject DONEBTN;
    public GameObject ScreenShotBTN;

    public GameObject DoneSafePrefab;

    public GameObject ScanEnvPanel;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        allowPanel.SetActive(true);
        //AllowBTN.SetActive(true);
        saveAnchorPanel.SetActive(false);
        //doneButton.SetActive(false);
        SwitchAddObjectPanel(false);
        SwitchAddObject(false);
        SwitchScanEnvPanel(false);
        SwitchDoneButton(true);
        SwitchScreenShotBTN(false);
    }

    public void SwitchDoneButton(bool enable)
    {
        DONEBTN.SetActive(enable);
    }

    public void TextAnchorName()
    {
        anchorNameText.text = "AnchorName: " + AnchorInfo.Instance.anchorName ;
        anchorKeyText.text = "AnchorKey: \n" + AnchorInfo.Instance.anchorKey; 
    }

    public void SwitchScanEnvPanel(bool enable)
    {
        ScanEnvPanel.SetActive(enable);
    }

    public void SwitchScreenShotBTN(bool enable)
    {
        ScreenShotBTN.SetActive(enable);
    }

    public void AllowPanelOff()
    {
        allowPanel.SetActive(false);
    }

    public void AllowBTNSwitch(bool switchy)
    {
        //AllowBTN.SetActive(switchy);
    }

    public void SwitchDoneSafePrefab(bool switchy)
    {
        DoneSafePrefab.SetActive(switchy);
    }

    public void SwitchAddObjectPanel(bool switchy)
    {
        AddObjectPanel.SetActive(switchy);
    }

    public void SwitchAddObject(bool switchy)
    {
        AddObjects.SetActive(switchy);
    }

    public void SwitchAdvenceButton(bool switchy)
    {
        allowPanel.SetActive(switchy);
    }

    public void BackHome()
    {
        //SceneManager.LoadScene(sceneBuildIndex:0);
    }

    public void SwitchSaveAnchorPanel(bool switchy)
    {
        saveAnchorPanel.SetActive(switchy);
    }

    public void SwitchDonebutton(bool switchy)
    {
        doneButton.SetActive(switchy);
    }

    public void SaveInputFieldValue()
    {
        anchorNameText.text = anchorNameInput.text;
        AnchorInfo.Instance.SetAnchorName(anchorNameInput.text);
    }

    public void DoneBTNoff()
    {
        SwitchDonebutton(false);
    }
}