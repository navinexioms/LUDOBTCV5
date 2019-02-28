using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerIV_Script : MonoBehaviour 
{

	// Use this for initialization
	public static string BluePlayerIV_ColName;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			BluePlayerIV_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI BlueIV in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () 
	{

		BluePlayerIV_ColName = "none";

	}
}
