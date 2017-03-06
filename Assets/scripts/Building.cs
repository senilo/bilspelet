using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { Hem, Dagis, Jobb, Mammas_jobb, Affär, Sjukhus, Stugan}

public class Building : MonoBehaviour {
    public BuildingType buildingType;

    void OnValidate()
    {
        name = buildingType.ToString();
    }
	// Use this for initialization
	void Start () {
        Game.instance.RegisterBuilding(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {

    }
}
