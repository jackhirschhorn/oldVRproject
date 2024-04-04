using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveControllerVR : MonoBehaviour {
	
	private Hand hand;
	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    public Vector2 getTrackPadPos()
    {
        SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.touchpos;
        return trackpadPos.GetAxis(hand.handType);
    }

    public bool getPinch()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(hand.handType);
    }

    public bool getPinchDown()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(hand.handType);
    }

    public bool getPinchUp()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(hand.handType);
    }

    public bool getGrip()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand.handType);
    }

    public bool getGrip_Down()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(hand.handType);
    }

    public bool getGrip_Up()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(hand.handType);
    }

    public bool getMenu()
    {
        return SteamVR_Input._default.inActions.menubutton.GetState(hand.handType);
    }

    public bool getMenu_Down()
    {
        return SteamVR_Input._default.inActions.menubutton.GetStateDown(hand.handType);
    }

    public bool getMenu_Up()
    {
        return SteamVR_Input._default.inActions.menubutton.GetStateUp(hand.handType);
    }

    public bool getTouchPad()
    {
        return SteamVR_Input._default.inActions.Teleport.GetState(hand.handType);
    }

    public bool getTouchPad_Down()
    {
        return SteamVR_Input._default.inActions.Teleport.GetStateDown(hand.handType);
    }

    public bool getTouchPad_Up()
    {
        return SteamVR_Input._default.inActions.Teleport.GetStateUp(hand.handType);
    }

    public Vector3 getControllerPosition()
    {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0)
        {
            return poseActions[0].GetLocalPosition(hand.handType);
        }
        return new Vector3(0, 0, 0);
    }

    public Quaternion getControllerRotation()
    {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0)
        {
            return poseActions[0].GetLocalRotation(hand.handType);
        }
        return Quaternion.identity;
    }
	
	// Use this for initialization
	//private SteamVR_TrackedObject trackedObj;
	public Transform cubeplacevolume;
	
	// 1
	public GameObject collidingObject; 
	// 2
	public GameObject objectInHand; 

	public Transform pointedobject;
	
	public float throwstr = 2.0f;
	
	private FixedJoint hing;

	/*private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}*/

	/*void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();

	}*/

	private void SetCollidingObject(Collider col)
	{
		// 1
		if (collidingObject || (!col.GetComponent<Rigidbody>()  && col.gameObject.layer != 15))
		{
			return;
		}
		// 2
		collidingObject = col.gameObject;
	}

	// 1
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
	}

	// 2
	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
	}

	// 3
	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}

		collidingObject = null;
	}

	public Vector3 freezepos = Vector3.zero;
	public bool cangrab = true;
	public int grabcooldown = 0;
	public bool hookto = false;
	public void forceGrabObject(Transform t){
		collidingObject = t.gameObject;
		GrabObject3();
	}
	
	public void GrabObject3(){
		StartCoroutine(GrabObject2());
	}
	
	IEnumerator GrabObject2(){
		yield return new WaitForEndOfFrame();
		GrabObject();
	}
	
	private void GrabObject()
	{
		if (GetComponent<FixedJoint>())
		{
			// 2
			objectInHand.GetComponent<Rigidbody>().useGravity = true;
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// 3
			if (objectInHand != null) {
				holding = false;
				if (objectInHand.transform.GetComponent<item>()){
					objectInHand.transform.GetComponent<item>().onentity = false;
					objectInHand.transform.GetComponent<item>().held = false;
					objectInHand.transform.GetComponent<item>().par = wheregoes.tnone;
				}
				if (transform.parent.parent.parent.localScale.x >= 1.5f) {
					//Debug.Log(hand.GetTrackedObjectVelocity());
					//Debug.Log(hand.GetTrackedObjectAngularVelocity());
					objectInHand.GetComponent<Rigidbody> ().velocity = hand.GetTrackedObjectVelocity() * (transform.parent.parent.parent.localScale.x-0.5f)*throwstr;
					objectInHand.GetComponent<Rigidbody> ().angularVelocity = hand.GetTrackedObjectAngularVelocity() * (transform.parent.parent.parent.localScale.x-0.5f)*throwstr;
				} else {
					objectInHand.GetComponent<Rigidbody> ().velocity = hand.GetTrackedObjectVelocity() * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10))*throwstr;
					objectInHand.GetComponent<Rigidbody> ().angularVelocity = hand.GetTrackedObjectAngularVelocity() * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10))*throwstr;

				}
				/*if (objectInHand.transform.GetComponent <unit>()) {
					objectInHand.transform.GetComponent <unit>().isgrabbed = false;
					objectInHand.transform.GetComponent <unit> ().isreleasedfunc ();
				}*/
			}
			/*if (pointedobject != null) {
				if (transform.parent.parent.parent.localScale.x >= 1.0f) {
					pointedobject.GetComponent<Rigidbody> ().velocity = Controller.velocity * transform.parent.parent.parent.localScale.x;
					pointedobject.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity * transform.parent.parent.parent.localScale.x;
				} else {
					pointedobject.GetComponent<Rigidbody> ().velocity = Controller.velocity * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10));
					pointedobject.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10));

				}
				pointedobject.transform.GetComponent <unit>().isgrabbed = false;
				pointedobject.transform.GetComponent <unit> ().isreleasedfunc ();
			}*/
		}
		// 4
		objectInHand = null;
		if(hand.otherHand.GetComponent<ViveControllerVR>().objectInHand != null &&  hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>() != null && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().id2 == 1 && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().id == 1)hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().varuse();
		
		if(collidingObject != null){
			if(hand.otherHand.GetComponent<ViveControllerVR>().objectInHand != null && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.gameObject.layer == 15 && cangrab){
				hand.otherHand.GetComponent<ViveControllerVR>().ReleaseObject();
			}
			if(collidingObject.gameObject.layer == 15){
				if( cangrab){play1.movetype = 1;
					objectInHand = collidingObject;
					freezepos = transform.position;
					hand.otherHand.GetComponent<ViveControllerVR>().cangrab = false;
				}
			} else {
				// 1
				objectInHand = collidingObject;
				objectInHand.transform.parent = master.items;
				objectInHand.GetComponent<Rigidbody>().useGravity = false;
				objectInHand.GetComponent<Rigidbody>().isKinematic = false;
				if(objectInHand.GetComponent<item>()){
					objectInHand.GetComponent<item>().onpickup();
					objectInHand.GetComponent<item>().resetscal();
				}
				/*if (objectInHand.transform.GetComponent <unit>()) {
					objectInHand.transform.GetComponent <unit>().isgrabbed = true;

					//objectInHand.transform.parent.position = objectInHand.transform.position;
					//objectInHand.transform.localPosition = new Vector3 (0, 0, 0);
					objectInHand.transform.GetComponent <unit> ().isgrabbedfunc ();
				}*/
				bool stuck = false;
				if(objectInHand != null && objectInHand.transform != null && objectInHand.transform.GetComponent<item>() != null && objectInHand.transform.GetComponent<item>().par != null){
					foreach(FixedJoint h in objectInHand.transform.GetComponent<item>().par.GetComponents<FixedJoint>()){
						if(h.connectedBody == objectInHand.GetComponent<Rigidbody>()){
							stuck = true;
							hing = h;
							//Debug.Log(h);
							//Debug.Log(hing);
							break;
						}
					}
				}
				//Debug.Log(hing);
				if(stuck)Destroy(hing);
				if(!objectInHand.transform.GetComponent<Hand>()){objectInHand.transform.GetComponent<Rigidbody>().isKinematic = false;
					objectInHand.transform.parent = master.items;
				objectInHand.transform.GetComponent<item>().resetscal();
				}
				if(objectInHand.transform.GetComponent<item>()){
					objectInHand.transform.GetComponent<item>().changecolliders(true, false, true);
					objectInHand.transform.GetComponent<item>().par = transform;
					objectInHand.transform.GetComponent<item>().onentity = true;
					objectInHand.transform.GetComponent<item>().held = true;
					objectInHand.transform.GetComponent<item>().hidden = false;
				}
				
				
				collidingObject = null;
				// 2
				var joint = AddFixedJoint();
				joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
				GetComponent<Rigidbody>().isKinematic = true;
			}
		}
	}
	
	public bool holding = false;
	
	private void GrabObjectFromPoint()
	{
		if(collidingObject.GetComponent<item>() == null || collidingObject.GetComponent<item>() != null && collidingObject.GetComponent<item>().holdpoints.Count == 0){
			GrabObject();
			return;
		}
		// 1
		objectInHand = collidingObject;
		/*if (objectInHand.transform.GetComponent <unit>()) {
			objectInHand.transform.GetComponent <unit>().isgrabbed = true;

			//objectInHand.transform.parent.position = objectInHand.transform.position;
			//objectInHand.transform.localPosition = new Vector3 (0, 0, 0);
			objectInHand.transform.GetComponent <unit> ().isgrabbedfunc ();
		}*/
		bool stuck = false;
		if(objectInHand != null && objectInHand.transform != null && objectInHand.transform.GetComponent<item>() != null && objectInHand.transform.GetComponent<item>().par != null){
			foreach(FixedJoint h in objectInHand.transform.GetComponent<item>().par.GetComponents<FixedJoint>()){
				if(h.connectedBody == objectInHand.GetComponent<Rigidbody>()){
					stuck = true;
					hing = h;
					//Debug.Log(h);
					//Debug.Log(hing);
					break;
				}
			}
		}
		//Debug.Log(hing);
		if(stuck)Destroy(hing);
		if(!objectInHand.transform.GetComponent<Hand>()){objectInHand.transform.GetComponent<Rigidbody>().isKinematic = false;
			objectInHand.transform.parent = master.items;
		}
		if(objectInHand.transform.GetComponent<item>()){
			
			objectInHand.transform.GetComponent<item>().resetscal();
			objectInHand.GetComponent<item>().onpickup();
			objectInHand.GetComponent<item>().par = transform;
			objectInHand.GetComponent<item>().changecolliders(true, false, true);
			objectInHand.GetComponent<item>().onentity = true;
			objectInHand.GetComponent<item>().held = true;
			objectInHand.GetComponent<item>().hidden = false;
		}
		
		
		collidingObject = null;
		// 2
		if(objectInHand.GetComponent<item>() && objectInHand.GetComponent<item>().holdpoints.Count != 0){
			float close = 100;
			foreach(Transform t in objectInHand.GetComponent<item>().holdpoints){
				close = (Vector3.Distance(t.position, transform.position) < close ? Vector3.Distance(t.position, transform.position) :close);
			}
			foreach(Transform t in objectInHand.GetComponent<item>().holdpoints){
				if(Mathf.Approximately(Vector3.Distance(t.position, transform.position), close)){
					objectInHand.transform.position = transform.position;
					objectInHand.transform.rotation = transform.rotation;
					objectInHand.transform.rotation *= Quaternion.Euler(-90,0,0);
					objectInHand.transform.rotation *= Quaternion.Euler(0,90,0);
					objectInHand.transform.position = Vector3.MoveTowards(transform.position,t.position, -Vector3.Distance(transform.position,t.position));
					objectInHand.transform.RotateAround(t.position, objectInHand.GetComponent<item>().getholdpointturn(), objectInHand.GetComponent<item>().holdpointturn2);
				}
			}
		}
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
		holding = true;
	}

	public void GrabObjectFromDist(Transform t)
	{
		collidingObject = t.gameObject;
		GrabObject();
		
	}

	// 3
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = Mathf.Infinity;
		fx.breakTorque = Mathf.Infinity;
		return fx;
	}

	public void ReleaseObject()
	{
	// 1
		if (GetComponent<FixedJoint>())
		{
			// 2
			objectInHand.GetComponent<Rigidbody>().useGravity = true;
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// 3
			if (objectInHand != null) {
				holding = false;
				if (objectInHand.transform.GetComponent<item>()){
					objectInHand.transform.GetComponent<item>().onentity = false;
					objectInHand.transform.GetComponent<item>().held = false;
					objectInHand.transform.GetComponent<item>().par = wheregoes.tnone;
				}
				if (transform.parent.parent.parent.localScale.x >= 1.5f) {
					//Debug.Log(hand.GetTrackedObjectVelocity());
					//Debug.Log(hand.GetTrackedObjectAngularVelocity());
					objectInHand.GetComponent<Rigidbody> ().velocity = hand.GetTrackedObjectVelocity() * (transform.parent.parent.parent.localScale.x-0.5f)*throwstr;
					objectInHand.GetComponent<Rigidbody> ().angularVelocity = hand.GetTrackedObjectAngularVelocity() * (transform.parent.parent.parent.localScale.x-0.5f)*throwstr;
				} else {
					objectInHand.GetComponent<Rigidbody> ().velocity = hand.GetTrackedObjectVelocity() * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10))*throwstr;
					objectInHand.GetComponent<Rigidbody> ().angularVelocity = hand.GetTrackedObjectAngularVelocity() * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10))*throwstr;

				}
				/*if (objectInHand.transform.GetComponent <unit>()) {
					objectInHand.transform.GetComponent <unit>().isgrabbed = false;
					objectInHand.transform.GetComponent <unit> ().isreleasedfunc ();
				}*/
			}
			/*if (pointedobject != null) {
				if (transform.parent.parent.parent.localScale.x >= 1.0f) {
					pointedobject.GetComponent<Rigidbody> ().velocity = Controller.velocity * transform.parent.parent.parent.localScale.x;
					pointedobject.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity * transform.parent.parent.parent.localScale.x;
				} else {
					pointedobject.GetComponent<Rigidbody> ().velocity = Controller.velocity * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10));
					pointedobject.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity * (transform.parent.parent.parent.localScale.x * (Mathf.Pow (1 - transform.parent.parent.parent.localScale.x, 2) * 10));

				}
				pointedobject.transform.GetComponent <unit>().isgrabbed = false;
				pointedobject.transform.GetComponent <unit> ().isreleasedfunc ();
			}*/
		}
		// 4
		objectInHand = null;
		if(hand.otherHand.GetComponent<ViveControllerVR>().objectInHand != null &&  hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>() != null && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().id2 == 1 && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().id == 1)hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().varuse();
		//pointedobject = null;
	}

	private RaycastHit hit2;
	
	// Use this for initialization
	// 1
	//private SteamVR_TrackedObject trackedObj;
	private float movex = 0.0f;
	private float movey = 0.0f;
	[SerializeField]
	public Transform ball; 
	public Transform clone;
	public float timer = 1.0f;
	public float countdown = 0.0f;
	public bool right = false;
	public Transform eyes;
	public static float scl = 0.2f;
	// 2
	/*private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}*/

	// Update is called once per frame

	public static bool moving;
	
	public LayerMask mask1;
	private RaycastHit hit;
	public float movespeed = 0.03f;
	public int jumpframes = 0;
	public int jumpmaxframes = 6;
	public int airframes = 0;
	public Vector3 movx = Vector3.zero;
	public Vector3 movy = Vector3.zero;
	public Vector3 movyover = Vector3.zero;
	public Vector3 movz = Vector3.zero;
	public player play1;
	
	public spellmaster spellmasta;
	void LateUpdate() {
		if(objectInHand == null && (GetComponent<spell>() == null || GetComponent<spell>().id != 2))hookto = false;
		grabcooldown++;
		if(grabcooldown > 10)cangrab = true;
		jumpframes++;
		if(getGrip_Down() && right){
			spellmasta.equipingspell = 1;
		} 
		if(getGrip_Up() && right){
			spellmasta.equipingspell = 4;
		}
		if(getTouchPad_Down() && right){
			if(getTrackPadPos().x < 0 && hand.otherHand.GetComponent<spell>()){
				hand.otherHand.GetComponent<spell>().destroyspell();
			} else if(getTrackPadPos().x > 0 && GetComponent<spell>()) {
				GetComponent<spell>().destroyspell();
			}
		}
		if (getPinch())
		{
			if(GetComponent<spell>()){
				
			} else {
				//Debug.Log(gameObject.name + " Trigger Press");
				//GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (0, 0, 100));
				if (objectInHand == null ){ // use this for spell 0
					/*if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, 100.0f, mask1)) {
						GetComponent<LineRenderer> ().SetPosition (0, transform.position);
						GetComponent<LineRenderer> ().SetPosition (1, hit.point);
						if(hit.transform != null && hit.transform.parent.GetComponent<chunk>() != null){
							Vector3 tempvec = hit.transform.parent.GetComponent<chunk>().translatehit(hit.point+(hit.normal*-0.1f));
							//Debug.Log(hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog);
							hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog++;
							if(hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog >= 100){
								hit.transform.parent.GetComponent<chunk>().changeblock((int)tempvec.x,(int)tempvec.y,(int)tempvec.z, -1);
								hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog = 0;
							}
							
						}
					} else {
						GetComponent<LineRenderer> ().SetPosition (0, transform.position);
						GetComponent<LineRenderer> ().SetPosition (1, transform.position+(transform.forward*100));
					
					}*/
				} else if(objectInHand.gameObject.layer == 15){
					transform.position = freezepos;
					//GetComponent<LineRenderer> ().SetPosition (0, transform.position);
					//GetComponent<LineRenderer> ().SetPosition (1, transform.position);
					
				}
			}
		}

		/*if (Controller.GetHairTrigger ()) {
			if (Physics.SphereCast (this.transform.position, 0.01f, this.transform.forward, out hit, 1000.0f)) {
				//Debug.Log (hit.collider.gameObject.name);
				//if (hit.collider.gameObject.layer == 10) {
				//hit.collider.transform.GetComponent<enemy> ().takedamage (laser * 0.3f);
				GetComponent<LineRenderer> ().SetPosition (0, transform.position);
				GetComponent<LineRenderer> ().SetPosition (1, hit.point);
				//Debug.Log (hit.collider.transform.name);
				if (countdown <= 0 && right) {
					clone = Instantiate (ball, this.transform.position, new Quaternion (0, 0, 0, 0));
					clone.GetComponent<Rigidbody> ().AddForce (transform.forward * 1000);
					countdown = timer;
				}
				if (!right) {
					if(hit.collider.transform.GetComponent<Rigidbody> ())hit.collider.transform.GetComponent<Rigidbody> ().AddForce (transform.forward * 1000);
				}
				//}
			} else {
				//this.transform.Find ("laser").GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (0.5f, 1000, 0));

			}
			//GetComponent<LineRenderer> ().endWidth += 0.1f;
		}*/

		// 3
		if (getPinchUp())
		{
			//Debug.Log(gameObject.name + " Trigger Release");
			//GetComponent<LineRenderer> ().SetPosition (0, new Vector3 (0, 0, 0));
			//GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (0, 0, 0));
			//GetComponent<LineRenderer> ().endWidth = 0.01f;
		}
		if(play1.movetype != 3){
			movyover = Vector3.zero;
		}
		if(play1.movetype == 0){
			if(!right)moving = false;
			movy = Vector3.zero;
			if(transform.parent.parent.parent.GetComponent<CharacterController> ().isGrounded){
				
				
					movx = Vector3.zero;
					movz = Vector3.zero;
				if (getTrackPadPos() != Vector2.zero)
				{
					//Debug.Log(gameObject.name + Controller.GetAxis());
					if (!right) {
						moving = true;
						//movex = eyes.forward * Controller.GetAxis ().y * 0.01f;
						//movey = eyes.right * Controller.GetAxis ().x * 0.01f;
						//if (Controller.GetPress (SteamVR_Controller.ButtonMask.Grip)) {
							//transform.parent.parent.parent.localScale = new Vector3 (transform.parent.parent.parent.localScale.x + Controller.GetAxis ().y * 0.005f, transform.parent.parent.parent.localScale.y + Controller.GetAxis ().y * 0.005f, transform.parent.parent.parent.localScale.z + Controller.GetAxis ().y * 0.005f);
							//transform.parent.parent.parent.localScale = new Vector3 (Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.x, 0.05f), 1.5f), Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.y, 0.05f), 1.5f), Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.z, 0.05f), 1.5f));
							//transform.parent.position = new Vector3 (transform.parent.position.x, 0, transform.parent.position.z);
						//} else {
							//Ray ray = new Ray(transform.parent.parent.parent.position, Vector3.down);
							//Physics.Raycast(ray, out hit, 100, mask1, QueryTriggerInteraction.Ignore);
				
							//transform.parent.parent.parent.GetComponent<CharacterController> ().Move ((eyes.forward * getTrackPadPos().y * movespeed) + (eyes.right * getTrackPadPos().x * movespeed));
							movx = (eyes.right * getTrackPadPos().x * (getTouchPad()?movespeed*3:movespeed));
							movz = (eyes.forward * getTrackPadPos().y * (getTouchPad()?movespeed*3:movespeed));
							
							//transform.parent.position = new Vector3 (transform.parent.position.x, hit.point.y, transform.parent.position.z);
						//}
					}
				}
				airframes = 0;
				if(!right && getGrip_Down()){ //jump
					jumpframes = 0;
					airframes = 1;
				}
			} else {
				airframes++;
			}
			if (airframes == 0 && jumpframes < jumpmaxframes)jumpframes = 500;
			if(jumpframes == jumpmaxframes)airframes = -airframes;
			if(jumpframes > jumpmaxframes){
				movy -= new Vector3(0,master.playgrav*Time.deltaTime+Mathf.Max(Mathf.Min((airframes * 0.01f),6.81f),0),0);
			} else {
				movy += new Vector3(0,0.5f - (airframes * 0.01f),0);
			}
			movx.y = 0.0f;
			movz.y = 0.0f;
		} else if(play1.movetype == 1){ //climb
			airframes = 0;
			moving = true;
			movx = Vector3.zero;
			movy = Vector3.zero;
			movz = Vector3.zero;
			if( objectInHand != null && objectInHand.gameObject.layer == 15)movx = new Vector3(hand.GetTrackedObjectVelocity().x *-1,(hand.GetTrackedObjectVelocity().y * -1),hand.GetTrackedObjectVelocity().z * -1)* Time.deltaTime;
		} else if(play1.movetype == 2){ //fly
			airframes = 0;
			movx = Vector3.zero;
			movy = Vector3.zero;
			movz = Vector3.zero;
			if (getTrackPadPos() != Vector2.zero)
				{
					//Debug.Log(gameObject.name + Controller.GetAxis());
					if (!right) {
						moving = true;
						//movex = eyes.forward * Controller.GetAxis ().y * 0.01f;
						//movey = eyes.right * Controller.GetAxis ().x * 0.01f;
						//if (Controller.GetPress (SteamVR_Controller.ButtonMask.Grip)) {
							//transform.parent.parent.parent.localScale = new Vector3 (transform.parent.parent.parent.localScale.x + Controller.GetAxis ().y * 0.005f, transform.parent.parent.parent.localScale.y + Controller.GetAxis ().y * 0.005f, transform.parent.parent.parent.localScale.z + Controller.GetAxis ().y * 0.005f);
							//transform.parent.parent.parent.localScale = new Vector3 (Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.x, 0.05f), 1.5f), Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.y, 0.05f), 1.5f), Mathf.Min (Mathf.Max (transform.parent.parent.parent.localScale.z, 0.05f), 1.5f));
							//transform.parent.position = new Vector3 (transform.parent.position.x, 0, transform.parent.position.z);
						//} else {
							//Ray ray = new Ray(transform.parent.parent.parent.position, Vector3.down);
							//Physics.Raycast(ray, out hit, 100, mask1, QueryTriggerInteraction.Ignore);
				
							//transform.parent.parent.parent.GetComponent<CharacterController> ().Move ((eyes.forward * getTrackPadPos().y * movespeed) + (eyes.right * getTrackPadPos().x * movespeed));
							movx = (eyes.right * getTrackPadPos().x * (getTouchPad()?movespeed*3:movespeed));
							movz = (eyes.forward * getTrackPadPos().y * (getTouchPad()?movespeed*3:movespeed));
							
							//transform.parent.position = new Vector3 (transform.parent.position.x, hit.point.y, transform.parent.position.z);
						//}
					}
				}
		} else if(play1.movetype == 3){
			airframes = 0;
			movx = Vector3.zero;
			movy = movyover;
			movz = Vector3.zero;
		}
		play1.movx += movx;
		play1.movy += movy;
		play1.movz += movz;
	}
	
	void Update () {
		//if(objectInHand == null)play1.movetype = 0;
		cubeplacevolume.GetComponent<Renderer>().enabled = false;
					
		if(getMenu()){
			if(GetComponent<spell>()){
				GetComponent<spell>().getmenu();
			} else {
				if(objectInHand != null && objectInHand.GetComponent<item>()){
					if(objectInHand.GetComponent<item>().id == 0){
						if(Physics.Raycast (this.transform.position, this.transform.forward, out hit, 1.0f, mask1) && hit.transform != null &&  hit.transform.parent != null &&  hit.transform.parent.GetComponent<chunk>() != null){
						
							Vector3 tempvec = hit.transform.parent.GetComponent<chunk>().translatehit(hit.point+(hit.normal*0.1f));
							cubeplacevolume.GetComponent<Renderer>().enabled = true;
							cubeplacevolume.position = hit.transform.parent.localPosition+tempvec;
						}
					}
				}
			}
		}
		if(getMenu_Up()){
			if(GetComponent<spell>()){
				
				GetComponent<spell>().getmenuup();
			} else {
				if(objectInHand != null && objectInHand.GetComponent<item>() != null && objectInHand.GetComponent<item>().id == 0){
					if(Physics.Raycast (this.transform.position, this.transform.forward, out hit, 1.0f, mask1) && hit.transform != null &&  hit.transform.parent != null &&  hit.transform.parent.GetComponent<chunk>() != null){
						Vector3 tempvec = hit.transform.parent.GetComponent<chunk>().translatehit(hit.point+(hit.normal*+0.1f));
						hit.transform.parent.GetComponent<chunk>().changeblock((int)tempvec.x,(int)tempvec.y,(int)tempvec.z,objectInHand.GetComponent<item>().id2);
						Destroy(objectInHand.gameObject, 0.01f);
						ReleaseObject();
						
						cubeplacevolume.GetComponent<Renderer>().enabled = false;
						//cubeplacevolume.position = hit.transform.parent.localPosition+tempvec;
					}
				}
				if(objectInHand != null && objectInHand.GetComponent<item>()){
					if(objectInHand.GetComponent<item>().id == 1){
						if(objectInHand.GetComponent<item>().id2 == 3 )objectInHand.GetComponent<item>().varuse2();
					}
				}
			}
		}
		if(getMenu_Down()){
			if(GetComponent<spell>()){
				
				GetComponent<spell>().getmenudown();
			} else {
				if(objectInHand != null && objectInHand.GetComponent<item>()){
					if(objectInHand.GetComponent<item>().id == 1){
						if(objectInHand.GetComponent<item>().id2 != 1 )objectInHand.GetComponent<item>().varuse();
					}
				}
			}
		}
		// 1
		if (getPinchDown()){
			if(GetComponent<spell>()){
				
				GetComponent<spell>().getpinchdown();
			} else {
				if (collidingObject && !getMenu()){
					GrabObject();
				} else if (collidingObject){
					GrabObjectFromPoint();
				}
			}
		}
		if (getPinch()){
			if(GetComponent<spell>()){
				
				GetComponent<spell>().getpinch();
			} else {
				if (objectInHand == null && collidingObject != null && collidingObject.gameObject.layer == 15){
					GrabObject();
				}
			}
		}
		if(getGrip_Down()){
			
			/*if(!holding){
				if (collidingObject){
					GrabObjectFromPoint();
				}
			} else {
				if (objectInHand || pointedobject){
					ReleaseObject();
				}
			}*/
		}
		


		// 2
		if (getPinchUp())
		{
			if(GetComponent<spell>()){
				
				GetComponent<spell>().getpinchup();
			} else {
				if (objectInHand || pointedobject)
				{
					//Debug.Log(objectInHand.gameObject.layer);
					if(objectInHand.gameObject.layer == 15 ){
						if(hand.otherHand.GetComponent<ViveControllerVR>().objectInHand == null || (hand.otherHand.GetComponent<ViveControllerVR>().objectInHand != null && hand.otherHand.GetComponent<ViveControllerVR>().objectInHand.gameObject.layer != 15))play1.movetype = 0;
						objectInHand = null;
					} else {
						ReleaseObject();
					}
				}
			}
		}
		
		countdown -= Time.deltaTime;

		
	}
}
