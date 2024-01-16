using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CellMesh : MonoBehaviour {
    
    private float cellSize;
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        cellSize = GetComponent<Cell>().GetCellSize();
        
        CreateMesh();
    }

    private void CreateMesh() {
        mesh = GetComponent<MeshFilter>().mesh = new Mesh {
            name = "Hex Mesh"
        };

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        vertices.Add(Vector3.zero);
        vertices.Add(new Vector3(0f, 0f,cellSize / 2));
        vertices.Add(new Vector3(Mathf.Cos(Mathf.Deg2Rad * 30f) * cellSize / 2, 0f,cellSize / 4));
        vertices.Add(new Vector3(Mathf.Cos(Mathf.Deg2Rad * 30f) * cellSize / 2, 0f,-cellSize / 4));
        vertices.Add(new Vector3(0f, 0f,-cellSize / 2));
        vertices.Add(new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 30f) * cellSize / 2, 0f,-cellSize / 4));
        vertices.Add(new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 30f) * cellSize / 2, 0f,cellSize / 4));
        
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);
        
        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(4);
        
        triangles.Add(0);
        triangles.Add(4);
        triangles.Add(5);
        
        triangles.Add(0);
        triangles.Add(5);
        triangles.Add(6);
        
        triangles.Add(0);
        triangles.Add(6);
        triangles.Add(1);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    // public void Init(float cellSize) {
    //     this.cellSize = cellSize;
    //     meshRenderer = GetComponent<MeshRenderer>();
    //     
    //     CreateMesh();
    // }

    public void ChangeMeshColor(Color newColor) {
        meshRenderer.material.color = newColor;
    }
    
    
}
