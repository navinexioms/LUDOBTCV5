using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Photon.Pun.UtilityScripts
{
	public class Random4PlayerConnectionManager : MonoBehaviourPunCallbacks {

	// Use this for initialization
	public static bool isMaster,isRemote,JoinedRoomFlag;
	void Awake()
	{
		DontDestroyOnLoad (this);
	}
	void Start()
	{
		print ("hello");

	}
	// Use this for initialization
	public void CreateRoomMethod()
	{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}
				string PlayerName = PlayerPrefs.GetString ("userid");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			}
	}
	public override void OnConnectedToMaster()
	{
		print ("Conneced to master server:");
		PhotonNetwork.JoinLobby ();
	}
	public override void OnJoinedLobby()
	{
		if (ExtraSceneController.Playmode == 0) {
			print ("Joined lobby");
			PhotonNetwork.CreateRoom ("nsd1", new Photon.Realtime.RoomOptions { 
				MaxPlayers = 4,
				PlayerTtl = 300000, 
				EmptyRoomTtl = 10000 
			}
				, null);
		} 
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
		print (PhotonNetwork.CurrentRoom.Name);
		if (PhotonNetwork.PlayerList.Length == 2) 
		{
			isRemote = true;
		}
		if (!JoinedRoomFlag) {
			SceneManager.LoadScene ("FourPlayerGameScene");
			//				SceneManager.LoadScene("BettingAmountScene");
		}
		JoinedRoomFlag = true;
	}
}
}
