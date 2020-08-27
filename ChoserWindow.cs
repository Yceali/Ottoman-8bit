using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoserWindow : Window
{

    private NPC npc;

    public NPC MyNpc { get => npc; set => npc = value; }

    public override void Close()
    {
        base.Close();
    }

    public void OpenRole()
    {
        
        if (MyNpc.Current == MyNpc.ChoseWindow)
        {
            MyNpc.ChoseWindow.Close();
            MyNpc.Current = MyNpc.Window;
            MyNpc.Current.Open(MyNpc);
            MyNpc.IsInteracting = true;
        }
    }

    public void OpenChat()
    {
        if (MyNpc.Current == MyNpc.ChoseWindow)
        {
            MyNpc.ChoseWindow.Close();
            MyNpc.Current = MyNpc.ChatWindow;
            MyNpc.Current.Open(MyNpc);
            MyNpc.IsInteracting = true;

        }

    }
}
