using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] items;
    private Chest[] chests;
    private CharButton[] equipment;
    [SerializeField]
    private ActionButtons[] actionButtons;
    [SerializeField]
    private SavedGame[] saveSlots;

    [SerializeField]
    private GameObject dialogue;
    [SerializeField]
    private Text dialogueText;

    private string action;

    private SavedGame current;
    // Start is called before the first frame update
    void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();

        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);
        }       
        
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load)")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Player.m_instance.SetDefaults();
        }
    }

    public void ShowDialog(GameObject clickButton)
    {
        action = clickButton.name;

        switch (action)
        {
            case "Load":
                dialogueText.text = "Kaydı Yükle?";
                break;
            case "Save":
                dialogueText.text = "Oyunu Kaydet?";
                break;
            case "Delete":
                dialogueText.text = "Kaydı Sil?";
                break;
        }

        current = clickButton.GetComponentInParent<SavedGame>();
        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }

        CloseDialogue();
    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.MyIndex);
            SceneManager.LoadScene(data.MyScene);
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }
    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideInfo();
    }

    private void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name+".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

            SaveData data = new SaveData();
            data.MyScene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveActionButtons(data);
            SaveQuests(data);
            SaveQuestGivers(data);

            bf.Serialize(file, data);
            file.Close();

            ShowSavedFiles(savedGame);
        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.m_instance.MyLevel,
            Player.m_instance.MyXp.MyCurrentValue, Player.m_instance.MyXp.MyMaxValue,
            Player.m_instance.MyHealth.MyCurrentValue, Player.m_instance.MyHealth.MyMaxValue,
            Player.m_instance.MyStamina.MyCurrentValue,Player.m_instance.MyStamina.MyMaxValue,
            Player.m_instance.transform.position);
    }

    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                if (chests[i].MyItems.Count >0)
                {
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyTitle, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
                }
            }
        }
    }

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.m_instance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.m_instance.MyBags[i].MySlotCount, InventoryScript.m_instance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (CharButton charButton in equipment)
        {
            if (charButton.MyEquipedArmor != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquipedArmor.MyTitle, charButton.name));
            }
        }
    }


    private void SaveActionButtons(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUsable != null)
            {
                ActionButtonData action;
                if (actionButtons[i].MyUsable is Throwables)
                {
                     action = new ActionButtonData((actionButtons[i].MyUsable as Throwables).MyTitle, false, i);
                }
                else
                {
                     action = new ActionButtonData((actionButtons[i].MyUsable as Item).MyTitle, true, i);
                }
                data.MyActionButtonData.Add(action);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.m_instance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in QuestLog.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives,quest.MyQuestGiver.MyQuestGiverID));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverID, questGiver.MyCompletedQuests));
        }
    }
    // --------------------------------------------------------LOAD-----------------------------------------
    private void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/"+ savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);
            
            file.Close();
            LoadEquipment(data);
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadChests(data);
            LoadActionButtons(data);
            LoadQuests(data);
            LoadQuestGivers(data);
        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
            SceneManager.LoadScene(0);
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.m_instance.MyLevel = data.MyPlayerData.MyLevel;
        Player.m_instance.UpdateLevel();
        Player.m_instance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.m_instance.MyStamina.Initialize(data.MyPlayerData.MyStamina, data.MyPlayerData.MyMaxStamina);
        Player.m_instance.MyXp.Initialize(data.MyPlayerData.MyExp, data.MyPlayerData.MyMaxExp);
        Player.m_instance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.MyName);

            foreach (ItemData itemData in chest.MyItems)
            {
                Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));
                item.MySlot = c.MyBag.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);
                c.MyItems.Add(item);
            }
        }
    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            Bag newBag = (Bag)Instantiate(items[0]);
            newBag.Initialize(bagData.MySlotCount);
            InventoryScript.m_instance.AddBag(newBag, bagData.MyBagIndx);
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharButton cb = Array.Find(equipment, x => x.name == equipmentData.MyType);
            cb.EquipArmor(Array.Find(items, x => x.MyTitle == equipmentData.MyTitle) as Armor);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.MyActionButtonData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.MyIndex].SetUsable(InventoryScript.m_instance.GetUseable(buttonData.MyAction));
            }
            else
            {
                actionButtons[buttonData.MyIndex].SetUsable(SkillBook.m_instance.GetThrowable(buttonData.MyAction));
            }
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.m_instance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestData questData in data.MyQuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.MyQuestGiverID == questData.MyQuestGiverID);
            Quest q = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyTitle);
            q.MyQuestGiver = qg;
            q.MyKillObjectives = questData.MyKillObjective;
            QuestLog.MyInstance.AcceptQuest(q);
        }
    }

    private void LoadQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.MyQuestGiverID == questGiverData.MyQuestGiverID);
            questGiver.MyCompletedQuests = questGiverData.MyCompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }

}
