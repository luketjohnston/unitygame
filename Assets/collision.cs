using Unity.Entities;
using UnityEngine;
using Unity.NetCode;
using Unity.Physics.Systems;
using Unity.Networking.Transport;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;




[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class CollisionSystem : JobComponentSystem {

  private BuildPhysicsWorld buildPhysicsWorld;
  private StepPhysicsWorld stepPhysicsWorld;
  private EndSimulationEntityCommandBufferSystem commandBufferSystem;
  private EntityManager entityManager;

  protected override void OnCreate() {
    buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    entityManager = EntityManager;
  }

  protected override JobHandle OnUpdate(JobHandle inputDeps) {


    var job = new ApplicationJob {
      hurtboxGroup = GetComponentDataFromEntity<Hurtbox>(),
      swordGroup = GetComponentDataFromEntity<Sword>(),
      swordHitboxGroup = GetComponentDataFromEntity<SwordHitbox>(),
      shieldGroup = GetComponentDataFromEntity<Shield>(),
      shieldHitboxGroup = GetComponentDataFromEntity<ShieldHitbox>(),
      healthGroup = GetComponentDataFromEntity<Health>(),
      associatedEntityGroup = GetComponentDataFromEntity<AssociatedEntity>(),
      playerGroup = GetComponentDataFromEntity<OwningPlayer>(),

      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
    };

    JobHandle handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    commandBufferSystem.AddJobHandleForProducer(handle);
    return handle;
  }

  private struct ApplicationJob : ITriggerEventsJob {

    [ReadOnly] public ComponentDataFromEntity<Hurtbox> hurtboxGroup;
    [ReadOnly] public ComponentDataFromEntity<Sword> swordGroup;
    [ReadOnly] public ComponentDataFromEntity<SwordHitbox> swordHitboxGroup;
    [ReadOnly] public ComponentDataFromEntity<Shield> shieldGroup;
    [ReadOnly] public ComponentDataFromEntity<ShieldHitbox> shieldHitboxGroup;
    [ReadOnly] public ComponentDataFromEntity<AssociatedEntity> associatedEntityGroup;
    [ReadOnly] public ComponentDataFromEntity<OwningPlayer> playerGroup;

    public ComponentDataFromEntity<Health> healthGroup;

    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(TriggerEvent triggerEvent) {
      Debug.Log("Processing trigger event");
      Entity entityA = triggerEvent.EntityA;
      Debug.Log(entityA);
      Entity entityB = triggerEvent.EntityB;
      Debug.Log(entityB);


      if (hurtboxGroup.Exists(entityA) && hurtboxGroup.Exists(entityB)) {
        return;
        // two agents collide
      }

      if (swordHitboxGroup.Exists(entityA) && swordHitboxGroup.Exists(entityB)) {
        return;
        // two swords collide
      }

      if (hurtboxGroup.HasComponent(entityA) && swordHitboxGroup.HasComponent(entityB)) {
        Debug.Log("IN HERE 1");
        OwningPlayer player = playerGroup[entityA];
        Health modHealth = healthGroup[player.Value]; 
        Sword sword = swordGroup[associatedEntityGroup[entityB].Value];
        modHealth.Value -= sword.damage;
        healthGroup[player.Value] = modHealth;
      }

      if (hurtboxGroup.HasComponent(entityB) && swordHitboxGroup.HasComponent(entityA)) {
        Debug.Log("IN HERE 2");
        OwningPlayer player = playerGroup[entityB];
        Health modHealth = healthGroup[player.Value]; 
        Sword sword = swordGroup[associatedEntityGroup[entityA].Value];
        modHealth.Value -= sword.damage;
        healthGroup[player.Value] = modHealth;
      }
    }
  }
}


//[UpdateAfter(typeof(EndFramePhysicsSystem))]
//[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
//public class CollisionSystem : ComponentSystem {
//
//  protected override void OnUpdate() {
//    Entities.ForEach((Entity ent, ref Sword sword, ref OwningPlayer player, ref Usable usable) => {
//      SwordMono swordMono = EntityManager.GetComponentObject<SwordMono>(ent);
//      ComponentDataFromEntity<Shield> shieldGroup = GetComponentDataFromEntity<Shield>();
//      ComponentDataFromEntity<BusyTimer> busyTimerGroup = GetComponentDataFromEntity<BusyTimer>();
//      ComponentDataFromEntity<FreezeTimer> freezeTimerGroup = GetComponentDataFromEntity<FreezeTimer>();
//
//      if (swordMono.stabee != Entity.Null) {
//        if (usable.inuse) {
//          if (shieldGroup.HasComponent(swordMono.stabee)) {
//            //GameObject playerObj = EntityManager.GetComponentObject<GameObject>(player.Value);
//            //Animator anim = playerObj.GetComponent<Animator>();
//            //anim.SetBool("Stabbing", false);
//            
//            usable.inuse = false;
//            busyTimerGroup[player.Value] = new BusyTimer{Value = 0.1f};
//            freezeTimerGroup[player.Value] = new FreezeTimer{Value = 0.1f};
//            swordMono.stabee = Entity.Null;
//          } else {
//
//          
//            //Debug.Log("detected collision!");
//            ComponentDataFromEntity<Health> healthGroup = GetComponentDataFromEntity<Health>();
//            Health modHealth = healthGroup[swordMono.stabee];
//            modHealth.Value -= sword.damage;
//            healthGroup[swordMono.stabee] = modHealth;
//          }
//        }
//      }
//    });
//  }
//
//  //protected override void OnUpdate() {
//  //  Entities.ForEach((ref Sword sword, ref OwningPlayer player) => {
//  //    GameObject obj = EntityManager.GetComponentObject<GameObject>(player.Value);
//  //    //Debug.Log(obj.name);
//  //    PlayerMono mono = obj.GetComponent<PlayerMono>();
//  //    //Debug.Log(mono);
//  //    if (mono.collision != Entity.Null) {
//  //      if (mono.collision_type == 1) {
//  //        //Debug.Log("detected collision!");
//  //        ComponentDataFromEntity<Health> healthGroup = GetComponentDataFromEntity<Health>();
//  //        Health modHealth = healthGroup[player.Value];
//  //        modHealth.Value -= sword.damage;
//  //        healthGroup[player.Value] = modHealth;
//  //      } else if (mono.collision_type == 2) {
//  //        // pass 
//  //      }
//  //    }
//  //  });
//  //}
//
//}

