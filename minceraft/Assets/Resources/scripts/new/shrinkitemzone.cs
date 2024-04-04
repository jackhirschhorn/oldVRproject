using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkitemzone : MonoBehaviour {
	//fix flickering at some point
	
	void OnTriggerEnter(Collider col){
		if(col.transform.GetComponent<item>() && col.transform.GetComponent<item>().held == true && col.transform.GetComponent<item>().containseffect(2,true) == 3){
			col.transform.GetComponent<item>().hidden = true;
		}
	}
	
	public Transform tran1 = wheregoes.tnone;
	public Transform tran2 = wheregoes.tnone;
	
	void OnTriggerExit(Collider col){
		if(col.transform.GetComponent<item>()){
			col.transform.GetComponent<item>().hidden = false;
		}
	}
	
	
}
