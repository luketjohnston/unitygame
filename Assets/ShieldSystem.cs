using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class ShieldSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Shield shield, ref Usable usable, ref OwningPlayer player, ref AngleInput angle, ref Releasable releasable) => 
    {

       if (releasable.released) {
         usable.inuse = false;
         releasable.released = false;
       }
       if (usable.inuse) {
         usable.canuse = true; // keep this true always
         DestinationComponent dest = EntityManager.GetComponentData<DestinationComponent>(player.Value);
         dest.Valid = false; 
         EntityManager.SetComponentData<DestinationComponent>(player.Value, dest);
       } else {
         // pass
       }
    });
  }

  public static Entity AddAbility(Entity agent, EntityManager manager, GhostPrefabCollectionComponent ghostCollection) {

    //var ghostId = NetAgentGhostSerializerCollection.FindGhostType<ShieldSnapshotData>();
    var ghostId = 2;
    var prefab = manager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
    var ability = manager.Instantiate(prefab);
    
    var agentGhostId = manager.GetComponentData<GhostComponent>(agent).ghostId;

    Usable usable; KeyCodeComp keycode; OwningPlayer player; Shield shield; 

    shield.strength = 10;
    shield.damage = 10;

    keycode.Value = KeyCode.S;
    usable.inuse = false;
    usable.canuse = true;
    player.Value = agent;
    player.PlayerId = manager.GetComponentData<AgentComponent>(agent).PlayerId;

    manager.SetComponentData<Usable>(ability, usable);
    manager.SetComponentData<KeyCodeComp>(ability, keycode);
    manager.SetComponentData<Shield>(ability, shield);
    manager.SetComponentData<OwningPlayer>(ability, player);

    return ability;
  }

}



// update health bar prefabs with correct health
// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ShieldUpdateAnimationSystem : ComponentSystem
{
  protected override void OnUpdate() {

    Entities.ForEach((ref Shield shield, ref Usable usable, ref OwningPlayer player) => 
    {
       if (EntityManager.HasComponent<AnimationInitialized>(player.Value)) {
         GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(player.Value);
         Animator anim = animatingBody.GetComponent<Animator>();
         if (usable.inuse) {
           anim.SetBool("Idle", true);
           anim.SetBool("Blocking", true);
           anim.SetLayerWeight(1, 1);
         } else { 
           anim.SetBool("Blocking", false);
           anim.SetLayerWeight(1, 0);
         }
       }
    });
  }
}


