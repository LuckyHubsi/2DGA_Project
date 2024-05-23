using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWithEnemy : MonoBehaviour
{
    private PlayerCharacterModel playerModel;

    private void Start()
    {
        // Get the PlayerCharacterModel component attached to the same GameObject
        playerModel = GetComponent<PlayerCharacterModel>();
        if (playerModel == null)
        {
            Debug.LogError("PlayerCharacterModel component not found on the player!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerModel != null)
        {
            if (playerModel.state == PlayerCharacterModel.PlayerState.Attacking)
            {
                // Get the parent GameObject of the collision object (the enemy skeleton)
                GameObject enemySkeleton = collision.transform.parent.gameObject;

                // Check if the enemySkeleton has an Enemy script attached
                Enemy enemyScript = enemySkeleton.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Call the Dying function of the Enemy script
                    enemyScript.Dying();
                }
            }
            else if (playerModel.state != PlayerCharacterModel.PlayerState.Shielding)
            {
                GameManager.instance.DecreaseHealth(1);
            }
        }
    }
}
