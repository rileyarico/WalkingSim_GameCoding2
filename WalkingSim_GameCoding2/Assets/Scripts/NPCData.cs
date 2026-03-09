using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Data")]

public class NPCData : ScriptableObject
{
    [Header("Speaker")]
    public string displayName;

    [Header("Dialogue")]
    [TextArea(3,10)]
    public string[] lines;

    [Header("If there are no choices we show buttons after line ends")]
    public DialogueChoice[] dialogueChoices;

    [Header("If no choices, auto continue to this next node")]
    public NPCData nextNode;

    [Header("If requesting an item call this function and ask for this item.")]
    private RequestManager requestM;
    public InventoryItem requestingItem;

    private RequestManager req;

    private void Awake()
    {
        Debug.Log("Running Start from NPCData");
        requestM = FindFirstObjectByType<RequestManager>();
        Debug.Log("Checking for request");
        if (requestM != null)
        {
            req = requestM.GetComponent<RequestManager>();
        }
        if(requestingItem != null && req == null)
        {
            Debug.Log("Req manager not linked on " + this);
        }
        if (requestM != null && requestingItem != null)
        {
            Debug.Log("Both RequestManager & Requesting item found");
            req.MakeActive();
            Debug.Log("Called MakeActive() in RequestSlot");
        }
    }
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public NPCData nextNode;

}
