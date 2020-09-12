using Unity.NetCode;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct Damage : IComponentData {
  public float Value;
}
