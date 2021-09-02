using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject spawnPos;
    [Space]
    [SerializeField] int defaultPoolSize = 3;

    List<GameObject> _bulletsPool = new List<GameObject>();
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        // Initialize the pool
        for(int i = 0; i < defaultPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform);
            bullet.SetActive(false);
            _bulletsPool.Add(bullet);
        }
    }
    #endregion

    #region Functions
    public void RequestBullet(Vector2 dir)
    {
        // Activate a bullet that is not
        foreach (GameObject bullet in _bulletsPool)
        {
            if (bullet.activeInHierarchy == false)
            {
                bullet.GetComponent<Bullet>().Shoot(dir, spawnPos.transform);
                return;
            }
        }

        // Create a new bullet if all the bullets of the pool are active
        GameObject newBullet = Instantiate(bulletPrefab, this.gameObject.transform);
        _bulletsPool.Add(newBullet);

        newBullet.GetComponent<Bullet>().Shoot(dir, spawnPos.transform);
    }
    #endregion
}
