using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minichunkl : MonoBehaviour {

	/*public bool tickon = true;
	public int ticknum = 1;

	public void dotick(int i){
		if(i == ticknum)StartCoroutine("dotick2");
	}
	
	public IEnumerator dotick2(){
		yield return new WaitForEndOfFrame();
		replace();
		foreach(Transform child in transform){
			if(!child.GetComponent<liquid>().settled)child.GetComponent<liquid>().dotick(1);
		}
	}
	
	int loneid(){
		int workid = transform.GetChild(0).GetComponent<liquid>().id;
		foreach(Transform child in transform){
			if(child.GetComponent<liquid>().id != workid){
				return 0;
			}
		}
		return workid;
	}
	
	void replace(){
		if(transform.childCount >= 256 && loneid() != 0){
			for(int i = 16; i > 0; i--){
				int c = 0;
				foreach(Transform child in transform){
					if(child.localPosition.y == ((i - 8.0f)/16.0f)) c++;
				}
				if(c >= 256){
					master.addliquid(transform.position, loneid(), i, 0, 0, false, master.liquids);
					i = -1;
					Destroy(gameObject);
				}
			}
		}
	}*/
}
