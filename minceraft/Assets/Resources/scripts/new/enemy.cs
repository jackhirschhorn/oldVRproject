using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

	public List<int[]> effects = new List<int[]>();
	
	public bool invincable = false;

	public void takedamage(int i, int i2){ //damage, type
		if(!invincable)die();
		
	}
		
		
	public void die(){
		foreach(FixedJoint j in GetComponents<FixedJoint>()){
			j.connectedBody.transform.GetComponent<item>().par = wheregoes.tnone;
			j.connectedBody.transform.GetComponent<item>().onentity = false;
			j.connectedBody.isKinematic = false;
	
		}
		Destroy(this.gameObject, 0.01f);
		//GetComponent<Rigidbody>().velocity = new Vector3(100,100,100);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(timer());
	}
	
	public IEnumerator timer(){
		while(true){
			for(int i = 0; i < effects.Count; i++){
				if(effects[i].Length > 2 && effects[i][2] > 0){
					effects[i][2] -= 1;
					if(effects[i][2] == 0){
						if(effects[i][0] == -1)invincable = false;
						effects.RemoveAt(i);
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
