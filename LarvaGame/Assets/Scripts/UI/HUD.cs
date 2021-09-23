using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject[] ammos = new GameObject[2];
    #endregion

    #region Functions
    public void UpdateAmmo(GameObject playerGO)
    {
        Player player = playerGO.GetComponent<Player>();

        // Show the ammos the player can shoot
        switch (player.CurrentAmmos)
        {
            case 2:
                ammos[0].SetActive(true);
                ammos[1].SetActive(true);
                break;

            case 1:
                ammos[0].SetActive(true);
                ammos[1].SetActive(false);
                break;

            case 0:
                ammos[0].SetActive(false);
                ammos[1].SetActive(false);
                break;
        }
    }
    #endregion
}
