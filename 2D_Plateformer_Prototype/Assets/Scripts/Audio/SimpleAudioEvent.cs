using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Default Simple Audio Event", menuName = "Audio/SimpleAudioEvent")]
public class SimpleAudioEvent : AudioEvent
{
    #region Variables
    [SerializeField] [Multiline] string description;
    [Space]

    [Tooltip("Les clips audios à jouer aléatoirement")]
    public AudioClip[] clips;
    [Space]

    [Tooltip("Le groupe associé au Mixer")]
    public AudioMixerGroup mixerGroup;
    [Space]

    [Header("VOLUME")]
    [SerializeField] [Range(0f, 1f)] float minVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float maxVolume = 1f;
    [Space]

    [Header("PITCH")]
    [SerializeField] [Range(0f, 2f)] float minPitch = 1f;
    [SerializeField] [Range(0f, 2f)] float maxPitch = 1f;
    [Space]

    [Header("PAN")]
    [SerializeField] [Range(-1f, 1f)] float minPan = 0f;
    [SerializeField] [Range(-1f, 1f)] float maxPan = 0f;
    #endregion

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0)
            return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.panStereo = Random.Range(minPan, maxPan);

        source.Play();
    }
}
