using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct NetAgentGhostDeserializerCollection : IGhostDeserializerCollection
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "AgentGhostSerializer",
            "DashGhostSerializer",
            "SwordGhostSerializer",
            "ShieldGhostSerializer",
        };
        return arr;
    }

    public int Length => 4;
#endif
    public void Initialize(World world)
    {
        var curAgentGhostSpawnSystem = world.GetOrCreateSystem<AgentGhostSpawnSystem>();
        m_AgentSnapshotDataNewGhostIds = curAgentGhostSpawnSystem.NewGhostIds;
        m_AgentSnapshotDataNewGhosts = curAgentGhostSpawnSystem.NewGhosts;
        curAgentGhostSpawnSystem.GhostType = 0;
        var curDashGhostSpawnSystem = world.GetOrCreateSystem<DashGhostSpawnSystem>();
        m_DashSnapshotDataNewGhostIds = curDashGhostSpawnSystem.NewGhostIds;
        m_DashSnapshotDataNewGhosts = curDashGhostSpawnSystem.NewGhosts;
        curDashGhostSpawnSystem.GhostType = 1;
        var curSwordGhostSpawnSystem = world.GetOrCreateSystem<SwordGhostSpawnSystem>();
        m_SwordSnapshotDataNewGhostIds = curSwordGhostSpawnSystem.NewGhostIds;
        m_SwordSnapshotDataNewGhosts = curSwordGhostSpawnSystem.NewGhosts;
        curSwordGhostSpawnSystem.GhostType = 2;
        var curShieldGhostSpawnSystem = world.GetOrCreateSystem<ShieldGhostSpawnSystem>();
        m_ShieldSnapshotDataNewGhostIds = curShieldGhostSpawnSystem.NewGhostIds;
        m_ShieldSnapshotDataNewGhosts = curShieldGhostSpawnSystem.NewGhosts;
        curShieldGhostSpawnSystem.GhostType = 3;
    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_AgentSnapshotDataFromEntity = system.GetBufferFromEntity<AgentSnapshotData>();
        m_DashSnapshotDataFromEntity = system.GetBufferFromEntity<DashSnapshotData>();
        m_SwordSnapshotDataFromEntity = system.GetBufferFromEntity<SwordSnapshotData>();
        m_ShieldSnapshotDataFromEntity = system.GetBufferFromEntity<ShieldSnapshotData>();
    }
    public bool Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        ref DataStreamReader reader, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                return GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeDeserialize(m_AgentSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 1:
                return GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeDeserialize(m_DashSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 2:
                return GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeDeserialize(m_SwordSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 3:
                return GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeDeserialize(m_ShieldSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    public void Spawn(int serializer, int ghostId, uint snapshot, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                m_AgentSnapshotDataNewGhostIds.Add(ghostId);
                m_AgentSnapshotDataNewGhosts.Add(GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeSpawn<AgentSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 1:
                m_DashSnapshotDataNewGhostIds.Add(ghostId);
                m_DashSnapshotDataNewGhosts.Add(GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeSpawn<DashSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 2:
                m_SwordSnapshotDataNewGhostIds.Add(ghostId);
                m_SwordSnapshotDataNewGhosts.Add(GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeSpawn<SwordSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 3:
                m_ShieldSnapshotDataNewGhostIds.Add(ghostId);
                m_ShieldSnapshotDataNewGhosts.Add(GhostReceiveSystem<NetAgentGhostDeserializerCollection>.InvokeSpawn<ShieldSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<AgentSnapshotData> m_AgentSnapshotDataFromEntity;
    private NativeList<int> m_AgentSnapshotDataNewGhostIds;
    private NativeList<AgentSnapshotData> m_AgentSnapshotDataNewGhosts;
    private BufferFromEntity<DashSnapshotData> m_DashSnapshotDataFromEntity;
    private NativeList<int> m_DashSnapshotDataNewGhostIds;
    private NativeList<DashSnapshotData> m_DashSnapshotDataNewGhosts;
    private BufferFromEntity<SwordSnapshotData> m_SwordSnapshotDataFromEntity;
    private NativeList<int> m_SwordSnapshotDataNewGhostIds;
    private NativeList<SwordSnapshotData> m_SwordSnapshotDataNewGhosts;
    private BufferFromEntity<ShieldSnapshotData> m_ShieldSnapshotDataFromEntity;
    private NativeList<int> m_ShieldSnapshotDataNewGhostIds;
    private NativeList<ShieldSnapshotData> m_ShieldSnapshotDataNewGhosts;
}
public struct EnableNetAgentGhostReceiveSystemComponent : IComponentData
{}
public class NetAgentGhostReceiveSystem : GhostReceiveSystem<NetAgentGhostDeserializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableNetAgentGhostReceiveSystemComponent>();
    }
}
