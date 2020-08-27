using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{

    private static QuestGiverWindow instance;

    [SerializeField]
    private GameObject AcptBtn, BackBtn, CompleteBtn, questDescription;

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questArea;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    public static QuestGiverWindow MyInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject gameObject in quests)
        {
            Destroy(gameObject);
        }

        questDescription.SetActive(false);
        questArea.gameObject.SetActive(true);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);

                go.GetComponent<Text>().text = "["+quest.MyLevel+"] " + quest.MyTitle + "<color=#ffbb04><size=22> !</size></color>";

                go.GetComponent<QGQuestScript>().MyQuest = quest;
                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text = quest.MyTitle+"<color=#ffbb04><size=22> ?</size></color>";
                }
                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;
                    c.a = 0.5f;
                    go.GetComponent<Text>().color = c;
                    go.GetComponent<Text>().text = quest.MyTitle + "<color=#c0c0c0><size=22> ?</size></color>";
                }
            }            
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            AcptBtn.SetActive(false);
            CompleteBtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.HasQuest(quest))
        {
            AcptBtn.SetActive(true);
        }

        BackBtn.SetActive(true);
        questDescription.SetActive(true);
        questArea.gameObject.SetActive(false);

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        questDescription.GetComponent<Text>().text = string.Format("{0} \n <size=16> {1}</size>", quest.MyTitle, quest.MyDescription);
    }

    public void Back()
    {
        BackBtn.SetActive(false);
        AcptBtn.SetActive(false);
        ShowQuests(questGiver);
        CompleteBtn.SetActive(false);
    }

    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        CompleteBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }

            foreach (CollectObjective co in selectedQuest.MyCollectObjectives)
            {
                InventoryScript.m_instance.itemCountChangedEvent -= new ItemCountChanged(co.UpdateItemCount);
                co.Complete();
            }
            foreach (KillObjective co in selectedQuest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(co.UpdateKillCount);
            }

            Player.m_instance.GainXp(XpManager.CalculateXp(selectedQuest));

            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}
