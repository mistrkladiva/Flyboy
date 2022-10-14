using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI txtFlyCount;

    [SerializeField]
    Image Plocha;

    [SerializeField]
    public AudioSource MusicBoss;

    [SerializeField]
    GameObject Boss, Player, Stena;

    public List<GameObject> Flys;
    public static List<GameObject> s_Flys;

    bool boss;

    void Start()
    {
        s_Flys = Flys;
    }

    // Update is called once per frame
    void Update()
    {
        txtFlyCount.text = s_Flys.Count.ToString();

        if (s_Flys.Count == 0)
        {
            boss = true;
        }
        if (boss && !Boss.IsDestroyed())
        {       
            
            Boss.SetActive(true);
            Boss.GetComponent<AudioSource>().volume = 0f;
            s_Flys.Add(Boss);
            StartCoroutine(FadeINOUT());
            boss = false;
        }
    }

    IEnumerator FadeINOUT()
    {
        Color c = Plocha.color;
        for (float alpha = 0; alpha < 1.1f; alpha += 0.1f)
        {
            c.a = alpha;
            Plocha.color = c;
            yield return new WaitForSeconds(0.05f);
        }

        Stena.SetActive(true);
        MusicBoss.volume = 0f;
        MusicBoss.Play();
        Player.transform.position = new Vector3(29.92f, 0, 0);
        Camera.main.transform.position = new Vector3(33.92f, 0, -10);
        Camera.main.GetComponent<CameraRoll>().enabled = false;
        yield return new WaitForSeconds(1f);

        float volume = 0;
        c = Plocha.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            Plocha.color = c;
            MusicBoss.volume = volume;
            Boss.GetComponent<AudioSource>().volume = volume;
            volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
