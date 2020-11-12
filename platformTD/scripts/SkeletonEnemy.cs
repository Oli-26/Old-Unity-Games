using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkeletonEnemy : Enemy {
    public Sprite[] walkingAnimation;
    int animationFrame = 0;
    int timer = 0;
	// Use this for initialization
	void Start () {
        
        attack_cooldown = 2f;
        attack_cd = 2f;
        range = 0.2f;
        damage = 0.7f;
        health = 25f;
        dropAmount = 1;
		Heart = GameObject.FindWithTag("Heart");
        currentTarget = Heart;
        
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
                   currentTarget.GetComponent<Defense>().takeDamage(generateDamage());
                   attack_cd = attack_cooldown;
               }
               
            }
        }catch (Exception e) {
            findTarget();
        }
        Movement();
        
        if(timer > 10){
            animationFrame++;
            if(animationFrame > 3){
                animationFrame = 0;
            }
            GetComponent<SpriteRenderer>().sprite = walkingAnimation[animationFrame];
            timer = 0;
        }else{
            timer++;
        }            
	}
}
