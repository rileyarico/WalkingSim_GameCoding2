using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static means a variable that belongs t the class itself rather than the specific instance of that class
    public static GameManager instance;
    public TextMeshProUGUI requestsLeftText;

    [HideInInspector] public int requestTotal;
    public string nextScene;
    private DialogueManager diaM;
    [HideInInspector] public bool showRequestsLeft;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void aAwake()
    {
        //if we don't have a game manager in next scene, then don't destroy this Game Manager
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //Destroy game manager if there is one in the next scene
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("NPC");
        requestTotal = objectsWithTag.Length;
        Debug.Log("Objects with NPC tag: " + requestTotal);
        diaM = FindAnyObjectByType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (requestTotal > 0 && showRequestsLeft)
        {
            requestsLeftText.text = "Requests left: " + (requestTotal - diaM.requestsDone);
        }
        else
        {
            requestsLeftText.text = "";
        }

        if (requestTotal == diaM.requestsDone)
        {
            Debug.Log("Total requests = requests done");
            //waiting for dialogue to be innactive
            if (diaM.isActive == false)
            {
                Debug.Log("Dialogue finished. Attempting to load next scene");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene(nextScene);
            }
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //if we click the button, we load the scene that we are in
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Reloaded");
        }
    }

}
