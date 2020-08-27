using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private Text questDescription;

    private static QuestLog instance;

    private List<QuestScript> questScripts = new List<QuestScript>();
    private List<Quest> quests = new List<Quest>();

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt;

    [SerializeField]
    private int maxCount;
    private int currentCount;

    public static QuestLog MyInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }

            return instance;
        }
    }

    public List<Quest> MyQuests { get => quests; set => quests = value; }

    public void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }


    public void AcceptQuest(Quest quest)
    {
        if(currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;

            foreach (CollectObjective objective in quest.MyCollectObjectives)
            {
                InventoryScript.m_instance.itemCountChangedEvent += new ItemCountChanged(objective.UpdateItemCount);
                objective.UpdateItemCount();
            }

            foreach (KillObjective killObjective in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent += new KillConfirmed(killObjective.UpdateKillCount);
            }

            MyQuests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            qs.MyQuest = quest;
            quest.MyQuestScript = qs;
            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.MyTitle;

            CheckCompletion();
        }
        
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;
            selected = quest;
            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("{0} \n <size=16>{1}</size>\n\n Hedefler\n<size=16>{2}</size>", title, quest.MyDescription, objectives);
        }
       
    }

    public void CheckCompletion()
    {
        foreach (QuestScript questScript in questScripts)
        {
            questScript.MyQuest.MyQuestGiver.UpdateQuestStatus();
            questScript.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha ==1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective co in selected.MyCollectObjectives)
        {
            InventoryScript.m_instance.itemCountChangedEvent -= new ItemCountChanged(co.UpdateItemCount);
            co.Complete();
        }
        foreach (KillObjective co in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(co.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        MyQuests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;

        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return MyQuests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
