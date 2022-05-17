using TMPro;
using UnityEngine;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class InputText : MonoBehaviour
{
    public InputField inputText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private ObjectSelect objectSelect;
    public GameObject Inputfield;
    [SerializeField] private GameObject canvas;

    private void Update()
    {
        if (inputText)
        {
            if(objectSelect.Selected == false)
            {
                inputText.IsDestroyed();
            }
        }
        else
        {
            if(objectSelect.Selected == true)
            {
               
                PlaceInputField();

            }
        }

        
    }

    public void UpdateText()
    {
        if (objectSelect.Selected == true)
        {
            Debug.Log("test");
            text.text = inputText.text;
        }
    }

    public void PlaceInputField()
    {
        AnchorInfo.Instance.roomManager.SwitchInPutFiled(true);
        GameObject temp = AnchorInfo.Instance.roomManager.InputField;
        inputText = temp.transform.GetComponent<InputField>();
        inputText.text = text.text;
        inputText.onValueChanged.AddListener(delegate { UpdateText(); });
        
    }

}