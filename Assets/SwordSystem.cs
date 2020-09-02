
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


// update health bar prefabs with correct health
// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class SwordSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Sword sword, ref Usable usable, ref OwningPlayer player, ref Cooldown cooldown) => 
    {

       GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(player.Value);
       Animator anim = animatingBody.GetComponent<Animator>();
       float speed = anim.GetFloat("StabSpeed");
       float stabTime = Utility.AnimationLength("CharArmature|Stab", animatingBody) / speed;

       
       // TODO the order of operations matters here probably?
       if (usable.inuse) {

         BusyTimer busyTimer = EntityManager.GetComponentData<BusyTimer>(player.Value);
         if (cooldown.timer == cooldown.duration) {

           // make agent unable to move, unset destination, start animation
           DestinationComponent dest = EntityManager.GetComponentData<DestinationComponent>(player.Value);
           dest.Valid = false; 
           busyTimer.Value = stabTime;
           EntityManager.SetComponentData<DestinationComponent>(player.Value, dest);
           EntityManager.SetComponentData<BusyTimer>(player.Value, busyTimer);
           anim.SetBool("Idle", true);
           anim.SetBool("Stabbing", true);
         }

         if (cooldown.timer > 0 && cooldown.timer < cooldown.duration - stabTime) {
           
           usable.inuse = false;
           anim.SetBool("Stabbing", false);
           anim.SetBool("Idle", true);
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


// update health bar prefabs with correct health
// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ClientAndServerSimulationSystemGroup))]
public class SwordUpdateAnimationSystem : ComponentSystem
{
  protected override void OnUpdate() {

    Entities.ForEach((ref Sword sword, ref Usable usable, ref OwningPlayer player, ref Cooldown cooldown, ref Translation trans, ref Rotation rot) => 
    {
       GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(player.Value);
       if (usable.inuse) {
         animatingBody.GetComponent<Animator>().SetBool("Idle", true);
         animatingBody.GetComponent<Animator>().SetBool("Stabbing", true);
       } else { 
         animatingBody.GetComponent<Animator>().SetBool("Stabbing", false);
       }
    });
  }
}

