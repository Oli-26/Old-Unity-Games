using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Astroid : MonoBehaviour {
    public static int Amount = 0;
    GameObject Player;
    public bool isMega = false;
    public float health = 2;
    
    int val;
	// Use this for initialization
	void Start () {
		Astroid.Amount++;
        Player = GameObject.Find("Player");
        val = (int)Random.Range(2000, 4000);
	}
	
	// Update is called once per frame
	void Update () {
        if(Mathf.Abs(Player.transform.position.x - transform.position.x) > 10f){
            Astroid.Amount--;
            Destroy(gameObject);
        }else if(Mathf.Abs(Player.transform.position.y - transform.position.y) > 15f){
            Astroid.Amount--;
            Destroy(gameObject);
        }          
        
	}
    
    
    void OnCollisionEnter2D(Collision2D col){
        
        if(col.gameObject.tag == "Shot"){
            ShipPlayer script = GameObject.Find("Player").GetComponent<ShipPlayer>();
            Destroy(col.gameObject);
            
            
            health -= script.getDamage();
            if(health <= 0){
                
                
                
                if(script.weaponMVDouble1){
                    float rand = UnityEngine.Random.Range(0, 100f);
                    if(rand > (100-12f)){
                        val = val*2;
                        GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), Camera.main.transform.position + new Vector3(4, -4f, 10),  Quaternion.identity);
                        text.GetComponent<TextMesh>().text = "Double MV!";
                        Destroy(text, 2);
                    }
                    
                }
            
                
                
                Drop();
                Astroid.Amount--;
                Destroy(gameObject);
                
            }
          
            }
    
    }
    
    void Drop(){
        float rand = Random.Range(1,100);
        ShipPlayer script = GameObject.Find("Player").GetComponent<ShipPlayer>();
        if(isMega){
            val = 5*val;
        }
        val = (int)(val * script.getMiningValue());
        
        script.metal += val;
        GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position,  Quaternion.identity);
        text.GetComponent<TextMesh>().text = val + " metal";
        Destroy(text, 3);
        
        
        
        val = (int)Random.Range(2f, 8f);
        if(isMega){
            val = 5*val;
        }
        script.fuel += val;
        if(script.fuel >= script.maxFuel){
            script.fuel = script.maxFuel;
        }
        text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position + new Vector3(0, 0.25f, 0),  Quaternion.identity);
        text.GetComponent<TextMesh>().text = val + " fuel";
        Destroy(text, 3);
    
    
    }
}
