using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    [SerializeField] private CellSO cellSO;
    
    private float size;
    private CellMesh cellMesh;
    private bool isSelected = false;
    
    //private Crossing[] crossingArray = new Crossing[6];

    // public struct Crossing {
    //     public Vector3 position;
    //     public bool isFilled;
    // }
    
    // public Vector3[] GetHexVertices() {
    //     float halfCellSize = cellSize / 2;
    //     float quarterCellSize = cellSize / 4;
    //     float xOffset = halfCellSize * Mathf.Cos(Mathf.Deg2Rad * 30f);
    //
    //     var hexPosition = transform.position;
    //     
    //     return new Vector3[] {
    //         new Vector3(0f, 0f, halfCellSize) + hexPosition,
    //         new Vector3(xOffset, 0f, quarterCellSize) + hexPosition,
    //         new Vector3(xOffset, 0f, -quarterCellSize) + hexPosition,
    //         new Vector3(0f, 0f, -halfCellSize) + hexPosition,
    //         new Vector3(-xOffset, 0f, quarterCellSize) + hexPosition,
    //         new Vector3(-xOffset, 0f, -quarterCellSize) + hexPosition,
    //     };
    // }

    private void Awake() {
        cellMesh = GetComponent<CellMesh>();
    }

    public void Init(float cellSize) { //, List<Cell.Crossing> cellCrossings
        size = cellSize;

        //crossingArray = cellCrossings.ToArray();
    }

    public float GetSize() => size;
    public CellSO GetCellSO() => cellSO;
    public bool IsSelected() => isSelected;
    
    public void SelectCell() {
        isSelected = true;
        cellMesh.UpdateMeshColor();
    }
    public void UnselectCell() { 
        isSelected = false;
        cellMesh.UpdateMeshColor(); 
    }
    
    public void ChangeCellSO(CellSO newCellSO) {
        cellSO = newCellSO;
        cellMesh.UpdateMeshColor();
    }
}
