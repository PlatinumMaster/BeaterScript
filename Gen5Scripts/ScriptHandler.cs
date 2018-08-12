using System;
using System.IO;
using System.Collections.Generic;


namespace Gen5Scripts
{
    public class ScriptHandler
    {
        public List<uint> pointers = new List<uint>();
        public List<string> ScriptData = new List<string>();

        CommandHandler cmdhndlr = new CommandHandler();
        BinaryReader b;

        public BinaryReader LoadScript(string script)
        {
            return b = new BinaryReader(File.Open(script, FileMode.Open));
        }

        public uint GetParam(string type)
        {
            uint param = 0;
            switch (type)
            {
                case "UInt32":
                    param = b.ReadUInt32();
                    break;
                case "UInt16":
                    param = b.ReadUInt16();
                    break;
                case "UInt8":
                    param = (uint) b.ReadSByte();
                    break;
                default:
                    break;
            }
            return param;
        }

        public void FetchPointers(string script)
        {
            b.BaseStream.Position = 0;
            while (true)
            {
                pointers.Add(b.ReadUInt32());
                if (b.ReadUInt16() == 0xFD13) break;
                b.BaseStream.Position -= 2;
            }
        }

        public void FetchCommands(int pointer_number, string script)
        {
            bool isEnd = false;
            var cmd_num = 0;

            b.BaseStream.Position = 0 + (4 * (pointer_number + 1)) + pointers[pointer_number];

            while (isEnd == false)
            {
                Console.WriteLine("pos " + b.BaseStream.Position);
                var cmd = b.ReadUInt16();
                Console.WriteLine("cmd " + cmd);
                if (cmdhndlr.GetCommand(cmd) == "End") isEnd = true;
                var cmdparams = cmdhndlr.GetParameters(cmd);
                ScriptData.Add(cmdhndlr.GetCommand(cmd));
                if (cmdparams != null)
                    for (int i = 0; i < cmdparams.Count; i++)
                    {
                        ScriptData[cmd_num] += " ";
                        ScriptData[cmd_num] += GetParam((cmdparams[i]).ToString());
                    }
                cmd_num++;
            }
            b.Close();
        }

        public void WriteCommands(string script_name, List<string> script)
        {
            List<byte> scriptdata = new List<byte>();
            int i = 0;
            while (true)
            {
                var splitstr = script[i].Split(' ');
                var cmdid = cmdhndlr.GetCommandID(splitstr[0]);
                var cmdparameters = cmdhndlr.GetParameters(cmdid);

                foreach (byte Byte in BitConverter.GetBytes((ushort)cmdid))
                    scriptdata.Add(Byte);

                if (cmdparameters != null)
                    for (int j = 0; j < cmdparameters.Count; j++)
                    {
                        int k = 1;
                        switch (cmdhndlr.GetParamTypeInt(cmdid, j))
                        {
                            case "UInt32":
                                foreach (byte Byte in BitConverter.GetBytes(UInt32.Parse(splitstr[k])))
                                    scriptdata.Add(Byte);
                                break;
                            case "UInt16":
                                foreach (byte Byte in BitConverter.GetBytes(UInt16.Parse(splitstr[k])))
                                    scriptdata.Add(Byte);
                                break;
                            case "UInt8":
                                foreach (byte Byte in BitConverter.GetBytes(SByte.Parse(splitstr[k])))
                                    scriptdata.Add(Byte);
                                break;
                        }
                        k++;
                    }

                if (i+1 != script.Count)
                    i++;
                else
                    break;
            }

            using (BinaryWriter b = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(script_name) + "_rawcmd.bin")))
                foreach (byte Value in scriptdata)
                    b.Write(Value);
        }

        public void BringItHome(string scriptname)
        {
            byte[] header = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x13, 0xFD };
            using (BinaryWriter b = new BinaryWriter(File.Open(Path.GetFileNameWithoutExtension(scriptname), FileMode.Create)))
            {
                // For each script write it to this new file after getting the size.
                b.Write(header, 0, header.Length);
                var script = File.ReadAllBytes(Path.GetFileNameWithoutExtension(scriptname) + "_rawcmd.bin");
                b.Write(script);
            }
        }
    }
}
