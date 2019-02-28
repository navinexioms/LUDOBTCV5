using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowWhiteSafePoint : MonoBehaviour {

	public List<GameObject> Pices ;
	public List<GameObject> PositionFor2Pice;
	public List<GameObject> PositionFor3Pice;
	public List<GameObject> PositionFor4Pice;
	public List<GameObject> PositionFor5Pice;
	public List<GameObject> PositionFor6Pice;
	public GameObject PiceSize;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		Pices.Add (col.gameObject);
		if (Pices.Count > 1) {
			StartCoroutine (ChangePosition ());
		}

	}
	IEnumerator ChangePosition()
	{
		yield return new WaitForSeconds (.5f);
		switch(Pices.Count)
		{
		case 1:
			if (Pices.Count == 1) {
				Pices [0].transform.localScale = new Vector3 (1, 1, 1);
				Pices [0].transform.position = this.transform.position;
			}
			break;
		case 2:
			Pices [0].transform.localScale = Pices [1].transform.localScale = PiceSize.transform.localScale;
			Pices [0].transform.position = PositionFor2Pice [0].transform.position;
			Pices [1].transform.position = PositionFor2Pice [1].transform.position;
			break;
		case 3:
			Pices [0].transform.position = PositionFor3Pice [0].transform.position;
			Pices [1].transform.position = PositionFor3Pice [1].transform.position;
			Pices [2].transform.position = PositionFor3Pice [2].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = PiceSize.transform.localScale;
			break;
		case 4:
			Pices [0].transform.position = PositionFor4Pice [0].transform.position;
			Pices [1].transform.position = PositionFor4Pice [1].transform.position;
			Pices [2].transform.position = PositionFor4Pice [2].transform.position;
			Pices [3].transform.position = PositionFor4Pice [3].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = PiceSize.transform.localScale;
			break;
		case 5:
			Pices [0].transform.position = PositionFor5Pice [0].transform.position;
			Pices [1].transform.position = PositionFor5Pice [1].transform.position;
			Pices [2].transform.position = PositionFor5Pice [2].transform.position;
			Pices [3].transform.position = PositionFor5Pice [3].transform.position;
			Pices [4].transform.position = PositionFor5Pice [4].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = Pices [4].transform.localScale = PiceSize.transform.localScale;
			break;
		case 6:
			Pices [0].transform.position = PositionFor6Pice [0].transform.position;
			Pices [1].transform.position = PositionFor6Pice [1].transform.position;
			Pices [2].transform.position = PositionFor6Pice [2].transform.position;
			Pices [3].transform.position = PositionFor6Pice [3].transform.position;
			Pices [4].transform.position = PositionFor6Pice [4].transform.position;
			Pices [5].transform.position = PositionFor6Pice [5].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = Pices [4].transform.localScale = Pices [5].transform.localScale = PiceSize.transform.localScale;
			break;
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		col.gameObject.transform.localScale = new Vector3 (1, 1, 1);
		Pices.Remove (col.gameObject);
		switch(Pices.Count)
		{
		case 1:
			if (Pices.Count == 1) {
				Pices [0].transform.localScale = new Vector3 (1, 1, 1);
				Pices [0].transform.position = this.transform.position;
			}
			break;
		case 2:
			Pices [0].transform.localScale = Pices [1].transform.localScale = PiceSize.transform.localScale;
			Pices [0].transform.position = PositionFor2Pice [0].transform.position;
			Pices [1].transform.position = PositionFor2Pice [1].transform.position;
			break;
		case 3:
			Pices [0].transform.position = PositionFor3Pice [0].transform.position;
			Pices [1].transform.position = PositionFor3Pice [1].transform.position;
			Pices [2].transform.position = PositionFor3Pice [2].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = PiceSize.transform.localScale;
			break;
		case 4:
			Pices [0].transform.position = PositionFor4Pice [0].transform.position;
			Pices [1].transform.position = PositionFor4Pice [1].transform.position;
			Pices [2].transform.position = PositionFor4Pice [2].transform.position;
			Pices [3].transform.position = PositionFor4Pice [3].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = PiceSize.transform.localScale;
			break;
		case 5:
			Pices [0].transform.position = PositionFor5Pice [0].transform.position;
			Pices [1].transform.position = PositionFor5Pice [1].transform.position;
			Pices [2].transform.position = PositionFor5Pice [2].transform.position;
			Pices [3].transform.position = PositionFor5Pice [3].transform.position;
			Pices [4].transform.position = PositionFor5Pice [4].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = Pices [4].transform.localScale = PiceSize.transform.localScale;
			break;
		case 6:
			Pices [0].transform.position = PositionFor6Pice [0].transform.position;
			Pices [1].transform.position = PositionFor6Pice [1].transform.position;
			Pices [2].transform.position = PositionFor6Pice [2].transform.position;
			Pices [3].transform.position = PositionFor6Pice [3].transform.position;
			Pices [4].transform.position = PositionFor6Pice [4].transform.position;
			Pices [5].transform.position = PositionFor6Pice [5].transform.position;
			Pices [0].transform.localScale = Pices [1].transform.localScale = Pices [2].transform.localScale = Pices [3].transform.localScale = Pices [4].transform.localScale = Pices [5].transform.localScale = PiceSize.transform.localScale;
			break;
		}
	}
}
