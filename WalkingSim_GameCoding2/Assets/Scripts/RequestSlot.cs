using UnityEngine;
using UnityEngine.EventSystems;

public class RequestSlot : MonoBehaviour, IDropHandler
{
    public InventoryItem requestedItem;
    public InventoryItem heldObject;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform == requestedItem)
        {
            Debug.Log("Trying to assign InventoryItem as a parent of Request Slot");
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            heldObject = inventoryItem;
        }
    }

    private void Update()
    {
        //Debug.Log("Child of Request slot is " +  heldObject);
        heldObject = this.GetComponentInChildren<InventoryItem>();

        if (heldObject != null && requestedItem !=null)
        {
            if (heldObject == requestedItem)
            {
                Debug.Log("given item matches request!"); //i know this works because it would run when both were null
                //DestroyChild();
            }
        }
    }

    //called outside this script, checks if held item is the one that they are looking for
    public bool CheckItem(InventoryItem item)
    {
        Debug.Log("CheckItem was called");
        //if they are equal, tell that to the caller and destroy the held object.
        if(heldObject == item)
        {
            Debug.Log("Given item matches request! Destroying InventoryItem & moving to next node");
            Destroy(heldObject);
            return true;
        }
        return false;
    }

    public void SetRequest(InventoryItem item)
    {
        Debug.Log("Took request");
        requestedItem = item;
    }

    public void DestroyChild()
    {
        //Destroy(heldObject.gameObject);
        heldObject = null;
    }
}
