using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block {
	public int id; //-1 = air
	public bool tickon = false;
	public int ticknum = 1;
	public bool transparent;
	public int breakprog = 0;
	public bool canshape = false;
	
	public void dotick(int i){
		if(i == ticknum);//dostuff
	}
	
	public void buildblock(){
		transparent = false;
		if(id == 0){ //test 1
		} else if(id == 1) { //test 2
			//tickon = true;
			//ticknum = Random.Range(1,11);
		} else if(id == -1) { //air
			transparent = true;
		} else if(id == 2) { //test 3
			//transparent = true;
		}
	}
}
