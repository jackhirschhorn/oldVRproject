using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {
	
	static Transform chunks;
	public Transform chunks2;
	public static Transform items;
	public Transform items0;
	public static Transform liquids;
	public Transform liquids2;
	public static Transform miniliquids;
	public Transform miniliquids2;
	static Transform blocks;
	public Transform blocks2;
	static Transform miniblocks;
	public Transform miniblocks2;
	static Transform liquidprefab;
	public Transform liquidprefab2;
	public static Transform minichunkliquidprefab;
	public Transform minichunkliquidprefab2;
	public static Transform testbox;
	public Transform testbox2;
	
	public static Texture2D[] matslist1 = new Texture2D[256];
	public Texture2D[] matslist1p = new Texture2D[256];
	public static Texture2D[] matslist2 = new Texture2D[256];
	public Texture2D[] matslist2p = new Texture2D[256];
	
	public static Material mat1;
	public Material mat01;
	public static Material mat2;
	public Material mat02;
	
	public static float playgrav;
	public float playgrav0 = 3.0f;
	public static Transform play1;
	public Transform play01;
	public static Transform lefthand;
	public static Transform righthand;
	public Transform lefthand0;
	public Transform righthand0;
	
	
	public int tickrate = 10;
	public int ticktimer = 0;
	public int tickcount = 0;
	public int maxtickcount = 10;
	
	public static Transform blockitem;
	public Transform blockitem0;
	
	public static List<Transform> spellidobj = new List<Transform>();
	public List<Transform> spellidobj2 = new List<Transform>();
	
	public static LayerMask mask1;
	public LayerMask mask01;
	
	public static LayerMask mask2;
	public LayerMask mask02;
	
	public static LayerMask mask3;
	public LayerMask mask03;

	// Use this for initialization
	void Awake () {
		chunks = chunks2;
		items = items0;
		liquidprefab = liquidprefab2;
		liquids = liquids2;
		blocks = blocks2;
		miniliquids = miniliquids2;
		miniblocks = miniblocks2;
		minichunkliquidprefab = minichunkliquidprefab2;
		testbox = testbox2;
		matslist1 = matslist1p;
		matslist2 = matslist2p;
		mat1 = mat01;
		mat2 = mat02;
		blockitem = blockitem0;
		playgrav = playgrav0;
		spellidobj = spellidobj2;
		mask1 = mask01;
		mask2 = mask02;
		mask3 = mask03;
		play1 = play01;
		lefthand = lefthand0;
		righthand = righthand0;
	}
	
	void Start (){
		StartCoroutine("loadchunks");
	}
	
	private bool startticking = false;
	
	IEnumerator loadchunks (){
		bool alldone = false;
		int its = 1;
		while(alldone == false && its < 100){
			alldone = true;
			foreach(Transform c in chunks){
				if(!c.GetComponent<chunk>().loaded && Vector3.Distance(c.GetComponent<chunk>().xyz,Vector3.zero) < its+0.05f){
					c.GetComponent<chunk>().framessinceupdate = c.GetComponent<chunk>().greedyframes + 1;
					c.GetComponent<chunk>().converttogreedymesh(false);
					while(!c.GetComponent<chunk>().loaded){
						yield return new WaitForEndOfFrame();
					}
					alldone = false;
				}
			}
			its++;
		}
		startticking = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(startticking)ticktimer++;
		if(ticktimer == tickrate){
			tickcount++;
			/*foreach(Transform child in blocks){
				if(child.GetComponent<block>().tickon)child.GetComponent<block>().dotick(tickcount);
			}
			
			/*foreach(Transform child in miniblocks){
				if(child.GetComponent<block>().tickon)child.GetComponent<block>().dotick();
			}
			
			foreach(Transform child in liquids){
				if(!child.GetComponent<liquid>().settled)child.GetComponent<liquid>().dotick(tickcount);
			}
			
			foreach(Transform child in miniliquids){
				if(child.GetComponent<minichunkl>().tickon)child.GetComponent<minichunkl>().dotick(tickcount);
			}*/
			
			foreach(Transform child in chunks){
				if(child.GetComponent<chunk>().tickon){
					child.GetComponent<chunk>().dotick(tickcount);
				} else if(!child.GetComponent<chunk>().isgreedy) {
					child.GetComponent<chunk>().converttogreedymesh(false);
				}
			}
			ticktimer = 0;
			if(tickcount == maxtickcount)tickcount = 0;
		}
	}
	
	/*public static void addliquid(Vector3 v, int i, int i2, int i3, int i4, bool b, Transform t){
		Transform clone;
		clone = Instantiate(liquidprefab);
		clone.parent = t;
		clone.transform.position = v;
		clone.GetComponent<liquid>().id = i;
		clone.GetComponent<liquid>().level = i2;
		clone.GetComponent<liquid>().inf = i3;
		clone.GetComponent<liquid>().sources[i4] = 1;
		clone.GetComponent<liquid>().mini = b;
		if(!b)clone.GetComponent<liquid>().ticknum = Random.Range(1,11);
		if(b)clone.transform.localScale = new Vector3(1.0f/16.0f,1.0f/16.0f,1.0f/16.0f);
		//clone.GetComponent<liquid>().checksurround(false);
		if(!b){
			foreach(Collider c in Physics.OverlapBox(clone.position, clone.localScale/2.01f, Quaternion.identity, (1 << 11))){
				Destroy(c.gameObject);
			}
		}
	}*/
}
