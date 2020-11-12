using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
    public float damage;
    public bool energyReturn = false;
    public bool exploding = false;
    public float timer = 0;
    public bool bouncy = false;
    public GameObject ignored = null;
    public bool expanding = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(exploding){
            timer -= Time.deltaTime;
            if(timer <= 0){
                shootShots();
                Destroy(gameObject);
            }
            
            
        }
        if(expanding){
            transform.localScale += new Vector3(0f,8f*Time.deltaTime, 0);
            transform.position += transform.up * 4f * Time.deltaTime;
        }
	}
    
    void shootShots(){
        for(int i = 0; i < 12; i++){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.transform.Rotate(0, 0, 30f*i, Space.Self);
            shot.GetComponent<Rigidbody2D>().AddForce(shot.transform.up*2.5f);  
            shot.GetComponent<Shot>().damage = damage;
            
         
        }
         
      
    }
    
    void OnCollisionEnter2D(Collision2D col){
        ShipPlayer script = GameObject.Find("Player").GetComponent<ShipPlayer>();
      
        if(bouncy){
            gameObject.GetComponent<Rigidbody2D>().velocity = -160f*gameObject.GetComponent<Rigidbody2D>().velocity*gameObject.GetComponent<Rigidbody2D>().mass;
        }
        
        if(exploding){
            return;
        }
        if(energyReturn){
            script.energy += 1;
            energyReturn = false;
            
            GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position,  Quaternion.identity);
            text.GetComponent<TextMesh>().text = "1 energy";
            text.GetComponent<TextMesh>().color = new Color(255f,255f,255f,150f);
            Destroy(text, 1);
        }
       
    
    }
}
