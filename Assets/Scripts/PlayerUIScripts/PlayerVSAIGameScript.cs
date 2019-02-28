using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVSAIGameScript : MonoBehaviour
{
	public GameObject WinPanel;
	public GameObject LossPanel;

	private int totalBlueInHouse, totalGreenInHouse;

	public int OccuranceOfSix;
	public int TrialOFTossing;

	public GameObject BlueFrame, GreenFrame;

	public GameObject BluePlayerI_Border,BluePlayerII_Border,BluePlayerIII_Border,BluePlayerIV_Border;
	public GameObject GreenPlayerI_Border,GreenPlayerII_Border,GreenPlayerIII_Border,GreenPlayerIV_Border;

	public Sprite[] DiceSprite=new Sprite[7];

	public Vector3[] BluePlayers_Pos;
	public Vector3[] GreenPlayers_Pos;

	public Button BluePlayerI_Button, BluePlayerII_Button, BluePlayerIII_Button, BluePlayerIV_Button;
	public Button GreenPlayerI_Button,GreenPlayerII_Button,GreenPlayerIII_Button,GreenPlayerIV_Button;

	public GameObject BlueScreen,GreenScreen;

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

	//----------------Selection of dice number Animation------------------
	private int selectDiceNumAnimation;

	//Players movement corresponding to blocks
	public List<GameObject> blueMovemenBlock=new List<GameObject>();
	public List<GameObject> greenMovementBlock=new List<GameObject>();

	public List<GameObject> BluePlayers=new List<GameObject>();
	public List<GameObject> GreenPlayers=new List<GameObject>();

	private System.Random randomNo;
	public GameObject confirmScreen;

	void DisablingBordersOFBluePlayer()
	{
		BluePlayerI_Border.SetActive (false);
		BluePlayerII_Border.SetActive (false);
		BluePlayerIII_Border.SetActive (false);
		BluePlayerIV_Border.SetActive (false);
	}
	void DisablingButtonsOFBluePlayes()
	{
//		BluePlayerI_Button.interactable = false;
//		BluePlayerII_Button.interactable = false;
//		BluePlayerIII_Button.interactable = false;
//		BluePlayerIV_Button.interactable = false;

		BluePlayerI_Button.enabled = false;
		BluePlayerII_Button.enabled = false;
		BluePlayerIII_Button.enabled = false;
		BluePlayerIV_Button.enabled = false;

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
//		GreenPlayerI_Button.interactable = false;
//		GreenPlayerII_Button.interactable = false;
//		GreenPlayerIII_Button.interactable = false;
//		GreenPlayerIV_Button.interactable = false;

		GreenPlayerI_Button.enabled = false;
		GreenPlayerII_Button.enabled = false;
		GreenPlayerIII_Button.enabled = false;
		GreenPlayerIV_Button.enabled = false;

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

	void InitializeDice()
	{
//		print ("Dice interactable becomes true");
//		print ("Dice interactable becomes true");
		DiceRollButton.interactable = true;
		DiceRollButton.GetComponent<Button> ().enabled = true;

		//==============CHECKING WHO PLAYER WINS SURING===========//
		if (totalBlueInHouse > 3) 
		{
			print ("Player Wins");
			playerTurn = "none";

			WinPanel.SetActive (true);
		}
		if (totalGreenInHouse > 3) 
		{
			print ("AI Wins");
			playerTurn = "none";

			LossPanel.SetActive (true);
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
//			print ("currentPlayerName:" + currentPlayerName);
//			print ("currentPlayer:" + currentPlayer);
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
//			print ("currentPlayerName:" + currentPlayerName);
//			print ("currentPlayer:" + currentPlayer);
		}

		//============PLayer vs Opponent=============//
		if (currentPlayer != "none") 
		{
			int i = 0;
			if (currentPlayerName.Contains ("BLUE PLAYER")) 
			{
				for (i = 0; i < 4; i++) {
//					print ("Blue Player vs GreenPlayer");
					if ((i == 0 && currentPlayer == GreenPlayerI_Script.GreenPlayerI_ColName && (currentPlayer != "Star" && GreenPlayerI_Script.GreenPlayerI_ColName != "Star")) ||
					    (i == 1 && currentPlayer == GreenPlayerII_Script.GreenPlayerII_ColName && (currentPlayer != "Star" && GreenPlayerII_Script.GreenPlayerII_ColName != "Star")) ||
					    (i == 2 && currentPlayer == GreenPlayerIII_Script.GreenPlayerIII_ColName && (currentPlayer != "Star" && GreenPlayerIII_Script.GreenPlayerIII_ColName != "Star")) ||
					    (i == 3 && currentPlayer == GreenPlayerIV_Script.GreenPlayerIV_ColName && (currentPlayer != "Star" && GreenPlayerIV_Script.GreenPlayerIV_ColName != "Star"))) {
						print (" BluePlayer  Beaten GreenPlayerI");
//							GreenPlayerI.transform.position = GreenPlayerI_Pos;
//							GreenPlayerI_Script.GreenPlayerI_ColName = "none";
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
					}
				}
			}

			if (currentPlayerName.Contains ("GREEN PLAYER")) 
			{
//				print ("GreenPlayer VS blue Player");
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
					}
				}
			}
		}

		if (playerTurn == "BLUE") 
		{
			diceRoll.position = BlueDiceRollPosition.position;
			DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
			EnablingBluePlayersRaycast ();
			DisablingGreenPlayerRaycast ();
			BlueFrame.SetActive (true);
			GreenFrame.SetActive (false);
		}
		if (playerTurn == "GREEN") 
		{
			diceRoll.position = GreenDiceRollPosition.position;
			DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
			EnablingGreenPlayerRaycast ();
			DisablingBluePlayersRaycast ();
			BlueFrame.SetActive (false);
			GreenFrame.SetActive (true);
			StartCoroutine (AIMoveTurn ());
		}
			//=================disabling buttons==============//
		DisablingBordersOFBluePlayer();
		DisablingButtonsOFBluePlayes ();
		DisablingBordersOFGreenPlayer ();
		DisablingButtonsOfGreenPlayers ();
		selectDiceNumAnimation = 0;
	}
	IEnumerator AIMoveTurn()
	{
		yield return new WaitForSeconds (1f);
		DiceRoll();
	}

	void AImove()
	{
		int num = Random.Range (1, 5);
//		print ("num:" + num);
		if (num == 1) {
			StartCoroutine (FirstPattern());
		} else if (num == 2) {
			StartCoroutine (SecondPattern());
		} else if (num == 3) {
			StartCoroutine (ThirdPattern());
		} else if (num == 4) {
			StartCoroutine  (FourthPattern());
		}
	}
	IEnumerator FirstPattern()
	{
		yield return new WaitForSeconds (.5f);
		if (GreenPlayerI_Border.activeInHierarchy) {
			GreenPlayerI_UI ();
		} else if (GreenPlayerII_Border.activeInHierarchy) {
			GreenPlayerII_UI ();
		} else if (GreenPlayerIII_Border.activeInHierarchy) {
			GreenPlayerIII_UI();
		} else if (GreenPlayerIV_Border.activeInHierarchy) {
			GreenPlayerIV_UI ();
		}
	}
	IEnumerator SecondPattern()
	{
		yield return new WaitForSeconds (.5f);
		if (GreenPlayerII_Border.activeInHierarchy) {
			GreenPlayerII_UI ();
		} else if ( GreenPlayerIII_Border.activeInHierarchy) {
			GreenPlayerIII_UI ();
		} else if (GreenPlayerI_Border.activeInHierarchy) {
			GreenPlayerI_UI();
		} else if (GreenPlayerIV_Border.activeInHierarchy) {
			GreenPlayerIV_UI ();
		}
	}
	IEnumerator ThirdPattern()
	{
		yield return new WaitForSeconds (.5f);
		if (GreenPlayerIII_Border.activeInHierarchy) {
			GreenPlayerIII_UI ();
		} else if ( GreenPlayerII_Border.activeInHierarchy) {
			GreenPlayerII_UI ();
		} else if (GreenPlayerI_Border.activeInHierarchy) {
			GreenPlayerI_UI();
		} else if (GreenPlayerIV_Border.activeInHierarchy) {
			GreenPlayerIV_UI ();
		}
	}
	IEnumerator FourthPattern()
	{
		yield return new WaitForSeconds (.5f);
		if (GreenPlayerIV_Border.activeInHierarchy) {
			GreenPlayerIV_UI ();
		} else if ( GreenPlayerII_Border.activeInHierarchy) {
			GreenPlayerII_UI ();
		} else if (GreenPlayerI_Border.activeInHierarchy) {
			GreenPlayerI_UI();
		} else if (GreenPlayerIII_Border.activeInHierarchy) {
			GreenPlayerIII_UI ();
		}
	}
	public void DiceRoll()
	{
		DiceRollButton.interactable = false;
		DiceRollButton.GetComponent<Button> ().enabled = false;
		StartCoroutine (DiceToss ());
	}


	//This Function is to avoid Placement of two Pices at the Same position Other Than the safePosition And Teporary Safeposition;
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

	IEnumerator DiceToss()
	{
		TrialOFTossing += 1;
		int randomDice = 0;
		for (int i = 0; i < 8;i++) 
		{
			randomDice = Random.Range (0, 6);
			DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [randomDice];
			yield return new WaitForSeconds(.12f);
		}
		selectDiceNumAnimation = randomDice + 1;

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
//		selectDiceNumAnimation=6;
		StartCoroutine (PlayersNotInitialized ());
	}




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
				BluePlayerI_Button.enabled = false;
			}
			if ((blueMovemenBlock.Count - BluePlayer_Steps [1]) >= selectDiceNumAnimation && BluePlayer_Steps [1] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [1])) {
				BluePlayerII_Border.SetActive (true);
//				BluePlayerII_Button.interactable = true;

			} else {
				BluePlayerII_Border.SetActive (false);
				BluePlayerII_Button.enabled = false;
			}
			if ((blueMovemenBlock.Count - BluePlayer_Steps [2]) >= selectDiceNumAnimation && BluePlayer_Steps [2] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [2])) {
				BluePlayerIII_Border.SetActive (true);
				BluePlayerIII_Button.enabled = true;

			} else {
				BluePlayerIII_Border.SetActive (false);
//				BluePlayerIII_Button.interactable = false;
			}
			if ((blueMovemenBlock.Count - BluePlayer_Steps [3]) >= selectDiceNumAnimation && BluePlayer_Steps [3] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [3])) {
				BluePlayerIV_Border.SetActive (true);
				BluePlayerIV_Button.enabled = true;

			} else {
				BluePlayerIV_Border.SetActive (false);
//				BluePlayerIV_Button.interactable = false;
			}
			//===============Players border glow When Opening===============//

			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [0] == 0) {
				BluePlayerI_Border.SetActive (true);
				BluePlayerI_Button.enabled = true;
			}

			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [1] == 0) {
				BluePlayerII_Border.SetActive (true);
				BluePlayerII_Button.enabled = true;
			}

			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [2] == 0) {
				BluePlayerIII_Border.SetActive (true);
				BluePlayerIII_Button.enabled = true;
			}

			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [3] == 0) {
				BluePlayerIV_Border.SetActive (true);
				BluePlayerIV_Button.enabled = true;
			}

			//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

			if (!BluePlayerI_Border.activeInHierarchy && !BluePlayerII_Border.activeInHierarchy &&
			    !BluePlayerIII_Border.activeInHierarchy && !BluePlayerIV_Border.activeInHierarchy)
			{
				DisablingButtonsOFBluePlayes ();
//				print ("PLAYERS DON'T HAVE OPTION TO MOVE , SWITCH TO NEXT PLAYER TURN");
				playerTurn = "GREEN";
				InitializeDice ();
				//AI passes his turn
				//DiceRoll();
			}
			break;
		case "GREEN":
			//=============condition for border glow=============

			if ((greenMovementBlock.Count - GreenPlayer_Steps [0]) >= selectDiceNumAnimation && GreenPlayer_Steps [0] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [0])) {
				GreenPlayerI_Border.SetActive (true);
				GreenPlayerI_Button.interactable = true;
//				print ("Glowing 1st green Player");
			} else {
				GreenPlayerI_Border.SetActive (false);
				GreenPlayerI_Button.enabled = false;
			}
			if ((greenMovementBlock.Count - GreenPlayer_Steps [1]) >= selectDiceNumAnimation && GreenPlayer_Steps [1] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [1])) {
				GreenPlayerII_Border.SetActive (true);
				GreenPlayerII_Button.enabled = true;
//				print ("Glowing 2nd green Player");

			} else {
				GreenPlayerII_Border.SetActive (false);
				GreenPlayerII_Button.enabled = false;
			}
			if ((greenMovementBlock.Count - GreenPlayer_Steps [2]) >= selectDiceNumAnimation && GreenPlayer_Steps [2] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [2])) {
				GreenPlayerIII_Border.SetActive (true);
				GreenPlayerIII_Button.enabled = true;
//				print ("Glowing 3rd green Player");
			} else {
				GreenPlayerIII_Border.SetActive (false);
				GreenPlayerIII_Button.interactable = false;
			}
			if ((greenMovementBlock.Count - GreenPlayer_Steps [3]) >= selectDiceNumAnimation && GreenPlayer_Steps [3] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [3])) {
				GreenPlayerIV_Border.SetActive (true);
				GreenPlayerIV_Button.enabled = true;
//				print ("Glowing 4th green Player");
			} else {
				GreenPlayerIV_Border.SetActive (false);
				GreenPlayerIV_Button.interactable = false;
			}

			//===============Players border glow When Opening===============//

			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
