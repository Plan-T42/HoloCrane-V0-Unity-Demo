using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class testTargetSet : MonoBehaviour
{
    public int setHoistTarget = new Int32();
    public int setTrolleyTarget = new Int32();
    public int setBridgeTarget = new Int32();
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("set Target");
        setHoistTarget = 2454;
        setTrolleyTarget = 7890;
        setBridgeTarget = 21328;

    }
    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("TestConnection");

        go.GetComponent<testMultiThread>().hoistTarget = setHoistTarget;

        go.GetComponent<testMultiThread>().trolleyTarget = setTrolleyTarget;

        go.GetComponent<testMultiThread>().bridgeTarget = setBridgeTarget;
    }
}
