using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerMoveToActualSafeZone : MonoBehaviour {
	public List<GameObject> BlueSafePlaces;
	public List<GameObject> Pices;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		print ("Triggered");
		Pices.Add (col.gameObject);
		StartCoroutine (PlaceingPices ());
	}
	IEnumerator PlaceingPices()
	{
		yield return new WaitForSeconds (.5f);
		if (Pices.Count == 1) {
			print ("Entered");
			Pices [0].transform.position = BlueSafePlaces [0].transform.position;
			Rigidbody2D rgb = Pices [0].GetComponent<Rigidbody2D> ();
			rgb.constraints = RigidbodyConstraints2D.FreezeAll;
		}if (Pices.Count == 2) {
			print ("Entered");
			Pices [1].transform.position = BlueSafePlaces [1].transform.position;
			Rigidbody2D rgb = Pices [1].GetComponent<Rigidbody2D> ();
			rgb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		if (Pices.Count == 3) {
			print ("Entered");
			Pices [2].transform.position = BlueSafePlaces [2].transform.position;
			Rigidbody2D rgb = Pices [2].GetComponent<Rigidbody2D> ();
			rgb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		if (Pices.Count == 4) {
			print ("Entered");
			Pices [3].transform.position = BlueSafePlaces [3].transform.position;
			Rigidbody2D rgb = Pices [3].GetComponent<Rigidbody2D> ();
			rgb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}
}
