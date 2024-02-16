using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Building {
    
    protected override void Awake() {
        base.Awake();
        buildingType = BuildingType.Cell;
    }

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

}
