using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractible
{
    [SerializeField]
    private string[] chatIndex;

    private int currentChat = 0;

    [SerializeField]
    private Window window;

    [SerializeField]
    private Window chatWindow;

    [SerializeField]
    private Window choseWindow;

    [SerializeField]
    private ChoserWindow choserWindow;

    [SerializeField]
    private ChatWindow chatWindowUI;

    [SerializeField]
    private Sprite npcPortrait;

    [SerializeField]
    private ChatManager chatManager;

    private Window current;
    public bool IsInteracting { get; set; }
    public Window Current { get => current; set => current = value; }
    public Window ChoseWindow { get => choseWindow; set => choseWindow = value; }
    public Window ChatWindow { get => chatWindow; set => chatWindow = value; }
    public Window Window { get => window; set => window = value; }
    public Sprite MyNpcPortrait { get => npcPortrait; set => npcPortrait = value; }
    public int MyCurrentChat { get => currentChat; set => currentChat = value; }
    public string[] MyChatIndex { get => chatIndex;}

    public virtual void Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            choserWindow.MyNpc = this;
            chatWindowUI.MyCurrentNPC = this;
            chatManager.MyCurrentNPC = this;
            chatManager.SetChat();
            chatWindowUI.SetChatWindow();
            Current = ChoseWindow;
            Current.Open(this);            
        }
    }

    public virtual void StopInteract()
    {
        if (IsInteracting)
        {            
            if(Current == ChoseWindow || Current == ChatWindow || Current == Window)
            {
                Current.Close();
                Current = null;
            }
            IsInteracting = false;
        }
    }

    
}
