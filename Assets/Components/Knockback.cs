using Unity.NetCode;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct Knockback : IComponentData {
  public float Value;
}
