using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public sealed class Commands
{
    // Client / Server
    public const ushort CLIENT_HELLO = 0;
    public const ushort SERVER_HELLO = 1;
    public const ushort KEEP_ALIVE = 2;
    public const ushort KEEP_ALIVE_OK = 3;

    // Lobby / Game
    public const ushort PLAYER_JOIN = 4;
    public const ushort PLAYER_LEAVE = 5;
    public const ushort PLAYER_LOBBY_MOVE = 6;
    public const ushort PLAYER_LIST = 7;

    public const ushort LOBBY_QUEUE = 8;
    public const ushort GAME_START = 9;

    // Pong
    public const ushort PONG_MOVE = 10;

    private static Dictionary<ushort, Command> commandMap = new Dictionary<ushort, Command>();

    public static void RegisterAllCommands()
    {
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
        {
            assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Command))).ToList().ForEach(type =>
            {
                ConstructorInfo constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], new ParameterModifier[0]);

                if (constructor == null)
                {
                    return;
                }

                Command command = (Command)constructor.Invoke(new object[0]);

                FieldInfo idField = typeof(Command).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);

                if (idField == null)
                {
                    return;
                }

                ushort id = (ushort)idField.GetValue(command);
                commandMap.Add(id, command);
            });
        });
    }

    public static Command GetCommand(ushort id)
    {
        if(!commandMap.ContainsKey(id))
        {
            return null;
        }

        return (Command)Activator.CreateInstance(commandMap[id].GetType(), true);
    }
}