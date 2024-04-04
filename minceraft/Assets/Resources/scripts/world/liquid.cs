using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liquid : MonoBehaviour {
	public int id; //id,-1 = air, water = 0, lava = 2, ect
	public bool[] sources = new bool[7]; //where is the source? north/west/south/east/up/down(pressure)/none
	public int level; //water level
	public int inf; //is liquid infinite(copies full ammount, ie: ocean), or finite (flows)
	public Transform lvl;
	public int mask = (1 << 8) | (1 << 9) | (1 << 11) | (1 << 13); //comment these out when updating them...
	public int mask2 = (1 << 8) | (1 << 9) | (1 << 11);
	public bool settled = false;
	public int drawn = 0; // 0 = never drawn, 1 = drawn, 2 = needs redraw;
	public int passcount = 0;
	
	
	void Update(){
		
	}
	public bool tickon = false;
	public int ticknum = 1;
	
	public void dotick(int i){
		//if(!settled)flow(); //fix later
	}
	
	public void buildliquid(){
		ticknum = Random.Range(1,11);
	}
	
	/*public void checksurround(bool b){
		if(b){
			if(!checkblock(5)){
				foreach(Transform child in transform){
					child.GetComponent<Renderer>().enabled = false;
					child.GetChild(0).GetComponent<Renderer>().enabled = false;
				}
			}
		} else {
			if(!checkblock(0) && !checkblock(1) && !checkblock(2) && !checkblock(3) && !checkblock(4) &&!checkblock(5)){
				foreach(Transform child in transform){
					child.GetComponent<Renderer>().enabled = false;
					child.GetChild(0).GetComponent<Renderer>().enabled = false;
				}
			}
		}
	}*/

	/*public bool outchunk(int i){
		Transform clone1;
		clone1 = Instantiate(master.testbox);
		clone1.parent = transform.parent;
		clone1.localPosition = transform.localPosition+new Vector3((i == 0?(1.0f/16.0f):(i == 2?-(1.0f/16.0f):0)), (i == 4?-(1.0f/16.0f):0), (i == 1?(1.0f/16.0f):(i == 3?-(1.0f/16.0f):0)));
		if(boxContains(Physics.OverlapBox(transform.parent.localPosition, transform.parent.localScale /2, Quaternion.identity, (1 << 12)),clone1)){
			Destroy(clone1.gameObject);
			return false;
		} else {
			Destroy(clone1.gameObject);
			return true;
		}
	}
	
	public bool boxContains(Collider[] c, Transform t){
		foreach(Collider co in c){
			if(co.transform == t){
				return true;
			}
		}
		return false;
	}
	
	public Transform newchunk(int i){ //don't make new chunks if there's already a chunk there
		if(neednewchunk(transform.parent.localPosition + new Vector3((i == 0?1:(i == 2?-1:0)), (i == 4?-1:0), (i == 1?1:(i == 3?-1:0)))) == null){
			Transform clone;
			clone = Instantiate(master.minichunkliquidprefab);
			clone.parent = master.miniliquids;
			clone.localPosition = transform.parent.localPosition + new Vector3((i == 0?1:(i == 2?-1:0)), (i == 4?-1:0), (i == 1?1:(i == 3?-1:0)));
			clone.GetComponent<minichunkl>().ticknum = Random.Range(1,11);
			return clone;
		} else {
			return neednewchunk(transform.parent.localPosition + new Vector3((i == 0?1:(i == 2?-1:0)), (i == 4?-1:0), (i == 1?1:(i == 3?-1:0))));
		}
	}
	
	public Transform neednewchunk(Vector3 v){
		foreach(Transform child in master.miniliquids){
			if(child.localPosition == v)return child;
		}
		return null;
	}
	
	public void flow (){
		if(inf == 0){ //ocean infinite
			if(!mini && Physics.Raycast(transform.position, new Vector3(1,0,0), 1.0f, 1 << 11)){
				foreach(Vector3 vec in checkblockbigtosmall(0)){
					master.addliquid(vec, id, 16, inf, 2, true, (outchunk(0)?newchunk(0):transform.parent));
				}
			} else {
				if(checkblock(0)){ // north
					master.addliquid(transform.position + new Vector3((mini?1.0f/16.0f:1),0,0), id, level, inf, 2, mini, (mini?(outchunk(0)?newchunk(0):transform.parent):transform.parent));
				}
			}
			if(!mini && Physics.Raycast(transform.position, new Vector3(0,0,1), 1.0f, 1 << 11)){
				foreach(Vector3 vec in checkblockbigtosmall(1)){
					master.addliquid(vec, id, 16, inf, 2, true, (outchunk(0)?newchunk(0):transform.parent));
				}
			} else {
				if(checkblock(1)){ // west
					master.addliquid(transform.position + new Vector3(0,0,(mini?1.0f/16.0f:1)), id, level, inf, 3, mini, (mini?(outchunk(1)?newchunk(1):transform.parent):transform.parent));
				}
			}
			if(!mini && Physics.Raycast(transform.position, new Vector3(-1,0,0), 1.0f, 1 << 11)){
				foreach(Vector3 vec in checkblockbigtosmall(2)){
					master.addliquid(vec, id, 16, inf, 2, true, (outchunk(0)?newchunk(0):transform.parent));
				}
			} else {
				if(checkblock(2)){ // south
					master.addliquid(transform.position + new Vector3((mini?-(1.0f/16.0f):-1),0,0), id, level, inf, 0, mini, (mini?(outchunk(2)?newchunk(2):transform.parent):transform.parent));
				}
			}
			if(!mini && Physics.Raycast(transform.position, new Vector3(0,0,-1), 1.0f, 1 << 11)){
				foreach(Vector3 vec in checkblockbigtosmall(3)){
					master.addliquid(vec, id, 16, inf, 2, true, (outchunk(0)?newchunk(0):transform.parent));
				}
			} else {
				if(checkblock(3)){ // east
					master.addliquid(transform.position + new Vector3(0,0,(mini?-(1.0f/16.0f):-1)), id, level, inf, 1, mini, (mini?(outchunk(3)?newchunk(3):transform.parent):transform.parent));
				}
			}
			if(!mini && Physics.Raycast(transform.position, new Vector3(0,-1,0), 1.0f, 1 << 11)){
				foreach(Vector3 vec in checkblockbigtosmall(4)){
					master.addliquid(vec, id, 16, inf, 2, true, (outchunk(0)?newchunk(0):transform.parent));
				}
			} else {
				if(checkblock(4)){ // down
					master.addliquid(transform.position + new Vector3(0,(mini?-(1.0f/16.0f):-1),0), id, level, inf, 4, mini, (mini?(outchunk(4)?newchunk(4):transform.parent):transform.parent));
				}
			}
			if(checkblock(5)){ // up, pressure
				master.addliquid(transform.position + new Vector3(1,0,0), id, level, inf);
			}
			if(!checkblock(0) && !checkblock(1) && !checkblock(2) && !checkblock(3) && !checkblock(4)){
				settled = true;
				checksurround(true);
			}
		}
	}
	
	public bool checkblock(int i){
		if(i == 0){ //north
			if(Physics.Raycast(transform.position, new Vector3(1,0,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		if(i == 1){ //west
			if(Physics.Raycast(transform.position, new Vector3(0,0,1), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		if(i == 2){ //south
			if(Physics.Raycast(transform.position, new Vector3(-1,0,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		if(i == 3){ //east
			if(Physics.Raycast(transform.position, new Vector3(0,0,-1), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		if(i == 4){ //down
			if(Physics.Raycast(transform.position, new Vector3(0,-1,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		if(i == 5){ //up
			if(Physics.Raycast(transform.position, new Vector3(0,1,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				return false;
			}
		}
		return true;
	}
	
	public List<Vector3> checkblockbigtosmall(int i){
		List<Vector3> vecs = new List<Vector3>();
		if(i == 0){ //north
			for(int i2 = 1; i2 <= level; i2++){
				for(int i3 = 1; i2 <= 16; i3++){
					if(Physics.Raycast(transform.position+ new Vector3(((-17.0f/32.0f)+((i3 +0.0f)/16.0f)),((-17.0f/32.0f)+((i2 +0.0f)/16.0f)),0), new Vector3(1,0,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
						//vecs.Add(transform.position+
					}
				}
			}
		}
		if(i == 1){ //west
			if(Physics.Raycast(transform.position, new Vector3(0,0,1), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				//return false;
			}
		}
		if(i == 2){ //south
			if(Physics.Raycast(transform.position, new Vector3(-1,0,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				//return false;
			}
		}
		if(i == 3){ //east
			if(Physics.Raycast(transform.position, new Vector3(0,0,-1), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				//return false;
			}
		}
		if(i == 4){ //down
			if(Physics.Raycast(transform.position, new Vector3(0,-1,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				//return false;
			}
		}
		if(i == 5){ //up
			if(Physics.Raycast(transform.position, new Vector3(0,1,0), (mini?1.0f/16.0f:1), (mini?mask2:mask))){
				//return false;
			}
		}
		return vecs;
	}*/
}
