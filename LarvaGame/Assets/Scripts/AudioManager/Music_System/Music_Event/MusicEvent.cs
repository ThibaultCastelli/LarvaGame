using UnityEngine;
using UnityEngine.Audio;

namespace MusicTC
{
    #region Enum
    public enum LayerType
    {
        Additive,
        Single
    }
    #endregion

    [CreateAssetMenu(fileName = "Default Music Event", menuName = "Audio/Music Event")]
    public class MusicEvent : ScriptableObject
    {
        #region Variables
        [SerializeField] [TextArea] string description;
        [Space]

        [Header("COMPONENTS")]
        [Tooltip("A list of audio clips that represent different layers of a music.")]
        [SerializeField] AudioClip[] musicLayers;
        [Tooltip("Mixer's group that will be assign to each music layer.")]
        [SerializeField] AudioMixerGroup mixerGroup;
        [Space]

        [Header("MUSIC INFOS")]
        [Tooltip("Select if the layers should automatically replay.")]
        [SerializeField] bool loop = true;
        [Tooltip("Select if you want the music to stop when going from one scene to the other.")]
        [SerializeField] bool stopOnSceneChange = false;
        [Tooltip("Select the default volume for each layer.\n0 = mute | 1 = full sound")]
        [SerializeField] [Range(0, 1)] float defaultVolume = 1;
        [Tooltip("The type of layer blend: \nAdditive : All the layer can be play at the same time.\nSingle : Only one layer can be play at the same time.")]
        [SerializeField] LayerType layerType = LayerType.Additive;

        // Variable for preview functions
        [HideInInspector] public int currentLayer = 0;
        #endregion

        #region Properties
        public AudioClip[] MusicLayers => musicLayers;
        public AudioMixerGroup MixerGroup => mixerGroup;
        public LayerType LayerType => layerType;
        public bool Loop => loop;
        public bool StopOnSceneChange => stopOnSceneChange;
        public float DefaultVolume => defaultVolume;
        #endregion

        #region Functions
        public void Play(float fadeTime = 0)
        {
            MusicManager.Instance.Play(this, fadeTime);
        }

        public void Replay(float fadeTime = 0)
        {
            MusicManager.Instance.Replay(this, fadeTime);
        }

        public void Stop(float fadeTime = 0)
        {
            MusicManager.Instance.Stop(this, fadeTime);
        }

        public void IncreaseLayer(float fadeTime = 0)
        {
            MusicManager.Instance.IncreaseLayer(this, fadeTime);
        }

        public void DecreaseLayer(float fadeTime = 0)
        {
            MusicManager.Instance.DecreaseLayer(this, fadeTime);
        }
        #endregion

        #region Preview Functions
        public void PlayPreview(AudioSource[] previewers)
        {
            for (int i = 0; i < musicLayers.Length; i++)
            {
                if (musicLayers[i] == null)
                    continue;

                previewers[i].clip = musicLayers[i];
                previewers[i].volume = 0;
                previewers[i].loop = loop;
                previewers[i].Play();
            }

            SetLayersVolume(previewers);
        }

        public void StopPreview(AudioSource[] previewers)
        {
            foreach (AudioSource source in previewers)
                source.Stop();
        }

        public void IncreaseLayerPreview(AudioSource[] previewers)
        {
            currentLayer = Mathf.Clamp(++currentLayer, 0, musicLayers.Length - 1);
            Debug.Log("Current Layer : " + currentLayer);

            SetLayersVolume(previewers);
        }

        public void DecreaseLayerPreview(AudioSource[] previewers)
        {
            currentLayer = Mathf.Clamp(--currentLayer, 0, musicLayers.Length - 1);
            Debug.Log("Current Layer : " + currentLayer);

            SetLayersVolume(previewers);
        }

        void SetLayersVolume(AudioSource[] previewers)
        {
            for (int i = 0; i < musicLayers.Length; i++)
            {
                if (musicLayers[i] == null)
                    continue;

                if (layerType == LayerType.Additive)
                {
                    if (i <= currentLayer)
                        previewers[i].volume = defaultVolume;
                    else
                        previewers[i].volume = 0;
                }

                else
                {
                    if (i == currentLayer)
                        previewers[i].volume = defaultVolume;
                    else
                        previewers[i].volume = 0;
                }
            }
        }
        #endregion
    }
}
