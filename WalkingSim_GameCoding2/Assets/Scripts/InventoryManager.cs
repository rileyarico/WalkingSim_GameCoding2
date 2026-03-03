using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public void AddItem(Item item)
    {
        //for every inventory slot,
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Gets the slot that we are on
            InventorySlot slot = inventorySlots[i];
            //Gets the item that is in the slot we are on
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            //if there is no item in this slot
            if(itemInSlot == null)
            {
                //spawn item in this slot
                SpawnNewItem(item, slot);
                return;
            }
        }

    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
