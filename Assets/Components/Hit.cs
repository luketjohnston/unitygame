using Unity.Entities;
using Unity.Mathematics;

public struct Hit : IBufferElementData {
  public Entity ent;
  public float3 knockback;
  public float damage;
  public static implicit operator Entity(Hit h) { return h.ent; }
}
