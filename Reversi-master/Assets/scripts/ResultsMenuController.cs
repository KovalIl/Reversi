using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsMenuController : MonoBehaviour
{
    [SerializeField] private Button _toMainMenuButton;
    [SerializeField] private Button _toRestartButton;

    private const string PlaySceneName = "SampleScene";
    private const string MenuSceneName = "MainMenu";

    private void Start()
    {
        GameManager.Instance.FieldsParent.SetActive(false);
        if (_toMainMenuButton)
        {
            _toMainMenuButton.onClick.AddListener(ToMainMenu);
        }
        if (_toRestartButton)
        {
            _toRestartButton.onClick.AddListener(ToRestart);
        }
    }

    private void ToMainMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    private void ToRestart()
    {
        SceneManager.LoadScene(PlaySceneName);
    }
}