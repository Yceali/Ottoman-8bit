using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillBook : MonoBehaviour
{
    private static SkillBook instance;

    public static SkillBook m_instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillBook>();
            }

            return instance;
        }
    }
    [SerializeField]
    private Image castingBar;
    [SerializeField]
    private Text currentThrowable;
    [SerializeField]
    private Text castTime;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine SkillRoutine;
    private Coroutine FadeRotuine;

    [SerializeField]
    private Throwables[] throwables;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cast(ICastable castable)
    {         
        castingBar.fillAmount = 0;
        castingBar.color = castable.MyColor;
        currentThrowable.text = castable.MyTitle;
        icon.sprite = castable.MyIcon;

        SkillRoutine = StartCoroutine(Progress(castable));
        FadeRotuine = StartCoroutine(FadeBar());
 
    }

    private IEnumerator Progress(ICastable castable)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / castable.MyCastTime;
        float progress = 0.0f;

        while(progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            castTime.text = (castable.MyCastTime - timePassed).ToString("F2");

            if(castable.MyCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
                castingBar.fillAmount = 1;
            }
            yield return null;
        }

        StopCast();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.25f;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }

    public void StopCast()
    {
        if(FadeRotuine != null)
        {
            StopCoroutine(FadeRotuine);
            canvasGroup.alpha = 0;
            FadeRotuine = null;
        }
        if(SkillRoutine != null)
        {
            StopCoroutine(SkillRoutine);
            SkillRoutine = null;
        }
    }

    public Throwables GetThrowable(string throwableName)
    {
        Throwables throwable = Array.Find(throwables, x => x.MyTitle == throwableName);
        return throwable;        
    }
}
