using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using SimpleJSON;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
	public class FourPlayerGameLogic : MonoBehaviourPunCallbacks,IPunTurnManagerCallbacks 
	{


		private PunTurnManager turnManager;

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
		public Image ImageThree;
		public Image ImageFour;

		public Vector3 TimerOnePosition;
		public Vector3 TimerTwoPosition;
		public Vector3 TimerThreePosition;
		public Vector3 TimerFourPosition;

		private int totalBlueInHouse, totalGreenInHouse;

		public GameObject BlueFrame,RedFrame ,GreenFrame, YellowFrame;

		public GameObject BluePlayerI_Border,BluePlayerII_Border,BluePlayerIII_Border,BluePlayerIV_Border;
		public GameObject GreenPlayerI_Border,GreenPlayerII_Border,GreenPlayerIII_Border,GreenPlayerIV_Border;
		public GameObject RedPlayerI_Border,RedPlayerII_Border,RedPlayerIII_Border,RedPlayerIV_Border;
		public GameObject YellowPlayerI_Border, YellowPlayerII_Border, YellowPlayerIII_Border, YellowPlayerIV_Border;

		public Sprite[] DiceSprite=new Sprite[7];

		public Vector3[] BluePlayers_Pos;
		public Vector3[] GreenPlayers_Pos;
		public Vector3[] RedPlayers_Pos;
		public Vector3[] YellowPlayers_Pos;


		public Button BluePlayerI_Button, BluePlayerII_Button, BluePlayerIII_Button, BluePlayerIV_Button;
		public Button GreenPlayerI_Button,GreenPlayerII_Button,GreenPlayerIII_Button,GreenPlayerIV_Button;
		public Button RedPlayerI_Button,RedPlayerII_Button,RedPlayerIII_Button,RedPlayerIV_Button;
		public Button YellowPlayerI_Button,YellowPlayerII_Button,YellowPlayerIII_Button,YellowPlayerIV_Button;

		public string playerTurn="BLUE";

		public Transform diceRoll;

		public Button DiceRollButton;

		public Transform BlueDiceRollPosition, GreenDiceRollPosition,RedDiceRollPosition,YellowDicePosition;

		private string currentPlayer="none";
		private string currentPlayerName = "none";

		public GameObject BluePlayerI, BluePlayerII, BluePlayerIII, BluePlayerIV;
		public GameObject GreenPlayerI, GreenPlayerII, GreenPlayerIII, GreenPlayerIV;
		public GameObject RedPlayerI, RedPlayerII, RedPlayerIII, RedPlayerIV;
		public GameObject YellowPlayerI,YellowPlayerII,YellowPlayerIII,YellowPlayerIV;


		public List<int> BluePlayer_Steps=new List<int>();
		public List<int> GreenPlayer_Steps=new List<int>();
		public List<int> RedPlayer_Steps=new List<int>();
		public List<int> YellowPlayer_Steps=new List<int>();

		//----------------Selection of dice number Animation------------------
		private int selectDiceNumAnimation;

		//Players movement corresponding to blocks
		public List<GameObject> blueMovemenBlock=new List<GameObject>();
		public List<GameObject> greenMovementBlock=new List<GameObject>();
		public List<GameObject> redMovementBlock = new List<GameObject> ();
		public List<GameObject> yellowMovementBlock = new List<GameObject> ();


		public List<GameObject> BluePlayers=new List<GameObject>();
		public List<GameObject> GreenPlayers=new List<GameObject>();
		public List<GameObject> RedPlayers = new List<GameObject> ();
		public List<GameObject> YellowPlayers = new List<GameObject> ();

		private System.Random randomNo;

		public bool isMyTurn;
		public bool PlayerCanPlayAgain;
		public bool ActualPlayerCanPlayAgain;

		void Start () 
		{
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 5000 ;
			GameObject OneOnOneConnectionManagerController = GameObject.Find ("SceneSWitchController");
			this.turnManager = this.gameObject.AddComponent<PunTurnManager> ();
			this.turnManager.TurnManagerListener = this;
			string name = null;
			if(PhotonNetwork.InRoom)
				name=PhotonNetwork.CurrentRoom.Name;
			print (name);

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

			RedPlayers_Pos [0] = RedPlayerI.transform.position;
			RedPlayers_Pos [1] = RedPlayerII.transform.position;
			RedPlayers_Pos [2] = RedPlayerIII.transform.position;
			RedPlayers_Pos [3] = RedPlayerIV.transform.position;

			YellowPlayers_Pos [0] = YellowPlayerI.transform.position;
			YellowPlayers_Pos [1] = YellowPlayerII.transform.position;
			YellowPlayers_Pos [2] = YellowPlayerIII.transform.position;
			YellowPlayers_Pos [3] = YellowPlayerIV.transform.position;



			DisablingBordersOFBluePlayer ();

			DisablingBordersOFGreenPlayer ();

			DisablingBorderOfYellowPlayer ();

			DisablingBorderOrRedPlayer ();

			playerTurn = "YELLOW";
			BlueFrame.SetActive (false);
			GreenFrame.SetActive (false);

			TimerImage.transform.position = TimerOnePosition;
			diceRoll.position = YellowDicePosition.position;
	
			switch(PhotonNetwork.CurrentRoom.PlayerCount)
			{
			case 1:
				print ("First player Entered Room");
				DisablingBluePlayer ();
				DisablingRedPlayer ();
				DisablingGreenPlayer ();
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive(false);
				DisconnectText.text="WAIT TILL THE OTHER PLAYER TO CONNECT";
				break;
			case 2:
				EnablingBluePlayer ();
				DisablingRedPlayer ();
				DisablingGreenPlayer ();
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive(false);
				DisconnectText.text="WAIT TILL THE OTHER TO PLAYER CONNECT";
				break;
			case 3:
				EnablingBluePlayer ();
				EnablingRedPlayer ();
				DisablingGreenPlayer ();
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive(false);
				DisconnectText.text="WAIT TILL THE OTHER TO PLAYER CONNECT";
				break;
			case 4:
				DisconnectPanel.SetActive (false);
				DisconnectText.text = null;
				YellowFrame.SetActive (true);
				EnablingBluePlayer ();
				EnablingRedPlayer ();
				EnablingGreenPlayer ();
				break;
			}
		}

		void Update()
		{
			print(PhotonNetwork.CurrentRoom.PlayerCount);
		}
		void DisablingBluePlayer()
		{
			BluePlayerI.SetActive (false);
			BluePlayerII.SetActive (false);
			BluePlayerIII.SetActive (false);
			BluePlayerIV.SetActive (false);
		}

		void DisablingRedPlayer()
		{
			RedPlayerI.SetActive (false);
			RedPlayerII.SetActive (false);
			RedPlayerIII.SetActive (false);
			RedPlayerIV.SetActive (false);
		}

		void DisablingGreenPlayer()
		{
			GreenPlayerI.SetActive (false);
			GreenPlayerII.SetActive (false);
			GreenPlayerIII.SetActive (false);
			GreenPlayerIV.SetActive (false);
		}

		void EnablingBluePlayer()
		{
			BluePlayerI.SetActive (true);
			BluePlayerII.SetActive (true);
			BluePlayerIII.SetActive (true);
			BluePlayerIV.SetActive (true);
		}

		void EnablingRedPlayer()
		{
			RedPlayerI.SetActive (true);
			RedPlayerII.SetActive (true);
			RedPlayerIV.SetActive (true);
			RedPlayerIV.SetActive (true);
		}

		void EnablingGreenPlayer()
		{
			GreenPlayerI.SetActive (true);
			GreenPlayerII.SetActive (true);
			GreenPlayerIII.SetActive (true);
			GreenPlayerIV.SetActive (true);
		}

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

		void DisablingBorderOfYellowPlayer()
		{
			YellowPlayerI_Border.SetActive (false);
			YellowPlayerII_Border.SetActive (false);
			YellowPlayerIII_Border.SetActive (false);
			YellowPlayerIV_Border.SetActive (false);
		}
		void DisablingBorderOrRedPlayer()
		{
			RedPlayerI_Border.SetActive (false);
			RedPlayerII_Border.SetActive (false);
			RedPlayerIII_Border.SetActive (false);
			RedPlayerIV_Border.SetActive (false);
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
		void EnableFrameAndBorderForFirstTime()
		{
			GreenPlayerI.SetActive (true);
			GreenPlayerII.SetActive (true);
			GreenPlayerIII.SetActive (true);
			GreenPlayerIV.SetActive (true);

			BlueFrame.SetActive (true);
		}
		public void OnTurnBegins(int turn)
		{}
		public void OnTurnCompleted(int turn)
		{}
		public void OnPlayerMove(Player player, int turn, object move)
		{}
		public void OnPlayerFinished(Player player, int turn, object move)
		{}
		public void OnTurnTimeEnds(int turn)
		{}
		public override void OnPlayerEnteredRoom (Player newPlayer)
		{
			switch(PhotonNetwork.CurrentRoom.PlayerCount)
			{
			case 1:
				DisablingBluePlayer ();
				DisablingRedPlayer ();
				DisablingGreenPlayer ();
				break;
			case 2:
				print ("Second player Entered Room");
				EnablingBluePlayer ();
				DisablingRedPlayer ();
				DisablingGreenPlayer ();
				break;
			case 3:
				print ("Third player Entered Room");
				EnablingBluePlayer ();
				EnablingRedPlayer ();
				DisablingGreenPlayer ();
				break;
			case 4:
				print ("Fourth player Entered Room");
				YellowFrame.SetActive (true);
				EnablingBluePlayer ();
				EnablingRedPlayer ();
				EnablingGreenPlayer ();
				if (this.turnManager.Turn == 0) {
					isMyTurn = true;
				}
				break;
			}
		}
	}
}
