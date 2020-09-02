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
      keys.Add(KeyCode.Z);
      keys.Add(KeyCode.X);
      keys.Add(KeyCode.C);
      keys.Add(KeyCode.V);
    }


    protected override void OnUpdate() {
        var playerAgent = GetSingleton<CommandTargetComponent>().targetEntity;
        //var localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player, ref Sword sword) => {
            if (player.Value == playerAgent && key.Value == KeyCode.None) {
              key.Value = KeyCode.Space;
            }
        });

        Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player, ref Shield shield) => {
            if (player.Value == playerAgent && key.Value == KeyCode.None) {
              key.Value = KeyCode.S;
            }
        });

        Entities.ForEach((ref KeyCodeComp key, ref OwningPlayer player, ref Dash dash) => {
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.x == 1) {
              key.Value = KeyCode.W;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.x == -1) {
              key.Value = KeyCode.Q;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.z == 1) {
              key.Value = KeyCode.E;
            }
            if (player.Value == playerAgent && key.Value == KeyCode.None && dash.dir.z == -1) {
              key.Value = KeyCode.R;
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
              

              



