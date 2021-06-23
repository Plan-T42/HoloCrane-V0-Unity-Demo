using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;

public class TestControl : MonoBehaviour
{
    RestClient client = new RestClient("http://192.168.0.77");
    public int accessCode = 19952011;
    public int watchdogNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start testControl");
        client.Timeout = 300;
    }

    // Update is called every 100ms
    private void FixedUpdate()
    {
        // change watchdog num 
        watchdogNum = (watchdogNum % 30000) + 1;

        string querySetValues = $@"mutation {{
					AccessCode: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.AccessCode"", value: {accessCode}, dataType: ""Int32"")  {{ok}}
					Watchdog: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Watchdog"", value: {watchdogNum}, dataType: ""Int16"")  {{ok}}
				}}";

        var request = new RestRequest("graphql");
        request.AddParameter("application/graphql", querySetValues, ParameterType.RequestBody);

        var response = client.Post(request);
        var content = response.Content;
        var statusCode = response.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
        var errorMessage = response.ErrorMessage; // error 
        int numericStatusCode = (int)statusCode;

        if (numericStatusCode == 200 || numericStatusCode == 307)
            Debug.Log("Watchdog request content: " + content + ", status: " + statusCode + " , status Code: " + numericStatusCode);
        else
            Debug.Log("Watchdog ERROR! status Code: " + numericStatusCode + ", error message: " + errorMessage);


        // Test hoist up with speed 20
        float speed = 20f;
        string movePart = "Hoist";
        string direction = "Up";
        string directionOpposite = "Down";

        //   string moveQuery = $@"mutation {{
        //	directionOpposite: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.{directionOpposite}"", value: ""false"", dataType: ""Boolean"")  {{ok}}
        //	direction: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.{direction}"", value: ""true"", dataType: ""Boolean"")  {{ok}}
        //	speed: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.speed"", value: {speed}, dataType: ""Float"")  {{ok}}		
        //}}";

        string moveQuery = $@"mutation {{
						directionOpposite: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Hoist.Down"", value: {true}, dataType: ""Boolean"")  {{ok}}
						direction: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Hoist.Up"", value: {false}, dataType: ""Boolean"")  {{ok}}
						speed: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Hoist.speed"", value: {20f}, dataType: ""Float"")  {{ok}}		
					}}";

        var moveRequest = new RestRequest("graphql");
        moveRequest.AddParameter("application/graphql", moveQuery, ParameterType.RequestBody);

        var moveResponse = client.Post(moveRequest);
        var moveContent = moveResponse.Content;
        var moveStatusCode = moveResponse.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
        var moveErrorMessage = moveResponse.ErrorMessage;
        int moveNumericStatusCode = (int)moveStatusCode;

        if (moveNumericStatusCode == 200 || moveNumericStatusCode == 307)
        {
            Debug.Log("moveCrane request content: " + moveContent + ", status: " + moveStatusCode + " , status Code: " + moveNumericStatusCode);
        }
        else { Debug.Log("moveCrane ERROR! status Code: " + moveStatusCode + ", " + moveNumericStatusCode + ", error message: " + moveErrorMessage); }

    }

}

