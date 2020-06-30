using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace BeaterScriptEngine
{
    public class CommandsListHandler
    {
        public Dictionary<ushort, Command> commands;
        public Dictionary<string, ushort> command_map;

        public CommandsListHandler(string game)
        {
            // Parse Commands from YAML, and store them.
            commands = new Dictionary<ushort, Command>();
            command_map = new Dictionary<string, ushort>();
            using (var s = File.OpenText($"{game}.yml"))
            {
                var deserializer = new Deserializer();
                var commands_yaml = deserializer.Deserialize<Dictionary<int, YamlMappingNode>>(s);
                foreach (var entry in commands_yaml)
                {
                    bool hasFunction = false;
                    bool hasMovement = false;
                    List<Type> types = new List<Type>();
                    command_map.Add(commands_yaml[entry.Key]["Name"].ToString(), (ushort)entry.Key);
                    try
                    {
                        var parameters = (YamlSequenceNode)commands_yaml[entry.Key]["Parameters"];
                        foreach (var p in parameters.Children)
                            switch(p.ToString())
                            {
                                case "uint":
                                    types.Add(typeof(uint));
                                    break;
                                case "ushort":
                                    types.Add(typeof(ushort));
                                    break;
                                case "byte":
                                    types.Add(typeof(byte));
                                    break;
                                default:
                                    break;
                            }
                        if ((string)(YamlScalarNode)commands_yaml[entry.Key]["HasFunction"] == "true")
                            hasFunction = true;
                        else if ((string)(YamlScalarNode)commands_yaml[entry.Key]["HasMovement"] == "true")
                            hasMovement = true;

                    }
                    catch (KeyNotFoundException) {} // Ignore, as the command does not have parameters.
                    commands.Add((ushort)entry.Key, new Command((string)commands_yaml[entry.Key]["Name"], hasFunction, hasMovement, types.ToArray()));
                }
            }
        }
    }
}
