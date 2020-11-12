using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public Slider healthBar;
    public Slider energyBar;
    public Text healthText;
    public Text energyText;
    public Text notorietyText;
    public GameObject shop;
    public Text metalText;
    public Text metalShopText;
    public Button[] bottomBarButtons = new Button[5];
    public Image[] bottomBarOverlays = new Image[5];
    
    public Image[] shopOverlays = new Image[5];
    public Text[] shopItemNames = new Text[5];
    public Text[] shopItemStats = new Text[5];
    public Text[] shopItemUpgrades = new Text[5];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject ship = this.gameObject.GetComponent<UserInput>().playerShip;
        PlayerShip shipScript = ship.GetComponent<PlayerShip>() as PlayerShip;
        
		updateBars(shipScript);
        updateNotoriety(shipScript);
        updateMetal(shipScript);
        
        loadUIElements(ship);
        
	}
    
    void updateBars(PlayerShip shipScript){
        healthBar.value = shipScript.health/shipScript.maxHealth;
        healthText.text = (Mathf.Round(shipScript.health * 10f) / 10f).ToString();
        energyBar.value = shipScript.energy/shipScript.maxEnergy;
        energyText.text = (Mathf.Round(shipScript.energy * 10f) / 10f).ToString(); 
    }
    
    void updateNotoriety(PlayerShip shipScript){
        notorietyText.text = (Mathf.Round(shipScript.notoriety)).ToString() + "%";
        
    }
    
    void updateMetal(PlayerShip shipScript){
        metalText.text = shipScript.metal.ToString() + " metal";
        metalShopText.text = shipScript.metal.ToString() + " metal";
    }
    
    public void openShop(){
        shop.SetActive(true);
    }
    
    public void closeShop(){
        shop.SetActive(false);
    }
    
    void loadUIElements(GameObject ship){
        Invent invent = ship.GetComponent<Invent>();
        for(int i = 0; i < 5; i++){
            if(invent.checkSlot(i)){
                bottomBarOverlays[i].gameObject.SetActive(true);
                shopOverlays[i].gameObject.SetActive(true);
                bottomBarOverlays[i].sprite = invent.getSlot(i).image;
                shopOverlays[i].sprite = invent.getSlot(i).image;
                shopItemNames[i].text = invent.getSlot(i).getName() + "\nLvl " + invent.getSlot(i).getLevel();
                Item item = invent.getSlot(i);
                switch(item.type){
                case "shooter":
                    shopItemStats[i].text = ((Shooter)item).getStats();
                    shopItemUpgrades[i].text = ((Shooter)item).getUpgradeStats();
                    break;
                case "default":
                default: 
                    shopItemStats[i].text = "child class stats not defined.";
                    shopItemUpgrades[i].text = "child class upgrade stats not defined";
                    break;
            }
            }else{
                bottomBarOverlays[i].gameObject.SetActive(false);
                shopOverlays[i].gameObject.SetActive(false);
                shopItemNames[i].text = "";
                shopItemStats[i].text = "";
                shopItemUpgrades[i].text = "";
            }
        }
        
    }
}
