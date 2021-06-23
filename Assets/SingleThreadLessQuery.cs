using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using RestSharp;
using System.Json;

//using System.Threading;
//using System.Threading.Tasks;

public class SingleThreadLessQuery : MonoBehaviour
{

	// Start is called before the first frame update
	RestClient client = new RestClient("http://192.168.0.77");

	public float fixedDeltaTime = 0.1f;
	public int timeout = 1000;

	public string watchdogQueryLog = "watchdogQuery";
	public string monitorQueryLog = "monitorQuery";
	public string controlQueryLog = "controlQuery";

	public bool startWatchdog = false;
	public int accessCode = 19952011;
	public int setWatchdog = 1;
	public int watchdog = 0;
	public bool WatchdogFault = true;

	public bool startMonitor = false;
	public int checkMonitorUpdate = 1;

	public float hoistPos = new float();
	public float trolleyPos = new float();
	public float bridgePos = new float();
	public float load = new float();
	//public bool Inching = new bool();
	//public bool MicroSpeed = new bool();
	//public bool SwayControl = new bool();
	//public bool RopeAngleFeaturesBypass = new bool();

	public bool startControl = false;
	public int checkControlUpdate = 1;
	public float speedLimit = 20f;

	public float hoistTarget = new float();
	public float trolleyTarget = new float();
	public float bridgeTarget = new float();

	public float differenceHoist = new float();
	public float differenceTrolley = new float();
	public float differenceBridge = new float();

	public string controlHoist = "stop";
	public string controlTrolley = "stop";
	public string controlBridge = "stop";


	//public bool startTargetPositioning = false;
	//public bool selectionInUse = false;
	//public int checkTargetPositioningUpdate = 1;
	//public short target = new short();
	//public bool atTarget = new bool();
	//public bool positioningToTarget = new bool();
	//public short targetNbrSelected = new short();


