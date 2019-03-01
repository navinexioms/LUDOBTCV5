using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VoxelBusters;
using VoxelBusters.NativePlugins;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
namespace Photon.Pun.UtilityScripts
{
	public class TPPWFConnectionManager : MonoBehaviourPunCallbacks {
		public static bool isMaster,isRemote,JoinedRoomFlag;
		public bool isSelectedAmount;
		private GameObject QuitPanel;
		public GameObject AmountCheckButton;
		public GameObject RoomEnterButton;
		public Text WarningText;
		public string GameLobbyName=null;
		public int PlayerLength=0;
		public List<GameObject> Amounts;



		void Awake()
		{
			DontDestroyOnLoad (this);
		}


		public void WarningMethod()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				print ("no Internet connection is there");
				StartCoroutine (RoomNameWarning ("PLEASE CONNECT TO INTERNET"));
			} else if (!isSelectedAmount) {
				StartCoroutine (RoomNameWarning ("PLEASE select the amount"));
			} else if (isSelectedAmount && !RoomEnterButton.activeInHierarchy) {
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}


		public void CreateOrJoinRoomMethod()
		{
			 if (PlayerPrefs.GetString("roomname").Length > 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)) {
				InviteFriend ();
			}
		}
		public IEnumerator RoomNameWarning(string warn)
		{
			WarningText.text = warn;
			yield return new WaitForSeconds (1);
			WarningText.text = null;
		}

		public void InviteFriend()
		{
			ShareSheet shareSheet = new ShareSheet ();
			shareSheet.Text +="Lets Play LudoBtc Game Together. Please Join My Room "+"Room Name:"+ PlayerPrefs.GetString("roomname")+" Amount: "+ PlayerPrefs.GetString("amount");
			NPBinding.Sharing.ShowView (shareSheet, FinishSharing);
		}
		private void FinishSharing(eShareResult _result)
		{
			print (_result);
			print ("FinishSharing ()");





			StartCoroutine(AmountCheckingAfterEntering());


		}

		public void AmountSelectionMethod()
		{
			print ("AmountSelectionMethod");
			isSelectedAmount = true;
			GameLobbyName= EventSystem.current.currentSelectedGameObject.name;
			PlayerPrefs.SetString ("amount", GameLobbyName);
			AmountCheckButton.SetActive (false);
			StartCoroutine (AmountCheckingBeforeEntering ());
		}

		IEnumerator AmountCheckingBeforeEntering()
		{
			print ("AmountCheckingBeforeEntering");
			GameLobbyName = EventSystem.current.currentSelectedGameObject.name;
//			http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid=2&gamesessionid=1&dblbidamt=100
//			id=null;
//			id=PlayerPrefs.GetString("userid");
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
				RoomEnterButton.SetActive (true);
				AmountCheckButton.SetActive (false);
			} else if (msg.Equals ("Youdon'thavesufficientbalanceforbid")) {
				print ("You don't have sufficient balance for bid");
				GameLobbyName = null;
				GameLobbyName = "nothing";
				RoomEnterButton.SetActive (false);
				AmountCheckButton.SetActive (true);
				PlayerPrefs.SetString ("amount", null);
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}


		IEnumerator AmountCheckingAfterEntering()
		{
			print ("AmountCheckingAfterEntering");
//			http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid=2&gamesessionid=1&intWalletType=2&dblamt=10000&gametype=2&roomid=2PLDO1&date=27/02/2019
			string id=PlayerPrefs.GetString("userid");
			string RandomRoomName = PlayerPrefs.GetString ("roomname");
			print("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid="+1+"&intWalletType="+2+"&dblamt="+GameLobbyName+"&gametype="+2+"&roomid="+RandomRoomName+"&date="+System.DateTime.Now.ToString ("d"));
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid="+1+"&intWalletType="+2+"&dblamt="+GameLobbyName+"&gametype="+2+"&roomid="+RandomRoomName+"&date="+System.DateTime.Now.ToString ("d"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something Went wrong");
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				msg = null;
				msg = jn [0];
				print (msg);
				if (msg.Equals ("Successfullybidforgame")) {
//					SceneManager.LoadScene ("OneOnOneGameBoard");

					if (PhotonNetwork.AuthValues == null) {
						PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
					}
					string PlayerName = PlayerPrefs.GetString ("userid");
					print (PlayerPrefs.GetString ("userid"));
					PhotonNetwork.AuthValues.UserId = PlayerName;
					PhotonNetwork.LocalPlayer.NickName = PlayerName;
					PhotonNetwork.ConnectUsingSettings ();


				} else {
					StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
				}
			}
		}
			


		IEnumerator WarningForRoom(string msg,int time)
		{
			WarningText.text = msg;
			yield return new WaitForSeconds (time);	
			WarningText.text = "";
		}


		#region Room Related Callback method

		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			PhotonNetwork.JoinLobby ();
		}
		public override void OnJoinedLobby()
		{
			
			PhotonNetwork.JoinOrCreateRoom (PlayerPrefs.GetString("roomname"), new Photon.Realtime.RoomOptions {
				MaxPlayers = 2,
				PlayerTtl = 300000,
				EmptyRoomTtl = 10000
			}, null);
		}
		public override void OnCreatedRoom()
		{
			print ("Room Created Successfully");
			isMaster = true;
			SceneManager.LoadScene ("OneOnOneGameBoard");
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print(msg1);

			PhotonNetwork.JoinRoom ("nsd");
		}
		public override void OnJoinedRoom()
		{
			print (PhotonNetwork.MasterClient.NickName);
			print ("Room Joined successfully");
			if (PhotonNetwork.PlayerList.Length == 2) 
			{
				isRemote = true;
			}
			if (!JoinedRoomFlag) {
				SceneManager.LoadScene ("OneOnOneGameBoard");
			}
			JoinedRoomFlag = true;
		}

		//#endregion

		void OnApplicationQuit()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
				print ("Quit the Application When internet connection is There and Logged out");
				StartCoroutine (HitUrl11 (0));
				PlayerPrefs.SetString ("userid", null);
			}
			else if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				print ("no Internet connection is there");
			}
		}
		IEnumerator HitUrl11(int status)
		{
			print ("HitUrl11");
			string id= PlayerPrefs.GetString ("userid");
			UnityWebRequest request11 =new UnityWebRequest("http://apienjoybtc.exioms.me/api/Home/login_session?userid="+id+"&gamesessionid="+status);

			request11.chunkedTransfer = false;

			request11.downloadHandler = new DownloadHandlerBuffer ();

			yield return request11.SendWebRequest ();

			if (request11.error != null) {

			} else {
				print (request11.downloadHandler.text);
			}
		}
		#endregion
	}
}
