using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public Slider difficultySlider;
    public TextMeshProUGUI difficultyText;
    public Toggle fullscreenToggle;

    [Header("Settings")]
    public float defaultVolume = 0.75f;
    public float defaultDifficulty = 1f;
    public bool defaultFullscreen = true;

    // Settings keys
    private const string VolumeKey = "Volume";
    private const string DifficultyKey = "Difficulty";
    private const string FullscreenKey = "Fullscreen";

    private void Start()
    {
        // Load saved settings or use defaults
        LoadSettings();
        
        // Add listeners to UI elements
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        
        if (difficultySlider != null)
        {
            difficultySlider.onValueChanged.AddListener(SetDifficulty);
        }
        
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    private void LoadSettings()
    {
        // Load volume
        float volume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }
        SetVolume(volume);
        
        // Load difficulty
        float difficulty = PlayerPrefs.GetFloat(DifficultyKey, defaultDifficulty);
        if (difficultySlider != null)
        {
            difficultySlider.value = difficulty;
        }
        SetDifficulty(difficulty);
        
        // Load fullscreen
        bool fullscreen = PlayerPrefs.GetInt(FullscreenKey, defaultFullscreen ? 1 : 0) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = fullscreen;
        }
        SetFullscreen(fullscreen);
    }

    public void SetVolume(float volume)
    {
        // Set audio volume
        AudioListener.volume = volume;
        
        // Save setting
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetDifficulty(float difficulty)
    {
        // Update difficulty text
        if (difficultyText != null)
        {
            if (difficulty < 0.33f)
            {
                difficultyText.text = "Easy";
            }
            else if (difficulty < 0.66f)
            {
                difficultyText.text = "Medium";
            }
            else
            {
                difficultyText.text = "Hard";
            }
        }
        
        // Save setting
        PlayerPrefs.SetFloat(DifficultyKey, difficulty);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Apply fullscreen setting
        Screen.fullScreen = isFullscreen;
        
        // Save setting
        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ResetToDefaults()
    {
        // Reset to default values
        if (volumeSlider != null)
        {
            volumeSlider.value = defaultVolume;
        }
        
        if (difficultySlider != null)
        {
            difficultySlider.value = defaultDifficulty;
        }
        
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = defaultFullscreen;
        }
        
        // Apply default settings
        SetVolume(defaultVolume);
        SetDifficulty(defaultDifficulty);
        SetFullscreen(defaultFullscreen);
    }

    public void SaveAndExit()
    {
        // Settings are saved as they're changed, so just exit
        gameObject.SetActive(false);
    }
} 