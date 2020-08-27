using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    [SerializeField]
    private int level;
    [SerializeField]
    private int xp;

    public bool IsComplete
    {
        get
        {
            foreach (Objective objective in collectObjectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective objective in MyKillObjectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }


    public int MyLevel { get => level; set => level = value; }
    public int MyXp { get => xp;}

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount { get => amount; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type; }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.m_instance.GetItemCount(item.MyTitle);
            if (MyCurrentAmount <= MyAmount)
            {                
                MessageFeedMenager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }           

            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {        
        MyCurrentAmount = InventoryScript.m_instance.GetItemCount(MyType);

        QuestLog.MyInstance.CheckCompletion();
        QuestLog.MyInstance.UpdateSelected();        
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.m_instance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(CharacterScript character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;
                MessageFeedMenager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType, MyCurrentAmount, MyAmount));

                QuestLog.MyInstance.CheckCompletion();
                QuestLog.MyInstance.UpdateSelected();
            }

        }
    }
}
