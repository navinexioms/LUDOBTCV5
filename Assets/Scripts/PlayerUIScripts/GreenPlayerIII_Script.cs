using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerIII_Script : MonoBehaviour {

	// Use this for initialization
	public static string GreenPlayerIII_ColName;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			GreenPlayerIII_ColName = col.gameObject.name;
//			print ("GreenPlayerIII_ColName:" + GreenPlayerIII_ColName);
			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI GreenI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () 
	{

		GreenPlayerIII_ColName = "none";

	}
}
