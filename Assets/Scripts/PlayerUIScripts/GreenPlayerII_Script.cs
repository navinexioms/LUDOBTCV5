using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerII_Script : MonoBehaviour {

	// Use this for initialization
	public static string GreenPlayerII_ColName;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			GreenPlayerII_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI GreenI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () 
	{

		GreenPlayerII_ColName = "none";

	}
}
