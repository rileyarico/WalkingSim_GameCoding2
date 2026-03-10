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
    public InventoryItem requestingItem;

    private void Awake()
    {
        
    }
    
    
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public NPCData nextNode;

}
