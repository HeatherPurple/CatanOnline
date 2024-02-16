using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingSO : ScriptableObject {
    public GameObject buildingVisual;
    
    [CanBeNull] public Type GetBuildingType() {
        return Type.GetType(GetType().FullName.Replace("SO", ""));
    }
}
