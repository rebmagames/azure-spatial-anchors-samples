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
    public void FindScene()
    {
        SceneManager.LoadScene(2);
    }
    public void CreateScene()
    {
        SceneManager.LoadScene(1);
    }

    public void MenuScene()
    {
        SceneManager.LoadScene(0);
    }
}