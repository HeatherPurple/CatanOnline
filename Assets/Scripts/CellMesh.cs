using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CellMesh : MonoBehaviour {
    
    private float cellSize;
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private List<Vector3> vertices;
    private List<int> triangles;
   

    private void CreateMesh() {
        mesh = GetComponent<MeshFilter>().mesh = new Mesh {
            name = "Hex Mesh"
        };
        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        Vector3 position = transform.position;
        
        vertices.Add(new Vector3(position.x ,position.y, position.z));
        vertices.Add(new Vector3(position.x, position.y,position.z + cellSize / 2));
        vertices.Add(new Vector3(position.x + Mathf.Sqrt(cellSize / 2 * cellSize / 2 - cellSize / 4 * cellSize / 4), position.y,position.z + cellSize / 4));
        vertices.Add(new Vector3(position.x + Mathf.Sqrt(cellSize / 2 * cellSize / 2 - cellSize / 4 * cellSize / 4), position.y,position.z - cellSize / 4));
        vertices.Add(new Vector3(position.x, position.y,position.z - cellSize / 2));
        vertices.Add(new Vector3(position.x - Mathf.Sqrt(cellSize / 2 * cellSize / 2 - cellSize / 4 * cellSize / 4), position.y,position.z - cellSize / 4));
        vertices.Add(new Vector3(position.x - Mathf.Sqrt(cellSize / 2 * cellSize / 2 - cellSize / 4 * cellSize / 4), position.y,position.z + cellSize / 4));
        
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
    
    public void Init(float cellSize) {
        this.cellSize = cellSize;
        meshRenderer = GetComponent<MeshRenderer>();

        CreateMesh();
    }

    public void ChangeCellColor(Color newColor) {
        meshRenderer.material.color = newColor;
    }
    
    
}
