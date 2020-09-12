using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


public struct RenderedHealthBar : IComponentData {}
public struct HealthBarComp : IComponentData {}
public struct HealthDisplayComp : IComponentData {}


//public class HealthBar : MonoBehaviour
//{
//  public Slider slider;
//
//  // always called after update function
//  void LateUpdate() {
//    transform.LookAt(transform.position + Camera.main.transform.forward);
//  }
//
//  public void setMaxHealth(int health) {
//    slider.maxValue = health;
//  }
//
//  public void setHealth(int health) {
//    slider.value = health;
//  }
//}


// regen health
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class HealthRegenSystem : ComponentSystem {
  protected override void OnUpdate() {
    var deltaTime = Time.DeltaTime;
    Entities.ForEach((ref Health health) => {
      health.Value += health.regen * deltaTime;
      health.Value = Mathf.Min(health.Value, health.max);
      health.Value = Mathf.Max(health.Value, 0f);
    });
  }
}


// update health bar prefabs with correct health
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class HealthBarSystem : ComponentSystem
{

  protected override void OnCreate() {
  }

  protected override void OnUpdate() {

    Entities.WithNone<RenderedHealthBar>().ForEach((Entity agent, ref Health health) => {

       // TODO: why readonly? from what I understand, this means we can't modify it here, but POstUpdateCommands can always modify
       var myHealthBar = EntityManager.CreateEntity(
           ComponentType.ReadOnly<LocalToWorld>(),
           ComponentType.ReadOnly<RenderMesh>(),
           ComponentType.ReadOnly<RenderBounds>(),
           ComponentType.ReadOnly<Translation>(),
           ComponentType.ReadOnly<Rotation>(),
           ComponentType.ReadOnly<CompositeScale>(),
           ComponentType.ReadOnly<HealthBarComp>(),
           ComponentType.ReadOnly<OwningPlayer>()
       );

       var myHealthDisplay = EntityManager.CreateEntity(
           ComponentType.ReadOnly<LocalToWorld>(),
           ComponentType.ReadOnly<RenderMesh>(),
           ComponentType.ReadOnly<RenderBounds>(),
           ComponentType.ReadOnly<Translation>(),
           ComponentType.ReadOnly<Rotation>(),
           ComponentType.ReadOnly<CompositeScale>(),
           ComponentType.ReadOnly<OwningPlayer>(),
           ComponentType.ReadOnly<HealthDisplayComp>()
       );

       EntityManager.SetComponentData(myHealthBar, new LocalToWorld {
           Value = new float4x4(rotation: quaternion.identity, translation: new float3(0,0,0))
       });
       EntityManager.SetComponentData(myHealthDisplay, new LocalToWorld {
           Value = new float4x4(rotation: quaternion.identity, translation: new float3(0,0,0))
       });

       EntityManager.SetComponentData(myHealthBar, new Translation { Value = new float3(0,1.5f,0) });
       EntityManager.SetComponentData(myHealthDisplay, new Translation { Value = new float3(0,1.5f,0) });

       EntityManager.SetComponentData(myHealthBar, new Rotation { Value = Quaternion.Euler(0,40,0) });
       EntityManager.SetComponentData(myHealthDisplay, new Rotation { Value = Quaternion.Euler(0,40,0) });

       EntityManager.SetComponentData(myHealthBar, new CompositeScale { Value = float4x4.Scale(0.05f)});
       EntityManager.SetComponentData(myHealthDisplay, new CompositeScale { Value = float4x4.Scale(0.05f) });

       EntityManager.SetComponentData(myHealthBar, new OwningPlayer { Value = agent });
       EntityManager.SetComponentData(myHealthDisplay, new OwningPlayer { Value = agent });


       // TODO make sure this is actually shared data across all health bars... do we have to make new one every time?
       EntityManager.SetSharedComponentData(myHealthBar, new RenderMesh {
           mesh = Resources.Load<Mesh>("ObjectModels/HealthBar"),
           material = Resources.Load<Material>("Materials/HealthBar")
       });

       EntityManager.SetSharedComponentData(myHealthDisplay, new RenderMesh {
           mesh = Resources.Load<Mesh>("ObjectModels/HealthDisplay"),
           material = Resources.Load<Material>("Materials/HealthDisplay")
       });

       PostUpdateCommands.AddComponent<RenderedHealthBar>(agent);

       Debug.Log("GOt here, made health bar");
     });

    Entities.WithAll<HealthBarComp>().ForEach((ref Translation trans, ref OwningPlayer owner) => {
        var owner_trans = EntityManager.GetComponentData<Translation>(owner.Value);
        trans.Value = owner_trans.Value + new float3(0, 6.5f, 0);
    });

    Entities.WithAll<HealthDisplayComp>().ForEach((ref Translation trans, ref OwningPlayer owner, ref CompositeScale scale) => {
        var owner_trans = EntityManager.GetComponentData<Translation>(owner.Value);
        trans.Value = owner_trans.Value + new float3(0, 6.5f, 0);

        var health = EntityManager.GetComponentData<Health>(owner.Value);
        var healthScale = health.Value / health.max;
        //Debug.LogError("health");
        //Debug.LogError(health.Value);
        //Debug.LogError(health.max);
        

        scale.Value = float4x4.Scale(0.05f) * float4x4.Scale(healthScale,1,1);



    });




  }
}

