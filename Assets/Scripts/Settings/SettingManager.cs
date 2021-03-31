using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider sensitivitySlider;
    public Slider volumeSlider;

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

        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    public void OnFullScreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }

    public void OnSensitivityChange()
    {
        gameSettings.sensitivity = sensitivitySlider.value;
    }

    public void OnVolumeChange()
    {
        volumeSource.volume = gameSettings.volume = volumeSlider.value;
    }

    public void SaveSettings()
    {
        //OPTIONS MENU PART 3 @ 14:30 GAMEGRIND
    }

    public void LoadSettings()
    {

    }
}
