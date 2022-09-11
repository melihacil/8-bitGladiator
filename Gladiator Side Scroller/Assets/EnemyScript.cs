using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayerMask;



    [Header("Enemy Movement")]
    [SerializeField] private float moveSpeed;
    [Header("Animation Variables")]
    private Animator animator;



    private bool isPlayerInSightRange;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }


    public void Update()
    {
        if (GetComponentInParent<EnemyStats>().isDead)
            animator.SetTrigger("Death");


        

    }

    private void FixedUpdate()
    {
        isPlayerInSightRange = Physics2D.OverlapCircle(rb.position, 14f, playerLayerMask);
        if (isPlayerInSightRange)
        {
            animator.SetInteger("AnimState", 2);
            Debug.Log("Player in sight range");
            rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }
    public void DamageBackwards(Transform player)
    {
        Debug.Log("Damaging");
        animator.SetTrigger("Hurt");
        rb.AddForce((transform.position - player.position) * 1.5f, ForceMode2D.Impulse);
        GetComponentInParent<EnemyStats>().UpdateHealth(20);
    }
}
