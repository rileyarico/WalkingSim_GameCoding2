using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject[] inventoryItemPrefab;
    public Item[] itemList;

    private GameObject objectToSpawn;
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
                Debug.Log("Spawned " + item + " at " + slot);
                return;
            }
        }

    }

    private void Start()
    {
        objectToSpawn = inventoryItemPrefab[0]; //CHANGE OBJECT YOU WANT TO SPAWN!! This code sucks
        //wut is this girl talking about
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(objectToSpawn, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
