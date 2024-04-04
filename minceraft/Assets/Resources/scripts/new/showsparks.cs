using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showsparks : MonoBehaviour {

	// Use this for initialization

	float speed = 0.0f;
	Vector3 mLastPosition;
	
	public float minspeed = 0.0f;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		speed = (transform.position - this.mLastPosition).magnitude / Time.deltaTime;
		mLastPosition = transform.position;
	}
	
	void OnCollisionEnter(Collision col){
		if(col.collider.sharedMaterial != null && col.collider.sharedMaterial.name == "hard" && speed > minspeed){
			GetComponent<ParticleSystem>().Play(false);
			GetComponent<AudioSource>().Play(0);
		}
	}
}
