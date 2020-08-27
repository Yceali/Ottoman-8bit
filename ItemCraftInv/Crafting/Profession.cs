using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profession : MonoBehaviour
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private ItemInfo itemInfo;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private Text countText;

    private int maxAmount, amount;

    private int MyAmount
    {
        get
        {
            return amount;
        }
        set
        {
            countText.text = value.ToString();
            amount = value;
        }
    }

    private List<int> amounts = new List<int>();

    private void Start()
    {
        InventoryScript.m_instance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);
    }
    // Start is called before the first frame update
    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.DeSelect();
        }
        this.selectedRecipe = recipe;
        this.selectedRecipe.Select();

        foreach (GameObject gameObject in materials)
        {
            Destroy(gameObject);
        }
        materials.Clear();

        title.text = recipe.Output.MyTitle;
        description.text = recipe.MyDescription + " " +recipe.Output.MyTitle;
        itemInfo.Initialize(recipe.Output, 1);

        foreach (CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);
            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);
            materials.Add(tmp);
        }

        UpdateMaterialCount(null);
    }


    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }

        if (CanCraft())
        {
            maxAmount = amounts[0];

            if (countText.text == "0")
            {
                MyAmount = 1;
            }
            else if (int.Parse(countText.text) > maxAmount)
            {
                MyAmount = maxAmount;
            }
        }
        else
        {
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool all)
    {        
        if (CanCraft() && !Player.m_instance.IsAttacking)
        {
            if (all)
            {
                amounts.Sort();
                countText.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                StartCoroutine(CraftRoutine(MyAmount));
            }
        }
    }

    private bool CanCraft()
    {
        bool canCraft = true;
        amounts = new List<int>();

        foreach (CraftingMaterial material in selectedRecipe.Materials)
        {
            int count = InventoryScript.m_instance.GetItemCount(material.MyItem.MyTitle);

            if (count>= material.MyCount)
            {
                amounts.Add(count/material.MyCount);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }          
        }
        return canCraft;
    }

    public void ChangeAmount(int i)
    {
        if((amount + i)> 0 && amount + i <= maxAmount)
        {
            MyAmount += i;
        }
    }

    private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Player.m_instance.MyInitRoutine = StartCoroutine(Player.m_instance.CraftRoutine(selectedRecipe));
        }
    }

    public void AddItemsToInventory()
    {
        if (InventoryScript.m_instance.AddItem(itemInfo.MyItem))
        {
            foreach (CraftingMaterial material in selectedRecipe.Materials)
            {
                for (int i = 0; i < material.MyCount; i++)
                {
                    InventoryScript.m_instance.RemoveItem(material.MyItem);
                }
            }
        }
        InventoryScript.m_instance.AddItem(itemInfo.MyItem);
    }
}
