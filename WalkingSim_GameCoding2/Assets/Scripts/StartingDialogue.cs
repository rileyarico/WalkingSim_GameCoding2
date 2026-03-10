using UnityEngine;

public class StartingDialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public NPCData start;
    void Start()
    {
        CCPlayer player = FindAnyObjectByType<CCPlayer>();
        player.RequestDialogue(start);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
