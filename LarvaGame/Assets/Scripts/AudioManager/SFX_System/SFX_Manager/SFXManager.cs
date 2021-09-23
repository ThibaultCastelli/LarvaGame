using System.Collections.Generic;
using UnityEngine;

namespace SFXTC
{
    [RequireComponent(typeof(AudioSourcesPool))]
    public class SFXManager : MonoBehaviour
    {
        #region Variables
        [Header("BEHAVIOUR")]
        [Tooltip("Check if you want to disable all SFXs")]
        [SerializeField] bool useNullSFXPlayer;
        [Tooltip("Check if you want to log any change of state of SFXs")]
        [SerializeField] bool useLoggedSFXPlayer;

        AudioSourcesPool audioSourcesPool;
        #endregion

        #region Initialization
        // Singleton Lazy Initailization
        static SFXManager _instance;
        public static SFXManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Search if the GameObject already exist
                    _instance = FindObjectOfType<SFXManager>();

                    if (_instance == null)
                    {
                        // Create the GameObject with a SFXManager and AudioSourcesPool component
                        GameObject musicManager = new GameObject("SFX_Manager");
                        musicManager.AddComponent<AudioSourcesPool>();
                        _instance = musicManager.AddComponent<SFXManager>();

                        // Prevent the MusicManager to be destroy when changing scene
                        DontDestroyOnLoad(musicManager);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            // Prevent from having more than one SFXManager in the scene
            if (_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            // Prevent the MusicManager to be destroy when changing scene
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            audioSourcesPool = GetComponent<AudioSourcesPool>();
        }
        #endregion

        #region Functions
        public void Play(SFXEvent SFX)
        {
            // Check if the SFX can be play at different audio source
            if (SFX.source != null && SFX.source.isPlaying && !SFX.MultiplePlay)
            {
                Debug.LogError("ERROR : This SFX can't be played in two different audio source at the same time.");
                return;
            }

            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Play {SFX.name}.");
            if (useNullSFXPlayer)
                return;

            // Set the audio source of the SFX and play it
            SFX.source = audioSourcesPool.Request();
            SFX.SetAudioSource();
            SFX.source.Play();
        }

        public void PlayDelayed(SFXEvent SFX, float delay)
        {
            // Check if the SFX can be play at different audio source
            if (SFX.source != null && SFX.source.isPlaying && !SFX.MultiplePlay)
            {
                Debug.LogError("ERROR : This SFX can't be played in two different audio source at the same time.");
                return;
            }

            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Play {SFX.name} with a delay of {delay}s.");
            if (useNullSFXPlayer)
                return;

            // Set the audio source of the SFX and play it with a delay
            SFX.source = audioSourcesPool.Request();
            SFX.SetAudioSource();
            SFX.source.PlayDelayed(delay);
        }

        public void PlayScheduled(SFXEvent SFX, double time)
        {
            // Check if the SFX can be play at different audio source
            if (SFX.source != null && SFX.source.isPlaying && !SFX.MultiplePlay)
            {
                Debug.LogError("ERROR : This SFX can't be played in two different audio source at the same time.");
                return;
            }

            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Play {SFX.name} scheduled at {time}s.");
            if (useNullSFXPlayer)
                return;

            // Set the audio source of the SFX and play it at scheduled time
            SFX.source = audioSourcesPool.Request();
            SFX.SetAudioSource();
            SFX.source.PlayScheduled(time);
        }

        public void Stop(SFXEvent SFX)
        {
            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Stop {SFX.name}.");
            if (useNullSFXPlayer)
                return;

            // Check if the SFX has an audio source and stop it
            if (SFX.source != null)
                SFX.source.Stop();
        }

        public void Pause(SFXEvent SFX)
        {
            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Pause {SFX.name}.");
            if (useNullSFXPlayer)
                return;

            // Check if the SFX has an audio source and pause it
            if (SFX.source != null)
                SFX.source.Pause();
        }

        public void UnPause(SFXEvent SFX)
        {
            // Use different behaviour based on the type of player selected
            if (useLoggedSFXPlayer)
                Debug.Log($"Unpause {SFX.name}.");
            if (useNullSFXPlayer)
                return;

            // Check if the SFX has an audio source and unpause it
            if (SFX.source != null)
                SFX.source.UnPause();
        }
        #endregion
    }
}
