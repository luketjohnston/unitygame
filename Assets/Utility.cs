using Unity.Mathematics;
using UnityEngine;


public static class Utility {
  public static Vector2 v3to2(Vector3 v) {
    return new Vector2(v.x, v.z);
  }
  public static Vector3 v2to3(Vector2 v) {
    return new Vector3(v.x, 0f, v.y);
  }

  public static Vector2 f2tov2(float2 f) {
    return new Vector2(f.x, f.y);
  }
  public static Vector3 f3tov3(float3 f) {
    return new Vector3(f.x, f.y, f.z);
  }
  public static Vector3 f2tov3(float2 f) {
    return new Vector3(f.x, 0f, f.y);
  }
  public static float3 v3tof3(Vector3 v) {
    return new float3(v.x, v.y, v.z);
  }

  public static float2 v3tof2(Vector3 v) {
    return new float2(v.x, v.z);
  }


  public static float AnimationLength(string animationName, GameObject animatingBody) {
    Animator anim = animatingBody.GetComponent<Animator>();
    RuntimeAnimatorController ac = anim.runtimeAnimatorController;
    for (int i = 0; i < ac.animationClips.Length; i++) {
      if (ac.animationClips[i].name == animationName) {
        return ac.animationClips[i].length;
      }
    }
    throw new System.ArgumentException("AnimationLength could not find animation " + animationName);
  }
}



