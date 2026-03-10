using UnityEngine;

public class DestroyInteractable : Interactable
{
    public Item item;
    public override void Interact(CCPlayer ccplayer)
    {
        Destroy(gameObject);

        Debug.Log("Destroyed " + gameObject.name);

        if(item != null)
        {
            InventoryManager inventoryM = FindAnyObjectByType<InventoryManager>();
            inventoryM.AddItem(item);
        }
    }
}
