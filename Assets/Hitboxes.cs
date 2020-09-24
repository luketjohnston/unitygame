
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateAfter(typeof(SwordSystem))]
public class UpdateGamePositions : ComponentSystem
{
    protected override void OnUpdate()
    {
        // TODO put below two methods somewhere else, they shouldn't be in hitboxes.cs
        Entities.ForEach((Entity ent, ref GamePosition position, ref OwningPlayer player) =>
        {
          GamePosition playerPos = EntityManager.GetComponentData<GamePosition>(player.Value);
          position.Value = playerPos.Value;
        });

        // TODO remove this "withnone"
        Entities.WithNone<ShieldHitbox>().ForEach((Entity ent, ref Rotation rot, ref OwningPlayer player) =>
        {
          Rotation playerRot = EntityManager.GetComponentData<Rotation>(player.Value);
          rot.Value = playerRot.Value;
        });

        // remove hits on self
        Entities.ForEach((DynamicBuffer<Hit> hitBuffer, ref Hurtbox hurtbox, ref OwningPlayer player) => {
          
          int i = 0;
          while (i < hitBuffer.Length) {
            Entity hittingPlayer = EntityManager.GetComponentData<OwningPlayer>(hitBuffer[i].ent).Value;
            if (hittingPlayer == player.Value) {
              hitBuffer.RemoveAt(i);
            } else {
              i++;
            }
          }
        });


        // TODO: make sure this happens after the knockbacksystems
        // first, compile hits on all hitboxes into OwningPlayer entity
        Entities.WithNone<ShieldHitbox>().ForEach((DynamicBuffer<Hit> hitBuffer, ref Hurtbox hurtbox, ref OwningPlayer player) => {
          //Debug.Log("In compile hurts onto agent");
          //Debug.Log(player.Value);
          var playerBuffer = EntityManager.GetBuffer<Hit>(player.Value);
          List<Hit> hitList = new List<Hit>();
          // for some reason, adding to playerBuffer invalidates hitBuffer. IDK
          foreach (var hit in hitBuffer) {
            //Debug.Log(hit);
            hitList.Add(hit);
          }
          foreach (var hit in hitList) {
            playerBuffer.Add(hit);
          }
          hitBuffer.Clear();
        });

        Entities.ForEach((Entity ent, DynamicBuffer<Hit> hitBuffer, ref ShieldHitbox hitbox, ref OwningPlayer player, ref Rotation rot) => {
          var playerBuffer = EntityManager.GetBuffer<Hit>(player.Value);


          float3 dir_vec = math.rotate(rot.Value, Utility.v3tof3(Vector3.forward));
          // check here if blockDir dotprod hitdir is positive
          foreach (Hit hit in hitBuffer) {
            // if hit and shield are facing the same way, that means we're hitting the shield
            // from the back, which shouldn't be blocked.
            //Debug.Log("Knockback + dirvec:");
            //Debug.Log(hit.ent);
            //Debug.Log(ent);
            //Debug.Log(hit.knockback);
            //Debug.Log(dir_vec);
            //Debug.Log(math.dot(hit.knockback, dir_vec));
            if (math.dot(hit.knockback, dir_vec) < 0) {
              int i = 0;
              while (i < playerBuffer.Length) {
                if (playerBuffer[i].ent == hit.ent) {
                    playerBuffer.RemoveAt(i);
                } else {
                  i++;
                }
              }
            }
          }
          hitBuffer.Clear();
        });

        // Then, apply damage from each hit
        Entities.ForEach((DynamicBuffer<Hit> hitBuffer, ref Health health, ref Knockback knockback) => {
          foreach (Hit hit in hitBuffer) {
            health.Value -= hit.damage;
            knockback.Value += hit.knockback;
            knockback.active = true;
          }
          hitBuffer.Clear();
        });



    }
}

[UpdateInGroup(typeof(ClientAndServerSimulationSystemGroup))]
[UpdateBefore(typeof(MoveThingsSystem))]
public class ManageHitboxDisables : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((DynamicBuffer<HitboxElement> hitboxBuffer, ref Usable usable) => {
          //Debug.Log("inside hitboxbuffer foreach");
          // TODO this is probably slow. Should only add / remove when inuse is changed.
          if (usable.inuse) {
            foreach (Entity hitbox in hitboxBuffer.Reinterpret<Entity>()) {
              //EntityManager.SetComponentData<Disableable>(hitbox, new Disableable {disabled = false});
              EntityManager.SetEnabled(hitbox, true);
              //disableable.disabled = false;
            }
          } else {
            //Debug.Log("inside hitboxbuffer else");
            foreach (Entity hitbox in hitboxBuffer.Reinterpret<Entity>()) {
              EntityManager.SetEnabled(hitbox, false);
              //EntityManager.SetComponentData<Disableable>(hitbox, new Disableable {disabled = true});
              //disableable.disabled = true;
            }
          }
        });
    }
}

// this update has to take place after netcode information is sent
// TODO: consider whether this ordering has any effect on server v client consistency
//[UpdateBefore(typeof(ManageHitboxDisables))]
//[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
//public class ServerManageDisabledHitboxes : ComponentSystem {
//  protected override void OnUpdate() {
//    Entities.ForEach((Entity ent, ref Disableable disableable) => {
//      if (disableable.disabled) {
//        EntityManager.SetEnabled(ent, false);
//      }
//    });
//    Entities.ForEach((Entity ent, ref Disabled disabled, ref Disableable disableable) => {
//      if (!disableable.disabled) {
//        EntityManager.SetEnabled(ent, true);
//      }
//    });
//  }
//}

//[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
//public class ClientManageDisabledHitboxes : ComponentSystem {
//  protected override void OnUpdate() {
//    Entities.ForEach((Entity ent, ref Disableable disableable) => {
//      if (disableable.disabled) {
//        EntityManager.SetEnabled(ent, false);
//      }
//    });
//    Entities.ForEach((Entity ent, ref Disabled disabled, ref Disableable disableable) => {
//      if (!disableable.disabled) {
//        EntityManager.SetEnabled(ent, true);
//      }
//    });
//  }
//}
