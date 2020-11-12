using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Enemy : MonoBehaviour {
    public GameObject currentTarget;
    
    public GameObject Heart;
    
    public GameObject healthText;
    public GameObject damageTagPrefab;
    public GameObject coinPrefab;
    protected float health = 20f;
    public float damage = 1f; 
    protected float speed = 1.5f;
    public float range = 0.4f;
    protected float attack_cooldown = 1f;
    protected float attack_cd = 1f;
    protected float findNewTargetCoolDown = 5f;
    protected int dropAmount = 1;
    public int xpWorth = 10;
	// Use this for initialization
	void Start () {
        Heart = GameObject.FindWithTag("Heart");
        currentTarget = Heart;
        
        
        
        if(transform.position.x > currentTarget.transform.position.x){
            GetComponent<SpriteRenderer>().flipX = true;  
        }
        findTarget();
        
        
	}
    
    public void waveStatMultiplier(int wave){
        int difficulty = wave / 5;
        
        damage = damage + (difficulty)*(damage)*0.18f;
        health = health + (difficulty)*(health)*0.25f;
        xpWorth = xpWorth + (int)(xpWorth*difficulty*0.1f);
    }
    
    public void updateHealthText(){
        healthText.GetComponent<TextMesh>().text = Mathf.Round(health*100f)/100f + "";
    }
	
    public void findTarget(){
        GameObject[] Defenses = GameObject.FindGameObjectsWithTag("AgroDefense");
        float minDistance = 10000f;
        if(transform.position.x > Heart.transform.position.x){
            foreach(GameObject g in Defenses){
                if(g.transform.position.x < transform.position.x && g.transform.position.x > Heart.transform.position.x){
                    float gDist = Vector3.Distance(g.transform.position, transform.position);
                    if(gDist < minDistance){
                        minDistance = gDist;
                        currentTarget = g;
                    }
                }
            }
            if(minDistance == 10000f){
                currentTarget = Heart;
            } 
        }
        if(transform.position.x < Heart.transform.position.x){
            foreach(GameObject g in Defenses){
                if(g.transform.position.x > transform.position.x && g.transform.position.x < Heart.transform.position.x){
                    float gDist = Vector3.Distance(g.transform.position, transform.position);
                    if(gDist < minDistance){
                        minDistance = gDist;
                        currentTarget = g;
                    }
                }
            }
            if(minDistance == 10000f){
                currentTarget = Heart;
            } 
        }
        
    }
    
	// Update is called once per frame
	void Update () {
		Movement();
        healthText.GetComponent<TextMesh>().text = health + "";
        
       
        
	}
    
    protected void Movement(){
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        
       
            if(transform.position.x < currentTarget.transform.position.x ){
                if(Vector3.Distance(transform.position, currentTarget.transform.position) > (range-0.1f)){
                    rb2D.velocity = new Vector3((transform.right.x * speed * Time.deltaTime * 10), rb2D.velocity.y, 0f);
                }
                
                
            }else{
                if(Vector3.Distance(transform.position, currentTarget.transform.position) > (range-0.1f)){
                    rb2D.velocity = new Vector3((-transform.right.x * speed * Time.deltaTime * 10), rb2D.velocity.y, 0f);
                }
                
                
            }
            
        
    }
    
    public void takeDamage(float d){
        health -= d;
        GameObject damageText = Instantiate(damageTagPrefab, transform.position+new Vector3(0f, 0.15f, 0f), Quaternion.identity);
        damageText.GetComponent<TextMesh>().text = Mathf.Round(100f*d)/100f+"";
        damageText.GetComponent<TextMesh>().color = new Color(1f,0.5f,0.5f);
        Destroy(damageText, 1.8f);
        if(health <= 0f){
                Die();
        }
    }
      
    void OnTriggerEnter2D(Collider2D col){
        
        if(col.gameObject.tag == "BShot"){
            col.gameObject.GetComponent<BallistaShot>().reduceHits();
           
            takeDamage(col.gameObject.GetComponent<BallistaShot>().damage);
            
        }
        
        
    }
    protected float generateDamage(){
        
        float d = damage;
        
        try{
            if(transform.position.y - 0.1f >= currentTarget.transform.position.y){
                d = d*1.1f;
            }
        }catch (Exception e) {
            Debug.Log("Target not found: Re-targeting");
            findTarget();
        }
        return d;
    }
    
    void Die(){
        Drop();
        Destroy(gameObject);
    }
    
    void Drop(){
        for(int i = 0; i<dropAmount; i++){
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        GameObject p = GameObject.FindWithTag("Player");
        p.GetComponent<Character>().gainXP(xpWorth);
       
    }
}
