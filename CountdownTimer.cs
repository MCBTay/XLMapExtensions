using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;


public class CountdownTimer : MonoBehaviour
{
    [Tooltip("Place your TextMesh Pro text object into here")]
    [SerializeField] public TextMeshProUGUI countdownText;
    [Tooltip("Set in seconds how long you wish the timer to be")]
    [SerializeField] float timeLimit = 0f;
    [Tooltip("If you check this box it will display you timer as 2:00 for 2 minutes and countdown like 2:00, 1:59, 1:58, etc. If you don't check it, it will countdown like 120, 119, 118 etc")]
    [SerializeField] bool inMinutes;

    [Space]
  
    public UnityEvent onStart; 
    public UnityEvent onComplete;

    float time;
    bool startTimer;

    TimeSpan timeConverter;

    private void Start()
    {
        countdownText.text = timeLimit.ToString();
        time = timeLimit;
        startTimer = false;

        if (inMinutes)
        {
            timeConverter = TimeSpan.FromSeconds(time);
            float minutes = timeConverter.Minutes;
            float seconds = timeConverter.Seconds;

            countdownText.text = $"{minutes}:{seconds}";
        }
        else
        {
            countdownText.text = Mathf.CeilToInt(time).ToString();
        }

    }
    public void StartTimer()
    {
        startTimer = true;

        onStart?.Invoke();
    }
    private void Update()
    {
        if (!startTimer) return;

        if(time > 0f)
        {
            time -= Time.deltaTime;

            if (inMinutes)
            {
                timeConverter = TimeSpan.FromSeconds(time);
                float minutes = timeConverter.Minutes;
                float seconds = timeConverter.Seconds;

                countdownText.text = $"{minutes}:{seconds}";
            }
            else
            {
             countdownText.text = Mathf.CeilToInt(time).ToString();
            }

        }
        else
        {
            startTimer = false;
            onComplete?.Invoke();
        }
    }

    public void StopTimer()
    {
        if (startTimer)
        {
            startTimer = false;
        }
    }

    public void RestartTimer()
    {
        time = timeLimit;
        if (inMinutes)
        {
            timeConverter = TimeSpan.FromSeconds(time);
            float minutes = timeConverter.Minutes;
            float seconds = timeConverter.Seconds;

            countdownText.text = $"{minutes}:{seconds}";
        }
        else
        {
            countdownText.text = Mathf.CeilToInt(time).ToString();
        }

        startTimer = true;
    }
}
