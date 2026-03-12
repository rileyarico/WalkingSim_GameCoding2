using TMPro;
using UnityEngine;

public class FirstGhost : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public NPCData startingDialogue;
    public GameObject firstGhost;
    private DialogueManager diaM;
    private GameManager gameManager;
    private CCPlayer player;
    private bool triggered = false;
    private GameObject[] objectsWithTag = null;
    public TextMeshProUGUI requestsLeftText;

    void Start()
    {
        diaM = FindAnyObjectByType<DialogueManager>();
        player = FindAnyObjectByType<CCPlayer>();
        gameManager = FindAnyObjectByType<GameManager>();
        //set all npcs innactive
        objectsWithTag = GameObject.FindGameObjectsWithTag("NPC");
        for(int i = 0; i < objectsWithTag.Length; i++)
        {
            objectsWithTag[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && diaM.isActive == false)
        {
            //set active to all NPCs'
            for (int i = 0; i < objectsWithTag.Length; i++)
            {
                objectsWithTag[i].SetActive(true);
            }
            //we need to turn request visibility off before dialogue with firstghost
            gameManager.requestTotal = objectsWithTag.Length;
            //& destroy this.
            Destroy(firstGhost.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player.RequestDialogue(startingDialogue);
        triggered = true;
    }
}
