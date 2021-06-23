using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcalculation : MonoBehaviour
{
    
   
    private new Transform transform;
    public string objectName = "";

    public float x = new float();
    //public float SCALE = 0.161f;
    //public float scaledX = new float();
    //public float TROLLEY_OFFSET = 1.189f;
    public float trolleyTarget;

    public float y = new float();
    //public float BRIDGE_OFFSET = 27.979f;
    public float bridgeTarget;

    public float z = new float();
    //public float HOIST_OFFSET = 1.749f;
    public float hoistTarget;


    private float TROLLEY_OFFSET = 1.189f; //0.700f; //Z // 0.589f; //1.189f;
    private float BRIDGE_OFFSET = 27.979f; //23.354f; //X //27.979f;       
    private float HOIST_OFFSET = 1.749f; // 0.374f; //Y //1.749f;
    private float SCALE = -0.161f;


    // Start is called before the first frame update
    void Start()
    {
        objectName = name;

        transform = GetComponent<Transform>();

        //trolleyTarget = (-1f) * SCALE * transform.position.x + TROLLEY_OFFSET;
        //bridgeTarget = (-1f) * SCALE * transform.position.y + BRIDGE_OFFSET;
        //hoistTarget = (-1f) * SCALE * transform.position.z + HOIST_OFFSET;

        //trolleyTarget = transform.position.x + TROLLEY_OFFSET;
        //hoistTarget = transform.position.y + HOIST_OFFSET;
        //bridgeTarget = transform.position.z + BRIDGE_OFFSET;

        trolleyTarget = SCALE * transform.localPosition.x + TROLLEY_OFFSET;
        bridgeTarget = SCALE * transform.localPosition.y + BRIDGE_OFFSET;
        hoistTarget = SCALE * transform.localPosition.z + HOIST_OFFSET;


        //scaledX = SCALE * transform.position.x;
        x = transform.localPosition.x;
        y = transform.localPosition.y;
        z = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
