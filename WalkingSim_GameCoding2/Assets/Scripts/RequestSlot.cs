using UnityEngine;
using UnityEngine.EventSystems;

public class RequestSlot : MonoBehaviour, IDropHandler
{
    public string nameOfRequestedObject;
    public InventoryItem heldObject = null;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.gameObject.name == nameOfRequestedObject)
        {
            Debug.Log("Trying to assign InventoryItem as a parent of Request Slot");
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            heldObject = inventoryItem;
        }
    }

    private void Update()
    {
        heldObject = this.GetComponentInChildren<InventoryItem>();
    }

    public bool CheckItem(InventoryItem item)
    {
        if(heldObject == item)
        {
            return true;
        }
        return false;
    }

    public void GiveRequest(string newRequest)
    {
        nameOfRequestedObject = newRequest;
    }

    public void DestroyChild()
    {
        Destroy(heldObject.gameObject);
        heldObject = null;
    }
}
