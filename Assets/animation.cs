using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;



// TODO Make this only run once
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class InitializeAnimatorParams : ComponentSystem {
  protected override void OnUpdate() {
    Entities.ForEach((Entity ent, ref AnimationInitialized anim_init) => {
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      animatingBody.GetComponent<Animator>().SetFloat("StabSpeed", 3f);
    });
  }
}






[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class UpdateAnimationSystem : ComponentSystem {
  protected override void OnUpdate() {

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((Entity ent, ref Translation trans, ref DestinationComponent dest, ref GamePosition position, ref AnimationInitialized anim_init) => {
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      bool idle = (dest.Value.Equals(position.Value) || !dest.Valid);
      if (!idle) {
        // update animatingbody's position
        animatingBody.transform.localPosition = Utility.f2tov3(position.Value);
      } 
      animatingBody.GetComponent<Animator>().SetBool("Idle", idle);
    });

    Entities.ForEach((Entity ent, ref Rotation rot, ref DestinationComponent dest, ref GamePosition position, ref GameOrientation orientation, ref AnimationInitialized anim_init) => {
      // update animatingbody's rotation
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      Animator anim = animatingBody.GetComponent<Animator>();
      
      float2 deltaPos = dest.Value - position.Value;
      float angle_to_movement = Vector2.SignedAngle(Utility.f2tov2(orientation.Value), Utility.f2tov2(deltaPos));
      float angle_from_right = angle_to_movement + 90;
      if (angle_from_right < 0) {
        angle_from_right += 360;
      }

      // TODO I do this computation in many places, can we save it and reuse? (or at least modularize it)
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      float ray_enter = 0f;
      Plane arenaPlane = new Plane(Vector3.up, Vector3.zero);
      arenaPlane.Raycast(ray, out ray_enter);
      Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
      float2 input_orientation = new float2(target.x, target.y) - position.Value;
      float2 player_orientation = orientation.Value;
      float angle_to_cursor = Vector2.SignedAngle(Utility.f2tov2(input_orientation), Utility.f2tov2(player_orientation));

      anim.SetFloat("BlockAngle", angle_to_cursor);
      anim.SetFloat("AngleFromRight", angle_from_right);
      animatingBody.transform.localRotation = rot.Value;
    });

    Entities.ForEach((Entity ent, ref FreezeTimer freezeTimer) => {
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      Animator anim = animatingBody.GetComponent<Animator>();

      if (freezeTimer.Value > 0) {
        freezeTimer.Value -= deltaTime;
        anim.enabled = false;
      } else {
        anim.enabled = true;
      }
    });

  }
}

