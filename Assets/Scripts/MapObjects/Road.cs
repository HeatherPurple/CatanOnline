using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building {

    protected override void Awake() {
        base.Awake();
        buildingType = BuildingType.Road;
    }
    
}
