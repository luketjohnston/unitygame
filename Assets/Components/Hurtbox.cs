using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Hurtbox : IComponentData
{
  public Entity hitToProcess;
}
