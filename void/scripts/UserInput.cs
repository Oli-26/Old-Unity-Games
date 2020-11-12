using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInput : MonoBehaviour {
    public GameObject playerShip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		takeInput();
	}
    
    void takeInput() {
        foreach (Touch touch in Input.touches) {
            if (touch.fingerId == 0 && !IsTapOverUIObject()) {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                playerShip.GetComponent<PlayerShip>().Turn(targetPosition);
                playerShip.GetComponent<PlayerShip>().Move();
            } 
        }
        //mouseInput(); //To be disabled in phone builds
        
    }
    
    
    void mouseInput(){
        if(Input.GetMouseButton(1)){
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            playerShip.GetComponent<PlayerShip>().Turn(targetPosition);
            playerShip.GetComponent<PlayerShip>().Move();
        }
    }
    private bool IsTapOverUIObject() {
         Touch touch = Input.GetTouch(0);
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
         eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
         return results.Count > 0;
    }
    
    
    public void itemPressed(int i){
        PlayerShip shipScript = playerShip.GetComponent<PlayerShip>();
        Invent invScript = playerShip.GetComponent<Invent>();
        if(invScript.checkSlot(i)){
            Item item = invScript.getSlot(i);
            switch(item.type){
                case "shooter":
                    item.setShip(playerShip); // Inefficient 
                    ((Shooter)item).activate();
                    break;
                case "default":
                default: 
                    item.setShip(playerShip); // Inefficient 
                    ((Shooter)item).activate();
                    break;
            }
                
            
        }
    }
    
    public void itemUpgrade(int i){
        PlayerShip shipScript = playerShip.GetComponent<PlayerShip>();
        Invent invScript = playerShip.GetComponent<Invent>();
        if(invScript.checkSlot(i)){
            Item item = invScript.getSlot(i);
            switch(item.type){
                case "shooter":
                    if(shipScript.getMetal() >= ((Shooter)item).upgradeCost()){
                        shipScript.spendMetal(((Shooter)item).upgradeCost());
                        ((Shooter)item).upgrade();
                    }
                    break;
                case "default":
                default: 
                    
                    Debug.Log("Default item upgrade not valid");
                    break;
            }
                
            
        }
    }
    
    
    public void itemSell(int i){
        PlayerShip shipScript = playerShip.GetComponent<PlayerShip>();
        Invent invScript = playerShip.GetComponent<Invent>();
        if(invScript.checkSlot(i)){
            Item item = invScript.getSlot(i);
            shipScript.metal += item.getSellValue();
            invScript.emptySlot(i);
        }
    }
}
