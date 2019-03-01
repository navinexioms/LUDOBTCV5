using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using UnityEngine.SceneManagement;
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
			
			isSelectedGameType = true;
			TwoPlayerGameObject.SetActive (true);
			FourPlayerGameObject.SetActive (false);
			TwoPlayerButton.SetActive (true);
			FourPlayerButton.SetActive (false);
		}
		public void SelectFourPlayerGamePlay()
		{
			print ("Hello");
			StartCoroutine(RoomNameWarning("This feature will be added very soon"));
//			FourPlayerGameObject.SetActive (true);
			TwoPlayerGameObject.SetActive (false);
			isSelectedGameType = true;
			TwoPlayerButton.SetActive (false);
//			FourPlayerButton.SetActive (true);
		}
		public void TakeRoomNameForTwoPlayers()
		{
			if (!isSelectedGameType) {
				StartCoroutine (RoomNameWarning ("Please select the gametype"));
			}else if (TwoPlayerText.text.Length == 0) {
				StartCoroutine (RoomNameWarning ("Please enter the room name"));
			} else if (TwoPlayerText.text.Length > 0) {
				PlayerPrefs.SetString ("roomname", TwoPlayerText.text);
				SceneManager.LoadScene ("BettingAmountFor2PlayerPlayWithFriends");
			}
		}
		public void TakeRoomNameForFourPlayers()
		{
			if (!isSelectedGameType) {
				StartCoroutine (RoomNameWarning ("Please select the gametype"));
			}else if (FourPlayerText.text.Length == 0) {
				StartCoroutine (RoomNameWarning ("Please enter the room name"));
			} else if (FourPlayerText.text.Length > 0) {
				PlayerPrefs.SetString ("roomname", FourPlayerText.text);
				SceneManager.LoadScene ("BettingAmountFor4PlayerPlayWithFriends");
			}
		}

		public IEnumerator RoomNameWarning(string warn)
		{
			WarningText.text = warn;
			yield return new WaitForSeconds (1);
			WarningText.text = null;
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
