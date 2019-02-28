using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class newScriptss : MonoBehaviour {

	// Use this for initialization
	public void Scene1()
	{
		GameObject.Find ("GameObject").GetComponent<DND> ().enabled = false;
		StartCoroutine (Wait1 ());

	}
	IEnumerator Wait1()
	{
		yield return new WaitForSeconds (.5f);
		SceneManager.LoadScene (0);
	}
}
