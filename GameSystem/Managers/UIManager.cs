using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private RectTransform tooltipRect;


    private Text toolTipText;
    public static UIManager m_instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButtons[] actionButtons;
    
    [SerializeField]
    private GameObject targetFrame;

    private Stat healthState;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup skillBookUI;

    private GameObject[] keybindButtons;

    [SerializeField]
    private CharacterPanel charPanel;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private CanvasGroup[] menus;


    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        toolTipText = toolTip.GetComponentInChildren<Text>();
    }
    void Start()
    {      
        healthState = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.m_instance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenClose(menus[3]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(menus[6]);
        }
    }


    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        healthState.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        portraitFrame.sprite = target.MyPortrait;
        levelText.text = target.MyLevel.ToString();
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Player.m_instance.MyLevel +5)
        {
            levelText.color = Color.red;
        }
        else if (target.MyLevel == Player.m_instance.MyLevel + 3 || target.MyLevel == Player.m_instance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.MyLevel >= Player.m_instance.MyLevel - 2 && target.MyLevel <= Player.m_instance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.m_instance.MyLevel - 3 && target.MyLevel > XpManager.CalculateGraylevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.gray;
        }
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthState.MyCurrentValue = health;
    }


    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);           
        }
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.enabled = true;
            clickable.MyIcon.enabled = true; ;
        }
        else
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = true;
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.enabled = false;
            clickable.MyStackText.enabled = false;
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.enabled = false;
        clickable.MyIcon.enabled = true;
    }
    public void ShowTooltip(Vector2 pivot,Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        toolTip.SetActive(true);
        toolTip.transform.position = position;
        toolTipText.text = description.GetDescription();
    }
    public void HideTooltip()
    {
        toolTip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        toolTipText.text = description.GetDescription();
    }
}
