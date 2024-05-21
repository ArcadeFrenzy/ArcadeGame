using System;
using System.IO;
using UnityEngine;

public abstract class Command
{
    private readonly ushort id;

    protected Command(ushort id)
    {
        this.id = id;
    }

    protected abstract void Encode(BinaryWriter writer);

    protected abstract void Decode(BinaryReader reader);

    public abstract void Process();

    public void EncodeCommand(BinaryWriter writer)
    {
        writer.Write(this.id);

        using (MemoryStream stream = new MemoryStream())
        {
            BinaryWriter writer2 = new BinaryWriter(stream);

            this.Encode(writer2);

            writer.Write((int)stream.Length);
            writer.Write(stream.ToArray());
        }
    }

    public static Command DecodeCommand(BinaryReader reader)
    {
        ushort id = reader.ReadUInt16();
        Command command = Commands.GetCommand(id);

        int length = reader.ReadInt32();
        byte[] data = reader.ReadBytes(length);

        if (command == null)
        {
            Debug.LogError($"Unknown command {id}.");
            return null;
        }

        using(MemoryStream stream = new MemoryStream(data))
        {
            using(BinaryReader reader2 = new BinaryReader(stream))
            {
                command.Decode(reader2);
            }
        }

        Debug.Log($"Received server command {command.GetType().Name}.");
        return command;
    }
}