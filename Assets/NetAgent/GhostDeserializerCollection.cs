using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct plzworkGhostDeserializerCollection : IGhostDeserializerCollection
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "AgentGhostSerializer",
            "DashGhostSerializer",
            "SwordGhostSerializer",
        };
        return arr;
    }

    public int Length => 3;
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
    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_AgentSnapshotDataFromEntity = system.GetBufferFromEntity<AgentSnapshotData>();
        m_DashSnapshotDataFromEntity = system.GetBufferFromEntity<DashSnapshotData>();
        m_SwordSnapshotDataFromEntity = system.GetBufferFromEntity<SwordSnapshotData>();
    }
    public bool Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        ref DataStreamReader reader, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                return GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeDeserialize(m_AgentSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 1:
                return GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeDeserialize(m_DashSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 2:
                return GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeDeserialize(m_SwordSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
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
                m_AgentSnapshotDataNewGhosts.Add(GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeSpawn<AgentSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 1:
                m_DashSnapshotDataNewGhostIds.Add(ghostId);
                m_DashSnapshotDataNewGhosts.Add(GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeSpawn<DashSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 2:
                m_SwordSnapshotDataNewGhostIds.Add(ghostId);
                m_SwordSnapshotDataNewGhosts.Add(GhostReceiveSystem<plzworkGhostDeserializerCollection>.InvokeSpawn<SwordSnapshotData>(snapshot, ref reader, compressionModel));
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
}
public struct EnableplzworkGhostReceiveSystemComponent : IComponentData
{}
public class plzworkGhostReceiveSystem : GhostReceiveSystem<plzworkGhostDeserializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableplzworkGhostReceiveSystemComponent>();
    }
}
