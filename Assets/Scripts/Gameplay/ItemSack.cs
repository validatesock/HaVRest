﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSack : MonoBehaviour
{
    [SerializeField]
    private List<ItemSlotInteraction> mItemInteractions;
    public List<ItemSlotInteraction> Items
    {
        get { return mItemInteractions; }
        set { mItemInteractions = value; }
    }

    [SerializeField]
    private GameObject mSackTop;
    public GameObject SackTop
    { 
        get { return mSackTop; }
        set { mSackTop = value; }
    }
}
