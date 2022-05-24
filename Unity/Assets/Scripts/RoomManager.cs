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
    public GameObject InputField;

    public GameObject CamBlocked;

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
        SwitchInPutFiled(false);
        SwitchScreenShotBTN(false);
        SwitchCamBlocked(false);
    }

    public void ASABAckToStart()
    {
        allowPanel.SetActive(true);
        //AllowBTN.SetActive(true);
        saveAnchorPanel.SetActive(false);
        //doneButton.SetActive(false);
        SwitchAddObjectPanel(false);
        SwitchAddObject(false);
        SwitchScanEnvPanel(false);
        SwitchDoneButton(true);
        SwitchInPutFiled(false);
        SwitchScreenShotBTN(false);
        SwitchScreenShotBTN(false);
    }

    public void SwitchCamBlocked(bool enable)
    {
        CamBlocked.SetActive(enable);
    }
        

    public void SwitchInPutFiled(bool enable)
    {
        InputField.SetActive(enable);
        //InputField ifield;
        //ifield = InputField.GetComponent<InputField>();
        //ifield.text = "";
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