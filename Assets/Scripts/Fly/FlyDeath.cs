using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyDeath : MonoBehaviour
{
    SpriteRenderer sprFlyDeath;
    void Start()
    {
        sprFlyDeath = GetComponent<SpriteRenderer>();
        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        Color c = sprFlyDeath.color;
        for (float alpha = 1; alpha > 0f; alpha -= 0.1f)
        {
            c.a = alpha;
            sprFlyDeath.color = c;
            yield return new WaitForSeconds(0.5f);
        }
        GameController.s_FlysDeath.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
