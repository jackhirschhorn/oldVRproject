using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour {

	[SerializeField]
	public static weap[] weaponlist = new weap[]{
		new weap("shotgun", 1, 0, 0, 0, 7, 4, 30.0f, 0.0f, false, 0, 0, 1.0f, 10, Resources.Load("guns/Shotgun") as GameObject), //0
		new weap("sniper rifle", 5, 0, 0, 0, 1, 3, 3.0f, 0.0f, false, 1, 20, 1.0f, 25, Resources.Load("guns/Barretm107") as GameObject), //1
		new weap("LMG", 1, 0, 0, 0, 10, 6, 15.0f, 0.1f, false, 2, 10, 1.0f, 15, Resources.Load("guns/Gatling") as GameObject), //2
		new weap("assault rifle", 2, 0, 0, 0, 3, 5, 9.0f, 0.2f, false, 3, 5, 1.0f, 10, Resources.Load("guns/ColtM4") as GameObject), //3
		new weap("sawed off", 1, 0, 0, 0, 7, 2, 45.0f, 0.0f, false, 0, 0, 1.0f, 20, Resources.Load("guns/Shotgun") as GameObject), //4
		new weap("revolver", 2, 0, 0, 0, 2, 3, 6.0f, 0.7f, false, 1, 10, 1.0f, 15, Resources.Load("guns/Handgun") as GameObject), //5
		new weap("machine pistol", 1, 0, 0, 0, 10, 2, 21.0f, 0.1f, false, 2, -10, 1.0f, 5, Resources.Load("guns/Uzi") as GameObject), //6
		new weap("pistol", 1, 0, 0, 0, 3, 4, 9.0f, 0.4f, false, 3, 0, 1.0f, 5, Resources.Load("guns/Handgun") as GameObject), //7
	};

	public class weap{
		string sname;
		int idamage;
		int idamagerange; //unused currently, but implemented
		int icrit; // unused currently
		int icritdamage; //unused currently
		int ibullets;
		int iclipsize; //unused currently
		float fspread;
		float ffiredelay;
		bool bsecondary;
		int iweaptype;
		int ibulletpenflat;
		float fbulletpenmult;
		float fenvdam;
		GameObject gweapmodel;
		public weap(string n, int d, int dr, int c, int cd, int b, int cs, float s, float fd, bool se, int wt, int bpf, float bpm, float ed, GameObject wm){
			this.sname = n;
			this.idamage = d;
			this.idamagerange = dr; //unused currently
			this.icrit = c; // unused currently
			this.icritdamage = cd; //unused currently
			this.ibullets = b;
			this.iclipsize = cs; //unused currently
			this.fspread = s;
			this.ffiredelay = fd;
			this.bsecondary = se;
			this.iweaptype = wt;
			this.ibulletpenflat = bpf;
			this.fbulletpenmult = bpm;
			this.fenvdam = ed;
			this.gweapmodel = wm;
		}
		public string name { get { return sname;}}
		public int damage { get { return idamage;}}
		public int damagerange { get { return idamagerange;}}
		public int crit { get { return icrit;}}
		public int critdamage { get { return icritdamage;}}
		public int bullets { get { return ibullets;}}
		public int clipsize { get { return iclipsize;}}
		public float spread { get { return fspread;}}
		public float firedelay { get { return ffiredelay;}}
		public bool secondary { get { return bsecondary;}}
		public int weapontype { get { return iweaptype;}}
		public int bulletpenflat { get { return ibulletpenflat;}}
		public float bulletpenmult { get { return fbulletpenmult;}}
		public float envdam { get { return fenvdam;}}
		public GameObject weapmodel { get { return gweapmodel;}}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
