using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    private float currentValue;
    private float currentFill;

    private float overFlow;

    [SerializeField]
    private float lerpSpeed;
    [SerializeField]
    private Text fieldText;
    public float MyMaxValue { get; set; }

    public float MyOverFlow
    {
        get
        {
            float tmp = overFlow;
            overFlow = 0;
            return tmp;
        }
    }

    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }
    public float MyCurrentValue 
    {
        get
        {
            return currentValue;
        }        
        set 
        {
            if(value > MyMaxValue)
            {
                overFlow = value - MyMaxValue;
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if(fieldText != null)
            {
                fieldText.text = currentValue + "/" + MyMaxValue;
            }
            
        } 
    }

    

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
