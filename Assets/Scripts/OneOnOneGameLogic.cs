using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using SimpleJSON;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
namespace Photon.Pun.UtilityScripts
{
	public class OneOnOneGameLogic :  MonoBehaviourPunCallbacks,IPunTurnManagerCallbacks
	{
		public string ThisLobbyName=null;
		public string ThisRoomName = null;
		public Text Countdown;

		public int TimePeriod;
		public int TimePeriodTriggeredTime;
		bool isTimeOut;

		public GameObject QuitPanel;

		private PunTurnManager turnManager;

		public int OccuranceOfSix;
		public int TrialOFTossing;

		public int TriggeredTime;
		public int TriggerCounter;
		public int timer;
		public int playerCounter;

		public int TriggeredTime2;
		public int TriggerCounter2;
		public int timer2;

		public int ImageFillingCounter;

		public GameObject DisconnectPanel;
		public GameObject WinPanel, LosePanel;

		public Text DisconnectText;
		public Text DisconnectText1;
		public GameObject ReconnectButton;


		JSONNode rootNode=new JSONClass();
		JSONNode childNode = new JSONClass ();
		JSONNode BlankTurn1 = new JSONClass ();
		JSONNode BlankTurn2=new JSONClass();

		public Image TimerImage;

		public Image ImageOne;
		public Image ImageTwo;

		public Vector3 TimerOnePosition;
		public Vector3 TimerTwoPosition;

		public GameObject GameOver;
		public Text GameOverText;
		private int totalBlueInHouse, totalGreenInHouse;

		public GameObject BlueFrame, GreenFrame;

		public GameObject BluePlayerI_Border,BluePlayerII_Border,BluePlayerIII_Border,BluePlayerIV_Border;
		public GameObject GreenPlayerI_Border,GreenPlayerII_Border,GreenPlayerIII_Border,GreenPlayerIV_Border;

		public Sprite[] DiceSprite=new Sprite[7];

		public Vector3[] BluePlayers_Pos;
		public Vector3[] GreenPlayers_Pos;

		public Button BluePlayerI_Button, BluePlayerII_Button, BluePlayerIII_Button, BluePlayerIV_Button;
		public Button GreenPlayerI_Button,GreenPlayerII_Button,GreenPlayerIII_Button,GreenPlayerIV_Button;
	
		public string playerTurn="BLUE";

		public Transform diceRoll;

		public Button DiceRollButton;

		public Transform BlueDiceRollPosition, GreenDiceRollPosition;

		private string currentPlayer="none";
		private string currentPlayerName = "none";

		public GameObject BluePlayerI, BluePlayerII, BluePlayerIII, BluePlayerIV;
		public GameObject GreenPlayerI, GreenPlayerII, GreenPlayerIII, GreenPlayerIV;

		public List<int> BluePlayer_Steps=new List<int>();
		public List<int> GreenPlayer_Steps=new List<int>();

		private int bluePlayerI_Steps,bluePlayerII_Steps,bluePlayerIII_Steps,bluePlayerIV_Steps;
		private int GreenPlayerI_Steps,GreenPlayer_StepsII,GreenPlayerIII_Steps,GreenPlayerIV_Steps;

		//----------------Selection of dice number Animation------------------
		private int selectDiceNumAnimation;

		//Players movement corresponding to blocks
		public List<GameObject> blueMovemenBlock=new List<GameObject>();
		public List<GameObject> greenMovementBlock=new List<GameObject>();

		public List<GameObject> BluePlayers=new List<GameObject>();
		public List<GameObject> GreenPlayers=new List<GameObject>();

		private System.Random randomNo;

		public bool isMyTurn;
		public bool PlayerCanPlayAgain;
		public bool ActualPlayerCanPlayAgain;
		private bool IsPressed=false;





		//==========================In this section initial setup is done when player Enter in BoardRoom==========================// 
		void Start () 
		{
			print ("http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&intWalletType=1&dblamt=" + ThisLobbyName + "&gametype=2&roomid=" + PhotonNetwork.CurrentRoom.Name + "&date=" + System.DateTime.Now.ToString ("d") + "&playercolor=" + 2 + "&playerid=" + 2); 
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 1000 ;
			GameObject OneOnOneConnectionManagerController = GameObject.Find ("SceneSWitchController");
			this.turnManager = this.gameObject.AddComponent<PunTurnManager> ();
			this.turnManager.TurnManagerListener = this;
			string name = null;
			if(PhotonNetwork.InRoom)
				name=PhotonNetwork.CurrentRoom.Name;
			ThisRoomName = name;
			ThisLobbyName = PhotonNetwork.CurrentLobby.Name;
			print (ThisLobbyName);
			print (ThisRoomName);

			QualitySettings.vSyncCount = 1;
			Application.targetFrameRate = 30;
			randomNo = new System.Random ();

			TimerOnePosition = ImageOne.transform.position;
			TimerTwoPosition = ImageTwo.transform.position;

			//Player initial positions...........
			BluePlayers_Pos[0]=BluePlayerI.transform.position;
			BluePlayers_Pos[1] = BluePlayerII.transform.position;
			BluePlayers_Pos[2] = BluePlayerIII.transform.position;
			BluePlayers_Pos[3] = BluePlayerIV.transform.position;

			GreenPlayers_Pos[0] = GreenPlayerI.transform.position;
			GreenPlayers_Pos[1] = GreenPlayerII.transform.position;
			GreenPlayers_Pos[2] = GreenPlayerIII.transform.position;
			GreenPlayers_Pos[3] = GreenPlayerIV.transform.position;

			if (PhotonNetwork.PlayerList.Length == 1) 
			{
				GreenPlayerI.SetActive (false);
				GreenPlayerII.SetActive (false);
				GreenPlayerIII.SetActive (false);
				GreenPlayerIV.SetActive (false);
			}

			DisablingBordersOFBluePlayer ();

			DisablingBordersOFGreenPlayer ();

			playerTurn = "BLUE";
			BlueFrame.SetActive (false);
			GreenFrame.SetActive (false);

			TimerImage.transform.position = TimerOnePosition;
			diceRoll.position = BlueDiceRollPosition.position;

			if (PhotonNetwork.PlayerList.Length == 2) {
				EnableFrameAndBorderForFirstTime ();
				ImageFillingCounter = 1;
			} else if (PhotonNetwork.PlayerList.Length == 1) {
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive(false);
				DisconnectText.text="WAIT TILL THE COUNTDOWN TO CONNECT OTHER PLAYER";
			}
		}

		//============================This method is called One Per frame============================//

		void Update()
		{		
			RoomCanBeDiscard ();
			if (Input.GetKeyDown (KeyCode.Escape) ) {
				print ("Pressing Escape");
				QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 1);
			}
			if (!turnManager.CancelGame) {
				if (!PhotonNetwork.InRoom) {
					return;
				}

				SendBlankTurn ();

				TimerLogicWhenIsMyTurn ();

			} else {
				print ("Player connected to the room properly, Game Can't be Continue");
			}
			DisableAllButtons ();
		}

