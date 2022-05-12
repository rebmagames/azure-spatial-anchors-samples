using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class TestSpawn : MonoBehaviour
{
    public GameObject Anchor;
    public GameObject Parent;
    private string scaleChange = "(30, 30, 30)";


    public void PlaceAnchor()
    {
        GameObject obj = Instantiate(Anchor);
        obj.transform.position = Parent.transform.position;
        obj.transform.parent = Parent.transform;
        Vector3 scale = StringToVector3(scaleChange);
        obj.transform.localScale = scale;
           
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