using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
public class ShowTransactionScript : MonoBehaviour {
	public GameObject RowData;
	public GameObject ParentObj;
	public GameObject ParentObj1;
	// Use this for initialization
	void Start () {
		StartCoroutine (HitTransactionApi ());
	}
	IEnumerator HitTransactionApi(){
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1"); 
		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			string msg = www.downloadHandler.text;
			print (msg);
			msg = msg.Substring (1, msg.Length - 2);
			print (msg);
			msg = msg.Insert (0, "[");
			msg = msg.Insert (msg.Length , "]");
			print (msg);
			JSONNode jn = SimpleJSON.JSONData.Parse (msg);

			print (jn);
			int num = 0;
			foreach (JSONNode jn1 in jn.Childs) {
				print (jn1);
				print (jn1[0]+" "+jn[1]+" "+jn[2] );

				GameObject data = Instantiate (RowData, ParentObj.transform.position, ParentObj.transform.rotation,ParentObj1.transform);
				data.transform.localScale = ParentObj.transform.localScale;
				data.transform.GetComponent<Text> ().text ="   "+ jn1 [0].Value+"  "+jn[1].Value+" "+jn[2].Value;

			}
		}
	}
	// Update is called once per frame
}
