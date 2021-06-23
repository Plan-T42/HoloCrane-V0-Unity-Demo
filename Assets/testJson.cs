using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Json;
using System;


public class testJson : MonoBehaviour
{
    // Start is called before the first frame update

    public int hoistPos = new Int32();
    public int trolleyPos = new Int32();
    public bool change = false;
    void Start()
    {
        string jsonString = "{ \"data\":{ \"Hoist\":{ \"variable\":{ \"value\":2895} }, \"Trolley\":{ \"variable\":{ \"value\":3000} } }}";
        //string jsonString = "{\"a\": 20,\"b\": \"string value\",\"c\":[{\"Value\": 1}, {\"Value\": 2,\"SubObject\":[{\"SubValue\":3}]}]}";

        // Verify your JSON if you get any errors here
        JsonValue json = JsonValue.Parse(jsonString);
        hoistPos = json["data"]["Hoist"]["variable"]["value"];
        trolleyPos = json["data"]["Trolley"]["variable"]["value"];
        Debug.Log("json[\"hoistPos\"]" + " = " + hoistPos + " ,json[\"trolleyPos\"]" + " = " + trolleyPos);

        //// int test
        //if (json.ContainsKey("a"))
        //{
        //    int a = json["a"]; // type already set to int
        //    Debug.Log("json[\"a\"]" + " = " + a);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if (change == true)
            hoistPos = 10;
    }
}
