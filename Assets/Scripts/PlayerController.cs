using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [SerializeField]
    GameObject FlyDeath, Fly;
    [SerializeField]
    TextMeshProUGUI txtFlyDeathCount;


    float direction = 0, speedRun = 30, Scale;
    bool run, jump, idle, attack, grounded;
    int jumpCount = 0;
    int flyDeathCount = 0;

    Vector2 velocity;
    Collider2D colider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Scale = transform.localScale.x;
        colider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        grounded = colider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        
        // je ve vzduchu?
        if (grounded)
        {
            jump = false;
            jumpCount = 0;
        }
        else
        {
            jump = true;
            if (!attack)
                animator.Play("PlayerJump");
        }
            
        if (direction != 0)
        {
            Run();
        }

        if (direction == 0)
        {
            if (grounded && !jump && !attack)
            {
                animator.Play("PlayerIdle");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject goColider = collision.transform.parent.gameObject;

        if (collision.tag == "Fly" && attack)
        {   
            flyDeathCount++;
            txtFlyDeathCount.text = flyDeathCount.ToString();
            if (flyDeathCount > 9)
            {
                // You Win!
                flyDeathCount = 0;
            }

            var flyDeathInstance = Instantiate(FlyDeath, goColider.transform.position, Quaternion.identity);
            FlyRespawn.s_Flys.Remove(goColider);
            Destroy(goColider);

            Physics2D.IgnoreCollision(flyDeathInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());            

            foreach (var createFly in FlyRespawn.s_Flys)
            {
                Physics2D.IgnoreCollision(flyDeathInstance.GetComponent<Collider2D>(), createFly.GetComponent<Collider2D>());
            }            
        }
    }

    void OnMove(InputValue input)
    {
        direction = input.Get<float>();
    }

    void OnJump()
    {
        Jump();
    }

    void OnAttack()
    {
        Attack();
    }

    void Run()
    {
        transform.localScale = new Vector2(Scale * direction, transform.localScale.y);
        var velocity = rb.velocity;
        velocity.x += direction * Time.deltaTime * speedRun;
        rb.velocity = velocity;

        if (grounded && (!jump && !attack))
            animator.Play("PlayerRun");
    }

    void Jump()
    {
        if (jumpCount < 1)
        {
            velocity = rb.velocity;
            velocity.y = 15;
            rb.velocity = velocity;
            if(!attack)
                animator.Play("PlayerJump", 0, 0);
            jumpCount++;
            jump = true;
        }
    }
    void Attack()
    {
        attack = true;
        animator.Play("PlayerAttack");
    }

    // metoda pro Animation event spustí se po dokonèení animace
    public void AttackEnd()
    {
        attack = false;
    }

}
