using UnityEngine;

public class BuildingTypeAttribute : PropertyAttribute {
    public readonly System.Type buildingType;

    public BuildingTypeAttribute(System.Type type) {
        buildingType = type;
    }
}