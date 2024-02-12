using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    private float LightBeamRange;
    [SerializeField] private bool IsLightOn;

    private Light TempLight;

    private Vector3 LightPosition;
    private Vector3 LightDirection;

    private GameObject Shadow;
    private Mesh ShadowMesh;
    private MeshCollider ShadowCollider;
    private MeshFilter ShadowMeshFilter;
    private MeshRenderer ShadowMeshRenderer;
    private Material ShadowMeshMaterial;
    [SerializeField] private float ShadowDepth = 0.1f;
    

    // Start is called before the first frame update
    private void Awake()
    {
        this.TempLight = GetComponentInChildren<Light>();
        this.LightPosition = transform.GetChild(1).position;
        this.LightDirection = transform.GetChild(1).forward;
        this.LightBeamRange = this.TempLight.range;

        this.Shadow = new GameObject("CustomCollider");
        this.ShadowCollider = this.Shadow.AddComponent<MeshCollider>();
        this.ShadowMeshFilter = this.Shadow.AddComponent<MeshFilter>();
        
        this.ShadowMeshRenderer = this.Shadow.AddComponent<MeshRenderer>();
        this.ShadowMeshRenderer.material = this.ShadowMeshMaterial;
        
        this.ShadowCollider.convex = true;
        this.ShadowCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        
        EventManager.Instance.FOnDimensionSwitch += OnDimensionSwitch;
    }
    
    private void FixedUpdate()
    {
        if (this.IsLightOn) RaycastFunction();
    }

    private void RaycastFunction()
    {
        // Raycast to Get only ShadowWall and ShadowObject
        RaycastHit[] raycastHit = Physics.RaycastAll(this.LightPosition, this.LightDirection, this.LightBeamRange,
            ((1 << LayerMask.NameToLayer("ShadowWall")) | (1 << LayerMask.NameToLayer("ShadowObject"))));

        if (!CheckIfLayerHit(raycastHit, LayerMask.NameToLayer("ShadowWall"))) return;
        if(!CheckIfLayerHit(raycastHit, LayerMask.NameToLayer("ShadowObject"))) return;
        
        RaycastHit objectHit = GetLayerObjectHit(raycastHit, LayerMask.NameToLayer("ShadowObject"));
        
        Vector3[] meshLocalVertices = GetMeshLocalVertices(objectHit);

        Vector3[] meshWorldVertices = ConvertLocalToWorldVertices(meshLocalVertices, objectHit.collider.transform);
        
        // If I want the vertices in the world space
        //Vector3[] verticesOnWall = RemoveDuplicates(meshWorldVertices);
        Vector3[] verticesOnWall = ComputeVerticesOnWall(RemoveDuplicates(meshWorldVertices));
        
        Vector3[] verticesToGround = CreateGroundVertices(verticesOnWall, objectHit.point);
        
        Vector3[] depthVertices = CreateDepthVertices(verticesToGround, objectHit.normal);
        
        SetNewMesh(depthVertices);
        
    }

    private Vector3[] GetMeshLocalVertices(RaycastHit objectHit)
    {
        GetAllMeshFilter script = objectHit.collider.gameObject.GetComponent<GetAllMeshFilter>();
        List<MeshFilter> meshList = script.GetMeshFilters();

        List<Vector3> listOfAllMesh = new List<Vector3>();

        foreach (MeshFilter meshFilter in meshList)
        {
            foreach (Vector3 vertex in meshFilter.mesh.vertices)
            {
                listOfAllMesh.Add(vertex);
            }
        }

        return listOfAllMesh.ToArray();
    }

    private bool CheckIfLayerHit(IEnumerable<RaycastHit> raycastHits, int numberOfLayer)
    {
        bool isThereALayer = false;

        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.gameObject.layer == numberOfLayer) isThereALayer = true;
        }

        return isThereALayer;
    }
    
    RaycastHit GetLayerObjectHit(IEnumerable<RaycastHit> raycastHits, int numberOfLayer)
    {
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.gameObject.layer == numberOfLayer) return hit;
        }

        return new RaycastHit();
    }
    
    private Vector3[] ConvertLocalToWorldVertices(Vector3[] meshWorldVertices, Transform objectTransform)
    {
        List<Vector3> listOfVertices = new List<Vector3>();
        
        foreach (Vector3 point in meshWorldVertices)
        {
            Matrix4x4 localToWorld = objectTransform.localToWorldMatrix;
            Vector3 worldVertex = localToWorld.MultiplyPoint3x4(point);

            listOfVertices.Add(worldVertex);
        }

        return listOfVertices.ToArray();
    }
    
    private Vector3[] ComputeVerticesOnWall(Vector3[] meshWorldVertices)
    {
        List<Vector3> listOfVertices = new List<Vector3>();
        
        foreach (Vector3 point in meshWorldVertices)
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

    private Vector3[] RemoveDuplicates(Vector3[] array)
    {
        HashSet<Vector3> uniqueSet = new HashSet<Vector3>();

        List<Vector3> uniqueList = new List<Vector3>();
        foreach (Vector3 vector in array)
        {
            if (uniqueSet.Add(vector))
            {
                uniqueList.Add(vector);
            }
        }

        return uniqueList.ToArray();
    }

    private Vector3[] CreateGroundVertices(Vector3[] vertices, Vector3 hitPosition)
    {
        List<Vector3> tempVertices = new List<Vector3>();

        foreach (Vector3 vertex in vertices)
        {
            tempVertices.Add(vertex);

            if (Physics.Raycast(vertex, Vector3.down, out RaycastHit hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Ground"))))
            {
                tempVertices.Add(hit.point);
            }
        }

        return tempVertices.ToArray();
    }
    
    // Nur eine ausgelagerte Funktion, die nicht mehr gebraucht wird
    void CreateTriangles(Vector3[] vertices)
    {
        int[] triangles = new int[(vertices.Length - 2) * 3];

        for (int i = 0; i < vertices.Length - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
    }
    
    Vector3[] CreateDepthVertices(Vector3[] vertices, Vector3 wallNormal)
    {
        List<Vector3> tempVertices = new List<Vector3>();

        foreach (Vector3 vertex in vertices)
        {
            tempVertices.Add(vertex);
            tempVertices.Add(vertex + wallNormal * this.ShadowDepth);
        }

        return tempVertices.ToArray();
    }

    void SetNewMesh(Vector3[] depthVertices)
    {
        Mesh shadowMesh = new Mesh();
        
        shadowMesh.vertices = depthVertices;
        //shadowMesh.triangles = triangles;
        
        shadowMesh.RecalculateNormals();

        this.ShadowMeshFilter.mesh = shadowMesh;

        this.ShadowCollider.sharedMesh = shadowMesh;
    }
    
    private void OnDimensionSwitch()
    {
        this.IsLightOn = !this.IsLightOn;
    }
}