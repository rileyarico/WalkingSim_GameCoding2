using UnityEngine;

public class LightListen : MonoBehaviour
{
    //listener is like our Twitter followers
    public Light sceneLight;

    //when this objects become active, it is saying when your event fires, call this light function
    public void OnEnable()
    {
        ButtonEvent.onButtonPressed += ChangeLight;
    }


    //when disabled, we remove ourselves from the list
    public void OnDisable()
    {
        ButtonEvent.onButtonPressed -= ChangeLight;
    }

    void ChangeLight()
    {
        sceneLight.color = Random.ColorHSV();
    }
}
