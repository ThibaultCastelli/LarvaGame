using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    #region Variables
    [Header("AUDIO EVENTS")]
    [SerializeField] SimpleAudioEvent onShootAudio;
    [SerializeField] SimpleAudioEvent onShootAirAudio;
    [SerializeField] SimpleAudioEvent onEmptyShootAudio;
    [Space]

    [Header("AUDIO SOURCE")]
    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] AudioSource emptyShootAudioSource;
    #endregion

    #region Properties
    public SimpleAudioEvent OnShootAudio { get { return onShootAudio; } }
    public SimpleAudioEvent OnShootAirAudio { get { return onShootAirAudio; } }
    public SimpleAudioEvent OnEmptyShootAudio { get { return onEmptyShootAudio; } }

    public AudioSource ShootAudioSource { get { return shootAudioSource; } }
    public AudioSource EmptyShootAudioSource { get { return emptyShootAudioSource; } }
    #endregion
}
