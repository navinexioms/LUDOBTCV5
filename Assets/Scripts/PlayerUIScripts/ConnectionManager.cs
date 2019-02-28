using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Pun.Demo.Shared;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Demo.Cockpit
{
	public class ConnectionManager : MonoBehaviourPunCallbacks 
	{
		public Text LobbyNameText;

		private void Start()
		{
			PhotonNetwork.ConnectUsingSettings ();
		}
		public override void OnConnectedToMaster()
		{
			print ("Connected to master");
//			PhotonNetwork.JoinLobby ();
			TypedLobby sqlLobby = new TypedLobby ("2Player", LobbyType.SqlLobby);
			PhotonNetwork.JoinLobby (sqlLobby);
		}
		public override void OnJoinedLobby()
		{
			print ("Joined lobby:");
			LobbyNameText.text = "" + PhotonNetwork.CurrentLobby.Name;
			PhotonNetwork.JoinRandomRoom ();
//			PhotonNetwork.CreateRoom ("nsd", null, null);

		}
		public override void OnJoinRandomFailed (short returnCode, string message)
		{
			base.OnJoinRandomFailed (returnCode, message);
			PhotonNetwork.CreateRoom ("nsd", null, null);
		}
		public override void OnCreatedRoom()
		{
			print ("Room Created");
		}
		public override void OnJoinedRoom()
		{
			print ("Joined room:");
			print (PhotonNetwork.CurrentRoom.Name);
//			PhotonNetwork.
		}
	}
}
