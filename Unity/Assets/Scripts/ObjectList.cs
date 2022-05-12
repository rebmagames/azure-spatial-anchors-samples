using UnityEngine;
using UnityEngine.UI;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ObjectList : MonoBehaviour
{
    public GameObject[] Objects;
    public GameObject Button;
    public RoomManager RoomManager;
    public GetPrefab GetPrefab;
    public SelectManager SelectManager;

    public GameObject parent;

    public GameObject Spawn;

    public Text debug;

    private void Start()
    {

    }

    public void GetObjectList()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Debug.Log("???test");
        //parent = GameObject.Find("AzureSpatialAnchorsDemoObject");
        foreach (GameObject item in Objects)
        {
            GameObject button = Instantiate(Button, this.transform);
            button.transform.Find("Name_TXT").GetComponent<Text>().text = item.name;

            button.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {
                RoomManager.SwitchAddObjectPanel(false);
                GameObject inst = Instantiate(item);
                inst.transform.position = Spawn.transform.position;
                parent = AnchorInfo.Instance.Anchor;
                if (parent != null)
                {
                    inst.transform.parent = parent.transform;
                    inst.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
                    debug.text = inst.transform.parent.name;
                }
                //inst.transform.parent = anchor.transform;
                SelectManager.AddPlacementObjectToList(inst.GetComponent<ObjectSelect>());
                GetPrefab.AddObject(inst);
            });
        }
    }

    public void GetParent(GameObject anchor)
    {
        parent = anchor;
        
    }

}