using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] AudioMixer audioMixer;
    public static bool isGamePaused = false;
    #endregion

    #region Starts & Updates
    private void Start()
    {
        // Set the fullscreen and resolution based on playerpref
        if (PlayerPrefs.HasKey("fullscreen") && PlayerPrefs.GetInt("fullscreen") == 1)
            Screen.fullScreen = true;
        else if (PlayerPrefs.HasKey("fullscreen") && PlayerPrefs.GetInt("fullscreen") == 0)
            Screen.fullScreen = false;

        // Set the volumes of the audio mixer
        if (PlayerPrefs.HasKey("music"))
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("music"));
            
        if (PlayerPrefs.HasKey("sound"))
            audioMixer.SetFloat("soundVolume", PlayerPrefs.GetFloat("sound"));

        isGamePaused = false;
    }
    #endregion
}
