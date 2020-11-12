using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShot : MonoBehaviour {
    public float damage = 1f;
    public int remainingHits = 1;
    public GameObject target = null;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime*0.5f);
        
	}
    
    public void setTarget(GameObject g){
        target = g;
    }
    
    public void setDamage(float d){
        damage = d;
    }
    
    public void setHits(int h){
        remainingHits = h;
    }
    public void reduceHits(){
        if(remainingHits == 1){
            Destroy(gameObject);
            return;
        }
        remainingHits--;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "AgroDefense"){
            col.gameObject.GetComponent<Defense>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
