using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public float LightBeamRange = 0;
    public bool IsLightOn = false;

    private Light TempLight;

    private Vector3 LightPosition;

    private GameObject Shadow;
    private Mesh ShadowMesh;
    
    private MeshCollider ShadowCollider;
    private MeshFilter ShadowMeshFilter;

    private MeshRenderer ShadowMeshRenderer;

    public Material ShadowMeshMaterial;

    // Start is called before the first frame update
    private void Awake()
    {
        this.TempLight = GetComponentInChildren<Light>();
        this.LightPosition = transform.GetChild(1).position;
        this.LightBeamRange = this.TempLight.range;

        this.Shadow = new GameObject("CustomCollider");
        this.ShadowCollider = this.Shadow.AddComponent<MeshCollider>();
        this.ShadowMeshFilter = this.Shadow.AddComponent<MeshFilter>();
        //
        this.ShadowMeshRenderer = this.Shadow.AddComponent<MeshRenderer>();
        this.ShadowMeshRenderer.material = this.ShadowMeshMaterial;
        //
        this.ShadowCollider.convex = true;
        this.ShadowCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
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

        
        if (!CheckIfLayerHit(raycastHit, LayerMask.NameToLayer("ShadowWall"))) return;
        if(!CheckIfLayerHit(raycastHit, LayerMask.NameToLayer("ShadowObject"))) return;
        
        
        RaycastHit objectHit = GetLayerObjectHit(raycastHit, LayerMask.NameToLayer("ShadowObject"));

        Vector3[] meshLocalVertices = objectHit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices;

        Vector3[] meshWorldVertices = ConvertLocalToWorldVertices(meshLocalVertices, objectHit.collider.transform);
        
        Vector3[] verticesOnWall = RemoveDuplicates(ComputeVerticesOnWall(meshWorldVertices));



        Mesh shadowMesh = new Mesh();
        
        int[] triangles = new int[(verticesOnWall.Length - 2) * 3];

        for (int i = 0; i < verticesOnWall.Length - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        shadowMesh.vertices = verticesOnWall;
        shadowMesh.triangles = triangles;
        
        shadowMesh.RecalculateNormals();

        this.ShadowMeshFilter.mesh = shadowMesh;

        this.ShadowCollider.sharedMesh = shadowMesh;
    }

    private bool CheckIfLayerHit(IEnumerable<RaycastHit> _raycastHits, int numberOfLayer)
    {
        bool isThereALayer = false;

        foreach (RaycastHit hit in _raycastHits)
        {
            if (hit.collider.gameObject.layer == numberOfLayer) isThereALayer = true;
        }

        return isThereALayer;
    }
    
    RaycastHit GetLayerObjectHit(IEnumerable<RaycastHit> _raycastHits, int numberOfLayer)
    {
        foreach (RaycastHit hit in _raycastHits)
        {
            if (hit.collider.gameObject.layer == numberOfLayer) return hit;
        }

        return new RaycastHit();
    }
    
    private Vector3[] ConvertLocalToWorldVertices(Vector3[] _meshWorldVertices, Transform _objectTransform)
    {
        List<Vector3> listOfVertices = new List<Vector3>();
        
        foreach (Vector3 point in _meshWorldVertices)
        {
            Matrix4x4 localToWorld = _objectTransform.localToWorldMatrix;
            Vector3 worldVertex = localToWorld.MultiplyPoint3x4(point);

            listOfVertices.Add(worldVertex);
        }

        return listOfVertices.ToArray();
    }
    
    private Vector3[] ComputeVerticesOnWall(Vector3[] _meshWorldVertices)
    {
        List<Vector3> listOfVertices = new List<Vector3>();
        
        foreach (Vector3 point in _meshWorldVertices)
        {
            Vector3 vectorLightToVertex = point - this.LightPosition;

            RaycastHit hit;
            Physics.Raycast(this.LightPosition, vectorLightToVertex.normalized, out hit, this.LightBeamRange, (1 << LayerMask.NameToLayer("ShadowWall")));

            if (hit.collider != null)
            {
                Debug.DrawLine(this.LightPosition, hit.point, Color.magenta, .01f);
                listOfVertices.Add(hit.point);
            }
        }

        return listOfVertices.ToArray();
    }

    private Vector3[] RemoveDuplicates(Vector3[] _array)
    {
        HashSet<Vector3> uniqueSet = new HashSet<Vector3>();

        List<Vector3> uniqueList = new List<Vector3>();
        foreach (Vector3 vector in _array)
        {
            if (uniqueSet.Add(vector))
            {
                uniqueList.Add(vector);
            }
        }

        return uniqueList.ToArray();
    }

    void CreateMeshOnWall(Vector3[] _vertices)
    {
    }
}