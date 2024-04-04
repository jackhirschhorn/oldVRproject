using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellslave : MonoBehaviour {
	public int id = 0;
	public bool on = false;
	public bool sub = true;
	public spellmaster master;
	
	void Start(){
		if(sub){
			master = transform.parent.parent.GetComponent<spellmaster>();
		} else {
			master = transform.parent.GetComponent<spellmaster>();
		}
	}

	void OnTriggerEnter(Collider col){
		if(on){
			if(col.gameObject.tag == "Player"){
				if(sub){
					master.whichsub = id;
				} else {
					master.whichcata = id;
				}
				master.lastcollider = col.transform;
			}
		}
	}
	
	void OnTriggerExit(Collider col){
		if(on){
			if(col.gameObject.tag == "Player"){
				master.whichsub = -1;
				master.whichcata = -1;
			}
		}
	}
	
}
