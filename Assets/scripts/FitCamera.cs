using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCamera : MonoBehaviour {

    public Transform moveTransform = null;
    public float minDistance = 10.0f;
    public float padding = 0.0f;
    public UnityEngine.Camera settingsCamera = null;

    private List<Transform> fitTransforms = new List<Transform>();
    private List<Vector3> planePoints = new List<Vector3>();

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerCameraFollow");
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i].activeInHierarchy)
                this.fitTransforms.Add(players[i].transform);
        }
        this.fitCamera();
    }

    private void Update()
    {
        this.fitCamera();
    }

    private void fitCamera()
    {
        //Setup camera plane
        Vector3 planeForward = this.moveTransform.forward;
        Vector3 planeRight = this.moveTransform.right;
        Vector3 planeUp = this.moveTransform.up;

        //Find closest point
        Vector3 closestPoint = Vector3.zero;
        float closestDist = Mathf.Infinity;
        planePoints.Clear();
        for (int i = 0; i < this.fitTransforms.Count; ++i)
        {
            Vector3 pos = this.fitTransforms[i].position;
            float dot = Vector3.Dot(planeForward, pos);
            if (dot < closestDist)
            {
                closestPoint = pos;
                closestDist = dot;
            }
        }

        //Find plane min max points
        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;
        for (int i = 0; i < this.fitTransforms.Count; ++i)
        {
            Vector3 relPos = this.fitTransforms[i].position - closestPoint;
            float dot = Vector3.Dot(planeForward, relPos);
            Vector3 planePos = relPos - (planeForward * dot);
            float x = Vector3.Dot(planeRight, planePos);
            float y = Vector3.Dot(planeUp, planePos);
            if (x < min.x) min.x = x;
            if (x > max.x) max.x = x;
            if (y < min.y) min.y = y;
            if (y > max.y) max.y = y;
        }

        //Calculate center position and width/height
        min.x -= this.padding;
        min.y -= this.padding;
        max.x += this.padding;
        max.y += this.padding;

        float width = max.x - min.x;
        float height = max.y - min.y;
        Vector3 position = closestPoint + (planeRight * (min.x + (width * 0.5f))) + (planeUp * (min.y + (height * 0.5f)));

        float targetFrustumDist = Mathf.Max(height, width / this.settingsCamera.aspect) * 0.5f / Mathf.Tan(this.settingsCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        targetFrustumDist = Mathf.Max(targetFrustumDist, this.minDistance);
        this.moveTransform.position = position - (planeForward * targetFrustumDist);
    }
}
