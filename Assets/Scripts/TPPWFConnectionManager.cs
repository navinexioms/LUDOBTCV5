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
		private GameObject QuitPanel;
		public Text RoomName,WarningText;
		public string RoomN=null, Amount=null;
		public int PlayerLength=0;
		public string GameLobbyName=null;
		public List<GameObject> Amounts;
		void Awake()
		{
			DontDestroyOnLoad (this);
		}

		public void CreateOrJoinRoomMethod()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				print ("no Internet connection is there");
				StartCoroutine (RoomNameWarning ("PLEASE CONNECT TO INTERNET"));
			} else if (!PlayWithFriendSceneManager.isSelectedGameType) {
				StartCoroutine (RoomNameWarning ("PLEASE SELECT THE GAMETYPE"));
			}else if (RoomName.text.Length > 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)) {
				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}
				string PlayerName = PlayerPrefs.GetString ("userid");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			} else if (RoomName.text.Length == 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork))  {
				StartCoroutine (RoomNameWarning("PLEASE ENTER THE ROOM NAME"));
			}
		}

		public IEnumerator RoomNameWarning(string warn)
		{
			WarningText.text = warn;
			yield return new WaitForSeconds (1);
			WarningText.text = null;
		}



		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			PhotonNetwork.JoinLobby ();
		}
		public override void OnJoinedLobby()
		{
			RoomN = RoomName.text;
				PhotonNetwork.JoinOrCreateRoom (RoomName.text, new Photon.Realtime.RoomOptions {
				MaxPlayers = 2,
				PlayerTtl = 300000,
				EmptyRoomTtl = 10000
			}, null);
		}
		public override void OnCreatedRoom()
		{
			print ("Room Created Successfully");

			isMaster = true;
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
//				SceneManager.LoadScene ("OneOnOneGameBoard");

				SceneManager.LoadScene("BettingAmountFor2PlayerPlayWithFriends");
				StartCoroutine(	AddFunction ());
			}
			JoinedRoomFlag = true;
		}

		IEnumerator AddFunction()
		{
			yield return new WaitForSeconds (1);
			GameObject btn = GameObject.Find ("Button") as GameObject;
			btn.GetComponent<Button> ().onClick.AddListener(() => InviteFriend ());
			Amounts.Add (GameObject.Find ("100") as GameObject);
			Amounts.Add (GameObject.Find ("500") as GameObject);
			Amounts.Add (GameObject.Find ("1000") as GameObject);
			Amounts.Add (GameObject.Find ("5000") as GameObject);
			Amounts.Add (GameObject.Find ("10000") as GameObject);
			Amounts.Add (GameObject.Find ("25000") as GameObject);
			Amounts.Add (GameObject.Find ("50000") as GameObject);
			Amounts.Add (GameObject.Find ("100000") as GameObject);
			Amounts [0].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [1].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [2].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [3].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [4].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [5].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [6].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});
			Amounts [7].GetComponent<Toggle> ().onValueChanged.AddListener (delegate {AmountSelectionMethod ();});

		}

		public void AmountSelectionMethod()
		{
			print ("AmountSelectionMethod");
			GameLobbyName= EventSystem.current.currentSelectedGameObject.name;
		}
		public void InviteFriend()
		{
			if(GameLobbyName.Length > 0)
			{
				ShareSheet shareSheet = new ShareSheet ();
				shareSheet.Text +="Room Name:"+ RoomN+" Amount:"+GameLobbyName;
				NPBinding.Sharing.ShowView (shareSheet, FinishSharing);
			}else{
				StartCoroutine(WarningForAvatar());
			}
		}

		private void FinishSharing(eShareResult _result)
		{
			print (_result);
			SceneManager.LoadScene ("OneOnOneGameBoard");
		}
		IEnumerator WarningForAvatar()
		{
			GameObject WarningText1 = GameObject.Find ("WarningText");
			WarningText1.GetComponent<Text>().text = "PLEASE SELECT THE Amount";
			yield return new WaitForSeconds (1);	
			WarningText1.GetComponent<Text>().text = "";
		}
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
	}
}
