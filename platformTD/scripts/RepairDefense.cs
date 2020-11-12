using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RepairDefense : Defense {
    public GameObject healthTextPrefab;
    public GameObject repairTarget;
    bool repairTargetSet = false;
	// Use this for initialization
	void Start () {
        coolDown = 5f;
        cd = 5f;
        
        range = 0.5f;
        baseDamage = 2f;
        healthPoints = 10f;
        maxHealthPoints = 10f;
        popValue = 2;
        type = "repair tower";
	}
	
	// Update is called once per frame
	void Update () {
        updateHealthText();
		if(cd <= 0){
            Repair();
            cd = coolDown;
        }else{
            cd -= Time.deltaTime;
        }
	}
    
    void Repair(){
        if(repairTargetSet){
            try{
                float repairValue = generateDamage() + repairTarget.GetComponent<Defense>().generateHealth()*(0.05f+0.01f*(level-1));
                GameObject g = Instantiate(healthTextPrefab, transform.position+new Vector3(0f,0.05f,0f), Quaternion.identity);
                g.GetComponent<TextMesh>().text = "+" + (Mathf.Round(100f*repairValue)/100f);
                g.GetComponent<TextMesh>().color = new Color(0.5f,1f,0.5f);
                repairTarget.GetComponent<Defense>().Repair(repairValue);
            }catch (Exception e) {
                repairTargetSet = false;
            } 
        }else{
            GenerateRepairTarget();
        }
    }
    
    void GenerateRepairTarget(){
       
        GameObject[] targets = GameObject.FindGameObjectsWithTag("AgroDefense");
        foreach(GameObject t in targets){
            if(Vector3.Distance(t.transform.position, transform.position) < range){
                if(t.GetComponent<Defense>().isMaxHealth() || t == gameObject){
                    
                    
                }else{
                    repairTarget = t;
                    repairTargetSet = true;
                    return;
                }
            }
        }
    
    }
    
    
}
