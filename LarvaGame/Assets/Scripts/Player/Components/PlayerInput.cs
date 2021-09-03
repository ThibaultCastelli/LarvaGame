using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerControls Controls { get; private set; }
    public bool _isUsingGamepad { get; private set; }

    InputControlScheme _gamepadControl;
    InputControlScheme _keyboardControl;
    InputControlScheme _currentControl;

    private void Awake()
    {
        // Enable the input controls
        Controls = new PlayerControls();
        Controls.Player.Enable();
    }

    public float GetMovementX() => Mathf.Round(Controls.Player.Move.ReadValue<Vector2>().x);
    public Vector2 GetAim() => new Vector2(-Mathf.Round(Controls.Player.Aim.ReadValue<Vector2>().x), -Mathf.Round(Controls.Player.Aim.ReadValue<Vector2>().y));
    public Vector2 GetGunDirection() => new Vector2(Mathf.Round(Controls.Player.Aim.ReadValue<Vector2>().x), Mathf.Round(Controls.Player.Aim.ReadValue<Vector2>().y));

    //public Vector2 GetAimDirection() => new Vector2(Controls.Player.Aim.ReadValue<Vector2>().x, Controls.Player.Aim.ReadValue<Vector2>().y);
}
