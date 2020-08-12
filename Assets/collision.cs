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

    Debug.Log("in my update");

    var job = new ApplicationJob {
      agentGroup = GetComponentDataFromEntity<AgentComponent>(),
      swordGroup = GetComponentDataFromEntity<Sword>(),
      healthGroup = GetComponentDataFromEntity<Health>(),
      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
    };

    JobHandle handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    commandBufferSystem.AddJobHandleForProducer(handle);
    return handle;
  }

  private struct ApplicationJob : ITriggerEventsJob {

    [ReadOnly] public ComponentDataFromEntity<AgentComponent> agentGroup;
    [ReadOnly] public ComponentDataFromEntity<Sword> swordGroup;
    public ComponentDataFromEntity<Health> healthGroup;

    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(TriggerEvent triggerEvent) {

      Debug.Log("in execute");

      Entity entityA = triggerEvent.EntityA;
      Entity entityB = triggerEvent.EntityB;


      // check if both are same type
      if (agentGroup.Exists(entityA) && agentGroup.Exists(entityB)) {
        return;
      }

      // check if both are same type
      if (swordGroup.Exists(entityA) && swordGroup.Exists(entityB)) {
        return;
      }

      if (agentGroup.HasComponent(entityA) && swordGroup.HasComponent(entityB)) {
        Health modHealth = healthGroup[entityA]; 
        modHealth.Value -= swordGroup[entityB].damage;
        healthGroup[entityA] = modHealth;

        Debug.Log("Detected collicion 1");
      }

      if (swordGroup.HasComponent(entityA) && agentGroup.HasComponent(entityB)) {
        Health modHealth = healthGroup[entityB]; 
        modHealth.Value -= swordGroup[entityA].damage;
        healthGroup[entityB] = modHealth;
        Debug.Log("Detected collicion 2");
        //Rotate rotate = rotateGroup[triggerEvent.Entities.EntityB];
        //rotate.radiansPerSecond = math.radians(90f);
        //rotateGroup[triggerEvent.Entities.EntityB] = rotate;
      }
    }
  }
}
      



