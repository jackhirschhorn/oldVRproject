using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class item : MonoBehaviour {
	public Collision lastcol;
	
	//technical
	public Transform par = wheregoes.tnone;
	public bool ghost = false;
	public bool onentity = false;
	public bool hidden = false;
	public bool held = false;
	public List<Transform> holdpoints = new List<Transform>();
	public Vector3 holdpointturn = Vector3.zero;
	public int holdpointturn2 = 0;
	public Transform invhold;
	public Vector3 getholdpointturn(){
		if(id == 1){
			if(id2 == 0){
				holdpointturn = transform.up;
			} else if(id2 == 1){
				holdpointturn = Vector3.Lerp(par.transform.forward,par.transform.up,0.3f);
			} else if(id2 == 2){
				holdpointturn = transform.up;
			} else if(id2 == 3){
				holdpointturn = Vector3.Lerp(par.right,-par.up,0.5f);
			} 
		}
		return holdpointturn;
	}
	
	public int id = 0;
	/*
	0 = block
	1 = item
	*/
	public int id2 = -1;
	/*
	matches id for type (ie: block 0 is rock);
	*/ 
	public Vector3 scal = new Vector3(1,1,1);
	
	public Transform obj;
	public Transform model;
	//stats
	public List<int[]> effects = new List<int[]>();
	public int cooldown = 10;
	public List<Transform> vartrans = new List<Transform>();
	public List<bool> custombool = new List<bool>();
	public List<Vector3> varvec = new List<Vector3>();
	public List<float> varfloat = new List<float>();
	
	public int heat; //heat of item, used for forging
	public stat[] stats = new stat[3]; //durability/max, damage/type, wieldskill/wieldtype
	/*
	wieldskill/wieldtype
		0 = regular item type;
		1 = helmet
		2 = armor
		3 = wrist
		4 = boots
	*/
	
	public LayerMask mask1;
	//note: timer is measured in 1/10ths of seconds
	/*
	-1= can't collide / none / timer
	0= none
	1 doesdamage/bonusdamage/timer/# of die of damage/d# damage/type of damage
	2 itemtype/type (0 = regular item, 1 = helmet, 2 = bodyarmor, 3 = small item for bag)
	3 armor/reduction%/timer/reduction #/hp
	*/
	//types of damage
	/*
	0 = bludgeoning
	*/
	
	//public List<Transform> ignore = new List<Transform>();
	
	
	public class funct {
		public int id;
	}
	
	public class stat{
		public int stat1;
		public int stat1mod;		
	}
	
	void Update(){
		//Debug.Log(lastcol);
		if(lastcol != wheregoes.cnone){
			if(lastcol.collider.transform.GetComponent<enemy>() && !ghost){
				lastcol.collider.transform.GetComponent<enemy>().takedamage(1, 1);//change this, add force requirement
				effects.Add(new int[]{-1, 0, cooldown});
				//changecolliders(false, true, false);
				/*Physics.IgnoreCollision(lastcol.collider, transform.GetComponent<Collider>());
				ignore.Add(lastcol.collider.transform);
				foreach(FixedJoint j in lastcol.collider.transform.GetComponents<FixedJoint>()){
					Physics.IgnoreCollision(j.connectedBody.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());
					ignore.Add(j.connectedBody.transform);
				}*/
			
			}else if(lastcol.collider.transform.parent.GetComponent<item>() && lastcol.collider.transform.parent.GetComponent<item>().containseffect(3, false) != 0 && lastcol.collider.transform.parent.GetComponent<item>().par != wheregoes.tnone){
				ghost = true;
				lastcol.collider.transform.parent.GetComponent<item>().takedamage(1, 1);
				effects.Add(new int[]{-1, 0, cooldown});
				//changecolliders(false, true, false);
				//Physics.IgnoreCollision(lastcol.collider, transform.collider);
				//ignore.Add(lastcol);
				if(lastcol.collider.transform.parent.GetComponent<item>().par != wheregoes.tnone){
					lastcol.collider.transform.parent.GetComponent<item>().par.GetComponent<enemy>().invincable = true;
					lastcol.collider.transform.parent.GetComponent<item>().par.GetComponent<enemy>().effects.Add(new int[]{-1,0,cooldown});
				}
				
				/*Physics.IgnoreCollision(lastcol.collider.transform.GetComponent<item>().par.GetComponent<Collider>(), transform.GetComponent<Collider>());
				ignore.Add(lastcol.collider.transform);
				foreach(FixedJoint j in lastcol.collider.transform.GetComponent<item>().par.GetComponents<FixedJoint>()){
					Physics.IgnoreCollision(j.connectedBody.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());
					ignore.Add(j.connectedBody.transform);
				}*/
			}
			lastcol = wheregoes.cnone;
		}
	}
	
	void takedamage(int i, int i2){ //damage, type
		
	}
	
	public void changecolliders(bool b, bool b2, bool b3){
		foreach(Transform t in transform){
			//foreach(Collider c in t){
				if(t.GetComponent<Collider>()){
					if(!b2)t.GetComponent<Collider>().enabled = b;
				} // || t.gameObject.layer != 12
			//}
		}
		//foreach(Collider c in GetComponent<Collider>()){
			if(b3)GetComponent<Collider>().enabled = b;
		//}
	}
	
	void Start(){
		makeitem(id, id2);
		StartCoroutine(timer());
		lastcol = wheregoes.cnone;
		par = wheregoes.tnone;
		foreach(Transform t in transform){
			if(t.gameObject.tag == "grab")holdpoints.Add(t);
		}
	}
	
	public bool dochange = false;
	
	public IEnumerator timer(){
		while(true){
			for(int i = 0; i < effects.Count; i++){
				if(effects[i].Length > 2 && effects[i][2] > 0){
					effects[i][2] -= 1;
					if(effects[i][2] == 0){
						if(effects[i][0] == -1)dochange = true;
						effects.RemoveAt(i);
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	void LateUpdate(){
		if(hidden){ //fix this later
			//gameObject.layer = 15;
			transform.localScale = scal*0.1f;
			if(GetComponent<TrailRenderer>())GetComponent<TrailRenderer>().enabled = false;
		} else if(onentity){
			//gameObject.layer = 13;
			transform.localScale = scal;
			if(GetComponent<TrailRenderer>())GetComponent<TrailRenderer>().enabled = true;
		} else {
			//gameObject.layer = 14;
			transform.localScale = scal;
			if(GetComponent<TrailRenderer>())GetComponent<TrailRenderer>().enabled = true;
		
		}
		if(dochange){
			//changecolliders(true, true, false);
			/*foreach(Transform t in ignore){
				Physics.IgnoreCollision(t.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);
			}
			ignore.Clear();*/
			dochange = false;
		}
		if(id == 1){ 
			if(id2 == 0){ // grappling hook
				if(!held && custombool[0]){
					GetComponent<Rigidbody>().isKinematic = true;
				} else {
					GetComponent<Rigidbody>().isKinematic = false;
				}
				if(vartrans.Count == 0){
					RaycastHit hit;
					GetComponent<LineRenderer>().SetPosition(0, transform.position);
					if(Physics.Raycast (this.transform.position + new Vector3(0,0.05f,0), Vector3.down, out hit, 32.5f, mask1)){
						GetComponent<LineRenderer>().SetPosition(1, hit.point);
						transform.GetChild(0).localScale = new Vector3(0.1f,hit.distance,0.1f);
						transform.GetChild(0).localPosition = new Vector3(0, -hit.distance/2.0f ,0);
					} else {
						GetComponent<LineRenderer>().SetPosition(1, transform.position + Vector3.down * 32);
						transform.GetChild(0).localScale = new Vector3(0.1f,32,0.1f);
						transform.GetChild(0).localPosition = new Vector3(0, -16 ,0);
					}
				} else if(vartrans[0] != null)  {
					if(!held && custombool[0] && vartrans[0].GetComponent<item>().custombool[0]){
						transform.GetChild(1).GetComponent<BoxCollider>().isTrigger = false;
					} else {
						transform.GetChild(1).GetComponent<BoxCollider>().isTrigger = true;
					}
					GetComponent<LineRenderer>().SetPosition(0, transform.position);
					GetComponent<LineRenderer>().SetPosition(1, vartrans[0].position);
					transform.GetChild(0).localScale = new Vector3(0.1f,0.1f, Vector3.Distance(transform.position, vartrans[0].position)/2.0f);
					transform.GetChild(0).LookAt(vartrans[0].position, Vector3.up);
					transform.GetChild(0).position = Vector3.Lerp(transform.position, vartrans[0].position, 0.5f);
					transform.GetChild(1).localScale = new Vector3(0.1f,0.1f, Vector3.Distance(transform.position, vartrans[0].position)/2.0f);
					transform.GetChild(1).LookAt(vartrans[0].position, Vector3.up);
					transform.GetChild(1).position = Vector3.Lerp(transform.position, vartrans[0].position, 0.5f);
					
				}
			} else if(id2 == 1){ //bow and arrow
				GetComponent<LineRenderer>().SetPosition(0, vartrans[0].position);
				GetComponent<LineRenderer>().SetPosition(1, vartrans[0].position);
				GetComponent<LineRenderer>().SetPosition(2, vartrans[1].position);
				if(held){
					GetComponent<BoxCollider>().isTrigger = true;
				}
				if(held && vartrans.Count == 2 && par != null && par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand != null && par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>() != null && par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.transform != transform && Vector3.Distance(par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.transform.position,Vector3.Lerp(vartrans[0].position,vartrans[1].position,0.5f)) < 0.2f){
					vartrans.Add(par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.transform);					
				}
				if(held && vartrans.Count > 2){
					GetComponent<LineRenderer>().SetPosition(1, vartrans[2].position);
				
				}
				if(!held){
					GetComponent<BoxCollider>().isTrigger = false;
				}
			} else if(id2 == 2){// hook
				if(!held && custombool[0]){
					GetComponent<Rigidbody>().isKinematic = true;
				} else {
					GetComponent<Rigidbody>().isKinematic = false;
				}
			} else if(id2 == 3){//hookshot
				if(held){
					if(varfloat.Count != 0 ){
						if(!custombool[0]){
							varfloat[0] += Time.deltaTime *5;
							transform.GetChild(0).position = Vector3.Lerp(transform.position, varvec[0], varfloat[0] / (Vector3.Distance(transform.position, varvec[0])/5));
							if(varfloat[0] / (Vector3.Distance(transform.position, varvec[0])/5) >= 1){
								custombool[0] = true;
								par.GetComponent<ViveControllerVR>().hookto = true;
								varvec.Add(transform.position);
							}
						} else {
							par.GetComponent<ViveControllerVR>().movyover = Vector3.zero;
							//transform.position = Vector3.Lerp(varvec[0], varvec[1], varfloat[0] / (Vector3.Distance(varvec[1], varvec[0])/5));
							transform.GetChild(0).position = varvec[0];
							if(Vector3.Distance(varvec[0],par.GetComponent<ViveControllerVR>().play1.transform.position) > 1.0f && par.GetComponent<ViveControllerVR>().play1.movetype == 3){
								par.GetComponent<ViveControllerVR>().movyover = (varvec[0] - par.GetComponent<ViveControllerVR>().play1.transform.position).normalized * ((Time.deltaTime * 50) / (Vector3.Distance(varvec[1], varvec[0])/5));
							}
							if(par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().hookto){
								custombool[0] = false;
								varuse2();//par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().effects.Add(new int[3]{0,1,10});
							}
							
						}
					} else {
						transform.GetChild(0).position = Vector3.Lerp(transform.GetChild(0).position,transform.position + transform.forward * 0.2f, Time.deltaTime * Mathf.Max(32,1/Mathf.Min(1,Vector3.Distance(transform.position, transform.GetChild(0).position))));
					}
				} else {
					transform.GetChild(0).position = Vector3.Lerp(transform.GetChild(0).position,transform.position + transform.forward * 0.2f, Time.deltaTime * Mathf.Max(32,1/Mathf.Min(1,Vector3.Distance(transform.position, transform.GetChild(0).position))));
				}
				GetComponent<LineRenderer>().SetPosition(0, transform.position);
				GetComponent<LineRenderer>().SetPosition(1, transform.GetChild(0).position);
				
			}
		}
	}
	
	public bool idcheck(int i, int i2){
		if(i == id && i2 == id2)return true;
		return false;
	}
	
	void OnCollisionEnter(Collision col){
		if(id == 1){
			if(id2 == 0 || id2 == 2){
				if(!held && col.collider.gameObject.layer == 13){
					custombool[0] = true;
				}
			}
		}
	}
	
	public void onpickup(){
		if(invhold != null)invhold.GetComponent<inventoryhelper>().holding = false;
		invhold = null;
		if(id == 1){
			if(id2 == 0 || id2 == 2){
				custombool[0] = false;
			}
		}
	}
	
	public void ondrop(){
		
	}
	
	public void varuse(){
		if(id == 1){
			if(id2 == 0){
				if(vartrans.Count == 0){ //spawn a new point
					Transform clone = Instantiate(gameObject).transform;
					clone.position = transform.position + new Vector3(0, 0.4f,0);
					clone.GetComponent<item>().id = 1;
					clone.GetComponent<item>().id2 = 2;
					clone.GetComponent<item>().makeitem(1,2);
					clone.GetComponent<item>().held = false;
					clone.parent = master.items;
					clone.GetComponent<Rigidbody>().useGravity = true;
					vartrans.Add(clone);
					clone.GetComponent<item>().vartrans.Add(transform);
				} else { // break old point
					Destroy(vartrans[0].gameObject, 0.01f);
					vartrans.RemoveAt(0);
					transform.GetChild(1).GetComponent<BoxCollider>().isTrigger = false;
				}
			} else if(id2 == 1){
				if(vartrans.Count != 2){
					vartrans[2].GetComponent<Rigidbody>().velocity += holdpoints[0].right*Mathf.Pow(Mathf.Max((Vector3.Distance(vartrans[0].position,vartrans[2].position) + Vector3.Distance(vartrans[1].position,vartrans[2].position))*3,1),2);
					vartrans.RemoveAt(2);
					//StartCoroutine(varcor());
				}
			} else if(id2 == 2){
				if(vartrans.Count != 0){
					vartrans[0].GetComponent<item>().vartrans.RemoveAt(0);
					Transform clone = Instantiate(vartrans[0].gameObject).transform;
					clone.position = transform.position;
					clone.parent = master.items;
					clone.GetComponent<Rigidbody>().useGravity = true;
					clone.GetComponent<Rigidbody>().isKinematic = false;
					clone.GetComponent<item>().makeitem(1,0);
					Destroy(vartrans[0].gameObject);
					Destroy(gameObject);
					par.GetComponent<ViveControllerVR>().forceGrabObject(clone);
					par.GetComponent<ViveControllerVR>().ReleaseObject();
					//add so you are holding the copy
				}
			} else if (id2 == 3){
				RaycastHit hit;
				if(Physics.Raycast(transform.position, transform.forward, out hit, 32.5f, mask1)){
					varvec.Add(hit.point);
					varfloat.Add(0.0f);
				}
			}
		}
	}
	
	public void varuse2(){
		if(id == 1){
			if(id2 == 0){
				
			} else if(id2 == 1){
				
			} else if (id2 == 2){
				
			} else if (id2 == 3){
				varvec.Clear();
				varfloat.Clear();
				custombool[0] = false;
				par.GetComponent<ViveControllerVR>().hookto = false;
			}
		}
	}
	
	public IEnumerator varcor(){
		yield return new WaitForEndOfFrame();
		if(id == 1){
			if(id2 == 0){
				yield return new WaitForSeconds(0.1f);
				GetComponent<BoxCollider>().isTrigger = false;
			}
		}
	}
	
	public int containseffect(int i, bool b){
		int inc = 0;
		foreach(int[] i2 in effects){
			if(i2[0] == i){
				if(b){inc += i2[1];
				} else {inc = 1;
				}
			}
		}
		return inc;
	}
	
	public void resetscal(){
		if(id == 0){
			scal = new Vector3(0.2f,0.2f,0.2f);
		} else if(id == 1){
			scal = new Vector3(1,1,1);
		}
	}
	
	public void makeitem(int i, int i2){
		//Debug.Log("WHAT");
		if(i == 0){ //block
			Material mat = new Material(master.mat1);
			mat.mainTexture = master.matslist1[i2];
			gameObject.GetComponent<Renderer>().material = mat;
			scal = new Vector3(0.2f,0.2f,0.2f);
					
		} else if(i == 1){ //item
			if(i2 == 0){ //grappling hook
				gameObject.AddComponent<LineRenderer>();
				GetComponent<LineRenderer>().SetPosition(0, transform.position);
				GetComponent<LineRenderer>().SetPosition(1, transform.position);
				GetComponent<LineRenderer>().startWidth = 0.1f;
				GetComponent<LineRenderer>().endWidth = 0.1f;
				custombool.Clear();
				custombool.Add(false);
				transform.GetChild(1).GetComponent<BoxCollider>().isTrigger = true;
				//scal = new Vector3(0.1f,0.1f,0.1f);
			} else if(i2 == 1){ //bow
				vartrans.Add(transform.GetChild(2));
				vartrans.Add(transform.GetChild(3));
				gameObject.AddComponent<LineRenderer>();
				GetComponent<LineRenderer>().positionCount = 3;
				GetComponent<LineRenderer>().SetPosition(0, vartrans[0].position);
				GetComponent<LineRenderer>().SetPosition(1, vartrans[1].position);
				GetComponent<LineRenderer>().SetPosition(2, vartrans[1].position);
				GetComponent<LineRenderer>().startWidth = 0.003f;
				GetComponent<LineRenderer>().endWidth = 0.003f;
			} else if(i2 == 2){ //grappling hook hold thing
				Destroy(GetComponent<LineRenderer>());
				Destroy(transform.GetChild(1).gameObject);
				Destroy(transform.GetChild(0).gameObject);
			} else if(i2 == 3){ //grappling hook
				gameObject.AddComponent<LineRenderer>();
				GetComponent<LineRenderer>().SetPosition(0, transform.position);
				GetComponent<LineRenderer>().SetPosition(1, transform.GetChild(0).position);
				GetComponent<LineRenderer>().startWidth = 0.1f;
				GetComponent<LineRenderer>().endWidth = 0.1f;
				custombool.Add(false);
				//scal = new Vector3(0.1f,0.1f,0.1f);
			}
		}
		/*if(i == 0){//rock
			//technical
			scal = new Vector3(0.2f,0.2f,0.2f);
			//model = Resources.Load("3D/items/rock") as Transform;
			//obj = Resources.Load("3D/models/rock") as Transform;
			//stats
			effects.Add(new int[]{1, 0, -1, 1, 4, 1}); //damage
			effects.Add(new int[]{2, 3});//item type
			
		} else if(i == 1){ //testhelmet
			//model = Resources.Load("3D/items/helmet") as Transform;
			//obj = Resources.Load("3D/models/helmet") as Transform;
			//Debug.Log("what " + (Resources.Load("3D/models/helmet") as Transform));
			effects.Add(new int[]{2, 1});//helmet type
			effects.Add(new int[]{3, 5, -1, 1, 100});
		} else if(i == 2){//test armor
			effects.Add(new int[]{2, 2});//body armor type
			effects.Add(new int[]{3, 25, -1, 1, 100});
		} else if(i == 3){//test sword
			scal = new Vector3(1.2f,1.2f,1);
			effects.Add(new int[]{1, 0, -1, 1, 8, 1}); //damage
			effects.Add(new int[]{2, 0});//item type
			cooldown = 10;
		} else if(i == 4){//torch
			effects.Add(new int[]{1, 0, -1, 1, 4, 2}); //damage
			effects.Add(new int[]{2, 3});//item type
			cooldown = 10;
		}*/
		
	}
	
	public void glow(){
		if(transform.GetChild(0).childCount == 0){
			Transform clone = Instantiate(model);
			clone.position = transform.GetChild(0).position;
			clone.rotation = transform.GetChild(0).rotation;
			clone.parent = transform.GetChild(0);
			clone.localScale = transform.GetChild(0).localScale + new Vector3(0.01f,0.01f,0.01f);
		}
	}
	
	public void dontglow(){
		if(transform.GetChild(0).childCount != 0)Destroy(transform.GetChild(0).GetChild(0).gameObject);
	}
	
	
	
}
