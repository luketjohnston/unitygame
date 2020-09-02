using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Entities;

public struct ShieldSnapshotData : ISnapshotData<ShieldSnapshotData>
{
    public uint tick;
    private int AngleInputValue;
    private int OwningPlayerValue;
    private int OwningPlayerPlayerId;
    private uint Releasablereleased;
    private int RotationValueX;
    private int RotationValueY;
    private int RotationValueZ;
    private int RotationValueW;
    private int TranslationValueX;
    private int TranslationValueY;
    private int TranslationValueZ;
    private uint Usableinuse;
    private uint Usablecanuse;
    uint changeMask0;

    public uint Tick => tick;
    public float GetAngleInputValue(GhostDeserializerState deserializerState)
    {
        return AngleInputValue * 0.001f;
    }
    public float GetAngleInputValue()
    {
        return AngleInputValue * 0.001f;
    }
    public void SetAngleInputValue(float val, GhostSerializerState serializerState)
    {
        AngleInputValue = (int)(val * 1000);
    }
    public void SetAngleInputValue(float val)
    {
        AngleInputValue = (int)(val * 1000);
    }
    public Entity GetOwningPlayerValue(GhostDeserializerState deserializerState)
    {
        if (OwningPlayerValue == 0)
            return Entity.Null;
        if (!deserializerState.GhostMap.TryGetValue(OwningPlayerValue, out var ghostEnt))
            return Entity.Null;
        if (Unity.Networking.Transport.Utilities.SequenceHelpers.IsNewer(ghostEnt.spawnTick, Tick))
            return Entity.Null;
        return ghostEnt.entity;
    }
    public void SetOwningPlayerValue(Entity val, GhostSerializerState serializerState)
    {
        OwningPlayerValue = 0;
        if (serializerState.GhostStateFromEntity.Exists(val))
        {
            var ghostState = serializerState.GhostStateFromEntity[val];
            if (ghostState.despawnTick == 0)
                OwningPlayerValue = ghostState.ghostId;
        }
    }
    public void SetOwningPlayerValue(int val)
    {
        OwningPlayerValue = val;
    }
    public int GetOwningPlayerPlayerId(GhostDeserializerState deserializerState)
    {
        return (int)OwningPlayerPlayerId;
    }
    public int GetOwningPlayerPlayerId()
    {
        return (int)OwningPlayerPlayerId;
    }
    public void SetOwningPlayerPlayerId(int val, GhostSerializerState serializerState)
    {
        OwningPlayerPlayerId = (int)val;
    }
    public void SetOwningPlayerPlayerId(int val)
    {
        OwningPlayerPlayerId = (int)val;
    }
    public bool GetReleasablereleased(GhostDeserializerState deserializerState)
    {
        return Releasablereleased!=0;
    }
    public bool GetReleasablereleased()
    {
        return Releasablereleased!=0;
    }
    public void SetReleasablereleased(bool val, GhostSerializerState serializerState)
    {
        Releasablereleased = val?1u:0;
    }
    public void SetReleasablereleased(bool val)
    {
        Releasablereleased = val?1u:0;
    }
    public quaternion GetRotationValue(GhostDeserializerState deserializerState)
    {
        return GetRotationValue();
    }
    public quaternion GetRotationValue()
    {
        return new quaternion(RotationValueX * 0.001f, RotationValueY * 0.001f, RotationValueZ * 0.001f, RotationValueW * 0.001f);
    }
    public void SetRotationValue(quaternion q, GhostSerializerState serializerState)
    {
        SetRotationValue(q);
    }
    public void SetRotationValue(quaternion q)
    {
        RotationValueX = (int)(q.value.x * 1000);
        RotationValueY = (int)(q.value.y * 1000);
        RotationValueZ = (int)(q.value.z * 1000);
        RotationValueW = (int)(q.value.w * 1000);
    }
    public float3 GetTranslationValue(GhostDeserializerState deserializerState)
    {
        return GetTranslationValue();
    }
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX * 0.01f, TranslationValueY * 0.01f, TranslationValueZ * 0.01f);
    }
    public void SetTranslationValue(float3 val, GhostSerializerState serializerState)
    {
        SetTranslationValue(val);
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 100);
        TranslationValueY = (int)(val.y * 100);
        TranslationValueZ = (int)(val.z * 100);
    }
    public bool GetUsableinuse(GhostDeserializerState deserializerState)
    {
        return Usableinuse!=0;
    }
    public bool GetUsableinuse()
    {
        return Usableinuse!=0;
    }
    public void SetUsableinuse(bool val, GhostSerializerState serializerState)
    {
        Usableinuse = val?1u:0;
    }
    public void SetUsableinuse(bool val)
    {
        Usableinuse = val?1u:0;
    }
    public bool GetUsablecanuse(GhostDeserializerState deserializerState)
    {
        return Usablecanuse!=0;
    }
    public bool GetUsablecanuse()
    {
        return Usablecanuse!=0;
    }
    public void SetUsablecanuse(bool val, GhostSerializerState serializerState)
    {
        Usablecanuse = val?1u:0;
    }
    public void SetUsablecanuse(bool val)
    {
        Usablecanuse = val?1u:0;
    }

    public void PredictDelta(uint tick, ref ShieldSnapshotData baseline1, ref ShieldSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        AngleInputValue = predictor.PredictInt(AngleInputValue, baseline1.AngleInputValue, baseline2.AngleInputValue);
        OwningPlayerValue = predictor.PredictInt(OwningPlayerValue, baseline1.OwningPlayerValue, baseline2.OwningPlayerValue);
        OwningPlayerPlayerId = predictor.PredictInt(OwningPlayerPlayerId, baseline1.OwningPlayerPlayerId, baseline2.OwningPlayerPlayerId);
        Releasablereleased = (uint)predictor.PredictInt((int)Releasablereleased, (int)baseline1.Releasablereleased, (int)baseline2.Releasablereleased);
        RotationValueX = predictor.PredictInt(RotationValueX, baseline1.RotationValueX, baseline2.RotationValueX);
        RotationValueY = predictor.PredictInt(RotationValueY, baseline1.RotationValueY, baseline2.RotationValueY);
        RotationValueZ = predictor.PredictInt(RotationValueZ, baseline1.RotationValueZ, baseline2.RotationValueZ);
        RotationValueW = predictor.PredictInt(RotationValueW, baseline1.RotationValueW, baseline2.RotationValueW);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);
        Usableinuse = (uint)predictor.PredictInt((int)Usableinuse, (int)baseline1.Usableinuse, (int)baseline2.Usableinuse);
        Usablecanuse = (uint)predictor.PredictInt((int)Usablecanuse, (int)baseline1.Usablecanuse, (int)baseline2.Usablecanuse);
    }

    public void Serialize(int networkId, ref ShieldSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (AngleInputValue != baseline.AngleInputValue) ? 1u : 0;
        changeMask0 |= (OwningPlayerValue != baseline.OwningPlayerValue) ? (1u<<1) : 0;
        changeMask0 |= (OwningPlayerPlayerId != baseline.OwningPlayerPlayerId) ? (1u<<2) : 0;
        changeMask0 |= (Releasablereleased != baseline.Releasablereleased) ? (1u<<3) : 0;
        changeMask0 |= (RotationValueX != baseline.RotationValueX ||
                                           RotationValueY != baseline.RotationValueY ||
                                           RotationValueZ != baseline.RotationValueZ ||
                                           RotationValueW != baseline.RotationValueW) ? (1u<<4) : 0;
        changeMask0 |= (TranslationValueX != baseline.TranslationValueX ||
                                           TranslationValueY != baseline.TranslationValueY ||
                                           TranslationValueZ != baseline.TranslationValueZ) ? (1u<<5) : 0;
        changeMask0 |= (Usableinuse != baseline.Usableinuse) ? (1u<<6) : 0;
        changeMask0 |= (Usablecanuse != baseline.Usablecanuse) ? (1u<<7) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(AngleInputValue, baseline.AngleInputValue, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedIntDelta(OwningPlayerValue, baseline.OwningPlayerValue, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedIntDelta(OwningPlayerPlayerId, baseline.OwningPlayerPlayerId, compressionModel);
        if ((changeMask0 & (1 << 3)) != 0)
            writer.WritePackedUIntDelta(Releasablereleased, baseline.Releasablereleased, compressionModel);
        if ((changeMask0 & (1 << 4)) != 0)
        {
            writer.WritePackedIntDelta(RotationValueX, baseline.RotationValueX, compressionModel);
            writer.WritePackedIntDelta(RotationValueY, baseline.RotationValueY, compressionModel);
            writer.WritePackedIntDelta(RotationValueZ, baseline.RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(RotationValueW, baseline.RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 5)) != 0)
        {
            writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);
        }
        if ((changeMask0 & (1 << 6)) != 0)
            writer.WritePackedUIntDelta(Usableinuse, baseline.Usableinuse, compressionModel);
        if ((changeMask0 & (1 << 7)) != 0)
            writer.WritePackedUIntDelta(Usablecanuse, baseline.Usablecanuse, compressionModel);
    }

    public void Deserialize(uint tick, ref ShieldSnapshotData baseline, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        changeMask0 = reader.ReadPackedUIntDelta(baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            AngleInputValue = reader.ReadPackedIntDelta(baseline.AngleInputValue, compressionModel);
        else
            AngleInputValue = baseline.AngleInputValue;
        if ((changeMask0 & (1 << 1)) != 0)
            OwningPlayerValue = reader.ReadPackedIntDelta(baseline.OwningPlayerValue, compressionModel);
        else
            OwningPlayerValue = baseline.OwningPlayerValue;
        if ((changeMask0 & (1 << 2)) != 0)
            OwningPlayerPlayerId = reader.ReadPackedIntDelta(baseline.OwningPlayerPlayerId, compressionModel);
        else
            OwningPlayerPlayerId = baseline.OwningPlayerPlayerId;
        if ((changeMask0 & (1 << 3)) != 0)
            Releasablereleased = reader.ReadPackedUIntDelta(baseline.Releasablereleased, compressionModel);
        else
            Releasablereleased = baseline.Releasablereleased;
        if ((changeMask0 & (1 << 4)) != 0)
        {
            RotationValueX = reader.ReadPackedIntDelta(baseline.RotationValueX, compressionModel);
            RotationValueY = reader.ReadPackedIntDelta(baseline.RotationValueY, compressionModel);
            RotationValueZ = reader.ReadPackedIntDelta(baseline.RotationValueZ, compressionModel);
            RotationValueW = reader.ReadPackedIntDelta(baseline.RotationValueW, compressionModel);
        }
        else
        {
            RotationValueX = baseline.RotationValueX;
            RotationValueY = baseline.RotationValueY;
            RotationValueZ = baseline.RotationValueZ;
            RotationValueW = baseline.RotationValueW;
        }
        if ((changeMask0 & (1 << 5)) != 0)
        {
            TranslationValueX = reader.ReadPackedIntDelta(baseline.TranslationValueX, compressionModel);
            TranslationValueY = reader.ReadPackedIntDelta(baseline.TranslationValueY, compressionModel);
            TranslationValueZ = reader.ReadPackedIntDelta(baseline.TranslationValueZ, compressionModel);
        }
        else
        {
            TranslationValueX = baseline.TranslationValueX;
            TranslationValueY = baseline.TranslationValueY;
            TranslationValueZ = baseline.TranslationValueZ;
        }
        if ((changeMask0 & (1 << 6)) != 0)
            Usableinuse = reader.ReadPackedUIntDelta(baseline.Usableinuse, compressionModel);
        else
            Usableinuse = baseline.Usableinuse;
        if ((changeMask0 & (1 << 7)) != 0)
            Usablecanuse = reader.ReadPackedUIntDelta(baseline.Usablecanuse, compressionModel);
        else
            Usablecanuse = baseline.Usablecanuse;
    }
    public void Interpolate(ref ShieldSnapshotData target, float factor)
    {
        SetRotationValue(math.slerp(GetRotationValue(), target.GetRotationValue(), factor));
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));
    }
}
