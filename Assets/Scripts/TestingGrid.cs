using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour {
    
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int mapSize;
    void Start() {
        HexGrid hexGrid = new HexGrid(10, pointPrefab, mapSize);
        
    }

}
