using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellmaster : MonoBehaviour {
	public Transform[] catagories = new Transform[7];
	public Transform[] subcatas = new Transform[29];
	public List<int> spellids = new List<int>();
	public int equipingspell = 0;
	public int whichcata = -1;
	public int whichsub = -1;
	LineRenderer line;
	public Transform cam1;
	public Vector3 snap = Vector3.zero;
	public Transform lastcollider;
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		for(int i = 0;i < 7; i++){
			catagories[i] = transform.GetChild(i);
		}
	}
	
	
	// Update is called once per frame
	void LateUpdate () {
		if(equipingspell != 0){
			line.positionCount = spellids.Count;
			int temp = 0;
			foreach(int i in spellids){
				if(i >= 0 && i <= 6){
					line.SetPosition(temp, catagories[i].position);
				} else {
					line.SetPosition(temp, subcatas[i-7].position);
				}
				temp++;
			}
			if(snap == Vector3.zero){ 
				snap = cam1.position + (cam1.forward);
				transform.position = snap;
				transform.LookAt(cam1, Vector3.up);
			} else {
				transform.position = snap;
			}
		} else {
			line.positionCount = 0;
			
		}
		if(equipingspell == 1){
			foreach(Transform t in catagories){
				t.GetComponent<MeshRenderer>().enabled = true;
				t.GetComponent<spellslave>().on = true;
			}
			equipingspell = 2;
		} else if(equipingspell == 2){
			if(whichcata != -1){
				foreach(Transform t in transform.GetChild(whichcata)){
					t.GetComponent<MeshRenderer>().enabled = true;
					t.GetComponent<spellslave>().on = true;
				}
				foreach(Transform t in catagories){
					if(t != transform.GetChild(whichcata)){
						t.GetComponent<MeshRenderer>().enabled = false;
						t.GetComponent<spellslave>().on = false;
					}
				}
				spellids.Add(whichcata);
				equipingspell = 3;
			}
		} else if(equipingspell == 3){
			if(whichsub != -1 && spellids[spellids.Count-1] != whichsub){
				spellids.Add(whichsub);
			}
			if(whichcata != -1 && spellids[spellids.Count-1] != whichcata){
				spellids.Add(whichcata);
			}
		} else if(equipingspell == 4){
			//equip spell here
			doequipspell();
			
			foreach(Transform t in subcatas){
					t.GetComponent<MeshRenderer>().enabled = false;
					t.GetComponent<spellslave>().on = false;
				}
				foreach(Transform t in catagories){
					t.GetComponent<MeshRenderer>().enabled = false;
					t.GetComponent<spellslave>().on = false;
				}
			
			equipingspell = 0;
			whichcata = -1;
			whichsub = -1;
			spellids.Clear();
			snap = Vector3.zero;
		}
	}
	
	void doequipspell(){
		int temp = spellfindid();
		//Debug.Log(temp);
		if(temp != -1 && lastcollider.GetComponent<spell>() == null){
			//Debug.Log("yes");
			lastcollider.gameObject.AddComponent<spell>();
			lastcollider.GetComponent<spell>().id = temp;
			lastcollider.GetComponent<spell>().makespell();
		}
	}
	
	int spellfindid(){
		//telekinisis
		if(spellids.Count == 2 && spellids[0] == 1 && spellids[1] == 13){ 
			return 0;
		}
		//fly
		if(spellids.Count == 2 && spellids[0] == 3 && spellids[1] == 16){
			return 1;
		}
		//grapple vine
		if(spellids.Count == 2 && spellids[0] == 5 && spellids[1] == 24){
			return 2;
		}
		return -1;
	}
}
