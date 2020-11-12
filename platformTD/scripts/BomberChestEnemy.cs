using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BomberChestEnemy : Enemy {

	// Use this for initialization
	void Start () {
        
        attack_cooldown = 0f;
        attack_cd = 0f;
        range = 0.12f;
        damage = 25f;
        health = 25f;
        dropAmount = 4;
        speed = 2.5f;
		Heart = GameObject.FindWithTag("Heart");
        currentTarget = Heart;
        xpWorth = 20;
        dropAmount = 4;
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
            findNewTargetCoolDown = 0.8f;
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
                   Destroy(gameObject);
                   attack_cd = attack_cooldown;
               }
               
            }
        }catch (Exception e) {
            findTarget();
        }
        Movement();
	}
}
