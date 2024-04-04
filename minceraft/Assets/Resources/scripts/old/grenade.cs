using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour {

	[SerializeField]
	public bool showpreview = false;
	public float timer = 0.2f;
	public float countdown = 0.0f;
	public Transform unitsel;
	public Transform previewobj;
	public RaycastHit hit;
	public bool launchgrenade = false;
	public LineRenderer line;
	public Transform point;

	private Transform clone;
	private Transform clone2;
	private Vector3[] arc = new Vector3[]{
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(0,0,0)
	};

	// Use this for initialization
	void Start () {
		
	}



	// Update is called once per frame
	void Update () {
		if (showpreview) {
			point.position = hit.point;
			line.enabled = true;
			Vector3 middlepoint = Vector3.Lerp(unitsel.Find ("body/gun/out").position,point.position,0.5f)+new Vector3(0,((Vector3.Distance(unitsel.Find ("body/gun/out").position,point.position)*100.0f)/30.0f)*(1-Mathf.Cos(15.0f* Mathf.Deg2Rad))*1.0f,0);
			arc [0] = unitsel.Find ("body/gun/out").position;
			arc [1] = Vector3.Lerp(unitsel.Find ("body/gun/out").position,middlepoint,0.5f)+new Vector3(0,((Vector3.Distance(unitsel.Find ("body/gun/out").position,middlepoint)*100.0f)/30.0f)*(1-Mathf.Cos(15.0f* Mathf.Deg2Rad))*0.5f,0);
			arc [2] = middlepoint;
			arc [3] = Vector3.Lerp(middlepoint,point.position,0.5f)+new Vector3(0,((Vector3.Distance(middlepoint,point.position)*100.0f)/30.0f)*(1-Mathf.Cos(15.0f* Mathf.Deg2Rad))*0.5f,0);
			arc [4] = point.position;
			Debug.Log ((Vector3.Distance (unitsel.Find ("body/gun/out").position, point.position) / 30) * (1 - Mathf.Cos (15 * Mathf.Deg2Rad)));
			line.SetPositions (arc);
			if(launchgrenade){
				//if (clone2 != null && clone != null)clone.GetComponent<grenadepreviewobj> ().lasthelper = clone2;
				//clone2 = clone;
				clone = Instantiate (previewobj, unitsel.Find("body/gun/out").position, unitsel.Find("body/gun/out").rotation);
				//clone.GetComponent<Rigidbody> ().velocity = ((((hit.point-unitsel.Find("body/gun/out").position)/2.0f)+new Vector3(0,Vector3.Distance(hit.point,unitsel.Find("body/gun/out").position)/3.0f,0))*1.0f);//*(Vector3.Distance((((hit.point-unitsel.Find("body/gun/out").position)/2.0f)+new Vector3(0,Vector3.Distance(hit.point,unitsel.Find("body/gun/out").position)/3.0f,0)),unitsel.Find("body/gun/out").position)+Vector3.Distance((((hit.point-unitsel.Find("body/gun/out").position)/2.0f)+new Vector3(0,Vector3.Distance(hit.point,unitsel.Find("body/gun/out").position)/3.0f,0)),hit.point)));
				//clone.GetComponent<Rigidbody>().velocity = new Vector3(Vector3.Distance(hit.point,unitsel.Find("body/gun/out").position)*100*Mathf.Cos(30.0f), Vector3.Distance(hit.point,unitsel.Find("body/gun/out").position)*100*Mathf.Sin(30.0f), 0f);
				var rigid = clone.GetComponent<Rigidbody>();
		 
		        Vector3 p = hit.point;
		 
		        float gravity = Physics.gravity.magnitude;
		        // Selected angle in radians
		        float angle = 30 * Mathf.Deg2Rad;
		 
		        // Positions of this object and the target on the same plane
		        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
		        Vector3 planarPostion = new Vector3(clone.transform.position.x, 0, clone.transform.position.z);
		 
		        // Planar distance between objects
		        float distance = Vector3.Distance(planarTarget, planarPostion);
		        // Distance along the y axis between objects
		        float yOffset = clone.transform.position.y - p.y;
		 
				float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt(Mathf.Abs((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset)));
		 
		        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
		 
		        // Rotate our velocity to match the direction between the two objects
				float angleBetweenObjects = Vector3.SignedAngle(Vector3.forward, planarTarget - planarPostion,Vector3.up);
				Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
		        // Fire!
		        rigid.velocity = finalVelocity;
				Debug.Log(angleBetweenObjects);

				//clone.GetComponent<grenadepreviewobj> ().lasthelper = unitsel.transform.Find("body/gun/out");
				//countdown = timer;
				showpreview = false;
				launchgrenade = false;
				line.enabled = false;
			}
		}
	}
}
