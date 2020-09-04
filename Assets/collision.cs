//using Unity.Entities;
//using UnityEngine;
//using Unity.NetCode;
//using Unity.Physics.Systems;
//using Unity.Networking.Transport;
//using Unity.Physics;
//using Unity.Transforms;
//using Unity.Mathematics;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Collections;
//
//
//
//
///* The below is the pure ecs collisions system. Currently using hybrid approach
// *
//[UpdateAfter(typeof(EndFramePhysicsSystem))]
//[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
//public class CollisionSystem : JobComponentSystem {
//
//  private BuildPhysicsWorld buildPhysicsWorld;
//  private StepPhysicsWorld stepPhysicsWorld;
//  private EndSimulationEntityCommandBufferSystem commandBufferSystem;
//  private EntityManager entityManager;
//
//  protected override void OnCreate() {
//    buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
//    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
//    commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//    entityManager = EntityManager;
//  }
//
//  protected override JobHandle OnUpdate(JobHandle inputDeps) {
//
//
//    var job = new ApplicationJob {
//      agentGroup = GetComponentDataFromEntity<AgentComponent>(),
//      swordGroup = GetComponentDataFromEntity<Sword>(),
//      healthGroup = GetComponentDataFromEntity<Health>(),
//      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
//    };
//
//    JobHandle handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
//    commandBufferSystem.AddJobHandleForProducer(handle);
//    return handle;
//  }
//
//  private struct ApplicationJob : ITriggerEventsJob {
//
//    [ReadOnly] public ComponentDataFromEntity<AgentComponent> agentGroup;
//    [ReadOnly] public ComponentDataFromEntity<Sword> swordGroup;
//    public ComponentDataFromEntity<Health> healthGroup;
//
//    public EntityCommandBuffer entityCommandBuffer;
//
//    public void Execute(TriggerEvent triggerEvent) {
//
//
//      Entity entityA = triggerEvent.EntityA;
//      Entity entityB = triggerEvent.EntityB;
//
//
//      // check if both are same type
//      if (agentGroup.Exists(entityA) && agentGroup.Exists(entityB)) {
//        return;
//      }
//
//      // check if both are same type
//      if (swordGroup.Exists(entityA) && swordGroup.Exists(entityB)) {
//        return;
//      }
//
//      if (agentGroup.HasComponent(entityA) && swordGroup.HasComponent(entityB)) {
//        Health modHealth = healthGroup[entityA]; 
//        modHealth.Value -= swordGroup[entityB].damage;
//        healthGroup[entityA] = modHealth;
//
//      }
//
//      if (swordGroup.HasComponent(entityA) && agentGroup.HasComponent(entityB)) {
//        Health modHealth = healthGroup[entityB]; 
//        modHealth.Value -= swordGroup[entityA].damage;
//        healthGroup[entityB] = modHealth;
//        //Rotate rotate = rotateGroup[triggerEvent.Entities.EntityB];
//        //rotate.radiansPerSecond = math.radians(90f);
//        //rotateGroup[triggerEvent.Entities.EntityB] = rotate;
//      }
//    }
//  }
//}
//*/
//
//
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
//
//
//}
//
