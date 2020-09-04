using UnityEngine;
using Unity.Entities;

public class SwordMono : MonoBehaviour {
  public Entity entity;
  public Entity player;
  public Entity stabee;

  //void OnTriggerEnter(Collider collider) {
  //  {
  //    // for a collision, we just update the root's collision data in PlayerMono. Then a 
  //    // separate collision processing system will handle it.
  //    var root = collider.transform.root;
  //    PlayerMono opponentMono = root.GetComponent<PlayerMono>();
  //    PlayerMono selfMono = this.transform.root.GetComponent<PlayerMono>();
  //    if (player != opponentMono.entity) {
  //      // sword collides with sword
  //      SwordMono swordMono = collider.gameObject.GetComponent<SwordMono>();
  //      ShieldMono shieldMono = collider.gameObject.GetComponent<ShieldMono>();

  //      if (swordMono != null) {
  //        //do nothing for now
  //      // sword collides with shield
  //      } else if (shieldMono != null) {
  //        stabee = shieldMono.entity;
  //        //selfMono.collision_type = 2;
  //      // sword collides with character
  //      } else {
  //        stabee = opponentMono.entity;
  //        //opponentMono.collision_type = 1;
  //      }
  //    }
  //  }
  //}

}
