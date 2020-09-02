using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;

public struct DashGhostSerializer : IGhostSerializer<DashSnapshotData>
{
    private ComponentType componentTypeCooldown;
    private ComponentType componentTypeDash;
    private ComponentType componentTypeKeyCodeComp;
    private ComponentType componentTypeOwningPlayer;
    private ComponentType componentTypeLocalToWorld;
    private ComponentType componentTypeRotation;
    private ComponentType componentTypeTranslation;
    private ComponentType componentTypeUsable;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Cooldown> ghostCooldownType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Dash> ghostDashType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<OwningPlayer> ghostOwningPlayerType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Rotation> ghostRotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Translation> ghostTranslationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Usable> ghostUsableType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 2;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<DashSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeCooldown = ComponentType.ReadWrite<Cooldown>();
        componentTypeDash = ComponentType.ReadWrite<Dash>();
        componentTypeKeyCodeComp = ComponentType.ReadWrite<KeyCodeComp>();
        componentTypeOwningPlayer = ComponentType.ReadWrite<OwningPlayer>();
        componentTypeLocalToWorld = ComponentType.ReadWrite<LocalToWorld>();
        componentTypeRotation = ComponentType.ReadWrite<Rotation>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        componentTypeUsable = ComponentType.ReadWrite<Usable>();
        ghostCooldownType = system.GetArchetypeChunkComponentType<Cooldown>(true);
        ghostDashType = system.GetArchetypeChunkComponentType<Dash>(true);
        ghostOwningPlayerType = system.GetArchetypeChunkComponentType<OwningPlayer>(true);
        ghostRotationType = system.GetArchetypeChunkComponentType<Rotation>(true);
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>(true);
        ghostUsableType = system.GetArchetypeChunkComponentType<Usable>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref DashSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataCooldown = chunk.GetNativeArray(ghostCooldownType);
        var chunkDataDash = chunk.GetNativeArray(ghostDashType);
        var chunkDataOwningPlayer = chunk.GetNativeArray(ghostOwningPlayerType);
        var chunkDataRotation = chunk.GetNativeArray(ghostRotationType);
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        var chunkDataUsable = chunk.GetNativeArray(ghostUsableType);
        snapshot.SetCooldowntimer(chunkDataCooldown[ent].timer, serializerState);
        snapshot.SetCooldownduration(chunkDataCooldown[ent].duration, serializerState);
        snapshot.SetDashdistance_traveled(chunkDataDash[ent].distance_traveled, serializerState);
        snapshot.SetDashmax_distance(chunkDataDash[ent].max_distance, serializerState);
        snapshot.SetDashspeed(chunkDataDash[ent].speed, serializerState);
        snapshot.SetDashdir(chunkDataDash[ent].dir, serializerState);
        snapshot.SetOwningPlayerValue(chunkDataOwningPlayer[ent].Value, serializerState);
        snapshot.SetOwningPlayerPlayerId(chunkDataOwningPlayer[ent].PlayerId, serializerState);
        snapshot.SetRotationValue(chunkDataRotation[ent].Value, serializerState);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value, serializerState);
        snapshot.SetUsableinuse(chunkDataUsable[ent].inuse, serializerState);
        snapshot.SetUsablecanuse(chunkDataUsable[ent].canuse, serializerState);
    }
}
