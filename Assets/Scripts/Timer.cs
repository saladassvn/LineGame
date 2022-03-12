using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float startTime = 0;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float t = Time.time - startTime;

        string min = ((int)t / 60).ToString();
        string sec = (t % 60).ToString("f0");

        timerText.text = min + ":" + sec;
    }
}
