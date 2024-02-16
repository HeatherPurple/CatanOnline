using System;
using JetBrains.Annotations;
using UnityEngine;

public struct GridObject {

    private readonly Transform gridTransform;
    private readonly float size;
    private readonly Vector3 position;
    private readonly Type buildingType;
    private Building building;

    public GridObject(Transform gridTransform, float size, Vector3 position, Type buildingType) {
        this.gridTransform = gridTransform;
        this.size = size;
        this.position = position;
        this.buildingType = buildingType;
        building = null;
        SpawnBuildingGameObject();
    }
    
    private void SpawnBuildingGameObject() {
        GameObject spawnedBuildingGameObject = new GameObject {
            transform = {
                parent = gridTransform,
                position = position,
            }
        };
        spawnedBuildingGameObject.transform.localScale *= size;
        building = spawnedBuildingGameObject.AddComponent(buildingType) as Building;
    }
    
    public Vector3 GetPosition() => position;
    public bool IsTypeOfBuilding<T>() => buildingType.Equals(typeof(T));

    public Building GetBuilding() => building;

}