		void RoomCanBeDiscard(){
			if (!isTimeOut && PhotonNetwork.CurrentRoom.PlayerCount == 1) {
				if (TimePeriodTriggeredTime == 0) {
					TimePeriodTriggeredTime = (int)Time.time;
				}
				if (TimePeriod < 60) {
					TimePeriod = (int)(Time.time - TimePeriodTriggeredTime);
					Countdown.text = "" + (60 - TimePeriod);
				}
				if (TimePeriod == 60 && !isTimeOut) {
					//call the room discard coroutine
					isTimeOut=true;
					StartCoroutine(RoomDiscard());
				}
			}
		}
		//http://apienjoybtc.exioms.me/api/Home/discardroom?struserid=2&strgamesessionid=1&roomid=4dsfvcdwsdvgerfgregr&intstatus=3  
		IEnumerator RoomDiscard()
		{
			print("RoomDiscard()");
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Home/discardroom?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid="+1+"&roomid="+PhotonNetwork.CurrentRoom.Name+"&intstatus="+3);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went Wrong");
			} else {
				string msg = www.downloadHandler.text;
				print( www.downloadHandler.text);
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				msg = jn [0];
				print (msg);
				if (msg.Equals ("Discardroomorsuccessfullyplayedgame")) {
					
					StartCoroutine (AutoDisconnectFromRoom ());
				
				}
			}
		}
		IEnumerator AutoDisconnectFromRoom(){
			DisconnectText.text = "OTHER PLAYER IS NOT CONNECTED WITHIN TIME SO, QUITING THIS GAME";
			yield return new WaitForSeconds (2);
			QuitTheRoom ();
		}
		//=====================Enable the Quit Button=====================//
		public void EnableQuitTheRoom()
		{
			QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 1);
		}
		//=====================Disable the Quit Button=====================//
		public void CancelQuiting()
		{
			QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 2);
		}
		//=====================Quit the Room and direct to Dashboard=====================//
		public void QuitTheRoom()
		{
			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.LeaveLobby ();
			PhotonNetwork.Disconnect ();

			StartCoroutine (QuitTheRoom1 ());
		}

		IEnumerator QuitTheRoom1()
		{
			yield return new WaitForSeconds (.5f);
			SceneManager.LoadScene ("GameMenu");

			Destroy (GameObject.Find("SceneSwitchController"));

			Destroy (this.gameObject);
		}
		//====================Disable All the Buttons When is MyTurn is False======================//
		void DisableAllButtons()
		{
			if (isMyTurn){
				TriggerCounter2 = 0;
				TriggeredTime2 = 0;
				timer2 = 0;
				DiceRollButton.interactable = true;
			}
			if (!isMyTurn) {
				DiceRollButton.interactable = false;
				BluePlayerI_Button.enabled = false;
				BluePlayerII_Button.enabled = false;
				BluePlayerIII_Button.enabled = false;
				BluePlayerIV_Button.enabled = false;

				GreenPlayerI_Button.enabled = false;
				GreenPlayerII_Button.enabled = false;
				GreenPlayerIII_Button.enabled = false;
				GreenPlayerIV_Button.enabled = false;
			}
		}

		//==========================Sends the blank turn==========================//
		void SendBlankTurn()
		{
			if (ActualPlayerCanPlayAgain == true && isMyTurn == true && !PhotonNetwork.IsMasterClient) {
				print ("Sending Blank Turn" + " isMyTurn" + isMyTurn);
				ActualPlayerCanPlayAgain = false;
				BlankTurn1.Add ("None", "" + 0);
				temp = BlankTurn1.ToString ();
				this.MakeTurn (temp);
			}
		}
		void TimerLogicWhenIsMyTurn()
		{
			if (isMyTurn && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				if (TriggerCounter < 1) 
				{
					TriggerCounter += 1;
					TriggeredTime = (int)Time.time;
					TimerImage.fillAmount = 1;
				}
				if (TriggerCounter == 1 && timer!=31) 
				{
					TimerImage.fillAmount -= 1.0f / 30 * Time.deltaTime;
					timer = (int)Time.time - TriggeredTime;
				}
				if (TimerImage.fillAmount == 0 || timer==31) {
					print ("Filling Image");
					string msg = null;
					if (PhotonNetwork.IsMasterClient) {
						msg = "Master";
					} else {
						msg = "Remote";
					}
					BlankTurn2.Add ("None1", msg);
					temp = BlankTurn2.ToString ();
					this.MakeTurn (temp);
				}
			}
			else if (!isMyTurn  && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				if (TriggerCounter2 < 1 && ImageFillingCounter == 1) {
					ImageFillingCounter = 2;
					TriggerCounter2 += 1;
					TriggeredTime2 = (int)Time.time;
				}
				if (TriggerCounter2 == 1 && timer2 != 31) {
					TimerImage.fillAmount -= 1.0f / 30 * Time.deltaTime;
					timer2 = (int)Time.time - TriggeredTime2;
				}
			}
			if (!isMyTurn && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				if (TriggerCounter2 < 1) {
					TriggerCounter2 += 1;
					TriggeredTime2 = (int)Time.time;
					TriggeredTime2 += 2;
				}
				if (timer2 != 3) {
					timer2 = (int)(Time.time - TriggeredTime2);
				} else if (TimerImage.fillAmount != 0) {
					TimerImage.fillAmount -= 1.0f / 30 * Time.deltaTime;
				}
			}
		}

		/// <summary>
		//Disable and Enable Blueplayers Pices and GreenPlayers Pices
		/// </summary>
		void DisablingBordersOFBluePlayer()
		{
			BluePlayerI_Border.SetActive (false);
			BluePlayerII_Border.SetActive (false);
			BluePlayerIII_Border.SetActive (false);
			BluePlayerIV_Border.SetActive (false);
		}
		void DisablingButtonsOFBluePlayes()
		{
			BluePlayerI_Button.interactable = false;
			BluePlayerII_Button.interactable = false;
			BluePlayerIII_Button.interactable = false;
			BluePlayerIV_Button.interactable = false;
		}

		void DisablingBordersOFGreenPlayer ()
		{
			GreenPlayerI_Border.SetActive (false);
			GreenPlayerII_Border.SetActive (false);
			GreenPlayerIII_Border.SetActive (false);
			GreenPlayerIV_Border.SetActive (false);
		}

		void DisablingButtonsOfGreenPlayers()
		{
			GreenPlayerI_Button.interactable = false;
			GreenPlayerII_Button.interactable = false;
			GreenPlayerIII_Button.interactable = false;
			GreenPlayerIV_Button.interactable = false;	
		}

		void DisablingBluePlayersRaycast()
		{
			BluePlayerI_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerII_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerIII_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerIV_Button.GetComponent<Image> ().raycastTarget = false;
		}

		void EnablingBluePlayersRaycast()
		{
			BluePlayerI_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerII_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerIII_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerIV_Button.GetComponent<Image> ().raycastTarget = true;
		}

		void DisablingGreenPlayerRaycast()
		{
			GreenPlayerI_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerII_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerIII_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerIV_Button.GetComponent<Image> ().raycastTarget = false;
		}

		void EnablingGreenPlayerRaycast()
		{
			GreenPlayerI_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerII_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerIII_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerIV_Button.GetComponent<Image> ().raycastTarget = true;
		}


		//=====================Checks the Repetation of 6 if occures 3 times then changes by any number within 1-5==================//
		void CheckToChangePossibilityOfTwoPlayerATSamePosition ()
		{
			if (playerTurn == "BLUE") {
				if (BluePlayer_Steps [0] > 0 &&
					((BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [2]) || (BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [0] + selectDiceNumAnimation != 9 && BluePlayer_Steps [0] + selectDiceNumAnimation != 14 && BluePlayer_Steps [0] + selectDiceNumAnimation != 22 && BluePlayer_Steps [0] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [0] + selectDiceNumAnimation != 35 && BluePlayer_Steps [0] + selectDiceNumAnimation != 40 && BluePlayer_Steps [0] + selectDiceNumAnimation != 48 && BluePlayer_Steps [0] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [1] > 0
					&& ((BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [2]) || (BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [1] + selectDiceNumAnimation != 9 && BluePlayer_Steps [1] + selectDiceNumAnimation != 14 && BluePlayer_Steps [1] + selectDiceNumAnimation != 22 && BluePlayer_Steps [1] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [1] + selectDiceNumAnimation != 35 && BluePlayer_Steps [1] + selectDiceNumAnimation != 40 && BluePlayer_Steps [1] + selectDiceNumAnimation != 48 && BluePlayer_Steps [1] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [2] > 0 &&
					((BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [2] + selectDiceNumAnimation != 9 && BluePlayer_Steps [2] + selectDiceNumAnimation != 14 && BluePlayer_Steps [2] + selectDiceNumAnimation != 22 && BluePlayer_Steps [2] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [2] + selectDiceNumAnimation != 35 && BluePlayer_Steps [2] + selectDiceNumAnimation != 40 && BluePlayer_Steps [2] + selectDiceNumAnimation != 48 && BluePlayer_Steps [2] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [3] > 0 &&
					((BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [2]))) {
					if (BluePlayer_Steps [3] + selectDiceNumAnimation != 9 && BluePlayer_Steps [3] + selectDiceNumAnimation != 14 && BluePlayer_Steps [3] + selectDiceNumAnimation != 22 && BluePlayer_Steps [3] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [3] + selectDiceNumAnimation != 35 && BluePlayer_Steps [3] + selectDiceNumAnimation != 40 && BluePlayer_Steps [3] + selectDiceNumAnimation != 48 && BluePlayer_Steps [3] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				}
			}
			if (playerTurn == "GREEN") {
				if (GreenPlayer_Steps [0] > 0 &&
					((GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [2]) || (GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [0] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [0] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 48 && BluePlayer_Steps [0] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [1] > 0 &&
					((GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [2]) || (GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [1] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [1] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 48 && BluePlayer_Steps [0] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]){
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [2] > 0 &&
					((GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [2] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [2] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 48 && BluePlayer_Steps [0] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [3] > 0 &&
					((GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [2]))) {
					if (GreenPlayer_Steps [3] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [3] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 48 && BluePlayer_Steps [0] + selectDiceNumAnimation != 57) {
						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				}
			}
		}


		//===========================Rolls The Dice======================//
		public void DiceRoll()
		{
			if (isMyTurn) {
				print ("Hello");
				DiceRollButton.interactable = false;
				DiceRollButton.GetComponent<Button> ().enabled = false;
				selectDiceNumAnimation = Random.Range (0, 6);
				selectDiceNumAnimation += 1;

				CheckToChangePossibilityOfTwoPlayerATSamePosition ();

				if ((selectDiceNumAnimation == 6 ) && TrialOFTossing < 4) {

					OccuranceOfSix += 1;
					if (OccuranceOfSix == 1) {
						print ("One Time got 1 or 6");
					}
					if (OccuranceOfSix == 2) {
						print ("Two Times got 1 or 6");
					}
					if (OccuranceOfSix == 3) {
						print ("Occurance of six or One three times");
						selectDiceNumAnimation = Random.Range (1, 6);
						print ("Changed Number And Got" + selectDiceNumAnimation);
						DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation-1];
						TrialOFTossing = 0;
						OccuranceOfSix = 0;
					}
				} else {
					print ("TrialOFTossing" + TrialOFTossing + " Reset TrialOFTossing");
					TrialOFTossing = 0;
					OccuranceOfSix = 0;
				}

				print ("selectDiceNumAnimation:" + selectDiceNumAnimation);
				rootNode.Add ("DiceNumber", selectDiceNumAnimation.ToString(temp));
				temp = rootNode.ToString ();

				StartCoroutine (PlayersNotInitializedForTimeDelay (temp));
			}
		}

		//====================Checks Which player can move before sending the data=====================//
		IEnumerator PlayersNotInitializedForTimeDelay(string temp)
		{
			yield return new WaitForSeconds (0);
			switch (playerTurn) 
			{
			case "BLUE":
				//=============condition for border glow=============

				if ((blueMovemenBlock.Count - BluePlayer_Steps [0]) >= selectDiceNumAnimation && BluePlayer_Steps [0] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [0])) {
					BluePlayerI_Border.SetActive (true);
					BluePlayerI_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					BluePlayerI_Border.SetActive (false);
					BluePlayerI_Button.interactable = false;

				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [1]) >= selectDiceNumAnimation && BluePlayer_Steps [1] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [1])) {
					BluePlayerII_Border.SetActive (true);
					BluePlayerII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					BluePlayerII_Border.SetActive (false);
					BluePlayerII_Button.interactable = false;


				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [2]) >= selectDiceNumAnimation && BluePlayer_Steps [2] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [2])) {
					BluePlayerIII_Border.SetActive (true);
					BluePlayerIII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					BluePlayerIII_Border.SetActive (false);
					BluePlayerIII_Button.interactable = false;

				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [3]) >= selectDiceNumAnimation && BluePlayer_Steps [3] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [3])) {
					BluePlayerIV_Border.SetActive (true);
					BluePlayerIV_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
					print ("not Skipping");
				} else {
					BluePlayerIV_Border.SetActive (false);
					BluePlayerIV_Button.interactable = false;

				}
				//===============Players border glow When Opening===============//
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (BluePlayer_Steps [0] == 0 || BluePlayer_Steps [1] == 0 || BluePlayer_Steps [2] == 0 || BluePlayer_Steps [3] == 0)) {
					print ("Players border glow When Opening");
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [0] == 0) {
						BluePlayerI_Border.SetActive (true);
						BluePlayerI_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [1] == 0) {
						BluePlayerII_Border.SetActive (true);
						BluePlayerII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [2] == 0) {
						BluePlayerIII_Border.SetActive (true);
						BluePlayerIII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [3] == 0) {
						BluePlayerIV_Border.SetActive (true);
						BluePlayerIV_Button.interactable = true;
					}
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Added:" + temp);
				}	
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!BluePlayerI_Border.activeInHierarchy && !BluePlayerII_Border.activeInHierarchy &&
					!BluePlayerIII_Border.activeInHierarchy && !BluePlayerIV_Border.activeInHierarchy) {
					rootNode.Add ("WaitingTime", "DontSkip");
					temp = rootNode.ToString ();
					print ("not Skipping");
				}

				BluePlayerI_Border.SetActive (false);
				BluePlayerI_Button.interactable = false;

				BluePlayerII_Border.SetActive (false);
				BluePlayerII_Button.interactable = false;

				BluePlayerIII_Border.SetActive (false);
				BluePlayerIII_Button.interactable = false;

				BluePlayerIV_Border.SetActive (false);
				BluePlayerIV_Button.interactable = false;

				break;
			case "GREEN":
				//=============condition for border glow=============

				if ((greenMovementBlock.Count - GreenPlayer_Steps [0]) >= selectDiceNumAnimation && GreenPlayer_Steps [0] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [0])) {
					GreenPlayerI_Border.SetActive (true);
					GreenPlayerI_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					GreenPlayerI_Border.SetActive (false);
					GreenPlayerI_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [1]) >= selectDiceNumAnimation && GreenPlayer_Steps [1] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [1])) {
					GreenPlayerII_Border.SetActive (true);
					GreenPlayerII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					GreenPlayerII_Border.SetActive (false);
					GreenPlayerII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [2]) >= selectDiceNumAnimation && GreenPlayer_Steps [2] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [2])) {
					GreenPlayerIII_Border.SetActive (true);
					GreenPlayerIII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					GreenPlayerIII_Border.SetActive (false);
					GreenPlayerIII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [3]) >= selectDiceNumAnimation && GreenPlayer_Steps [3] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [3])) {
					GreenPlayerIV_Border.SetActive (true);
					GreenPlayerIV_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
					print ("Border glowing");
				} else {
					GreenPlayerIV_Border.SetActive (false);
					GreenPlayerIV_Button.interactable = false;
				}

				//===============Players border glow When Opening===============//

				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (GreenPlayer_Steps [0] == 0 || GreenPlayer_Steps [1] == 0 || GreenPlayer_Steps [2] == 0 || GreenPlayer_Steps [3] == 0)) {
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
						GreenPlayerI_Border.SetActive (true);
						GreenPlayerI_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
						GreenPlayerII_Border.SetActive (true);
						GreenPlayerII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
						GreenPlayerIII_Border.SetActive (true);
						GreenPlayerIII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
						GreenPlayerIV_Border.SetActive (true);
						GreenPlayerIV_Button.interactable = true;
					}
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
				}
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!GreenPlayerI_Border.activeInHierarchy && !GreenPlayerII_Border.activeInHierarchy &&
					!GreenPlayerIII_Border.activeInHierarchy && !GreenPlayerIV_Border.activeInHierarchy && selectDiceNumAnimation!=6) 
				{
					rootNode.Add ("WaitingTime", "DontSkip");
					temp = rootNode.ToString ();
					print ("not Skipping");
				}

				GreenPlayerI_Border.SetActive (false);
				GreenPlayerI_Button.interactable = false;

				GreenPlayerII_Border.SetActive (false);
				GreenPlayerII_Button.interactable = false;

				GreenPlayerIII_Border.SetActive (false);
				GreenPlayerIII_Button.interactable = false;

				GreenPlayerIV_Border.SetActive (false);
				GreenPlayerIV_Button.interactable = false;

				break;
			}
			this.MakeTurn (temp);
		}
			
		IEnumerator DiceToss(int DiceNumber)
		{
			int randomDice = 0;
			for (int i = 0; i < 8;i++) 
			{
				randomDice = Random.Range (0, 6);
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [randomDice];
				yield return new WaitForSeconds(.12f);
			}
			DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [DiceNumber-1];
			selectDiceNumAnimation = DiceNumber;

			StartCoroutine (PlayersNotInitialized ());
		}
		//====================Checks Which player can move After sending the data=====================//
		IEnumerator PlayersNotInitialized()
		{
			yield return new WaitForSeconds (.8f);
			//game start initial position of each player(green and blue)
			switch (playerTurn) 
			{
			case "BLUE":
				//=============condition for border glow=============

				if ((blueMovemenBlock.Count - BluePlayer_Steps [0]) >= selectDiceNumAnimation && BluePlayer_Steps [0] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [0])) {
					BluePlayerI_Border.SetActive (true);
					BluePlayerI_Button.interactable = true;
				} else {
					BluePlayerI_Border.SetActive (false);
					BluePlayerI_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [1]) >= selectDiceNumAnimation && BluePlayer_Steps [1] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [1])) {
					BluePlayerII_Border.SetActive (true);
					BluePlayerII_Button.interactable = true;
				} else {
					BluePlayerII_Border.SetActive (false);
					BluePlayerII_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [2]) >= selectDiceNumAnimation && BluePlayer_Steps [2] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [2])) {
					BluePlayerIII_Border.SetActive (true);
					BluePlayerIII_Button.interactable = true;
				} else {
					BluePlayerIII_Border.SetActive (false);
					BluePlayerIII_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [3]) >= selectDiceNumAnimation && BluePlayer_Steps [3] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [3])) {
					BluePlayerIV_Border.SetActive (true);
					BluePlayerIV_Button.interactable = true;
				} else {
					BluePlayerIV_Border.SetActive (false);
					BluePlayerIV_Button.interactable = false;
				}
				//===============Players border glow When Opening===============//
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && (BluePlayer_Steps [0] == 0 || BluePlayer_Steps [1] == 0 || BluePlayer_Steps [2] == 0 || BluePlayer_Steps [3] == 0)) {
					print ("Players border glow When Opening");
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [0] == 0) {
						BluePlayerI_Border.SetActive (true);
						BluePlayerI_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [1] == 0) {
						BluePlayerII_Border.SetActive (true);
						BluePlayerII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [2] == 0) {
						BluePlayerIII_Border.SetActive (true);
						BluePlayerIII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [3] == 0) {
						BluePlayerIV_Border.SetActive (true);
						BluePlayerIV_Button.interactable = true;
					}
				}	
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!BluePlayerI_Border.activeInHierarchy && !BluePlayerII_Border.activeInHierarchy &&
					!BluePlayerIII_Border.activeInHierarchy && !BluePlayerIV_Border.activeInHierarchy)
				{
					DisablingButtonsOFBluePlayes ();
					playerTurn = "GREEN";
					InitializeDice ();
				}
				break;
			case "GREEN":
				//=============condition for border glow=============

				if ((greenMovementBlock.Count - GreenPlayer_Steps [0]) >= selectDiceNumAnimation && GreenPlayer_Steps [0] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [0])) {
					GreenPlayerI_Border.SetActive (true);
					GreenPlayerI_Button.interactable = true;
					//	PlayerCanPlayAgain = true;

					print ("Glowing 1st green Player");
				} else {
					GreenPlayerI_Border.SetActive (false);
					GreenPlayerI_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [1]) >= selectDiceNumAnimation && GreenPlayer_Steps [1] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [1])) {
					GreenPlayerII_Border.SetActive (true);
					GreenPlayerII_Button.interactable = true;
					print ("Glowing 2nd green Player");
				} else {
					GreenPlayerII_Border.SetActive (false);
					GreenPlayerII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [2]) >= selectDiceNumAnimation && GreenPlayer_Steps [2] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [2])) {
					GreenPlayerIII_Border.SetActive (true);
					GreenPlayerIII_Button.interactable = true;
					print ("Glowing 3rd green Player");
				} else {
					GreenPlayerIII_Border.SetActive (false);
					GreenPlayerIII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [3]) >= selectDiceNumAnimation && GreenPlayer_Steps [3] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [3])) {
					GreenPlayerIV_Border.SetActive (true);
					GreenPlayerIV_Button.interactable = true;
					print ("Glowing 4th green Player");
				} else {
					GreenPlayerIV_Border.SetActive (false);
					GreenPlayerIV_Button.interactable = false;
				}

				//===============Players border glow When Opening===============//

				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (GreenPlayer_Steps [0] == 0 || GreenPlayer_Steps [1] == 0 || GreenPlayer_Steps [2] == 0 || GreenPlayer_Steps [3] == 0)) {
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
						print ("Glowing 1st green Player");
						GreenPlayerI_Border.SetActive (true);
						GreenPlayerI_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
						print ("Glowing 2nd green Player");
						GreenPlayerII_Border.SetActive (true);
						GreenPlayerII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
						print ("Glowing 3rd green Player");
						GreenPlayerIII_Border.SetActive (true);
						GreenPlayerIII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
						print ("Glowing 4th green Player");
						GreenPlayerIV_Border.SetActive (true);
						GreenPlayerIV_Button.interactable = true;
					}
				}
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!GreenPlayerI_Border.activeInHierarchy && !GreenPlayerII_Border.activeInHierarchy &&
					!GreenPlayerIII_Border.activeInHierarchy && !GreenPlayerIV_Border.activeInHierarchy && selectDiceNumAnimation!=6) 
				{
					DisablingButtonsOfGreenPlayers ();
					print ("GREEN PLAYER DON'T HAVE OPTION TO MOVE , SWITCH TO NEXT PLAYER TURN");
					playerTurn = "BLUE";
					InitializeDice ();
				}
				break;
			}
		}

		//============================All Blue Pices Moving Method==============================//
		public void BluePlayerI_UI()
		{
			print ("BluePlayerI_UI()");
			//disabling BluePlayer border and Button script
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [0]) > selectDiceNumAnimation) 
			{
				if (BluePlayer_Steps [0] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [0] + i].transform.position;
					}
					BluePlayer_Steps [0] += selectDiceNumAnimation;
					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";	
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}
					currentPlayerName = "BLUE PLAYER I";
					MovingBlueOrGreenPlayer (BluePlayerI, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [0] == 0)
					{
						print ("BluePlayerI Player moving its first move");
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [0]].transform.position;
						BluePlayer_Steps [0] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER I";
						iTween.MoveTo (BluePlayerI, iTween.Hash ("position", bluePlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [0]) == selectDiceNumAnimation)
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [0] + i].transform.position;
					}

					BluePlayer_Steps [0] += selectDiceNumAnimation;
					playerTurn = "BLUE";
					MovingBlueOrGreenPlayer (BluePlayerI, bluePlayer_Path);
					totalBlueInHouse += 1;
					print ("Cool");
					BluePlayerI_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [0]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [1] + BluePlayer_Steps [2] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
		}
		public void BluePlayerII_UI()
		{
			print ("BluePlayerII_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [1]) > selectDiceNumAnimation)
			{
				if (BluePlayer_Steps [1] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [1] + i].transform.position;
					}

					BluePlayer_Steps [1] += selectDiceNumAnimation;
					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER II";
					MovingBlueOrGreenPlayer (BluePlayerII, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [1] == 0)
					{
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [1]].transform.position;
						BluePlayer_Steps [1] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER II";
						iTween.MoveTo (BluePlayerII, iTween.Hash ("position", bluePlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [1]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [1] + i].transform.position;
					}

					BluePlayer_Steps [1] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					MovingBlueOrGreenPlayer (BluePlayerII, bluePlayer_Path);
					totalBlueInHouse += 1;
					print ("Cool");
					BluePlayerII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [1]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [2] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
		}
		public  void BluePlayerIII_UI()
		{
			print ("BluePlayerIII_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [2]) > selectDiceNumAnimation) 
			{
				if (BluePlayer_Steps [2] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [2] + i].transform.position;
					}

					BluePlayer_Steps [2] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER III";
					MovingBlueOrGreenPlayer (BluePlayerIII, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [2] == 0)
					{
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [2]].transform.position;
						BluePlayer_Steps [2] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER III";
						iTween.MoveTo (BluePlayerIII, iTween.Hash ("position", bluePlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;

					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [2]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [2] + i].transform.position;
					}

					BluePlayer_Steps [2] += selectDiceNumAnimation;

					playerTurn = "BLUE";
					MovingBlueOrGreenPlayer(BluePlayerIII,bluePlayer_Path);
					totalBlueInHouse += 1;
					print ("Cool");
					BluePlayerIII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [2]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [1] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
		}
		public void BluePlayerIV_UI()
		{
			print ("BluePlayerIV_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [3]) > selectDiceNumAnimation) 
			{
				if (BluePlayer_Steps [3] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [3] + i].transform.position;
					}

					BluePlayer_Steps [3] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER IV";

					MovingBlueOrGreenPlayer (BluePlayerIV, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [3] == 0)
					{
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [3]].transform.position;
						BluePlayer_Steps [3] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER IV";
						iTween.MoveTo (BluePlayerIV, iTween.Hash ("position", bluePlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [3]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [3] + i].transform.position;
					}

					BluePlayer_Steps [3] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					MovingBlueOrGreenPlayer (BluePlayerIV, bluePlayer_Path);
					totalBlueInHouse += 1;
					print ("Cool");
					BluePlayerIV_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [3]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [1] + BluePlayer_Steps [2] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
		}

		//==================Green player Movement Method==================//

		public void GreenPlayerI_UI()
		{
			print ("Executing GreenPlayerI_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[0]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[0] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[0] + i].transform.position;
					}

					GreenPlayer_Steps[0] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER I";

					MovingBlueOrGreenPlayer (GreenPlayerI, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[0] == 0) 
					{
						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[0]].transform.position;
						GreenPlayer_Steps[0] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER I";
						iTween.MoveTo (GreenPlayerI, iTween.Hash ("position", greenPlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[0]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[0] + i].transform.position;
					}

					GreenPlayer_Steps[0] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					MovingBlueOrGreenPlayer (GreenPlayerI, greenPlayer_Path);
					totalGreenInHouse += 1;
					print ("Cool");
					GreenPlayerI_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[0]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[1] + GreenPlayer_Steps[2] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
		}
		public void GreenPlayerII_UI()
		{
			print ("Executing GreenPlayerII_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[1]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[1] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[1] + i].transform.position;
					}

					GreenPlayer_Steps[1] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER II";

					MovingBlueOrGreenPlayer (GreenPlayerII, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[1] == 0) 
					{
						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[1]].transform.position;
						GreenPlayer_Steps[1] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER II";
						iTween.MoveTo (GreenPlayerII, iTween.Hash ("position", greenPlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[1]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[1] + i].transform.position;
					}

					GreenPlayer_Steps[1] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					MovingBlueOrGreenPlayer (GreenPlayerII, greenPlayer_Path);
					totalGreenInHouse += 1;
					print ("Cool");
					GreenPlayerII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[1]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[2] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
		}
		public  void GreenPlayerIII_UI()
		{
			print ("Executing GreenPlayerIII_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[2]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[2] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[2] + i].transform.position;
					}

					GreenPlayer_Steps[2] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						print ("Keeping the Turn to blue Players side");
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER III";

					MovingBlueOrGreenPlayer (GreenPlayerIII, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[2] == 0) 
					{
						print ("Activating 3rd green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[2]].transform.position;
						GreenPlayer_Steps[2] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER III";
						iTween.MoveTo (GreenPlayerIII, iTween.Hash ("position", greenPlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[2]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[2] + i].transform.position;
					}

					GreenPlayer_Steps[2] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					MovingBlueOrGreenPlayer (GreenPlayerIII, greenPlayer_Path);
					totalGreenInHouse += 1;
					print ("Cool");
					GreenPlayerIV_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[2]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[1] + GreenPlayer_Steps[2] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
		}
		public void GreenPlayerIV_UI()
		{
			print ("Executing GreenPlayerIV_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[3]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[3] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[3] + i].transform.position;
					}

					GreenPlayer_Steps[3] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER IV";

					MovingBlueOrGreenPlayer (GreenPlayerIV, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[3] == 0) 
					{
						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[3]].transform.position;
						GreenPlayer_Steps[3] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER IV";
						iTween.MoveTo (GreenPlayerIV, iTween.Hash ("position", greenPlayer_Path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[3]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[3] + i].transform.position;
					}

					GreenPlayer_Steps[3] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					MovingBlueOrGreenPlayer (GreenPlayerIV, greenPlayer_Path);
					totalGreenInHouse += 1;
					print ("Cool");
					GreenPlayerIV_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[3]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[1] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
		}
		public string temp;

		public void BluePlayerI_UI_Method()
		{
			//this method calls the BluePlayerI_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("BLUE")) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerI_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerII_UI_Method()
		{
			//this method calls the BluePlayerII_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("BLUE")) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerIII_UI_Method()
		{
			//this method calls the BluePlayerIII_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("BLUE")) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerIII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerIV_UI_Method()
		{
			//this method calls the BluePlayerIV_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("BLUE")) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerIV_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}

		public void GreenPlayerI_UI_Method()
		{
			//this method calls the GreenPlayerI_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("GREEN")) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerI_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerII_UI_Method()
		{
			//this method calls the GreenPlayerII_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("GREEN")) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerIII_UI_Method()
		{
			//this method calls the GreenPlayerIII_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("GREEN")) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerIII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerIV_UI_Method()
		{
			//this method calls the GreenPlayerIV_UI() by sending the RandomNumber and MethodName;
			if (isMyTurn && playerTurn.Equals ("GREEN")) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerIV_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		void MovingBlueOrGreenPlayer(GameObject Player,Vector3[] path)
		{
			if (path.Length > 1) 
			{
				iTween.MoveTo (Player, iTween.Hash ("path", path, "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
			} 
			else
			{
				iTween.MoveTo (Player, iTween.Hash ("position", path [0], "speed", 400, "time", 0.5f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
			}
		}



		//============================Method to Beat the Player==============================//
		void InitializeDice()
		{
			print ("InitializeDice()");
			//	print ("Dice interactable becomes true");
			//	print ("Dice interactable becomes true");
			DiceRollButton.interactable = true;
			DiceRollButton.GetComponent<Button> ().enabled = true;

			//==============CHECKING WHO PLAYER WINS SURING===========//
			if (totalBlueInHouse > 3) 
			{
				print ("Player1 Wins");
				playerTurn = "none";
				if (PhotonNetwork.IsMasterClient) {
					WinPanel.SetActive (true);
					StartCoroutine(WinnerAPI("2","2"));

				} else {
					LosePanel.SetActive (true);
					StartCoroutine (LosserAPI ("2", "2"));
				}
			}
			if (totalGreenInHouse > 3) 
			{
				print ("Player2 Wins");
				playerTurn = "none";
				if (!PhotonNetwork.IsMasterClient) {
					WinPanel.SetActive (true);
					StartCoroutine(WinnerAPI("4","4"));
				} else {
					LosePanel.SetActive (true);
					StartCoroutine (LosserAPI ("4", "4"));
				}
			}

			//=============getting currentPlayer Value===========//
			if (currentPlayerName.Contains ("BLUE PLAYER")) 
			{
				if (currentPlayerName == "BLUE PLAYER I") 
					currentPlayer = BluePlayerI_Script.BluePlayerI_ColName;
				if (currentPlayerName == "BLUE PLAYER II") 
					currentPlayer = BluePlayerII_Script.BluePlayerII_ColName;
				if (currentPlayerName == "BLUE PLAYER III") 
					currentPlayer = BluePlayerIII_Script.BluePlayerIII_ColName;
				if (currentPlayerName == "BLUE PLAYER IV") 
					currentPlayer = BluePlayerIV_Script.BluePlayerIV_ColName;
			}
			if (currentPlayerName.Contains ("GREEN PLAYER"))
			{
				if (currentPlayerName == "GREEN PLAYER I") 
					currentPlayer = GreenPlayerI_Script.GreenPlayerI_ColName;
				if (currentPlayerName == "GREEN PLAYER II") 
					currentPlayer = GreenPlayerII_Script.GreenPlayerII_ColName;
				if (currentPlayerName == "GREEN PLAYER III") 
					currentPlayer = GreenPlayerIII_Script.GreenPlayerIII_ColName;
				if (currentPlayerName == "GREEN PLAYER IV") 
					currentPlayer = GreenPlayerIV_Script.GreenPlayerIV_ColName;
			}

			//============PLayer vs Opponent=============//
			if (currentPlayer != "none") 
			{
				int i = 0;
				if (currentPlayerName.Contains ("BLUE PLAYER")) 
				{
					for (i = 0; i < 4; i++) {
						if ((i == 0 && currentPlayer == GreenPlayerI_Script.GreenPlayerI_ColName && (currentPlayer != "Star" && GreenPlayerI_Script.GreenPlayerI_ColName != "Star")) ||
							(i == 1 && currentPlayer == GreenPlayerII_Script.GreenPlayerII_ColName && (currentPlayer != "Star" && GreenPlayerII_Script.GreenPlayerII_ColName != "Star")) ||
							(i == 2 && currentPlayer == GreenPlayerIII_Script.GreenPlayerIII_ColName && (currentPlayer != "Star" && GreenPlayerIII_Script.GreenPlayerIII_ColName != "Star")) ||
							(i == 3 && currentPlayer == GreenPlayerIV_Script.GreenPlayerIV_ColName && (currentPlayer != "Star" && GreenPlayerIV_Script.GreenPlayerIV_ColName != "Star"))) {
							print (" BluePlayer  Beaten GreenPlayerI");
							GreenPlayers [i].transform.position = GreenPlayers_Pos [i];
							GreenPlayer_Steps [i] = 0;
							playerTurn = "BLUE";
							if (i == 0) {
								GreenPlayerI_Script.GreenPlayerI_ColName = "none";
							} else if (i == 1) {
								GreenPlayerII_Script.GreenPlayerII_ColName = "none";	
							} else if (i == 2) {
								GreenPlayerIII_Script.GreenPlayerIII_ColName = "none";	
							} else if (i == 3) {
								GreenPlayerIV_Script.GreenPlayerIV_ColName = "none";
							}
							if (PhotonNetwork.IsMasterClient) {

							}
							else 
							{
								PlayerCanPlayAgain = true;
							}
						}
					}
				}

				if (currentPlayerName.Contains ("GREEN PLAYER")) 
				{
					i = 0;
					for (i = 0; i < 4; i++) {
						if ((i == 0 && currentPlayer == BluePlayerI_Script.BluePlayerI_ColName && (currentPlayer != "Star" && BluePlayerI_Script.BluePlayerI_ColName != "Star")) ||
							(i == 1 && currentPlayer == BluePlayerII_Script.BluePlayerII_ColName && (currentPlayer != "Star" && BluePlayerII_Script.BluePlayerII_ColName != "Star")) ||
							(i == 2 && currentPlayer == BluePlayerIII_Script.BluePlayerIII_ColName && (currentPlayer != "Star" && BluePlayerIII_Script.BluePlayerIII_ColName != "Star")) ||
							(i == 3 && currentPlayer == BluePlayerIV_Script.BluePlayerIV_ColName && (currentPlayer != "Star" && BluePlayerIV_Script.BluePlayerIV_ColName != "Star"))) {
							print (" GreenPlayer  Beaten BluePlayerI");
							BluePlayers [i].transform.position = BluePlayers_Pos [i];
							if (i == 0) {
								BluePlayerI_Script.BluePlayerI_ColName = "none";
							} else if (i == 1) {
								BluePlayerII_Script.BluePlayerII_ColName = "none";
							} else if (i == 2) {
								BluePlayerIII_Script.BluePlayerIII_ColName = "none";
							} else if (i == 3) {
								BluePlayerIV_Script.BluePlayerIV_ColName = "none";
							}
							BluePlayer_Steps [i] = 0;
							playerTurn = "GREEN";
							if (PhotonNetwork.IsMasterClient) {
								PlayerCanPlayAgain = true;
							}
							else 
							{

							}
						}
					}
				}
			}

			if (playerTurn == "BLUE") 
			{
				diceRoll.position = BlueDiceRollPosition.position;
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				TimerImage.transform.position = TimerOnePosition;
				EnablingBluePlayersRaycast ();
				DisablingGreenPlayerRaycast ();
				BlueFrame.SetActive (true);
				GreenFrame.SetActive (false);
				TimerImage.fillAmount = 1;
			}
			if (playerTurn == "GREEN") 
			{
				diceRoll.position = GreenDiceRollPosition.position;
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				TimerImage.transform.position = TimerTwoPosition;
				EnablingGreenPlayerRaycast ();
				DisablingBluePlayersRaycast ();
				BlueFrame.SetActive (false);
				GreenFrame.SetActive (true);
			}
			//=================disabling buttons==============//
			DisablingBordersOFBluePlayer();
			DisablingButtonsOFBluePlayes ();
			DisablingBordersOFGreenPlayer ();
			DisablingButtonsOfGreenPlayers ();
			selectDiceNumAnimation = 0;
			TimerImage.fillAmount = 1;
		}
		//http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid=2&gamesessionid=1&intWalletType=1&dblamt=100&gametype=2&roomid=2PLDO1&date=27/02/2019&playercolor=1&playerid=1  
		//Amount added in wallet
		IEnumerator WinnerAPI(string playerid,string playerColor)
		{
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&intWalletType=1&dblamt="+ThisLobbyName+"&gametype=2&roomid="+PhotonNetwork.CurrentRoom.Name+"&date="+System.DateTime.Now.ToString ("d")+"&playercolor="+playerColor+"&playerid="+playerid);
			www.chunkedTransfer=false;
			www.downloadHandler=new DownloadHandlerBuffer();
			yield return www.SendWebRequest();
			if(www.error!=null){
				print("Something went Wrong");
			}else{
				print(www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				string msg2 = jn [0];
				print (msg2);
			}
		}
		//http://apienjoybtc.exioms.me/api/Balance/gameloss?userid=2&gamesessionid=1&intWalletType=2&dblamt=100&gametype=2&roomid=2PLDO1&date=27/02/2019&playercolor=1&playerid=1
		IEnumerator LosserAPI(string playerid,string playerColor){
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gameloss?userid="+PlayerPrefs.GetString("userid")+"&intWalletType=2&dblamt="+PhotonNetwork.CurrentLobby.Name+"&gametype=2&roomid="+PhotonNetwork.CurrentRoom.Name+"&date="+System.DateTime.Now.ToString ("d")+"&playercolor="+playerColor+"&playerid="+playerColor);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went wrong " + www.error);
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				msg = jn [0];
				if (msg.Equals ("Recordadded")) {
					print (msg);
				}
			}
		}
		public void MakeTurn(string data)
		{
			string temp1 = null;

			temp = data;

			this.turnManager.SendMove (data as object, true);
		}

		#region IPunTurnManagerCallbacks
		public void OnTurnBegins(int turn)
		{
			print ("OnTurnBegins(int turn)");
			if (ActualPlayerCanPlayAgain == true && isMyTurn==true ) 
			{
//				print ("Sending Blank Turn" + " isMyTurn" + isMyTurn);
//				ActualPlayerCanPlayAgain = false;
//				BlankTurn1.Add ("None", ""+0);
//				temp = BlankTurn1.ToString ();
//				this.MakeTurn (temp);
				SendBlankTurn();
			}
		}
		public void OnTurnCompleted(int turn)
		{}
		public void OnPlayerMove(Player player, int turn, object move)
		{}
		public void OnPlayerFinished(Player player, int turn, object move)
		{

			print ("OnPlayerFinished(Player player, int turn, object move) PlayerName:"+player.NickName);
			temp = move as string;

			print ("Temp" + temp);
			if (temp.Contains ("DiceNumber"))
			{
				JSONNode jn = SimpleJSON.JSONData.Parse (temp);
				print (jn [0]);
				int Place = int.Parse (jn ["DiceNumber"].Value);
				print ("Sending DiceNumber");
				StartCoroutine (DiceToss (Place));
			}

			//=====================for BluePlayer====================
			TriggeredTime = 0;
			TriggerCounter = 0;
			timer = 0;
			TimerImage.fillAmount = 1;
			JSONNode jn1 = SimpleJSON.JSONData.Parse (temp);
			if (jn1 [0].Value.Equals ("Master") ) {
				TimerImage.transform.position = TimerTwoPosition;
				diceRoll.position = GreenDiceRollPosition.position;
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				playerTurn = "GREEN";
				GreenFrame.SetActive (true);
				BlueFrame.SetActive (false);
				DisablingBordersOFBluePlayer ();
				if (!PhotonNetwork.IsMasterClient) {
					DiceRollButton.enabled = true;
					DiceRollButton.interactable = true;
				} else {
					DiceRollButton.interactable = false;
				}
			} 
			if (jn1 [0].Value.Equals ("Remote") ) {
				TimerImage.transform.position = TimerOnePosition;
				diceRoll.position = BlueDiceRollPosition.position;
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				playerTurn = "BLUE";
				BlueFrame.SetActive (true);
				GreenFrame.SetActive (false);
				DisablingBordersOFGreenPlayer ();
				if (PhotonNetwork.IsMasterClient) {
					DiceRollButton.enabled = true;
					DiceRollButton.interactable = true;
				} else {
					DiceRollButton.interactable = false;
				}
			}
			string temp1 = temp;
			temp = null;
			int Place1=0;
			print (jn1 [0]);
			switch (jn1 [0]) 
			{

			case "BluePlayerI_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerI_UI ();
				break;
			case "BluePlayerII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerII_UI ();
				break;
			case "BluePlayerIII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerIII_UI ();
				break;
			case "BluePlayerIV_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerIV_UI ();
				break;
			case "GreenPlayerI_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerI_UI ();
				break;
			case "GreenPlayerII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerII_UI ();
				break;
			case "GreenPlayerIII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerIII_UI ();
				break;
			case "GreenPlayerIV_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerIV_UI ();
				break;
			}

			if (player.IsLocal) 
			{
				print ("(player.IsLocal) ");
				isMyTurn = false;
				PlayerCanPlayAgain = false;
			}
			else
			{
				StartCoroutine (WaitForSomeTime (temp1));

			}
		}

		IEnumerator WaitForSomeTime(string temp1)
		{
			print ("StartCoroutine (WaitForSomeTime ())");
			print ("PlayerCanPlayAgain:" + PlayerCanPlayAgain);
			JSONNode jn = SimpleJSON.JSONData.Parse (temp1);


			string WhatToDo = jn ["WaitingTime"].Value;
			print(jn ["WaitingTime"].Value);
			float WaitingTime = 0;
			if (jn ["WaitingTime"].Value.Equals("Skip")) {
				print ("Skipping turn");
				ActualPlayerCanPlayAgain = true;
				WaitingTime = 0;
				WhatToDo = null;
			}
			else 
			{
				WaitingTime = .5f;
			}
			if (jn ["Wait"].Value.Equals ("2.5")) 
			{
				WaitingTime = 2.5f;
			}
			if (PlayerCanPlayAgain == true) 
			{
				ActualPlayerCanPlayAgain = true;
				PlayerCanPlayAgain = false;
			}
			yield return new WaitForSeconds (WaitingTime);
			isMyTurn = true;
			if (PlayerCanPlayAgain == true) 
			{
				ActualPlayerCanPlayAgain = true;
				PlayerCanPlayAgain = false;
			}
			if (PhotonNetwork.IsMasterClient)
			{
				StartTurn ();
			}
		}
		public void OnTurnTimeEnds(int turn)
		{

		}
		#endregion
		public void StartTurn()
		{
			print ("StartTurn000");

			if (PhotonNetwork.IsMasterClient)
			{
				print ("StartTurn1111");

				this.turnManager.BeginTurn();

			}
		}
		void EnableFrameAndBorderForFirstTime()
		{
			GreenPlayerI.SetActive (true);
			GreenPlayerII.SetActive (true);
			GreenPlayerIII.SetActive (true);
			GreenPlayerIV.SetActive (true);

			BlueFrame.SetActive (true);
		}
		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Debug.Log("Other player arrived turn = "+ this.turnManager.Turn );
			print ("PlayerCounter:" + PhotonNetwork.CountOfPlayersInRooms);
			playerCounter += 1;
			if (PhotonNetwork.PlayerList.Length == 2) {
				ImageFillingCounter = 1;
				if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient) {
					isMyTurn = true;
				}
				ReconnectButton.SetActive (false);
				DisconnectText.text=null;
				DisconnectPanel.SetActive (false);
				if (this.turnManager.Turn == 0) {
					isMyTurn = true;
				}
				EnableFrameAndBorderForFirstTime ();
			}
		}

		public void OnClickReconnectAndReJoin()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
				print ("internet connection");
				PhotonNetwork.ReconnectAndRejoin ();
			}
			else if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				print ("no Internet connection is there");
				StartCoroutine (NoInternetConnectionWarning ());
			}
			print ("Attempting to rejoin the room");
		}
		IEnumerator NoInternetConnectionWarning()
		{
			DisconnectText.text = "PLEASE CONNECT TO INTERNET AND THEN CLICK THE BUTTON:";
			yield return new WaitForSeconds (1);
			DisconnectText.text ="DISCONNECTED FROM THE ROOM CLICK THE BUTTON TO REENTER THE ROOM";
		}
		public override void OnLeftRoom ()
		{
			print ("Player left the room");
			//	base.OnLeftRoom ();
		}

		public override void OnJoinedRoom ()
		{
			print ("Joined Room");
			DisconnectText.text = null;
			ReconnectButton.SetActive (false);
			DisconnectPanel.SetActive (false);
			playerCounter += 1;
			if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient) {
					isMyTurn=true;
				} else if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient) {
					isMyTurn = true;
				}
			}
		}

		void OnServerConnect()
		{
			print ("Connected");
		}

		void OnServerDisconnect()
		{
			print ("disconnected");
		}

		public override void OnPlayerLeftRoom (Photon.Realtime.Player otherPlayer)
		{	
			print ("Player Disconnected");
			playerCounter -= 1;
			TriggeredTime = 0;
			TriggerCounter = 0;
			timer = 0;
			TimerImage.fillAmount = 1;
			DisconnectPanel.SetActive (true);
			ReconnectButton.SetActive (false);
			DisconnectText.text = null;
			DisconnectText.text = "OTHER PLAYER IS DISCONNECTED, WAIT FOR A WHILE TO RECONNECT OTHER PLAYER";
		}

		public override void OnDisconnected (DisconnectCause cause)
		{
			//Enable Reconnection Panel and Reset all the all the values of image and counters
			if (!isTimeOut) {
				playerCounter -= 1;
				TriggeredTime = 0;
				TriggerCounter = 0;
				timer = 0;
				TimerImage.fillAmount = 1;
				DisconnectPanel.SetActive (true);
				DisconnectText.text = "DISCONNECTED FROM THE ROOM CLICK THE BUTTON TO REENTER THE ROOM";
				ReconnectButton.SetActive (true);
			}	
		}
	}
}
