using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float damage = 0.5f;
    public float bonusDamage = 2f;
    public float bonusReserveMax = 10f;
    public float bonusReserve = 0f;
    public float chargeRate = 2f;

    public bool active = true;
    public bool equiped = false;
    public bool charging = false;
    public string type = "unset";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(equiped && charging){
            bonusReserve+=chargeRate*Time.deltaTime;
            if(bonusReserve > bonusReserveMax){
                bonusReserve = bonusReserveMax;
            }
            float m =  bonusReserve/bonusReserveMax;
            GetComponent<SpriteRenderer>().color = new Color((120f+135f*m)/255f, (120f-80f*m)/255f, (120f-80f*m)/255f);
        }
	}
    
    public void OnTriggerStay2D(Collider2D col){
        if(equiped && col.gameObject.tag == "Enemy" && active){
            float tempDam = damage;
            if(bonusReserve > bonusDamage){
                tempDam += bonusDamage;
                bonusReserve -= bonusDamage;
            }else{
                tempDam += bonusReserve;
                bonusReserve = 0;
            }            
            
            tempDam = tempDam;
            
            col.gameObject.GetComponent<Enemy>().takeDamage(tempDam);
            active = false;
            float m =  bonusReserve/bonusReserveMax;
            GetComponent<SpriteRenderer>().color = new Color(255f*m/255f, 180f/255f, 180f/255f);
        }
        if(col.gameObject.tag == "Player"){
            col.gameObject.GetComponent<Character>().pickUpItem(gameObject);
        }
        
    }
    
    public void Assume(GameObject g){
        Weapon wep = g.GetComponent<Weapon>();
        damage = wep.damage;
        bonusDamage = wep.bonusDamage;
        bonusReserve = wep.bonusReserve;
        chargeRate = wep.chargeRate;
        
        GetComponent<SpriteRenderer>().sprite = g.GetComponent<SpriteRenderer>().sprite;
    }
}
