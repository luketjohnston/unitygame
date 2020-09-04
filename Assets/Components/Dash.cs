using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;


[GenerateAuthoringComponent]
public struct Dash : IComponentData {
  [GhostField(Quantization=1000)]
  public float distance_traveled;
  [GhostField(Quantization=1000)]
  public float max_distance;
  [GhostField(Quantization=1000)]
  public float speed;
  [GhostField(Quantization=1000)]
  public float3 dir;
}


