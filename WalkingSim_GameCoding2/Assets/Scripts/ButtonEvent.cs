using System;
using TMPro;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    //action is a delegate
    //a delegate is a variable that can store a function
    //int number vs Action myFunction
    //can be used by anyone

    //static belongs to the class itself not a specific instance, meaning we dont need a reference to the specific Game Object
    //you can just += instead of FindObjectOfType
    //one shared version across the entire program

    //event is a special type of delegate
    //it is protected, if you do this without event it can break
    //other scripts can subscribe & unsubscribe, but they cannot invoke it
    public static event Action onButtonPressed;

    public void OnButtonPressed()
    {
        //invoke means call every function subscribed to this event
        // ?. means only do this if it isn't null (if someone is listening)
        onButtonPressed?.Invoke();

    }



    // E X A M P L E ! ! ! 

    //this is not SCALEABLE if we want 10 things to react when we press the button,
    //then we would have 10 references and have to keep changing the script.
    //also what happens if we delete something later? it can break our script
    //and why should button be responsible for light and text??
    /*private void PressMe()
    {
        Light light = GetComponent<Light>();
        TextMeshProUGUI statusText = GetComponent<TextMeshProUGUI>();

        light.color = Color.white;
        statusText.text = "Pressed!";

        //what if we want 10 things to react when we press the button?
        
    }
    */
}
