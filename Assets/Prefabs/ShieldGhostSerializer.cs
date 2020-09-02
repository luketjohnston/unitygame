using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;

public struct ShieldGhostSerializer : IGhostSerializer<ShieldSnapshotData>
{
    private ComponentType componentTypeAngleInput;
    private ComponentType componentTypeKeyCodeComp;
    private ComponentType componentTypeOwningPlayer;
    private ComponentType componentTypeReleasable;
    private ComponentType componentTypeShield;
    private ComponentType componentTypeLocalToWorld;
    private ComponentType componentTypeRotation;
    private ComponentType componentTypeTranslation;
    private ComponentType componentTypeUsable;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<AngleInput> ghostAngleInputType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<OwningPlayer> ghostOwningPlayerType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Releasable> ghostReleasableType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Rotation> ghostRotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Translation> ghostTranslationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Usable> ghostUsableType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<ShieldSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeAngleInput = ComponentType.ReadWrite<AngleInput>();
        componentTypeKeyCodeComp = ComponentType.ReadWrite<KeyCodeComp>();
        componentTypeOwningPlayer = ComponentType.ReadWrite<OwningPlayer>();
        componentTypeReleasable = ComponentType.ReadWrite<Releasable>();
        componentTypeShield = ComponentType.ReadWrite<Shield>();
        componentTypeLocalToWorld = ComponentType.ReadWrite<LocalToWorld>();
        componentTypeRotation = ComponentType.ReadWrite<Rotation>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        componentTypeUsable = ComponentType.ReadWrite<Usable>();
        ghostAngleInputType = system.GetArchetypeChunkComponentType<AngleInput>(true);
        ghostOwningPlayerType = system.GetArchetypeChunkComponentType<OwningPlayer>(true);
        ghostReleasableType = system.GetArchetypeChunkComponentType<Releasable>(true);
        ghostRotationType = system.GetArchetypeChunkComponentType<Rotation>(true);
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>(true);
        ghostUsableType = system.GetArchetypeChunkComponentType<Usable>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref ShieldSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataAngleInput = chunk.GetNativeArray(ghostAngleInputType);
        var chunkDataOwningPlayer = chunk.GetNativeArray(ghostOwningPlayerType);
        var chunkDataReleasable = chunk.GetNativeArray(ghostReleasableType);
        var chunkDataRotation = chunk.GetNativeArray(ghostRotationType);
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        var chunkDataUsable = chunk.GetNativeArray(ghostUsableType);
        snapshot.SetAngleInputValue(chunkDataAngleInput[ent].Value, serializerState);
        snapshot.SetOwningPlayerValue(chunkDataOwningPlayer[ent].Value, serializerState);
        snapshot.SetOwningPlayerPlayerId(chunkDataOwningPlayer[ent].PlayerId, serializerState);
        snapshot.SetReleasablereleased(chunkDataReleasable[ent].released, serializerState);
        snapshot.SetRotationValue(chunkDataRotation[ent].Value, serializerState);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value, serializerState);
        snapshot.SetUsableinuse(chunkDataUsable[ent].inuse, serializerState);
        snapshot.SetUsablecanuse(chunkDataUsable[ent].canuse, serializerState);
    }
}
