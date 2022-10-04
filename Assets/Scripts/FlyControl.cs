using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControl : MonoBehaviour
{
    //[SerializeField]
    Rigidbody2D rigidbody;

    [SerializeField]
    float SpeedX, SpeedY;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddRelativeForce((new Vector2(Random.Range(SpeedX * -1f, SpeedX), Random.Range(SpeedY * -1, SpeedY))), ForceMode2D.Impulse);
    }
}
