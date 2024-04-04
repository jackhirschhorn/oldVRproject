using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class terraingen : MonoBehaviour {
}
	/*public Vector3 mapdims = new Vector3(10,10,10);
	public int numrooms = 1;
	public List<int[]> mapinfo = new List<int[]>();//x,y,z,type, rot
	public List<Vector3> roomori = new List<Vector3>();
	public List<Vector3> roomorispace = new List<Vector3>();
	public List<bool> roomoriused = new List<bool>();
	public Transform cube;
	

	public Transform clone;
	
	public Transform player;

	public int total(List<int> i){
		int i2 = 0;
		foreach(int i3 in i){
			i2 += i3;
		}
		return i2;
	}
	
	// Use this for initialization
	void Start () {
		/*List<int> roomy = new List<int>();
		for(int i6 = 0; mapdims.y >= total(roomy) && i6 < mapdims.y;i6++){
			roomy.Add((int)Random.Range((i6%2 == 1? 3 : 1),(i6%2 == 1?mapdims.y-total(roomy)+1 : (mapdims.y-total(roomy)+1)/2.0f)));
		}
		bool flip3 = false;
		List<int> roomz = new List<int>();
		for(int i5 = 0; mapdims.z >= total(roomz) && i5 < mapdims.z;i5++){
			roomz.Add((int)Random.Range((i5%2 == 1? 3 : 1),(i5%2 == 1?mapdims.z-total(roomz)+1 : (mapdims.z-total(roomz)+1)/2.0f)));
		}
		bool flip2 = false;
		List<int> roomx = new List<int>();
		for(int i4 = 0; mapdims.x >= total(roomx) && i4 < mapdims.x;i4++){
			roomx.Add((int)Random.Range((i4%2 == 1? 3 : 1),(i4%2 == 1?mapdims.x-total(roomx)+1 : (mapdims.x-total(roomx)+1)/2.0f)));
		}
		bool flip = false;
		Debug.Log(roomx.Count + " " + roomz.Count + " " + roomy.Count);
		int yinc = 0;
		int yinc2 = 0;
		for(int i = 0; i < mapdims.y; i++){
			int yno2 = 0;
			//if(yn == 0){
				if(flip3){
					if(roomy[yinc] > yinc2){
						yno2 = 1;
						yinc2 += 1;
					} else {
						yinc += 1;
						yinc2 = 0;
						//roomy.RemoveAt(0);
						flip3 = false;
					}
				} else {
					if(roomy[yinc] > yinc2){
						yinc2 += 1;
					} else {
						yinc += 1;
						yinc2 = 0;
						//roomy.RemoveAt(0);
						yno2 = 1;
						flip3 = true;
					}
				}
			//}
				//first pass make rooms
				
			int zinc = 0;
			int zinc2 = 0;
			flip2 = false;
			for(int i2 = 0; i2 < mapdims.z; i2++){
				int yno = 0;
				//if(yn == 0){
					if(flip2){
						if(roomz[zinc] > zinc2){
							yno = 1;
							zinc2 += 1;
							//Debug.Log(roomz[0] + " " + flip2);
						} else {
							zinc += 1;
							zinc2 = 0;
							//roomz.RemoveAt(0);
							flip2 = false;
						}
					} else {
						if(roomz[zinc] > zinc2){
							zinc2 += 1;
							//Debug.Log(roomz[0] + " " + flip2);
						} else {
							zinc += 1;
							zinc2 = 0;
							//roomz.RemoveAt(0);
							yno = 1;
							flip2 = true;
						}
					}
				//}
				int xinc = 0;
				int xinc2 = 0;
				flip = false;
				//Debug.Log(xinc);
				for(int i3 = 0; i3 < mapdims.x; i3++){
					//Debug.Log(i3 + " " + i + " " +i2 + " " + ((int)mapdims.x-1) + " " + (i3 != (int)mapdims.x-1));
					int yn = (i3 != 0 && i3 != 1 && i2 != 0 && i2 != (int)mapdims.z-1 && i != 0 && i != (int)mapdims.y-1 && yno == 0 && yno2 == 0? 0: 1);
					if(yn == 0){
						if(flip){
							if(roomx[xinc] > xinc2){
								yn = 1;
								xinc2 += 1;
								//Debug.Log(roomx[xinc] + " " + xinc + " " + flip + " " + i3 + " " + i2);
							} else {
								//roomx.RemoveAt(0);
								xinc += 1;
								xinc2 = 0;
								flip = false;
							}
						} else {
							if(roomx[xinc] > xinc2){
								xinc2 += 1;
								//Debug.Log(roomx[xinc] + " " + xinc + " " + flip + " " + i3 + " " + i2);
							} else {
								//roomx.RemoveAt(0);
								xinc += 1;
								xinc2 = 0;
								yn = 1;
								flip = true;
							}
						}
					}
					mapinfo.Add(new int[]{i3-(int)(mapdims.x/2.0f),i-(int)(mapdims.y/2.0f),i2-(int)(mapdims.z/2.0f),yn,0});
				}
			}	
		}*/
		/*
		fill();
		
		createroom(player);
		int numrooms2 = numrooms;
		while(numrooms2 > 0){
			createroom(wheregoes.tnone);
			numrooms2--;
		}
		reconsile();
		int numrooms3 = (int)Random.Range(numrooms, numrooms*2+1);
		while(numrooms3 > 0){
			createhall();
			numrooms3--;
		}
		reconsile();
		//final pass
		foreach(int[] i4 in mapinfo){
			
			if(i4[3] == 1){clone = Instantiate(cube);
			}else if(i4[3] == 0){
				
			} else {
				//Debug.Log(i4[3]);
			}
			if(i4[3] != 0){
				clone.parent = transform;
				clone.position = new Vector3(i4[0],i4[1],i4[2]);
				clone.localRotation = Quaternion.Euler(0,0,0);
			}
		}
		StaticOcclusionCulling.Compute();
	}
	
	void fill(){
		for(int i = 0;i < mapdims.x;i++){
			for(int i2 = 0;i2 < mapdims.y;i2++){
				for(int i3 = 0;i3 < mapdims.z;i3++){
					mapinfo.Add(new int[]{(int)(0-(mapdims.x/2.0f)+i),(int)(0-(mapdims.y/2.0f)+i2),(int)(0-(mapdims.z/2.0f)+i3), 1, 0});
				}
			}
		}
	}
	
	void createroom (Transform t){
		int roomx = 5;//(int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.x+1)));
		int roomy = 5;//(int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.y+1)));
		int roomz = 5;//(int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.z+1)));
		Vector3 ori = new Vector3(0,0,0);
		if(t != wheregoes.tnone){
			ori = new Vector3((int)Mathf.Round(t.position.x),0,(int)Mathf.Round(t.position.z)); //(int)Mathf.Round(t.position.y)
		} else {
			bool done = false;
			int inc2 = 0;
			while(inc2 < 1000 && !done){
				roomx = (int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.x+1)));
				roomy = 5;//(int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.y+1)));
				roomz = (int)Random.Range(5, Mathf.Min(20,Mathf.Max(6,mapdims.z+1)));
		
				ori = new Vector3((int)Random.Range(-mapdims.x/2+roomx,mapdims.x/2-roomx+1),0,(int)Random.Range(-mapdims.z/2+roomz,mapdims.z/2-roomz+1)); //(int)Random.Range(-mapdims.y,mapdims.y+1)
					for(int inc = 0;inc < roomori.Count; inc++){
						if(!tooclose(roomori[inc],ori,roomorispace[inc] ,new Vector3(roomx,roomy,roomz)))done = true;
					}
				inc2++;
			}
			if(inc2 == 1000)return;
		}
		for(int i = 0; i <= roomx; i++){
			for(int i2 = 0; i2 <= roomy; i2++){
				for(int i3 = 0; i3 <= roomz; i3++){
					//if(mapcontains(new Vector3((int)(ori.x-(roomx/2.0f)+i),(int)(ori.y-(roomy/2.0f)+i2),(int)(ori.z-(roomz/2.0f)+i3))))removefrom(new Vector3((int)(ori.x-(roomx/2.0f)+i),(int)(ori.y-(roomy/2.0f)+i2),(int)(ori.z-(roomz/2.0f)+i3)));
					//mapinfo.Add(new int[]{(int)(ori.x-(roomx/2.0f)+i),(int)(ori.y-(roomy/2.0f)+i2),(int)(ori.z-(roomz/2.0f)+i3),(i != 0 && i != roomx && i2 != 0 && i2 != roomy && i3 != 0 && i3 != roomz ?0:(mapcontains(new Vector3((int)(ori.x-(roomx/2.0f)+i),(int)(ori.y-(roomy/2.0f)+i2),(int)(ori.z-(roomz/2.0f)+i3)))?0:1)),0});
					changeat(new Vector3((int)(ori.x-(roomx/2.0f)+i),(int)(ori.y-(roomy/2.0f)+i2),(int)(ori.z-(roomz/2.0f)+i3)), (i != 0 && i != roomx && i2 != 0 && i2 != roomy && i3 != 0 && i3 != roomz? 0:1));
				}
			}
		}
		roomori.Add(ori);
		roomorispace.Add(new Vector3(roomx,roomy,roomz));
		roomoriused.Add(false);
		
	}
	
	bool tooclose(Vector3 v, Vector3 v2, Vector3 v3, Vector3 v4){
		for(int i = (int)(v.x-(v3.x/2.0f));i < (int)(v.x+(v3.x/2.0f)); i++){
			for(int i2 = (int)(v2.x-(v4.x/2.0f));i2 < (int)(v2.x+(v4.x/2.0f)); i2++){
				if(i == i2){
					for(int i3 = (int)(v.z-(v3.z/2.0f));i3 < (int)(v.z+(v3.z/2.0f)); i3++){
						for(int i4 = (int)(v2.z-(v4.z/2.0f));i4 < (int)(v2.z+(v4.z/2.0f)); i4++){
							if(i3 == i4){
								return true; //add y later
							}
						}
					}
				}
			}
		}
		return false;
	}
	
	bool mapcontains(Vector3 v){
		foreach(int[] i in mapinfo){
			if(new Vector3((int)i[0],(int)i[1],(int)i[2]) == new Vector3((int)v.x,(int)v.y,(int)v.z)) return true;
		}
		return false;
	}
	
	bool mapcontains2(Vector3 v, int i2){
		foreach(int[] i in mapinfo){
			if(new Vector3((int)i[0],(int)i[1],(int)i[2]) == new Vector3((int)v.x,(int)v.y,(int)v.z) && i[3] == i2) return true;
		}
		return false;
	}
	
	void changeat(Vector3 v, int i2){
		//yield return new WaitForSeconds(0.1f);
		//Debug.Log(new Vector3((int)mapinfo[i][0],(int)mapinfo[i][1],(int)mapinfo[i][2]));
		//Debug.Log(v);
		for(int i = 0; i < mapinfo.Count; i++){
			if(new Vector3((int)mapinfo[i][0],(int)mapinfo[i][1],(int)mapinfo[i][2]) == new Vector3((int)v.x,(int)v.y,(int)v.z)){ mapinfo[i][3] = i2;
				//Debug.Log("yes");
				return;
			}
		}
		//Debug.Log(v);
		mapinfo.Add(new int[]{(int)v.x,(int)v.y,(int)v.z,i2,0});
	}
	
	void createhall(){
		int nfhi = -1;
		for(int i = 0; i < roomori.Count; i++){
			if(roomoriused[i] == false){
				nfhi = i;
				roomoriused[i] = true;
				break;
			}
		}
		if(nfhi != -1){
			Vector3 orifind = roomori[nfhi];
			Vector3 oriend = Vector3.zero;
			do{
				oriend = roomori[(int)Random.Range(0, roomori.Count)]; 
			} while(oriend == orifind);
			int pick = 0;//(int)Random.Range(0,2);
			int i19 = 0;
			Debug.Log("startwhile");
			while(orifind != oriend && i19 < 1000){
				if(orifind.x < oriend.x){
					//Debug.Log("X<X");
					if(pick == 0){
						orifind.x += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}
					} else if(orifind.z < oriend.z) {
						orifind.z += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
								
							
						}
					} else if(orifind.z > oriend.z){
						orifind.z -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
						}	
					}
				} else if(orifind.x > oriend.x){
					//Debug.Log("X>X");
					if(pick == 0){
						orifind.x -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}
					} else if(orifind.z < oriend.z) {
						orifind.z += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
								
							
						}
					} else if(orifind.z > oriend.z){
						orifind.z -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
						}	
					}
				}else if(orifind.z < oriend.z){
					//Debug.Log("Z<Z");
					if(pick == 0){
						orifind.z += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
								
							
						}
					} else if(orifind.x < oriend.x) {
						orifind.x += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}
					} else if(orifind.x > oriend.x){
						orifind.x -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}	
					}
				} else if(orifind.z > oriend.z){
					//Debug.Log("Z>Z");
					if(pick == 0){
						orifind.z -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(1,1,0),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(-1,1,0),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(1,0,0),1);
							//changeat(orifind+new Vector3(-1,0,0),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(1,-1,0),1);
							//changeat(orifind+new Vector3(-1,-1,0),1);
							//below feet
							//changeat(orifind+new Vector3(1,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(-1,-2,0),1);
								
							
						}
					} else if(orifind.x < oriend.x) {
						orifind.x += 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}
					} else if(orifind.x > oriend.x){
						orifind.x -= 1;
						if(!mapcontains2(orifind, 0)){
							//above head
							//changeat(orifind+new Vector3(0,1,1),1);
							//changeat(orifind+new Vector3(0,1,0),1);
							//changeat(orifind+new Vector3(0,1,-1),1);
							//head level
							changeat(orifind, 0);
							//changeat(orifind+new Vector3(0,0,1),1);
							//changeat(orifind+new Vector3(0,0,-1),1);
							//feet level
							changeat(orifind+new Vector3(0,-1,0),0);
							//changeat(orifind+new Vector3(0,-1,1),1);
							//changeat(orifind+new Vector3(0,-1,-1),1);
							//below feet
							//changeat(orifind+new Vector3(0,-2,1),1);
							//changeat(orifind+new Vector3(0,-2,0),1);
							//changeat(orifind+new Vector3(0,-2,-1),1);
								
							
						}	
					}
				}
				i19++;
			}
			
		} else {
			
		}
	}
	
	void reconsile(){
		for(int i = 0;i < mapinfo.Count;i++){
			for(int i2 = 0; i2 < mapinfo.Count;i2++){
				if(i != i2 && (int)mapinfo[i][0] == (int)mapinfo[i2][0] && (int)mapinfo[i][1] == (int)mapinfo[i2][1] && (int)mapinfo[i][2] == (int)mapinfo[i2][2]){
					if(mapinfo[i][3] == 0){mapinfo.RemoveAt(i);
						i--;
					} else if(mapinfo[i2][3] == 0){mapinfo.RemoveAt(i2);
						i2--;
					}
				
				}	
			}
		}
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
*/