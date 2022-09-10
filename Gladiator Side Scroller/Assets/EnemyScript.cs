using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;


    [Header("Animation Variables")]
    private Animator animator;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }



    public void DamageBackwards(Transform player)
    {
        Debug.Log("Damaging");
        animator.SetTrigger("Hurt");
        rb.AddForce((transform.position - player.position) * 1.5f, ForceMode2D.Impulse);
    }
}
