using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInformation : MonoBehaviour {
	public GameObject SizeForTwoPices;
	public GameObject SizeForThreePices;
	public GameObject SizeForFourPices;
	public GameObject SizeForFivePices;
	public GameObject SizeForSixPices;
	public GameObject ActualSize;
	public List<GameObject> PicesContains;
	public int Number=0;
	public bool Flag;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		Flag = false;
		PicesContains.Add (col.gameObject);
		if (PicesContains.Count >1) {
			StartCoroutine (ChangePosition ());
		}
	}
	IEnumerator ChangePosition()
	{
		yield return new WaitForSeconds (.5f);
		if (PicesContains.Count == 2) {
			PicesContains [0].transform.position = new Vector3 (PicesContains [0].transform.position.x + 5, PicesContains [0].transform.position.y, PicesContains [0].transform.position.z);
			PicesContains [0].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [1].transform.position = new Vector3 (PicesContains [1].transform.position.x - 5, PicesContains [1].transform.position.y, PicesContains [1].transform.position.z);
			PicesContains [1].transform.localScale = SizeForTwoPices.transform.localScale;
		} else if (PicesContains.Count == 3) {
			PicesContains [0].transform.position = new Vector3 (PicesContains [0].transform.position.x , PicesContains [0].transform.position.y + 5, 0);
			PicesContains [1].transform.position = new Vector3 (PicesContains [1].transform.position.x , PicesContains [1].transform.position.y + 5, 0);
			PicesContains [2].transform.position = new Vector3 (PicesContains [0].transform.position.x , PicesContains [0].transform.position.y - 10, 0);
			PicesContains [0].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [1].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [2].transform.localScale = SizeForTwoPices.transform.localScale;
//			PicesContains [2].transform.position = new Vector3 (PicesContains [2].transform.position.x - 5, PicesContains [1].transform.position.y - 5, 0);
		}
		else if (PicesContains.Count == 4) {
			PicesContains [0].transform.position = new Vector3 (PicesContains [0].transform.position.x , PicesContains [0].transform.position.y , 0);
			PicesContains [1].transform.position = new Vector3 (PicesContains [1].transform.position.x , PicesContains [1].transform.position.y , 0);
			PicesContains [2].transform.position = new Vector3 (PicesContains [0].transform.position.x , PicesContains [0].transform.position.y-10 , 0);
			PicesContains [3].transform.position = new Vector3 (PicesContains [1].transform.position.x, PicesContains [1].transform.position.y-10 , 0);
			PicesContains [0].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [1].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [2].transform.localScale = SizeForTwoPices.transform.localScale;
			PicesContains [3].transform.localScale = SizeForTwoPices.transform.localScale;
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		print ("OnTriggerExit2D");
		col.gameObject.transform.localScale = new Vector3 (1, 1, 1);
		PicesContains.Remove (col.gameObject);
		if (PicesContains.Count == 1) {
			PicesContains [0].transform.position = this.transform.position;
			PicesContains [0].transform.localScale = new Vector3 (1, 1, 1);
		}
	}
}
