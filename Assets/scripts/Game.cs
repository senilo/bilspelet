using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game instance;

    public Dictionary<BuildingType, GameObject> buildings;
    public BuildingType previousTarget;
    public BuildingType nextTarget;
    public GameObject water;
    //[HideInInspector]
    public float waterLevel;
    public UnityEngine.UI.Text debugText;

    void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
        buildings = new Dictionary<BuildingType, GameObject>();
    }
	// Use this for initialization
	void Start () {
        previousTarget = BuildingType.Hem;
        updateTarget();
        Sounds.instance.PlayRandom(nextTarget, 2f);
        waterLevel = water.transform.position.y - 2f;
    }



    void updateTarget()
    {
        var a = Enum.GetValues(typeof(BuildingType));
        Debug.Assert(a.Length > 1);
        do
        {
            nextTarget = (BuildingType)a.GetValue(UnityEngine.Random.Range(0, a.Length));
        } while (nextTarget == previousTarget || !buildings.ContainsKey(nextTarget));
        debugText.text = "Next target: " + nextTarget.ToString();
    }


	
	// Update is called once per frame
	void Update () {

    }

    public void RegisterBuilding(GameObject g)
    {
        var b = g.GetComponent<Building>();
        Debug.Assert(!buildings.ContainsKey(b.buildingType), "Multiple building of type: " + b.buildingType.ToString());
        buildings[b.buildingType] = g;
    }

    public void targetReached()
    {
        previousTarget = nextTarget;
        Sounds.instance.PlayRandom(Sounds.instance.bra);
        updateTarget();


        Sounds.instance.PlayRandom(nextTarget, 2f);
    }

    public GameObject getNextTarget()
    {
        return buildings[nextTarget];
    }
}
