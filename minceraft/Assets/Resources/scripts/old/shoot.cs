using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {
	/*
	[SerializeField]
	public Transform bullet;
	public Transform clone;
	public float timer = 1.0f;
	public float countdown = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*countdown -= Time.deltaTime;
		if (countdown < 0) {
			clone = Instantiate (bullet, this.transform.Find("out").transform.position, this.transform.Find("out").transform.rotation);
			clone.GetComponent<Rigidbody> ().AddForce (transform.forward * 1000);
			countdown = timer;
		}
	}
	*/
	public IEnumerator fire(int i){ //weapon
		/*for (int i2 = 0; i2 < weapons.weaponlist[i].bullets; i2++) {
			clone = Instantiate (bullet, this.transform.Find("out").transform.position, this.transform.Find("out").transform.rotation);
			//var rando = Random.Range(1, 101);
			clone.transform.Rotate (Random.Range (-weapons.weaponlist[i].spread, weapons.weaponlist[i].spread+1)* ViveControllerVR.scl, Random.Range (-weapons.weaponlist[i].spread , weapons.weaponlist[i].spread+1 )* ViveControllerVR.scl, 0.0f);
			clone.transform.GetComponent<bullet>().lastframepos = this.transform.position;
			clone.transform.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
			clone.transform.GetComponent<bullet> ().weap = i;
			clone.GetComponent<Rigidbody> ().AddForce (clone.transform.forward * 1000);
			if(weapons.weaponlist[i].firedelay != 0)yield return new WaitForSeconds(weapons.weaponlist[i].firedelay);
		}*/
		yield return new WaitForEndOfFrame();
	}
	
}
