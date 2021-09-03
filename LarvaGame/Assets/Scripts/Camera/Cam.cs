using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cam : MonoBehaviour
{
    #region Variables
    [Header("CAMERA")]
    [SerializeField] Transform target;
    [SerializeField] [Range(0.01f, 10f)] float followSpeed = 0.05f;
    [SerializeField] [Range(-5f, 5f)] float xOffset = 0f;
    [SerializeField] [Range(-5f, 5f)] float yOffset = 0f;
    [Space]

    [Header("CAMERA CONTROLS")]
    [SerializeField] [Range(0f, 10f)] float lookDistance = 2f;
    //[SerializeField] [Range(0.01f, 0.5f)] float speedLook = 0.05f;
    [SerializeField] [Range(-10f, 10f)] float xOffsetLookDistance = 0f;
    [SerializeField] [Range(-10f, 10f)] float yOffsetLookDistance = 0f;
    //[SerializeField] [Range(1f, 5f)] float dezoomMultiplier = 2f;
    [SerializeField] [Range(0f, 5f)] float dezoomSpeed = 2f;
    [SerializeField] bool showLookDistance = true;
    [Space]

    [Header("BOUNDARIES")]
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;
    [SerializeField] float topBound;
    [SerializeField] float bottomBound;
    [SerializeField] bool showBoundaries = true;
    [Space]

    [Header("CAMERA SHAKE")]
    [SerializeField] [Range(0f, 1f)] float shakePower = 1f;
    [SerializeField] [Range(0f, 2f)] float shakeDuration = 1f;
    [SerializeField] [Range(0f, 4f)] float shakeReduction = 1f;
    [SerializeField] [Range(0f, 4f)] float shakeFrequency = 1f;
    [Space]

    // Position
    float _zOffset = -10f;
    float _defaultCamSize;

    // Target
    Vector3 _targetPos;

    // Infos
    bool _gunInHand = true;
    bool _hasZoomed = true;

    PlayerInput input;
    #endregion

    #region Getters
    public float XOffset { get { return xOffset; } }
    public float YOffset { get { return yOffset; } }
    public float LeftBound { get { return leftBound; } }
    public float RightBound { get { return rightBound; } }
    public float TopBound { get { return topBound; } }
    public float BottomBound { get { return bottomBound; } }
    #endregion

    #region Starts & Updates
    private void Start()
    {
        _defaultCamSize = Camera.main.orthographicSize;

        input = target.GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGamePaused)
        {
            // Reset ortographic size
            if (!_hasZoomed && Camera.main.orthographicSize > _defaultCamSize)
                StartCoroutine(Zoom(_defaultCamSize));

            // Move the camera to the player
            if (_gunInHand)
                MoveCam(target.position.x + xOffset, target.position.y + yOffset, followSpeed);

            // Control the camera
            /*else if (input != null)
            {
                // Dezoom
                if (!_hasZoomed)
                    StartCoroutine(Zoom(_defaultCamSize * dezoomMultiplier));

                float xStick = input.GetAimDirection().x;
                float yStick = input.GetAimDirection().y;

                // Can only look in 4 directions
                if (Mathf.Abs(xStick) < 0.5f || Mathf.Abs(xStick) < Mathf.Abs(yStick))
                    xStick = 0f;
                else
                    xStick = xStick / Mathf.Abs(xStick);

                if (Mathf.Abs(yStick) < 0.5f || Mathf.Abs(yStick) < Mathf.Abs(xStick))
                    yStick = 0f;
                else
                    yStick = yStick / Mathf.Abs(yStick);

                float xTarget = (xStick * lookDistance) + target.position.x + xOffsetLookDistance + xOffset;
                float yTarget = (yStick * lookDistance) + target.position.y + yOffsetLookDistance + yOffset;

                MoveCam(xTarget, yTarget, speedLook);
            }*/
        }
    }
    #endregion

    #region Functions
    public void Initialize(float xPos, float yPos, float xOffset, float yOffset, float orthographicSize, float left, float right, float top, float bot)
    {
        transform.position = new Vector2(xPos, yPos);
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        Camera.main.orthographicSize = orthographicSize;
        leftBound = left;
        rightBound = right;
        topBound = top;
        bottomBound = bot;
    }

    private void MoveCam(float xPos, float yPos, float speed)
    {
        // Clamp the position
        _targetPos = new Vector3(
            Mathf.Clamp(xPos, leftBound, rightBound),
            Mathf.Clamp(yPos, bottomBound, topBound),
            _zOffset);

        // Move with smoothness
        transform.position = Vector3.Lerp(transform.position, _targetPos, speed);
    }

    public void ChangeOffset(Vector2 newOffset)
    {
        xOffset = newOffset.x;
        yOffset = newOffset.y;
    }

    public void ChangeOrtographicSize(float newOrtographicSize)
    {
        //StopAllCoroutines();
        StartCoroutine(Zoom(newOrtographicSize));
        _defaultCamSize = newOrtographicSize;
    }

    public void ChangeBoundaries(float left, float right, float top, float bot)
    {
        leftBound = left;
        rightBound = right;
        topBound = top;
        bottomBound = bot;
    }

    public void CheckGunInHand(bool isHide)
    {
        _gunInHand = isHide;
        _hasZoomed = false;
    }

    public void CamShake(Vector2 dir)
    {
        float limit = 0.1f;

        // Prevent cam shake when changing zone
        if (transform.position.x < leftBound - limit || transform.position.x > rightBound + limit || transform.position.y < bottomBound - limit || transform.position.y > topBound + limit)
            return;

        StartCoroutine("CamShakeCoroutine");
    }
    #endregion

    #region Coroutines
    IEnumerator CamShakeCoroutine()
    {
        float timeLeft = shakeDuration;
        float powerDivider = 1;

        while (timeLeft > 0)
        {
            float currentShakePower = shakePower * powerDivider;
            powerDivider -= Time.deltaTime * shakeReduction;
            timeLeft -= Time.deltaTime;

            float xTarget = UnityEngine.Random.insideUnitCircle.x * currentShakePower + transform.position.x;
            float yTarget = UnityEngine.Random.insideUnitCircle.y * currentShakePower + transform.position.y;

            transform.position = new Vector3(xTarget, yTarget, -10);
            yield return new WaitForSeconds(shakeFrequency);
        }
    }

    IEnumerator Zoom(float newZoomValue)
    {
        // Prevent shakes when changing zone
        StopCoroutine("CamShakeCoroutine");

        _hasZoomed = true;
        float t = 0f;
        float currentCamSize = Camera.main.orthographicSize;

        while (t < 1)
        {
            t = Mathf.Clamp(t + Time.deltaTime * dezoomSpeed, 0, 1);
            Camera.main.orthographicSize = Mathf.Lerp(currentCamSize, newZoomValue, t);
            yield return null;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (showLookDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(target.position.x + xOffsetLookDistance, target.position.y + yOffsetLookDistance), lookDistance);
        }
        if (showBoundaries)
        {
            float _halfHeight = Camera.main.orthographicSize;
            float _halfWidth = _halfHeight * Camera.main.aspect;

            Gizmos.color = Color.red;

            Vector3 topLeft = new Vector3(leftBound - _halfWidth, topBound + _halfHeight);
            Vector3 topRight = new Vector3(rightBound + _halfWidth, topBound + _halfHeight);
            Vector3 bottomLeft = new Vector3(leftBound - _halfWidth, bottomBound - _halfHeight);
            Vector3 bottomRight = new Vector3(rightBound + _halfWidth, bottomBound - _halfHeight);

            Gizmos.DrawLine(topLeft, bottomLeft);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }
}
