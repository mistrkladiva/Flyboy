using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlyRespawn : MonoBehaviour
{
    [SerializeField] GameObject Fly, Player;
    [SerializeField] float SpeedRespawn;

    public int live = 3;

    [Header("View Range")]
    [Range(0, 6)]
    public float radius = 6;
    [Range(0, 359)]
    public int rangeMin = 0, rangeMax = 180;
    public float posX, posY;

    void Start()
    {
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn()
    {
        while (true)
        {
            float rndAngle = Mathf.Deg2Rad * Random.Range(rangeMin, rangeMax);
            float rndRadius = Random.Range(0, radius);

            float screenX = Mathf.Cos(rndAngle) * rndRadius + transform.position.x + posX;
            float screenY = Mathf.Sin(rndAngle) * rndRadius + transform.position.y + posY;

            var InstantiateFly = Instantiate(Fly, new Vector3(screenX, screenY, Fly.transform.position.z), Quaternion.identity);
            Physics2D.IgnoreCollision(InstantiateFly.GetComponent<Collider2D>(), Player.GetComponent<Collider2D>());
            
            foreach (var deathFly in GameController.s_FlysDeath)
            {
                Physics2D.IgnoreCollision(InstantiateFly.GetComponent<Collider2D>(), deathFly.GetComponent<Collider2D>());
            }
            GameController.s_Flys.Add(InstantiateFly);
            yield return new WaitForSeconds(SpeedRespawn);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 stredkKoule, oldStredKoule;
        oldStredKoule = AngleToAxis(rangeMin);

        for (int i = rangeMin; i <= rangeMax; i += 10)
        {
            stredkKoule = AngleToAxis(i);
            Gizmos.DrawLine(stredkKoule, oldStredKoule);
            Gizmos.DrawLine(AngleToAxis(rangeMin), stredkKoule);
            Gizmos.DrawLine(oldStredKoule, AngleToAxis(rangeMax));
            oldStredKoule = stredkKoule;
        }
    }
    Vector2 AngleToAxis(float angle)
    {
        angle = Mathf.Deg2Rad * angle;
        float x = Mathf.Cos(angle) * radius + transform.position.x + posX;
        float y = Mathf.Sin(angle) * radius + transform.position.y + posY;
        return new Vector2(x, y);
    }
}