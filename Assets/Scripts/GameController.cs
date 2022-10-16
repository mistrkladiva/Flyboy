using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI txtFlyCount;
    [SerializeField] Image Plocha;    
    [SerializeField] GameObject Boss, Player, Stena;
    
    AudioSource BossMusic;
    public List<GameObject> Flys;
    public static List<GameObject> s_Flys;
    public List<GameObject> FlysDeath;
    public static List<GameObject> s_FlysDeath;
    bool boss;

    void Start()
    {
        s_Flys = Flys;
        s_FlysDeath = FlysDeath;

        BossMusic = FindObjectsOfType<AudioSource>()
            .Where(aSource => aSource.name == "BossMusic")
            .FirstOrDefault();
    }

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
        BossMusic.volume = 0f;
        BossMusic.Play();

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
            BossMusic.volume = volume;

            Boss.GetComponent<AudioSource>().volume = volume;
            volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
