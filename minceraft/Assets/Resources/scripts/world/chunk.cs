using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class chunk : MonoBehaviour {

	public block[,,] blocks = new block[16,16,16];
	public liquid[,,] liquids = new liquid[16,16,16];
	public List<Transform> quads = new List<Transform>();
	public List<Transform> hbxs = new List<Transform>();
	public Transform quad;
	public Transform quadclimb;
	public Transform hbx;
	public Transform hbx2; //water hitbox
	//public int tickrate = 10;
	//public int ticktimer = 0;
	public Vector3 xyz;
	public bool tickon = true;
	
	public bool test;
	public bool loaded = false;
	
	public Transform[] adjacentchunks = new Transform[4];
	
	public bool isgreedy = false;
	
	public int framessinceupdate = 0;
	public int greedyframes = 30;
	/*
	public Texture2D packed = new Texture2D(1,1);
	public List<Texture2D> packers = new List<Texture2D>();
	public Rect[] rects = new Rect[6];
	
	public void packt2d(){
		int w = 0;
		int h = 0;
		foreach(Texture2D t in packers){
			w += t.width;
			h += t.height;
		}
		
		packed.Resize(w,h);
		rects = packed.PackTextures(packers.ToArray(),0,Mathf.Max(w,h),false);
	}*/
	
	public IEnumerator applytex(Transform c, Material m){
		c.gameObject.GetComponent<Renderer>().material = m;
		yield return new WaitForEndOfFrame();
	}
	
	public IEnumerator scheduledroploot(Vector3 v, int id, float t){
		yield return new WaitForSeconds(t);
		droploot(v,id);
	}
	
	public void droploot(Vector3 v, int id){
		Transform clone2 = Instantiate(master.blockitem);
		clone2.parent = transform;
		clone2.localPosition = v; //???
		clone2.parent = master.items;
		clone2.GetComponent<item>().id = 0;
		clone2.GetComponent<item>().id2 = id;
		clone2.GetComponent<item>().makeitem(0, id);
	}
	
	/*public void converttogreedymesh2(){
		StartCoroutine(converttogreedymesh2());
	}*/
	
	List<GameObject> meshes = new List<GameObject>();

		void domesh(List<Vector3> verts, Material m_material){
			/*foreach(Transform c in transform){
				Destroy(c.gameObject);
			}*/
			
            int width = 17;
            int height = 17;
            int length = 17;
			

           
            int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
            int numMeshes = verts.Count / maxVertsPerMesh + 1;

            for (int i = 0; i < numMeshes; i++)
            {

                List<Vector3> splitVerts = new List<Vector3>();
                List<int> splitIndices = new List<int>();

                for (int j = 0; j < maxVertsPerMesh; j++)
                {
                    int idx = i * maxVertsPerMesh + j;

                    if (idx < verts.Count)
                    {
                        splitVerts.Add(verts[idx]);
                        splitIndices.Add(j);
                    }
                }

                if (splitVerts.Count == 0) continue;

                Mesh mesh = new Mesh();
                mesh.SetVertices(splitVerts);
                mesh.SetTriangles(splitIndices, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();

                GameObject go = new GameObject("Mesh");
                go.transform.parent = transform;
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.GetComponent<Renderer>().material = m_material;
                go.GetComponent<MeshFilter>().mesh = mesh;
                go.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2);

                meshes.Add(go);
            }
		}
		
		Transform domeshplane(Vector3 origin){
		/*foreach(Transform c in transform){
			Destroy(c.gameObject);
		}*/
		
		//int width = 17;
		//int height = 17;
		//int length = 17;
		List<Vector3> verts = new List<Vector3>();
		verts.Add(new Vector3(0.5f,0,0.5f) + origin);
		verts.Add(new Vector3(-0.5f,0,0.5f) + origin);
		verts.Add(new Vector3(-0.5f,0,-0.5f) + origin);
		verts.Add(new Vector3(0.5f,0,-0.5f) + origin);
		
		List<int> index = new List<int>();
		index.Add(0);
		index.Add(1);
		index.Add(2);
		
		index.Add(2);
		index.Add(3);
		index.Add(0);
		

			Mesh mesh = new Mesh();
			mesh.SetVertices(verts);
			mesh.SetTriangles(index, 0);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			GameObject go = new GameObject("Mesh");
			go.transform.parent = transform;
			go.AddComponent<MeshFilter>();
			go.AddComponent<MeshRenderer>();
			//go.GetComponent<Renderer>().material = m_material;
			go.GetComponent<MeshFilter>().mesh = mesh;
			go.transform.localPosition = origin;
			//meshes.Add(go);
			return go.transform;
		}
	
	public Material basemat;
	
	public void converttogreedymesh2(bool b){
		if(framessinceupdate > greedyframes || b){
			List<Vector3> verts = new List<Vector3>();
			List<Vector3> tempverts = new List<Vector3>();		
				
			for(int y = 0; y < 16; y++){ //non-transparent top
				for(int x = 0; x < 16; x++){
					for(int z = 0; z < 16; z++){
						if(!blocks[x,y,z].transparent && blocks[x,y,z].id != -1){
							if(y == 16 || blocks[x,y+1,z].transparent || blocks[x,y+1,z].id == -1){
								tempverts.Add(new Vector3(x,y,z)); 
								for(int z2 = z; z2 < 16; z2++){
									if(blocks[x,y,z2].transparent || blocks[x,y,z2].id == -1 || z2 == 15){
										tempverts.Add(new Vector3(x,y,z2));
										break;
									}
								}
								int x3t = x;
								int z3t = z;
								for(int x3 = x; x3 < 16; x3++){
									for(int z3 = z; z3 < 16; z3++){
										if(blocks[x3,y,z3].transparent || blocks[x3,y,z3].id == -1 || x3 == 15 && z3 == 15){
											tempverts.Add(new Vector3(x3t,y,z3t));
											break;
											break;
										}
										z3t++;
									}
									x3t++;
								}
								tempverts.Add(new Vector3(tempverts[0].x,y,tempverts[2].z));
								//add skip + texture
								//add (-0.5f/+0.5f/+0.5f,+0.5f,-0.5f/-0.5f,+0.5f) for top
								verts.Add(tempverts[0]+new Vector3(-0.5f,0.5f,-0.5f));
								verts.Add(tempverts[1]+new Vector3(+0.5f,0.5f,-0.5f));
								verts.Add(tempverts[2]+new Vector3(+0.5f,0.5f,+0.5f));
								
								verts.Add(tempverts[0]+new Vector3(-0.5f,0.5f,-0.5f));
								verts.Add(tempverts[2]+new Vector3(+0.5f,0.5f,+0.5f));
								verts.Add(tempverts[3]+new Vector3(-0.5f,0.5f,+0.5f));
								
								x = (int)tempverts[1].x;
								z = (int)tempverts[3].z;
								
							}
						}
					}
				}
			}
			
			domesh(verts, basemat);
			
			isgreedy = true;
			loaded = true;
		}
	}
	
	public void converttogreedymesh(bool b){ //do 2 passes, one for reg, one for transparent
		if(framessinceupdate > greedyframes || b){
			isgreedy = true;
			loaded = true;
			bool[,,] blockhere = new bool[16,16,16];
			bool[,,] blockhere0 = new bool[16,16,16];
			//Debug.Log("collect");
			for(int i = 0; i <= 15; i++){ //collect
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){					
						blockhere[i,i2,i3] = (blocks[i,i2,i3].id != -1 && !blocks[i,i2,i3].transparent?true:false);//collect blocks that are the same
						blockhere0[i,i2,i3] = (blocks[i,i2,i3].transparent && blocks[i,i2,i3].id != -1?true:false);//collect blocks that are the same
					
					}
					
				}	
			}
				
			//Debug.Log("reset");
			//yield return new WaitForSeconds(1.0f);
			//reset
			for(int i = 0; i < quads.Count; i++){ 
				if(quads[i].tag != "liquid"){
					//Destroy(quads[i].gameObject);
					//quads.RemoveAt(i);
					//i--;
					quads[i].gameObject.SetActive(false);
				}
			}
			for(int i = 0; i < hbxs.Count; i++){
				if(hbxs[i].tag != "liquid"){
					Destroy(hbxs[i].gameObject);
					hbxs.RemoveAt(i);
					i--;
				}
			}
			
			
			//Debug.Log("hitboxes");
			
			//yield return new WaitForSeconds(1.0f);
			//new
			//hitboxes (MAKE 3D?)
			bool[,,] blockhere2 = new bool[16,16,16]; //1st pass
			bool[,,] blockhere3 = new bool[16,16,16]; //1st pass backward
			bool[,,] blockhere4 = new bool[16,16,16]; //2nd pass 
			bool[,,] blockhere5 = new bool[16,16,16]; //2nd pass backward
			bool[,,] blockhere6 = new bool[16,16,16]; //hitboxes
			for(int i = 0; i <= 15; i++){ //collect
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
						blockhere3[i,i2,i3] = blockhere[i,i2,i3];
						blockhere4[i,i2,i3] = blockhere0[i,i2,i3]; 
						blockhere5[i,i2,i3] = blockhere0[i,i2,i3]; 
						blockhere6[i,i2,i3] = blockhere[i,i2,i3];//use this for hitboxes
					}
				}
			}
			//find transparent blocks left and right
			Transform tempadj = findadj(new Vector3(-1,0,0)); //forward
			Transform tempadj2 = findadj(new Vector3(1,0,0)); //backward
			for(int i2 = 0; i2 <= 15; i2++){ 
				for(int i3 = 0; i3 <= 15; i3++){ 
					if(tempadj != null && !tempadj.GetComponent<chunk>().blocks[15,i2,i3].transparent)blockhere2[0,i2,i3] = false;
					if(tempadj2 != null && !tempadj2.GetComponent<chunk>().blocks[0,i2,i3].transparent)blockhere3[15,i2,i3] = false;
					if(tempadj != null && tempadj.GetComponent<chunk>().blocks[15,i2,i3].id != -1)blockhere4[0,i2,i3] = false;
					if(tempadj2 != null && tempadj2.GetComponent<chunk>().blocks[0,i2,i3].id != -1)blockhere5[15,i2,i3] = false;
				}
			}
			
			for(int i = 0;i < 16; i++){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
						if(i!=0){
							if(blockhere[i-1,i02,i03]){
								blockhere2[i,i02,i03] = false;
								//blockhere6[i,i02,i03] = false;
							}
							if(blockhere0[i-1,i02,i03]){
								blockhere4[i,i02,i03] = false;
							}
							
						}
						if(i!=15){
							if(blockhere[i+1,i02,i03]){
								blockhere3[i,i02,i03] = false;
								
							}
							if(blockhere0[i+1,i02,i03]){
								blockhere5[i,i02,i03] = false;
								
							}
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere6[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere6[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere6[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere6[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere6[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere6[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere6[i,i7,i8] = false;
					}
				}
				Transform clone2 = Instantiate(hbx);
				clone2.parent = this.transform;
				clone2.localPosition = new Vector3(i,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
				clone2.localScale = new Vector3(1,i4-i2+1,i5-i3+1);
				hbxs.Add(clone2);
				//Transform tempadj = findadj(new Vector3(-1,0,0));
				/*if((i == 0 && (tempadj == null || tempadj.GetComponent<chunk>().blocks[15,i2,i3].transparent && (tempadj.GetComponent<chunk>().blocks[15,i2,i3].id == -1 || !blocks[i,i2,i3].transparent))) || i != 0 && blocks[i-1,i2,i3].transparent && (blocks[i-1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
					//left
					Transform clone = Instantiate(quad);
					clone.parent = this.transform;
					clone.localPosition = new Vector3(i-0.5f,i2,i3);
					clone.rotation = Quaternion.Euler(0,90,0);
					clone.localScale = new Vector3(1,1,1);
					clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
					quads.Add(clone);
				}*/
				
				
				//left
				i2 = 0;
				i3 = 0;
				while(blockhere2[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere2[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i,i7,i8] = false;
					}
				}
				
				Texture2D t2d = new Texture2D(1,1);
				Material mat = new Material(master.mat1);
				Transform clone;
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					//Debug.Log(clone);
					clone.parent = this.transform;
					clone.localPosition = new Vector3(i-0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
					clone.rotation = Quaternion.Euler(0,90,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i,i02,i03].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(-1-i05-((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//right
				i2 = 0;
				i3 = 0;
				while(blockhere3[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere3[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere3[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere3[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere3[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere3[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere3[i,i7,i8] = false;
					}
				}
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(i+0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
					clone.rotation = Quaternion.Euler(0,-90,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i,i02,i03].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				//left transparent
				i2 = 0;
				i3 = 0;
				while(blockhere4[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere4[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere4[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere4[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere4[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere4[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere4[i,i7,i8] = false;
					}
				}
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(i-0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
					clone.rotation = Quaternion.Euler(0,90,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat2);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i,i02,i03].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(-1-i05-((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				//right transparent
				i2 = 0;
				i3 = 0;
				while(blockhere5[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere5[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere5[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere5[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere5[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere5[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere5[i,i7,i8] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(i+0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
					clone.rotation = Quaternion.Euler(0,-90,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i,i02,i03].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//fix this one to check all
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i,i2,i3] == true)done = false;
						if(blockhere3[i,i2,i3] == true)done = false;
						if(blockhere4[i,i2,i3] == true)done = false;
						if(blockhere5[i,i2,i3] == true)done = false;
						if(blockhere6[i,i2,i3] == true)done = false;
					}
				}
				if(!done)i--;
			}
			
			//forward back
			
			for(int i = 0; i <= 15; i++){ //collect
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
						blockhere3[i,i2,i3] = blockhere[i,i2,i3];
						blockhere4[i,i2,i3] = blockhere0[i,i2,i3]; 
						blockhere5[i,i2,i3] = blockhere0[i,i2,i3]; 
						//blockhere6[i,i2,i3] = blockhere[i,i2,i3];//use this for hitboxes
					}
				}
			}
			//find transparent blocks left and right
			tempadj = findadj(new Vector3(0,0,-1)); //forward
			tempadj2 = findadj(new Vector3(0,0,1)); //backward
			for(int i2 = 0; i2 <= 15; i2++){ 
				for(int i3 = 0; i3 <= 15; i3++){ 
					if(tempadj != null && !tempadj.GetComponent<chunk>().blocks[i3,i2,15].transparent)blockhere2[i3,i2,0] = false;
					if(tempadj2 != null && !tempadj2.GetComponent<chunk>().blocks[i3,i2,0].transparent)blockhere3[i3,i2,15] = false;
					if(tempadj != null && tempadj.GetComponent<chunk>().blocks[i3,i2,15].id != -1)blockhere4[i3,i2,0] = false;
					if(tempadj2 != null && tempadj2.GetComponent<chunk>().blocks[i3,i2,0].id != -1)blockhere5[i3,i2,15] = false;
				}
			}
			
			for(int i = 0;i < 16; i++){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
						if(i!=0){
							if(blockhere[i03,i02,i-1]){
								blockhere2[i03,i02,i] = false;
							}
							if(blockhere0[i03,i02,i-1]){
								blockhere4[i03,i02,i] = false;
							}
							
						}
						if(i!=15){
							if(blockhere[i03,i02,i+1]){
								blockhere3[i03,i02,i] = false;
								
							}
							if(blockhere0[i03,i02,i+1]){
								blockhere5[i03,i02,i] = false;
								
							}
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				
				
				
				//left
				i2 = 0;
				i3 = 0;
				while(blockhere2[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere2[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i7,i] = false;
					}
				}
				Transform clone;
				Material mat;
				Texture2D t2d;
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i-0.5f);
					clone.rotation = Quaternion.Euler(0,0,90);
					clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i02,i].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i04+((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//right
				i2 = 0;
				i3 = 0;
				while(blockhere3[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere3[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere3[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere3[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere3[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere3[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere3[i8,i7,i] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
				
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i+0.5f);
					clone.rotation = Quaternion.Euler(180,0,90);
					clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i02,i].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(-1-i04-((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				//left transparent
				i2 = 0;
				i3 = 0;
				while(blockhere4[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere4[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere4[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere4[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere4[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere4[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere4[i8,i7,i] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i-0.5f);
					clone.rotation = Quaternion.Euler(0,0,90);
					clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i02,i].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i04+((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//right transparent
				i2 = 0;
				i3 = 0;
				while(blockhere5[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere5[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere5[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere5[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere5[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere5[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere5[i8,i7,i] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i+0.5f);
					clone.rotation = Quaternion.Euler(180,0,90);
					clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i02,i].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(-1-i04-((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//fix this one to check all
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i2,i] == true)done = false;
						if(blockhere3[i3,i2,i] == true)done = false;
						if(blockhere4[i3,i2,i] == true)done = false;
						if(blockhere5[i3,i2,i] == true)done = false;
					}
				}
				if(!done)i--;
			}
			
			//top bottom
			
			for(int i = 0; i <= 15; i++){ //collect
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
						blockhere3[i,i2,i3] = blockhere[i,i2,i3];
						blockhere4[i,i2,i3] = blockhere0[i,i2,i3]; 
						blockhere5[i,i2,i3] = blockhere0[i,i2,i3]; 
						//blockhere6[i,i2,i3] = blockhere[i,i2,i3];//use this for hitboxes
					}
				}
			}
			//find transparent blocks left and right
			tempadj = findadj(new Vector3(0,-1,0)); //forward
			tempadj2 = findadj(new Vector3(0,1,0)); //backward
			for(int i2 = 0; i2 <= 15; i2++){ 
				for(int i3 = 0; i3 <= 15; i3++){ 
					if(tempadj != null && !tempadj.GetComponent<chunk>().blocks[i3,15,i2].transparent)blockhere2[i3,0,i2] = false;
					if(tempadj2 != null && !tempadj2.GetComponent<chunk>().blocks[i3,0,i2].transparent)blockhere3[i3,15,i2] = false;
					if(tempadj != null && tempadj.GetComponent<chunk>().blocks[i3,15,i2].id != -1)blockhere4[i3,0,i2] = false;
					if(tempadj2 != null && tempadj2.GetComponent<chunk>().blocks[i3,0,i2].id != -1)blockhere5[i3,0,i2] = false;
				}
			}
			
			for(int i = 0;i < 16; i++){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
						if(i!=0){
							if(blockhere[i03,i-1,i02]){
								blockhere2[i03,i,i02] = false;
							}
							if(blockhere0[i03,i-1,i02]){
								blockhere4[i03,i,i02] = false;
							}
							
						}
						if(i!=15){
							if(blockhere[i03,i+1,i02]){
								blockhere3[i03,i,i02] = false;
								
							}
							if(blockhere0[i03,i+1,i02]){
								blockhere5[i03,i,i02] = false;
								
							}
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				
				
				
				//left
				i2 = 0;
				i3 = 0;
				while(blockhere2[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere2[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i,i7] = false;
					}
				}
				Transform clone;
				Material mat;
				Texture2D t2d;
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i-0.5f,Mathf.Lerp(i2,i4,0.5f));
					clone.rotation = Quaternion.Euler(-90,0,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i,i02].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				
				//right
				i2 = 0;
				i3 = 0;
				while(blockhere3[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere3[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere3[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere3[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere3[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere3[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere3[i8,i,i7] = false;
					}
				}
				
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = Instantiate(quadclimb);
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i+0.5f,Mathf.Lerp(i2,i4,0.5f));
					clone.rotation = Quaternion.Euler(90,0,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i,i02].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//left transparent
				i2 = 0;
				i3 = 0;
				while(blockhere4[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere4[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere4[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere4[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere4[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere4[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere4[i8,i,i7] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = grabfrompool();
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i-0.5f,Mathf.Lerp(i2,i4,0.5f));
					clone.rotation = Quaternion.Euler(-90,0,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i,i02].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				
				//right transparent
				i2 = 0;
				i3 = 0;
				while(blockhere5[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				i4 = i2;
				i5 = i3;
				i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere5[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere5[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere5[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					if(i4 != i2){
						i6 = i2;
						while(blockhere5[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere5[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere5[i8,i,i7] = false;
					}
				}
				
				if(i5-i3+1 != 0 && i4-i2+1 != 0){
					clone = Instantiate(quadclimb);
					clone.parent = this.transform;
					clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i+0.5f,Mathf.Lerp(i2,i4,0.5f));
					clone.rotation = Quaternion.Euler(90,0,0);
					clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
					mat = new Material(master.mat1);
					t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
					t2d.filterMode = FilterMode.Point;
					for(int i02 = i2; i02 <= i4 ; i02++){
						for(int i03 = i3; i03 <= i5; i03++){
							//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
							if(blocks[i03,i,i02].id != -1){
								for(int i04 = 0; i04 < 16; i04++){
									for(int i05 = 0; i05 < 16; i05++){
										t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
									}
								}
							}
						}
					}
					t2d.Apply();
					mat.mainTexture = t2d;
					StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
					
					quads.Add(clone);
				}
				
				//fix this one to check all
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i,i2] == true)done = false;
						if(blockhere3[i3,i,i2] == true)done = false;
						if(blockhere4[i3,i,i2] == true)done = false;
						if(blockhere5[i3,i,i2] == true)done = false;
					}
				}
				if(!done)i--;
			}
			
			//yield return new WaitForEndOfFrame();
			
			//Debug.Log("left");
			//yield return new WaitForSeconds(1.0f);
			//left
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			Transform tempadj = findadj(new Vector3(-1,0,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[15,i2,i3].transparent)blockhere2[0,i2,i3] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){ 
				//yield return new WaitForEndOfFrame(); 
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i-1,i02,i03])blockhere2[i,i02,i03] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				//Debug.Log("startpoint");
				//yield return new WaitForSeconds(1.0f);
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log("endpoint 1");
				//yield return new WaitForSeconds(1.0f);
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i,i6,i5] != true){
					i5--;
				}
				//Debug.Log("endpoint 2");
				//yield return new WaitForSeconds(1.0f);
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i,i7,i8] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i-0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
				clone.rotation = Quaternion.Euler(0,90,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i,i02,i03].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(-1-i05-((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						//Debug.Log(blocks[i,i2,i3].id);
						if(blockhere2[i,i2,i3] == true)done = false;
					}
				}
				
				//Debug.Log("reset");
				//yield return new WaitForSeconds(1.0f);
				if(!done)i--;
			}*/
			
			//right
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(1,0,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[0,i2,i3].transparent)blockhere2[15,i2,i3] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i+1,i02,i03])blockhere2[i,i02,i03] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i,i7,i8] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i+0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
				clone.rotation = Quaternion.Euler(0,-90,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i,i02,i03].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i,i2,i3] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//back
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,0,1));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,i2,0].transparent)blockhere2[i3,i2,15] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i02,i+1])blockhere2[i03,i02,i] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i7,i] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i+0.5f);
				clone.rotation = Quaternion.Euler(180,0,90);
				clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i02,i].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(-1-i04-((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i2,i] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//front
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,0,-1));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,i2,15].transparent)blockhere2[i3,i2,0] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i02,i-1])blockhere2[i03,i02,i] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i7,i] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i-0.5f);
				clone.rotation = Quaternion.Euler(0,0,90);
				clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i02,i].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i04+((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i2,i] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//top
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,-1,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,0,i2].transparent)blockhere2[i3,15,i2] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i+1,i02])blockhere2[i03,i,i02] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i,i7] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i+0.5f,Mathf.Lerp(i2,i4,0.5f));
				clone.rotation = Quaternion.Euler(90,0,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i,i02].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i,i2] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//bottom
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,1,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,15,i2].transparent)blockhere2[i3,0,i2] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){ 
				//yield return new WaitForEndOfFrame();
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i-1,i02])blockhere2[i03,i,i02] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i,i7] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i-0.5f,Mathf.Lerp(i2,i4,0.5f));
				clone.rotation = Quaternion.Euler(-90,0,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat1);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i,i02].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i,i2] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			
		
			
			//left transparent
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(-1,0,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[15,i2,i3].transparent)blockhere2[0,i2,i3] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){ 
				//yield return new WaitForEndOfFrame(); 
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i-1,i02,i03])blockhere2[i,i02,i03] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				//Debug.Log("startpoint");
				//yield return new WaitForSeconds(1.0f);
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log("endpoint 1");
				//yield return new WaitForSeconds(1.0f);
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i,i6,i5] != true){
					i5--;
				}
				//Debug.Log("endpoint 2");
				//yield return new WaitForSeconds(1.0f);
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i,i7,i8] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i-0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
				clone.rotation = Quaternion.Euler(0,90,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i,i02,i03].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(-1-i05-((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						//Debug.Log(blocks[i,i2,i3].id);
						if(blockhere2[i,i2,i3] == true)done = false;
					}
				}
				
				//Debug.Log("reset");
				//yield return new WaitForSeconds(1.0f);
				if(!done)i--;
			}*/
			
			//right
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(1,0,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[0,i2,i3].transparent)blockhere2[15,i2,i3] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i+1,i02,i03])blockhere2[i,i02,i03] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i,i2,i3] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i,i4,i5] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i,i4,i5] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i,i6,i5] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i,i6,i5] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i,i6,i5] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i,i7,i8] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i+0.5f,Mathf.Lerp(i2,i4,0.5f),Mathf.Lerp(i3,i5,0.5f));
				clone.rotation = Quaternion.Euler(0,-90,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i,i02,i03].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i,i02,i03].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i,i2,i3] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//back
			
			/*for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i02,i+1])blockhere2[i03,i02,i] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i7,i] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i+0.5f);
				clone.rotation = Quaternion.Euler(180,0,90);
				clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i02,i].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(-1-i04-((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i2,i] == true)done = false;
					}
				}
				if(!done)i--;
			}
			*/
			
			//front
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,0,-1));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,i2,15].transparent)blockhere2[i3,i2,0] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i02,i-1])blockhere2[i03,i02,i] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i2,i] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i4,i] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i4,i] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i6,i] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i6,i] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i6,i] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i7,i] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),Mathf.Lerp(i2,i4,0.5f),i-0.5f);
				clone.rotation = Quaternion.Euler(0,0,90);
				clone.localScale = new Vector3(i4-i2+1,i5-i3+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i4-i2+1)*16,(i5-i3+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//Debug.Log(blocks[i,i02,i03].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i02,i].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i04+((i02-i2)*16),-1-i05-((i03-i3)*16),master.matslist1[blocks[i03,i02,i].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i2,i] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//top
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,-1,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,0,i2].transparent)blockhere2[i3,15,i2] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){
				//yield return new WaitForEndOfFrame(); 
				if(i != 15){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i+1,i02])blockhere2[i03,i,i02] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i,i7] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i+0.5f,Mathf.Lerp(i2,i4,0.5f));
				clone.rotation = Quaternion.Euler(90,0,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i,i02].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i,i2] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			//bottom
			/*for(int i = 0; i <= 15; i++){ 
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
						blockhere2[i,i2,i3] = blockhere[i,i2,i3];
					}
				}
			}
			tempadj = findadj(new Vector3(0,1,0));
			if(tempadj != null){
				for(int i2 = 0; i2 <= 15; i2++){ 
					for(int i3 = 0; i3 <= 15; i3++){ 
					if(!tempadj.GetComponent<chunk>().blocks[i3,15,i2].transparent)blockhere2[i3,0,i2] = false;
					}
				}
			}
			for(int i = 0;i < 16; i++){ 
				//yield return new WaitForEndOfFrame();
				if(i != 0){
					for(int i02 = 0; i02 <= 15; i02++){ 
						for(int i03 = 0; i03 <= 15; i03++){ 
							if(blockhere[i03,i-1,i02])blockhere2[i03,i,i02] = false;
						}
					}
				}
				int i2 = 0;
				int i3 = 0;
				while(blockhere2[i3,i,i2] != true){ //startpoint
					i2++;
					if(i2 == 16){
						i2 = 0;
						i3++;
						
						if(i3 == 16){
							i3 = 15;
							break;
						}
					}
				}
				int i4 = i2;
				int i5 = i3;
				int i6 = i2;
				//Debug.Log(i + " " + i4 + " " + i5);
				while(blockhere2[i5,i,i4] == true){ //find endpoint 1
					i4++;
					if(i4 == 16)i4 = 15;
					if(i4 == 15)break;
				}
				if(blockhere2[i5,i,i4] != true){
					i4--;
				}
				//Debug.Log(i + " " + i6 + " " + i5 + " " +i4);
				while(blockhere2[i5,i,i6] == true){ //find endpoint 2
					i5++;
					if(i5 == 16)i5 = 15;
					if(i5 == 15)break;
					i6 = i2;
					if(i4 != i2){
						while(blockhere2[i5,i,i6] == true){ //find endpoint 2
							i6++;
							if(i6 == 16)i6 = 15;
							if(i6 == i4){
								break;
							}
						}
					}
				}
				if(blockhere2[i5,i,i6] != true){
					i5--;
				}
				for(int i7 = i2;i7 <= i4;i7++){
					for(int i8 = i3; i8 <= i5;i8++){
						blockhere2[i8,i,i7] = false;
					}
				}
				
				//Debug.Log(i+" "+i2+" "+i3+" "+i4+" "+i5);
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(Mathf.Lerp(i3,i5,0.5f),i-0.5f,Mathf.Lerp(i2,i4,0.5f));
				clone.rotation = Quaternion.Euler(-90,0,0);
				clone.localScale = new Vector3(i5-i3+1,i4-i2+1,1);
				Material mat = new Material(master.mat2);
				Texture2D t2d = new Texture2D((i5-i3+1)*16,(i4-i2+1)*16);
				t2d.filterMode = FilterMode.Point;
				for(int i02 = i2; i02 <= i4 ; i02++){
					for(int i03 = i3; i03 <= i5; i03++){
						//if(i2 == 6)Debug.Log(blocks[i03,i,i02].id + " " + i + " " + i02 + " " + i03);	
						if(blocks[i03,i,i02].id != -1){
							for(int i04 = 0; i04 < 16; i04++){
								for(int i05 = 0; i05 < 16; i05++){
									t2d.SetPixel(i05+((i03-i3)*16),i04+((i02-i2)*16),master.matslist1[blocks[i03,i,i02].id].GetPixel(i04,i05));
								}
							}
						}
					}
				}
				t2d.Apply();
				mat.mainTexture = t2d;
				StartCoroutine(applytex(clone,mat));//clone.gameObject.GetComponent<Renderer>().material = mat;
				
				quads.Add(clone);
				
				bool done = true;
				for(i2 = 0; i2 <= 15; i2++){
					for(i3 = 0; i3 <= 15; i3++){
						if(blockhere2[i3,i,i2] == true)done = false;
					}
				}
				if(!done)i--;
			}*/
			
			
			isgreedy = true;
			loaded = true;
		}
	}
	
	void Awake(){
		xyz = transform.position/16;
		populate();
	}
	
	void Start(){
		findadjacent();
		createpool();
	}
	
	void createpool(){
		for (int i = 0; i < 32; i++){
			Transform clone = Instantiate(quad);
			clone.parent = transform;
			clone.gameObject.SetActive(false);
		}
	}
	
	Transform grabfrompool(){
		foreach(Transform c in transform){
			if(!c.gameObject.activeSelf){
				c.gameObject.SetActive(true);
				return c;
			}
		}
		Transform clone = Instantiate(quad);
		return clone;
	}
	
	void findadjacent(){
		int adj = 0;
		foreach(Transform child in transform.parent){
			if(child != transform && Vector3.Distance(xyz,child.GetComponent<chunk>().xyz) < 1.05f){
				if(adj <4)adjacentchunks[adj] = child;
				adj++;
			}
		}
	}
	
	public int x,y,z;
	public int ticktimer = 0;
	public int tickrate = 60;
	
	void Update(){
		framessinceupdate++;
		/*ticktimer++;
		if(ticktimer == tickrate){
			breakblock(x,y,z);
			x++;
			if(x == 16){
				y++;
				x = 0;
				if(y == 16){
					z++;
					y = 0;
				}
			}
			ticktimer = 0;
		}*/
	}
	
	public void dotick(int i){
		bool tempon = false;
		foreach(block child in blocks){
			if(child.tickon){
				child.dotick(i);
				tempon = true;
			}
		}
		
		
		foreach(liquid child in liquids){
			if(child.tickon){
				child.dotick(i);
				tempon = true;
			}
		}
		
		updateliquids();
		for(int i4 = 0; i4 <= 15; i4++){ //x
			for(int i2 = 0; i2 <= 15; i2++){ //y 
				for(int i3 = 0; i3 <= 15; i3++){ //z
					
					if(liquids[i4,i2,i3].id != -1 && !liquids[i4,i2,i3].settled){
						if(liquids[i4,i2,i3].ticknum == i && (liquids[i4,i2,i3].level != 1 || liquids[i4,i2,i3].inf == 0))StartCoroutine(flow(i4,i2,i3));
						tempon = true;
						liquids[i4,i2,i3].passcount = 0;
					} else {
						liquids[i4,i2,i3].passcount++;
						if(liquids[i4,i2,i3].passcount > 3)liquids[i4,i2,i3].settled = true;
					}
				}
			}
		}
		if(!tempon)tickon = false;
		
	}
	
	IEnumerator flow(int i, int i2, int i3){
		//Debug.Log("yes");
		yield return new WaitForEndOfFrame();
		if(liquids[i,i2,i3].inf == 0){
			if(i != 0 && blocks[i-1,i2,i3].id == -1 && liquids[i-1,i2,i3].id == -1){
				liquid templiquid = new liquid();
				templiquid.id = liquids[i,i2,i3].id;
				templiquid.level = liquids[i,i2,i3].level;
				templiquid.buildliquid();
				liquids[i-1,i2,i3] = templiquid;
			}
			if(i != 15 && blocks[i+1,i2,i3].id == -1 && liquids[i+1,i2,i3].id == -1){
				liquid templiquid = new liquid();
				templiquid.id = liquids[i,i2,i3].id;
				templiquid.level = liquids[i,i2,i3].level;
				templiquid.buildliquid();
				liquids[i+1,i2,i3] = templiquid;
			}
			if(i == 0 || i == 15) {
				foreach(Transform c in adjacentchunks){
					if(c != null && c.GetComponent<chunk>().xyz.x-xyz.x == (i == 0?-1:1)){
						if(c.GetComponent<chunk>().liquids[(i == 0?15:0),i2,i3].id == -1 && c.GetComponent<chunk>().blocks[(i == 0?15:0),i2,i3].id == -1){
							liquid templiquid = new liquid();
							templiquid.id = liquids[i,i2,i3].id;
							templiquid.level = liquids[i,i2,i3].level;
							templiquid.buildliquid();
							c.GetComponent<chunk>().liquids[(i == 0?15:0),i2,i3] = templiquid;
							c.GetComponent<chunk>().tickon = true;
						}
					}
				}
			}
			if(i3 != 0 && blocks[i,i2,i3-1].id == -1 && liquids[i,i2,i3-1].id == -1){
				liquid templiquid = new liquid();
				templiquid.id = liquids[i,i2,i3].id;
				templiquid.level = liquids[i,i2,i3].level;
				templiquid.buildliquid();
				liquids[i,i2,i3-1] = templiquid;
			}
			if(i3 != 15 && blocks[i,i2,i3+1].id == -1 && liquids[i,i2,i3+1].id == -1){
				liquid templiquid = new liquid();
				templiquid.id = liquids[i,i2,i3].id;
				templiquid.level = liquids[i,i2,i3].level;
				templiquid.buildliquid();
				liquids[i,i2,i3+1] = templiquid;
			}
			if(i3 == 0 || i3 == 15) {
				foreach(Transform c in adjacentchunks){
					if(c != null  && c.GetComponent<chunk>().xyz.z-xyz.z == (i3 == 0?-1:1)){
						if(c.GetComponent<chunk>().liquids[i,i2,(i3 == 0?15:0)].id == -1 && c.GetComponent<chunk>().blocks[i,i2,(i3 == 0?15:0)].id == -1){
							liquid templiquid = new liquid();
							templiquid.id = liquids[i,i2,i3].id;
							templiquid.level = liquids[i,i2,i3].level;
							templiquid.buildliquid();
							c.GetComponent<chunk>().liquids[i,i2,(i3 == 0?15:0)] = templiquid;
							c.GetComponent<chunk>().tickon = true;
						}
					}
				}
			}
			if(i2 != 0 && blocks[i,i2-1,i3].id == -1 && liquids[i,i2-1,i3].id == -1){
				liquid templiquid = new liquid();
				templiquid.id = liquids[i,i2,i3].id;
				templiquid.level = liquids[i,i2,i3].level;
				templiquid.buildliquid();
				liquids[i,i2-1,i3] = templiquid;
			}
			if(i2 == 0 || i2 == 15) {
				foreach(Transform c in adjacentchunks){
					if(c != null && c.GetComponent<chunk>().xyz.y-xyz.y == (i2 == 0?-1:1)){
						if(c.GetComponent<chunk>().liquids[i,(i2 == 0?15:0),i3].id == -1 && c.GetComponent<chunk>().blocks[i,(i2 == 0?15:0),i3].id == -1){
							liquid templiquid = new liquid();
							templiquid.id = liquids[i,i2,i3].id;
							templiquid.level = liquids[i,i2,i3].level;
							templiquid.buildliquid();
							c.GetComponent<chunk>().liquids[i,(i2 == 0?15:0),i3] = templiquid;
							c.GetComponent<chunk>().tickon = true;
						}
					}
				}
			}
			liquids[i,i2,i3].settled = true;
		}
		//river
		if(liquids[i,i2,i3].inf != 0){
			bool[] dirs = new bool[6];
			for(int i01 = 0; i01 < 6; i01++){
				dirs[i01] = false;			
			}
			bool[] dirs2 = new bool[5];
			for(int i01 = 0; i01 < 5; i01++){
				dirs2[i01] = false;			
			}
			while(!(dirs2[0] && dirs2[1] && dirs2[2] && dirs2[3] && dirs2[4])){
				if(dirs2[4] && !dirs2[0] && Random.Range(1,5) == 1){
					dirs2[0] = true;
					if(i != 0 && blocks[i-1,i2,i3].id == -1 && liquids[i-1,i2,i3].id == -1 && liquids[i,i2,i3].level != 1){
						liquid templiquid = new liquid();
						templiquid.id = liquids[i,i2,i3].id;
						templiquid.level += 1;
						liquids[i,i2,i3].level -= 1;
						templiquid.inf = 1;
						templiquid.buildliquid();
						liquids[i-1,i2,i3] = templiquid;
						dirs[0] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i-1,i2,i3].drawn = 2;
					} else if(i != 0 && liquids[i-1,i2,i3].id == liquids[i,i2,i3].id && !liquids[i,i2,i3].sources[0] && liquids[i,i2,i3].level > liquids[i-1,i2,i3].level && liquids[i,i2,i3].level != 1){
						liquids[i-1,i2,i3].id = liquids[i,i2,i3].id;
						int temp = liquids[i,i2,i3].level-liquids[i-1,i2,i3].level;
						liquids[i-1,i2,i3].level += 1;
						liquids[i,i2,i3].level -= 1;
						//liquids[i-1,i2,i3].sources[1] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i-1,i2,i3].drawn = 2;
					}
				} else if(dirs2[4] && !dirs2[1] && Random.Range(1,4) == 1){
					dirs2[1] = true;
					if(i != 15 && blocks[i+1,i2,i3].id == -1 && liquids[i+1,i2,i3].id == -1 && liquids[i,i2,i3].level != 1){
						liquid templiquid = new liquid();
						templiquid.id = liquids[i,i2,i3].id;
						templiquid.level += 1;
						liquids[i,i2,i3].level -= 1;
						templiquid.inf = 1;
						templiquid.buildliquid();
						liquids[i+1,i2,i3] = templiquid;
						/*if(dirs[0]){
							int tempquid = liquids[i-1,i2,i3].level;
							liquids[i-1,i2,i3].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}*/
						dirs[1] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i+1,i2,i3].drawn = 2;
					} else if(i != 15 && liquids[i+1,i2,i3].id == liquids[i,i2,i3].id && !liquids[i,i2,i3].sources[1] && liquids[i,i2,i3].level > liquids[i+1,i2,i3].level && liquids[i,i2,i3].level != 1){
						liquids[i+1,i2,i3].id = liquids[i,i2,i3].id;
						int temp = liquids[i,i2,i3].level-liquids[i+1,i2,i3].level;
						liquids[i+1,i2,i3].level += 1;
						liquids[i,i2,i3].level -= 1;
						//liquids[i+1,i2,i3].sources[0] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i+1,i2,i3].drawn = 2;
					}
				} else if(dirs2[4] && !dirs2[2] && Random.Range(1,3) == 1){
					dirs2[2] = true;
					if(i3 != 0 && blocks[i,i2,i3-1].id == -1 && liquids[i,i2,i3-1].id == -1 && liquids[i,i2,i3].level != 1){
						liquid templiquid = new liquid();
						templiquid.id = liquids[i,i2,i3].id;
						templiquid.level += 1;
						liquids[i,i2,i3].level -= 1;
						templiquid.inf = 1;
						templiquid.buildliquid();
						liquids[i,i2,i3-1] = templiquid;
						/*if(dirs[0]){
							int tempquid = liquids[i-1,i2,i3].level;
							liquids[i-1,i2,i3].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}if(dirs[1]){
							int tempquid = liquids[i+1,i2,i3].level;
							liquids[i+1,i2,i3].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}*/
						dirs[2] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2,i3-1].drawn = 2;
					} else if(i3 != 0 && liquids[i,i2,i3-1].id == liquids[i,i2,i3].id && !liquids[i,i2,i3].sources[3] && liquids[i,i2,i3].level > liquids[i,i2,i3-1].level && liquids[i,i2,i3].level != 1){
						liquids[i,i2,i3-1].id = liquids[i,i2,i3].id;
						int temp = liquids[i,i2,i3].level-liquids[i,i2,i3-1].level;
						
						liquids[i,i2,i3-1].level += 1;
						liquids[i,i2,i3].level -= 1;
						//liquids[i,i2,i3-1].sources[2] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2,i3-1].drawn = 2;
					}
				} else if(dirs2[4] && !dirs2[3] && Random.Range(1,2) == 1){
					dirs2[3] = true;
					if(i3 != 15 && blocks[i,i2,i3+1].id == -1 && liquids[i,i2,i3+1].id == -1 && liquids[i,i2,i3].level != 1){
						liquid templiquid = new liquid();
						templiquid.id = liquids[i,i2,i3].id;
						
						templiquid.level += 1;
						liquids[i,i2,i3].level -= 1;
						templiquid.inf = 1;
						templiquid.buildliquid();
						liquids[i,i2,i3+1] = templiquid;
						/*if(dirs[0]){
							int tempquid = liquids[i-1,i2,i3].level;
							liquids[i-1,i2,i3].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}if(dirs[1]){
							int tempquid = liquids[i+1,i2,i3].level;
							liquids[i+1,i2,i3].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}if(dirs[2]){
							int tempquid = liquids[i,i2,i3-1].level;
							liquids[i,i2,i3-1].level -= Mathf.Max(1,(int)Mathf.Ceil(templiquid.level/2.0f));
							liquids[i,i2,i3].level += Mathf.Max(1,(int)Mathf.Floor(tempquid/2.0f));
						}*/
						dirs[3] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2,i3+1].drawn = 2;
					} else if(i3 != 15 && liquids[i,i2,i3+1].id == liquids[i,i2,i3].id && !liquids[i,i2,i3].sources[2] && liquids[i,i2,i3].level > liquids[i,i2,i3+1].level && liquids[i,i2,i3].level != 1){
						liquids[i,i2,i3+1].id = liquids[i,i2,i3].id;
						int temp = liquids[i,i2,i3].level-liquids[i,i2,i3+1].level;
						
						liquids[i,i2,i3+1].level += 1;
						liquids[i,i2,i3].level -= 1;
						//liquids[i,i2,i3+1].sources[3] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2,i3+1].drawn = 2;
					}
				} else if(!dirs2[4]){
					dirs2[4] = true;
					//feeding down fix
					if(i2 != 0 && blocks[i,i2-1,i3].id == -1 && liquids[i,i2-1,i3].id == -1 && liquids[i,i2,i3].level != 1){
						liquid templiquid = new liquid();
						templiquid.id = liquids[i,i2,i3].id;
						templiquid.level = Mathf.Max(1,liquids[i,i2,i3].level);
						liquids[i,i2,i3].level = 0;
						liquids[i,i2,i3].id = -1;
						templiquid.inf = 1;
						templiquid.buildliquid();
						liquids[i,i2-1,i3] = templiquid;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2-1,i3].drawn = 2;
						/*if(dirs[0])liquids[i-1,i2,i3].level -= templiquid.level/2;
						if(dirs[1])liquids[i+1,i2,i3].level -= templiquid.level/2;
						if(dirs[2])liquids[i,i2,i3-1].level -= templiquid.level/2;
						if(dirs[3])liquids[i,i2,i3+1].level -= templiquid.level/2;
						dirs[4] = true;*/
					} else if(i2 != 0 && liquids[i,i2-1,i3].id == liquids[i,i2,i3].id && !liquids[i,i2,i3].sources[6] && liquids[i,i2,i3].level > liquids[i,i2-1,i3].level && liquids[i,i2,i3].level != 1){
						liquids[i,i2-1,i3].id = liquids[i,i2,i3].id;
						int temp = liquids[i,i2,i3].level-liquids[i,i2-1,i3].level;
						liquids[i,i2-1,i3].level += temp;
						liquids[i,i2,i3].level = Mathf.Max(liquids[i,i2,i3].level - temp,0);
						if(liquids[i,i2,i3].level == 0)liquids[i,i2,i3].id = -1;
						//liquids[i,i2-1,i3].sources[5] = true;
						liquids[i,i2,i3].drawn = 2;
						liquids[i,i2-1,i3].drawn = 2;
					}
				}
					
					if(i == 0 || i == 15 && liquids[i,i2,i3].level != 1) {
						foreach(Transform c in adjacentchunks){
							if(c != null && c.GetComponent<chunk>().xyz.x-xyz.x == (i == 0?-1:1)){
								if(c.GetComponent<chunk>().liquids[(i == 0?15:0),i2,i3].id == -1 && c.GetComponent<chunk>().blocks[(i == 0?15:0),i2,i3].id == -1){
									liquid templiquid = new liquid();
									templiquid.id = liquids[i,i2,i3].id;
									templiquid.level = Mathf.Max(1,(int)Mathf.Ceil(liquids[i,i2,i3].level/2.0f));
									liquids[i,i2,i3].level = Mathf.Max(1,(int)Mathf.Floor(liquids[i,i2,i3].level/2.0f));
									templiquid.inf = 1;
									templiquid.buildliquid();
									c.GetComponent<chunk>().liquids[(i == 0?15:0),i2,i3] = templiquid;
									c.GetComponent<chunk>().tickon = true;
									liquids[i,i2,i3].drawn = 2;
									c.GetComponent<chunk>().liquids[(i == 0?15:0),i2,i3].drawn = 2;
								}
							}
						}
					}
					if(i2 == 0 || i2 == 15 && liquids[i,i2,i3].level != 1) {
						foreach(Transform c in adjacentchunks){
							if(c != null && c.GetComponent<chunk>().xyz.y-xyz.y == (i2 == 0?-1:1)){
								if(c.GetComponent<chunk>().liquids[i,(i2 == 0?15:0),i3].id == -1 && c.GetComponent<chunk>().blocks[i,(i2 == 0?15:0),i3].id == -1){
									liquid templiquid = new liquid();
									templiquid.id = liquids[i,i2,i3].id;
									templiquid.level = Mathf.Max(1,(int)Mathf.Ceil(liquids[i,i2,i3].level/2.0f));
									liquids[i,i2,i3].level = Mathf.Max(1,(int)Mathf.Floor(liquids[i,i2,i3].level/2.0f));
									templiquid.inf = 1;
									templiquid.buildliquid();
									c.GetComponent<chunk>().liquids[i,(i2 == 0?15:0),i3] = templiquid;
									c.GetComponent<chunk>().tickon = true;
									liquids[i,i2,i3].drawn = 2;
									liquids[i,(i2 == 0?15:0),i3].drawn = 2;
								}
							}
						}
					}
					if(i3 == 0 || i3 == 15 && liquids[i,i2,i3].level != 1) {
						foreach(Transform c in adjacentchunks){
							if(c != null  && c.GetComponent<chunk>().xyz.z-xyz.z == (i3 == 0?-1:1)){
								if(c.GetComponent<chunk>().liquids[i,i2,(i3 == 0?15:0)].id == -1 && c.GetComponent<chunk>().blocks[i,i2,(i3 == 0?15:0)].id == -1){
									liquid templiquid = new liquid();
									templiquid.id = liquids[i,i2,i3].id;
									templiquid.level = Mathf.Max(1,(int)Mathf.Ceil(liquids[i,i2,i3].level/2.0f));
									liquids[i,i2,i3].level = Mathf.Max(1,(int)Mathf.Floor(liquids[i,i2,i3].level/2.0f));
									templiquid.inf = 1;
									templiquid.buildliquid();
									c.GetComponent<chunk>().liquids[i,i2,(i3 == 0?15:0)] = templiquid;
									c.GetComponent<chunk>().tickon = true;
									liquids[i,i2,i3].drawn = 2;
									liquids[i,i2,(i3 == 0?15:0)].drawn = 2;
								}
							}
						}
					}
				
			}
			//if(liquids[i,i2,i3].level == 1)liquids[i,i2,i3].settled = true;
			if(liquids[i,i2,i3].inf == 2)liquids[i,i2,i3].level = 16;
		}
	}
	
	void populate(){
		for(int i = 0; i <= 15; i++){ //x
			for(int i2 = 0; i2 <= 15; i2++){ //y 
				for(int i3 = 0; i3 <= 15; i3++){ //z
					block tempblock = new block();
					tempblock.id = 0;
					if(i != 0 && i != 15 && i2 != 0 && i2 != 15 && i3 != 0 && i3 != 15)tempblock.id = 1;
					if((i == 9 || i == 5) && (i3 == 5 || i3 == 9) && i2 != 15 || i2 == 1 || i2 == 0 && (i3 == 4 || i == 11))tempblock.id = 2;
					if((i == 6 || i == 7 || i == 8 || i3 == 6 || i3 == 7 || i3 == 8) && (i2 <= 14 &&  i2 >= 1) || i2 == 15) tempblock.id = -1;
					//if((i == 7 || i == 8) && (i3 == 7 || i3 == 8) && (i2 == 15 || i2 == 14))tempblock.id = -1;
					//if(i2 == 15 || i2 == 14 && i != 0 && i != 15 && i3 != 0 && i3 != 15) tempblock.id = -1;
					tempblock.buildblock();
					blocks[i,i2,i3] = tempblock;
					
					//liquids
					liquid templiquid = new liquid();
					templiquid.id = -1;
					if(i == 4 && i2 == 15 && i3 == 4 && test){
						templiquid.id = 0;
						templiquid.level = 16;
						templiquid.inf = 2;
					}
					templiquid.buildliquid();
					liquids[i,i2,i3] = templiquid;
				}
			}
		}
	}
	
	Transform findadj(Vector3 v){
		foreach(Transform c in adjacentchunks){
			if(c != null){
				if(c.GetComponent<chunk>().xyz-xyz == v) return c;
			}
		}
		return null;
	}
	
	public void drawquadfirst(){
		StartCoroutine("drawquadfirst2");
	}
	
	public IEnumerator drawquadfirst2(){
		framessinceupdate = 0;
		isgreedy = false;
		foreach(Transform t in quads){
			Destroy(t.gameObject);
		}
		quads.Clear();
		foreach(Transform t in hbxs){
			Destroy(t.gameObject);
		}
		hbxs.Clear();
		for(int i = 0; i <= 15; i++){ //x
			for(int i2 = 0; i2 <= 15; i2++){ //y 
				for(int i3 = 0; i3 <= 15; i3++){ //z
					//blocks
					if(blocks[i,i2,i3].id != -1){
							Transform clone2 = Instantiate(hbx);
							clone2.parent = this.transform;
							clone2.localPosition = new Vector3(i,i2,i3);
							hbxs.Add(clone2);
						Transform tempadj = findadj(new Vector3(-1,0,0));
						if((i == 0 && (tempadj == null || tempadj.GetComponent<chunk>().blocks[15,i2,i3].transparent && (tempadj.GetComponent<chunk>().blocks[15,i2,i3].id == -1 || !blocks[i,i2,i3].transparent))) || i != 0 && blocks[i-1,i2,i3].transparent && (blocks[i-1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//left
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i-0.5f,i2,i3);
							clone.rotation = Quaternion.Euler(0,90,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						tempadj = findadj(new Vector3(1,0,0));
						if((i == 15 && (findadj(new Vector3(1,0,0)) == null || tempadj.GetComponent<chunk>().blocks[0,i2,i3].transparent && (tempadj.GetComponent<chunk>().blocks[0,i2,i3].id == -1 || !blocks[i,i2,i3].transparent))) || i != 15 && blocks[i+1,i2,i3].transparent && (blocks[i+1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//right
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i+0.5f,i2,i3);
							clone.rotation = Quaternion.Euler(0,-90,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						tempadj = findadj(new Vector3(0,-1,0));
						if((i2 == 0 && (findadj(new Vector3(0,-1,0)) == null || tempadj.GetComponent<chunk>().blocks[i,15,i3].transparent && (tempadj.GetComponent<chunk>().blocks[i,15,i3].id == -1 || !blocks[i,i2,i3].transparent))) || i2 != 0 && blocks[i,i2-1,i3].transparent && (blocks[i,i2-1,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//top
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2-0.5f,i3);
							clone.rotation = Quaternion.Euler(-90,0,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						tempadj = findadj(new Vector3(0,1,0));
						if((i2 == 15 && (findadj(new Vector3(0,1,0)) == null || tempadj.GetComponent<chunk>().blocks[i,0,i3].transparent && (tempadj.GetComponent<chunk>().blocks[i,0,i3].id == -1 || !blocks[i,i2,i3].transparent))) || i2 != 15 && blocks[i,i2+1,i3].transparent && (blocks[i,i2+1,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//bottom
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2+0.5f,i3);
							clone.rotation = Quaternion.Euler(90,0,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						tempadj = findadj(new Vector3(0,0,-1));
						if((i3 == 0 && (findadj(new Vector3(0,0,-1)) == null || tempadj.GetComponent<chunk>().blocks[i,i2,15].transparent && (tempadj.GetComponent<chunk>().blocks[i,i2,15].id == -1 || !blocks[i,i2,i3].transparent))) || i3 != 0 && blocks[i,i2,i3-1].transparent && (blocks[i,i2,i3-1].id == -1 || !blocks[i,i2,i3].transparent)){
							//front
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2,i3-0.5f);
							clone.rotation = Quaternion.Euler(0,0,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						tempadj = findadj(new Vector3(0,0,1));
						if((i3 == 15 && (findadj(new Vector3(0,0,1)) == null || tempadj.GetComponent<chunk>().blocks[i,i2,0].transparent && (tempadj.GetComponent<chunk>().blocks[i,i2,0].id == -1 || !blocks[i,i2,i3].transparent))) || i3 != 15 && blocks[i,i2,i3+1].transparent&& (blocks[i,i2,i3+1].id == -1 || !blocks[i,i2,i3].transparent)){
							//back
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2,i3+0.5f);
							clone.rotation = Quaternion.Euler(0,180,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist1[blocks[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							quads.Add(clone);
						}
						
						
						
						//change quads here
					}
					
					
						//liquids
					if(liquids[i,i2,i3].id != -1){
							Transform clone2 = Instantiate(hbx2);
							clone2.parent = this.transform;
							clone2.localPosition = new Vector3(i,i2-0.5f+(liquids[i,i2,i3].level/32.0f),i3);
							clone2.localScale = new Vector3(1,(liquids[i,i2,i3].level/16.0f),1);
							clone2.gameObject.tag = "liquid";
							hbxs.Add(clone2);
						/*if(i-1 == -1 || blocks[i-1,i2,i3].transparent && (blocks[i-1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//left
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i-0.5f,i2,i3);
							clone.rotation = Quaternion.Euler(0,90,0);
							clone.localScale = new Vector3(1,1,1);
							clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
							quads.Add(clone);
						}
						if(i+1 == 16 || blocks[i+1,i2,i3].transparent && (blocks[i+1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//right
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i+0.5f,i2,i3);
							clone.rotation = Quaternion.Euler(0,-90,0);
							clone.localScale = new Vector3(1,1,1);
							clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
							quads.Add(clone);
						}
						if(i2-1 == -1 || blocks[i,i2-1,i3].transparent && (blocks[i,i2-1,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//top
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2-0.5f,i3);
							clone.rotation = Quaternion.Euler(-90,0,0);
							clone.localScale = new Vector3(1,1,1);
							clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
							quads.Add(clone);
						}*/
						if(i2+1 == 16 || blocks[i,i2+1,i3].transparent && (blocks[i,i2+1,i3].id == -1 || !blocks[i,i2,i3].transparent)){
							//bottom
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2-0.5f+(liquids[i,i2,i3].level/16.0f),i3);
							clone.rotation = Quaternion.Euler(90,0,0);
							clone.localScale = new Vector3(1,1,1);
							Material mat = new Material(master.mat2);
							mat.mainTexture = master.matslist2[liquids[i,i2,i3].id];
							StartCoroutine(applytex(clone,mat));
							clone.gameObject.tag = "liquid";
							quads.Add(clone);
						}
						/*if(i3-1 == -1 || blocks[i,i2,i3-1].transparent && (blocks[i,i2,i3-1].id == -1 || !blocks[i,i2,i3].transparent)){
							//front
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2,i3-0.5f);
							clone.rotation = Quaternion.Euler(0,0,0);
							clone.localScale = new Vector3(1,1,1);
							clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
							quads.Add(clone);
						}
						if(i3+1 == 16 || blocks[i,i2,i3+1].transparent&& (blocks[i,i2,i3+1].id == -1 || !blocks[i,i2,i3].transparent)){
							//back
							Transform clone = Instantiate(quad);
							clone.parent = this.transform;
							clone.localPosition = new Vector3(i,i2,i3+0.5f);
							clone.rotation = Quaternion.Euler(0,180,0);
							clone.localScale = new Vector3(1,1,1);
							clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
							quads.Add(clone);
						}*/
						liquids[i,i2,i3].drawn = 0;
					}
				}	
			}
		}
		yield return new WaitForEndOfFrame();
		loaded = true;
	}
	
	bool updatingliquids = false;
	
	void updateliquids(){
		if(!updatingliquids){
			updatingliquids = true;
		
			/*for(int i = 0; i < quads.Count; i++){ 
				if(quads[i].tag == "liquid"){
					Destroy(quads[i].gameObject);
					quads.RemoveAt(i);
					i--;
				}
			}
			for(int i = 0; i < hbxs.Count; i++){
				if(hbxs[i].tag == "liquid"){
					Destroy(hbxs[i].gameObject);
					hbxs.RemoveAt(i);
					i--;
				}
			}*/
			for(int i = 0; i <= 15; i++){ //x
				for(int i2 = 0; i2 <= 15; i2++){ //y 
					for(int i3 = 0; i3 <= 15; i3++){ //z
						//if(i == 7 && i2 == 14 &&  (i3 == 7 || i3 == 8))Debug.Log(i+" "+i2+" "+i3+" "+liquids[i,i2,i3].level);
						//if(liquids[i,i2,i3].level < 1 && liquids[i,i2,i3].id != -1)liquids[i,i2,i3].level = 1;
						//liquids
						if(liquids[i,i2,i3].drawn == 2){
							for(int i00 = 0; i00 < quads.Count; i00++){ 
								if(quads[i00].tag == "liquid"){
									if(Vector3.Distance(quads[i00].localPosition, new Vector3(i,i2,i3)) < 0.51f){
										Destroy(quads[i00].gameObject);
										quads.RemoveAt(i00);
										i00--;
									}
								}
							}
							for(int i00 = 0; i00 < hbxs.Count; i00++){
								if(hbxs[i00].tag == "liquid"){
									if(Vector3.Distance(hbxs[i00].localPosition, new Vector3(i,i2,i3)) < 0.51f){
										Destroy(hbxs[i00].gameObject);
										hbxs.RemoveAt(i00);
										i00--;
									}
								}
							}
						}
						if(liquids[i,i2,i3].id != -1 && liquids[i,i2,i3].drawn != 1){
								
							
								Transform clone2 = Instantiate(hbx2);
								clone2.parent = this.transform;
								clone2.localPosition = new Vector3(i,i2-0.5f+(liquids[i,i2,i3].level/32.0f),i3);
								clone2.localScale = new Vector3(1,(liquids[i,i2,i3].level/16.0f),1);
								clone2.gameObject.tag = "liquid";
								hbxs.Add(clone2);
							/*if(i-1 == -1 || blocks[i-1,i2,i3].transparent && (blocks[i-1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
								//left
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i-0.5f,i2,i3);
								clone.rotation = Quaternion.Euler(0,90,0);
								clone.localScale = new Vector3(1,1,1);
								clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
								quads.Add(clone);
							}
							if(i+1 == 16 || blocks[i+1,i2,i3].transparent && (blocks[i+1,i2,i3].id == -1 || !blocks[i,i2,i3].transparent)){
								//right
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i+0.5f,i2,i3);
								clone.rotation = Quaternion.Euler(0,-90,0);
								clone.localScale = new Vector3(1,1,1);
								clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
								quads.Add(clone);
							}
							if(i2-1 == -1 || blocks[i,i2-1,i3].transparent && (blocks[i,i2-1,i3].id == -1 || !blocks[i,i2,i3].transparent)){
								//top
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i,i2-0.5f,i3);
								clone.rotation = Quaternion.Euler(-90,0,0);
								clone.localScale = new Vector3(1,1,1);
								clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
								quads.Add(clone);
							}*/
							if(i2+1 == 16 || (blocks[i,i2+1,i3].transparent && (blocks[i,i2+1,i3].id == -1 || !blocks[i,i2,i3].transparent)) && liquids[i,i2+1,i3].id == -1 && liquids[i,i2,i3].id != -1){
								//bottom
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i,i2-0.5f+(liquids[i,i2,i3].level/16.0f),i3);
								clone.rotation = Quaternion.Euler(90,0,0);
								clone.localScale = new Vector3(1,1,1);
								Material mat = new Material(master.mat2);
								mat.mainTexture = master.matslist2[liquids[i,i2,i3].id];
								StartCoroutine(applytex(clone,mat));
								clone.gameObject.tag = "liquid";
								quads.Add(clone);
							}
							/*if(i3-1 == -1 || blocks[i,i2,i3-1].transparent && (blocks[i,i2,i3-1].id == -1 || !blocks[i,i2,i3].transparent)){
								//front
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i,i2,i3-0.5f);
								clone.rotation = Quaternion.Euler(0,0,0);
								clone.localScale = new Vector3(1,1,1);
								clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
								quads.Add(clone);
							}
							if(i3+1 == 16 || blocks[i,i2,i3+1].transparent&& (blocks[i,i2,i3+1].id == -1 || !blocks[i,i2,i3].transparent)){
								//back
								Transform clone = Instantiate(quad);
								clone.parent = this.transform;
								clone.localPosition = new Vector3(i,i2,i3+0.5f);
								clone.rotation = Quaternion.Euler(0,180,0);
								clone.localScale = new Vector3(1,1,1);
								clone.gameObject.GetComponent<Renderer>().material = master.matslist1[blocks[i,i2,i3].id];
								quads.Add(clone);
							}*/
							
							liquids[i,i2,i3].drawn = 1;
						}
					}	
				}
			}
			for(int i00 = 0; i00 < quads.Count; i00++){ 
				if(quads[i00].tag == "liquid"){
					for(int i01 = 0; i01 < quads.Count; i01++){ 
						if(quads[i01].tag == "liquid" && i01 != i00){
							if(Vector3.Distance(quads[i00].localPosition, quads[i01].localPosition) < 0.11f){
								Destroy(quads[i01].gameObject);
								quads.RemoveAt(i01);
								i01--;
							}
						}
					}
				}
			}
			updatingliquids = false;
		}
	}
	
	public Vector3 translatehit(Vector3 v){
		Vector3 tempvec = v;
		v.x -= xyz.x*16;
		v.y -= xyz.y*16;
		v.z -= xyz.z*16;
		return new Vector3(Mathf.Round(v.x),Mathf.Round(v.y),Mathf.Round(v.z));
	}
	
	public void changeblock(int i, int i2, int i3, int id){ //fix adding blocks not adding hitboxes
		//Debug.Log("yes");
		framessinceupdate = 0;
		if(id == -1)StartCoroutine(scheduledroploot(new Vector3(i, i2, i3), blocks[i,i2,i3].id, 0.3f));
		blocks[i,i2,i3].id = id;
		blocks[i,i2,i3].buildblock();
		tickon = true;
		if(i!= 0){
			liquids[i-1,i2,i3].settled = false;
		} else {
			Transform tempadj = findadj(new Vector3(-1,0,0));
			if(tempadj != null){
				tempadj.GetComponent<chunk>().liquids[15,i2,i3].settled = false;
				tempadj.GetComponent<chunk>().tickon = true;
			}
		}
		if(i!= 15){
			liquids[i+1,i2,i3].settled = false;
		} else {
			Transform tempadj = findadj(new Vector3(1,0,0));
			if(tempadj != null){
				tempadj.GetComponent<chunk>().liquids[0,i2,i3].settled = false;
				tempadj.GetComponent<chunk>().tickon = true;
			}
		}
		if(i2!= 0){
			liquids[i,i2-1,i3].settled = false;
		} else {
			Transform tempadj = findadj(new Vector3(0,-1,0));
			if(tempadj != null){
				tempadj.GetComponent<chunk>().liquids[i,i2-1,i3].settled = false;
				tempadj.GetComponent<chunk>().tickon = true;
			}
		}
		if(i3!= 0){
			liquids[i,i2,i3-1].settled = false;
		} else {
			Transform tempadj = findadj(new Vector3(0,0,-1));
			if(tempadj != null){
				tempadj.GetComponent<chunk>().liquids[i,i2,15].settled = false;
				tempadj.GetComponent<chunk>().tickon = true;
			}
		}
		if(i3!= 15){
			liquids[i,i2,i3+1].settled = false;
		} else {
			Transform tempadj = findadj(new Vector3(0,0,1));
			if(tempadj != null){
				tempadj.GetComponent<chunk>().liquids[i,i2,0].settled = false;
				tempadj.GetComponent<chunk>().tickon = true;
			}
		}
		if(i == 0 && findadj(new Vector3(-1,0,0)) != null && findadj(new Vector3(-1,0,0)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(-1,0,0)).GetComponent<chunk>().converttogreedymesh(true);
		if(i == 15 && findadj(new Vector3(1,0,0)) && findadj(new Vector3(1,0,0)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(1,0,0)).GetComponent<chunk>().converttogreedymesh(true);
		if(i2 == 0 && findadj(new Vector3(0,-1,0)) && findadj(new Vector3(0,-1,0)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(0,-1,0)).GetComponent<chunk>().converttogreedymesh(true);
		if(i2 == 15 && findadj(new Vector3(0,1,0)) && findadj(new Vector3(0,1,0)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(0,1,0)).GetComponent<chunk>().converttogreedymesh(true);
		if(i3 == 0 && findadj(new Vector3(0,0,-1)) && findadj(new Vector3(0,0,-1)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(0,0,-1)).GetComponent<chunk>().converttogreedymesh(true);
		if(i3 == 15 && findadj(new Vector3(0,0,1)) && findadj(new Vector3(0,0,1)).GetComponent<chunk>().blocks[15,i2,i3].id != -1)findadj(new Vector3(0,0,1)).GetComponent<chunk>().converttogreedymesh(true);
		converttogreedymesh(true);
		/*if(isgreedy)drawquadfirst();
		
			
			foreach(Transform c in transform){
				if(Vector3.Distance(c.localPosition, new Vector3(i,i2,i3)) < 0.51f){
					quads.Remove(c);
					hbxs.Remove(c);
					Destroy(c.gameObject);
				}
			}
			if(i != 0 && blocks[i-1,i2,i3].id != -1){
				//right
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i-0.5f,i2,i3);
				clone.rotation = Quaternion.Euler(0,-90,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i-1,i2,i3].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
			}
			if(i != 15 && blocks[i+1,i2,i3].id != -1){
				
				//left
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i+0.5f,i2,i3);
				clone.rotation = Quaternion.Euler(0,90,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i+1,i2,i3].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
			}
			if(i2 != 0 && blocks[i,i2-1,i3].id != -1){
				//bottom
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i,i2-0.5f,i3);
				clone.rotation = Quaternion.Euler(90,0,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i,i2-1,i3].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
			}
			if(i2 != 15 && blocks[i,i2+1,i3].id != -1){
				//top
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i,i2+0.5f,i3);
				clone.rotation = Quaternion.Euler(-90,0,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i,i2+1,i3].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
				
			}
			if(i3 != 0 && blocks[i,i2,i3-1].id != -1){
				//back
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i,i2,i3-0.5f);
				clone.rotation = Quaternion.Euler(0,180,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i,i2,i3-1].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
			}
			if(i3 != 15 && blocks[i,i2,i3+1].id != -1){
				
				//front
				Transform clone = Instantiate(quad);
				clone.parent = this.transform;
				clone.localPosition = new Vector3(i,i2,i3+0.5f);
				clone.rotation = Quaternion.Euler(0,0,0);
				clone.localScale = new Vector3(1,1,1);
				Material mat = new Material(master.mat1);
				mat.mainTexture = master.matslist1[blocks[i,i2,i3+1].id];
				StartCoroutine(applytex(clone,mat));
				quads.Add(clone);
			}
		*/
	}
	
}