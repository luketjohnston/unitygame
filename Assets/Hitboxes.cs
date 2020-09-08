
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
    }
}


