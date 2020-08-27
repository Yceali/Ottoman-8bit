using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string throwableName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandScript.m_instance.TakeMovable(SkillBook.m_instance.GetThrowable(throwableName));
        }
    }
}
