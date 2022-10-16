using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoll : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float minX = 0.1f, maxX = 35f;

    void LateUpdate()
    {
        float distance = Player.transform.position.x - transform.position.x;
        if(distance > 4f && transform.position.x < maxX)
                transform.position = new Vector3 (Player.transform.position.x-4f, transform.position.y, transform.position.z);
        if (distance < -4f && transform.position.x > minX)
            transform.position = new Vector3(Player.transform.position.x + 4f, transform.position.y, transform.position.z);
    }

}
