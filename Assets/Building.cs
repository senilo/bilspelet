using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { HOME, KINDERGARTEN, WORK, GROCERY_STORE, HOSPITAL }

public class Building : MonoBehaviour {
    public BuildingType buildingType;
	// Use this for initialization
	void Start () {
        Game.instance.RegisterBuilding(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
