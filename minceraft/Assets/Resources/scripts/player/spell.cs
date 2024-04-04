using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class spell : MonoBehaviour {
	public int id = -1;
	public List<Transform> custtransforms = new List<Transform>();
	public List<float> custfloats = new List<float>();
	public List<Vector3> custvectors = new List<Vector3>();
	public List<bool> custbools = new List<bool>();
	public int mode = 0;
	RaycastHit hit;
	
	public void makespell(){
		//telekinisis
		if(id == 0){
			GetComponent<LineRenderer>().positionCount = 2;
			custtransforms.Add(Instantiate(master.spellidobj[0]));
			custtransforms[0].gameObject.SetActive(false);
		}
		// fly
		if(id == 1){
			//add particles?
		}
		// vine grapple
		if(id == 2){
			GetComponent<LineRenderer>().positionCount = 2;
			custtransforms.Add(Instantiate(master.spellidobj[1]));
			custtransforms[0].gameObject.SetActive(false);
			custbools.Add(false);
		}
	}
	
	public void getmenu(){
		if(id == 0){
			if(mode == 0 || mode == 2){
				LineRenderer temp = GetComponent<LineRenderer>();
				temp.SetPosition(0, transform.position);
				custtransforms[0].gameObject.SetActive(true);
				custtransforms[0].position = transform.position + (transform.forward * 32.5f);
				custtransforms[0].LookAt(transform.position, Vector3.up);
				custtransforms[0].GetComponent<ParticleSystem>().Play();
				if(Physics.Raycast(transform.position, transform.forward, out hit, 32.5f, master.mask1 + master.mask2)){
					custtransforms[0].GetComponent<custparts>().custvec[0] = hit.point;
					custtransforms[0].position = hit.point - transform.forward ;
					custtransforms[0].LookAt(transform.position, Vector3.up);
					temp.SetPosition(1, hit.point);
					if(hit.transform != null && hit.transform.GetComponent<item>() != null){
						if(Vector3.Distance(hit.point, transform.position) > 0.5f){
							hit.transform.GetComponent<Rigidbody>().velocity = -transform.forward * 5;
						}else {
							GetComponent<ViveControllerVR>().GrabObjectFromDist(hit.transform);
						}				
					}
				} else {
					temp.SetPosition(1, transform.position);
				}
				mode = 2;
			}
		}
	}
	
	public void getmenuup(){
		if(id == 0){
			LineRenderer temp = GetComponent<LineRenderer>();
			temp.SetPosition(0, transform.position);
			custtransforms[0].gameObject.SetActive(false);
			temp.SetPosition(1, transform.position);
			GetComponent<ViveControllerVR>().ReleaseObject();
			mode = 0;
		}
	}
	
	public void getmenudown(){
		if(id == 0){
			if(mode == 1){
				if(Physics.Raycast(transform.position, transform.forward, out hit, 32.5f, master.mask1 + master.mask2)){
					
					if(hit.transform != null && hit.transform.GetComponent<item>() != null){
						GetComponent<ViveControllerVR>().ReleaseObject();
						hit.transform.GetComponent<Rigidbody>().velocity = transform.forward * 32;				
					}
					mode = 3;
				}
			} else if(mode == 3){
				mode = 0;
			}
		}
	}
	
	public void getpinch(){
		if(id == 0){
			if(mode == 0 || mode == 1){
				LineRenderer temp = GetComponent<LineRenderer>();
				temp.SetPosition(0, transform.position);
				custtransforms[0].gameObject.SetActive(true);
				custtransforms[0].position = transform.position;
				custtransforms[0].LookAt(transform.position + transform.forward, Vector3.up);
				custtransforms[0].GetComponent<ParticleSystem>().Play();
				if(Physics.Raycast(transform.position, transform.forward, out hit, 32.5f, master.mask1 + master.mask2)){
					custtransforms[0].GetComponent<custparts>().custvec[0] = hit.point;
					temp.SetPosition(1, hit.point);
					custtransforms[0].LookAt(hit.point, Vector3.up);
					if(hit.transform != null && hit.transform.GetComponent<item>() == null && hit.transform.parent.GetComponent<chunk>() != null){
						Vector3 tempvec = hit.transform.parent.GetComponent<chunk>().translatehit(hit.point+(hit.normal*-0.1f));
						//Debug.Log(hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog);
						hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog++;
						if(hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog >= 100){
							hit.transform.parent.GetComponent<chunk>().changeblock((int)tempvec.x,(int)tempvec.y,(int)tempvec.z, -1);
							hit.transform.parent.GetComponent<chunk>().blocks[(int)tempvec.x,(int)tempvec.y,(int)tempvec.z].breakprog = 0;
						}
						
					} else if(hit.transform != null && hit.transform.GetComponent<item>() != null){
						//hit.transform.GetComponent<Rigidbody>().velocity = transform.forward
						GetComponent<ViveControllerVR>().GrabObjectFromDist(hit.transform);
					}
				} else {
					temp.SetPosition(1, transform.position);
				}
				mode = 1;
			}
		} else if (id == 1){
			master.play1.GetComponent<player>().movetype = 2;
		} else if(id == 2){
			if(custfloats.Count != 0 ){
				if(!custbools[0]){
					custfloats[0] += Time.deltaTime *5;
					custtransforms[0].position = Vector3.Lerp(transform.position, custvectors[0], custfloats[0] / (Vector3.Distance(transform.position, custvectors[0])/5));
					if(custfloats[0] / (Vector3.Distance(transform.position, custvectors[0])/5) >= 1){
						custbools[0] = true;
						GetComponent<ViveControllerVR>().hookto = true;
						custvectors.Add(transform.position);
					}
				} else {
					
					GetComponent<ViveControllerVR>().movyover = Vector3.zero;
					//transform.position = Vector3.Lerp(varvec[0], varvec[1], varfloat[0] / (Vector3.Distance(varvec[1], varvec[0])/5));
					custtransforms[0].position = custvectors[0];
					if(Vector3.Distance(custvectors[0],master.play1.position) > 1.0f && master.play1.GetComponent<player>().movetype == 3){
						GetComponent<ViveControllerVR>().movyover = (custvectors[0] - master.play1.position).normalized * ((Time.deltaTime * 50) / (Vector3.Distance(custvectors[1], custvectors[0])/5));
					}
					if(transform.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().hookto){
						custbools[0] = false;
						getpinchup();//par.GetComponent<Hand>().otherHand.GetComponent<ViveControllerVR>().objectInHand.GetComponent<item>().effects.Add(new int[3]{0,1,10});
					}
				}
			}
			GetComponent<LineRenderer>().SetPosition(0, transform.position);
			GetComponent<LineRenderer>().SetPosition(1, custtransforms[0].position);
				
		}
		
	}
	
	public void getpinchup(){
		if(id == 0){
			LineRenderer temp = GetComponent<LineRenderer>();
			temp.SetPosition(0, transform.position);
			custtransforms[0].gameObject.SetActive(false);
			temp.SetPosition(1, transform.position);
			GetComponent<ViveControllerVR>().ReleaseObject();
			mode = 0;
		} else if (id == 1){
			master.play1.GetComponent<player>().movetype = 0;
		} else if (id == 2){
			GetComponent<LineRenderer>().SetPosition(1, transform.position);
			custvectors.Clear();
			custfloats.Clear();
			custbools[0] = false;
			GetComponent<ViveControllerVR>().hookto = false;
		}
	}
	
	public void getpinchdown(){
		if(id == 0){
			if(mode == 3)mode = 0;
		} else if (id == 2){
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, 32.5f, master.mask1 + master.mask3)){
				custvectors.Add(hit.point);
				custfloats.Add(0.0f);
			}
		}
	}
	
	public void destroyspell(){
		GetComponent<LineRenderer>().positionCount = 0;
		foreach(Transform t in custtransforms){
			Destroy(t.gameObject, 0.01f);
		}
		Destroy(this, 0.01f);
	}
	
}
