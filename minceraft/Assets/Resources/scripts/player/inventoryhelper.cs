using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryhelper : MonoBehaviour {
	public inventory inv;
	public Transform anchor;
	public bool haptic;
	public bool holding = false;
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col){
		if(col.gameObject.layer == 14){
			inv.iscolliding(col.transform, anchor, true, haptic, transform);
		}
	}
	
	void OnTriggerExit(Collider col){
		if(col.gameObject.layer == 14){
			inv.iscolliding(col.transform, anchor, false, haptic, transform);
		}
	}
}
