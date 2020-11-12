using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    public GameObject shot;
    GameObject target;
    bool targetExists = false;
    float shotRate = 2f;
    float shotCoolown = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(shotCoolown > 0){
            shotCoolown -= Time.deltaTime;
            
        }
		if(shotCoolown <= 0 ){
            Shoot();
            shotCoolown = shotRate;
        }
            
           
        
        
        
	}
    
    void Shoot(){
        targetEnemy();
        if(targetExists){
            GameObject s = Instantiate(shot, transform.position + new Vector3(0, 0.25f, 0f), Quaternion.identity);
            s.GetComponent<Shot>().setTarget(target);
            Destroy(s, 5f);
        }
        
    }
    
    public void targetEnemy(){
        targetExists = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        for(int i = 0; i < enemies.Length; i++){
            if(Vector3.Distance(transform.position, enemies[i].transform.position) <= 5){
                target = enemies[i];
                targetExists = true;
                break;
            }
            
        }
        
    }
}
