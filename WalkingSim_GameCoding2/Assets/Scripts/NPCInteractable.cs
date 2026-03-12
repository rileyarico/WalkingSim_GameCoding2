using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class NPCInteractable : Interactable
{
    public bool requestDone = false;
    public NPCData npcData;

    public override void Interact(CCPlayer ccplayer)
    {
        if(npcData == null)
        {
            Debug.Log("NPC has no data: " + gameObject.name);
        }

        //if we are interacting with the NPC and it has data, then request dialogue
        ccplayer.RequestDialogue(npcData);
    }

    public void GetRequestingItem(InventoryItem item)
    {
        npcData.requestingItem = item;
        Debug.Log("Requesting item is null for this node");
        //return requestingItem;
    }

    

}
