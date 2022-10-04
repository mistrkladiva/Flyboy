using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControl : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [SerializeField]
    Animator animator;

    [SerializeField]
    AudioSource sndBee;
    
    [SerializeField]
    float SpeedX, SpeedY;

    bool fly = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(fly)
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
            fly = false;
            rigidbody.Sleep();
        }
    }

    void Flying()
    {
        animator.Play("FlyAnimation");
        sndBee.UnPause();
        StopCoroutine(Wait());
        

        float rndX = Random.Range(SpeedX * -1f, SpeedX);
        float rndY = Random.Range(SpeedY * -1f, SpeedY);

        // test letu vpravo - vlevo
        if (rndX < 0f)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        }
        rigidbody.AddRelativeForce(new Vector2(rndX, rndY), ForceMode2D.Impulse);
    }

    void Idle()
    {
        animator.Play("FlyIdleAnimation");
        sndBee.Pause();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (!fly)
        {
            
            yield return new WaitForSeconds(3f);
            fly = true;
        }
    }
}
