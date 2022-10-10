using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyRespawn : MonoBehaviour
{
    public List<GameObject> Flys;
    public static List<GameObject> s_Flys;

    [SerializeField]
    GameObject Fly, Player;

    void Start()
    {
        s_Flys = Flys;
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn()
    {
        while (true)
        {
            float screenX = Random.Range(-8f, 8f);
            float screenY = Random.Range(2f, 4f);

            var InstantiateFly = Instantiate(Fly, new Vector2(screenX, screenY), Quaternion.identity);

            Physics2D.IgnoreCollision(InstantiateFly.GetComponent<Collider2D>(), Player.GetComponent<Collider2D>());
            s_Flys.Add(InstantiateFly);
            yield return new WaitForSeconds(10f);
        }
    }
}
