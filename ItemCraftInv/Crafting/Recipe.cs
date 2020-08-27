using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour , ICastable
{
    [SerializeField]
    private CraftingMaterial[] materials;
    [SerializeField]
    private Item output;
    [SerializeField]
    private int outputCount;
    [SerializeField]
    private string description;
    [SerializeField]
    private Image highlight;

    public Item Output { get => output;}
    public int OutputCount { get => outputCount; set => outputCount = value; }
    public string MyDescription { get => description;}
    public CraftingMaterial[] Materials { get => materials;}

    [SerializeField]
    private float craftTime;
    [SerializeField]
    private Color barColor;


    public string MyTitle
    {
        get
        {
            return output.MyTitle;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return output.MyIcon;
        }
    }

    public float MyCastTime
    {
        get
        {
            return craftTime;
        }
    }

    public Color MyColor
    {
        get
        {
            return barColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = output.MyTitle;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = 0.3f;
        highlight.color = c;
    }

    public void DeSelect()
    {
        Color c = highlight.color;
        c.a = 0;
        highlight.color = c;
    }
}
