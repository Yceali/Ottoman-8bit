using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : Window
{
    [SerializeField]
    private Image chatPortrait;

    private NPC currentNPC;

    public NPC MyCurrentNPC { get => currentNPC; set => currentNPC = value; }

    public override void Close()
    {
        base.Close();
    }

    public void SetChatWindow()
    {
        chatPortrait.sprite = MyCurrentNPC.MyNpcPortrait;
    }
}
