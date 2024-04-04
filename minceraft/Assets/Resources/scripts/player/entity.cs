using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour {
	public stat[] stats = new stat[11]; //curhp/ maxhp, curmana/ maxmana, curstamina/ max stamina, str,con,dex,int,wis,cha,lck, level/xp 
	public int typeid = 0; // 0 = player
	public clss[] clsses = new clss[3]; //novice, base, prestige
	public item[] inventory = new item[48]; //inventory
	
	// Use this for initialization
	void Start () {
		if(typeid == 0){ //load player stats
			//TEMP VALUES
			stats[10].stat1 = 1; //level
			stats[10].stat1mod = 0; //xp
			
			stats[3].stat1 = 10; //str
			stats[3].basestat = true; 
			
			stats[4].stat1 = 10; //con
			stats[4].basestat = true; 
			
			stats[5].stat1 = 10; //dex
			stats[5].basestat = true; 
			
			stats[6].stat1 = 10; //int
			stats[6].basestat = true; 
			
			stats[7].stat1 = 10; //wis
			stats[7].basestat = true; 
			
			stats[8].stat1 = 10; //cha
			stats[8].basestat = true; 
			
			stats[9].stat1 = 10; //lck
			stats[9].basestat = true;

			stats[0].stat1mod = (stats[4].stat1mod * stats[10].stat1) + allclasshd(1); //max hp
			stats[0].stat1 = stats[0].stat1mod; //curhp
			
			stats[1].stat1mod = (stats[4].stat1mod * stats[10].stat1) + allclasshd(2); //max mana
			stats[1].stat1 = stats[1].stat1mod; //curmana
			
			stats[2].stat1mod = (stats[4].stat1mod * stats[10].stat1) + allclasshd(3); //max stamina
			stats[2].stat1 = stats[2].stat1mod; //curstamina
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	int allclasshd (int i){
		if(i == 1){ //hp
			return (clsses[0].hphd * Mathf.Min(12, stats[10].stat1 + 2)) + (clsses[1].hphd * Mathf.Min(0, stats[10].stat1 - 10)) + (clsses[2].hphd * Mathf.Min(0, stats[10].stat1 - 20));
		} else if(i == 2){ //mana
			return (clsses[0].manahd * Mathf.Min(12, stats[10].stat1 + 2)) + (clsses[1].manahd * Mathf.Min(0, stats[10].stat1 - 10)) + (clsses[2].manahd * Mathf.Min(0, stats[10].stat1 - 20));
		} else{ //stamina
			return (clsses[0].staminahd * Mathf.Min(12, stats[10].stat1 + 2)) + (clsses[1].staminahd * Mathf.Min(0, stats[10].stat1 - 10)) + (clsses[2].staminahd * Mathf.Min(0, stats[10].stat1 - 20));
		}
	}
	
	public class stat{
		public int stat1;
		public int stat1mod;
		public bool basestat;
		
		void Update () {
			if(basestat)stat1mod = stat1/5;
		}
	}
	
	public class clss{
		public int id;
		public int hphd;
		public int manahd;
		public int staminahd;
		public string name;
		
	}
}
