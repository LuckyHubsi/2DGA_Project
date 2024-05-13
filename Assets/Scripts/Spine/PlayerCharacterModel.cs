using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This is the M(odel) Part of the MVC Model
/// This class is responsible for all our characters functionality
/// as well as keeping track in which state the character currently is
/// </summary>

public class PlayerCharacterModel : MonoBehaviour
{
    #region variables
    public PlayerState state;
    public bool facingLeft;
    #endregion

    #region jump
    public void TryJump() {
        StartCoroutine(JumpRoutine());
    }

    IEnumerator JumpRoutine() {
        if (state == PlayerState.Jumping) {
            yield break;
        }


        state = PlayerState.Jumping;

        // Start the animation immediately
        float animationDuration = 0.3f; 
        float timer = 0f;
        while (timer < animationDuration)
        {
            // Update animation here
            timer += Time.deltaTime;
            yield return null;
        }

        // Delayed movement
        float delayTime = 0.3f; 
        yield return new WaitForSeconds(delayTime);

        // Horizontal movement while jumping
        float horizontalSpeed = 3f; 
        float horizontalDirection = Input.GetAxis("Horizontal"); 
        Vector3 horizontalMovement = new Vector3(horizontalDirection * horizontalSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(horizontalMovement);

        // our starting position in local space
        Vector3 position = transform.localPosition;
        // how long the jumo will take
        float jumpTime = 1.7f;
        float half = jumpTime * 0.5f;
        // the power of the jump, changes the jumpheight
        float jumpPower = 7f;

        // this is the first half of the jump, our character is getting propelled upwards
        for (float t = 0; t < half; t += Time.deltaTime) {
            // we calculate the power increment for the given frame
            // the power increment grows smaller with the t getting larger over time
            float powerIncrement = jumpPower * (half - t);
            // move the characters translate by the calculated power increment upwards each frame
            transform.Translate((powerIncrement * Time.deltaTime) * Vector3.up);
            // start next loop
            yield return null;
        }
        // this is the second half of the jump, which starts then the character reaches the highest point of the jump
        // our character is getting propelled downwards
        for (float t = 0; t < half; t += Time.deltaTime) {
            /* we calculate the power increment for the given frame
             * the power increment grows larger with the t getting larger over time
             * we want the power to get smaller to simulate our character getting dragged to the ground by gravity */
            float powerIncrement = jumpPower * t;
            // move the characters translate by the calculated power increment downward each frame
            transform.Translate((powerIncrement * Time.deltaTime) * Vector3.down);
            // start next loop
            yield return null;
        }
        // reset the character to starting position
        transform.localPosition = position;

        state = PlayerState.Idle;
    }
    #endregion

    #region movement
    public float moveSpeed = 5f; // Adjust this speed according to your needs

    public void TryMove(float direction) {

        if (state != PlayerState.Jumping)
        {
            state = (direction == 0) ? PlayerState.Idle : PlayerState.Running;

            // Move the character
            Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.Translate(movement);

            // Adjust facing direction
            if (direction != 0)
            {
                bool facingLeft = direction < 0f;
                this.facingLeft = facingLeft;
            }
        }
    }
    #endregion

    #region attack

    public void TryAttack()
    {
        if (state != PlayerState.Attacking && state != PlayerState.Jumping)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        state = PlayerState.Attacking;

        // Implement your attack logic here
        Debug.Log("Attacking...");

        yield return new WaitForSeconds(0.7f); // Adjust the attack duration

        state = PlayerState.Idle;
    }

    #endregion

    #region shield

    public void TryShield()
    {
        if (state != PlayerState.Shielding && state != PlayerState.Jumping)
        {
            StartCoroutine(ShieldRoutine());
        }
    }

    IEnumerator ShieldRoutine()
    {
        state = PlayerState.Shielding;

        // Implement your shield logic here
        Debug.Log("Shielding...");

        yield return new WaitForSeconds(3f); // Adjust the shield duration

        state = PlayerState.Idle;
    }

    #endregion

    public enum PlayerState { 
        Idle,
        Running,
        Jumping,
        Attacking,
        Shielding
    }
}
