using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControl : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    Animator animator;

    [SerializeField]
    AudioSource sndBee;

    [SerializeField]
    float SpeedX, SpeedY;
    float rndX, rndY;

    bool flying = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitForRotation());
    }

    void Update()
    {
        if(flying)
        {
            Flying();
        }
        else
        {
            Idle();
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Obstackle")
        {
            flying = false;
            rb.Sleep();
        }
    }

    void Flying()
    {
        animator.Play("FlyAnimation");
        sndBee.UnPause();
        StopCoroutine(Wait());
        
        // test letu vpravo - vlevo
        if (rndX < 0f)
        {
            transform.localScale = new Vector2(0.3f * -1f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(0.3f * 1f, transform.localScale.y);
        }
        rb.AddRelativeForce(new Vector2(rndX, rndY), ForceMode2D.Impulse);
    }

    void Idle()
    {
        animator.Play("FlyIdleAnimation");
        sndBee.Pause();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (!flying)
        {           
            yield return new WaitForSeconds(3f);
            flying = true;
        }
    }
    IEnumerator WaitForRotation()
    {
        while (true)
        {
            rndX = Random.Range(SpeedX * -1f, SpeedX);
            rndY = Random.Range(SpeedY * -1f, SpeedY);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
