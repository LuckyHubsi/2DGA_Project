using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; // Minimum distance for attack
    public float moveSpeed;
    public float timer; // Timer for cooldown between attacks
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance; // Store the distance between enemy and player
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    private bool facingLeft = true; // Determines the initial facing direction
    #endregion

    private void Awake()
    {
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // Flip the enemy to face the correct direction
        if (!facingLeft)
        {
            Flip();
        }
    }

    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, facingLeft ? Vector2.right : Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        // When Player is detected
        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else
        {
            inRange = false;
            anim.SetBool("CanWalk", false);
            StopAttack();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("CanWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Skeleton_Attacking"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; // Reset Timer when Player enters Attack Range
        attackMode = true; // To check if Enemy can still attack or not

        anim.SetBool("CanWalk", false);
        anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void Dying()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
        anim.SetBool("CanWalk", false);
        moveSpeed = 0;

        anim.SetBool("TookDamage", true);
    }

    void Death()
    {
        Destroy(this.gameObject);
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, facingLeft ? Vector2.right * rayCastLength : Vector2.left * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, facingLeft ? Vector2.right * rayCastLength : Vector2.left * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    public void SetFacingDirection(bool faceLeft)
    {
        facingLeft = faceLeft;
    }

    void Flip()
    {
        // Get the current rotation
        Vector3 currentRotation = transform.localEulerAngles;

        // Flip the rotation along the y-axis to 180 degrees
        transform.localEulerAngles = new Vector3(currentRotation.x, 180f, currentRotation.z);
    }
}
