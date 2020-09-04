using System;
using Unity.Mathematics;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using Unity.Burst;

// Control system updating in the default world
[UpdateInWorld(UpdateInWorld.TargetWorld.Default)]
public class Game : ComponentSystem
{
    // Singleton component to trigger connections once from a control system
    struct InitGameComponent : IComponentData
    {
    }
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<InitGameComponent>();
        // Create singleton, require singleton for update so system runs once
        EntityManager.CreateEntity(typeof(InitGameComponent));
    }

    protected override void OnUpdate()
    {
        // Destroy singleton to prevent system from running again
        EntityManager.DestroyEntity(GetSingletonEntity<InitGameComponent>());
        foreach (var world in World.All)
        {
            var network = world.GetExistingSystem<NetworkStreamReceiveSystem>();
            if (world.GetExistingSystem<ClientSimulationSystemGroup>() != null)
            {
                // Client worlds automatically connect to localhost
                NetworkEndPoint ep = NetworkEndPoint.LoopbackIpv4;
                ep.Port = 7979;
		// TODO When want to connect online, replace above two lines with below two
                //NetworkEndPoint ep = NetworkEndPoint.Parse("98.207.153.86", 7979);
#if UNITY_EDITOR
                //ep = NetworkEndPoint.Parse(ClientServerBootstrap.RequestedAutoConnect, 7979);
#endif
                network.Connect(ep);
            }
            #if UNITY_EDITOR || UNITY_SERVER
            else if (world.GetExistingSystem<ServerSimulationSystemGroup>() != null)
            {
                // Server world automatically listens for connections from any host
                NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
                ep.Port = 7979;
                network.Listen(ep);
            }
            #endif
        }
    }
}

public struct GoInGameRequest : IRpcCommand {}

// TODO will this still work with commented out?
// The system that makes the RPC request component transfer
//public class GoInGameRequestSystem : RpcCommandRequestSystem<GoInGameRequest>
//{
//}

// When client has a connection with network id, go in game and tell server to also go in game
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class GoInGameClientSystem : ComponentSystem
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Debug.LogError("in onpudate in goingamesystem");
        Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, ref NetworkIdComponent id) =>
        {
            //Debug.LogError("  Inside ForEach of goingamesystem");
            PostUpdateCommands.AddComponent<NetworkStreamInGame>(ent);
            var req = PostUpdateCommands.CreateEntity();
            PostUpdateCommands.AddComponent<GoInGameRequest>(req);
            PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = ent });
        });
    }
}

// When server receives go in game request, go in game and delete request
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class GoInGameServerSystem : ComponentSystem
{



    protected override void OnUpdate()
    {
        Entities.WithNone<SendRpcCommandRequestComponent>().ForEach((Entity reqEnt, ref GoInGameRequest req, ref ReceiveRpcCommandRequestComponent reqSrc) =>
        {
            PostUpdateCommands.AddComponent<NetworkStreamInGame>(reqSrc.SourceConnection);
            UnityEngine.Debug.Log(String.Format("Server setting connection {0} to in game", EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value));
            var ghostCollection = GetSingleton<GhostPrefabCollectionComponent>();


            //var serverPrefabs = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs);
            //for (int ghostId = 0; ghostId < serverPrefabs.Length; ++ghostId)
            //{
            //    if (EntityManager.HasComponent<MovableCubeComponent>(serverPrefabs[ghostId].Value))
            //        prefab = serverPrefabs[ghostId].Value;
            //}



	    // old implementation, was deprecated annoyingly
            //var ghostId = NetAgentGhostSerializerCollection.FindGhostType<AgentSnapshotData>();
            var ghostId = 0;
            var prefab = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
            var player = EntityManager.Instantiate(prefab);

            EntityManager.SetComponentData(player, new AgentComponent { PlayerId = EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value});

            InitializeAgent(player);
            
            PostUpdateCommands.AddBuffer<AgentInput>(player);

            PostUpdateCommands.SetComponent(reqSrc.SourceConnection, new CommandTargetComponent {targetEntity = player});

            PostUpdateCommands.DestroyEntity(reqEnt);

            Entity dash1 = DashSystem.AddAbility(player, 1, 1000, 7, new float3(0,0,1), KeyCode.A, EntityManager, ghostCollection);
            Entity dash2 = DashSystem.AddAbility(player, 1, 1000, 7, new float3(0,0,-1), KeyCode.S, EntityManager, ghostCollection);
            Entity dash3 = DashSystem.AddAbility(player, 1, 1000, 7, new float3(1,0,0), KeyCode.D, EntityManager, ghostCollection);
            Entity dash4 = DashSystem.AddAbility(player, 1, 1000, 7, new float3(-1,0,0), KeyCode.F, EntityManager, ghostCollection);
            Entity sword = SwordSystem.AddAbility(player, EntityManager, ghostCollection);
            Entity shield = ShieldSystem.AddAbility(player, EntityManager, ghostCollection);




//#if (!UNITY_EDITOR)
            //GameObject animate_prefab = Resources.Load<GameObject>("Prefabs/gladiator");
            //GameObject obj = UnityEngine.Object.Instantiate(animate_prefab);
            // setup pointer to entity from object
            //obj.GetComponent<PlayerMono>().entity = player;
            // setup pointers to entities from sword object
            //var swordObj = obj.transform.Find("CharArmature/Pelvis/Ribcage/Chest/Shoulder.R/UpperArm.R/Forearm.R/Hand.R/SwordArmature/Bone/Sword");
            //SwordMono swordMono = swordObj.GetComponent<SwordMono>();
            //swordMono.entity = sword;
            //swordMono.player = player;
            //EntityManager.AddComponentObject(sword, swordMono);

            //var shieldObj = obj.transform.Find("CharArmature/Pelvis/Ribcage/Chest/Shoulder.L/UpperArm.L/Forearm.L/ShieldArmature/Bone.001/Shield");
            //ShieldMono shieldMono = shieldObj.GetComponent<ShieldMono>();
            //shieldMono.entity = shield;
            //shieldMono.player = player;
            //EntityManager.AddComponentObject(shield, shieldMono);

            //obj.transform.localScale *= 0.5f;
            //EntityManager.AddComponentObject(player, obj);
            PostUpdateCommands.AddComponent<AnimationInitialized>(player);
//#endif

        });
    }

    protected void InitializeAgent(Entity player) {
      EntityManager.SetComponentData(player, new Speed {Value = 22f});
      EntityManager.SetComponentData(player, new BackwardModifier {Value = 0.70f});
      EntityManager.SetComponentData(player, new GameOrientation {Value = new float2(0,1)});
      EntityManager.SetComponentData(player, new BusyTimer {Value = 0});
      EntityManager.SetComponentData(player, new Health {Value = 100, regen = 2f, max = 100});

      //EntityManager.Instantiate(PrefabEntities.healthBar);


    }

}


// system to initialize any local structures to client when an agent is created
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ClientObjectsInit : ComponentSystem
{
    protected override void OnCreate() {}

    protected override void OnUpdate()
    {
        //Debug.LogError("in onpudate in goingamesystem");
        Entities.WithNone<AnimationInitialized>().ForEach((Entity ent, ref AgentComponent id) =>
        {
            GameObject animate_prefab = Resources.Load<GameObject>("Prefabs/gladiator");
            GameObject obj = UnityEngine.Object.Instantiate(animate_prefab);
            obj.transform.localScale *= 0.5f;
            EntityManager.AddComponentObject(ent, obj);
            PostUpdateCommands.AddComponent<AnimationInitialized>(ent);
        });
    }
}

