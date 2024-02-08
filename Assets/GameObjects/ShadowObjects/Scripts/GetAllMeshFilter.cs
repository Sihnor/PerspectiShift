using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllMeshFilter : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> ShadowMeshFilters = new List<MeshFilter>();
    
    public List<MeshFilter> GetMeshFilters()
    {
        return this.ShadowMeshFilters;
    }
}
