using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WarlockEnemy : Enemy {
    public GameObject magicBall;
    public GameObject soulShotPrefab;
	// Use this for initialization
	void Start () {
		attack_cooldown = 7f;
        attack_cd = 7f;
        range = 0.7f;
        damage = 8f;
        health = 10f;
        dropAmount = 2;
        
        waveStatMultiplier(GameObject.FindWithTag("spawner").GetComponent<Spawner>().waveNumber);
        
        Heart = GameObject.FindWithTag("Heart");
        currentTarget = Heart;
        
        
        
        if(transform.position.x > currentTarget.transform.position.x){
            GetComponent<SpriteRenderer>().flipX = true;  
        }
        findTarget();
	}
	
	// Update is called once per frame
	void Update () {
        
        if(findNewTargetCoolDown < 0f){
            findTarget();
            findNewTargetCoolDown = 1f;
        }else{
            findNewTargetCoolDown -= Time.deltaTime;
        }
        updateHealthText();
		 try{
            if(Vector3.Distance(transform.position, currentTarget.transform.position) < range){
               if(attack_cd > 0){
                   attack_cd -= Time.deltaTime;
               }else{
                   GameObject g = Instantiate(soulShotPrefab, transform.position, Quaternion.identity);
                   g.GetComponent<SoulShot>().setTarget(currentTarget);
                   g.GetComponent<SoulShot>().damage = damage;
                   //currentTarget.GetComponent<Defense>().takeDamage(generateDamage());
                   attack_cd = attack_cooldown;
               }
               
            }
        }catch (Exception e) {
            //Debug.Log("Target not found: Re-targeting");
            findTarget();
        }
        Movement();
	}
    

}
