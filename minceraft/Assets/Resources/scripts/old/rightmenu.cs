using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightmenu : MonoBehaviour {

	/*private SteamVR_TrackedObject trackedObj;
	[SerializeField]
	public Transform text;
	public Transform leftc;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
	// Use this for initialization
	void Start () {
		trackedObj = transform.GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		text.GetComponent<TextMesh> ().text = "";//Mathf.Abs(Controller.GetAxis ().x) + " " + Mathf.Abs(Controller.GetAxis ().y) + " " + Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f));
		if (Mathf.Abs(Controller.GetAxis ().x) + Mathf.Abs(Controller.GetAxis ().y)> 0.5f){//Controller.GetAxis ().y > 0.88f && Controller.GetAxis ().x > -0.2f && Controller.GetAxis ().x < 0.2f) {
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > -22.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < 22.5){
				text.GetComponent<TextMesh> ().text = "top button";
				if (transform.GetComponent<controllergrabobject> ().pointedobject != null) {
					if (transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> () && transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().playerteam) {
						if(transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().onsecondary){
							text.GetComponent<TextMesh> ().text = "swap weapon to " + weapons.weaponlist[transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().primary].name;
						} else {
							text.GetComponent<TextMesh> ().text = "swap weapon to " + weapons.weaponlist[transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().secondary].name;
						}
						if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
							transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().swapweap ();
						}
					}
				}
				if (leftc.transform.GetComponent<controllergrabobject> ().pointedobject != null) {
					if (leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> () && leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().playerteam) {
						if(leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().onsecondary){
							text.GetComponent<TextMesh> ().text = "swap weapon to " + weapons.weaponlist[leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().primary].name;
						} else {
							text.GetComponent<TextMesh> ().text = "swap weapon to " + weapons.weaponlist[leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit>().secondary].name;
						}
						if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
							leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().swapweap ();
						}
					}
				}


			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > 22.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < 67.5){
				text.GetComponent<TextMesh> ().text = "top right button";
				if (transform.GetComponent<grenade> ().showpreview) {
					text.GetComponent<TextMesh> ().text = "launch grenade";
					if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
						transform.GetComponent<grenade> ().launchgrenade = true;
					}
				}
				if (transform.GetComponent<controllergrabobject> ().pointedobject != null) {
					if (transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> () && transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().playerteam) {
						text.GetComponent<TextMesh> ().text = "grenade test";
						if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
							if (transform.GetComponent<grenade> ().showpreview) {
								transform.GetComponent<grenade> ().launchgrenade = true;
							} else {
								transform.GetComponent<grenade> ().showpreview = true;
								transform.GetComponent<grenade> ().unitsel = transform.GetComponent<controllergrabobject> ().pointedobject;

							}
						}
					}
				}
				if (leftc.transform.GetComponent<controllergrabobject> ().pointedobject != null) {
					if (leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> () && leftc.transform.GetComponent<controllergrabobject> ().pointedobject.transform.GetComponent<unit> ().playerteam) {
						text.GetComponent<TextMesh> ().text = "grenade test";
						if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
							if (transform.GetComponent<grenade> ().showpreview) {
								transform.GetComponent<grenade> ().launchgrenade = true;
							} else {
								transform.GetComponent<grenade> ().showpreview = true;
								transform.GetComponent<grenade> ().unitsel = leftc.transform.GetComponent<controllergrabobject> ().pointedobject;

							}
						}
					}
				}
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > 67.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < 112.5){
				text.GetComponent<TextMesh> ().text = "right button";
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > 112.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < 157.5){
				text.GetComponent<TextMesh> ().text = "bottom right button";
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > 157.5 || Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < -157.5){
				text.GetComponent<TextMesh> ().text = "bottom button";
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > -157.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < -112.5){
				text.GetComponent<TextMesh> ().text = "bottom left button";
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > -112.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < -67.5){
				text.GetComponent<TextMesh> ().text = "left button";
			}
			if(Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) > -67.5 && Vector2.SignedAngle(new Vector2(Controller.GetAxis ().x,Controller.GetAxis ().y), new Vector2(0.0f, 1.0f)) < -22.5){
				text.GetComponent<TextMesh> ().text = "top left button";
			}
		}
	}

*/
}
