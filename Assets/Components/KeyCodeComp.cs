using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[GenerateAuthoringComponent]
public struct KeyCodeComp : IComponentData {
  public KeyCode Value;
}
