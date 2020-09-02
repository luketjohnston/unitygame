using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;


[GenerateAuthoringComponent]
public struct Dash : IComponentData {
  [GhostDefaultField(1000)]
  public float distance_traveled;
  [GhostDefaultField(1000)]
  public float max_distance;
  [GhostDefaultField(1000)]
  public float speed;
  [GhostDefaultField(1000)]
  public float3 dir;
}