//				print ("Glowing 1st green Player");
				GreenPlayerI_Border.SetActive (true);
				GreenPlayerI_Button.interactable = true;
			}
			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
//				print ("Glowing 2nd green Player");
				GreenPlayerII_Border.SetActive (true);
				GreenPlayerII_Button.interactable = true;
			}
			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
//				print ("Glowing 3rd green Player");
				GreenPlayerIII_Border.SetActive (true);
				GreenPlayerIII_Button.interactable = true;
			}
			if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
//				print ("Glowing 4th green Player");
				GreenPlayerIV_Border.SetActive (true);
				GreenPlayerIV_Button.interactable = true;
			}
			if (GreenPlayerI_Border.activeInHierarchy || GreenPlayerII_Border.activeInHierarchy || GreenPlayerIII_Border.activeInHierarchy || GreenPlayerIV_Border.activeInHierarchy) 
			{
				AImove ();
			}
			//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

			if (!GreenPlayerI_Border.activeInHierarchy && !GreenPlayerII_Border.activeInHierarchy &&
				!GreenPlayerIII_Border.activeInHierarchy && !GreenPlayerIV_Border.activeInHierarchy) 
			{
				DisablingButtonsOfGreenPlayers ();
				print ("GREEN PLAYER DON'T HAVE OPTION TO MOVE , SWITCH TO NEXT PLAYER TURN");
				playerTurn = "BLUE";
				InitializeDice ();
			}
			break;
		}
	}

	//==================Moving player From Source to Destination=======================//
	void MovingBlueOrGreenPlayer(GameObject Player,Vector3[] path)
	{
		if (path.Length > 1) 
		{
			iTween.MoveTo (Player, iTween.Hash ("path", path, "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
		} 
		else
		{
			iTween.MoveTo (Player, iTween.Hash ("position", path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
		}
	}

	//==================Blue player Movement==================//

	public void BluePlayerI_UI()
	{
		//disabling BluePlayer border and Button script
		DisablingButtonsOFBluePlayes();
		DisablingBordersOFBluePlayer ();

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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [0] == 0) {
					print ("BluePlayerI Player moving its first move");
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
					bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [0]].transform.position;
					BluePlayer_Steps [0] += 1;
					playerTurn = "BLUE";
					currentPlayerName = "BLUE PLAYER I";
					iTween.MoveTo (BluePlayerI, iTween.Hash ("position", bluePlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
	public void BlueplayerII_UI()
	{
		
		DisablingButtonsOFBluePlayes();
		DisablingBordersOFBluePlayer ();

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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [1] == 0) {
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
					bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [1]].transform.position;
					BluePlayer_Steps [1] += 1;
					playerTurn = "BLUE";
					currentPlayerName = "BLUE PLAYER II";
					iTween.MoveTo (BluePlayerII, iTween.Hash ("position", bluePlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
		DisablingButtonsOFBluePlayes();
		DisablingBordersOFBluePlayer ();

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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [2] == 0) {
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
					bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [2]].transform.position;
					BluePlayer_Steps [2] += 1;
					playerTurn = "BLUE";
					currentPlayerName = "BLUE PLAYER III";
					iTween.MoveTo (BluePlayerIII, iTween.Hash ("position", bluePlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
		DisablingButtonsOFBluePlayes();
		DisablingBordersOFBluePlayer ();

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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [3] == 0) {
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
					bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [3]].transform.position;
					BluePlayer_Steps [3] += 1;
					playerTurn = "BLUE";
					currentPlayerName = "BLUE PLAYER IV";
					iTween.MoveTo (BluePlayerIV, iTween.Hash ("position", bluePlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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

	//==================Green player Movement==================//

	public void GreenPlayerI_UI()
	{
		DisablingBordersOFGreenPlayer ();
		DisablingButtonsOfGreenPlayers ();
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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
					print ("Activating 1st green player 1st time");
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
					greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps [0]].transform.position;
					GreenPlayer_Steps [0] += 1;
					playerTurn = "GREEN";
					currentPlayerName = "GREEN PLAYER I";
					iTween.MoveTo (GreenPlayerI, iTween.Hash ("position", greenPlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
		DisablingBordersOFGreenPlayer ();
		DisablingButtonsOfGreenPlayers ();
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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
					print ("Activating 1st green player 1st time");
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
					greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps [1]].transform.position;
					GreenPlayer_Steps [1] += 1;
					playerTurn = "GREEN";
					currentPlayerName = "GREEN PLAYER II";
					iTween.MoveTo (GreenPlayerII, iTween.Hash ("position", greenPlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
		DisablingBordersOFGreenPlayer ();
		DisablingButtonsOfGreenPlayers ();
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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
					print ("Activating 3rd green player 1st time");
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
					greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps [2]].transform.position;
					GreenPlayer_Steps [2] += 1;
					playerTurn = "GREEN";
					currentPlayerName = "GREEN PLAYER III";
					iTween.MoveTo (GreenPlayerIII, iTween.Hash ("position", greenPlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
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
		DisablingBordersOFGreenPlayer ();
		DisablingButtonsOfGreenPlayers ();
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
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
					print ("Activating 1st green player 1st time");
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
					greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps [3]].transform.position;
					GreenPlayer_Steps [3] += 1;
					playerTurn = "GREEN";
					currentPlayerName = "GREEN PLAYER IV";
					iTween.MoveTo (GreenPlayerIV, iTween.Hash ("position", greenPlayer_Path [0], "speed", 125, "time", 2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
					
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


	// Use this for initialization
	void Start () 
	{
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 30;
		randomNo = new System.Random ();

//		print (blueMovemenBlock [0].GetComponent<RectTransform>().TransformVector());
		//Player initial positions...........
		BluePlayers_Pos[0]=BluePlayerI.transform.position;
		BluePlayers_Pos[1] = BluePlayerII.transform.position;
		BluePlayers_Pos[2] = BluePlayerIII.transform.position;
		BluePlayers_Pos[3] = BluePlayerIV.transform.position;

		GreenPlayers_Pos[0] = GreenPlayerI.transform.position;
		GreenPlayers_Pos[1] = GreenPlayerII.transform.position;
		GreenPlayers_Pos[2] = GreenPlayerIII.transform.position;
		GreenPlayers_Pos[3] = GreenPlayerIV.transform.position;

		DisablingBordersOFBluePlayer ();

		DisablingBordersOFGreenPlayer ();

		playerTurn = "BLUE";
		BlueFrame.SetActive (true);
		GreenFrame.SetActive (false);
		DiceRollButton.transform.position = BlueDiceRollPosition.position;
	}
}
