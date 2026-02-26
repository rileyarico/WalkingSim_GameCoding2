using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static means a variable that belongs t the class itself rather than the specific instance of that class
    public static GameManager instance;
    
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

    // Update is called once per frame
    void Update()
    {
        
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
