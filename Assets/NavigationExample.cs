//This code is adopted from Holograms 211 course https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_211 
//Originally part of this code is in GestureAction.cs inside this course.

using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class NavigationExample : MonoBehaviour, INavigationHandler
{

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;

    private float rotationFactor;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        Debug.Log("Navigation is started");
    }
    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        rotationFactor = eventData.CumulativeDelta.x * RotationSensitivity;

        //transform.Rotate along the Y axis using rotationFactor.
        transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
    }
    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        Debug.Log("Navigation is completed");
    }
    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        Debug.Log("Navigation is canceled");
    }
}