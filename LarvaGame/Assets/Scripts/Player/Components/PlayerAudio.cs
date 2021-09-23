using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFXTC;

public class PlayerAudio : MonoBehaviour
{
    #region Variables
    [Header("SFX EVENTS")]
    [SerializeField] SFXEvent onShootGroundSFX;
    [SerializeField] SFXEvent onShootAirSFX;
    [SerializeField] SFXEvent onEmptyShootSFX;
    #endregion

    #region Properties
    public SFXEvent OnShootGroundSFX => onShootGroundSFX;
    public SFXEvent OnShootAirSFX => onShootAirSFX;
    public SFXEvent OnEmptyShootSFX => onEmptyShootSFX;

    #endregion
}
