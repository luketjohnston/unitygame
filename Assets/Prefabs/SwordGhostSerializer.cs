using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using Unity.Rendering;

public struct SwordGhostSerializer : IGhostSerializer<SwordSnapshotData>
{
    private ComponentType componentTypeCooldown;
    private ComponentType componentTypeKeyCodeComp;
    private ComponentType componentTypeOwningPlayer;
    private ComponentType componentTypeSword;
    private ComponentType componentTypePhysicsCollider;
    private ComponentType componentTypePhysicsGravityFactor;
    private ComponentType componentTypePhysicsMass;
    private ComponentType componentTypePhysicsVelocity;
    private ComponentType componentTypeCompositeScale;
    private ComponentType componentTypeLocalToWorld;
    private ComponentType componentTypeRotation;
    private ComponentType componentTypeTranslation;
    private ComponentType componentTypeUsable;
    private ComponentType componentTypeLinkedEntityGroup;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Cooldown> ghostCooldownType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<OwningPlayer> ghostOwningPlayerType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Rotation> ghostRotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Translation> ghostTranslationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Usable> ghostUsableType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkBufferType<LinkedEntityGroup> ghostLinkedEntityGroupType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ComponentDataFromEntity<Rotation> ghostChild0RotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ComponentDataFromEntity<Translation> ghostChild0TranslationType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<SwordSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeCooldown = ComponentType.ReadWrite<Cooldown>();
        componentTypeKeyCodeComp = ComponentType.ReadWrite<KeyCodeComp>();
        componentTypeOwningPlayer = ComponentType.ReadWrite<OwningPlayer>();
        componentTypeSword = ComponentType.ReadWrite<Sword>();
        componentTypePhysicsCollider = ComponentType.ReadWrite<PhysicsCollider>();
        componentTypePhysicsGravityFactor = ComponentType.ReadWrite<PhysicsGravityFactor>();
        componentTypePhysicsMass = ComponentType.ReadWrite<PhysicsMass>();
        componentTypePhysicsVelocity = ComponentType.ReadWrite<PhysicsVelocity>();
        componentTypeCompositeScale = ComponentType.ReadWrite<CompositeScale>();
        componentTypeLocalToWorld = ComponentType.ReadWrite<LocalToWorld>();
        componentTypeRotation = ComponentType.ReadWrite<Rotation>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        componentTypeUsable = ComponentType.ReadWrite<Usable>();
        componentTypeLinkedEntityGroup = ComponentType.ReadWrite<LinkedEntityGroup>();
        ghostCooldownType = system.GetArchetypeChunkComponentType<Cooldown>(true);
        ghostOwningPlayerType = system.GetArchetypeChunkComponentType<OwningPlayer>(true);
        ghostRotationType = system.GetArchetypeChunkComponentType<Rotation>(true);
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>(true);
        ghostUsableType = system.GetArchetypeChunkComponentType<Usable>(true);
        ghostLinkedEntityGroupType = system.GetArchetypeChunkBufferType<LinkedEntityGroup>(true);
        ghostChild0RotationType = system.GetComponentDataFromEntity<Rotation>(true);
        ghostChild0TranslationType = system.GetComponentDataFromEntity<Translation>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref SwordSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataCooldown = chunk.GetNativeArray(ghostCooldownType);
        var chunkDataOwningPlayer = chunk.GetNativeArray(ghostOwningPlayerType);
        var chunkDataRotation = chunk.GetNativeArray(ghostRotationType);
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        var chunkDataUsable = chunk.GetNativeArray(ghostUsableType);
        var chunkDataLinkedEntityGroup = chunk.GetBufferAccessor(ghostLinkedEntityGroupType);
        snapshot.SetCooldowntimer(chunkDataCooldown[ent].timer, serializerState);
        snapshot.SetCooldownduration(chunkDataCooldown[ent].duration, serializerState);
        snapshot.SetOwningPlayerValue(chunkDataOwningPlayer[ent].Value, serializerState);
        snapshot.SetOwningPlayerPlayerId(chunkDataOwningPlayer[ent].PlayerId, serializerState);
        snapshot.SetRotationValue(chunkDataRotation[ent].Value, serializerState);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value, serializerState);
        snapshot.SetUsablecanuse(chunkDataUsable[ent].canuse, serializerState);
        snapshot.SetChild0RotationValue(ghostChild0RotationType[chunkDataLinkedEntityGroup[ent][1].Value].Value, serializerState);
        snapshot.SetChild0TranslationValue(ghostChild0TranslationType[chunkDataLinkedEntityGroup[ent][1].Value].Value, serializerState);
    }
}
