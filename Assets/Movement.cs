using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

// TODO: ComponentSystems all run on main thread (not in parallel) - update?
// TODO: probably should put this back on ghostprediction system? First I'd like to see how it works without
// prediction though.
//[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class UpdateDestinationSystem : ComponentSystem
{
    protected uint last_processed_tick = 0;
    protected override void OnUpdate()
    {
        var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
        var tick = group.ServerTick;

        var deltaTime = Time.DeltaTime;

        Entities.ForEach((DynamicBuffer<AgentInput> inputBuffer, ref DestinationComponent destination, ref TargetOrientation target_orientation) =>
        {

            AgentInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            if (input.tick > last_processed_tick) {
              last_processed_tick = input.tick;

              switch (input.type) {
                case 1:
                  destination.Value = input.destination;
                  destination.Valid = true;
                  //Debug.Log("Server got 'set destination'");
                  break;
                case 2:
                  target_orientation.Value = input.target_orientation;
                  break;
                case 3:
                  Entity ability = GhostServerSystem.getGhost(input.ghostId);
                  //Debug.Log("ability:");
                  //Debug.Log(ability);
                  Usable usable = EntityManager.GetComponentData<Usable>(ability);
                  Cooldown cooldown = EntityManager.GetComponentData<Cooldown>(ability);
                  //Debug.Log("Server got 'use ability'");
                  if (usable.canuse) {
                    
                    usable.inuse = true;
                    usable.canuse = false;
                    cooldown.timer = cooldown.duration;
                    EntityManager.SetComponentData<Usable>(ability, usable);
                    EntityManager.SetComponentData<Cooldown>(ability, cooldown);
                  }
                  break;
              }
            }
        });
    }
}



[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class MoveAgentSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref DestinationComponent dest, ref GamePosition position, ref Speed speed, ref GameOrientation orientation, ref BackwardModifier modifier, ref CanMove canmove) => 
    {

       if (!dest.Value.Equals(position.Value) && dest.Valid && canmove.Value) {
         
         float2 deltaPos = dest.Value - position.Value;

         float angle_to_movement = Vector2.Angle(Utility.f2tov2(orientation.Value), Utility.f2tov2(deltaPos));

         float front_sidestep_mod = 0.5f;
         float back_sidestep_mod = 0.3f;
         float back_mod = 0.6f;
         float mov_mod;

         if (angle_to_movement <= 90) {
           mov_mod = 1f - (1f - front_sidestep_mod) * (angle_to_movement / 90f);
         } else {
           mov_mod = back_sidestep_mod - (angle_to_movement - 90f) / 90f * (back_sidestep_mod - back_mod);
         }

         if (math.length(deltaPos) < speed.Value * deltaTime * mov_mod) {
           position.Value = dest.Value;
           Debug.Log("Setting position to destination");
         } else {
           deltaPos = speed.Value * deltaTime * math.normalize(deltaPos) * mov_mod;
           position.Value += deltaPos;
           Debug.Log("updating position by deltapos");
         }


       }
    });

    Entities.ForEach((Entity ent, ref Translation trans, ref GamePosition position) =>  {
        if (!trans.Value.xz.Equals(position.Value)) {
          trans.Value.xz = position.Value;
          
          // update animatingbody's position
          //GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
          //animatingBody.transform.localPosition = Utility.f2tov3(position.Value);
        }
    });

    Entities.ForEach((ref Rotation rot, ref TargetOrientation target, ref GameOrientation orientation) => 
    {
        float rotSpeed = 720f;
        if (math.length(target.Value) != 0) {
          Quaternion target_orientation = Quaternion.LookRotation(Utility.f2tov3(target.Value), Vector3.up);
          rot.Value = Quaternion.RotateTowards(rot.Value, target_orientation, rotSpeed * deltaTime);
          orientation.Value = math.rotate(rot.Value, new float3(0,0,1)).xz;
        }

    });
  }
}

[UpdateInGroup(typeof(ClientAndServerSimulationSystemGroup))]
public class UpdateAnimationSystem : ComponentSystem {
  protected override void OnUpdate() {

    Entities.ForEach((Entity ent, ref Translation trans, ref DestinationComponent dest, ref GamePosition position, ref AnimationInitialized anim_init) => {
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      bool idle = dest.Value.Equals(position.Value);
      if (!idle) {
        // update animatingbody's position
        animatingBody.transform.localPosition = Utility.f2tov3(position.Value);
      } 
      Debug.Log(idle);
      animatingBody.GetComponent<Animator>().SetBool("Idle", idle);
    });

    Entities.ForEach((Entity ent, ref Rotation rot, ref DestinationComponent dest, ref GamePosition position, ref GameOrientation orientation, ref AnimationInitialized anim_init) => {
      // update animatingbody's rotation
      GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(ent);
      
      float2 deltaPos = dest.Value - position.Value;
      float angle_to_movement = Vector2.SignedAngle(Utility.f2tov2(orientation.Value), Utility.f2tov2(deltaPos));
      float angle_from_right = angle_to_movement + 90;
      if (angle_from_right < 0) {
        angle_from_right += 360;
      }
      animatingBody.GetComponent<Animator>().SetFloat("AngleFromRight", angle_from_right);
      Debug.Log("angle_from_right");
      Debug.Log(angle_from_right);
      animatingBody.transform.localRotation = rot.Value;
    });
  }
}
