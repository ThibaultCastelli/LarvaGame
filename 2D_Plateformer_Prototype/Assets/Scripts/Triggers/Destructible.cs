using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    #region Variables
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer sprite;
    [Space]
    [SerializeField] [Range(0f, 4f)] float timeToRespawn = 1f;

    public bool isActive { get; private set; } = true;

    Color _defaultColor;
    bool _playerTrigger;
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        _defaultColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _playerTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _playerTrigger = false;
    }
    #endregion

    #region Functions
    public void Destroy()
    {
        boxCollider.isTrigger = true;
        gameObject.layer = 0;
        sprite.color = new Color(1, 1, 1, 0);
        isActive = false;

        StartCoroutine(Reset());
    }
    #endregion

    #region Coroutines
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(timeToRespawn);
        while (_playerTrigger)
            yield return null;

        boxCollider.isTrigger = false;
        gameObject.layer = 10;
        sprite.color = _defaultColor;
        isActive = true;
    }
    #endregion
}
