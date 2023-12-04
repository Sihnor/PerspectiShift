using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public float LightBeamRange = 0;
    public float LightSpotAngle = 0;
    public bool IsLightOn = false;

    private Light TempLight;

    //[SerializeField] private Transform StartRayPosition;
    private Vector3 LightPosition;

    private bool NurEinmal = false;
    
    // Start is called before the first frame update
    private void Awake()
    {
        this.TempLight = GetComponentInChildren<Light>();
        this.LightPosition = transform.GetChild(1).position;
        this.LightBeamRange = this.TempLight.range;
        this.LightSpotAngle = this.TempLight.spotAngle;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (this.IsLightOn) RaycastFunction();
    }

    private void RaycastFunction()
    {
        // Raycast to Get only ShadowWall and ShadowObject
        RaycastHit[] raycastHit = Physics.RaycastAll(this.LightPosition, transform.forward, this.LightBeamRange, 
            ((1 << LayerMask.NameToLayer("ShadowWall")) | (1 << LayerMask.NameToLayer("ShadowObject"))));
        
        if (!CheckIfWall(raycastHit)) return;
        
        RaycastHit objectHit = GetShadowObjectHit(raycastHit);
        if (objectHit.collider == null) return;
        

        Vector3[] meshLocalVertices = objectHit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        
        List<Vector3> meshWorldVertices = new List<Vector3>();
        

        foreach (Vector3 point in meshLocalVertices)
        {
            Matrix4x4 localToWorld = objectHit.collider.transform.localToWorldMatrix;
            Vector3 worldVertex = localToWorld.MultiplyPoint3x4(point);
            
            meshWorldVertices.Add(worldVertex);
        }

        List<Vector3> verticesOnWall = new List<Vector3>();
        
        // Get Points on Wall
        foreach (Vector3 point in meshWorldVertices)
        {
            Vector3 vectorLightToVertex = point - this.LightPosition;
            
            //newVertices.Add(vectorLightToVertex);

            RaycastHit hit;
            Physics.Raycast(this.LightPosition, vectorLightToVertex.normalized, out hit, this.LightBeamRange, (1 << LayerMask.NameToLayer("ShadowWall")));

            if (hit.collider != null)
            {
                Debug.DrawLine(this.LightPosition, hit.point, Color.magenta, .01f);
                verticesOnWall.Add(hit.point);
            }
        }

        foreach (var VARIABLE in verticesOnWall)
        {
            Debug.Log(VARIABLE.ToString());
        }
        
        
        if (!this.NurEinmal)
        {
            this.NurEinmal = true;
            
            GameObject colliderObject = new GameObject("CustomCollider");
        
            MeshFilter meshFilter = colliderObject.AddComponent<MeshFilter>();
        
            Mesh mesh = new Mesh();
            mesh.vertices = verticesOnWall.ToArray();
        
            int[] triangles = new int[verticesOnWall.Count];
        
            for (int i = 0; i < verticesOnWall.Count; i++)
            {
                triangles[i] = i;
            }
        
            mesh.triangles = triangles;
        
            meshFilter.mesh = mesh;
            
            MeshCollider meshCollider = colliderObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }
        
    }
    
    private bool CheckIfWall(RaycastHit[] _raycastHit)
    {
        bool isThereAWall = false;

        foreach (RaycastHit hit in _raycastHit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ShadowWall")) isThereAWall = true;

        }

        return isThereAWall;
    }

    RaycastHit GetShadowObjectHit(RaycastHit[] _raycastHit)
    {
        foreach (RaycastHit hit in _raycastHit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ShadowObject")) return hit;
        }

        return new RaycastHit();
    }
}