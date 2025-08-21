using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject[] Menu,Game,Mission;
    public void OpenMenuTab()
    {
        foreach (GameObject item in Menu)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in Game)
        {
            item.SetActive(false);
        }
        Time.timeScale = 0;
    }
    public void CloseMenuTab()
    {
        foreach (GameObject item in Game)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in Menu)
        {
            item.SetActive(false);
        }
        Time.timeScale = 1;
    }
    public void OpenMissionTab()
    {
        foreach (GameObject item in Mission)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in Game)
        {
            item.SetActive(false);
        }
        Time.timeScale = 0;
    }
    public void CloseMissionTab()
    {
        foreach (GameObject item in Game)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in Mission)
        {
            item.SetActive(false);
        }
        Time.timeScale = 1;
    }
    public void exitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
