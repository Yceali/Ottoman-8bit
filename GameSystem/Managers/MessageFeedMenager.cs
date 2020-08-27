using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedMenager : MonoBehaviour
{

    private static MessageFeedMenager instance;

    [SerializeField]
    private GameObject messagePrefab;


    public static MessageFeedMenager MyInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageFeedMenager>();
            }
            return instance;
        }
    }

    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePrefab, transform);
        go.GetComponent<Text>().text = message;

        go.transform.SetAsFirstSibling();
        Destroy(go, 2);
    }
}
