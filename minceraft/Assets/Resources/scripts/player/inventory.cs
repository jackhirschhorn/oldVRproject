using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class inventory : MonoBehaviour {
	public List<Transform> inv = new List<Transform>(); //inventory
	public List<int[]> slottakes = new List<int[]>();
	public Transform[] collid1 = new Transform[3];
	public Transform[] collid2 = new Transform[3];
	public bool isplayerinv = false;

	
	void Start(){
		if(isplayerinv){
			//head
			slottakes.Add(new int[]{0,1});
			//body
			slottakes.Add(new int[]{0,2});
			//wrist1
			slottakes.Add(new int[]{0,3});
			//wrist2
			slottakes.Add(new int[]{0,3});
			//boots
			slottakes.Add(new int[]{0,4});
			//back
			slottakes.Add(new int[]{0,0});
			//belt left
			slottakes.Add(new int[]{0,0});
			//belt right
			slottakes.Add(new int[]{0,0});
			//belt front
			slottakes.Add(new int[]{0,0});
		} else {
			foreach(Transform t in inv){
				slottakes.Add(new int[]{0,0});
			}
		}
	}
	
	public void iscolliding(Transform t, Transform t2, bool b, bool b2, Transform t3){
		if(b){
			if(master.lefthand.GetComponent<ViveControllerVR>().objectInHand != null && master.lefthand.GetComponent<ViveControllerVR>().objectInHand.transform == t){
				collid1[0] = t;
				collid1[1] = t2;
				collid1[2] = t3;
				if(b2)master.lefthand.GetComponent<Hand>().TriggerHapticPulse(500,30,1);
			}
			if(master.righthand.GetComponent<ViveControllerVR>().objectInHand != null && master.righthand.GetComponent<ViveControllerVR>().objectInHand.transform == t){
				collid2[0] = t;
				collid2[1] = t2;
				collid2[2] = t3;
				if(b2)master.righthand.GetComponent<Hand>().TriggerHapticPulse(500,30,1);
			}
		} else {
			if(collid1[1] == t2){
				collid1[0] = null;
				collid1[1] = null;
				collid1[2] = null;
			}
			if(collid2[1] == t2){
				collid2[0] = null;
				collid2[1] = null;
				collid2[2] = null;
			}
		}
	}
	
	void Update(){
		if(collid1[0] != null && !collid1[2].GetComponent<inventoryhelper>().holding){
			if(master.lefthand.GetComponent<ViveControllerVR>().getPinchUp()){
				collid1[0].parent = collid1[1];
				collid1[0].GetComponent<item>().scal *= (1/1.5f);
				collid1[0].GetComponent<Rigidbody>().isKinematic = true;
				collid1[2].GetComponent<inventoryhelper>().holding = true;
				
			}	
		}
		if(collid2[0] != null && !collid2[2].GetComponent<inventoryhelper>().holding){
			if(master.righthand.GetComponent<ViveControllerVR>().getPinchUp()){
				collid2[0].parent = collid2[1];
				collid2[0].GetComponent<item>().scal *= (1/1.5f);
				collid2[0].GetComponent<Rigidbody>().isKinematic = true;
				collid2[2].GetComponent<inventoryhelper>().holding = true;
			}	
		}
	}
	
	
	
	/*public List<Transform> inv = new List<Transform>(); //default inventory
	public List<int[]> slottakes = new List<int[]>();
	/*
	-1 = break in slottakes 2;
	0 = any
	1 = helmet
	2 = armor
	public int[] slottakes2 = new int[]{};
	public List<int> slottakes3 = new List<int>();
	public List<Transform> slotpar = new List<Transform>();

	public Transform c1;
	public int c1cangrab = -1;
	public Transform c1obj;
	public Transform c2;
	public int c2cangrab = -1;
	public Transform c2obj;
	
	public Transform none;
	public Transform head;
	public Transform scabbard1;
	public Transform scabbard2;
	public Transform backscab;
	public Transform bodyarmor;
	
	public bool isplayerinv;
	public bool isbag = false;
	
	// Use this for initialization
	void Start () {
		if(isplayerinv && !isbag){
			//head
			slotpar.Add(head);
			inv.Add(none);
			slottakes.Add(new int[]{2,1});
			//scabbard1
			slotpar.Add(scabbard1);
			inv.Add(none);
			slottakes.Add(new int[]{0});
			//scabbard2;
			slotpar.Add(scabbard2);
			inv.Add(none);
			slottakes.Add(new int[]{0});
			//back scabbard
			slotpar.Add(backscab);
			inv.Add(none);
			slottakes.Add(new int[]{0});
			//body
			slotpar.Add(bodyarmor);
			inv.Add(none);
			slottakes.Add(new int[]{2,2});
		} else if(isbag){
			foreach(Transform t in transform){
				slotpar.Add(t);
				inv.Add(none);
				slottakes.Add(new int[]{2,3});//small items only
			}
		} else {
			foreach(int i in slottakes2){
				if(i != -1){
					slottakes3.Add(i);
				} else {
					slottakes.Add((int[])slottakes3.ToArray());
					slottakes3.Clear();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int t = 0; t < inv.Count; t++){
			if((c1.GetComponent<ViveControllerVR>().objectInHand != null && inv[t] == c1.GetComponent<ViveControllerVR>().objectInHand.transform) || (c2.GetComponent<ViveControllerVR>().objectInHand != null && inv[t] == c2.GetComponent<ViveControllerVR>().objectInHand.transform)){
				
				inv[t] = none;
			}
		}
		if(c1.GetComponent<ViveControllerVR>().objectInHand != null && c1.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>()){
			c1cangrab = -1;
			for(int i = 0; i < slotpar.Count; i++){
				if( (slottakes[i][0] == 0 || c1.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>().containseffect(slottakes[i][0], true) == slottakes[i][1]) && Vector3.Distance(c1.GetComponent<ViveControllerVR>().objectInHand.transform.position, slotpar[i].position) < (isbag? 0.02f :0.2f)){
					c1cangrab = i;
					c1obj = c1.GetComponent<ViveControllerVR>().objectInHand.transform;
					c1.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().glow();
					
				}
			}
			//c1.GetComponent<ViveControllerVR>().objectInHand.transform.parent = slotpar[0];
			//c1.GetComponent<ViveControllerVR>().objectInHand.transform.position = slotpar[0].position;
			//c1.GetComponent<ViveControllerVR>().objectInHand.transform.localRotation = slotpar[0].localRotation;
		} 
		if(c1cangrab == -1 && c1.GetComponent<ViveControllerVR>().objectInHand != null && c1.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>()){
			c1cangrab = -1;
			c1obj = null;
			c1.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>().dontglow();
			
		}
		if(c2.GetComponent<ViveControllerVR>().objectInHand != null && c2.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>()){
			c2cangrab = -1;
			for(int i = 0; i < slotpar.Count; i++){
				if((slottakes[i][0] == 0 || c2.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>().containseffect(slottakes[i][0], true) == slottakes[i][1]) && Vector3.Distance(c2.GetComponent<ViveControllerVR>().objectInHand.transform.position, slotpar[i].position) < (isbag? 0.02f :0.2f)){
					c2cangrab = i;
					c2obj = c2.GetComponent<ViveControllerVR>().objectInHand.transform;
					c2.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().glow();
				}
			}
			//c2.GetComponent<ViveControllerVR>().objectInHand.transform.parent = slotpar[0];
			//c2.GetComponent<ViveControllerVR>().objectInHand.transform.position = slotpar[0].position;
			//c2.GetComponent<ViveControllerVR>().objectInHand.transform.localRotation = slotpar[0].localRotation;
		} 
		if(c2cangrab == -1 && c2.GetComponent<ViveControllerVR>().objectInHand != null && c2.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>()){
			c2cangrab = -1;
			c2obj = null;
			c2.GetComponent<ViveControllerVR>().objectInHand.transform.GetComponent<item>().dontglow();
			
		}
	}
	
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = Mathf.Infinity;
		fx.breakTorque = Mathf.Infinity;
		return fx;
	}
	
	void LateUpdate(){
		if(c1obj != null && c1cangrab != -1 && c1.GetComponent<ViveControllerVR>().objectInHand == null){
			c1obj.GetComponent<Rigidbody>().isKinematic = true;
			if(isplayerinv){
				c1obj.parent = slotpar[c1cangrab];
				c1obj.position = slotpar[c1cangrab].position;
				c1obj.rotation = slotpar[c1cangrab].localRotation;
				c1obj.LookAt(slotpar[c1cangrab].GetChild(0).position, slotpar[c1cangrab].up);
				
			} else {
				c1obj.position = slotpar[c1cangrab].position;
				c1obj.rotation = slotpar[c1cangrab].localRotation;
				c1obj.LookAt(slotpar[c1cangrab].GetChild(0).position, slotpar[c1cangrab].up);
				var joint = AddFixedJoint();
				joint.connectedBody = c1obj.GetComponent<Rigidbody>();
				c1obj.GetComponent<item>().par = transform;
			}
			if(isbag){
				c1obj.GetComponent<item>().hidden = true;
			}
			c1obj.GetComponent<item>().onentity = true;
			c1obj.GetComponent<item>().dontglow();
			inv[c1cangrab] = c1obj;
			if(isplayerinv){c1obj.GetComponent<item>().changecolliders(false, false, false);
			} else {
				Physics.IgnoreCollision(c1obj.GetChild(0).GetComponent<Collider>(), GetComponent<Collider>());
			}
		}
		if(c2obj != null && c2cangrab != -1 && c2.GetComponent<ViveControllerVR>().objectInHand == null){
			c2obj.GetComponent<Rigidbody>().isKinematic = true;
			if(isplayerinv){
				c2obj.parent = slotpar[c2cangrab];
				c2obj.position = slotpar[c2cangrab].position;
				c2obj.rotation = slotpar[c2cangrab].localRotation;
				c2obj.LookAt(slotpar[c2cangrab].GetChild(0).position, slotpar[c2cangrab].up);
			
			} else {
				c2obj.position = slotpar[c2cangrab].position;
				c2obj.rotation = slotpar[c2cangrab].localRotation;
				c2obj.LookAt(slotpar[c2cangrab].GetChild(0).position, slotpar[c2cangrab].up);
				var joint = AddFixedJoint();
				joint.connectedBody = c2obj.GetComponent<Rigidbody>();
				c2obj.GetComponent<item>().par = transform;
			
			}
			if(isbag){
				c2obj.GetComponent<item>().hidden = true;
			}
			c2obj.GetComponent<item>().onentity = true;
			c2obj.GetComponent<item>().dontglow();
			inv[c2cangrab] = c2obj;
			if(isplayerinv){c2obj.GetComponent<item>().changecolliders(false, false, false);
			} else {
				Physics.IgnoreCollision(c2obj.GetChild(0).GetComponent<Collider>(), GetComponent<Collider>());
			}
		}
		if(c1.GetComponent<ViveControllerVR>().objectInHand == null){
			c1cangrab = -1;
			c1obj = null;
		}
		if(c2.GetComponent<ViveControllerVR>().objectInHand == null){
			c2cangrab = -1;
			c2obj = null;
		}
	}
	*/
}
