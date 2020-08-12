using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;



[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class KeySystem : ComponentSystem {

    protected List<KeyCode> keys = new List<KeyCode>();
    protected int usedKeys = 0;

    protected override void OnCreate() {
      keys.Add(KeyCode.Q);
      keys.Add(KeyCode.W);
      keys.Add(KeyCode.E);
      keys.Add(KeyCode.R);
    }


    protected override void OnUpdate() {
        var playerAgent = GetSingleton<CommandTargetComponent>().targetEntity;
        //var localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player, ref Sword sword) => {
            if (player.Value == playerAgent && key.Value == KeyCode.None) {
              key.Value = KeyCode.Space;
            }
        });

        Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player, ref Dash dash) => {
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.x == 1) {
              key.Value = KeyCode.S;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.x == -1) {
              key.Value = KeyCode.A;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.z == 1) {
              key.Value = KeyCode.F;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.z == -1) {
              key.Value = KeyCode.D;
            }
        });

        //Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player) => {
        //    if (player.Value == playerAgent && key.Value == KeyCode.None) {
        //      key.Value = keys[usedKeys];
        //      usedKeys += 1;
        //    }
        //});
    }
}
              

              



