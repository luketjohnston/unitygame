using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;





// TODO: we can probably compress this alot so only one detail is sent at a time,
// like only send destination when that changes, and only orientation when that changes, etc.
public struct AgentInput : ICommandData<AgentInput>
{
    public uint Tick => tick;
    public uint tick;
    // which input type are we sending?
    public ushort type;
    public float2 destination;
    public float2 target_orientation;
    public int ghostId;

    public void Deserialize(uint tick, ref DataStreamReader reader)
    {
        type = reader.ReadUShort();
        this.tick = reader.ReadUInt();

        switch (type) {
          case 1:
            destination.x = reader.ReadFloat();
            destination.y = reader.ReadFloat();
            break;
          case 2:
            target_orientation.x = reader.ReadFloat();
            target_orientation.y = reader.ReadFloat();
            break;
          case 3:
            ghostId = reader.ReadInt();
            break;
        }
    }
    public void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteUShort(type);
        writer.WriteUInt(tick);
        switch (type)
        {
          case 1:
            writer.WriteFloat(destination.x);
            writer.WriteFloat(destination.y);
            break;
          case 2:
            writer.WriteFloat(target_orientation.x);
            writer.WriteFloat(target_orientation.y);
            break;
          case 3:
            writer.WriteInt(ghostId);
            break;
        }
    }

    public void Deserialize(uint tick, ref DataStreamReader reader, AgentInput baseline,
        NetworkCompressionModel compressionModel)
    {
        Deserialize(tick, ref reader);
    }

    public void Serialize(ref DataStreamWriter writer, AgentInput baseline, NetworkCompressionModel compressionModel)
    {
        Serialize(ref writer);
    }
}




public class SendAgentInputCommandSystem : CommandSendSystem<AgentInput> { }
public class ReceiveAgentInputCommandSystem : CommandReceiveSystem<AgentInput> { }


[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SampleAgentInput : ComponentSystem
{
    Plane arenaPlane = new Plane(Vector3.up, Vector3.zero);
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
        RequireSingletonForUpdate<EnableNetAgentGhostReceiveSystemComponent>();
    }

    protected override void OnUpdate()
    {
        var playerAgent = GetSingleton<CommandTargetComponent>().targetEntity;
        var localPlayerId = GetSingleton<NetworkIdComponent>().Value;
        if (playerAgent == Entity.Null)
        {
            Entities.WithNone<AgentInput>().ForEach((Entity ent, ref AgentComponent agent) =>
            {
                if (agent.PlayerId == localPlayerId)
                {
                    PostUpdateCommands.AddBuffer<AgentInput>(ent);
                    PostUpdateCommands.SetComponent(GetSingletonEntity<CommandTargetComponent>(), new CommandTargetComponent {targetEntity = ent});
                }
            });
            return;
        }



        var input = default(AgentInput);
        input.tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;

        var game_position_c = EntityManager.GetComponentData<GamePosition>(playerAgent);
        var game_orientation_c = EntityManager.GetComponentData<GameOrientation>(playerAgent);
        var destination_c = EntityManager.GetComponentData<DestinationComponent>(playerAgent);
        var target_orientation_c = EntityManager.GetComponentData<TargetOrientation>(playerAgent);

        input.type = 0;

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("In mouse button down 1");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
            input.type = 1;
            input.destination = target;
            var inputBuffer = EntityManager.GetBuffer<AgentInput>(playerAgent);
            inputBuffer.AddCommandData(input);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
            //takeDamage(5);
            input.type = 2;
            input.target_orientation = new float2(target.x, target.y) - game_position_c.Value;

            var inputBuffer = EntityManager.GetBuffer<AgentInput>(playerAgent);
            inputBuffer.AddCommandData(input);

        }

        Entities.ForEach((Entity e, ref OwningPlayer player, ref KeyCodeComp key, ref Usable usable, ref GhostComponent ghost_comp) => {

          // TODO add check that agent is owning player
          if (Input.GetKeyDown(key.Value) && usable.canuse && player.Value == playerAgent) {
            input.type = 3;
            input.ghostId = ghost_comp.ghostId;

            var inputBuffer = EntityManager.GetBuffer<AgentInput>(playerAgent);
            inputBuffer.AddCommandData(input);
          }
        });
    }
}





[GenerateAuthoringComponent] 
public struct TrackedGhost : IComponentData {}


         
// Keep track of ghosts as they are created. Server creates all ghosts, this
// system just looks for newly created ghosts without a "TrackedGhost" component
// and adds them to the "Ghosts" collection
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class GhostClientSystem : ComponentSystem
{
  public static Dictionary<int, Entity> ghosts = new Dictionary<int, Entity>(); 
  protected override void OnUpdate()
  {
      Entities.WithNone<TrackedGhost>().ForEach((Entity ent, ref GhostComponent ghost_comp) =>
      {
          PostUpdateCommands.AddComponent<TrackedGhost>(ent);
          ghosts[ghost_comp.ghostId] = ent;
      });
  }

  public static Entity getGhost(int ghostId) {
    return ghosts[ghostId];
  }
}

// Keep track of ghosts as they are created. Server creates all ghosts, this
// system just looks for newly created ghosts without a "TrackedGhost" component
// and adds them to the "Ghosts" collection
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class GhostServerSystem : ComponentSystem
{
  public static Dictionary<int, Entity> ghosts = new Dictionary<int, Entity>(); 
  protected override void OnUpdate()
  {
      Entities.WithNone<TrackedGhost>().ForEach((Entity ent, ref GhostComponent ghost_comp) =>
      {
          PostUpdateCommands.AddComponent<TrackedGhost>(ent);
          ghosts[ghost_comp.ghostId] = ent;
      });
  }

  public static Entity getGhost(int ghostId) {
    return ghosts[ghostId];
  }
}




