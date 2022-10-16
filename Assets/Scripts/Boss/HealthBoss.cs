using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthBoss : MonoBehaviour
{
    Transform HealthForeground;
    float Live;

    void Start()
    {
        HealthForeground = gameObject.transform.Find("HealthBarForeground");
        Live = GetComponentInParent<BossController>().live;
        ShowHealth(Live);
    }

    public void ShowHealth(float healthCount)
    {
        Vector3 healtScaleX = new Vector3((Mathf.Abs(1 / Live) * healthCount), 0.1f, 1);
        HealthForeground.transform.localScale = healtScaleX;
    }
}
