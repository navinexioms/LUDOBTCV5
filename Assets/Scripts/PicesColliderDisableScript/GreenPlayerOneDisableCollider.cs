using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerOneDisableCollider : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.name.Equals ("Green Player III")) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (col.gameObject.name.Equals ("Green Player I")) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (col.gameObject.name.Equals ("Green Player IV")) {
			col.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		col.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		if (col.gameObject.name.Equals ("Green Player II")) {
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}
}
