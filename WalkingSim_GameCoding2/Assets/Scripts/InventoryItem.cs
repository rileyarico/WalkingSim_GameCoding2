using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //drag & drop
    [Header("UI")] public Image image;

    [HideInInspector] public Item item;
    [HideInInspector] public Transform parentAfterDrag;
    DialogueManager dialogueManager;
    CCPlayer player;
    NPCData npcData;

    private void Start()
    {
        InitializeItem(item);
        player = FindFirstObjectByType<CCPlayer>();
    }

    

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Tried to pick up " +
                  item); //This won't run after we have moved this item to another slot already. Meaning something wrong with PointerEventData?
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); //Remove item from this ItemSlot Parent
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag); //Change parent to the new InventorySlot
        Debug.Log("Moved " + item + " to " + parentAfterDrag); //Same problem as other Debug.Log
        
        if (player != null)
        {
            Debug.Log("Player is null");
            player.CheckRequest(item);
            Debug.Log("check request called");
            
        }
    }
    
    

    
}

