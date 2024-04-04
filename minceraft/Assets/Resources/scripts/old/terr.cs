using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terr : MonoBehaviour {

	[SerializeField]
	public int penchance = 100;
	public float hp = 100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			Destroy (this.gameObject);
		}
	}
}
