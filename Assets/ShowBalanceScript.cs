using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ShowBalanceScript : MonoBehaviour {
	public Text Balance;
	// Use this for initialization
	void Start () {
		StartCoroutine (HitBalanceApi ());
	}
	IEnumerator HitBalanceApi()
	{
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/getbalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1");
		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			print (www.downloadHandler.text);
			string msg = www.downloadHandler.text;
			msg = msg.Substring (1, msg.Length - 2);
			JSONNode jn = SimpleJSON.JSONData.Parse (msg);
			Balance.text = jn [0];
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
