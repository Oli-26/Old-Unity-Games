using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Character : MonoBehaviour {
    public GameObject moneyText;
    public GameObject weapon;
    bool weaponUp = true;
    
    public GameObject equiped;
    
    
    string direction = "none";
    bool moving = false;
    int jumpCount = 2;
    int jumpCountMax = 2;
    
    public float speed = 3.7f;
    public float health = 100f;
    public float maxHealth = 100f;
    
    public GameObject healthBar;
    
    GameObject defensePrefab;
    int tempCost = 0;
    bool inConstructMode = false;
    bool inSellMode = false;
    bool defenseHighlighted = false;
    GameObject selectedDefense;
    public GameObject balistaPrefab;
    public GameObject wallPrefab;
    public GameObject repairPrefab;
    
    int tower1Cost = 10;
    int tower2Cost = 15;
    int tower3Cost = 20;
    
    // STATS
    public float tower_power = 0f;
    public float tower_defense = 0f;
    public float tower_range = 0f;
    public float tower_speed = 0f;
    
    public GameObject tower_power_text;
    public GameObject tower_defense_text;
    public GameObject tower_range_text;
    public GameObject tower_speed_text;
    public GameObject skillPointsText;
    
    public GameObject tower_power_effect_text;
    public GameObject tower_defense_effect_text;
    public GameObject tower_range_effect_text;
    public GameObject tower_speed_effect_text;
    public GameObject pop_text;
    
    public GameObject item_display;
    public GameObject item_display_name;
    public GameObject item_display_stats;
    
    public GameObject tower_display;
    public GameObject tower_display_name;
    public GameObject tower_display_stats;
    
    public GameObject[,] invent;
    public bool[,] inventOccupied;
    public GameObject inventImageHolder;
    public int inventSlotSelected;

    
    public int level = 1;
    public int xp = 0;
    public GameObject xpSlider;
    public GameObject levelText;
    
    public int skillPoints = 0;
    // Inventory
    public int money = 0;
    
    public int popMax = 14;
    public int pop = 0;
	// Use this for initialization
	void Start () {
		money = 50;
        popMax = 14;

        invent = new GameObject[4,6];
        inventOccupied = new bool[4,6];
        updateCharacterMenu();
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.GetComponent<Text>().text = money+"$";
		if(moving){
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if(direction == "left"){
                
                rb2D.velocity = new Vector3((transform.right.x * speed * Time.deltaTime * 10), rb2D.velocity.y, 0f);
            }
            if(direction == "right"){
                rb2D.velocity = new Vector3((transform.right.x * speed * Time.deltaTime * 10), rb2D.velocity.y, 0f);
            }
            
        }
        if(inConstructMode){
            moveDefense();
        }
        if(inConstructMode == false){
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p = new Vector3(p.x, p.y, 0f);

            GameObject[] defenses = GameObject.FindGameObjectsWithTag("AgroDefense");
            GameObject closestDefense = null;
            float closestDefenseDistance = 10000;
            bool closestDefenseSet = false;
            bool defenseExists = false;
            foreach(GameObject d in defenses){
                float dist = Vector3.Distance(d.transform.position, p);
                
                if(dist < 0.1f){
                    if(closestDefenseSet){
                        if(dist < closestDefenseDistance){
                            closestDefense.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f);
                            closestDefense = d;
                            closestDefenseSet = true;
                            closestDefenseDistance = dist;
                        }
                    }else{
                        closestDefense = d;
                        closestDefenseSet = true;
                        closestDefenseDistance = dist;
                        defenseExists = true;
                    }
                }else{
                    d.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f);
                }
                
            }
            if(defenseExists){
                try{
                    if(defenseHighlighted){
                        defensePrefab.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f);
                    }
                }catch(Exception e){
                    
                }
                defensePrefab = closestDefense;
                defensePrefab.GetComponent<SpriteRenderer>().color = new Color(50f/255f, 255f/255f, 50f/255f);
                defenseHighlighted = true;
            }
        }
        
	}
    
    public void equipItem(){
        int n = inventSlotSelected;
        if(inventOccupied[n/6,n%6]){
            GameObject g = invent[n/6,n%6];
            equiped = g;

            weapon.GetComponent<Weapon>().Assume(g);
            updateInventDisplay();

        }else{
            return;
        }
        
    }
    
    public void itemStats(int n){
        if(inventOccupied[n/6,n%6]){
            GameObject g = invent[n/6,n%6];
            Weapon wep = g.GetComponent<Weapon>();    
            item_display.SetActive(true);
            item_display_name.GetComponent<Text>().text = wep.type;
            item_display_stats.GetComponent<Text>().text = Mathf.Round(100f*wep.damage)/100f + " base damage\n";
            item_display_stats.GetComponent<Text>().text += Mathf.Round(100f*wep.bonusDamage)/100f + " bonus damage\n";
            item_display_stats.GetComponent<Text>().text += Mathf.Round(100f*wep.bonusReserveMax)/100f + " bonus reserve\n";
            item_display_stats.GetComponent<Text>().text +=  Mathf.Round(100f*wep.chargeRate)/100f + " charge rate\n";
            item_display_stats.GetComponent<Text>().text +=  Mathf.Round(100f*weapon.GetComponent<Weapon>().bonusReserve)/100f + " current charge\n";
            inventSlotSelected = n;
        }else{
            return;
        }
    }
    
    public void Jump(){
        if(jumpCount > 0){
            jumpCount--;
            GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0f, 0f);
            GetComponent<Rigidbody2D>().AddForce(transform.up*3000);
        }
        
    }
    public void setDirection(string dir){
        direction = dir;
        if(direction == "left"){   
            transform.rotation =  Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        if(direction == "right"){
            transform.rotation =  Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
            
    }
    public void setMovement(bool m){
        moving = m;
    }
    
    
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Floor"){
            jumpCount = jumpCountMax;
            return;
        }
        if(col.gameObject.tag == "Coin"){
            Destroy(col.gameObject);
            money += 1;
            return;
        }
        
    }
    
    public void createDefense(string type){
        if(inConstructMode){
            return;
        } 
        switch(type){
            case "1":
                if(money >= tower1Cost && pop + 1 <= popMax){
                    defensePrefab = Instantiate(wallPrefab, gameObject.transform.position, Quaternion.identity);
                    defensePrefab.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
                    inConstructMode = true;
                    tempCost = tower1Cost;
                    pop += 1;
                    updateCharacterMenu();
                }
                break;
            case "2":
                if(money >= tower2Cost && pop + 2 <= popMax){
                    defensePrefab = Instantiate(balistaPrefab, gameObject.transform.position, Quaternion.identity);
                    defensePrefab.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
                    inConstructMode = true;
                    tempCost = tower2Cost;
                    pop += 2;
                    updateCharacterMenu();
                }
                break;
                
            case "3":
                if(money >= tower3Cost && pop + 2 <= popMax){
                    defensePrefab = Instantiate(repairPrefab, gameObject.transform.position, Quaternion.identity);
                    defensePrefab.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
                    defensePrefab.GetComponent<Defense>().setStats(5f, 0.5f, 3f, 40f);
                    inConstructMode = true;
                    tempCost = tower3Cost;
                    pop += 2;
                    updateCharacterMenu();
                }
                break;
            default:
                break;
            
        }
    }
    
    void moveDefense(){
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       
        defensePrefab.transform.position = new Vector3(pos.x, pos.y, 0f);
        
    }
    
    
    public void finishDefense(){
        if(inConstructMode == false){
            selectDefense();
            
        }
        if(inConstructMode && money >= tempCost){
            if(defensePrefab.GetComponent<Defense>().getFloorTouches() > 0){
                inConstructMode = false;
                defensePrefab.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
                defensePrefab.GetComponent<Defense>().setOwner(gameObject);
                defensePrefab.GetComponent<Defense>().finishedBuilding = true;
                defensePrefab.GetComponent<Defense>().cost = tempCost;
                defensePrefab = null;
                money -= tempCost;
            }
        }
    }
    
    public void cancleDefense(){
        if(inConstructMode){
            inConstructMode = false;
            pop -= defensePrefab.GetComponent<Defense>().popValue;
            Destroy(defensePrefab);
        }else{
            if(defenseHighlighted){
                defenseHighlighted = false;
                int sellPrice = (int)Mathf.Round(defensePrefab.GetComponent<Defense>().getHealthPercent()*defensePrefab.GetComponent<Defense>().cost);
                money += sellPrice;
                pop -= defensePrefab.GetComponent<Defense>().popValue;
                Destroy(defensePrefab);
                updateCharacterMenu();
            }
        }
    }
    
    public void rotateDefense(){
        if(inConstructMode){
            defensePrefab.GetComponent<Defense>().Rotate(); // need parents class for generic functions like rotate.
            if(defensePrefab.transform.rotation.y == 0f){
                defensePrefab.transform.rotation =  Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }else{
                defensePrefab.transform.rotation =  Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            
        }
    }
    

    
 
    
    public float getTowerPower(){
        return tower_power;
    }
    public float getTowerRange(){
        return tower_range;
    }
    public float getTowerDefense(){
        return tower_defense;
    }
    public float getTowerSpeed(){
        return tower_speed;
    }
    
    public void swingWeapon(){
        if(weaponUp){
            weapon.transform.Rotate(new Vector3(0f,0f,-30f));
            weaponUp = false;
        }else{
            weapon.transform.Rotate(new Vector3(0f,0f,30f));
            weaponUp = true;
        }
        weapon.GetComponent<Weapon>().active = true;
    }
    
    public void chargeWeapon(bool t){
        if(t){
            weapon.GetComponent<Weapon>().charging = true;
        }else{
            weapon.GetComponent<Weapon>().charging = false;
        }
    }
    public void levelStat(string type){
        
        if(skillPoints > 0){
            skillPoints--;
        
            switch(type){
                case "power":
                    tower_power++;
                    break;
                case "range":
                    tower_range++;
                    break;
                case "health":
                    tower_defense++;
                    break;
                case "speed":
                    tower_speed++;
                    break;
            }
        }
        updateCharacterMenu();
    }
    
    void updateCharacterMenu(){
        tower_power_text.GetComponent<Text>().text = tower_power+"";
        tower_speed_text.GetComponent<Text>().text = tower_speed+"";
        tower_range_text.GetComponent<Text>().text = tower_range+"";
        tower_defense_text.GetComponent<Text>().text = tower_defense+"";
        xpSlider.GetComponent<Slider>().value = ((float)xp)/((float)getXPToLevel());
        levelText.GetComponent<Text>().text = level+"";
        skillPointsText.GetComponent<Text>().text = skillPoints+"";
        tower_power_effect_text.GetComponent<Text>().text =  Mathf.Round(10000f*5*(1 - Mathf.Pow(0.995f, (float)tower_power)))/100f + "%";
        tower_defense_effect_text.GetComponent<Text>().text =  Mathf.Round(10000f*3*(1 - Mathf.Pow(0.98f, (float)tower_defense)))/100f + "%";
        tower_range_effect_text.GetComponent<Text>().text =  Mathf.Round(10000f*(1 - Mathf.Pow(0.96f, (float)tower_range)))/100f + "%";
        tower_speed_effect_text.GetComponent<Text>().text = Mathf.Round(10000f*(1 - Mathf.Pow(0.97f, (float)tower_speed)))/100f + "%";
        pop_text.GetComponent<Text>().text = pop + " / " + popMax;
    }
    
    int getXPToLevel(){
       int xpToLevel = level*100;
       return xpToLevel;
    }
    
    public void gainXP(int g){
        xp+=g;
        
        if(xp > getXPToLevel()){
            xp -= getXPToLevel();
            level += 1;
            skillPoints+=level;
        }
        updateCharacterMenu();
    }
    
    public void selectDefense(){
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p = new Vector3(p.x, p.y, 0f);
        GameObject[] defenses = GameObject.FindGameObjectsWithTag("AgroDefense");
        GameObject closestDefense = null;
        float closestDefenseDistance = 10000;
        bool closestDefenseSet = false;
        bool defenseExists = false;
        foreach(GameObject d in defenses){
            float dist = Vector3.Distance(d.transform.position, p);
            
            if(dist < 0.1f){
                if(closestDefenseSet){
                    if(dist < closestDefenseDistance){
                        closestDefense = d;
                        closestDefenseSet = true;
                        closestDefenseDistance = dist;
                    }
                }else{
                    closestDefense = d;
                    closestDefenseSet = true;
                    closestDefenseDistance = dist;
                    defenseExists = true;
                }
            }
                
        }
        if(defenseExists){
            selectedDefense = closestDefense;
            displayTowerStats();
        }
        
    }
    public void upgradeDefense(){
        try{
            int cost = selectedDefense.GetComponent<Defense>().level*25;
            if(money >= cost){
                money -= cost;
                selectedDefense.GetComponent<Defense>().upgrade();
                displayTowerStats();
            }
        }catch(Exception e){
            
        }
        
    }
    void displayTowerStats(){
        tower_display.SetActive(true);
        tower_display_name.GetComponent<Text>().text = selectedDefense.GetComponent<Defense>().type + "  " + selectedDefense.GetComponent<Defense>().level;
        tower_display_stats.GetComponent<Text>().text = Mathf.Round(100f*selectedDefense.GetComponent<Defense>().generateDamage())/100f + " damage\n";
        tower_display_stats.GetComponent<Text>().text +=  Mathf.Round(100f*selectedDefense.GetComponent<Defense>().generateHealth())/100f + " max health\n";
        tower_display_stats.GetComponent<Text>().text +=  Mathf.Round(100f*selectedDefense.GetComponent<Defense>().generateCoolDown())/100f + " cooldown\n";
        tower_display_stats.GetComponent<Text>().text +=  Mathf.Round(100f*selectedDefense.GetComponent<Defense>().generateRange())/100f + " range\n";
 
    }
    
    
    public int getNextFreeSlot(){
        
        for(int i = 0; i < 4; i++){
            for(int j = 0; j < 6; j++){
                if(inventOccupied[i,j] == false){
                    return i*4+j;
                }
            }
        }
        return -1;
    }
    
    public void pickUpItem(GameObject g){
        int n = getNextFreeSlot();
        if(n == -1){
            Debug.Log("No free space in inventory");
            return;
        }else{
            invent[n/6,n%6] = g;
            inventOccupied[n/6, n%6] = true;
            g.SetActive(false);
        }
        updateInventDisplay();
    }
    
    void updateInventDisplay(){
        int i = 0;
        int j = 0;
        foreach (Transform child in inventImageHolder.transform)
        {
            if(inventOccupied[i,j]){
                
                child.GetComponent<Image>().sprite = invent[i,j].GetComponent<SpriteRenderer>().sprite;
                if(equiped == invent[i,j]){
                    child.GetComponent<Image>().color = new Color(50f/255f, 250f/255f, 50f/255f);
                }else{
                    child.GetComponent<Image>().color = new Color(255f/255f, 255f/255f, 255f/255f);
                }
                    
            }
            j++;
            if(j>5){
                j=0;
                i++;
            }
        }
       
    }
    void updateHealthBar(){
        healthBar.GetComponent<Slider>().value = health/maxHealth;
    }
    
    public void takeDamage(float d){
        health -= d;
        updateHealthBar();
    }
   
}
