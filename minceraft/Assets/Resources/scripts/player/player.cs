using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
	
	CharacterController cont;
	public Transform head;
	public int movetype = 0; //0 = walking, 1 = climbing, 2 = flying, 3 = still (or hookshoting)
	public float timer = 0;
	public ViveControllerVR hand1;
	public ViveControllerVR hand2;
	public Vector3 vecoff;
	public Vector3 vecoff2;
	public Vector3 vecoff3;
	
	// Use this for initialization
	void Start () {
		cont = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if( movetype != 1 && movetype != 2 && (hand1.GetComponent<ViveControllerVR>().hookto || hand2.GetComponent<ViveControllerVR>().hookto)){
			movetype = 3;
		} else if(movetype != 1 && movetype != 2){
			movetype = 0;
		}
		
	}
	public Vector3 movx = Vector3.zero;
	public Vector3 movy = Vector3.zero;
	public Vector3 movz = Vector3.zero;
	public Transform headhold;
	public Vector3 headholdvec = Vector3.zero;
	public Vector3 vecoff4 = Vector3.zero;
	void FixedUpdate(){
		timer += Time.deltaTime;
		if(movetype == 1 || movetype == 3){
			cont.stepOffset = 0.05f;
			cont.height = 0.1f;
			cont.center = new Vector3(head.localPosition.x, head.localPosition.y, head.localPosition.z);
			timer = 0;
		} else {
			cont.height = Mathf.Max(Mathf.Lerp(cont.height,Mathf.Min(Mathf.Max((((vecoff3 != vecoff2?vecoff2.y-Mathf.Abs(vecoff3.y-vecoff2.y)-0.3f:vecoff2.y)-vecoff.y)/2.0f)*transform.localScale.x,0.1f), 2.5f), timer),0.1f);
			cont.stepOffset = (vecoff3 != vecoff2?Mathf.Lerp(cont.stepOffset,cont.height/1.9f, timer):0);
			cont.center = Vector3.Lerp(cont.center,new Vector3(head.localPosition.x, cont.height/2.0f, head.localPosition.z),timer);
			headholdvec = transform.position-new Vector3(0,(vecoff4.y > vecoff2.y?0:Mathf.Abs(vecoff4.y-(vecoff2.y*(vecoff4.y == 0?0:1)))+0.1f),0);
			headhold.position = headholdvec;
		}
	}
	
	public float speed = 3.0f; // replace with entity speed later
	
	void LateUpdate(){
		GetComponent<CharacterController> ().Move((movx*(movetype == 0 || movetype == 2?speed:1)) + movy + (movz*(movetype == 0 || movetype == 2?speed:1)));
	
		movx = Vector3.zero;
		movy = Vector3.zero;
		movz = Vector3.zero;
	}
	
	
}
