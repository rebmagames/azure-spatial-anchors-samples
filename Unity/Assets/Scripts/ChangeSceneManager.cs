using UnityEngine;
using UnityEngine.SceneManagement;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class ChangeSceneManager : MonoBehaviour
{
    public GameObject NoWifi;

    private void CheckWifi()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            NoWifi.SetActive(true);
            
        }

    }
    public void FindScene()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            NoWifi.SetActive(true);

        }
        else
        {
            SceneManager.LoadScene(2);
        }
        
    }
    public void CreateScene()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            NoWifi.SetActive(true);

        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
    }

    public void MenuScene()
    {
        SceneManager.LoadScene(0);
    }
}