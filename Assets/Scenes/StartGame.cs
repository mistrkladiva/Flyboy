using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] AudioSource BossMusic;
    [SerializeField] TextMeshProUGUI StatusText;
    public static bool BossMusicPlay;
    public static float BossMusicDuration;

    public static string StatText = string.Empty;
    public static bool YouLose;
    AudioSource sndYouLose;


    void Start()
    {
        sndYouLose = FindObjectsOfType<AudioSource>()
            .Where(aSource => aSource.name == "YouLoseMusic")
            .FirstOrDefault();

        StatusText.text = StatText;

        if(BossMusicPlay)
        {
            BossMusic.time = BossMusicDuration;
            BossMusic.Play();
        }
       
        if (YouLose)
        {
            sndYouLose.Play();
            YouLose = false;
        }
    }
}
