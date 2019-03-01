using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SceneController : MonoBehaviour
{
	public GameObject SplashScreen;
	public GameObject LoginPanel;
	public GameObject Blackscreen;
	public GameObject AvatarScreen;
	public Text Username1;
	public Text Password1;
	public Text WarningText;
	JSONNode rootnode = new JSONClass ();
	JSONNode rootnode1 = new JSONClass ();
	public string Username=null, Password=null;
	string KeyValues=null;
	public int AvatarSelectionValue = 0;
	private bool netConnectivity;

	private Animator Anim;

	void Start()
	{
		print ("Hello");
		Anim = Blackscreen.GetComponent<Animator> ();
		StartCoroutine (LoadPanel (SplashScreen,LoginPanel,1,"a"));
		print (PlayerPrefs.GetString ("userid"));
		PlayerPrefs.SetString ("Avatar", "0");
		Scene CurrScene = SceneManager.GetActiveScene ();
		string SceneName = CurrScene.name;
		print (SceneName);
		if (SceneName.Equals ("Home") ) {
			string id = null;
			id = PlayerPrefs.GetString ("userid");
			string AvatarNo = null;
			AvatarNo = PlayerPrefs.GetString ("Avatar");

			if (id.Length>0 ) {
				print ("User not Logged out");
				LoginPanel.SetActive (false);
				LoadGameOptionMenu ();
			}
		}
	}
	void Update()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
		{
//			print ("internet connection");
//			print(PlayerPrefs.GetString("userid"));
		}
		else if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			print ("no Internet connection is there");
		}
	}
	public void LoadAvatarPanel()
	{
		print ("hello111");
		if (Username.Length == 0) 
		{
			StartCoroutine (LoadPanel (LoginPanel, AvatarScreen, 1.1f, "please enter username"));
			PlayerPrefs.SetString ("userid", null);
		} 
		else if (Password.Length == 0) 
		{
			StartCoroutine (LoadPanel (LoginPanel, AvatarScreen, 1.1f, "please enter password"));
			PlayerPrefs.SetString ("userid", Username);
		}
		else if (Application.internetReachability == NetworkReachability.NotReachable) 
		{
			StartCoroutine (LoadPanel (LoginPanel, AvatarScreen, 1.1f, "please connect to internet"));	
		}
		else if (Username.Length > 0 && Password.Length > 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)) 
		{
			StartCoroutine (HitUrl());
		}

	}
	IEnumerator LoadPanel(GameObject panelToDisable,GameObject PanelToEnable,float time,string message)
	{
		if (time == 1.1f) 
		{
			yield return new WaitForSeconds (0);
			StartCoroutine (WaitForWhile (panelToDisable, PanelToEnable, 1.1f,message));	
		} 
		else 
		{
			yield return new WaitForSeconds (time);
			Anim.SetInteger ("Counter", 1);
			StartCoroutine (WaitForWhile (panelToDisable, PanelToEnable, 1,"a"));
		}
	}
	IEnumerator WaitForWhile(GameObject panelToDisable,GameObject PanelToEnable,float time,string message)
	{
		print ("time:"+time);
		if (time == 1.1f) 
		{
			print ("Waiting");
			GameObject WarningText= GameObject.Find ("NetConnectionWarning") as GameObject;
			WarningText.GetComponent<Text> ().text = ""+message;
			yield return new WaitForSeconds(1f);
			WarningText.GetComponent<Text>().text=null;
		} 
		else 
		{
			print ("Waiting");
			yield return new WaitForSeconds (time);
			panelToDisable.SetActive (false);
			PanelToEnable.SetActive (true);
			Anim.SetInteger ("Counter", 2);
		}
	}
	IEnumerator HoldForoneSec()
	{
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("GameMenu");

	}

	IEnumerator HitUrl()
	{
		UnityWebRequest request =new UnityWebRequest("http://apienjoybtc.exioms.me/api/Home/login?my_sponsar_id="+Username+"&password="+Password);

		request.chunkedTransfer = false;

		request.downloadHandler = new DownloadHandlerBuffer ();

		yield return request.SendWebRequest ();
		print(request.downloadHandler.text);
		if (request.error != null) {
			print ("something went wrong");
		} else {
			string msg = request.downloadHandler.text.ToString();
			if (msg.Contains ("{")) {
				msg=msg.Substring(1,msg.Length-2);
				rootnode = SimpleJSON.JSONData.Parse (msg);
				KeyValues = rootnode [0];
				if (!KeyValues.Equals ("Invalidlogincredentials")) {
					AvatarSelectionValue = int.Parse (rootnode [1]);
				}
				PlayerPrefs.SetString ("Avatar", ""+AvatarSelectionValue);
				print (AvatarSelectionValue);
				if (!KeyValues.Contains ("Invalidlogincredentials")) {
					PlayerPrefs.SetString ("userid", rootnode [0]);
					string name = PlayerPrefs.GetString ("userid");
					StartCoroutine(HitUrl11(1));				
				}
				else {
					print ("Invalid");
					StartCoroutine (LoadPanel (LoginPanel, AvatarScreen, 1.1f, "invalid login credentials"));
				}
			} 
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
			string msg = request11.downloadHandler.text.ToString ();
			msg = msg.Substring (1, msg.Length - 2);
			rootnode1 = SimpleJSON.JSONData.Parse (msg);
			string status1 =null;
			status1=rootnode1[0];
			if (status1.Equals ("Successful")) {
				if (AvatarSelectionValue == 11) {
					print ("Not Selected the Avatar");
					StartCoroutine (LoadPanel (LoginPanel, AvatarScreen, 1f, "a"));
				}else if (AvatarSelectionValue < 11) {
					print ("Selected the Avatar");
					LoadGameOptionMenu ();
				}
			}
		}
	}

	public void LoadGameOptionMenu1()
	{
		print ("Selected:" + AvatarSelectionValue);
		StartCoroutine (AvatarHitUrl (AvatarSelectionValue));
	}
	IEnumerator WarningForAvatar()
	{
		WarningText.text = "PLEASE SELECT THE AVATAR";
		yield return new WaitForSeconds (1);	
		WarningText.text = "OR";
	}
	IEnumerator AvatarHitUrl(int num)
	{
		print ("num:"+num);
		string id= PlayerPrefs.GetString ("userid");
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Home/avtarselection?userid="+id+"&avtarnumber="+num);
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
			print (msg);
			if (msg.Equals ("SuccessfullyAvtarselected")) {
				PlayerPrefs.SetString ("Avatar", "" + num);
				print ("Avatar:" + PlayerPrefs.GetString ("Avatar"));
				LoadGameOptionMenu ();
			}
		}
	}
	public void TakeuserName(string uname) 
	{
		Username = uname.ToString ();
		print ("Username:"+Username);
		print (name);
	}
	public void TakePassword(string pname)
	{
		Password = pname.ToString ();
		print ("Password:"+Password);
	} 
	void LoadGameOptionMenu()
	{
		Anim.SetInteger ("Counter",1);
		StartCoroutine (HoldForoneSec ());
	}
	public void LoadPlayerVSAIScene()
	{
		SceneManager.LoadScene ("PlayerVSAI");
	}
	public void AvtarSelectionMethod()
	{
		print(EventSystem.current.currentSelectedGameObject.name);
		AvatarSelectionValue=int.Parse(EventSystem.current.currentSelectedGameObject.name);
	}

	void OnApplicationQuit()
	{
		string id=PlayerPrefs.GetString ("userid");
		if ((Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) && id.Length>0) {
			print ("Quit the Application When internet connection is There and Logged out");
			StartCoroutine (HitUrl11 (0));
			PlayerPrefs.SetString ("userid", null);
		} else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
			print ("Player is not logged in so no need to Logout");
		}
		else if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			print ("Quitting When There is no internet connection");
		}
	}

}