using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    
    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    

    private void Update()
    {
        TimerUI();
    }

    private void TimerUI()
    {
        this.secondsCount += Time.deltaTime;
        double rounded = Math.Round(this.secondsCount);
        this.timerText.text = $"{this.hourCount:00}:{this.minuteCount:00}:{rounded:00}";
        if (this.secondsCount >= 60)
        {
            this.minuteCount++;
            this.secondsCount = 0;
        }
        else if (this.minuteCount >= 60)
        {
            this.hourCount++;
            this.minuteCount = 0;
        }
    }
}
