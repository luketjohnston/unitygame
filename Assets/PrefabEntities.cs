using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PrefabEntities : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {

  public static Entity healthBar;
  public GameObject healthBarObj;


  public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
    healthBar = conversionSystem.GetPrimaryEntity(healthBarObj);
  }

  public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
    referencedPrefabs.Add(healthBarObj);
  }
}


public class EntityPrefabConversionSystem : GameObjectConversionSystem {
  protected override void OnUpdate() {
    
  }
}
