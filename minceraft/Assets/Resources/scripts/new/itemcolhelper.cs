using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemcolhelper : MonoBehaviour {
	void OnCollisionEnter(Collision col){
		if(transform.parent.GetComponent<item>().lastcol == wheregoes.cnone || transform.parent.GetComponent<item>().lastcol.transform == transform.parent.GetComponent<item>().par){
			if(col.collider.transform.parent.GetComponent<item>())Debug.Log(col.collider.transform.parent.GetComponent<item>() + " " + col.collider.transform.parent.GetComponent<item>().containseffect(3, false) + " " + col.collider.transform.parent.GetComponent<item>().par);
			if(col.collider.transform.GetComponent<enemy>() || (col.collider.transform.parent.GetComponent<item>() && col.collider.transform.parent.GetComponent<item>().containseffect(3, false) != 0 && col.collider.transform.parent.GetComponent<item>().par != wheregoes.tnone)){
				Debug.Log(col.transform);
				transform.parent.GetComponent<item>().lastcol = col;
			}
		}
		//Debug.Log(col);
	}
	
	void LateUpdate(){
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
	}
}
