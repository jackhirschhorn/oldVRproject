using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyparthold : MonoBehaviour {
	public List<Transform> part = new List<Transform>();
	public List<float> partheight = new List<float>();
	RaycastHit hit;
	RaycastHit hit2;
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 2.55f, master.mask1)){
			for(int i = 0; i < part.Count; i++){
				part[i].position = Vector3.Lerp(hit.point,transform.position, partheight[i]);
				part[i].rotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
			}
			master.play1.GetComponent<player>().vecoff = hit.point;
			master.play1.GetComponent<player>().vecoff2 = transform.position;
			if(Physics.Raycast(hit.point, Vector3.up, out hit2, 5, master.mask1)){
				master.play1.GetComponent<player>().vecoff3 = hit2.point;
				master.play1.GetComponent<player>().vecoff4 = hit2.point;
			} else {
				master.play1.GetComponent<player>().vecoff3 = transform.position;
				master.play1.GetComponent<player>().vecoff4 = Vector3.zero;
			}
		} else {
			for(int i = 0; i < part.Count; i++){
				part[i].position = Vector3.Lerp(transform.position + (Vector3.down *2.55f),transform.position, partheight[i]);
				part[i].rotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
			}
		}		
	}
}
