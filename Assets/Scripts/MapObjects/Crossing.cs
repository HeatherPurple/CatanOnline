using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossing : Building {
    
    protected override void Awake() {
        base.Awake();
        buildingType = BuildingType.Crossing;
    }
    
    
    
}
