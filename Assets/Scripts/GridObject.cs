using System;
using JetBrains.Annotations;
using UnityEngine;

public struct GridObject {

    private readonly Vector3 position;
    private readonly Type buildingType;
    private readonly Building building;

    public GridObject(Vector3 position, Type buildingType) {
        this.position = position;
        this.buildingType = buildingType;
        building = null;
    }

    public Vector3 GetPosition() => position;
    public bool IsTypeOfBuilding<T>() => buildingType.Equals(typeof(T));
    public Building GetBuilding() => building;
    
}