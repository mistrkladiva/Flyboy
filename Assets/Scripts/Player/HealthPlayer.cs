using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    Transform HealthForeground;
    [SerializeField]
    GameObject Player;

    float Live;
    void Start()
    {
        HealthForeground = gameObject.transform.Find("HealthBarForeground");
        Live = Player.GetComponent<PlayerController>().live;
        ShowHealth(Live);
    }

    public void ShowHealth(float healthCount)
    {
        Vector3 healtScaleX = new Vector3((1 / Live) * healthCount, 0.1f, 1);
        HealthForeground.transform.localScale = healtScaleX;
    }

    private void Update()
    {
        transform.position = Player.transform.position + new Vector3(-0.7f,1.1f,0);
    }
}
