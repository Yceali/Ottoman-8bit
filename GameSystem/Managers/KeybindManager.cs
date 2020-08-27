using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KeybindManager : MonoBehaviour
{

    private static KeybindManager instance;

    public static KeybindManager MyInstance 
    { 
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }        
    }

    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    public Dictionary<string, KeyCode> Actionbinds { get; private set; }

    private string bindName;

    // Start is called before the first frame update
    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();
        Actionbinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key , KeyCode keybind)
    {
        Dictionary<string, KeyCode> currenDictionary = Keybinds;

        if (key.Contains("ACT"))
        {
            currenDictionary = Actionbinds;
        }
        if (!currenDictionary.ContainsKey(key))
        {
            currenDictionary.Add(key, keybind);
            UIManager.m_instance.UpdateKeyText(key, keybind);
        }
        else if (currenDictionary.ContainsValue(keybind))
        {
            string myKey = currenDictionary.FirstOrDefault(x => x.Value == keybind).Key;
            currenDictionary[myKey] = KeyCode.None;
            UIManager.m_instance.UpdateKeyText(key, KeyCode.None);
        }

        currenDictionary[key] = keybind;
        UIManager.m_instance.UpdateKeyText(key, keybind);
        bindName = string.Empty;
    }

    public void KeybindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if(bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
