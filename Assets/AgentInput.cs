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
    // the bits indicate, in order:
    // 00000001: whether we are sending a location
    // 00000010: whether we are sending an orientation
    // 00000100: whether we are sending a ghostId (for used ability)
    // 00001000: whether we are sending an angle
    // 00010000: indicates rotate left
    // 00100000: indicates rotate right
    // 01000000: indicates update destination
    // 10000000: indicates ability released
    public int type;


    public float2 location;
    public float2 orientation;
    public float angle;
    public int ghostId;
    public int ghostId_released;

    public void UpdateDestination(float2 destination) {
      this.type = this.type | 0b1000001;
      this.location = destination;
    }

    public void UseAbility(int ghostId) {
      this.type = this.type | 0b100;
      this.ghostId = ghostId;
    }
    public void UseAbility(int ghostId, float2 location) {
      this.type = this.type | 0b101;
      this.ghostId = ghostId;
      this.location = location;
    }
    public void UseAbility(int ghostId, float angle) {
      this.type = this.type | 0b1100;
      this.ghostId = ghostId;
      this.angle = angle;
    }
    public void RotateLeft() {
      this.type = this.type | 0b10000;
    }
    public void RotateRight() {
      this.type = this.type | 0b100000;
    }

    // not using this currently
    public void SendOrientation(float2 orientation) {
      this.type = this.type | 0b10;
      this.orientation = orientation;
    }

    public bool DestinationUpdated() {
      return (this.type & 0b1000000) != 0;
    }
    public bool AbilityUsed() {
      return (this.type & 0b100) != 0;
    }

    public bool AbilityReleased() {
      return (this.type & 0b10000000) != 0;
    }
    public bool RotatingLeft() {
      return (this.type & 0b10000) != 0;
    }
    public bool RotatingRight() {
      return (this.type & 0b100000) != 0;
    }
    public bool AngleSent() {
      return (this.type & 0b1000) != 0;
    }

    public void Deserialize(uint tick, ref DataStreamReader reader)
    {
        type = reader.ReadInt();
        this.tick = reader.ReadUInt();

        if ((type & 0b1) != 0) {
          location.x = reader.ReadFloat();
          location.y = reader.ReadFloat();
        }
        if ((type & 0b10) != 0) {
          orientation.x = reader.ReadFloat();
          orientation.y = reader.ReadFloat();
        }
        if ((type & 0b100) != 0) {
          ghostId = reader.ReadInt();
        }
        if ((type & 0b1000) != 0) {
          angle = reader.ReadFloat();
        }
        if ((type & 0b10000) != 0) {
          // pass (rotate left)
        }
        if ((type & 0b100000) != 0) {
          // pass (rotate right)
        }
        if ((type & 0b10000000) != 0) {
          ghostId_released = reader.ReadInt();
        }

    }
    public void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteInt(type);
        writer.WriteUInt(tick);

        if ((type & 0b1) != 0) {
            writer.WriteFloat(location.x);
            writer.WriteFloat(location.y);
        }
        if ((type & 0b10) != 0) {
            writer.WriteFloat(orientation.x);
            writer.WriteFloat(orientation.y);
        }
        if ((type & 0b100) != 0) {
            writer.WriteInt(ghostId);
        }
        if ((type & 0b1000) != 0) {
            writer.WriteFloat(angle);
        }
        if ((type & 0b10000) != 0) {
            //pass (rotate left)
        }
        if ((type & 0b100000) != 0) {
            //pass (rotate right)
        }
        if ((type & 0b10000000) != 0) {
            writer.WriteInt(ghostId_released);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));

            input.type = input.type | 0b1;
            input.type = input.type | 0b1000000;

            input.location = target;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
            //takeDamage(5);
            input.type = input.type | 0b10;
            input.orientation = new float2(target.x, target.y) - game_position_c.Value;

        }

        if (Input.GetKey(KeyCode.F) && !Input.GetKey(KeyCode.G)) {
          input.type = input.type | 0b10000; // rotate left
        } else if (Input.GetKey(KeyCode.G)) {
          input.type = input.type | 0b100000; // rotate left
        }

        // TODO this is being called for entities with an AngleInput, which we only want called below.
        // Probably should refactor whole input system, this one is very clunky. SHould just send all available data every time
        // (would prevent current bug where if you enter two inputs at exactly the same time, only one is sent).
        Entities.ForEach((Entity e, ref OwningPlayer player, ref KeyCodeComp key, ref Usable usable, ref GhostComponent ghost_comp) => {
          // TODO: is really getKey best? switched to it so shield would work
          if (Input.GetKey(key.Value) && usable.canuse && player.Value == playerAgent) {
            input.type = input.type | 0b100;
            input.ghostId = ghost_comp.ghostId;
          }
        });

        // Have not tested this line of code, just put in here since i'll need it later (added when added angle input)
        Entities.ForEach((Entity e, ref OwningPlayer player, ref KeyCodeComp key, ref Usable usable, ref GhostComponent ghost_comp, ref LocationInput loc) => {
          // TODO add check that agent is owning player
          if (Input.GetKeyDown(key.Value) && usable.canuse && player.Value == playerAgent) {
            input.type = input.type | 0b1;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            input.location = Utility.v3tof2(ray.GetPoint(ray_enter));
          }
        });

        Entities.ForEach((Entity e, ref OwningPlayer player, ref KeyCodeComp key, ref Usable usable, ref GhostComponent ghost_comp, ref AngleInput angle) => {
          // TODO: should we change everything to GetKey?
          if (Input.GetKey(key.Value) && usable.canuse && player.Value == playerAgent) {
            input.type = input.type | 0b1000;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float ray_enter = 0f;
            arenaPlane.Raycast(ray, out ray_enter);
            Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
            float2 input_orientation = new float2(target.x, target.y) - game_position_c.Value;
            float2 player_orientation = EntityManager.GetComponentData<GameOrientation>(playerAgent).Value;
            float input_angle = Vector2.SignedAngle(Utility.f2tov2(input_orientation), Utility.f2tov2(player_orientation));
            input.angle = input_angle;

          }
        });

        Entities.ForEach((Entity e, ref OwningPlayer player, ref KeyCodeComp key, ref Usable usable, ref GhostComponent ghost_comp, ref Releasable releasable) => {
            if (usable.inuse && player.Value == playerAgent) {
              if (!Input.GetKey(key.Value)) {
                input.type = input.type | 0b10000000;
                input.ghostId_released = ghost_comp.ghostId;
              }
            }
        });
                




        if (input.type != 0) {
          var inputBuffer = EntityManager.GetBuffer<AgentInput>(playerAgent);
          inputBuffer.AddCommandData(input);
        }
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




