using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour {

	[SerializeField]
	public bool isgrabbed = false;
	public bool playerteam = true;
	public int hp = 5;
	public int move = 5;
	public int weapon = 0; //0 = shotgun, 1 = sniper, 2 = lmg, 3 = rifle NOTE: these are weapon archtypes, not weapon stats
	public int primary = 0; //0 = shotgun, 1 = sniper, 2 = lmg, 3 = rifle
	public int secondary = 0; //0 = sawed off, 1 = revolver, 2 = machine pistol, 3 = pistol
	public bool onsecondary = false; // is currently using secondary
	public Transform gun;
	public Transform gunmodel1;
	public Transform gunmodel2;
	public Material basemat;
	public Material glowmat;
	public Transform bod;

	private Transform aimatunit;
	private Vector3 frontaim;
	private float timer = 1.0f;
	private float countdown = 0.0f;
	private float looktimer = 0.0f;
	private RaycastHit hit;
	private RaycastHit[] hits;
	private Transform clone;

	// Use this for initialization
	void Start () {
		gunmodel1 = Instantiate (weapons.weaponlist [primary].weapmodel.transform, gun.position, gun.rotation, gun);
		gunmodel2 = Instantiate (weapons.weaponlist [secondary].weapmodel.transform, gun.position, gun.rotation, gun);
		gunmodel2.GetComponent<Renderer> ().enabled = false;
		foreach (Transform child in gunmodel2) {
			child.GetComponent<Renderer> ().enabled = false;
		}
		
	}

	public void swapweap(){
		onsecondary = !onsecondary;
		if (onsecondary) {
			weapon = weapons.weaponlist [secondary].weapontype;
			gunmodel2.GetComponent<Renderer> ().enabled = true;
			foreach (Transform child in gunmodel2) {
				child.GetComponent<Renderer> ().enabled = true;
			}
			gunmodel1.GetComponent<Renderer> ().enabled = false;
			foreach (Transform child in gunmodel1) {
				child.GetComponent<Renderer> ().enabled = false;
			}
		} else {
			weapon = weapons.weaponlist [primary].weapontype;
			gunmodel2.GetComponent<Renderer> ().enabled = false;
			foreach (Transform child in gunmodel2) {
				child.GetComponent<Renderer> ().enabled = false;
			}
			gunmodel1.GetComponent<Renderer> ().enabled = true;
			foreach (Transform child in gunmodel1) {
				child.GetComponent<Renderer> ().enabled = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			//do death effect later
			Destroy(transform.parent.gameObject);
		}
		countdown -= Time.deltaTime;
		looktimer += Time.deltaTime;
		if (isgrabbed) {
			countdown = timer;
			aimatunit = null;
			looktimer = 0;
			frontaim = (transform.Find ("body/gun").position);
		} else {
		}
		if (countdown < 0) {
			aimatunit = null;
			frontaim = (transform.Find ("body/gun").position);
			looktimer = 0;
			aimatenemy ();
			countdown = timer;
		}
	}

	public void isgrabbedfunc(){
		//transform.rotation = Quaternion.Euler (0.0f, transform.rotation.eulerAngles.y, 0.0f);
		transform.parent.Find("m").localScale = new Vector3(move,move,move);
		transform.parent.Find ("m").GetComponent<MeshRenderer> ().enabled = true;
		transform.parent.Find("m2").localScale = new Vector3(move*2,move*2,move*2);
		transform.parent.Find ("m2").GetComponent<MeshRenderer> ().enabled = true;
		//bod.GetComponent<MeshRenderer> ().material = glowmat;

	}

	public void isreleasedfunc(){
		transform.parent.Find ("m").GetComponent<MeshRenderer> ().enabled = false;
		transform.parent.Find ("m2").GetComponent<MeshRenderer> ().enabled = false;
		//bod.GetComponent<MeshRenderer> ().material = basemat;
	}

	public void resetmove(){
		transform.parent.position = transform.position;
		transform.localPosition = new Vector3 (0, 0, 0);
	}

	public void attack(){
		if (onsecondary) {
			StartCoroutine (gun.GetComponent<shoot> ().fire (secondary));
		} else {
			StartCoroutine (gun.GetComponent<shoot> ().fire (primary));
		}
			
	}

	public void aimatenemy(){
		if (playerteam) { //player
			if (transform.parent.parent.parent.Find ("enemy").childCount != 0) {
				var aimatunitchance = 1.0f;
				var aimatunitchancecomp = 1.0f;
				foreach (Transform child in transform.parent.parent.parent.Find ("enemy")) {
					aimatunitchancecomp = 1.0f;
					if (aimatunit == null) {
						aimatunit = child.Find ("unit/body");
						hits = Physics.SphereCastAll (gun.position, 0.01f, aimatunit.position-gun.position, Vector3.Distance (gun.position, aimatunit.position), 1 << 10); 
						for (int inc = 0; inc < hits.Length; inc++) {
							if(hits[inc].collider.transform.GetComponent<terr>())aimatunitchance *= hits[inc].collider.transform.GetComponent<terr>().penchance*0.01f;
						}

					} else {
						hits = Physics.SphereCastAll (gun.position, 0.01f, child.Find ("unit/body").position-gun.position, Vector3.Distance (gun.position, child.Find ("unit/body").position), 1 << 10); 
						for (int inc = 0; inc < hits.Length; inc++) {
							if(hits[inc].collider.transform.GetComponent<terr>())aimatunitchancecomp *= hits[inc].collider.transform.GetComponent<terr>().penchance*0.01f;
						}
						if (aimatunitchance < aimatunitchancecomp) {
							aimatunitchance = aimatunitchancecomp;
							aimatunit = child.Find ("unit/body");
							looktimer = 0;
							frontaim = (transform.Find ("body/gun").position);
						}
						if (aimatunitchance == aimatunitchancecomp) {
							if (weapon == 0 && Vector3.Distance (transform.position, aimatunit.transform.position) > Vector3.Distance (transform.position, child.transform.position)) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 1 && Vector3.Distance (transform.position, aimatunit.transform.position) < Vector3.Distance (transform.position, child.transform.position)) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 2 && aimatunit.parent.GetComponent<unit> ().hp < child.Find ("unit").GetComponent<unit> ().hp) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 3 && aimatunit.parent.GetComponent<unit> ().hp > child.Find ("unit").GetComponent<unit> ().hp) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
						}
					}
				}
			}
		} else { // update when done with player aim!
			if (transform.parent.parent.parent.Find ("player").childCount != 0) {
				var aimatunitchance = 1.0f;
				var aimatunitchancecomp = 1.0f;
				foreach (Transform child in transform.parent.parent.parent.Find ("player")) {
					aimatunitchancecomp = 1.0f;
					if (aimatunit == null) {
						aimatunit = child.Find ("unit/body");
						hits = Physics.SphereCastAll (gun.position, 0.01f, aimatunit.position-gun.position, Vector3.Distance (gun.position, aimatunit.position), 1 << 10); 
						for (int inc = 0; inc < hits.Length; inc++) {
							if(hits[inc].collider.transform.GetComponent<terr>())aimatunitchance *= hits[inc].collider.transform.GetComponent<terr>().penchance*0.01f;
						}

					} else {
						hits = Physics.SphereCastAll (gun.position, 0.01f, child.Find ("unit/body").position-gun.position, Vector3.Distance (gun.position, child.Find ("unit/body").position), 1 << 10); 
						for (int inc = 0; inc < hits.Length; inc++) {
							if(hits[inc].collider.transform.GetComponent<terr>())aimatunitchancecomp *= hits[inc].collider.transform.GetComponent<terr>().penchance*0.01f;
						}
						if (aimatunitchance < aimatunitchancecomp) {
							aimatunitchance = aimatunitchancecomp;
							aimatunit = child.Find ("unit/body");
							looktimer = 0;
							frontaim = (transform.Find ("body/gun").position);
						}
						if (aimatunitchance == aimatunitchancecomp) {
							if (weapon == 0 && Vector3.Distance (transform.position, aimatunit.transform.position) > Vector3.Distance (transform.position, child.transform.position)) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 1 && Vector3.Distance (transform.position, aimatunit.transform.position) < Vector3.Distance (transform.position, child.transform.position)) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 2 && aimatunit.parent.GetComponent<unit> ().hp < child.Find ("unit").GetComponent<unit> ().hp) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
							if (weapon == 3 && aimatunit.parent.GetComponent<unit> ().hp > child.Find ("unit").GetComponent<unit> ().hp) {
								aimatunitchance = aimatunitchancecomp;
								aimatunit = child.Find ("unit/body");
								looktimer = 0;
								frontaim = (transform.Find ("body/gun").position);
							}
						}
					}
				}
			}
		}
	}

	void LateUpdate(){
		transform.position = Vector3.Lerp (transform.position, new Vector3 (Mathf.Round (transform.position.x * 40) / 40, transform.position.y, Mathf.Round (transform.position.z * 40) / 40), looktimer);
			
		if (aimatunit != null) {
			transform.Find ("body").LookAt (Vector3.Lerp (frontaim, aimatunit.position, Mathf.Min (looktimer, 1.0f)));
		} else {

			looktimer = 0;

			frontaim = (transform.Find ("body/gun").position);
		}
		//Debug.Log ("" + movearea.bounds.min.y + " " + movearea.bounds.max.y);
		transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, move );
		if (transform.GetComponent<Rigidbody> ().velocity.magnitude < 0.01f && !isgrabbed) {
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		}
		if (Physics.SphereCast (this.transform.Find("body/gun").position, 0.01f, this.transform.Find("body/gun").forward, out hit, Mathf.Infinity, ~0,QueryTriggerInteraction.Ignore)) {
			this.transform.Find ("body/gun").GetComponent<LineRenderer> ().SetPosition (0, this.transform.Find("body/gun").position);
			this.transform.Find ("body/gun").GetComponent<LineRenderer> ().SetPosition (1, hit.point);
		}
	}
}
