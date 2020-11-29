using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace BeaterScript
{
    public class CommandsListHandler
    {
        public Dictionary<ushort, Command> commands = new Dictionary<ushort, Command>();
        public Dictionary<string, ushort> command_map = new Dictionary<string, ushort>();

        public CommandsListHandler(string game)
        {
            // Parse Commands from YAML, and store them.
            using var s = File.OpenText($"{game}.yml");
            var deserializer = new Deserializer();
            var commands_yaml = deserializer.Deserialize<Dictionary<int, YamlMappingNode>>(s);

            foreach (var (key, node) in commands_yaml)
            {
                var cmd = ReadCommandDetail(node, key);
                commands.Add((ushort)key, cmd);
                command_map.Add(cmd.Name, (ushort)key);
            }
        }

        private static Command ReadCommandDetail(YamlMappingNode node, int key)
        {
            var name = node["Name"].ToString();
            var types = ReadCommandParameters(node);

            bool hasFunction = false;
            try { hasFunction = node["HasFunction"].ToString() == "true"; }
            catch (KeyNotFoundException) { }

            bool hasMovement = false;
            try { hasMovement = node["HasMovement"].ToString() == "true"; }
            catch (KeyNotFoundException) { }

            return new Command(name, (ushort)key, hasFunction, hasMovement, types);
        }

        private static List<Type> ReadCommandParameters(YamlMappingNode node)
        {
            List<Type> types = new List<Type>();
            try
            {
                var parameters = (YamlSequenceNode) node["Parameters"];
                foreach (var p in parameters.Children)
                {
                    switch (p.ToString())
                    {
                        case "int":
                            types.Add(typeof(int));
                            break;
                        case "ushort":
                            types.Add(typeof(ushort));
                            break;
                        case "byte":
                            types.Add(typeof(byte));
                            break;
                    }
                }
            }
            catch (KeyNotFoundException) { } // Ignore, as the command does not have parameters.

            return types;
        }
    }
}
