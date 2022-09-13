using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform player;


    [Header("Enemy Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRadius;

    [Header("Animation Variables")]
    private Animator animator;
    [SerializeField] private Transform attackPos;


    private float distance;
    private bool isPlayerInSightRange;
    private bool isDead;
    private bool isInterrupted = false;
    private bool hasAttacked = false;

    private bool enemyForward = true;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
    }

    private void Update()
    {
        distance = rb.position.x - player.position.x;
        if (distance != 0)
        {
            CheckDirection(distance > 0);
            
        }
    }


    private void CheckDirection(bool direction)
    {
        if (direction != enemyForward)
        {
            Turn();
        }
    }
    private void Turn()
    {
        transform.forward *= -1;
        enemyForward = !enemyForward;
    }

    private void FixedUpdate()
    {     
        if (isDead || isInterrupted)
            return;
        isPlayerInSightRange = Physics2D.OverlapCircle(rb.position, 14f, playerLayerMask);
        if (isPlayerInSightRange)
        {
            //Attack block
            if (Vector2.Distance(rb.position, player.position) <= attackRange)
            {
                //Animation state equals to combat idle 
                //Attack();
                animator.SetInteger("AnimState", 1);
                if (!hasAttacked)
                {
                    animator.SetTrigger("Attack");
                    hasAttacked = true;
                    Invoke(nameof(ResetAttack), 1f);
                }
            }
            //Move towards player 
            else
            {
                Vector2 target = new Vector2(player.position.x, rb.position.y);
                Vector2 newPosition = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
                animator.SetInteger("AnimState", 2);
            }
        }
        //Idle
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }

    public void DeathState()
    {

        isDead = true;
        animator.SetTrigger("Death");
    }

    public void ChangeInterruption()
    {
        isInterrupted = !isInterrupted;
    }
    public void DamageBackwards(Transform player)
    {
        Debug.Log("Damaging");
        animator.SetTrigger("Hurt");
        //rb.AddForce((transform.position - player.position) * 1.5f, ForceMode2D.Impulse);
        GetComponentInParent<EnemyStats>().UpdateHealth(20);
    }

    public void Attack()
    {
        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, attackRadius, playerLayerMask);
        if (collision != null)
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponentInParent<PlayerStats>().DamagePlayer(20);



        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }
}
