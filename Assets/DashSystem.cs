using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;


// TODO: ComponentSystems all run on main thread (not in parallel) - update?
// TODO: probably should put this back on ghostprediction system? First I'd like to see how it works without
// prediction though.
//[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class DashSystem : ComponentSystem
{

  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Dash dash, ref Usable usable, ref OwningPlayer player, ref Cooldown cooldown) => 
    {
       if (usable.inuse) {

         if (cooldown.timer == cooldown.duration) {
           // just used ability
           DestinationComponent dest = EntityManager.GetComponentData<DestinationComponent>(player.Value);
           CanMove canmove = EntityManager.GetComponentData<CanMove>(player.Value);
           dest.Valid = false; 
           canmove.Value = false;
           EntityManager.SetComponentData<DestinationComponent>(player.Value, dest);
           EntityManager.SetComponentData<CanMove>(player.Value, canmove);
         }

         Debug.Log("DashSystem inuse is true");
         GamePosition pos = EntityManager.GetComponentData<GamePosition>(player.Value);

         Rotation rot = EntityManager.GetComponentData<Rotation>(player.Value);
         float3 direction = math.rotate(rot.Value, dash.dir);
         pos.Value += direction.xz * dash.speed * deltaTime;

         dash.distance_traveled += dash.speed * deltaTime;
         EntityManager.SetComponentData<GamePosition>(player.Value, pos);

         if (dash.distance_traveled >= dash.max_distance) {
           usable.inuse = false;
           EntityManager.SetComponentData<CanMove>(player.Value, new CanMove {Value = true});
           if (cooldown.timer < 0) {
             usable.canuse = true;
           }
           dash.distance_traveled = 0;
         }

       }
    });
  }

  public static Entity AddAbility(Entity agent, float cooldown_length, float speed, float distance, float3 dir, KeyCode key, EntityManager manager, GhostPrefabCollectionComponent ghostCollection) {

    var ghostId = NetAgentGhostSerializerCollection.FindGhostType<DashSnapshotData>();
    var prefab = manager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
    var ability = manager.Instantiate(prefab);
    
    var agentGhostId = manager.GetComponentData<GhostComponent>(agent).ghostId;

    Cooldown cooldown; Usable usable; Dash dash; KeyCodeComp keycode; OwningPlayer player;
    cooldown.timer = 0;
    cooldown.duration = cooldown_length;
    dash.speed = speed;
    dash.distance_traveled = 0;
    dash.max_distance = distance;
    dash.dir = dir;
    keycode.Value = key; //This key is set by keycodesystem, not here.
    usable.inuse = false;
    usable.canuse = true;
    player.Value = agent;
    player.PlayerId = manager.GetComponentData<AgentComponent>(agent).PlayerId;

    manager.SetComponentData<Cooldown>(ability, cooldown);
    manager.SetComponentData<Usable>(ability, usable);
    manager.SetComponentData<KeyCodeComp>(ability, keycode);
    manager.SetComponentData<Dash>(ability, dash);
    manager.SetComponentData<OwningPlayer>(ability, player);

    return ability;
  }
}
