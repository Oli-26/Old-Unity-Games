using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BalistaTower : Defense {
    
    
    public GameObject body;

    
    public GameObject shot;
    public GameObject shotPrefab;
    
    
    
    int hitCount = 1;

	// Use this for initialization
	void Start () {
		healthPoints = 30;
        coolDown = 2.5f;
        cd = 2.5f;
        
        range = 1.2f;
        baseDamage = 3f;
        healthPoints = 30f;
        maxHealthPoints = 30f;
        popValue = 2;
        type = "Balista";
	}
	
	// Update is called once per frame
	void Update () {
        updateHealthText();
        if(targetSet && finishedBuilding){
            Aim();
        }
        if(targetSet == false && finishedBuilding){
            GenerateTarget();
        }
	}
    
    void Aim(){
        try {
       
            if(direction == "left"){
                if(target.transform.position.x < gameObject.transform.position.x){
                    
                    float x = target.transform.position.x - gameObject.transform.position.x;
                    float y = target.transform.position.y - gameObject.transform.position.y;
                    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                    body.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-180f));
                    
                    if(cd > 0){
                        cd-=Time.deltaTime;
                    }else{
                        Shoot();
                        cd = generateCoolDown();
                    }
                }else{
                    targetSet = false;
                    
                }
            }
            if(direction == "right"){
                if(target.transform.position.x > gameObject.transform.position.x){
                    float x = -(target.transform.position.x - gameObject.transform.position.x);
                    float y = target.transform.position.y - gameObject.transform.position.y;
                    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                    body.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, angle-180f));
                    
                    if(cd > 0){
                        cd-=Time.deltaTime;
                    }else{
                        Shoot();
                        cd = generateCoolDown();
                    }
                }else{
                    targetSet = false;
                    
                }
                
            
            }
         
        }
        catch (Exception e) {
            targetSet = false;
        }  
    }
    

   
    
    public void Shoot(){
        shot = Instantiate(shotPrefab, body.transform.position + new Vector3(0f, 0.01f, 0f), body.transform.rotation);

        shot.GetComponent<BallistaShot>().setDamage(generateDamage());
        
        
        shot.GetComponent<BallistaShot>().setHits(hitCount);
    }
    
   

}
