using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ChangeScale : MonoBehaviour
{
    public GameObject Cube;
    private Vector3 scaleChange;

    private void Start()
    {
        ChangeScaleTest();
    }
    public void ChangeScaleTest()
    {
        scaleChange = StringToVector3("(32.3, 32.3, 32.3)") * 2;
        //scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
        Cube.transform.localScale = scaleChange;
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}