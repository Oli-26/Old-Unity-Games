using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 
public class ShipPlayer : MonoBehaviour {
    Rigidbody2D rb;
    float baseMoveSpeed = 75f;
    float baseBreakSpeed = 1f;

    public bool Breaking = false;
    
    public float turningRate = 50f; 
    
    public GameObject shopButton;
    public GameObject healthBar;
    public GameObject energyBar;
    public Text healthText;
    public Text energyText;
    public Text damageText;
    public Text shipText;
    public Text killText;
    public GameObject xpBar;
    public Text levelText;
    
    public GameObject fuelBar;
    public Text fuelText;
    
    GameObject thruster1;
    GameObject thruster2;
    public GameObject armour1;
    public GameObject armour2;
    
    bool thruster1Active = false;
    bool thruster2Active = false;
   
    public Quaternion targetRotation = Quaternion.identity;
    Vector3 targetPosition = new Vector3(0,0,0);
	
    public GameObject lazerShotPrefab;
    
    public GameObject Station;
    public GameObject startCanvas;
    
    public GameObject upgradeCanvas;
    public GameObject shipUpgrades;
    public GameObject weaponUpgrades;
    public GameObject shopCanvas;
    
    public int metal = 0;
    public int money = 0;
    
    public int killCount = 0;
    public int stage = 0;
    
    public int level = 0;
    public int xp = 0;
    public int levelPoints = 0;
    
    public bool paused = false;
    
    // WEAPON STATS
    float weaponDamage = 2f;
    float weaponFireCost = 5f;
    float weaponMiningValue = 0.01f;
    float weaponDoubleShotChance = 0f;
    // SHIP STATS
        
    public float health = 50;
    public float maxHealth = 50;
    public float energy = 100;
    public float maxEnergy = 100;
    public float shipKillCharge = 2f;
    public float fuel = 100;
    public float maxFuel = 100;
    public float armour = 0;
    
    float energyRechargeRate = 0.5f;
    float thrusterUseCost = 0.75f;
    
    
    //  // Upgrades 
   
    // Weapon
    bool weaponDamage1 = false;
    bool weaponDamage2 = false;
    bool weaponCost1 = false;
    
    bool weaponCostReturn1 = false;
    bool weaponMV1 = false;
    bool weaponMV2 = false;
    
    bool weaponDoubleShot1 = false;
    bool weaponEnergyReturn1 = false;
    
    bool weaponFreeShot1 = false;
    public bool weaponMVDouble1 = false;
    
    public bool weaponLifeLeach1 = false;
    bool weaponBouncyShot1 = false;

    
    // Ship
    bool shipRecharge1 = false;
    bool shipMaxEnergy1 = false;
    bool shipMaxEnergy2 = false;
    bool shipMaxHealth1 = false;
    
    bool shipThrusterCost1 = false;

    bool shipThrusterBoost1 = false;
    bool shipTurnRate1 = false;
    bool shipArmour1 = false;
    
    
    bool shipHealAmount1 = false;
    bool shipRecharge2 = false;
    
    bool shipReflect1 = false;
    bool shipAbsorb1 = false;
    
    bool shipEnergyKill1 = false;
    public bool shipDoubleXP1 = false;
    
    bool shipBlock1 = false;
    bool shipThrusterBoost2 = false;
    // Items
    
    bool itemDoubleShot1 = false;
    public bool itemBossDamage1 = false;
    bool itemAstroidSpawn1 = false;
    public bool itemDoubleCargo1 = false;
    
    
    
    
    
    
    // // Global stats
    public float astroidSpawnRate = 8f;
    public float astroidCooldown = 8f;
    public int astroidMax = 10;
    
