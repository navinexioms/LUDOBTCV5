using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerII_Script: MonoBehaviour {

	// Use this for initialization
	public static string BluePlayerII_ColName;

	void OnTriggerEnter2D(Collider2D col)
	{
	
		if (col.gameObject.tag == "blocks") 
		{
		
			BluePlayerII_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{
			
				print ("Entered PlayerI BlueII in safe house");
			
			}
		}
	}
	// Use this for initialization
	void Start () 
	{
		
		BluePlayerII_ColName = "none";
	
	}
}
