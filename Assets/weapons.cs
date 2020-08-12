using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class Weapon : Ability {
//
//  protected GameObject obj;
//
//
//
//  public Weapon(GameObject prefab, Agent agent, float cooldown, KeyCode key) : base (cooldown, agent, key) {
//    // initiate weapon from prefab as child of agent
//    obj = Agent.Instantiate(prefab, agent.transform.position, agent.transform.rotation, agent.transform);
//    obj.SetActive(false);
//  }
//
//  // TODO: add collider points?
//  //public abstract float getDamage(Agent attacker, Agent victim);
//
//}
//
//
//
//public class Sword : Weapon {
//
//  protected static GameObject prefab = Resources.Load("sword1") as GameObject;
//  protected float rotated = 0;
//  protected static float rotSpeed = 720;
//
//  public Sword(Agent agent, float cooldown, KeyCode key) : base (prefab, agent, cooldown, key) {
//    obj.transform.localPosition = new Vector3(2f, 0f, 0f);
//    obj.transform.eulerAngles = new Vector3(90f, 180f, 0f);
//    obj.transform.RotateAround(agent.transform.position, Vector3.up, -90);
//    Debug.Log(obj);
//    // pass
//  }
//
//  protected override void startAbility() {
//    obj.SetActive(true);
//    rotated = 0;
//    in_use = true;
//  }
//
//  protected override void updateAbility() {
//    obj.transform.RotateAround(agent.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
//    rotated += rotSpeed * Time.deltaTime;
//    if (rotated > 180) {
//      in_use = false;
//      obj.SetActive(false);
//      obj.transform.RotateAround(agent.transform.position, Vector3.up, -rotated);
//    }
//  }
//}
//
//
//
