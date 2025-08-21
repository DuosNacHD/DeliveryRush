using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [SerializeField]GameObject MenuPanel, SettingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettings() 
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        MenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
