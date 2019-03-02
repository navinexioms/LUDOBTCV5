using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Realtime;
using UnityEngine.Networking;
using SimpleJSON;
namespace Photon.Pun.UtilityScripts
{
	public class FourPlayerConnectionManager : MonoBehaviourPunCallbacks 
	{
		public static bool isMaster,isRemote1,isRemote2,isRemote3,JoinedRoomFlag;
		public Text WarningText;
		private GameObject QuitPanel;
		public string GameLobbyName=null;
		public string RandomRoomName = null;
	// Use this for initialization
		void Awake()
		{
			DontDestroyOnLoad (this);
		}
		public void AmountSelectionMethod()
		{
			GameLobbyName = EventSystem.current.currentSelectedGameObject.name;
			StartCoroutine (AmountCheckingBeforeEntering ());
//			StartCoroutine(RoomCreationMethod());
//			http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid=2&strgamesessionid=1&intgametype=2&dblamount=100
		}


		IEnumerator AmountCheckingBeforeEntering()
		{
			print ("AmountCheckingBeforeEntering");
			GameLobbyName = EventSystem.current.currentSelectedGameObject.name;
//			http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid=2&gamesessionid=1&dblbidamt=100
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&dblbidamt="+EventSystem.current.currentSelectedGameObject.name);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went Wrong");
			}
			string msg = www.downloadHandler.text;
			msg = msg.Substring (1, msg.Length - 2);
			JSONNode jn = SimpleJSON.JSONData.Parse (msg);
			msg = null;
			msg = jn [0];
			if (msg.Equals ("Successful")) {
				print ("Have enough balance");
			} else if (msg.Equals ("Youdon'thavesufficientbalanceforbid")) {
				print ("You don't have sufficient balance for bid");
				GameLobbyName = null;
				GameLobbyName = "nothing";
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}


		IEnumerator RoomCreationMethod()
		{
			print ("RoomCreationMethod");
			string userid = PlayerPrefs.GetString ("userid");
			string sessionId = "" + 1;
			int gameType = 4;
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid="+userid+"&strgamesessionid="+sessionId+"&intgametype="+gameType+"&dblamount="+GameLobbyName);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print (www.error);
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				RandomRoomName = jn [0];

				TypedLobby sqlLobby = new TypedLobby (GameLobbyName, LobbyType.SqlLobby);
				print (PhotonNetwork.CurrentLobby.Name);
				PhotonNetwork.CreateRoom (RandomRoomName, new Photon.Realtime.RoomOptions { 
					MaxPlayers = 4,
					PlayerTtl = 300000, 
					EmptyRoomTtl = 30000 
				}, typedLobby:sqlLobby);
			}
		}


		IEnumerator AmountCheckingAfterEntering()
		{
			print ("AmountCheckingAfterEntering");
//			http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid=2&gamesessionid=1&intWalletType=2&dblamt=10000&gametype=2&roomid=2PLDO1&date=27/02/2019
			RandomRoomName = PhotonNetwork.CurrentRoom.Name;
			string tempRoomName = RandomRoomName;
			print (tempRoomName);
			print("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid="+1+"&intWalletType="+2+"&dblamt="+GameLobbyName+"&gametype="+4+"&roomid="+RandomRoomName+"&date="+System.DateTime.Now.ToString ("d"));
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid="+1+"&intWalletType="+2+"&dblamt="+GameLobbyName+"&gametype="+4+"&roomid="+RandomRoomName+"&date="+System.DateTime.Now.ToString ("d"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something Went wrong");
			} else {
				print (www.downloadHandler.text);
				RandomRoomName = www.downloadHandler.text;
				RandomRoomName = RandomRoomName.Substring (1, RandomRoomName.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (RandomRoomName);
				RandomRoomName = null;
				RandomRoomName = jn [0];
				print (RandomRoomName);
				if (RandomRoomName.Equals ("Successfullybidforgame")) {
					RandomRoomName = tempRoomName;
					PlayerPrefs.SetString ("roomid", RandomRoomName);
					SceneManager.LoadScene ("ColorPickingFor4playerRandom");
				} else {
					StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
				}
			}
		}


		public void CreateRoomMethod()
		{
			if(GameLobbyName.Length==0){
				StartCoroutine (WarningForRoom ("Please select the amount",2));
			}
			else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}
				string PlayerName = PlayerPrefs.GetString ("userid");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			} 
		}
		IEnumerator WarningForRoom(string msg,int timer)
		{
			WarningText.text = msg;
			yield return new WaitForSeconds (timer);	
			WarningText.text = "";
		}

		#region Room Creation And Join Related CallbackMethods

		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			TypedLobby sqlLobby = new TypedLobby (GameLobbyName, LobbyType.SqlLobby);
			PhotonNetwork.JoinLobby (sqlLobby);
		}
		public override void OnJoinedLobby()
		{
			print ("Joined lobby");
			PhotonNetwork.JoinRandomRoom ();
		}
		public override void OnCreatedRoom()
		{
			
			print ("Room Created Successfully");
			StartCoroutine(AmountCheckingAfterEntering ());
			print ("Room Created Successfully");
			isMaster = true;
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print (msg1);
		}
		public override void OnJoinRandomFailed (short returnCode, string message)
		{
			print(message);
			print (PhotonNetwork.CurrentLobby.Name);
			print ("No rooms are available in this lobby, So the Room Creation Failed");
			StartCoroutine (RoomCreationMethod ());
		}
		public override void OnJoinedRoom()
		{
			print ("Joined Random Successfully");
			print (PhotonNetwork.MasterClient.NickName);
			print ("Room Joined successfully");

			switch(PhotonNetwork.CurrentRoom.PlayerCount)
			{
			case 1:
				isMaster = true;
				break;
			case 2:
				isRemote1 = true;
				if (!JoinedRoomFlag) {
					StartCoroutine(AmountCheckingAfterEntering ());
					SceneManager.LoadScene ("ColorPickingFor4playerRandom");
				}
				break;
			case 3:
				isRemote2 = true;
				if (!JoinedRoomFlag) {
					StartCoroutine(AmountCheckingAfterEntering ());
					SceneManager.LoadScene ("ColorPickingFor4playerRandom");
				}
				break;
			case 4:
				isRemote3 = true;
				if (!JoinedRoomFlag) {
					StartCoroutine(AmountCheckingAfterEntering ());
					SceneManager.LoadScene ("ColorPickingFor4playerRandom");
				}
				break;
			}
			JoinedRoomFlag = true;
		}
		#endregion
	}
}
