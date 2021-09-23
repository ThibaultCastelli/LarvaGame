using UnityEngine;
using ObserverTC;

public class PlayerCollision : MonoBehaviour
{
    #region Variables
    [Header("LAYERS")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask plateformCheckLayer;
    [Space]

    [Header("EVENTS")]
    [SerializeField] NotifierGameObject collectAmmoEvent;

    Player player;

    Coroutine _deathCoroutine;
    Collider2D lastAmmoTouched;
    #endregion

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Ammo":
                if (player.CurrentAmmos < player.DefaultAmmoCount && collision != lastAmmoTouched)
                    collectAmmoEvent.Notify(collision.gameObject);
                lastAmmoTouched = collision;
                break;

            case "Kill":
                _deathCoroutine = StartCoroutine(player.DeathCoroutine());
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Kill")
            StopCoroutine(_deathCoroutine);

        lastAmmoTouched = null;
    }

    #region Functions
    public Collider2D CheckGroundCollision(Transform target, Transform collider)
    {
        Collider2D groundCollider = Physics2D.OverlapBox(collider.position,
            new Vector2(target.localScale.x * 0.9f, target.localScale.y / 20), 0, groundLayer);

        if (groundCollider == null)
            return groundCollider;

        Destructible destructibleObstacle = groundCollider.gameObject.GetComponent<Destructible>();

        if (destructibleObstacle != null && !destructibleObstacle.isActive)
            return null;
        else
            return groundCollider;
    }

    public bool CheckWallCollision(bool checkLeft, Transform target)
    {
        // Initialize direction
        Vector2 dir;
        if (checkLeft)
            dir = Vector2.left;
        else
            dir = Vector2.right;

        RaycastHit2D hit;
        hit = Physics2D.Raycast(target.position, dir, target.localScale.x * 0.7f, groundLayer);

        if (hit.collider != null)
            return true;

        return false;
    }

    public Collider2D CheckClimbCollision(Transform plateformPos)
    {
        return Physics2D.OverlapBox(plateformPos.position, new Vector2(0.05f, 0.1f), 0, plateformCheckLayer);
    }
    #endregion
}
