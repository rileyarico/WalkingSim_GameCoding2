using TMPro;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject panel;
    public TextMeshProUGUI text;
    void Start()
    {
        panel.SetActive(false);
        text.text = "";
    }

    public void MakeActive()
    {
        panel.SetActive(true);
        text.text = "Drag item to give.";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
