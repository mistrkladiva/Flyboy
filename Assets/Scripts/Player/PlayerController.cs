using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Windows;
using System.Security.Claims;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField]
    Animator bossAnimator;
    [SerializeField]
    Image Plocha;

    [SerializeField]
    GameObject FlyDeath, Fly, HealthPlayer;

    float direction = 0, speedRun = 30, Scale;
    bool run, jump, idle, attack, attackPressed, grounded;
    int jumpCount = 0;

    [SerializeField]
    public int live;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fly" && !attack)
        {
            HealthPlayer.GetComponent<HealthPlayer>().ShowHealth(--live);

        }

        if (collision.tag == "Boss" && !attack)
        {
            HealthPlayer.GetComponent<HealthPlayer>().ShowHealth(--live);
        }

        if (live <= 0)
        {
            StatsText.StatText = "YOU LOSE!";
            SceneManager.LoadScene("Start");
        }
            
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Fly" && attack)
        {
            GameObject goColider = collision.transform.parent.gameObject;

            // vytvo�� mrtvou mouchu a odstran� p�vodn� mouchu
            var flyDeathInstance = Instantiate(FlyDeath, goColider.transform.position, Quaternion.identity);
            GameController.s_Flys.Remove(goColider);
            Destroy(goColider);
            // moucha by nem�la kolidovat s playerem
            Physics2D.IgnoreCollision(flyDeathInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // Ostatn� mouchy by nem�ly kolidovat s mrtvolami - moc nefunguje TODO
            foreach (var createFly in GameController.s_Flys)
            {
                Physics2D.IgnoreCollision(flyDeathInstance.GetComponent<Collider2D>(), createFly.GetComponent<Collider2D>());
            }

        }

        if (collision.tag == "Garbage" && attackPressed)
        {
            // animace blik�n� garbage
            collision.GetComponent<Animator>().Play("GarbageUnderAttack", 0, 0);
            collision.GetComponent<AudioSource>().Play();
            // ode�et �ivot� garbage
            float garbageLive = --collision.GetComponent<FlyRespawn>().live;
            // zobraz� aktu�ln� healtBar
            collision.GetComponentInChildren<Health>().ShowHealth(garbageLive);
            // po dosa�en� limitu zni� objekt
            if (collision.GetComponent<FlyRespawn>().live < 1)
                Destroy(collision.gameObject);

            attackPressed = false;
        }

        if (collision.tag == "Boss" && attackPressed)
        {
            bossAnimator.Play("FlyBossUnderAttack");
            float bossLive = --collision.GetComponent<BossController>().live;
            collision.GetComponentInChildren<HealthBoss>().ShowHealth(bossLive);

            if (collision.GetComponent<BossController>().live < 1)
            //if (GameController.s_Flys.Count == 0)
            {
                GameController.s_Flys.Remove(collision.gameObject);
                Destroy(collision.gameObject);
                StartCoroutine(FadeOUT());
            }

            attackPressed = false;
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

    void OnAttack(InputValue input)
    {
        if (input.isPressed)
        {
            attackPressed = true;
            Attack();
        }
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
            if (!attack)
                animator.Play("PlayerJump", 0, 0);
            jumpCount++;
            jump = true;
        }
    }
    void Attack()
    {
        attack = true;
        animator.Play("PlayerAttack");
        StartCoroutine(AttackAnimation());
        GetComponent<AudioSource>().Play();
    }

    // metoda pro Animation event spust� se po dokon�en� animace
    public void AttackEnd()
    {
        attack = false;
        attackPressed = false;
    }

    IEnumerator AttackAnimation()
    {
        //animator.Play("PlayerAttack");
        yield return new WaitForSeconds(0.3f);
        attack = false;
        attackPressed = false;
    }

    IEnumerator FadeOUT()
    {
        Color c = Plocha.color;
        for (float alpha = 0; alpha < 1.1f; alpha += 0.1f)
        {
            c.a = alpha;
            Plocha.color = c;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
        StatsText.StatText = "YOU WIN!";
        SceneManager.LoadScene("Start");
        //float volume = 0;
        //c = Plocha.color;
        //for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        //{
        //    c.a = alpha;
        //    volume += 0.1f;
        //    yield return new WaitForSeconds(0.1f);
        //}
    }
}
