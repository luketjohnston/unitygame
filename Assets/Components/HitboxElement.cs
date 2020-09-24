using Unity.Entities;
using Unity.Mathematics;

public struct HitboxElement : IBufferElementData {
  public Entity ent;
  public static implicit operator Entity(HitboxElement h) { return h.ent; }
}
