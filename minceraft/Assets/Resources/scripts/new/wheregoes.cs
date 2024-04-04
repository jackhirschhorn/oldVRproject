using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheregoes : MonoBehaviour {
	
	public static Collision cnone;
	public Collision cnone2 = new Collision();
	
	public static Transform tnone;
	public Transform tnone2;
	
	public static Transform items;
	public Transform items2;

	// Use this for initialization
	void Awake () {
		items = items2;
		tnone = tnone2;
		cnone = cnone2;
	}
}
