using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
public class ColorPickerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (ColorAvailableAPI ());
	}
	IEnumerator ColorAvailableAPI(){
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/colorpick?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&roomid="+PlayerPrefs.GetString("roomid")  );
		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			print (www.downloadHandler.text);
		}
	}
	public void FourPlayerGame()
	{
		SceneManager.LoadScene ("FourPlayerGameScene");
	}
	// Update is called once per frame
}
