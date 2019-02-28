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
		public static bool isMaster1,isRemote1,JoinedRoomFlag1;
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
			StartCoroutine(RoomCreationMethod());
			//			http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid=2&strgamesessionid=1&intgametype=2&dblamount=100
		}
		IEnumerator RoomCreationMethod()
		{
			string userid = PlayerPrefs.GetString ("userid");
			string sessionId = "" + 1;
			int gameType = 4;
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid="+userid+"&strgamesessionid="+sessionId+"&intgametype="+gameType+"&dblamount="+EventSystem.current.currentSelectedGameObject.name);
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
			}
		}
		public void CreateRoomMethod()
		{
			if(GameLobbyName.Length==0){
				StartCoroutine (WarningForRoom ());
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
		IEnumerator WarningForRoom()
		{
			WarningText.text = "PLEASE SELECT THE Amount";
			yield return new WaitForSeconds (1);	
			WarningText.text = "";
		}
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
			isMaster1 = true;
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print(msg1);
		}
		public override void OnJoinRandomFailed (short returnCode, string message)
		{

			print (PhotonNetwork.CurrentLobby.Name);
			Scene currscene = SceneManager.GetActiveScene ();
			string CurrSceneName = currscene.name;
			print ("No rooms are available in this lobby, So the Room Creation Failed");
			TypedLobby sqlLobby = new TypedLobby (GameLobbyName, LobbyType.SqlLobby);
			PhotonNetwork.CreateRoom (RandomRoomName, new Photon.Realtime.RoomOptions { 
				MaxPlayers = 4,
				PlayerTtl = 300000, 
				EmptyRoomTtl = 30000 
			}
				, typedLobby:sqlLobby);
		}
		public override void OnJoinedRoom()
		{
			print ("Joined Random Successfully");
			print (PhotonNetwork.MasterClient.NickName);
			print ("Room Joined successfully");
			if (PhotonNetwork.PlayerList.Length == 2) 
			{
				isRemote1 = true;
				RoomOptions roomOptions = new RoomOptions ();
				roomOptions.IsVisible = false;
				roomOptions.IsOpen = false;
			}
			if (!JoinedRoomFlag1) {
				SceneManager.LoadScene ("ColorPickingFor4playerRandom");
				//				SceneManager.LoadScene("BettingAmountScene");
			}
			JoinedRoomFlag1 = true;
		}
	}
}
