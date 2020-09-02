using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public struct Sword : IComponentData {
  public float damage;
}
