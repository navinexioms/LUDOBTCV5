using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
namespace Photon.Pun.UtilityScripts
{
	public class PlayWithFriendSceneManager : Photon.Pun.MonoBehaviourPun
	{

		public Toggle TwoPlayerToggle, FourPlayerToggle;
		public GameObject TwoPlayerGameObject, FourPlayerGameObject,TwoPlayerButton,FourPlayerButton;
		public static bool isSelectedGameType;
		private TPPWFConnectionManager TPPWF;
		private FPPWFConnectionManager FPPWF;
		public Text TwoPlayerText;
		public Text FourPlayerText;
		public Text WarningText;
		public void SelectTwoPlayerGamePlay()
		{
			Destroy (this.GetComponent<FPPWFConnectionManager> ());
			if (this.GetComponent<TPPWFConnectionManager> () == null) {
				this.gameObject.AddComponent<TPPWFConnectionManager> ();
				TPPWF = this.GetComponent<TPPWFConnectionManager> ();
				Button btn = TwoPlayerButton.GetComponent<Button> ();
				btn.onClick.AddListener(() =>TPPWF.CreateOrJoinRoomMethod());
				TPPWF.RoomName = TwoPlayerText;
				TPPWF.WarningText = WarningText;
			}
			TwoPlayerGameObject.SetActive (TwoPlayerToggle.isOn);
			isSelectedGameType = true;
			FourPlayerGameObject.SetActive (false);
			FourPlayerButton.SetActive (false);
			TwoPlayerButton.SetActive (true);
		}
		public void SelectFourPlayerGamePlay()
		{
			Destroy (this.GetComponent<TPPWFConnectionManager> ());
			if (this.GetComponent<FPPWFConnectionManager> () == null) {
				this.gameObject.AddComponent<FPPWFConnectionManager> ();
				FPPWF = this.GetComponent<FPPWFConnectionManager> ();
				Button btn = FourPlayerButton.GetComponent<Button> ();
				btn.onClick.AddListener (() => FPPWF.CreateOrJoinRoomMethod());
				FPPWF.WarningText = WarningText;
				FPPWF.RoomName = FourPlayerText;
			}
			TwoPlayerGameObject.SetActive (false);
			isSelectedGameType = true;
			FourPlayerGameObject.SetActive (FourPlayerToggle.isOn);
			FourPlayerButton.SetActive (true);
			TwoPlayerButton.SetActive (false);
		}
		public void TakeRoomNameForTwoPlayers()
		{
			
		}
		public void TakeRoomNameForFourPlayers()
		{
			
		}

		//==============Method to Invite friend and Create Room======================//
		public void InviteFriend()
		{
			
		}

		//=============Method to Join Game=================//
		public void JoinGame()
		{
			
		}
		// Use this for initialization
		void Start () 
		{
		
		}
	}
}
