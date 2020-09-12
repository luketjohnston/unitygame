
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


        // first, compile hits on all hitboxes into OwningPlayer entity
        Entities.WithNone<ShieldHitbox>().ForEach((DynamicBuffer<Hit> hitBuffer, ref Hurtbox hurtbox, ref OwningPlayer player) => {
          Debug.Log("In compile hurts onto agent");
          Debug.Log(player.Value);
          var playerBuffer = EntityManager.GetBuffer<Hit>(player.Value);
          List<Hit> hitList = new List<Hit>();
          // for some reason, adding to playerBuffer invalidates hitBuffer. IDK
          foreach (var hit in hitBuffer) {
            Debug.Log(hit);
            hitList.Add(hit);
          }
          foreach (var hit in hitList) {
            playerBuffer.Add(hit);
          }
          hitBuffer.Clear();
        });

        Entities.ForEach((DynamicBuffer<Hit> hitBuffer, ref ShieldHitbox hitbox, ref OwningPlayer player) => {
          var playerBuffer = EntityManager.GetBuffer<Hit>(player.Value).Reinterpret<Entity>();
          foreach (Entity hit in hitBuffer.Reinterpret<Entity>()) {
            //remove from owningPlayer hitbuffer
            for (int i = 0; i < playerBuffer.Length; i++) {
              if (playerBuffer[i] == hit) {
                playerBuffer.RemoveAt(i);
              }
            }
          }
          hitBuffer.Clear();
        });

        // Then, apply damage from each hit
        Entities.ForEach((DynamicBuffer<Hit> hitBuffer, ref Health health) => {
          foreach (Entity hit in hitBuffer.Reinterpret<Entity>()) {
            Damage damage = EntityManager.GetComponentData<Damage>(hit);
            health.Value -= damage.Value;
          }
          hitBuffer.Clear();
        });

        //Entities.ForEach((ref Hitbox hitbox) => {
        //  hitbox.hitToProcess = Entity.Null;
        //});


        // TODO NEXT: implement "direction" of attack, and then implement blocking collisions.
        // Plan of attack: give hitboxes a dynamic buffer of hurtbox entities hit. 
        //  - assignn to this dynamic buffer during collision phase
        //  - process this dynamic buffer k









    }
}


