using System.Collections;
using UnityEngine;

public class PlayerCharacterModel : MonoBehaviour
{
    #region variables
    public PlayerState state;
    public bool facingLeft;
    public PlayerCharacterView view; // Reference to the view

    public float moveSpeed = 5f;

    // Cooldown variables
    public float attackCooldown = 1.5f;
    public float shieldCooldown = 2f;

    private float attackCooldownTimer;
    private float shieldCooldownTimer;
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
        if (state != PlayerState.Attacking && state != PlayerState.Shielding && state != PlayerState.Jumping && attackCooldownTimer <= 0)
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

        // Determine the new size and offset based on the facing direction
        Vector2 newSize = originalSize + new Vector2(15f, 0f);
        Vector2 newOffset = originalOffset + new Vector2(7.5f, 0f);

        if (facingLeft)
        {
            // If facing left, move the offset to the left by two times the width
            newOffset = originalOffset + new Vector2(-4.5f - originalSize.x, 0f);
        }

        // Set the new size and offset
        hitboxCollider.size = newSize;
        hitboxCollider.offset = newOffset;

        yield return new WaitForSeconds(0.7f); // Adjust the attack duration

        // Reset the hitbox collider size and offset
        hitboxCollider.size = originalSize;
        hitboxCollider.offset = originalOffset;

        state = PlayerState.Idle;
        attackCooldownTimer = attackCooldown;
    }
    #endregion

    #region shield
    public void TryShield()
    {
        if (state != PlayerState.Shielding && state != PlayerState.Attacking && state != PlayerState.Jumping && shieldCooldownTimer <= 0)
        {
            StartCoroutine(ShieldRoutine());
        }
    }

    IEnumerator ShieldRoutine()
    {
        state = PlayerState.Shielding;

        yield return new WaitForSeconds(0.7f); // Adjust the shield duration

        state = PlayerState.Idle;
        shieldCooldownTimer = shieldCooldown;
    }
    #endregion

    void Update()
    {
        // Update cooldown timers
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (shieldCooldownTimer > 0)
        {
            shieldCooldownTimer -= Time.deltaTime;
        }
    }

    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Attacking,
        Shielding
    }
}
