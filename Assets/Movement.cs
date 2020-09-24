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
public class MoveThingsSystem : ComponentSystem
{
    protected uint last_processed_tick = 0;
    protected override void OnUpdate()
    {
        var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
        var tick = group.ServerTick;

        var deltaTime = Time.DeltaTime;

        Entities.ForEach((DynamicBuffer<AgentInput> inputBuffer, ref DestinationComponent destination, ref Rotating rotating) =>
        {

            AgentInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            rotating.Value = 0;
            if (input.tick > last_processed_tick) {
              last_processed_tick = input.tick;

              if (input.DestinationUpdated()) {
                destination.Value = input.location;
                destination.Valid = true;
              }

              // if input.OrientationUdpdated
              // target_orientation.Value = input.target_orientation;

              // TODO can we process canuse somewhere else?
              if (input.AbilityUsed()) {
                Entity ability = GhostServerSystem.getGhost(input.ghostId);
                Usable usable = EntityManager.GetComponentData<Usable>(ability);
                if (usable.canuse) {
                  
                  usable.inuse = true;
                  usable.canuse = false;
                  EntityManager.SetComponentData<Usable>(ability, usable);

                  if (input.AngleSent()) {
                    AngleInput angle = EntityManager.GetComponentData<AngleInput>(ability);
                    angle.Value = input.angle;
                    EntityManager.SetComponentData<AngleInput>(ability, angle);
                  }
                }
              }
              if (input.RotatingLeft()) {
                rotating.Value = -1;
              }
              if (input.RotatingRight()) {
                rotating.Value = +1;
              }
              if (input.AbilityReleased()) {
                Entity ability = GhostServerSystem.getGhost(input.ghostId_released);
                Releasable releasable = EntityManager.GetComponentData<Releasable>(ability);
                releasable.released = true;
                EntityManager.SetComponentData<Releasable>(ability, releasable);
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

    Entities.ForEach((ref BusyTimer busyTimer) => {
        busyTimer.Value -= deltaTime;
    });

    Entities.ForEach((ref DestinationComponent dest, ref GamePosition position, ref Speed speed, ref GameOrientation orientation, ref BusyTimer busyTimer, ref Knockback knockback) => 
    {

       if (!dest.Value.Equals(position.Value) && dest.Valid && busyTimer.Value <= 0 && !knockback.active) {
         
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
           dest.Valid = false;
         } else {
           deltaPos = speed.Value * deltaTime * math.normalize(deltaPos) * mov_mod;
           position.Value += deltaPos;
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

    Entities.ForEach((ref Rotation rot, ref Rotating rotating, ref GameOrientation orientation) => 
    {
        float rotSpeed = 720f;

        
        rot.Value *= Quaternion.AngleAxis(rotSpeed * rotating.Value * deltaTime, Vector3.up);
        orientation.Value = math.rotate(rot.Value, new float3(0,0,1)).xz;

        //if (math.length(target.Value) != 0) {
        //  Quaternion target_orientation = Quaternion.LookRotation(Utility.f2tov3(target.Value), Vector3.up);
        //  rot.Value = Quaternion.RotateTowards(rot.Value, target_orientation, rotSpeed * deltaTime);
        //  orientation.Value = math.rotate(rot.Value, new float3(0,0,1)).xz;
        //}

    });

    Entities.ForEach((ref GamePosition position, ref Knockback knockback) => {
      //Debug.Log("in knockback calc 1");
      if (knockback.active) {
      //Debug.Log("in knockback if statement");
        position.Value += knockback.Value.xz;
        knockback.Value *= 0.1f;
        //Debug.Log(knockback.Value);
        knockback.active = math.length(knockback.Value) > 0.1;
      }
    });

  }



}




