using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour {
    public GameObject owner;
    public GameObject target = null;
    public bool targetSet = false;
    
    public float coolDown = 3f;
    public float cd = 3f;
    
    public float range = 1f;
    public float baseDamage = 4f;
    public float healthPoints = 100f;
    public float maxHealthPoints = 100f;
    public string direction = "left";
    public bool finishedBuilding = false;
    public int popValue = 1;
    public string type = "-";
    public int cost = 10;
    public int level = 1;
    List<GameObject> FloorTouches = new List<GameObject>();
    public GameObject healthText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public float getHealthPercent(){
        return healthPoints/generateHealth();
    }
    
    public void updateHealthText(){
        healthText.GetComponent<TextMesh>().text = Mathf.Round(healthPoints*100f)/100f + "";
    }
    
    public void setStats(float c, float r, float d, float h){
        coolDown = c;
        cd = c;
        range = r;
        baseDamage = d;
        healthPoints = h;
    }
    
    public void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Floor"){
            FloorTouches.Add(col.gameObject);
            GetComponent<SpriteRenderer>().color = new Color(50f/255f, 220f/255f, 50f/255f);
        }
    }
    
    public void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.tag == "Floor"){
            FloorTouches.Remove(col.gameObject);
            GetComponent<SpriteRenderer>().color = new Color(200f/255f, 50f/255f, 50f/255f);
        }
    }
    
    public int getFloorTouches(){
        return FloorTouches.Count;
    }
    
    public void Rotate(){
        if(direction == "left"){
            direction = "right";
            healthText.transform.rotation =  Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }else{
            direction = "left";
            healthText.transform.rotation =  Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
   
    }
    
    public void GenerateTarget(){
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject t in targets){
            if(direction == "left"){
                if(t.transform.position.x < transform.position.x && Vector3.Distance(t.transform.position, transform.position) < generateRange()){
                    target = t;
                    targetSet = true;
                    return;
                }
            }else{
                if(t.transform.position.x > transform.position.x && Vector3.Distance(t.transform.position, transform.position) < generateRange()){
                    target = t;
                    targetSet = true;
                    return;
                }
            }
        }
    }
    
     
    public void setTarget(GameObject g){
        target = g;
        targetSet = true;
        Destroy(GetComponent<Rigidbody2D>());
    }
    
    public void setOwner(GameObject g){
        owner = g;
        
        healthPoints = generateHealth();
        //maxHealthPoints = generateHealth();
    }
    
    public float generateDamage(){
        float characterPower = owner.GetComponent<Character>().getTowerPower();
        float d = (baseDamage * (1f+0.1f*(level-1)) * (1f + 5*(1f - Mathf.Pow(0.995f,characterPower))) );
        if(targetSet && transform.position.y - 0.1f >= target.transform.position.y){
            d = d*1.1f;
        }
        return d;
    }
    public float generateCoolDown(){
        float characterTowerSpeed = owner.GetComponent<Character>().getTowerSpeed();
        float s = coolDown  *(100f *Mathf.Pow(0.92f, (float)level-1)* Mathf.Pow(0.97f, (float)characterTowerSpeed))/100f;
        return s;
    }
    public float generateHealth(){
        float characterTowerDefense = owner.GetComponent<Character>().getTowerDefense();
        float h = maxHealthPoints *  (1+0.2f*(level-1)) *(1f + 3*(1f - Mathf.Pow(0.98f,characterTowerDefense)));
        return h;
    }
    
  
    public float generateRange(){
        float characterTowerRange = owner.GetComponent<Character>().getTowerRange();
        float r = range *  (1f + (1f - Mathf.Pow(0.96f,characterTowerRange)));
        return r;
    }
    
    public void takeDamage(float d){
        healthPoints -= d;
        if(healthPoints <= 0){
            owner.GetComponent<Character>().pop -= popValue;
            Destroy(gameObject);
        }
    }
    
    public float getHealth(){
        return healthPoints;
    }
    public bool isMaxHealth(){
        return healthPoints == maxHealthPoints;
    }
    
    public void Repair(float r){
        healthPoints += r;
        if(healthPoints > generateHealth()){
            healthPoints = maxHealthPoints;
        }
    }
    
    public void upgrade(){
        level++;
        healthPoints = generateHealth();
    }
}
