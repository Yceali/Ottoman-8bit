using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag",order =1)]
public class Bag : Item ,IUsable
{
    [SerializeField]
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }

    public BagButton MyBagButton { get; set; }
    public int MySlotCount { get => slots;}

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScript.m_instance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.m_instance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            if(MyBagButton == null)
            {
                InventoryScript.m_instance.AddBag(this);
            }
            else
            {
                InventoryScript.m_instance.AddBag(this,MyBagButton);
            }

            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
           
        }
    }

    public void SetUpScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.m_instance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} Gözlü sırt çantası ", slots);
    }
}
