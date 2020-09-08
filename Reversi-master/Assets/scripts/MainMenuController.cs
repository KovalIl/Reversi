using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static int Chip1Value;
    public static int Chip2Value;

    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backFromSettingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Dropdown _chip1;
    [SerializeField] private Dropdown _chip2;

    private const int BlackColor = 4;
    private const int WhiteColor = 3;
    private const string PlayScene = "Samplescene";

    private void Awake()
    {
        _settingsCanvas.enabled = false;
        if (_playButton)
        {
            _playButton.onClick.AddListener(ToPlayButton);
        }
        if (_settingsButton)
        {
            _settingsButton.onClick.AddListener(ToSettingsButton);
        }
        if (_backFromSettingsButton)
        {
            _backFromSettingsButton.onClick.AddListener(ToBackButton);
        }
        if (_exitButton)
        {
            _exitButton.onClick.AddListener(ToExitButton);
        }
        Chip1Value = Chip.Player1Color;
        Chip2Value = Chip.Player2Color;
    }

    private void ToPlayButton()
    {
        if (Chip1Value == Chip2Value)
        {
            Chip1Value = BlackColor;
            Chip2Value = WhiteColor;
        }
        SceneManager.LoadScene(PlayScene);
    }

    private void ToExitButton()
    {
        Application.Quit();
    }

    private void ToSettingsButton()
    {
        _settingsCanvas.enabled = true;
        _mainMenuCanvas.enabled = false;
        _chip1.value = Chip.Player1Color;
        _chip2.value = Chip.Player2Color;
    }

    private void ValueChanged()
    {
        Chip1Value = _chip1.value;
        Chip2Value = _chip2.value;
    }

    private void ToBackButton()
    {
        _mainMenuCanvas.enabled = true;
        _settingsCanvas.enabled = false;
    }
}