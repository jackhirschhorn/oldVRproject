using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camhelper : MonoBehaviour {
	public Transform pos;
	public Transform lok;
	
	// Update is called once per frame
	void Update () {
		transform.position = pos.position;
		transform.LookAt(lok, Vector3.up);
		Debug.Log(transform.position + " " + pos.position);
	}
}
