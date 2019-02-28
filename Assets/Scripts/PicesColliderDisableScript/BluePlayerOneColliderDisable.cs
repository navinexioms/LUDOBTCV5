using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerOneColliderDisable : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.name.Equals ("Blue Player III")) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (col.gameObject.name.Equals ("Blue Player II")) {
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (col.gameObject.name.Equals ("Blue Player IV")) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		col.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		if (col.gameObject.name.Equals ("Blue Player II")) {
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}
}
