using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] float paralaxSpeed = 0.1f, paralaxLayer;
    void FixedUpdate()
    {
        transform.position = new Vector3
            (
                Camera.main.transform.position.x * paralaxSpeed,
                transform.position.y,
                paralaxLayer
            );
    }
}
