using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;



    //drag & drop
    [Header("UI")]
    public Image image;


    [HideInInspector] public Transform parentAfterDrag;

    private void Start()
    {
        InitializeItem(item);
    }
    public void InitializeItem(Item newItem)
    {
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
        image.raycastTarget = false;
        transform.SetParent(parentAfterDrag); //Change parent to the new InventorySlot
    }
}
