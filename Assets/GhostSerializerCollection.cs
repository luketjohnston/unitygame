using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct NetAgentGhostSerializerCollection : IGhostSerializerCollection
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
    public static int FindGhostType<T>()
        where T : struct, ISnapshotData<T>
    {
        if (typeof(T) == typeof(AgentSnapshotData))
            return 0;
        if (typeof(T) == typeof(DashSnapshotData))
            return 1;
        if (typeof(T) == typeof(SwordSnapshotData))
            return 2;
        if (typeof(T) == typeof(ShieldSnapshotData))
            return 3;
        return -1;
    }

    public void BeginSerialize(ComponentSystemBase system)
    {
        m_AgentGhostSerializer.BeginSerialize(system);
        m_DashGhostSerializer.BeginSerialize(system);
        m_SwordGhostSerializer.BeginSerialize(system);
        m_ShieldGhostSerializer.BeginSerialize(system);
    }

    public int CalculateImportance(int serializer, ArchetypeChunk chunk)
    {
        switch (serializer)
        {
            case 0:
                return m_AgentGhostSerializer.CalculateImportance(chunk);
            case 1:
                return m_DashGhostSerializer.CalculateImportance(chunk);
            case 2:
                return m_SwordGhostSerializer.CalculateImportance(chunk);
            case 3:
                return m_ShieldGhostSerializer.CalculateImportance(chunk);
        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int GetSnapshotSize(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_AgentGhostSerializer.SnapshotSize;
            case 1:
                return m_DashGhostSerializer.SnapshotSize;
            case 2:
                return m_SwordGhostSerializer.SnapshotSize;
            case 3:
                return m_ShieldGhostSerializer.SnapshotSize;
        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int Serialize(ref DataStreamWriter dataStream, SerializeData data)
    {
        switch (data.ghostType)
        {
            case 0:
            {
                return GhostSendSystem<NetAgentGhostSerializerCollection>.InvokeSerialize<AgentGhostSerializer, AgentSnapshotData>(m_AgentGhostSerializer, ref dataStream, data);
            }
            case 1:
            {
                return GhostSendSystem<NetAgentGhostSerializerCollection>.InvokeSerialize<DashGhostSerializer, DashSnapshotData>(m_DashGhostSerializer, ref dataStream, data);
            }
            case 2:
            {
                return GhostSendSystem<NetAgentGhostSerializerCollection>.InvokeSerialize<SwordGhostSerializer, SwordSnapshotData>(m_SwordGhostSerializer, ref dataStream, data);
            }
            case 3:
            {
                return GhostSendSystem<NetAgentGhostSerializerCollection>.InvokeSerialize<ShieldGhostSerializer, ShieldSnapshotData>(m_ShieldGhostSerializer, ref dataStream, data);
            }
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    private AgentGhostSerializer m_AgentGhostSerializer;
    private DashGhostSerializer m_DashGhostSerializer;
    private SwordGhostSerializer m_SwordGhostSerializer;
    private ShieldGhostSerializer m_ShieldGhostSerializer;
}

public struct EnableNetAgentGhostSendSystemComponent : IComponentData
{}
public class NetAgentGhostSendSystem : GhostSendSystem<NetAgentGhostSerializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableNetAgentGhostSendSystemComponent>();
    }

    public override bool IsEnabled()
    {
        return HasSingleton<EnableNetAgentGhostSendSystemComponent>();
    }
}
