using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;


// TODO: remove elements from AgentInput buffer? in entity inspector it seems they pile up.


// TODO write system for checking if abilites are in use / can be used
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class CooldownSystem : ComponentSystem
{
  protected override void OnUpdate() {
    var group = World.GetExistingSystem<ServerSimulationSystemGroup>();
    var tick = group.ServerTick;

    var deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Cooldown cooldown, ref Usable usable) => 
    {
      if (cooldown.timer > 0) {
        cooldown.timer -= deltaTime;
        Debug.Log("Decreasing cooldown");
        if (cooldown.timer <= 0) {
          Debug.Log("Setting canuse true");
          usable.canuse = true;
        }
      }
    });
  }
}












//public abstract class Ability {
//  protected float cooldown;
//  protected float cooldown_timer;
//  protected bool in_use;
//  protected Agent agent;
//  protected KeyCode key;
//
//  public Ability(float cooldown, Agent agent, KeyCode key) {
//    this.cooldown = cooldown;
//    this.cooldown_timer = 0;
//    this.in_use = false;
//    this.agent = agent;
//    this.key = key;
//  }
//
//  public void update() {
//    if (in_use) {
//      updateAbility();
//    }
//    if (cooldown_timer > 0) {
//      cooldown_timer -= Time.deltaTime;
//    } else if (Input.GetKeyDown(key) && agent.can_move && !in_use) {
//      cooldown_timer = cooldown;
//      startAbility();
//    }
//  }
//
//  // called to start using an ability. May be overriden. Must set in_use = true.
//  protected virtual void startAbility() {
//    in_use = true;
//  }
//  // called when in_use is "true" to process an ability.
//  protected abstract void updateAbility();
//}
//
////class ForwardBlink : Ability {
////  float cooldown = 0.5f;
////  float distance = 5f;
////  public ForwardBlink(Agent agent) : base(cooldown, agent) {
////    // pass
////  }
////
////  public override void update() {
////    if (in_use) {
////      // do ability, update in_use if necessary
////    }
////    if (cooldown_timer > 0) {
////      cooldown_timer -= Time.deltaTime;
////    } else if (Input.GetKeyDown(KeyCode.Space)) {
////      cooldown_timer = cooldown;
////      //bool at_dest = agent.atDestination();
////      agent.transform.localPosition += Utility.v2to3(agent.OrientationVector()) * distance;
////      //if (at_dest) {
////      agent.destination = Utility.v3to2(agent.transform.localPosition);
////      //}
////    }
////  }
////}
//
//
//class DirectedDash : Ability {
//  protected float angle;
//  protected float speed;
//  protected float distance_traveled;
//  protected float distance;
//  public DirectedDash(Agent agent, float angle, float speed, float distance, float cooldown, KeyCode key) : base(cooldown, agent, key) {
//    this.angle = angle;
//    this.speed = speed;
//    this.distance = distance;
//    this.distance_traveled = 0f;
//  }
//
//  protected override void startAbility() {
//    base.startAbility();
//    agent.can_move = false;
//    agent.can_rotate = false;
//  }
//
//  protected override void updateAbility() {
//    // do ability, update in_use if necessary
//    Vector3 change = Utility.v2to3(agent.OrientationVector()) * speed;
//    change = Quaternion.Euler(0, angle, 0) * change;
//    agent.transform.localPosition += change;
//    distance_traveled += speed;
//    if (distance_traveled >= distance) {
//      in_use = false;
//      agent.can_move = true;
//      agent.can_rotate = true;
//      agent.destination = Utility.v3to2(agent.transform.localPosition);
//      distance_traveled = 0;
//    }
//  }
//}
//
//class DirectedBlink : Ability {
//  protected float angle;
//  protected float distance;
//  public DirectedBlink(Agent agent, float angle, float distance, float cooldown, KeyCode key) : base(cooldown, agent, key) {
//    this.angle = angle;
//    this.distance = distance;
//  }
//
//
//
//  protected override void updateAbility() {
//    // do ability, update in_use if necessary
//    Vector3 change = Utility.v2to3(agent.OrientationVector()) * distance;
//    change = Quaternion.Euler(0, angle, 0) * change;
//    agent.transform.localPosition += change;
//    in_use = false;
//    agent.destination = Utility.v3to2(agent.transform.localPosition);
//  }
//}


    