	void StopCrane(string stopPart)
	{
		Debug.Log("stop " + stopPart);

		string[] stopList;

		if (stopPart == "Hoist")
			stopList = new[] { "Up", "Down" };
		else
			stopList = new[] { "Forward", "Backward" };

		// set the movement in corresponding dimension as false and its speed as 0
		string stopQuery = $@"mutation {{
						{stopList[0]}: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{stopPart}.{stopList[0]}"", value: {false}, dataType: ""Boolean"")  {{ok}}
						{stopList[1]}: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{stopPart}.{stopList[1]}"", value: {false}, dataType: ""Boolean"")  {{ok}}
						SpeedZero: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{stopPart}.speed"", value: {0f}, dataType: ""Float"")  {{ok}}		
					}}";

		var stopRequest = new RestRequest("graphql");
		stopRequest.AddParameter("application/graphql", stopQuery, ParameterType.RequestBody);

		var stopResponse = client.Post(stopRequest);
		var stopContent = stopResponse.Content;
		var stopStatusCode = stopResponse.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
		var stopErrorMessage = stopResponse.ErrorMessage;
		int stopNumericStatusCode = (int)stopStatusCode;
		//JsonValue stopRes = JsonValue.Parse(stopContent);

		if (stopNumericStatusCode == 200 || stopNumericStatusCode == 307)
		{
			Debug.Log(stopContent + ", status: " + stopStatusCode + " , status Code: " + stopNumericStatusCode);
			controlQueryLog = stopContent + ", status: " + stopStatusCode + " , status Code: " + stopNumericStatusCode;
		}
		else
		{
			Debug.Log("stopCrane ERROR! status Code: " + stopStatusCode + ", " + stopNumericStatusCode + ", error message: " + stopErrorMessage);
			controlQueryLog = "stopCrane ERROR! status Code: " + stopStatusCode + ", " + stopNumericStatusCode + ", error message: " + stopErrorMessage;
		}
	}

	void MoveCrane(string direction, string movePart, float speed)
	{
		speed = Math.Max(speed, speedLimit);
		Debug.Log(movePart + " " + direction + " with speed " + speed);
		string directionOpposite = null;

		if (direction == "Forward")
			directionOpposite = "Backward";
		else if (direction == "Backward")
			directionOpposite = "Forward";
		else if (direction == "Up")
			directionOpposite = "Down";
		else if (direction == "Down")
			directionOpposite = "Up";

		// Debug.Log("directionOpposite:" + directionOpposite);


		// set opposite direction movement as false, direction movement as true, and its speed as calculated speed
		string moveQuery = $@"mutation {{
						{directionOpposite}: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.{directionOpposite}"", value: {false}, dataType: ""Boolean"")  {{ok}}
						{direction}: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.{direction}"", value: {true}, dataType: ""Boolean"")  {{ok}}
						speed: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.{movePart}.speed"", value: {speed}, dataType: ""Float"")  {{ok}}		
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
			Debug.Log(moveContent + ", status: " + moveStatusCode + " , status Code: " + moveNumericStatusCode);
			controlQueryLog = moveContent + ", status: " + moveStatusCode + " , status Code: " + moveNumericStatusCode;
		}
		else
		{
			Debug.Log("moveCrane ERROR! status Code: " + moveStatusCode + ", " + moveNumericStatusCode + ", error message: " + moveErrorMessage);
			controlQueryLog = "moveCrane ERROR! status Code: " + moveStatusCode + ", " + moveNumericStatusCode + ", error message: " + moveErrorMessage;
		}

	}

	void Watchdog()
	{
		setWatchdog = (setWatchdog % 30000) + 1;

		//Debug.Log("watchdog incremented: " + watchdogNum);

		string querySetValues = $@"mutation {{
					AccessCode: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.AccessCode"", value: {accessCode}, dataType: ""Int32"")  {{ok}}
					Watchdog: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Watchdog"", value: {setWatchdog}, dataType: ""Int16"")  {{ok}}
					DisableTargePositioning: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.TargetPositioning.DriveToTarget."", value: {false}, dataType: ""Boolean"")  {{ok}}
				}}";


		var request = new RestRequest("graphql");
		request.AddParameter("application/graphql", querySetValues, ParameterType.RequestBody);

		var response = client.Post(request);
		var content = response.Content;
		var statusCode = response.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
		var errorMessage = response.ErrorMessage; // error 
		int numericStatusCode = (int)statusCode;

		if (numericStatusCode == 200 || numericStatusCode == 307)
		{
			Debug.Log("Watchdog request content: " + content + ", status: " + statusCode + " , status Code: " + numericStatusCode);
			watchdogQueryLog = content + ", status: " + statusCode + " , status Code: " + numericStatusCode;
		}

		else
		{
			Debug.Log("Watchdog ERROR! status Code: " + statusCode + ", " + numericStatusCode + ", error message: " + errorMessage);
			watchdogQueryLog = "Watchdog ERROR! status Code: " + statusCode + ", " + numericStatusCode + ", error message: " + errorMessage;
		}



		//TODO: check status, translate the checkStatus function to make the code more robust

		//Thread.Sleep(100); //millisecond -> 0.1s in time.sleep() for python

		//startWatchdog = false;
	}

	void Monitor()
	{
		checkMonitorUpdate = (checkMonitorUpdate % 30000) + 1;

		string query = $@"query {{
					Hoist: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Status.Hoist.Position.Position_m""){{variable{{value}}}}
					Trolley: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Status.Trolley.Position.Position_m""){{variable{{value}}}}
					Bridge: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Status.Bridge.Position.Position_m""){{variable{{value}}}}
					Load: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Status.Hoist.Load.Load_t""){{variable{{value}}}}
					WatchDogFault: 	node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Status.WatchDogFault""){{variable{{value}}}}
					Watchdog: node(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.Watchdog""){{variable{{value}}}}
				}}";

	//Inching: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.RadioSelection.Inching""){ { variable{ { value} } } }
	//MicroSpeed: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.RadioSelection.MicroSpeed""){ { variable{ { value} } } }
	//SwayControl: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.RadioSelection.SwayControl""){ { variable{ { value} } } }
	//RopeAngleFeaturesBypass: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.RadioSelection.RopeAngleFeaturesBypass""){ { variable{ { value} } } }

		//AtTarget: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.Status.TargetPositioning.AtTarget""){ { variable{ { value} } } }
		//TargetNbrSelected: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.Status.TargetPositioning.TargetNbr_Selected""){ { variable{ { value} } } }
		//PositioningToTarget: node(server: ""Ilmatar"", nodeId: ""ns = 7; s = SCF.PLC.DX_Custom_V.Status.TargetPositioning.PositioningToTarget""){ { variable{ { value} } } }

		var request = new RestRequest("graphql");
		request.AddParameter("application/graphql", query, ParameterType.RequestBody);

		var response = client.Post(request);
		var content = response.Content;
		var statusCode = response.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
		var errorMessage = response.ErrorMessage; // error 
		int numericStatusCode = (int)statusCode;

		if (numericStatusCode == 200 || numericStatusCode == 307)
		{

			JsonValue res = JsonValue.Parse(content);

			if (res["data"].ContainsKey("Hoist")) { hoistPos = res["data"]["Hoist"]["variable"]["value"]; }
			if (res["data"].ContainsKey("Trolley")) { trolleyPos = res["data"]["Trolley"]["variable"]["value"]; }
			if (res["data"].ContainsKey("Bridge")) { bridgePos = res["data"]["Bridge"]["variable"]["value"]; }
			if (res["data"].ContainsKey("Load")) { load = res["data"]["Load"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("Inching")) { Inching = res["data"]["Inching"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("MicroSpeed")) { MicroSpeed = res["data"]["MicroSpeed"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("SwayControl")) { SwayControl = res["data"]["SwayControl"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("RopeAngleFeaturesBypass")) { RopeAngleFeaturesBypass = res["data"]["RopeAngleFeaturesBypass"]["variable"]["value"]; }
			if (res["data"].ContainsKey("WatchDogFault")) { WatchdogFault = res["data"]["WatchDogFault"]["variable"]["value"]; }
			if (res["data"].ContainsKey("Watchdog")) { watchdog = res["data"]["Watchdog"]["variable"]["value"]; }

			//if (res["data"].ContainsKey("AtTarget")) { atTarget = res["data"]["AtTarget"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("TargetNbrSelected")) { targetNbrSelected = res["data"]["TargetNbrSelected"]["variable"]["value"]; }
			//if (res["data"].ContainsKey("PositioningToTarget")) { positioningToTarget = res["data"]["PositioningToTarget"]["variable"]["value"]; }
			monitorQueryLog = content + "status: " + statusCode + " , status Code: " + numericStatusCode;
		}

		else
		{
			WatchdogFault = true;
			Debug.Log("monitor ERROR! status Code: " + statusCode + ", " + numericStatusCode + ", error message: " + errorMessage);
			monitorQueryLog = "monitor ERROR! status Code: " + statusCode + ", " + numericStatusCode + ", error message: " + errorMessage;

		}

		//Thread.Sleep(100); //millisecond -> 0.1s in time.sleep() for python
		//startMonitor = false;

	}

	void Control()
	{
		//Debug.Log("control start");
		//var client = new RestClient("http://192.168.0.77");

		checkControlUpdate = (checkControlUpdate % 30000) + 1;

		//TODO: get target position from selected fixed/movabele targets
		//int[] targetLocation = new int[3];

		//compare crane pos and target
		differenceHoist = hoistPos - hoistTarget;
		differenceTrolley = trolleyPos - trolleyTarget;
		differenceBridge = bridgePos - bridgeTarget;

		float speed = new float();

		if (differenceBridge > 0.02f)
		{
			speed = differenceBridge / 0.02f;
			controlBridge = "Backward";
			MoveCrane("Backward", "Bridge", speed);
		}
		else if (differenceBridge < -0.02f)
		{
			speed = differenceBridge / -0.02f;
			controlBridge = "Forward";
			MoveCrane("Forward", "Bridge", speed);
		}
		else
		{
			controlBridge = "Stop";
			StopCrane("Bridge");
		}

		if (differenceTrolley > 0.02f)
		{
			speed = differenceTrolley / 0.02f;
			controlTrolley = "Backward";
			MoveCrane("Backward", "Trolley", speed);
		}
		else if (differenceTrolley < -0.02f)
		{
			speed = differenceTrolley / -0.02f;
			controlTrolley = "Forward";
			MoveCrane("Forward", "Trolley", speed);
		}
		else
		{
			controlTrolley = "stop";
			StopCrane("Trolley");
		}

		if (differenceTrolley > -0.02f && differenceTrolley < 0.02f
			&& differenceBridge < 0.02f && differenceBridge > -0.02f)
		{
			if (differenceHoist > 0.02f)
			{
				speed = differenceHoist / -0.02f;
				controlHoist = "Down";
				MoveCrane("Down", "Hoist", speed);
			}
			else if (differenceHoist < -0.02f)
			{
				speed = differenceHoist / -0.02f;
				controlHoist = "Up";
				MoveCrane("Up", "Hoist", speed);
			}
			else
			{
				controlHoist = "stop";
				StopCrane("Hoist");
			}
		}

	}


	// NOTE: since "Controls.TargetPositioning.Target" can only change the value of target_requsted but not of the target_selected in Status, 
	// however, only the latter one will determine where the crane move towards in targetpositining control mode. It seems that target can only be selected with the manual controller interface.   

	//void TargetPostioning()
	//{
	//	checkTargetPositioningUpdate = (checkTargetPositioningUpdate % 30000) + 1;


	//	string querySetValues = $@"mutation {{
	//				DriveToHome: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.TargetPositioning.DriveToHome"", value: false, dataType: ""Boolean"")  {{ok}}
	//				SelectionInUse: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.TargetPositioning.SelectionInUse"", value: {selectionInUse}, dataType: ""Boolean"")  {{ok}}
	//				DriveToTarget: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.TargetPositioning.DriveToTarget"", value: true, dataType: ""Boolean"")  {{ok}}
	//				Target: setValue(server: ""Ilmatar"", nodeId: ""ns=7;s=SCF.PLC.DX_Custom_V.Controls.TargetPositioning.Target"", value: {target}, dataType: ""Int16"")  {{ok}}
	//			}}";

	//	//bool notAtTarget = new bool();
	//	//notAtTarget = !atTarget;

	//	//if (atTarget)
	//	//	target = 0;

	//	var request = new RestRequest("graphql");
	//	request.AddParameter("application/graphql", querySetValues, ParameterType.RequestBody);

	//	var response = client.Post(request);
	//	var content = response.Content;
	//	var statusCode = response.StatusCode; //400: one of the error cases; 200: correct; 307: might also correct (validate during developing, check the print) 
	//	var errorMessage = response.ErrorMessage; // error 
	//	int numericStatusCode = (int)statusCode;

	//	if (numericStatusCode == 200 || numericStatusCode == 307)
	//		Debug.Log("TargetPostioning request content: " + content + ", status: " + statusCode + " , status Code: " + numericStatusCode);
	//	else
	//		Debug.Log("TargetPostioning ERROR! status Code: " + numericStatusCode + ", error message: " + errorMessage);
	//}


	public void Start()
	{
		Debug.Log("Connection start");
		Time.fixedDeltaTime = fixedDeltaTime; // default 100ms;  
		client.Timeout = timeout; // default: 1s

	}

	private void FixedUpdate()
	{
		if (startWatchdog == true)
		{
			Debug.Log("watchdog start");
			Watchdog();
		}

		if (startMonitor == true)
		{
			Debug.Log("monitor start");
			Monitor();
		}

		if (startControl == true)
		{
			Debug.Log("control start");
			Control();
		}

	}
}


