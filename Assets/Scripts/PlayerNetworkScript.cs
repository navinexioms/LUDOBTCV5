using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkScript : MonoBehaviour 
{
	[SerializeField] private GameObject PlayerCamera;
	[SerializeField] private MonoBehaviour[] PlayerControllerScripts;
	private PhotonView photonView;
	// Use this for initialization
	void Start () 
	{
		photonView = GetComponent<PhotonView> ();
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	private void Initialize()
	{
		if (photonView.IsMine)
		{
			
		}
		else
		{
			PlayerCamera.SetActive (false);
			foreach (MonoBehaviour m in PlayerControllerScripts) 
			{
				m.enabled = false;	
			}	
		}
	}
}
