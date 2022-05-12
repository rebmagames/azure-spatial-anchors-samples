using UnityEngine;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class GetInputValue : MonoBehaviour
{
    public InputField inputField;

    private void Update()
    {
        if (inputField)
        {
            Debug.Log(inputField.text);
        }
        else
        {
            Debug.Log("nope");
        }
        
    }
}