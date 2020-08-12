using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public struct Sword : IComponentData {
  public float damage;
}

// THis component is added to the player that has a sword
[GenerateAuthoringComponent]
public struct HasSword {
  [GhostDefaultField]
  public Entity sword;
}




// update health bar prefabs with correct health
// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class SwordSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Sword sword, ref Usable usable, ref OwningPlayer player, ref Cooldown cooldown, ref Translation trans, ref Rotation rot) => 
    {

       
       if (usable.inuse) {

         if (cooldown.timer == cooldown.duration) {
           // just used ability
           // Make sword visible and 

           //DestinationComponent dest = EntityManager.GetComponentData<DestinationComponent>(player.Value);
           //CanMove canmove = EntityManager.GetComponentData<CanMove>(player.Value);
           //dest.Valid = false; 
           //canmove.Value = false;
           //EntityManager.SetComponentData<DestinationComponent>(player.Value, dest);
           //EntityManager.SetComponentData<CanMove>(player.Value, canmove);
         }

         float swingTime = 0.12f;
         float fullAngle = 180f;
         float angle = fullAngle * (cooldown.duration - cooldown.timer) / swingTime;
         if (angle > fullAngle) {
           usable.inuse = false;
           // make sword invisible
           trans.Value.x = 1000000; // make invisible TODO
         } else {

           float2 playerPos = EntityManager.GetComponentData<GamePosition>(player.Value).Value;
           quaternion playerRot = EntityManager.GetComponentData<Rotation>(player.Value).Value;
           float3 adjustment = new float3(-3, 1, 0);
           rot.Value = math.mul(playerRot, quaternion.AxisAngle(new float3(0,1,0), Mathf.Deg2Rad * angle));
           adjustment = math.mul(rot.Value, adjustment);
           trans.Value = new float3(playerPos.x + adjustment.x, adjustment.y, playerPos.y + adjustment.z);
         }

       }
    });
  }

  public static Entity AddAbility(Entity agent, EntityManager manager, GhostPrefabCollectionComponent ghostCollection) {

    var ghostId = NetAgentGhostSerializerCollection.FindGhostType<SwordSnapshotData>();
    var prefab = manager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
    var ability = manager.Instantiate(prefab);
    
    var agentGhostId = manager.GetComponentData<GhostComponent>(agent).ghostId;

    Cooldown cooldown; Usable usable; KeyCodeComp keycode; OwningPlayer player; Sword sword; Translation trans; Scale scale;
    cooldown.timer = 0;
    cooldown.duration = 1;
    sword.damage = 10;
    keycode.Value = KeyCode.A;
    usable.inuse = false;
    usable.canuse = true;
    player.Value = agent;
    player.PlayerId = manager.GetComponentData<AgentComponent>(agent).PlayerId;
    trans.Value = new float3(100000,1,0); // so it starts invisible

    manager.SetComponentData<Cooldown>(ability, cooldown);
    manager.SetComponentData<Usable>(ability, usable);
    manager.SetComponentData<KeyCodeComp>(ability, keycode);
    manager.SetComponentData<Sword>(ability, sword);
    manager.SetComponentData<OwningPlayer>(ability, player);
    manager.SetComponentData<Translation>(ability, trans);

    return ability;
  }

}
