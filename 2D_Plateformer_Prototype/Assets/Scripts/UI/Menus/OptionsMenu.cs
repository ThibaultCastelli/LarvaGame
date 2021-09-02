using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [Space]

    [SerializeField] AudioMixer audioMixer;

    #endregion

    #region Starts & Updates
    private void OnEnable()
    {
        // Put the slider and toggle at the right position based on playerpref
        if (PlayerPrefs.HasKey("fullscreen") && PlayerPrefs.GetInt("fullscreen") == 1)
            fullscreenToggle.isOn = true;
        else if (PlayerPrefs.HasKey("fullscreen") && PlayerPrefs.GetInt("fullscreen") == 0)
            fullscreenToggle.isOn = false;

        if (PlayerPrefs.HasKey("music"))
            musicSlider.value = MyMath.Normalized(PlayerPrefs.GetFloat("music"), -80, 20, 0, 10);

        if (PlayerPrefs.HasKey("sound"))
            soundSlider.value = MyMath.Normalized(PlayerPrefs.GetFloat("sound"), -80, 20, 0, 10);
    }
    #endregion

    #region Functions
    public void SetFullscreen(bool isFullscreen)
    {
        // Set the fullscreen toggle and save it to the player pref
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
            PlayerPrefs.SetInt("fullscreen", 1);
        else
            PlayerPrefs.SetInt("fullscreen", 0);

        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {

    }

    public void SetMusic(float volume)
    {
        // Set the music volume and save it to the player pref
        float normalizedVolume = MyMath.Normalized(volume, 0, 10, -80, 20);
        audioMixer.SetFloat("musicVolume", normalizedVolume);

        PlayerPrefs.SetFloat("music", normalizedVolume);
        PlayerPrefs.Save();
    }

    public void SetSound(float volume)
    {
        // Set the sound volume and save it to the player pref
        float normalizedVolume = MyMath.Normalized(volume, 0, 10, -80, 20);
        audioMixer.SetFloat("soundVolume", normalizedVolume);

        PlayerPrefs.SetFloat("sound", normalizedVolume);
        PlayerPrefs.Save();
    }
    #endregion

}
