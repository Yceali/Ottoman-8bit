using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : NPC , IInteractible
{
    [SerializeField]
    private VendorItem[] items;

    public VendorItem[] MyItems { get => items; }
}
