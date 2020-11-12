using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShip : MonoBehaviour {
    public GameObject thrusterFlame;

    public GameObject backGround;
    public GameObject station;
    Vector3 target;
    int burnTime = 15; // how long (frames) the thruster sprite stays active after player stops accelerating
    
    
    float speed = 6f; // speed of ship

    public int notoriety = 0; // Difficulty modifier
    
    
    public float maxHealth = 100f;
    public float health = 100f;
    
    public float maxEnergy = 50f;
    public float energy = 50f;
    
    public int metal = 0;
    
    float shipEnergyRegen = 1.5f; // Amount of energy regenerated per second.
    
	// Use this for initialization
	void Start () {
		hideThrusterFlame();
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
        if(burnTime >= 0){
           burnTime--;
        }else{
           hideThrusterFlame(); 
           burnTime=15;
        }
        
        paralaxMove();
        regenerateEnergy();
	}
    
    public void hideThrusterFlame() {
        thrusterFlame.SetActive(false);
    }
    public void Move() {
        Rigidbody2D rb2D = this.gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
        rb2D.AddForce(transform.up * speed);
        thrusterFlame.SetActive(true);
        burnTime=15;
    }
    
    public void Turn (Vector3 tar){
            tar.z = 2f;
            target = new Vector3(tar.x, tar.y, 2f);
            Vector3 objectPos = transform.position;
            tar.x = tar.x - objectPos.x;
            tar.y = tar.y - objectPos.y;
            float angle = Mathf.Atan2(tar.y, tar.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90f));
    }
     
    
    public void paralaxMove(){
        backGround.transform.position = new Vector3(transform.position.x/1.5f, transform.position.y/1.5f, 100f);
    }
    
    public void regenerateEnergy(){
        if(energy == maxEnergy)
            return;
        energy += shipEnergyRegen * Time.deltaTime;
        if(energy > maxEnergy){
            energy = maxEnergy;
        }
    }
    
    public int getMetal(){
        return metal;
    }
    public void spendMetal(int cost){
        metal -= cost;
        if(metal < 0){
            metal = 0;
        }
    }
}
