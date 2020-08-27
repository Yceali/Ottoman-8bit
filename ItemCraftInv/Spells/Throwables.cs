using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Throwables : IUsable, IMovable, IDescribable, ICastable
{
    [SerializeField]
    private string title;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private string description;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    [SerializeField]
    private Sprite Icon;

    public string MyTitle { get => title; }
    public int MyDamage { get => damage; }
    public float MySpeed { get => speed; }
    public float MyCastTime { get => castTime; }
    public GameObject MySpellPrefab { get => spellPrefab; }
    public Color MyColor { get => barColor; }

    public Sprite MyIcon  { get => Icon; }



    public string GetDescription()
    {
        return string.Format("{0}\nFırlatma süresi: {1} saniye.\n{2}\nHedefe {3} hasar verir.", title,castTime,description,damage);
    }

    public void Use()
    {
        Player.m_instance.Throwables(this);
    }
}
