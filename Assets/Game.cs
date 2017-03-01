using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game instance;

    public Dictionary<BuildingType, GameObject> buildings;
    public BuildingType previousTarget;
    public BuildingType nextTarget;


    public UnityEngine.UI.Text debugText;

    void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
        buildings = new Dictionary<BuildingType, GameObject>();
    }
	// Use this for initialization
	void Start () {
        previousTarget = BuildingType.HOME;
        nextTarget = BuildingType.HOME;
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
        if (previousTarget == nextTarget)
        {
            Debug.Assert(buildings.Keys.Count > 1);
            if (buildings.Keys.Count < 2) return;
            updateTarget();
        }
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
        updateTarget();
    }

    public GameObject getNextTarget()
    {
        return buildings[nextTarget];
    }
}