    public float enemySpawnRate = 20f;
    public float enemyCooldown = 20f;
    
    
    
    
    public Text topText;
    public bool bossSpawned = false;
    
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
        thruster1 = GameObject.Find("Thruster1");
        thruster2 = GameObject.Find("Thruster2");
        thruster1.SetActive(false);
        thruster2.SetActive(false);
        loadGlobal();
        Load();
	}
    
    public void Pause(bool setting){
        paused = setting;
        if(paused){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        
        // Breaking and thrusters.
        if(Breaking){
            rb.velocity = rb.velocity - rb.velocity*baseBreakSpeed*Time.deltaTime;
        }else{
            
            if(thruster1Active){
                if(fuel < thrusterUseCost*Time.deltaTime){
                    deactivateThruster(1);
                }else{
                    fuel -= thrusterUseCost*Time.deltaTime;
                    transform.Rotate(new Vector3(0, 0, -Time.deltaTime*turningRate));
                    rb.AddForce(Vector3.Normalize(transform.up*getMoveSpeed()* Time.deltaTime));
                }
            }
            if(thruster2Active){
                if(fuel < thrusterUseCost*Time.deltaTime){
                    deactivateThruster(2);
                }else{
                    fuel -= thrusterUseCost*Time.deltaTime;
                    transform.Rotate(new Vector3(0, 0, Time.deltaTime*turningRate));
                    rb.AddForce(Vector3.Normalize(transform.up*getMoveSpeed()* Time.deltaTime));
                }
            }
        }
        
        // Camera tracking
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        if(!paused){
            // Asteroid spawning.
            astroidCooldown -= Time.deltaTime;
            if(astroidCooldown <= 0 && Astroid.Amount < astroidMax){
                float xDistance = UnityEngine.Random.Range(6f,7f);
                float yDistance = UnityEngine.Random.Range(0f, 10f);
                if(UnityEngine.Random.Range(0,2f) < 1){
                    xDistance = -xDistance;
                }
                if(UnityEngine.Random.Range(0,2f) < 1){
                    yDistance = -yDistance;
                }
                GameObject astroid ;
                if(UnityEngine.Random.Range(0,100f) > 96){
                   astroid = Instantiate(Resources.Load<GameObject>("Mega Astroid"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                }else{
                   astroid = Instantiate(Resources.Load<GameObject>("Asteroid Brown"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                }
                
                astroidCooldown = astroidSpawnRate;
                astroid.GetComponent<Rigidbody2D>().velocity = (-astroid.transform.position + transform.position)/10f;
            }
            
            // Enemy spawning
            enemyCooldown -= Time.deltaTime;
            if(enemyCooldown <= 0 && Vector3.Distance(transform.position, Station.transform.position) > 4f){
                spawnEnemy();
                
            }
        }
        
        // UI rendering
        topText.text = "metal : " + metal + "\nMoney : " + money + "\nSkill points : " + levelPoints;
        healthBar.GetComponent<Slider>().value = health/maxHealth;
        energyBar.GetComponent<Slider>().value = energy/maxEnergy;
        fuelBar.GetComponent<Slider>().value = fuel/maxFuel;
        healthText.text = (Mathf.Round(health * 100f) / 100f) + "/" + maxHealth;
        energyText.text = (Mathf.Round(energy * 100f) / 100f) + "/" + maxEnergy;
        fuelText.text =  (Mathf.Round(fuel * 100f) / 100f) + "/" + maxFuel;
        damageText.text = weaponDamage + " Damage\n" + (100*weaponMiningValue) + " percent Mining value\n" + weaponFireCost + " energy per shot";
        shipText.text = "Max Health : " + maxHealth + "\nMax energy : " + maxEnergy + "\nMax Fuel : " + maxFuel + "\nThruster consumption : " + thrusterUseCost + "/s\nEnergy recharge : " + energyRechargeRate + "/s\nEnergy per kill : " + shipKillCharge;
        killText.text = "Stage : " + stage + "\nKills : " + killCount + "/40" ;
        xpBar.GetComponent<Slider>().value = ((float)(xp - levelXP(level-1))/(levelXP(level)- levelXP(level-1)));
        levelText.text = level + " ";
        
        // Other
        
        energy += energyRechargeRate*Time.deltaTime;
        if(energy > maxEnergy){
            energy = maxEnergy;
        }
        
        
	}
    
    
    void spawnEnemy(){
        
        
        float xDistance = UnityEngine.Random.Range(3f,6f);
        float yDistance = UnityEngine.Random.Range(4f, 6f);
        if(UnityEngine.Random.Range(0,2f) < 1){
            xDistance = -xDistance;
        }
        if(UnityEngine.Random.Range(0,2f) < 1){
            yDistance = -yDistance;
        }
        GameObject enemy ;
        
        if(stage == 1){
            if(killCount > 40 && !bossSpawned){
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip9"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);  
                bossSpawned = true;
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance+1, yDistance+1, 0),  Quaternion.identity); 
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance-1, yDistance-1, 0),  Quaternion.identity); 
            }else{
                enemyCooldown = enemySpawnRate + UnityEngine.Random.Range(-0.3f, 0.3f)*enemySpawnRate;
                float chance = UnityEngine.Random.Range(0,100f);
                chance += killCount;
                if(chance > 125f){
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);     
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance, yDistance+1, 0),  Quaternion.identity); 
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance, yDistance-1, 0),  Quaternion.identity); 
                }if(chance > 110f){
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip8"), transform.position + new Vector3(xDistance, yDistance+2, 0),  Quaternion.identity);
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip8"), transform.position + new Vector3(xDistance, yDistance-2, 0),  Quaternion.identity);
                }else if(chance > 90f){
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip8"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                }else if(chance > 60f){
                    chance = chance = UnityEngine.Random.Range(0,100f);
                    if(chance >= 50){
                       enemy = Instantiate(Resources.Load<GameObject>("EnemyShip7"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                    }else{
                       enemy = Instantiate(Resources.Load<GameObject>("EnemyShip6"), transform.position + new Vector3(xDistance, yDistance + 1f, 0),  Quaternion.identity);
                       enemy = Instantiate(Resources.Load<GameObject>("EnemyShip6"), transform.position + new Vector3(xDistance, yDistance - 1f, 0),  Quaternion.identity); 
                    }
                    
                }else{
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip6"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                }
                
                chance = UnityEngine.Random.Range(0,100f);
                if(chance >= 90f){
                    xDistance = UnityEngine.Random.Range(3f,6f);
                    yDistance = UnityEngine.Random.Range(4f, 6f);
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyBonusShip"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                    
                    enemy.GetComponent<Rigidbody2D>().velocity = (-enemy.transform.position + transform.position)/6f;
                  
                }
            }
        }
        
        
        
        
        
        if(stage == 0){
            enemyCooldown = enemySpawnRate + UnityEngine.Random.Range(-0.3f, 0.3f)*enemySpawnRate;
            if(killCount > 40 && !bossSpawned){
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip4"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance + 1.5f, yDistance + 1.5f, 0),  Quaternion.identity);
                enemy.GetComponent<Enemy>().shootRange = 15;
                enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance - 1.5f, yDistance - 1.5f, 0),  Quaternion.identity);
                enemy.GetComponent<Enemy>().shootRange = 15;
                bossSpawned = true;
                
            }else{
                float chance = UnityEngine.Random.Range(0,100f);
                chance += killCount;
                if(chance > 125f){
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance + 1.5f, yDistance, 0),  Quaternion.identity); 
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance, yDistance - 1.5f, 0),  Quaternion.identity); 
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance + 1.5f, yDistance, 0),  Quaternion.identity); 
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance, yDistance + 1.5f, 0),  Quaternion.identity); 
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                    enemy.GetComponent<Enemy>().shootRange = 15;
                }else if(chance > 110f){
                    
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance - 0.75f, yDistance, 0),  Quaternion.identity);
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance + 0.75f, yDistance, 0),  Quaternion.identity);
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance + 0.75f, 0),  Quaternion.identity); 
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance - 0.75f, 0),  Quaternion.identity);
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance - 2.5f, 0),  Quaternion.identity);
                }else if(chance > 96f){
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance + 0.75f, 0),  Quaternion.identity); 
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance - 0.75f, 0),  Quaternion.identity);
                }else if(chance > 85f){
                   
                    if(UnityEngine.Random.Range(0,100f) < 60){
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip2"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                    }else{
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance, yDistance + 0.75f, 0),  Quaternion.identity); 
                    }
                }else if(chance > 70f){
                    if(UnityEngine.Random.Range(0,100f) < 60){
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance + 0.75f, 0),  Quaternion.identity);
                    }else{
                        enemy = Instantiate(Resources.Load<GameObject>("EnemyShip3"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity); 
                    }
                }else{
                   enemy = Instantiate(Resources.Load<GameObject>("EnemyShip"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                }

                
                enemy.GetComponent<Rigidbody2D>().velocity = (-enemy.transform.position + transform.position)/50f;
                //Bonus Ship
                chance = UnityEngine.Random.Range(0,100f);
                if(chance >= 90f){
                    xDistance = UnityEngine.Random.Range(3f,6f);
                    yDistance = UnityEngine.Random.Range(4f, 6f);
                    enemy = Instantiate(Resources.Load<GameObject>("EnemyBonusShip"), transform.position + new Vector3(xDistance, yDistance, 0),  Quaternion.identity);
                    
                    enemy.GetComponent<Rigidbody2D>().velocity = (-enemy.transform.position + transform.position)/6f;
                  
                }
            }
        }
    }
    int levelXP(int l){
        return (int)((100*(l+1)) + 12*Mathf.Pow(level, 1.7f));
    }
    public void gainXP(int gain){
        xp += gain;
        while(xp > levelXP(level)){
            level+=1;
            levelPoints +=1;
        }
        
    }
    
    void Movement () {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0);
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
            
           
            if(IsTapOverUIObject()){
                return;
            }
            if(touch.phase == TouchPhase.Began){
              Shoot();  
            }

        }else if (Input.GetMouseButtonDown(0)){
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            if(IsPointerOverUIObject()){
                return;
            }
            Shoot();  

        }
    }

    
    public void activateThruster(int index){
        if(index == 1){
            thruster1.SetActive(true);
            thruster1Active = true;
        }else{
            thruster2.SetActive(true);
            thruster2Active = true;
        } 
    }
    public void deactivateThruster(int index){
        if(index == 1){
            thruster1.SetActive(false);
            thruster1Active = false;
        }else{
            thruster2.SetActive(false);
            thruster2Active = false;
        }
    }
    
    public void Shoot(){
        float tempFireCost = weaponFireCost;
        
        if(energy < tempFireCost){
           return;
        }
        if(weaponFreeShot1){
            float rand = UnityEngine.Random.Range(0,100f);
            if(rand > (100-12f)){
                tempFireCost = 0;
                GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), Camera.main.transform.position + new Vector3(4, -4f, 10f),  Quaternion.identity);
                text.GetComponent<TextMesh>().text = "Free shot!";
                Destroy(text, 2);
            }
            
        }
        
        
        float chance =  UnityEngine.Random.Range(0,100f);
        if(itemDoubleShot1){
            chance -= 1f*killCount;
        }
        if(weaponDoubleShotChance >= chance){
            energy -= tempFireCost;
            GameObject shot = Instantiate(lazerShotPrefab, transform.position + transform.up/2f - transform.right/8f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*2.5f);    
            shot.GetComponent<Shot>().damage = weaponDamage;
            shot.GetComponent<Shot>().energyReturn = weaponEnergyReturn1;
            if(weaponBouncyShot1){
                shot.GetComponent<Shot>().bouncy = true;
            }
            

            shot = Instantiate(lazerShotPrefab, transform.position + transform.up/2f + transform.right/8f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*2.5f);    
            shot.GetComponent<Shot>().damage = weaponDamage;
            shot.GetComponent<Shot>().energyReturn = weaponEnergyReturn1;
            if(weaponBouncyShot1){
                shot.GetComponent<Shot>().bouncy = true;
            }
        }else{
            energy -= tempFireCost;
            GameObject shot = Instantiate(lazerShotPrefab, transform.position + transform.up/2f, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            Destroy(shot, 3);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
            shot.GetComponent<Rigidbody2D>().AddForce(transform.up*2.5f);    
            shot.GetComponent<Shot>().damage = weaponDamage;
            shot.GetComponent<Shot>().energyReturn = weaponEnergyReturn1;
            if(weaponBouncyShot1){
                shot.GetComponent<Shot>().bouncy = true;
            }
        }        
        
    }
    
    float getMoveSpeed(){
        float moveSpeed = baseMoveSpeed;
        if(shipThrusterBoost2 && thruster1Active && thruster2Active){
            return 1.7f*moveSpeed/100f;
        }
        return moveSpeed/100f;
    }
    
    public float getDamage(){
        return weaponDamage;
    }
    public float getMiningValue(){
        return weaponMiningValue;
    }
    
    public void toggleBreak(){
        GameObject button = GameObject.Find("BreakButton");
        if(Breaking){
            Breaking = false;
            button.GetComponent<Button>().image.color = new Color(255f,255f,255f);
        }else{
            Breaking = true;
            button.GetComponent<Button>().image.color = new Color(255f,0f,0f);
        }
    }
    
    
    
    private bool IsTapOverUIObject()
     {
         Touch touch = Input.GetTouch(0);
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
         eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
         return results.Count > 0;
     }
    
     private bool IsPointerOverUIObject()
    {
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
         eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
         return results.Count > 0;
    }
    
    public void BuyItem(string upg){
        
        switch(upg){ 
            case "itemDoubleShot1":
                if(!itemDoubleShot1 && money >= 200){
                    itemDoubleShot1 = true;
                    GameObject.Find("ItemDoubleShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    money -= 200;
                }
                break;
            case "itemBossDamage1":
                if(!itemBossDamage1 && money >= 100){
                    itemBossDamage1 = true;
                    GameObject.Find("ItemBossDamage1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    money -= 100;
                }
                break;
            case "itemAstroidSpawn1":
                if(!itemAstroidSpawn1 && money >= 140){
                    itemAstroidSpawn1 = true;
                    GameObject.Find("ItemAstroidSpawn1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    astroidSpawnRate= astroidSpawnRate/2f;
                    astroidMax += 5;
                    money -=140;
                }
                break;
            case "itemDoubleCargo1":
                if(!itemDoubleCargo1 && money >= 80){
                    itemDoubleCargo1 = true;
                    GameObject.Find("ItemDoubleCargo1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    money -= 80;
                }
                break;
        }
    }
    public void Upgrade(string upg){
        switch(upg){
            case "weaponDamage1":
                if(!weaponDamage1 && metal >= 200){
                    metal -= 200;
                    weaponDamage1 = true;
                    weaponDamage+= 2;
                    GameObject.Find("WeaponDamage1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }                
                break;
            case "weaponDamage2":
                if(weaponDamage1 && !weaponDamage2 && !weaponMV1 && metal >= 200){
                    metal -= 200;
                    weaponDamage2 = true;
                    weaponDamage+= 1;
                    GameObject.Find("WeaponDamage2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }                
                break;
                
             case "weaponMV1":
                if(weaponDamage1 && !weaponMV1 && !weaponDamage2 && metal >= 400){
                    metal -= 400;
                    weaponMV1 = true;
                    weaponMiningValue+= 0.002f;
                    GameObject.Find("WeaponMV1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }                
                break;    
            case "weaponCost1":
                if((weaponDamage2 || weaponMV1) && !weaponCost1 && !weaponDoubleShot1 && metal >= 500){
                    metal -= 500;
                    weaponCost1 = true;
                    weaponFireCost -= 1;
                    GameObject.Find("WeaponCost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }                
                break;
            case "weaponDoubleShot1":
                if((weaponDamage2 || weaponMV1) && !weaponDoubleShot1 && !weaponCost1 && metal >= 600){
                    metal -= 600;
                    weaponDoubleShot1 = true;
                    weaponDoubleShotChance += 12f;
                    GameObject.Find("WeaponDoubleShot1").GetComponent<Image>().color =  new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;
            case "weaponEnergyReturn1":
                if((weaponDoubleShot1 || weaponCost1) && !weaponEnergyReturn1 && metal >= 700){
                    metal -= 700;
                    weaponEnergyReturn1 = true;
                    GameObject.Find("WeaponEnergyReturn1").GetComponent<Image>().color =  new Color(0f, 47f/255f, 255f/255f, 1f);
                    
                }
                break;
            case "weaponMV2":
                if((weaponDoubleShot1 || weaponCost1) && !weaponEnergyReturn1 && metal >= 600){
                    metal -= 600;
                    weaponMV2 = true;
                    weaponMiningValue += 0.003f;
                    GameObject.Find("WeaponMV2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;
             case "weaponFreeShot1":
                if((weaponEnergyReturn1 || weaponMV2) && !weaponFreeShot1 && !weaponMVDouble1 && metal >= 800){
                    metal -= 800;
                    weaponFreeShot1 = true;
                    
                    GameObject.Find("WeaponFreeShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;  
            case "weaponMVDouble1":
                if((weaponEnergyReturn1 || weaponMV2) && !weaponFreeShot1 && !weaponMVDouble1 && metal >= 800){
                    metal -= 800;
                    weaponMVDouble1 = true;
                    
                    GameObject.Find("WeaponMVDouble1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break; 
            case "weaponLifeLeach1":
                if((weaponFreeShot1 || weaponMVDouble1) && !weaponLifeLeach1 && !weaponBouncyShot1 && metal >= 900){
                    metal -= 900;
                    weaponLifeLeach1 = true;
                    
                    GameObject.Find("WeaponLifeLeach1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break; 
             case "weaponBouncyShot1":
                if((weaponFreeShot1 || weaponMVDouble1) && !weaponLifeLeach1 && !weaponBouncyShot1 && metal >= 900){
                    metal -= 900;
                    weaponBouncyShot1 = true;
                    
                    GameObject.Find("WeaponBouncyShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;     
            case "shipMaxEnergy1":
                if(!shipMaxEnergy1 && levelPoints >= 1){
                   levelPoints -= 1;
                    shipMaxEnergy1 = true;
                    maxEnergy += 20;
                    GameObject.Find("ShipMaxEnergy1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    energy += 20;
                }
                break;
            case "shipMaxEnergy2":
                if(shipRecharge1 && !shipMaxEnergy2 && !shipThrusterCost1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipMaxEnergy2 = true;
                    maxEnergy += 30;
                    energy += 30;
                    GameObject.Find("ShipMaxEnergy2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;
            case "shipRecharge1":
                if(shipMaxEnergy1 && !shipRecharge1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipRecharge1 = true;
                    energyRechargeRate += 0.3f;
                    GameObject.Find("ShipRecharge1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;    
            case "shipTurnRate1":
                if((shipMaxEnergy2 || shipThrusterCost1) && !shipTurnRate1 && !shipMaxHealth1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipTurnRate1 = true;
                    turningRate += 7;
                    GameObject.Find("ShipTurnRate1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;    
            case "shipThrusterCost1":
                if(shipRecharge1 && !shipThrusterCost1 && !shipMaxEnergy2 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipThrusterCost1 = true;
                    thrusterUseCost -= 0.25f;
                    GameObject.Find("ShipThrusterCost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                }
                break;  
            case "shipMaxHealth1":
                if((shipMaxEnergy2 || shipThrusterCost1) && !shipMaxHealth1 && !shipTurnRate1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipMaxHealth1 = true;
                    maxHealth += 25;
                    health += 25;
                    GameObject.Find("ShipMaxHealth1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
                    
                }
                break;
            case "shipArmour1":
                if((shipTurnRate1 || shipMaxHealth1) && !shipArmour1 && !shipThrusterBoost1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipArmour1 = true;
                    armour1.SetActive(true);
                    armour2.SetActive(true);
                    GameObject.Find("ShipArmour1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;
            case "shipThrusterBoost1":
                if((shipTurnRate1 || shipMaxHealth1) && !shipArmour1 && !shipThrusterBoost1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipThrusterBoost1 = true;
                    baseMoveSpeed += 35;
                    GameObject.Find("ShipThrusterBoost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;
            case "shipHealAmount1":
                if((shipThrusterBoost1 || shipArmour1) && !shipHealAmount1 && !shipRecharge2 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipHealAmount1 = true;

                    GameObject.Find("ShipHealAmount1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;
            case "shipRecharge2":
                if((shipThrusterBoost1 || shipArmour1) && !shipHealAmount1 && !shipRecharge2 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipRecharge2 = true;
                    energyRechargeRate += 0.3f;
                    GameObject.Find("ShipRecharge2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;    
            case "shipReflect1":
                if((shipHealAmount1 || shipRecharge2) && !shipReflect1 && !shipAbsorb1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipReflect1 = true;

                    GameObject.Find("ShipReflect1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break; 
            case "shipAbsorb1":
                if((shipHealAmount1 || shipRecharge2) && !shipReflect1 && !shipAbsorb1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipAbsorb1 = true;

                    GameObject.Find("ShipAbsorb1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;  
            case "shipEnergyKill1":
                if((shipReflect1 || shipAbsorb1) && !shipEnergyKill1 && !shipDoubleXP1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipEnergyKill1 = true;
                    shipKillCharge += 5;
                    GameObject.Find("ShipEnergyKill1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;    
            case "shipDoubleXP1":
                if((shipReflect1 || shipAbsorb1) && !shipEnergyKill1 && !shipDoubleXP1 && levelPoints >= 1){
                    levelPoints -= 1;
                    shipDoubleXP1 = true;

                    GameObject.Find("ShipDoubleXP1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;   
            case "shipBlock1":
                if((shipDoubleXP1||shipEnergyKill1) && !shipBlock1 && !shipThrusterBoost2 && levelPoints >= 1){
                    levelPoints-=1;
                    shipBlock1 = true;
                    armour += 1;
                    GameObject.Find("ShipBlock1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;
            case "shipThrusterBoost2":
                if((shipDoubleXP1||shipEnergyKill1) && !shipBlock1 && !shipThrusterBoost2 && levelPoints >= 1){
                    levelPoints-=1;
                    shipThrusterBoost2 = true;
                    
                    GameObject.Find("ShipThrusterBoost2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
                }
                break;    
            default:
                return;
        }
        

    }
    
    public void Repair(){
        if(metal >= 50){
            
            
            if(health >= maxHealth){
                return;
            }
            metal -= 50;
            float healAmount = 2;
            if(shipHealAmount1){
                healAmount += 2;
            }
            health += healAmount;
            if(health >= maxHealth){
                health = maxHealth;
            }
        }
    }
    public void EnergyClick(){
        energy += energyRechargeRate*0.1f;
        
        GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), Camera.main.transform.position + new Vector3(-0.7f, -6f, 10f),  Quaternion.identity);
        text.GetComponent<TextMesh>().text = (energyRechargeRate*0.1f) + " energy";
        text.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0.6f, 0);
        Destroy(text, 1.5f);
    }
    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "ShotEnemy"){
            if(shipAbsorb1){
                energy += col.gameObject.GetComponent<Shot>().damage;
                if(energy > maxEnergy){
                    energy = maxEnergy;
                }
                
            }

            health -= col.gameObject.GetComponent<Shot>().damage;
            
            
            GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position + new Vector3(0, -0.5f, 0),  Quaternion.identity);
            text.GetComponent<TextMesh>().text = col.gameObject.GetComponent<Shot>().damage + " ";
            Destroy(text, 1);
            text.GetComponent<TextMesh>().color = new Color(255f, 0, 0, 255f);
            if(health <= 0){
                 PlayerPrefs.DeleteAll();
                 saveGlobal();
                 Debug.Log("All data reset");
                 SceneManager.LoadScene("SampleScene");
                 loadGlobal();
                
            
                
            }
              
        } 
         
    }    
    void OnCollisionEnter2D(Collision2D col){
        
        if(col.gameObject.tag == "ShotEnemy"){
            if(shipAbsorb1){
                energy += col.gameObject.GetComponent<Shot>().damage;
                if(energy > maxEnergy){
                    energy = maxEnergy;
                }
                
            }
            if(shipReflect1){
                col.gameObject.GetComponent<Rigidbody2D>().velocity = -160f*col.gameObject.GetComponent<Rigidbody2D>().velocity*col.gameObject.GetComponent<Rigidbody2D>().mass;
                col.gameObject.tag = "Shot";
                Physics2D.IgnoreCollision( GetComponent<Collider2D>(), col.gameObject.GetComponent<Shot>().ignored.GetComponent<Collider2D>(), false );
            }
            float dam = col.gameObject.GetComponent<Shot>().damage;
            dam -= armour;
            if(dam < 1){
                dam = 1;
            }
            health -= dam;
            
            
            GameObject text = Instantiate(Resources.Load<GameObject>("TextPrefab"), transform.position + new Vector3(0, -0.5f, 0),  Quaternion.identity);
            text.GetComponent<TextMesh>().text = dam + " ";
            Destroy(text, 1);
            text.GetComponent<TextMesh>().color = new Color(255f, 0, 0, 255f);
            if(health <= 0){
                 PlayerPrefs.DeleteAll();
                 saveGlobal();
                 Debug.Log("All data reset");
                 SceneManager.LoadScene("SampleScene");
                 loadGlobal();
                
            
                
            }
          
        }
        if(col.gameObject.tag == "Asteroid"){
            Vector3 vel = gameObject.GetComponent<Rigidbody2D>().velocity - col.gameObject.GetComponent<Rigidbody2D>().velocity;
            
            Debug.Log(Vector3.Distance(vel, new Vector3(0,0,0)));
            if(Vector3.Distance(vel, new Vector3(0,0,0)) > 0.1f){
                health -= 2;
            }
            
        }
    
    }
    
    public void QuickCash(){
        metal += 1000;
        gainXP(200);
        money += 50;
    }
    
    
    public void QuickKills(){
        killCount += 100;
    }
    
    private void OnApplicationQuit()
    {
        Save();
        saveGlobal();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus){
            Save();
            saveGlobal();
        }else{
            
            
            Load();
            loadGlobal();
        }
        
    }
    
    
    public void Save(){
        Debug.Log("Save called");
        PlayerPrefs.SetInt("saved", 2);
        PlayerPrefs.SetString("close_time", DateTime.Now.ToBinary().ToString());
        
        PlayerPrefs.SetFloat("weaponDamage", weaponDamage);
        PlayerPrefs.SetFloat("health", health);
        PlayerPrefs.SetFloat("energy", energy);
        PlayerPrefs.SetFloat("fuel", fuel);
        PlayerPrefs.SetFloat("maxHealth", maxHealth);
        PlayerPrefs.SetFloat("maxEnergy", maxEnergy);
        PlayerPrefs.SetFloat("maxFuel", maxFuel);
        
        PlayerPrefs.SetFloat("baseMoveSpeed", baseMoveSpeed);
        PlayerPrefs.SetFloat("baseBreakSpeed", baseBreakSpeed);
        PlayerPrefs.SetFloat("turningRate", turningRate);
      
        PlayerPrefs.SetInt("metal", metal);
        PlayerPrefs.SetFloat("armour", armour);
        
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("xp", xp);
        PlayerPrefs.SetInt("levelPoints", levelPoints);
        PlayerPrefs.SetInt("stage", stage);
        
        
        PlayerPrefs.SetFloat("weaponFireCost", weaponFireCost);
        PlayerPrefs.SetFloat("weaponMiningValue", weaponMiningValue);
        PlayerPrefs.SetFloat("weaponDoubleShotChance", weaponDoubleShotChance);
        
        PlayerPrefs.SetInt("killCount", killCount);
        PlayerPrefs.SetFloat("shipKillCharge", shipKillCharge);
        
        PlayerPrefs.SetFloat("energyRechargeRate", energyRechargeRate);
        PlayerPrefs.SetFloat("thrusterUseCost", thrusterUseCost);
        
    


        // Upgrades
        
        PlayerPrefs.SetInt("weaponDamage1", weaponDamage1?1:0);
        PlayerPrefs.SetInt("weaponDamage2", weaponDamage2?1:0);
        PlayerPrefs.SetInt("weaponCost1", weaponCost1?1:0);
        PlayerPrefs.SetInt("weaponEnergyReturn1", weaponEnergyReturn1?1:0);
        PlayerPrefs.SetInt("weaponDoubleShot1", weaponDoubleShot1?1:0);
        PlayerPrefs.SetInt("weaponMV1", weaponMV1?1:0);
        PlayerPrefs.SetInt("weaponMV2", weaponMV2?1:0);
        PlayerPrefs.SetInt("weaponFreeShot1", weaponFreeShot1?1:0);
        PlayerPrefs.SetInt("weaponMVDouble1", weaponMVDouble1?1:0);
        PlayerPrefs.SetInt("weaponLifeLeach1", weaponLifeLeach1?1:0);
        PlayerPrefs.SetInt("weaponBouncyShot1", weaponBouncyShot1?1:0);
        
        
        PlayerPrefs.SetInt("shipMaxEnergy1", shipMaxEnergy1?1:0);
        PlayerPrefs.SetInt("shipMaxEnergy2", shipMaxEnergy2?1:0);
        PlayerPrefs.SetInt("shipRecharge1", shipRecharge1?1:0);
        PlayerPrefs.SetInt("shipThrusterCost1", shipThrusterCost1?1:0);
        PlayerPrefs.SetInt("shipThrusterBoost1", shipThrusterBoost1?1:0);
        PlayerPrefs.SetInt("shipArmour1", shipArmour1?1:0);
        PlayerPrefs.SetInt("shipTurnRate1", shipTurnRate1?1:0);
        PlayerPrefs.SetInt("shipMaxHealth1", shipMaxHealth1?1:0);
        PlayerPrefs.SetInt("shipHealAmount1", shipHealAmount1?1:0);
        PlayerPrefs.SetInt("shipRecharge1", shipRecharge1?1:0);
        PlayerPrefs.SetInt("shipReflect1", shipReflect1?1:0);
        PlayerPrefs.SetInt("shipAbsorb1", shipAbsorb1?1:0);
        PlayerPrefs.SetInt("shipEnergyKill1", shipEnergyKill1?1:0);
        PlayerPrefs.SetInt("shipDoubleXP1", shipDoubleXP1?1:0);
        PlayerPrefs.SetInt("shipBlock1", shipBlock1?1:0);
        PlayerPrefs.SetInt("shipThrusterBoost2", shipThrusterBoost2?1:0);
        
   
    } 
    
    public void ResetAll(){
        PlayerPrefs.DeleteAll();
        Debug.Log("All data reset");
        SceneManager.LoadScene("SampleScene");
        Pause(false);
        
        
    }
    
    public void saveGlobal(){
        PlayerPrefs.SetInt("money", money);
        
        
        PlayerPrefs.SetInt("itemDoubleShot1", itemDoubleShot1?1:0);
        PlayerPrefs.SetInt("itemBossDamage1", itemBossDamage1?1:0);
        PlayerPrefs.SetInt("itemAstroidSpawn1", itemAstroidSpawn1?1:0);
        PlayerPrefs.SetInt("itemDoubleCargo1", itemDoubleCargo1?1:0);
    }
    
    public void loadGlobal(){
        money = PlayerPrefs.GetInt("money");
        
        itemDoubleShot1 = PlayerPrefs.GetInt("itemDoubleShot1")==1?true:false;
        itemBossDamage1 = PlayerPrefs.GetInt("itemBossDamage1")==1?true:false;
        itemAstroidSpawn1 = PlayerPrefs.GetInt("itemAstroidSpawn1")==1?true:false;
        itemDoubleCargo1 = PlayerPrefs.GetInt("itemDoubleCargo1")==1?true:false;
        shopCanvas.SetActive(true);
        if(itemDoubleShot1){
           GameObject.Find("ItemDoubleShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
        }
        if(itemBossDamage1){
            GameObject.Find("ItemBossDamage1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(itemAstroidSpawn1){
            astroidSpawnRate = 3f;
            astroidMax = 15;
            GameObject.Find("ItemAstroidSpawn1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(itemDoubleCargo1){
            GameObject.Find("ItemDoubleCargo1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        shopCanvas.SetActive(false);
        
    }
    public void Load(){
        Debug.Log("Load called");
        
        if(PlayerPrefs.GetInt("saved") != 2){
            Debug.Log("No save to load");
            startCanvas.SetActive(true);
            return;
        }
        
        float seconds= (float)DateTime.Now.Subtract(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("close_time")))).TotalSeconds;
        
        
        weaponDamage = PlayerPrefs.GetFloat("weaponDamage");
        health = PlayerPrefs.GetFloat("health");
        energy = PlayerPrefs.GetFloat("energy");
        fuel = PlayerPrefs.GetFloat("fuel");
        maxHealth = PlayerPrefs.GetFloat("maxHealth");
        maxEnergy = PlayerPrefs.GetFloat("maxEnergy");
        maxFuel = PlayerPrefs.GetFloat("maxFuel");
        
        baseMoveSpeed = PlayerPrefs.GetFloat("baseMoveSpeed");
        baseBreakSpeed = PlayerPrefs.GetFloat("baseBreakSpeed");
        turningRate = PlayerPrefs.GetFloat("turningRate");
        
     
        metal = PlayerPrefs.GetInt("metal");
        armour = PlayerPrefs.GetFloat("armour");
              
        level = PlayerPrefs.GetInt("level");
        xp = PlayerPrefs.GetInt("xp");
        levelPoints = PlayerPrefs.GetInt("levelPoints");
        stage = PlayerPrefs.GetInt("stage");
        
        weaponFireCost = PlayerPrefs.GetFloat("weaponFireCost");
        weaponMiningValue = PlayerPrefs.GetFloat("weaponMiningValue");
        weaponDoubleShotChance = PlayerPrefs.GetFloat("weaponDoubleShotChance");
        
        killCount = PlayerPrefs.GetInt("killCount");
        shipKillCharge = PlayerPrefs.GetFloat("shipKillCharge");
        
        energyRechargeRate = PlayerPrefs.GetFloat("energyRechargeRate");
        thrusterUseCost = PlayerPrefs.GetFloat("thrusterUseCost");
        
        
        // Offline tick
        if(fuel < 20){
            fuel += seconds*5/60;
            if(fuel > 20){
                fuel = 20;
            }
        }
        
        energy += seconds*energyRechargeRate/20;
        if(energy > maxEnergy){
            energy = maxEnergy;
        }
        
       

        // Upgrades
        
        weaponDamage1 = PlayerPrefs.GetInt("weaponDamage1")==1?true:false;
        
        
        weaponDamage2 = PlayerPrefs.GetInt("weaponDamage2")==1?true:false;
        weaponCost1 = PlayerPrefs.GetInt("weaponCost1")==1?true:false;
        weaponEnergyReturn1 = PlayerPrefs.GetInt("weaponEnergyReturn1")==1?true:false;
        weaponDoubleShot1 = PlayerPrefs.GetInt("weaponDoubleShot1")==1?true:false;
        weaponMV1 = PlayerPrefs.GetInt("weaponMV1")==1?true:false;
        weaponMV2 = PlayerPrefs.GetInt("weaponMV2")==1?true:false;
        weaponFreeShot1 = PlayerPrefs.GetInt("weaponFreeShot1")==1?true:false;
        weaponMVDouble1 = PlayerPrefs.GetInt("weaponMVDouble1")==1?true:false;
        weaponLifeLeach1 = PlayerPrefs.GetInt("weaponLifeLeach1")==1?true:false;
        weaponBouncyShot1 = PlayerPrefs.GetInt("weaponBouncyShot1")==1?true:false;
        
        
        shipMaxEnergy1 = PlayerPrefs.GetInt("shipMaxEnergy1")==1?true:false;
        shipMaxEnergy2 = PlayerPrefs.GetInt("shipMaxEnergy2")==1?true:false;
        shipRecharge1 = PlayerPrefs.GetInt("shipRecharge1")==1?true:false;
        shipThrusterCost1 = PlayerPrefs.GetInt("shipThrusterCost1")==1?true:false;
        shipThrusterBoost1 = PlayerPrefs.GetInt("shipThrusterBoost1")==1?true:false;
        shipArmour1 = PlayerPrefs.GetInt("shipArmour1")==1?true:false;
        shipTurnRate1 = PlayerPrefs.GetInt("shipTurnRate1")==1?true:false;
        shipMaxHealth1 = PlayerPrefs.GetInt("shipMaxHealth1")==1?true:false;
        shipHealAmount1 = PlayerPrefs.GetInt("shipHealAmount1")==1?true:false;
        shipRecharge2 = PlayerPrefs.GetInt("shipRecharge2")==1?true:false;
        shipReflect1 = PlayerPrefs.GetInt("shipReflect1")==1?true:false;
        shipAbsorb1 = PlayerPrefs.GetInt("shipAbsorb1")==1?true:false;
        shipEnergyKill1 = PlayerPrefs.GetInt("shipEnergyKill1")==1?true:false;
        shipDoubleXP1 = PlayerPrefs.GetInt("shipDoubleXP1")==1?true:false;
        shipBlock1 = PlayerPrefs.GetInt("shipBlock1")==1?true:false;
        shipThrusterBoost2 = PlayerPrefs.GetInt("shipThrusterBoost2")==1?true:false;
        
    
        upgradeCanvas.SetActive(true);
        shipUpgrades.SetActive(true);
        weaponUpgrades.SetActive(true);
        
    
        if(weaponDamage1){
            GameObject.Find("WeaponDamage1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponDamage2){   
            GameObject.Find("WeaponDamage2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponCost1){
            GameObject.Find("WeaponCost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);  
        }
        
        if(weaponDoubleShot1){
            GameObject.Find("WeaponDoubleShot1").GetComponent<Image>().color =  new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(weaponEnergyReturn1){
            GameObject.Find("WeaponEnergyReturn1").GetComponent<Image>().color =  new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(weaponMV1){
            GameObject.Find("WeaponMV1").GetComponent<Image>().color =  new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponMV2){
            GameObject.Find("WeaponMV2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponFreeShot1){
            GameObject.Find("WeaponFreeShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponMVDouble1){
            GameObject.Find("WeaponMVDouble1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponLifeLeach1){
            GameObject.Find("WeaponLifeLeach1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(weaponBouncyShot1){
            GameObject.Find("WeaponBouncyShot1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipMaxEnergy1){
            GameObject.Find("ShipMaxEnergy1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }

        if(shipMaxEnergy2){
            GameObject.Find("ShipMaxEnergy2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipRecharge1){
            GameObject.Find("ShipRecharge1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipTurnRate1){
            GameObject.Find("ShipTurnRate1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipThrusterCost1){
            GameObject.Find("ShipThrusterCost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }

        if(shipArmour1){
            GameObject.Find("ShipArmour1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f); 
            armour1.SetActive(true);
            armour2.SetActive(true);
            
        }
        
        if(shipThrusterBoost1){
            GameObject.Find("ShipThrusterBoost1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipMaxHealth1){
            GameObject.Find("ShipMaxHealth1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipHealAmount1){
             GameObject.Find("ShipHealAmount1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipRecharge2){
             GameObject.Find("ShipRecharge2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        
        if(shipReflect1){
             GameObject.Find("ShipReflect1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipAbsorb1){
             GameObject.Find("ShipAbsorb1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipEnergyKill1){
             GameObject.Find("ShipEnergyKill1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipDoubleXP1){
             GameObject.Find("ShipDoubleXP1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipBlock1){
            GameObject.Find("ShipBlock1").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }
        if(shipThrusterBoost2){
            GameObject.Find("ShipThrusterBoost2").GetComponent<Image>().color = new Color(0f, 47f/255f, 255f/255f, 1f);
        }

        
        
        upgradeCanvas.SetActive(false);
        shipUpgrades.SetActive(false);
        weaponUpgrades.SetActive(false);
        
    }
    
    public void initGame(int difficulty){
        if(difficulty == 1){
            weaponFireCost = 4;
            health = 100f;
            maxHealth = 100f;
            maxEnergy = 140f;
            energy = 140f;
        }
        if(difficulty == 2){
            weaponFireCost = 5;
            health = 60f;
            maxHealth = 60f;
        }
        startCanvas.SetActive(false);
    }
    
    
    public void tradeMetal(){
        if(metal >= 100){
            metal-=100;
            money+=2;
        }
    }
}
