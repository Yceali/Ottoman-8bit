using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

public class DayN : MonoBehaviour
{
    private int mins;
    private int hours;
    private int days;
    [SerializeField]
    private int time;
    [SerializeField]
    private Text clock;
    private bool IsNight;

    [SerializeField]
    private Light2D sunLight;

    public bool MyIsNight { get => IsNight; set => IsNight = value; }

    // Start is called before the first frame update
    void Start()
    {
        days = 0;
        InvokeRepeating("Timer", 0f, 0.25f);
        hours = time;
    }

    // Update is called once per frame
    void Update()
    {
       
       if ( hours > 17 || hours < 6)
        {
            IsNight = true;
            sunLight.color = Color.Lerp(sunLight.color, Color.blue, Time.deltaTime*0.1f);            
        }
        else
        {
            IsNight = false;
            sunLight.color = Color.Lerp(sunLight.color, Color.white, Time.deltaTime * 0.1f);
        }
    }

    private void Timer()
    {        
        if (mins > 59)
        {
            mins = 0;
            hours++;
            if (hours > 23)
            {
                hours = 0;
                days++;
                if (days > 6)
                {
                    days = 0;
                }
            }
        }
        UpdateTime();
        mins++;
    }

    private void UpdateTime()
    {
        if (mins <10)
        {
            if (hours < 10)
            {
                clock.text = "0"+hours + ":0" + mins;
            }
            else
            clock.text = hours + ":0" + mins;
        }
        else
        {
            if (hours <10)
            {
                clock.text = "0" + hours + ":" + mins;
            }
            else
            clock.text = hours + ":" + mins;
        }

        switch (days)
        {
            case 0:
                clock.text += "\n Pzt.";
                break;
            case 1:
                clock.text += "\n Sal.";
                break;
            case 2:
                clock.text += "\n Çar.";
                break;
            case 3:
                clock.text += "\n Per.";
                break;
            case 4:
                clock.text += "\n Cum.";
                break;
            case 5:
                clock.text += "\n Cmt.";
                break;
            case 6:
                clock.text += "\n Paz.";
                break;

            default:
                break;
        }

    }
}
