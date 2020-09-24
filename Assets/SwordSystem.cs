
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

       //GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(player.Value);
       //Animator anim = animatingBody.GetComponent<Animator>();
       //float speed = anim.GetFloat("StabSpeed");
       //float stabTime = Utility.AnimationLength("CharArmature|Stab", animatingBody) / speed;
       float stabTime = 0.25f;

       
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
           //anim.SetBool("Idle", true);
           //anim.SetBool("Stabbing", true);
         }

         if (cooldown.timer > 0 && cooldown.timer < cooldown.duration - stabTime) {
           usable.inuse = false;
           //anim.SetBool("Stabbing", false);
           //anim.SetBool("Idle", true);
         }
       }
    });



    Entities.ForEach((Entity ent, DynamicBuffer<Hit> hitBuffer, ref StabHitbox stab, ref Rotation rot, ref Damage damage) => {
        
      List<Hit> hitList = new List<Hit>();
      for (int i = 0; i < hitBuffer.Length; i++) {
        var hit = hitBuffer[i];
        hit.knockback = math.rotate(rot.Value, Utility.v3tof3(Vector3.forward));
        hit.damage = damage.Value;
        hitBuffer[i] = hit;
        hitList.Add(hit);
      }

      for (int i = 0; i < hitList.Count; i++) {
        var hit = hitList[i];
        var hurtboxBuffer = EntityManager.GetBuffer<Hit>(hit.ent);
        hit.ent = ent;
        hurtboxBuffer.Add(hit);
      }
      hitBuffer.Clear();


    });




  }

  public static Entity AddAbility(Entity agent, EntityManager manager, GhostPrefabCollectionComponent ghostCollection) {

    //var ghostId = NetAgentGhostSerializerCollection.FindGhostType<SwordSnapshotData>();
    var ghostId = 3;
    var prefab = manager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
    var ability = manager.Instantiate(prefab);
    
    var agentGhostId = manager.GetComponentData<GhostComponent>(agent).ghostId;

    Cooldown cooldown; Usable usable; KeyCodeComp keycode; OwningPlayer player; Sword sword; 
    cooldown.timer = 0;
    cooldown.duration = 1;
    sword.damage = 10;
    keycode.Value = KeyCode.Space;
    usable.inuse = false;
    usable.canuse = true;
    player.Value = agent;
    player.PlayerId = manager.GetComponentData<AgentComponent>(agent).PlayerId;
    //trans.Value = new float3(100000,1,0); // so it starts invisible

    manager.SetComponentData<Cooldown>(ability, cooldown);
    manager.SetComponentData<Usable>(ability, usable);
    manager.SetComponentData<KeyCodeComp>(ability, keycode);
    manager.SetComponentData<Sword>(ability, sword);
    manager.SetComponentData<OwningPlayer>(ability, player);
    //manager.SetComponentData<Translation>(ability, trans);



   // initialize hitboxes
   ghostId = 6;
   prefab = manager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
   var swordHitbox = manager.Instantiate(prefab);
   manager.SetComponentData<OwningPlayer>(swordHitbox, player);
   manager.SetComponentData<AssociatedEntity>(swordHitbox, new AssociatedEntity {Value = ability});
   manager.SetComponentData<Damage>(swordHitbox, new Damage {Value = sword.damage});
   manager.SetComponentData<Disableable>(swordHitbox, new Disableable {disabled = false});
   manager.AddBuffer<Hit>(swordHitbox);
   var hitboxBuffer = manager.AddBuffer<HitboxElement>(ability).Reinterpret<Entity>();
   hitboxBuffer.Add(swordHitbox);

   return ability;
  }

}


// update health bar prefabs with correct health
// TODO: Make sure this happens after agent move update, otherwise it lags behind 
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SwordUpdateAnimationSystem : ComponentSystem
{
  protected override void OnUpdate() {

    Entities.ForEach((ref Sword sword, ref Usable usable, ref OwningPlayer player, ref Cooldown cooldown, ref Translation trans, ref Rotation rot) => 
    {
       if (EntityManager.HasComponent<AnimationInitialized>(player.Value)) {
         GameObject animatingBody = EntityManager.GetComponentObject<GameObject>(player.Value);
         if (animatingBody != null) {
           if (usable.inuse) {
             animatingBody.GetComponent<Animator>().SetBool("Idle", true);
             animatingBody.GetComponent<Animator>().SetBool("Stabbing", true);
           } else { 
             animatingBody.GetComponent<Animator>().SetBool("Stabbing", false);
           }
         }
       }
    });

    //Entities.WithNone<HitboxElement>().ForEach((Entity parent, ref Sword sword) => {
    //  Debug.Log("in here adding hitboxbuffer");
    //  var hitboxBuffer = EntityManager.AddBuffer<HitboxElement>(parent).Reinterpret<Entity>();
    //  Entities.ForEach((Entity hitbox, ref StabHitbox stab, ref AssociatedEntity ent) => {
    //    if (ent.Value == parent) {
    //      hitboxBuffer.Add(hitbox);
    //    }
    //  });
    //});
        
  }
}

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ClientSwordInit : ComponentSystem
{
    protected override void OnCreate() {}
    protected override void OnUpdate()
    {
        Entities.WithNone<HitboxElement>().ForEach((Entity ent, ref Sword sword) => {
          EntityManager.AddBuffer<HitboxElement>(ent);
        });

        //Debug.LogError("in onpudate in goingamesystem");
        Entities.WithNone<ClientInit>().ForEach((Entity ent, ref StabHitbox stab, ref AssociatedEntity sword) =>
        {
            var buffer = EntityManager.GetBuffer<HitboxElement>(sword.Value).Reinterpret<Entity>();
            buffer.Add(ent);
            PostUpdateCommands.AddComponent<ClientInit>(ent);
        });
    }
}





