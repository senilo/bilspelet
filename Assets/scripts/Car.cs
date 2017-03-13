using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;


public class Car : MonoBehaviour {
    public float speed;
    public int points;
    HashSet<BuildingType> wrongBuilding;
    float upsideDownCounter;
    float underWaterCounter;
    // Use this for initialization
    void Start () {
        upsideDownCounter = 0;
        underWaterCounter = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas);
        transform.position = hit.position;
        wrongBuilding = new HashSet<BuildingType>();
    }
	
	// Update is called once per frame
	void Update () {

        if(transform.position.y < Game.instance.waterLevel)
        {
            underWaterCounter += Time.deltaTime;
            if (underWaterCounter > 5f)
            {
                resetPosition();
            }
        } else
        {
            underWaterCounter = 0;
        }

        if (transform.up.y < 0)
        {
            upsideDownCounter += Time.deltaTime;
            if (upsideDownCounter > 5f)
            {
                resetPosition();
            }
        }
        else
        {
            upsideDownCounter = 0;
        }
    }

    private void resetPosition()
    {

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas);
        NavMesh.SamplePosition(hit.position + (hit.position - transform.position).normalized * 10f,out hit, Mathf.Infinity, NavMesh.AllAreas);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        var newPos = hit.position;
        newPos.y += 10f;
        transform.position = newPos;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var building = other.GetComponent<Building>();
        if (building != null)
        {
            if (building.buildingType == Game.instance.nextTarget)
            {
                Game.instance.targetReached();
                wrongBuilding.Clear();
                wrongBuilding.Add(building.buildingType);
                points++;
            } else
            {
                if (wrongBuilding.Add(building.buildingType))
                {
                    Sounds.instance.PlayRandom(Sounds.instance.nej);
                }
            }
        }
    }
}
