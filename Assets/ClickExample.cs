using UnityEngine;

using HoloToolkit.Unity.InputModule;

public class ClickExample : MonoBehaviour, IInputClickHandler
{

    private Color[] colors = new Color[] { Color.red, Color.cyan};
    private int i = 0;
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    //here we learn that this game object is clicked
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("clicked");
        //here we change the color of the cube
        rend.material.color = colors[i];
        //index is calculated with mode operation so that
        //it stays in the size of the array
        i = (i + 1) % colors.Length;
    }
}