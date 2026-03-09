using UnityEngine;
using UnityEngine.EventSystems;

public class RequestSlot : MonoBehaviour, IDropHandler
{
    public string nameOfRequestedObject;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.gameObject.name == nameOfRequestedObject)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
