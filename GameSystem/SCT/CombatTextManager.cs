using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTType { damage,heal,xp}
public class CombatTextManager : MonoBehaviour
{
    
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }

            return instance;
        } 
    }

    [SerializeField]
    private GameObject combatTextPrefab;
  
    public void CreateText(Vector2 position,string text, SCTType type , bool crit)
    {
        position.y += 0.8f;
        Text sct =  Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;

        string before = string.Empty;
        string after = string.Empty;
        switch (type)
        {
            case SCTType.damage:
                before = "-";
                sct.color = Color.red;
                break;
            case SCTType.heal:
                before = "+";
                sct.color = Color.green;
                break;
            case SCTType.xp:
                before = "+";
                after = "TP";
                sct.color = Color.yellow;
                break;
            default:
                break;
        }

        sct.text = before + text + after;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
