using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SFXTC
{
    [CreateAssetMenu(fileName = "Default SFX Event", menuName = "Audio/SFX Event")]
    public class SFXEvent : ScriptableObject
    {
        #region Variables
        [TextArea] [SerializeField] string description;

        [Header("COMPONENTS")]
        [Tooltip("List of clips to be played.\n Only one will be randomly selected when playing.")]
        [SerializeField] List<AudioClip> clips;

        [Tooltip("Mixer's group that will be assign to the clip.")]
        [SerializeField] AudioMixerGroup mixerGroup;
        [Space]

        [Header("INFOS")]
        [Tooltip("Select if the clip should be playing automatically on start.")]
        [SerializeField] bool playOnAwake;

        [Tooltip("Select if the clip should automatically replay.")]
        [SerializeField] bool loop;

        [Tooltip("Select if the clip should be mute.")]
        [SerializeField] bool mute;

        [Tooltip("Select if the clip should ignore the effects applied to his audio source.")]
        [SerializeField] bool bypassEffects;

        [Tooltip("Select if the clip should ignore the reverb zones")]
        [SerializeField] bool bypassReverbZones;

        [Tooltip("Select if this sfx can be play in multiple audio source at the same time.")]
        [SerializeField] bool multiplePlay = true;
        public bool MultiplePlay => multiplePlay;
        [Space]

        [Header("SPECS")]
        [Tooltip("Select the priority of a clip.\nA clip with a low value will have priority on a clip with a high value.\n0 = Highest priority | 256 = Lowest priority")]
        [Range(0, 256)] [SerializeField] int priority = 128;

        [HideInInspector] public bool useRandomVolume;
        [HideInInspector] public float volume = 1f;
        [MinMaxRange(0, 1)]
        [HideInInspector] public RangedFloat randomVolume;

        [HideInInspector] public bool useRandomPan;
        [HideInInspector] [Range(-1f, 1f)] public float pan = 0f;
        [MinMaxRange(-1, 1)]
        [HideInInspector] public RangedFloat randomPan;

        [HideInInspector] public bool useRandomPitch;
        [HideInInspector] [Range(0f, 2f)] public float pitch = 1f;
        [MinMaxRange(0, 2)]
        [HideInInspector] public RangedFloat randomPitch;

        [HideInInspector] public bool useRandomReverbZoneMix;
        [HideInInspector] [Range(0f, 1.1f)] public float reverbZoneMix = 1f;
        [MinMaxRange(0, 1.1f)]
        [HideInInspector] public RangedFloat randomReverbZoneMix;

        [HideInInspector] public AudioSource source;
        #endregion

        #region Functions
        public void Play() { SFXManager.Instance.Play(this); }
        public void PlayDelayed(float delay) { SFXManager.Instance.PlayDelayed(this, delay); }
        public void PlayScheduled(double time) { SFXManager.Instance.PlayScheduled(this, time); }
        public void Stop() { SFXManager.Instance.Stop(this); }
        public void Pause() { SFXManager.Instance.Pause(this); }
        public void UnPause() { SFXManager.Instance.UnPause(this); }

        public void SetAudioSource()
        {
            // Initialize the audio source values
            if (source == null)
            {
                Debug.LogError($"ERROR : There is no source for '{name}'.");
                return;
            }
            if (clips.Count == 0)
            {
                Debug.LogError($"ERROR : There is no audio clip for '{name}'.");
                return;
            }

            source.clip = clips[Random.Range(0, clips.Count)];
            source.outputAudioMixerGroup = mixerGroup;

            source.playOnAwake = playOnAwake;
            source.loop = loop;
            source.mute = mute;
            source.bypassEffects = bypassEffects;
            source.bypassReverbZones = bypassReverbZones;

            source.priority = priority;

            if (useRandomVolume)
                source.volume = Random.Range(randomVolume.minValue, randomVolume.maxValue);
            else
                source.volume = volume;

            if (useRandomPan)
                source.panStereo = Random.Range(randomPan.minValue, randomPan.maxValue);
            else
                source.panStereo = pan;

            if (useRandomPitch)
                source.pitch = Random.Range(randomPitch.minValue, randomPitch.maxValue);
            else
                source.pitch = pitch;

            if (useRandomReverbZoneMix)
                source.reverbZoneMix = Random.Range(randomReverbZoneMix.minValue, randomReverbZoneMix.maxValue);
            else
                source.reverbZoneMix = reverbZoneMix;
        }
        #endregion

        #region Preview Functions
        public void Preview(AudioSource source)
        {
            // Used only to preview on the inspector

            if (clips.Count == 0)
            {
                Debug.LogError($"ERROR : There is no audio clip for '{name}'.");
                return;
            }

            source.clip = clips[Random.Range(0, clips.Count)];
            source.outputAudioMixerGroup = mixerGroup;

            source.playOnAwake = playOnAwake;
            source.loop = loop;
            source.mute = mute;
            source.bypassEffects = bypassEffects;
            source.bypassReverbZones = bypassReverbZones;

            source.priority = priority;

            if (useRandomVolume)
                source.volume = Random.Range(randomVolume.minValue, randomVolume.maxValue);
            else
                source.volume = volume;

            if (useRandomPan)
                source.panStereo = Random.Range(randomPan.minValue, randomPan.maxValue);
            else
                source.panStereo = pan;

            if (useRandomPitch)
                source.pitch = Random.Range(randomPitch.minValue, randomPitch.maxValue);
            else
                source.pitch = pitch;

            if (useRandomReverbZoneMix)
                source.reverbZoneMix = Random.Range(randomReverbZoneMix.minValue, randomReverbZoneMix.maxValue);
            else
                source.reverbZoneMix = reverbZoneMix;

            source.Play();
        }

        public void StopPreview(AudioSource previewer)
        {
            previewer.Stop();
        }
        #endregion
    }
}
