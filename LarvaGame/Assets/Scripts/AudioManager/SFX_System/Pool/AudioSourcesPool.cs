using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFXTC
{
    [RequireComponent(typeof(SFXManager))]
    public class AudioSourcesPool : MonoBehaviour
    {
        #region Variables
        [Header("POOL INFOS")]
        [SerializeField] [Range(1, 50)] int defaultPoolSize = 10;

        List<AudioSource> pool;

        GameObject SFXPlayerGO;
        #endregion

        private void Awake()
        {
            SFXPlayerGO = new GameObject("SFX_Player");
            SFXPlayerGO.transform.parent = this.transform;

            // Instantiate and fill the pool
            pool = new List<AudioSource>(defaultPoolSize);

            for (int i = 0; i < defaultPoolSize; i++)
                pool.Add(SFXPlayerGO.AddComponent<AudioSource>());
        }

        public AudioSource Request()
        {
            // Return an audio source that has no clip or that is not playing a clip
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].clip == null || !pool[i].isPlaying)
                    return pool[i];
            }

            // If no audio source are available, create a new one
            AudioSource newAudioSource = SFXPlayerGO.AddComponent<AudioSource>();
            pool.Add(newAudioSource);
            return newAudioSource;
        }
    }
}
