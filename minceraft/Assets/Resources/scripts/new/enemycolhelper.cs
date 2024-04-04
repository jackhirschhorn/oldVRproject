using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemycolhelper : MonoBehaviour {

	void OnTriggerExit(Collider col){
		if(col.transform.parent.GetComponent<item>())col.transform.parent.GetComponent<item>().ghost = false;
	}
}
