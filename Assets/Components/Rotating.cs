using Unity.Entities;
using Unity.NetCode;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Rotating : IComponentData
{
  [GhostField]
  public int Value;
}
