using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Button resetLevelButton;
    public Button applyButton;

    public MouseLook mouseLook;
    public Canvas options;
    public AudioSource volumeSource;
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            if(options.gameObject.activeSelf == true)
            {
                options.gameObject.SetActive(false);
                Debug.Log("Set to hidden");

                //make cursor hidden, resume 3d mouselook
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                options.gameObject.SetActive(true);
                Debug.Log("Set to visible");

                //make cursor visible, to use UI
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }


    }
    private void OnEnable()
    {
        gameSettings = new GameSettings();
        options.gameObject.SetActive(false);

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
        resetLevelButton.onClick.AddListener(delegate { OnResetLevelClick(); });

        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }

    public void OnFullScreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnSensitivityChange()
    {
        gameSettings.sensitivity = sensitivitySlider.value;
    }

    public void OnVolumeChange()
    {
        volumeSource.volume = gameSettings.volume = volumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void OnResetLevelClick()
    {
        SceneManager.LoadScene("PracticeLevel");
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        sensitivitySlider.value = gameSettings.sensitivity;
        volumeSlider.value = gameSettings.volume;
    }
}
