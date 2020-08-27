using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : Window
{
    

    [SerializeField]
    private VendorButton[] vendorButtons;

    [SerializeField]
    private Text pageNumber;

    private List<List<VendorItem>> pages = new List<List<VendorItem>>();

    private int PageIndex;

    private Vendor vendor;

    public void CreatePages(VendorItem[] vendorItems)
    {

        pages.Clear();
        List<VendorItem> page = new List<VendorItem>();
        for (int i = 0; i < vendorItems.Length; i++)
        {
            page.Add(vendorItems[i]);

            if (page.Count == 10 || i == vendorItems.Length -1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {       
        pageNumber.text = PageIndex +1 + "/" + pages.Count;

        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[PageIndex].Count; i++)
            {
                if (pages[PageIndex][i]!=null)
                {
                    vendorButtons[i].AddItem(pages[PageIndex][i]);
                }
            }

        }
    }


    public void NextPage()
    {
        if (PageIndex < pages.Count -1)
        {
            ClearButtons();
            PageIndex++;
            AddItems();
        }
    }

    public void PrevPage()
    {
        if (PageIndex >0 )
        {
            ClearButtons();
            PageIndex--;
            AddItems();
        }
    }

    public void ClearButtons()
    {
        foreach (VendorButton button in vendorButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public override void Open(NPC npc)
    {
        CreatePages((npc as Vendor).MyItems);
        base.Open(npc);
    }


}
