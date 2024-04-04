using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feethelper : MonoBehaviour {
	
	public Transform cam;
	public float heightmod = 0.5f;
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3(transform.position.x,Mathf.Lerp(transform.parent.position.y, cam.position.y, heightmod),transform.position.z);
	}
}
