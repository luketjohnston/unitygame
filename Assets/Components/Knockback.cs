using Unity.NetCode;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Knockback : IComponentData {
  public float3 Value;
  public bool active;
}
