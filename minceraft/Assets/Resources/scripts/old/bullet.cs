using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	[SerializeField]
	public int weap = 0;

	public Vector3 lastframepos = new Vector3(0,0,0);
	public bool firstframe = true;
	public bool done = false;
	public RaycastHit hit;
	public float timer = 0.0f;


	public int damage(){
		return weapons.weaponlist [weap].damage + Random.Range (-weapons.weaponlist [weap].damagerange, weapons.weaponlist [weap].damagerange + 1);
	}

	public int crit(){
		if (Random.Range (0, 101) < weapons.weaponlist [weap].crit) {
			return weapons.weaponlist [weap].critdamage;
		}
		return 0;
	}
	// Use this for initialization
	void Start () {
		
	}
		
	void remove(){
		transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		done = true;
		Destroy (this.gameObject, 0.5f);
	}

	void dodamage(Transform t){
		//Debug.Log (t);
		if (t.parent.GetComponent<unit> ()) {
			t.parent.GetComponent<unit> ().hp -= damage()+crit();
			//transform.GetComponent<LineRenderer> ().SetPosition (1, t.position);
			transform.GetComponent<LineRenderer> ().SetPosition (1, hit.point);
		}
		remove();
	}

	void dopen(Transform t){

		//Debug.Log (t);
		if (t.GetComponent<terr> ()) {
			//Debug.Log (t + " " + t.GetComponent<terr> ().penchance);
			if (Random.Range (1, 101) < (t.GetComponent<terr> ().penchance+weapons.weaponlist[weap].bulletpenflat)*weapons.weaponlist[weap].bulletpenmult) {
				//reduce damage? figure this out later
				t.GetComponent<terr>().hp -= weapons.weaponlist[weap].envdam;
			} else {
				t.GetComponent<terr>().hp -= weapons.weaponlist[weap].envdam;
				transform.GetComponent<LineRenderer> ().SetPosition (1, hit.point);
				remove();
			}
		} else {
			remove();
		}

	}

	// Update is called once per frame
	void Update () {
		if(!done)transform.GetComponent<LineRenderer> ().SetPosition (1, this.transform.position);
		if (done) {
			timer += Time.deltaTime * 2;
			transform.GetComponent<LineRenderer> ().SetPosition (0, Vector3.Lerp (transform.GetComponent<LineRenderer> ().GetPosition (0), transform.GetComponent<LineRenderer> ().GetPosition (1), timer));
		}

	}

	void LateUpdate(){
		if (!firstframe && !done) {
			if (Physics.SphereCast (lastframepos, 0.01f, this.transform.forward, out hit, Vector3.Distance(lastframepos,this.transform.position), 1 << 9)) {
				dodamage (hit.collider.transform);
			}
			if (Physics.SphereCast (lastframepos, 0.01f, this.transform.forward, out hit, Vector3.Distance(lastframepos,this.transform.position), 1 << 10)) {
				dopen (hit.collider.transform);
			}
		}
		firstframe = false;
		lastframepos = this.transform.position;
	}
}
