using System.Collections;
using UnityEngine;
using Spine.Unity;

public class PlayerCharacterModel : MonoBehaviour
{
    #region variables
    public PlayerState state;
    public bool facingLeft;
    public PlayerCharacterView view; // Reference to the view

    public float moveSpeed = 5f;
    #endregion

    #region jump
    public void TryJump()
    {
        if (state != PlayerState.Jumping)
        {
            StartCoroutine(JumpRoutine());
        }
    }

    IEnumerator JumpRoutine()
    {
        state = PlayerState.Jumping;

        // Start the animation immediately
        float animationDuration = 0.3f;
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        float delayTime = 0.3f;
        yield return new WaitForSeconds(delayTime);

        float jumpTime = 1.7f;
        float half = jumpTime * 0.5f;

        float jumpPower = 7f;

        for (float t = 0; t < half; t += Time.deltaTime)
        {
            float powerIncrement = jumpPower * (half - t);
            transform.Translate((powerIncrement * Time.deltaTime) * Vector3.up);

            float horizontalDirection = Input.GetAxis("Horizontal");
            Vector3 horizontalMovement = new Vector3(horizontalDirection * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.Translate(horizontalMovement);

            if (horizontalDirection != 0)
            {
                facingLeft = horizontalDirection < 0;
            }

            yield return null;
        }

        for (float t = 0; t < half; t += Time.deltaTime)
        {
            float powerIncrement = jumpPower * t;
            transform.Translate((powerIncrement * Time.deltaTime) * Vector3.down);

            float horizontalDirection = Input.GetAxis("Horizontal");
            Vector3 horizontalMovement = new Vector3(horizontalDirection * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.Translate(horizontalMovement);

            if (horizontalDirection != 0)
            {
                facingLeft = horizontalDirection < 0;
            }

            yield return null;
        }

        state = PlayerState.Idle;
    }
    #endregion

    #region movement
    public void TryMove(float direction)
    {
        if (state != PlayerState.Attacking && state != PlayerState.Shielding)
        {
            if (state != PlayerState.Jumping)
            {
                state = (direction == 0) ? PlayerState.Idle : PlayerState.Running;


                Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
                transform.Translate(movement);

                if (direction != 0)
                {
                    facingLeft = direction < 0f;
                }

            }
        }
    }
    #endregion

    #region attack
    public void TryAttack()
    {
        if (state != PlayerState.Attacking && state != PlayerState.Shielding && state != PlayerState.Jumping)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        state = PlayerState.Attacking;

        // Find the BoxCollider2D component on the "hitbox" child object
        BoxCollider2D hitboxCollider = transform.Find("HitBox").GetComponent<BoxCollider2D>();

        if (hitboxCollider == null)
        {
            Debug.LogError("Hitbox BoxCollider2D not found!");
            yield break;
        }

        // Store the original size and offset of the hitbox collider
        Vector2 originalSize = hitboxCollider.size;
        Vector2 originalOffset = hitboxCollider.offset;

        // Extend the hitbox collider forward (adjust as needed)
        hitboxCollider.size += new Vector2(2f, 0f);
        hitboxCollider.offset += new Vector2(1f, 0f);

        yield return new WaitForSeconds(0.7f); // Adjust the attack duration

        // Reset the hitbox collider size and offset
        hitboxCollider.size = originalSize;
        hitboxCollider.offset = originalOffset;

        state = PlayerState.Idle;
    }
    #endregion

    #region shield
    public void TryShield()
    {
        if (state != PlayerState.Shielding && state != PlayerState.Attacking && state != PlayerState.Jumping)
        {
            StartCoroutine(ShieldRoutine());
        }
    }

    IEnumerator ShieldRoutine()
    {
        state = PlayerState.Shielding;

        yield return new WaitForSeconds(0.7f); // Adjust the attack duration

        state = PlayerState.Idle;
    }
    #endregion

    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Attacking,
        Shielding
    }
}
