using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NewSceneScript : MonoBehaviour 
{
	public Canvas can;
	public Button button;
	// Use this for initialization
	void Start () 
	{
		print ("Hello");
	}
	public void Scene2()
	{
		
		can.enabled = false;
		SceneManager.LoadScene (1);
		print ("Loading scene 2");
	}
	public void Scene1()
	{
		SceneManager.LoadScene (0);
		GameObject.Find ("Canvas1").GetComponent<Canvas>().enabled = true;
		print ("Loading scene 1");
	}
	IEnumerator GettingFunctionOnButton()
	{
		yield return new WaitForSeconds (.5f);
	}
	// Update is called once per frame
}
