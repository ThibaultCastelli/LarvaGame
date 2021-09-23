using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicTC
{
    public class MusicPlayer : MonoBehaviour
    {
        #region Variables
        // The current MusicEvent
        public MusicEvent musicEvent;
        // List of the audio sources for each layer of the MusicEvent
        List<AudioSource> audioSources = new List<AudioSource>();
        #endregion

        #region Initialization
        private void Awake()
        {
            // Create audio sources for playing layers
            for (int i = 0; i < MusicManager.Instance.MaxLayerCount; i++)
                audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
        #endregion

        #region Functions
        public void Play(MusicEvent musicEvent, float fadeTime = 0)
        {
            // Prevent errors
            if (musicEvent == null)
            {
                Debug.LogError("ERROR : The MusicEvent you try to play is null.");
                return;
            }
            else if (fadeTime < 0)
            {
                Debug.LogError("ERROR : The fade time can't be negative.");
                return;
            }

            // Give each layer an audio source and set the default values for the audio source
            SetAudioSources(musicEvent);

            // Prevent from having two fades at the same time
            StopAllCoroutines();

            // Use different functions based on the fade time (instant or fade) and the layer type of the MusicEvent (additive or single)
            if (fadeTime == 0)
            {
                if (musicEvent.LayerType == LayerType.Additive)
                    PlayImmediatelyAdditive();
                else
                    PlayImmediatelySingle();
            }
            else
            {
                if (musicEvent.LayerType == LayerType.Additive)
                    StartCoroutine(PlayFadeAdditive(fadeTime));
                else
                    StartCoroutine(PlayFadeSingle(fadeTime));
            }
        }

        void PlayImmediatelyAdditive()
        {
            // Set the layers that are lower or equal to the current layer audible, mute the others
            for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
            {
                if (musicEvent.MusicLayers[i] == null)
                    continue;

                if (i <= MusicManager.Instance.CurrentLayer)
                    audioSources[i].volume = musicEvent.DefaultVolume;
                else
                    audioSources[i].volume = 0;
            }
        }
        void PlayImmediatelySingle()
        {
            // Set the layers that are equal to the current layer audible, mute the others
            for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
            {
                if (musicEvent.MusicLayers[i] == null)
                    continue;

                if (i == MusicManager.Instance.CurrentLayer)
                    audioSources[i].volume = musicEvent.DefaultVolume;
                else
                    audioSources[i].volume = 0;
            }
        }

        public void Stop(float fadeTime)
        {
            // Prevent errors
            if (musicEvent == null)
            {
                Debug.LogError("ERROR : There is no MusicEvent currently playing.");
                return;
            }
            else if (fadeTime < 0)
            {
                Debug.LogError("ERROR : The fade time can't be negative.");
                return;
            }

            // Prevent from having two fades at the same time
            StopAllCoroutines();

            // Use different functions based on the fade time (instant or fade)
            if (fadeTime == 0)
                StopImmediately();
            else
                StartCoroutine(StopFade(fadeTime));
        }

        void StopImmediately()
        {
            // Stop all the audio sources and delete the current musicEvent;
            foreach (AudioSource source in audioSources)
                source.Stop();

            musicEvent = null;
        }

        void SetAudioSources(MusicEvent newMusicEvent)
        {
            // Prevent errors
            if (newMusicEvent == null)
            {
                Debug.LogError("ERROR : The MusicEvent you try to set is null.");
                return;
            }
            if (newMusicEvent == musicEvent)
                return;

            musicEvent = newMusicEvent;

            // Give each layer an audio source and set the default values for the audio source
            for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
            {
                audioSources[i].clip = musicEvent.MusicLayers[i];
                audioSources[i].outputAudioMixerGroup = musicEvent.MixerGroup;
                audioSources[i].loop = musicEvent.Loop;
                audioSources[i].volume = 0;
                audioSources[i].Play();
            }
        }

        float[] GetStartVolumes()
        {
            // Get the volumes of each layer when starting a fade
            float[] startVolumes = new float[musicEvent.MusicLayers.Length];
            for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
                startVolumes[i] = audioSources[i].volume;

            return startVolumes;
        }
        #endregion

        #region Coroutines
        IEnumerator PlayFadeAdditive(float fadeTime)
        {
            // Initialization
            float elapsedTime = 0;
            float[] startVolumes = GetStartVolumes();

            while (true)
            {
                // Fade in the layers that are lower or equal to the current layer, fade out the others
                for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
                {
                    if (musicEvent.MusicLayers[i] == null)
                        continue;

                    if (i <= MusicManager.Instance.CurrentLayer)
                        audioSources[i].volume = Mathf.Lerp(startVolumes[i], musicEvent.DefaultVolume, elapsedTime / fadeTime);
                    else
                        audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0, elapsedTime / fadeTime);
                }

                // Stop the coroutine when the fade time is passed
                if (elapsedTime == fadeTime)
                    yield break;

                // Update and clamp the elapsed time
                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0, fadeTime);
            }
        }
        IEnumerator PlayFadeSingle(float fadeTime)
        {
            // Initialization
            float elapsedTime = 0;
            float[] startVolumes = GetStartVolumes();

            while (true)
            {
                // Fade in the layers that are equal to the current layer, fade out the others
                for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
                {
                    if (musicEvent.MusicLayers[i] == null)
                        continue;

                    if (i == MusicManager.Instance.CurrentLayer)
                        audioSources[i].volume = Mathf.Lerp(startVolumes[i], musicEvent.DefaultVolume, elapsedTime / fadeTime);
                    else
                        audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0, elapsedTime / fadeTime);
                }

                // Stop the coroutine when the fade time is passed
                if (elapsedTime == fadeTime)
                    yield break;

                // Update and clamp the elapsed time
                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0, fadeTime);
            }
        }

        IEnumerator StopFade(float fadeTime)
        {
            // Initialization
            float elapsedTime = 0;
            float[] startVolumes = GetStartVolumes();

            while (true)
            {
                // Fade out all the layers
                for (int i = 0; i < musicEvent.MusicLayers.Length && i < audioSources.Count; i++)
                {
                    if (musicEvent.MusicLayers[i] == null)
                        continue;

                    audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0, elapsedTime / fadeTime);
                }

                // Stop all the layers and delete the current MusicEvent when fade time is passed
                if (elapsedTime == fadeTime)
                {
                    foreach (AudioSource source in audioSources)
                        source.Stop();
                    musicEvent = null;

                    yield break;
                }

                // Update and clamp the elapsed time
                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0, fadeTime);
            }
        }
        #endregion
    }
}
