using Unity.Entities;
using Unity.NetCode;


// indicates an ability that we need to know when the key is released (not just pressed initially). Example: holding s to block with shield
// need to know when s is released to stop blocking.
[GenerateAuthoringComponent]
public struct Releasable : IComponentData {
  [GhostField]
  public bool released;
}
