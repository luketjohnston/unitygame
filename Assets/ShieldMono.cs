using UnityEngine;
using Unity.Entities;

public class ShieldMono : MonoBehaviour {
  public Entity entity;
  public Entity player;
  public Entity blocked;

  void OnTriggerEnter(Collider collider) {
    {
      // for a collision, we just update the root's collision data in PlayerMono. Then a 
      // separate collision processing system will handle it.
      var root = collider.transform.root;
      PlayerMono opponentMono = root.GetComponent<PlayerMono>();
      PlayerMono selfMono = this.transform.root.GetComponent<PlayerMono>();
      if (player != opponentMono.entity) {
        // shield collides with sword
        SwordMono swordMono = collider.gameObject.GetComponent<SwordMono>();
        // shield collides with shield
        ShieldMono shieldMono = collider.gameObject.GetComponent<ShieldMono>();

        if (swordMono != null) {
          //do nothing for now
        // sword collides with shield
        } else if (shieldMono != null) {
          blocked = shieldMono.entity;
          //selfMono.collision_type = 2;
        // sword collides with character
        } else {
          blocked = opponentMono.entity;
          //opponentMono.collision_type = 1;
        }
      }
    }
  }

}
