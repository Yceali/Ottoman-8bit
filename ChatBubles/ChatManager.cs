using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    [SerializeField]
    private Text chatBuble;

    private NPC currentNPC;

    public NPC MyCurrentNPC { get => currentNPC; set => currentNPC = value; }

    public void SetChat()
    {
        chatBuble.text = MyCurrentNPC.MyChatIndex[MyCurrentNPC.MyCurrentChat];
    }

    public void NextChat()
    {
        if (MyCurrentNPC.MyCurrentChat < MyCurrentNPC.MyChatIndex.Length-1)
        {
            MyCurrentNPC.MyCurrentChat++;
            SetChat();
        }
        else
        {
            MyCurrentNPC.StopInteract();
        }

    }
}
