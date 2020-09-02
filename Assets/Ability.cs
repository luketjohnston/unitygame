using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;


// TODO: remove elements from AgentInput buffer? in entity inspector it seems they pile up.


// TODO write system for checking if abilites are in use / can be used
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class CooldownSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Cooldown cooldown, ref Usable usable) => 
    {
      if (cooldown.timer < 0) {
        usable.canuse = true;
      }
      if (cooldown.timer >= 0) {
        cooldown.timer -= deltaTime;
      }

      // separate systems must reset inuse to false when ability is done being used
      if (usable.inuse && cooldown.timer < 0) {
        cooldown.timer = cooldown.duration;
      }

    });
  }
}

