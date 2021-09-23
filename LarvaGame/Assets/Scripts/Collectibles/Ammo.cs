using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    #region Variables
    [SerializeField] [Range(0f, 10f)] float timeToRespawn;
    #endregion

    #region Functions
    public void DestroyAmmo(GameObject ammo)
    {
        ammo.SetActive(false);
        StartCoroutine(RespawnCoroutine(ammo));
    }

    IEnumerator RespawnCoroutine(GameObject ammo)
    {
        yield return new WaitForSeconds(timeToRespawn);
        while (GameManager.isGamePaused)
            yield return new WaitForSeconds(timeToRespawn);
        ammo.SetActive(true);
    }
    #endregion
}
