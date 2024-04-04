using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour {

	[SerializeField]
	public Transform[] factions = new Transform[] {

	};
	public float radius = 0.0f;
	public int damage = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void explode(){
		for (int i = 0; i < factions.Length; i++) {
			foreach(Transform child in factions[i]){
				if (Vector3.Distance (child.Find ("unit").position, transform.position) < radius) {
					//child.
				}
			}
		}
	}
}
