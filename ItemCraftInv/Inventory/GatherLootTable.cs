using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherLootTable : LootTable, IInteractible
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private Sprite getherSprite;

    [SerializeField]
    private GameObject minimapIndicator;
    private void Start()
    {
        RollLoot();
    }
    protected override void RollLoot()
    {
        MyDroppedItems = new List<Drop>();

        foreach (Loot lt in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= lt.MyDropChance)
            {
                int itemCount = Random.Range(1, 6);

                for (int i = 0; i < itemCount; i++)
                {
                    MyDroppedItems.Add(new Drop(Instantiate(lt.MyItem), this));
                }

                spriteRenderer.sprite = getherSprite;
                minimapIndicator.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        Player.m_instance.Gather(SkillBook.m_instance.GetThrowable("Toplayıcılık"), MyDroppedItems);
        LootWindow.MyInstance.MyInteractible = this;
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.MyInteractible = null;

        if (MyDroppedItems.Count == 0)
        {
            spriteRenderer.sprite = defaultSprite;
            gameObject.SetActive(false);
            minimapIndicator.SetActive(false);
        }

        LootWindow.MyInstance.Close();
    }
}
