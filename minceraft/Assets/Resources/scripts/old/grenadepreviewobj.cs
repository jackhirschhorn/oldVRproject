using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadepreviewobj : MonoBehaviour {

	[SerializeField]

	public Vector3 lastframepos = new Vector3(0,0,0);
	public bool firstframe = true;
	public bool done = false;
	public RaycastHit hit;
	public float timer = 0.0f;
	public Transform lasthelper;

	void remove(){
		transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		//transform.GetComponent<Rigidbody> ().useGravity = false;
		done = true;
		//Destroy (this.gameObject, 0.11f);
	}


	// Update is called once per frame
	void Update () {
		/*if(!done)transform.GetComponent<LineRenderer> ().SetPosition (1, this.transform.position);
		if (lasthelper != null) {
			transform.GetComponent<LineRenderer> ().SetPosition (0, lasthelper.transform.position);
		} else {
			transform.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
		}*/

	}

	void LateUpdate(){
		if (!firstframe && !done) {
			if (Physics.SphereCast (lastframepos, 0.01f, this.transform.forward, out hit, Vector3.Distance(lastframepos,this.transform.position), 1 << 10)) {
				transform.position = hit.point + new Vector3(0,transform.GetComponent<SphereCollider>().radius*transform.localScale.y,0);
				remove();
			}
		}
		firstframe = false;
		lastframepos = this.transform.position;
	}
}
