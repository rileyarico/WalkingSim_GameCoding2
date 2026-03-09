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
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            heldObject = inventoryItem;
        }
    }

    public void ChangeRequest(string newRequest)
    {
        nameOfRequestedObject = newRequest;
    }

    public void DestroyChild()
    {
        Destroy(heldObject.gameObject);
        heldObject = null;
    }
}
