using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LevelSelect;

    [Header("Map Settings")]
    [SerializeField] private string[] mapNames = { "Playground", "City_noof" };

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        LevelSelect.SetActive(false);
    }
    public void ShowLevelSelect()
    {
        MainMenu.SetActive(false);
        LevelSelect.SetActive(true);
    }
    public void LoadSpecificMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadRandomMap()
    {
        if (mapNames.Length > 0)
        {
            int index = Random.Range(0, mapNames.Length);
            SceneManager.LoadScene(mapNames[index]);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}