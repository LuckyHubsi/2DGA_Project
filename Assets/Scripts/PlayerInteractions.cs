using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerCharacterModel playerModel;

    private void Start()
    {
        // Get the PlayerCharacterModel component attached to the parent GameObject
        playerModel = transform.parent.GetComponent<PlayerCharacterModel>();
        if (playerModel == null)
        {
            Debug.LogError("PlayerCharacterModel component not found on the parent GameObject!");
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

        if (collision.gameObject.CompareTag("Collectable") && playerModel != null)
        {
            if (playerModel.state != PlayerCharacterModel.PlayerState.Attacking)
            {
                // Call the PickUp function of the Collectable script
                Collectable collectable = collision.gameObject.GetComponent<Collectable>();
                if (collectable != null)
                {
                    collectable.PickUp();
                }
            }
        }
    }
}
