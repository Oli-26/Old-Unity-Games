using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OrgeEnemy : Enemy {
	// Use this for initialization
	void Start () {
        
        attack_cooldown = 8f;
        attack_cd = 8f;
        range = 0.4f;
        damage = 25f;
        health = 190f;
        dropAmount = 4;
		Heart = GameObject.FindWithTag("Heart");
        currentTarget = Heart;
        xpWorth = 100;
        waveStatMultiplier(GameObject.FindWithTag("spawner").GetComponent<Spawner>().waveNumber);
        
        if(transform.position.x > currentTarget.transform.position.x){
            GetComponent<SpriteRenderer>().flipX = true;  
        }
        findTarget();
	}
	
	// Update is called once per frame
	void Update () {
        
        if(findNewTargetCoolDown < 0f){
            findTarget();
            findNewTargetCoolDown = 2f;
        }else{
            findNewTargetCoolDown -= Time.deltaTime;
        }
        updateHealthText();
		try{
            if(Vector3.Distance(transform.position, currentTarget.transform.position) < range){
               if(attack_cd > 0){
                   attack_cd -= Time.deltaTime;
               }else{
                   currentTarget.GetComponent<Defense>().takeDamage(generateDamage());
                   attack_cd = attack_cooldown;
               }
               
            }
        }catch (Exception e) {
            findTarget();
        }
        Movement();
	}
}