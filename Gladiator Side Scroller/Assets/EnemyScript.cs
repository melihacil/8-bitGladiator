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
    [Header("Animation Variables")]
    private Animator animator;



    private bool isPlayerInSightRange;
    private bool isDead;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }



    private void FixedUpdate()
    {     
        if (isDead)
            return;
        isPlayerInSightRange = Physics2D.OverlapCircle(rb.position, 14f, playerLayerMask);
        if (isPlayerInSightRange)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
            animator.SetInteger("AnimState", 2);
            //Debug.Log("Player in sight range");
            //rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
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

    public void DamageBackwards(Transform player)
    {
        Debug.Log("Damaging");
        animator.SetTrigger("Hurt");
        rb.AddForce((transform.position - player.position) * 1.5f, ForceMode2D.Impulse);
        GetComponentInParent<EnemyStats>().UpdateHealth(20);
    }
}
