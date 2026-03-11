using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;

    [Header("Choices")]
    public Transform choicesContainer; //parent object where choice buttons will spawn
    public Button choiceButtonPrefab; //prefab for a single choice button

    [Header("Request")]
    public GameObject requestPanel; //RequestSlot is a child of this.
    public TextMeshProUGUI requestText;

    private NPCData currentNode; //current node we are reading from the scriptable object (SO)
    private int lineIndex; //which line index we are currently on, keeping track of the dialogue
    private bool isActive; //are we currently in dialogue?
    

    //lock the player movement & camera
    private CCPlayer player;

    private void Awake()
    {
        //start with dialogue hidden
        if(dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        ClearChoices();
        //find our player
        player = FindFirstObjectByType<CCPlayer>();

        
       
        /*if(currentNode.requestingItem != null)
        {
            //find request slot
            InventorySlot reqSlot = requestPanel.GetComponentInChildren<InventorySlot>();
            //grab the requestSlot script
            RequestSlot yes = reqSlot.GetComponent<RequestSlot>();
            yes.SetRequest(currentNode.requestingItem);
        } */
    }
    
    
    private void OnEnable()
    {
        CCPlayer.OnDialogueRequested += StartDialogue;
        CCPlayer.OnCheckItem += CheckRequest;

    }

    private void OnDisable()
    {
        CCPlayer.OnDialogueRequested -= StartDialogue;
        CCPlayer.OnCheckItem -= CheckRequest;
        
    }

    private void Update()
    {
        if(!isActive)
        {
            return; //if no dialogue is active, ignore
        }
                        //uses Z key
        if(Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame) //doesn't give an error despite us using player input in this project!
        {
            if (ChoicesAreShowing()) //blocks only when button exists
            {
                return;
            }
            if(true) //if we are supposed to give NPC an object, can't advance.
            {

            }
            Advance();
        }

        if(!isActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
        if(isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

       
        //keyboard.current.qKey
    }

    void StartDialogue(NPCData npcData)
    {
        if(npcData == null)
        {
            Debug.Log("NPC Data is NULL");
            return;
        }
        
        //this is where  we would lock player camera & movement

        //set state
        currentNode = (NPCData)npcData;
        lineIndex = 0;
        isActive = true;

        if(dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        if(currentNode.requestCompleted)
        {
            currentNode = currentNode.requestComplete;
        }

        ShowLine();
    }

    bool HasChoices(NPCData node) //check the data
    {
        return node != null && node.dialogueChoices != null && node.dialogueChoices.Length > 0;
    }

    void Advance()
    {
        //if node is finished, end dialogue
        if(currentNode == null)
        {
            EndDialogue();
            return;
        }

        //move to next line
        lineIndex++;

        //if we still have lines to read in this node, show the next one
        if(currentNode.lines != null && lineIndex < currentNode.lines.Length)
        {
            //if we have something
            if(lineText != null)
            {
                //takes the text of our TMPro Obj & changes it to whatever the current line is (dependent on the lineIndex)
                lineText.text = currentNode.lines[lineIndex];
                return;
            }
        }

        //Otherwise we have reached the end
        FinishNode();
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        ClearChoices();
        if(choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.Log("Choices are not wired!");
            return;
        }

        foreach (DialogueChoice choice in choices)
        {
            //spawn the button as a child of the container
            Button bttn = Instantiate(choiceButtonPrefab, choicesContainer);

            //set visible button text
            TextMeshProUGUI tmp = bttn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = choice.choiceText;

                //cache nexxt node in a local variable
                NPCData next = choice.nextNode;

                //LAMBDA!! 
                //this is like onClick(), on our buttons
                //we are saying add a listener when the button is clicked, run this function
                bttn.onClick.AddListener(() =>
                {
                    Choose(next);
                });
            }

        }

    }

    void ShowRequest(InventoryItem requestingItem)
    {
        Debug.Log("attempting to make RequestPanel visible");
        //we need to make request panel active
        requestPanel.SetActive(true);
        Debug.Log("Made Request panel active");
        requestText.text = "Drag item to give.";
        Debug.Log("Made request text active");
    }

    public void HideRequest()
    {
        requestPanel.SetActive(false);
        requestText.text = "";
    }

    void FinishNode()
    {
        //1. if choice exists, show choices
        //2. else if, next node exists, continue automatically
        //3. else, end dialogue

        //if choices exist, show them
        if(HasChoices(currentNode))
        {
            ShowChoices(currentNode.dialogueChoices);
            return;
        }

        //if NPC is requesting an item, show the request slot
        if (currentNode.requestingItem != null)
        {
            //Debug.Log("calling ShowRequest()");
            ShowRequest(currentNode.requestingItem);
            return;
        }

        //auto continue our text

        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            ShowLine();
            return;
        }

        //end
        EndDialogue();
    }

    void ShowLine()
    {
        //when showing a line, we shouldn't be showing choices
        ClearChoices();
        //if no node, end dialogue
        if(currentNode == null)
        {
            EndDialogue();
            return;
        }

        /*if(currentNode.requestCompleted)
        {
            Choose(currentNode.requestComplete);
        }*/

        //update the speaker name
        if(displayName != null)
        {
            displayName.text = currentNode.displayName;
        }

        if(currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }

        //clamp index so we never go out of bounds
        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);
        //show line text
        if(lineText != null)
        {
            lineText.text = currentNode.lines[lineIndex];
        }
    }

    void Choose(NPCData nextNode)
    {
        //remove buttons ASAP!! so UI feels responsive
        ClearChoices();

        //if no next node, this choice ends the convo
        if(nextNode == null)
        {
            EndDialogue();
            return;
        }

        //otherwise, go to the chosen node
        currentNode = nextNode;
        lineIndex = 0;

        ShowLine();
        
    }

    //are the choices displaying in the UI? Do we see them on the screen?
    bool ChoicesAreShowing()
    {
        return choicesContainer != null & choicesContainer.childCount > 0;
        
        /*bool showing = choicesContainer != null & choicesContainer.childCount > 0;
        Debug.Log(showing);
        return; */
    }

    void ClearChoices()
    {   
        //if we don't have choice container, exit the function
        if(choicesContainer == null)
        {
            return;
        }

        for(int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            //for every child of the choice container, which is a button (UI Panel with Button children), subtract until we clear them all
            Destroy(choicesContainer.GetChild(i).gameObject);
        }

    }

    public void EndDialogue()
    {
        //lock player camera
        isActive = false; //no longer in dialogue
        currentNode = null; //we don't have a node next (SO)
        lineIndex = 0;

        ClearChoices();

        //turn off dialogue panel if we have which we should if this is running
        if(dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        //turn of request panel
        //requestPanel.SetActive(false);
        //requestText.text = "";
    }

    void CheckRequest(Item droppedItem)
    {
        if (currentNode == null) return;
        if (currentNode.requestingItem == null) return; // NPC wants nothing
    
        Debug.Log("requesting item name: " + currentNode.requestingItem.name);
        Debug.Log("dropped item name: " + droppedItem.name);
        if (droppedItem.name == currentNode.requestingItem.name)
        {
            Debug.Log("Correct item given!");
            // advance dialogue, remove from inventory, etc.
            //moving to next node if exists
            NPCData next = currentNode.nextNode;
            Choose(next);

            //destroy item
            Debug.Log("Destroying dropped item");
            InventoryItem getRequestSlotHeld = FindAnyObjectByType<RequestSlot>().GetComponentInChildren<InventoryItem>();
            Destroy(getRequestSlotHeld.gameObject);

            //Set request complete BUT WE NEED TO FIND ORIGINAL NODE, NOT THIS ONE BC THIS IS SETTING TRUE TO THE NODE ASKING FOR ITEM
            //NPCData originalLine =

            //currentNode.requestCompleted = true;
            //Debug.Log("requestCompleted = " currentNode.requestComplete.ToString());
            //set panel innactive
            HideRequest();

        }
        else
        {
            Debug.Log("Wrong item.");
        }
        
    }

}
