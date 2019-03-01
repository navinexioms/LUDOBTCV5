using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using SimpleJSON;
public class ExtraSceneController : MonoBehaviour 
{
	public GameObject QuitPanel;
	public static int HowManyPlayers;
	public static int Playmode = 0;
	public Toggle TwoPlayerToggle, FourPlayerToggle;
	public GameObject TwoPlayerGameObject, FourPlayerGameObject;
	string SceneName =null;
	public Text WarningText;
	public string Username = null,Emailid=null,Mobilenumber=null,Password=null,Newpassword=null;
	void Start()
	{
		Scene currScene = SceneManager.GetActiveScene();
		SceneName = currScene.name;
		if (SceneName.Equals ("BettingAmountFor2PlayerPlayWithFriends")) {
			print (PlayerPrefs.GetString ("roomname"));
		}
	}
//	.
//	IEnumerator WinnerAPI(string playerid,string playerColor)
//	{
//		string 	ThisLobbyName=""+200;		
//		string RName = "2PLDO1";
//		string id = PlayerPrefs.GetString ("userid");
//		string CurrentDate = System.DateTime.Now.ToString ("d");
////		UnityWebRequest www = new UnityWebRequest ("//http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&intWalletType=1&dblamt="+ThisLobbyName+"&gametype=2&roomid="+PhotonNetwork.CurrentRoom.Name+"&date="+System.DateTime.Now.ToString ("d")+"&playercolor="+playerColor+"&playerid="+playerid);
//		string url1="http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid="+id+"&gamesessionid=1&intWalletType=1&dblamt="+ThisLobbyName+"&gametype=2&roomid="+RName+"&date="+CurrentDate+"&playercolor="+playerColor+"&playerid="+playerid;
//		print (url1);
//		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid="+id+"&gamesessionid=1&intWalletType=1&dblamt="+ThisLobbyName+"&gametype=2&roomid="+RName+"&date="+CurrentDate+"&playercolor="+playerColor+"&playerid="+playerid);
//		www.chunkedTransfer=false;
//		www.downloadHandler=new DownloadHandlerBuffer();
//		yield return www.SendWebRequest();
//		if(www.error!=null){
//			print("Something went Wrong");
//		}else{
//			print(www.downloadHandler.text);
//			string msg = www.downloadHandler.text;
//			msg = msg.Substring (1, msg.Length - 2);
//			JSONNode jn = SimpleJSON.JSONData.Parse (msg);
//			string msg2 = jn [0];
//			print (msg2);
//			//Amount added in wallet
//			if (msg2.Equals ("Amountaddedinwallet")) {
//				print ("Amount Credited");
//			} else {
//				print ("Amount not Credited");
//			}
//		}
//	}
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) &&  SceneName.Equals("GameMenu")) {
			print ("Pressing Escape");
			print ("Player Will Logout");
//			QuitPanel.SetActive (true);
			QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 1);
		}
	}




	public void QuitTheGame()
	{
		Application.Quit ();
	}
	public void CancelTheQuit()
	{
		QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 2);
	}

	public void SelectTwoPlayerGamePlay()
	{
		TwoPlayerGameObject.SetActive (TwoPlayerToggle.isOn);
		FourPlayerGameObject.SetActive (false);
	}
	public void SelectFourPlayerGamePlay()
	{
		TwoPlayerGameObject.SetActive (false);
		FourPlayerGameObject.SetActive (FourPlayerToggle.isOn);
	}
	public void LoadBettingAmountForOneOnOneScene()
	{
		SceneManager.LoadScene ("BettingAmountForOneOnOne");
	}
	public void LoadBettingAmountFor4PlayerRandom()
	{
		StartCoroutine( Warningmethod("This feature will be added very soon"));
//		SceneManager.LoadScene ("BettingAmountFor4PlayerRandom");
	}
	public void LoadBettingAmountFor2PlayerPlayWithFriends()
	{
		SceneManager.LoadScene ("BettingAmountFor2PlayerPlayWithFriends");
	}
	public void LoadBettingAmountFor4PlayerPlayWithFriends()
	{
		SceneManager.LoadScene ("BettingAmountFor4PlayerPlayWithFriends");
	}

	public void LoadColorPickingForOneOnOne()
	{
		SceneManager.LoadScene ("ColorPickingForOneOnOne");
	}
	public void LoadColorPickingFor4PlayerRandom()
	{
		SceneManager.LoadScene ("ColorPickingFor4PlayerRandom");
	}
	public void LoadColorPicking2PlayerPlayWithFriends()
	{
		SceneManager.LoadScene ("ColorPickingFor2PlayerPlayWithFriends");
	}
	public void LoadColorPicking4PlayerPlayWithFriends()
	{
		SceneManager.LoadScene ("ColorPickingFor4PlayerPlayWithFriends");
	}
	public void LoadExtraScene()
	{
		SceneManager.LoadScene ("ExtraScenes");
	}
	public void LoadProfileScene()
	{
		SceneManager.LoadScene ("ProfileScene");
	}
	public void WalletScene()
	{
		SceneManager.LoadScene ("WalletScene");
	}
	public void  LoadTransactionScene()
	{
		SceneManager.LoadScene ("TransactionScene");
	}
	public void SettingScene()
	{
		SceneManager.LoadScene ("SettingScene");
	}
	public void Logout()
	{
//		SceneManager.LoadScene ("Home");
		StartCoroutine(HitUrl11(0));
	}
	public void LoadFourPlayerGame()
	{
		SceneManager.LoadScene ("FourPlayerGameScene");
	}
	public void SaveuserDataAndLoadExtraScene()
	{
		if (Password.Length > 0 && Newpassword.Length == 0) {
			print ("Enter New Password");
			StartCoroutine (Warningmethod ("Please Enter New Password"));
		} else if (Password.Length == 0 && Newpassword.Length > 1) {
			print ("Enter Password");
			StartCoroutine (Warningmethod ("Please Enter Password"));
		}else if (Username.Length == 0) {
			print ("Enter Username");
			StartCoroutine (Warningmethod ("Please Enter username"));
		} else if (Emailid.Length == 0) {
			print ("Enter Emailid");
			StartCoroutine (Warningmethod ("Please Enter MailID"));
		}else if (Mobilenumber.Length == 0) {
			print ("Enter Mobile Number");
			StartCoroutine (Warningmethod ("Please Enter Mobile Number"));
		} else if (Mobilenumber.Length < 10) {
			print ("Mobile number is too short");
			StartCoroutine (Warningmethod ("Mobile number is too short"));
		}else if (Application.internetReachability == NetworkReachability.NotReachable) {
			print ("No Internet Connection");
			StartCoroutine (Warningmethod ("Please connect to internet"));
		}else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
			print ("internet connection");
			if ((Password.Length == 0 && Newpassword.Length == 0) || (Password.Length > 0 && Newpassword.Length > 0)) {
				if (Password.Length > 0 && Newpassword.Length > 0) {
					if (Password.Equals (Newpassword)) {
						print ("Please enter different new Password");
					} else if (!Password.Equals (Newpassword)) {
						print ("Password is matching");
						Regex regex = new Regex (@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
						bool isValid = regex.IsMatch (Emailid.Trim ());
						if (!isValid) {
							print ("Not valid emailid");
							StartCoroutine (Warningmethod ("Please Enter Valid Email"));
						} else {
							print ("Valid Emailid");
							print ("Can Proceed");
							StartCoroutine (SaveUserData ());
						}
//					StartCoroutine (SaveUserData ());
					}
				}
				if (Password.Length == 0 && Newpassword.Length == 0) {
					Regex regex = new Regex (@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
					bool isValid = regex.IsMatch (Emailid.Trim ());
					if (!isValid) {
						print ("Not valid emailid");
						StartCoroutine (Warningmethod ("Please Enter Valid Email"));
					} else {
						print ("Valid Emailid");
						print ("Can Proceed");
						StartCoroutine (SaveUserData ());
					}
				}
			}
		}
	}	

	IEnumerator Warningmethod(string warning)
	{
		WarningText.text=warning;
		yield return new WaitForSeconds (1);
		WarningText.text = null;
	}

	IEnumerator SaveUserData()
	{
		string id = PlayerPrefs.GetString ("userid");
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Home/EditProfile?username="+Username+"&emailID="+Emailid+"ebtcmember@exioms.biz&mobilenumber="+Mobilenumber+"1213456&password="+Password+"ebtc@member&newpassword="+Newpassword+"ebtc@member&userid="+id);	
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
			msg = null;
			msg = jn [0];
			if (msg.Equals ("Profilesuccessfullyupdated")) {
				SceneManager.LoadScene ("ExtraScenes");
			}
		}
	}

	public void TakeUsername(string username)
	{
		Username = username;
	}
	public void TakeEmailid(string emailid)
	{
		Emailid = emailid;
	}
	public void TakeMobileNumber(string mobilenumber)
	{
		Mobilenumber = mobilenumber;
	}
	public void TakePassword(string password){
		Password = password;
	}
	public void TakeNewPassword(string newPassword)
	{
		Newpassword = newPassword;
	}
	public void LoadExtraSceneFromSetting()
	{
		SceneManager.LoadScene ("ExtraScenes");
	}
	public void LoadExtraScenefromTransaction()
	{
		SceneManager.LoadScene ("ExtraScenes");
	}
	public void LoadExtraSceneFromWalletScene()
	{
		SceneManager.LoadScene ("ExtraScenes");
	}
	public void LoadMainGameSceneFromExtraScene()
	{
		SceneManager.LoadScene ("GameMenu");
	}
	public void LoadPlayerVSAIScene()
	{
		HowManyPlayers = 2;
		SceneManager.LoadScene ("PlayerVSAI");
	}
	public void LoadTwoPlayWithFriend()
	{
		SceneManager.LoadScene ("PlayWithFriend");
	}
	public void LoadOneOnOneScene()
	{
		SceneManager.LoadScene ("OneOnOneGameBoard");
	}

	void OnApplicationQuit()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
			print ("Quit the Application When internet connection is There and Logged out");
			StartCoroutine (HitUrl11 (0));
			PlayerPrefs.SetString ("userid", null);
		} else if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			print ("no Internet connection is there can't Logout");
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
			string msg = request11.downloadHandler.text;
			msg = msg.Substring (1, msg.Length - 2);
			JSONNode jn = SimpleJSON.JSONData.Parse (msg);
			msg = jn [0];
			if (msg.Equals ("Successful")) {
				PlayerPrefs.SetString ("userid", null);
				SceneManager.LoadScene ("Home");
			}
		}
	}
}
