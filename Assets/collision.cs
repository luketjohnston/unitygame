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
      hitboxGroup = GetComponentDataFromEntity<Hitbox>(),
      hitBufferGroup = GetBufferFromEntity<Hit>(),
      //swordGroup = GetComponentDataFromEntity<Sword>(),
      //swordHitboxGroup = GetComponentDataFromEntity<SwordHitbox>(),
      //shieldGroup = GetComponentDataFromEntity<Shield>(),
      //shieldHitboxGroup = GetComponentDataFromEntity<ShieldHitbox>(),
      //healthGroup = GetComponentDataFromEntity<Health>(),
      //associatedEntityGroup = GetComponentDataFromEntity<AssociatedEntity>(),
      //playerGroup = GetComponentDataFromEntity<OwningPlayer>(),

      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
    };

    JobHandle handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    commandBufferSystem.AddJobHandleForProducer(handle);
    return handle;
  }

  private struct ApplicationJob : ITriggerEventsJob {

    [ReadOnly] public ComponentDataFromEntity<Hurtbox> hurtboxGroup;
    [ReadOnly] public ComponentDataFromEntity<Hitbox> hitboxGroup;
    public BufferFromEntity<Hit> hitBufferGroup;

    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(TriggerEvent triggerEvent) {
      Entity entityA = triggerEvent.EntityA;
      Entity entityB = triggerEvent.EntityB;


      if (hurtboxGroup.HasComponent(entityA) && hurtboxGroup.HasComponent(entityB)) {
        return;
        // two hurtboxes collide
      }

      if (hitboxGroup.HasComponent(entityA) && hitboxGroup.HasComponent(entityB)) {
        return;
        // two hitboxes collide
      }

      if (hurtboxGroup.HasComponent(entityA) && hitboxGroup.HasComponent(entityB)) {
        // add hit and hurt to respective buffers
        hitBufferGroup[entityA].Reinterpret<Entity>().Add(entityB);
        hitBufferGroup[entityB].Reinterpret<Entity>().Add(entityA);
      }

      if (hurtboxGroup.HasComponent(entityB) && hitboxGroup.HasComponent(entityA)) {
        // add hit and hurt to respective buffers
        hitBufferGroup[entityA].Reinterpret<Entity>().Add(entityB);
        hitBufferGroup[entityB].Reinterpret<Entity>().Add(entityA);

        // set indicators both on hurtbox and hitboxes, for which entity hit / was hit.
        //Hurtbox hurtbox = hurtboxGroup[entityB];
        //hurtbox.hitToProcess = entityA;
        //hurtboxGroup[entityB] = hurtbox;

        //Hitbox hitbox = hitboxGroup[entityA];
        //hitbox.hitToProcess = entityB;
        //hitboxGroup[entityA] = hitbox;

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

