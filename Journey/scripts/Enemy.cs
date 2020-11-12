using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float ShootCooldown = 5f;
    public float ShootRate = 5f;
    public float health = 5f;
    public float damage = 2f;
    public float dropMin = 20;
    public float dropMax = 50;
    public int type = 1;
    public float shootRange = 5f;
    public float turnRate = 0.5f;
    public int xp;
    public float armour = 0f;
    GameObject Player;
    

    public Vector3 targetPosition;
    bool targetExists = false;
    
	// Use this for initialization
	void Start () {
		Player =  GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if(!Player.GetComponent<ShipPlayer>().paused){
            if(Mathf.Abs(Player.transform.position.x - transform.position.x) > 15f){
                Astroid.Amount--;
                Destroy(gameObject);
            }else if(Mathf.Abs(Player.transform.position.y - transform.position.y) > 20f){
                Astroid.Amount--;
                Destroy(gameObject);
            }

            if(type == 3){
                transform.Rotate(0, 0, 50*Time.deltaTime, Space.Self);
            }else{
                Vector3 vectorToTarget = Player.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnRate);
                
            }
            
            

            if(type == 6){
                gameObject.GetComponent<Rigidbody2D>().velocity  = transform.right/4f; 
            }
           
            
            ShootCooldown -= Time.deltaTime;
            
            if(ShootCooldown <= 0 && Vector3.Distance(Player.transform.position, transform.position) < shootRange){
                ShootCooldown = ShootRate;
                Shoot();
            }
        }
    }
    
    
    void OnCollisionEnter2D(Collision2D col){
        
        if(col.gameObject.tag == "Shot"){
            ShipPlayer script = GameObject.Find("Player").GetComponent<ShipPlayer>();
            float dam = col.gameObject.GetComponent<Shot>().damage;
            
            
            
            
            
            if(type == 4 || type == 9){
                if(script.itemBossDamage1){
                    dam += 1;
                }
            }
            
            
            dam -= armour;
            if(dam < 1){
                dam = 1;
            }
            
            GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), col.gameObject.transform.position + new Vector3(0, -0.5f, 0),  Quaternion.identity);
            text.GetComponent<TextMesh>().text = dam + " ";
            Destroy(text, 1);
            text.GetComponent<TextMesh>().color = new Color(255f, 0, 0, 255f);
            
            
            health -= dam;
            if(health <= 0){
                Destroy(gameObject);
                Drop();
                
            }
          
        }
        if(col.gameObject.tag == "Asteroid"){
            Vector3 vel = gameObject.GetComponent<Rigidbody2D>().velocity - col.gameObject.GetComponent<Rigidbody2D>().velocity;
            
            if(Vector3.Distance(vel, new Vector3(0,0,0)) > 0.1f){
                health -= 2;
            }
            
        }
    
    }
    
    void Drop(){
        int rand1 = (int)Random.Range(dropMin,dropMax);
        ShipPlayer script = GameObject.Find("Player").GetComponent<ShipPlayer>();
        script.killCount++;
        script.energy += script.shipKillCharge;
        GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position + new Vector3(0, -0.25f, 0),  Quaternion.identity);
        text.GetComponent<TextMesh>().text = script.shipKillCharge + " energy";
        Destroy(text, 3);
        
        if(script.weaponLifeLeach1){
            script.health += 2;
            if(script.health > script.maxHealth){
                script.health = script.maxHealth;
            }
        }
        if(script.shipDoubleXP1){
            float rand = UnityEngine.Random.Range(0,100f);
            if(rand < (100-12)){
                xp = xp*2;
            }
            text = Instantiate(Resources.Load<GameObject>("TextPrefab"), Camera.main.transform.position + new Vector3(4, -4f, 10),  Quaternion.identity);
            text.GetComponent<TextMesh>().text = "Double XP!";
            Destroy(text, 2);
        }
        
        if(type == 4){
            script.stage++;
            script.killCount = 0;
            script.money += 50;
            script.bossSpawned = false;
        }
        
        if(type == 9){
            script.stage++;
            script.killCount = 0;
            script.money += 50;
            script.bossSpawned = false;
        }
        
        if(type == 5){
            int gain = 10;
            if(script.itemDoubleCargo1){
                gain = 20;
            }
            script.money += gain;
            text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position,  Quaternion.identity);
            text.GetComponent<TextMesh>().text = gain + " money";
            Destroy(text, 3);
        }else{
            GameObject.Find("Player").GetComponent<ShipPlayer>().metal += rand1;
            text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position,  Quaternion.identity);
            text.GetComponent<TextMesh>().text = rand1 + " metal";
            Destroy(text, 3);
        }
        
      
        
        script.gainXP(xp);
        text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position + new Vector3(0, 0.25f, 0),  Quaternion.identity);
        text.GetComponent<TextMesh>().text = xp + " xp";
        Destroy(text, 3);
        

        
    }
    void Shoot(){
        if(type == 1){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*2.5f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
        }
        if(type == 2){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f  + transform.right/4f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*1.5f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            
            
            shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f  - transform.right/4f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*1.5f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
        }
        
        if(type == 3){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f  + transform.right/4f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*4f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            
            shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up/2f  - transform.right/4f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*4f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
        }
        
        if(type == 4){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 5);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*4f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            
            shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up  + transform.right/6f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 5);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.transform.Rotate(0, 0, -7, Space.Self);
            shot.GetComponent<Rigidbody2D>().AddForce(shot.transform.up*4f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            
            
            shot = Instantiate(Resources.Load<GameObject>("lazershotenemy"), transform.position + transform.up  - transform.right/6f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 5);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.transform.Rotate(0, 0, 7, Space.Self);
            shot.GetComponent<Rigidbody2D>().AddForce(shot.transform.up*4f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            
        }
        
        if(type == 6){
            GameObject shot = Instantiate(Resources.Load<GameObject>("dotshotenemy"), transform.position + transform.up, transform.rotation);
            //shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 8);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*20f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
        }
        
        
        if(type == 7){
             GameObject shot = Instantiate(Resources.Load<GameObject>("dotshotenemy"), transform.position + transform.up, transform.rotation);
            //shot.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            Destroy(shot, 5);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*15f);  
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().exploding = true;
            shot.GetComponent<Shot>().timer = 3.5f;
            shot.GetComponent<SpriteRenderer>().color= new Color(255f,0f,20f,255f);
            shot.GetComponent<Shot>().ignored = gameObject;
        }
        if(type == 8){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazer"), transform.position + transform.up/2f, transform.rotation);

            Destroy(shot, 2.5f);

            shot.transform.localScale += new Vector3(0.3f,0f, 0);
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            shot.GetComponent<Shot>().expanding = true;
        }
        if(type == 9){
            GameObject shot = Instantiate(Resources.Load<GameObject>("lazer"), transform.position + transform.up/2f, transform.rotation);

            Destroy(shot, 6f);

            shot.transform.localScale += new Vector3(0.6f,0f, 0);
            shot.GetComponent<Shot>().damage = damage;
            shot.GetComponent<Shot>().ignored = gameObject;
            shot.GetComponent<Shot>().expanding = true; 
        }
    }
}
