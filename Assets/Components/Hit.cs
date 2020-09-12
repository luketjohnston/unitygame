using Unity.Entities;

public struct Hit : IBufferElementData {
  public Entity Value;
  public static implicit operator Entity(Hit h) { return h.Value; }
}
