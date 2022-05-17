using UnityEngine;
using Lean.Touch;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ObjectSelect : MonoBehaviour
{
    [SerializeField]
    private bool IsSelected;

    [SerializeField]
    private InputText inputText;

    private LeanPinchScale leanPinchScale;
    private LeanDragTranslate leanDragTranslate;
    private LeanTwistRotateAxis leanTwistRotateAxis;

    //private Outline outline;

    public bool Selected
    {
        get
        {
            return IsSelected;
        }
        set
        {
            IsSelected = value;
        }
    }

    public void Awake()
    {
        leanPinchScale = GetComponent<LeanPinchScale>();
        leanDragTranslate = GetComponent<LeanDragTranslate>();
        leanTwistRotateAxis = GetComponent<LeanTwistRotateAxis>();

        //outline = GetComponent<Outline>();
    }


    public void ObjectIsSelected()
    {
        //outline.enabled = true;
        LeanSetActiveTrue();
        if(gameObject.name == "Text(Clone)")
        {
            // InputText.
            inputText.PlaceInputField();
        }
    }

    public void ObjectIsNotSelected()
    {
        //outline.enabled = false;
        LeanSetActiveFalse();
    }

    public void LeanSetActiveTrue()
    {
        leanPinchScale.enabled = true;
        leanDragTranslate.enabled = true;
        leanTwistRotateAxis.enabled = true;
    }

    public void LeanSetActiveFalse()
    {
        leanPinchScale.enabled = false;
        leanDragTranslate.enabled = false;
        leanTwistRotateAxis.enabled = false;
    }
}