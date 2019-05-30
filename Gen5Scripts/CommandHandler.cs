using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen5Scripts
{
    class CommandHandler
    {
        List<(string name, List<string> parameters)> CommandList = new List<(string name, List<string> parameters)>
        {
            ("Nop", null),
            ("Nop", null),
            ("End", null),
            ("ReturnAfterDelay", null),
            ("CallRoutine", new List<string>{ "UInt16", "UInt32" }),
            ("EndFunction", null),
            ("Logic06", new List<string>{ "UInt16" }),
            ("Logic07", new List<string>{ "UInt16" }),
            ("CompareTo", new List<string>{ "UInt16" }),
            ("StoreVar", new List<string>{ "UInt16" }),
            ("ClearVar", new List<string>{ "UInt16" }),
            ("0B", new List<string>{ "UInt16" }),
            ("0C", new List<string>{ "UInt16" }),
            ("0D", new List<string>{ "UInt16" }),
            ("0E", new List<string>{ "UInt16" }),
            ("0F", new List<string>{ "UInt16" }),
            ("StoreFlag", new List<string>{ "UInt16" }),
            ("Condition", new List<string>{ "UInt16" }),
            ("12", new List<string>{ "UInt16", "UInt16" }),
            ("13", new List<string>{ "UInt16", "UInt16" }),
            ("14", new List<string>{ "UInt16" }),
            ("15", new List<string>{ "UInt16" }),
            ("16", new List<string>{ "UInt16" }),
            ("17", new List<string>{ "UInt16" }),
            ("Nop", null),
            ("Compare", new List<string>{ "UInt16", "UInt16" }),
            ("Nop", null),
            ("Nop", null),
            ("CallStd", new List<string>{ "UInt16" }),
            ("ReturnStd", null),
            ("Jump", new List<string>{ "UInt32" }),
            ("If", new List<string>{ "UInt8", "UInt32" }),
            ("Nop", null),
            ("21", new List<string>{ "UInt16" }),
            ("22", new List<string>{ "UInt16" }),
            ("SetFlag", new List<string>{ "UInt16" }),
            ("ClearFlag", new List<string>{ "UInt16" }),
            ("SetVarFlagStatus", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar26", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar27", new List<string>{ "UInt16", "UInt16" }),
            ("SetVarEqVal", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar29", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar2A", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar2B", new List<string>{ "UInt16" }),
            ("Nop", null),
            ("2D", new List<string>{ "UInt16" }),
            ("LockAll", null),
            ("UnlockAll", null),
            ("WaitMoment", null),
            ("Nop", null),
            ("WaitButton", null),
            ("MusicalMessage", new List<string>{ "UInt16" }),
            ("EventGreyMessage", new List<string>{ "UInt16", "UInt16" }),
            ("CloseMusicalMessage", null),
            ("CloseEventGreyMessage", null),
            ("Nop", null),
            ("BubbleMessage", new List<string>{ "UInt16", "UInt8" }),
            ("CloseBubbleMessage", null ),
            ("ShowMessageAt", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("CloseShowMessageAt", new List<string>{ "UInt16" }),
            ("Message", new List<string>{ "UInt8", "UInt8", "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("Message2", new List<string>{ "UInt8", "UInt8", "UInt16", "UInt16", "UInt16" }),
            ("CloseMessageKeyPress", null ),
            ("CloseMessageKeyPress2", null ),
            ("MoneyBox", new List<string>{ "UInt16", "UInt16" }),
            ("CloseMoneyBox", null ),
            ("UpdateMoneyBox", null ),
            ("BorderedMessage", new List<string>{ "UInt16", "UInt16" }),
            ("CloseBorderedMessage", null ),
            ("PaperMessage", new List<string>{ "UInt16", "UInt16" }),
            ("ClosePaperMessage", null ),
            ("YesNoBox", new List<string>{ "UInt16" }),
            ("Message3", new List<string>{ "UInt8", "UInt8", "UInt16", "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("DoubleMessage", new List<string>{ "UInt8", "UInt8", "UInt16", "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("AngryMessage", new List<string>{ "UInt16", "UInt8", "UInt16" }),
            ("CloseAngryMessage", null),
            ("SetVarHero", new List<string>{ "UInt8" }),
            ("SetVarItem", new List<string>{ "UInt8", "UInt16" }),
            ("4E", new List<string>{ "UInt8", "UInt16", "UInt16", "UInt8" }),
            ("SetVarItem2", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarItem3", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarMove", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarBag", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPartyPokemon", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPartyPokemon2", new List<string>{ "UInt8", "UInt16" }),
            ("SetVar????", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarType", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPokemon", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPokemon2", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarLocation", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPokemonNick", new List<string>{ "UInt8", "UInt16" }),
            ("SetVar????2", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarStoreVal5C", new List<string>{ "UInt8", "UInt16", "UInt16" }),
            ("SetVarMusicalInfo", new List<string>{ "UInt16", "UInt16" }),
            ("SetVarNations", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarActivities", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarPower", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarTrainerType", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarTrainerType2", new List<string>{ "UInt8", "UInt16" }),
            ("SetVarGeneralWord", new List<string>{ "UInt8", "UInt16" }),
            ("ApplyMovement", new List<string>{ "UInt16", "UInt32" }),
            ("WaitMovement", null ),
            ("StoreHeroPosition 0x66", new List<string>{ "UInt16", "UInt16" }),
            ("67", new List<string>{ "UInt16", "UInt16" }),
            ("StoreHeroPosition", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("StoreNPCPosition", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("6A", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("AddNPC", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("RemoveNPC", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("SetOWPosition", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("6E", new List<string>{ "UInt16" }),
            ("6F", new List<string>{ "UInt16" }),
            ("70", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("71", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("72", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("73", new List<string>{ "UInt16", "UInt16" }),
            ("FacePlayer", null),
            ("Release", new List<string>{ "UInt16" }),
            ("ReleaseAll", null),
            ("Lock", new List<string>{ "UInt16" }),
            ("78", new List<string>{ "UInt16" }),
            ("79", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("Nop", null),
            ("MoveNpctoCoordinates", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("7C", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("7D", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("TeleportupNPC", new List<string>{ "UInt16" }),
            ("7F", new List<string>{ "UInt16", "UInt16" }),
            ("80", new List<string>{ "UInt16" }),
            ("81", null ),
            ("82", new List<string>{ "UInt16", "UInt16" }),
            ("SetVar0x83", new List<string>{ "UInt16" }),
            ("SetVar0x84", new List<string>{ "UInt16" }),
            ("SingleTrainerBattle", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("DoubleTrainerBattle", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("87", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("88", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("Nop", null),
            ("8A", new List<string>{ "UInt16", "UInt16" }),
            ("PlayTrainerMusic", new List<string>{ "UInt16" }),
            ("EndBattle", null ),
            ("StoreBattleResult", new List<string>{ "UInt16" }),
            ("DisableTrainer", null ),
            ("Nop", null),
            ("dvar90", new List<string>{ "UInt16", "UInt16" }),
            ("91", new List<string>{ "UInt32", "UInt16", "UInt8" }),
            ("dvar92", new List<string>{ "UInt16", "UInt16" }),
            ("dvar93", new List<string>{ "UInt16", "UInt16" }),
            ("TrainerBattle", new List<string>{ "UInt16", "UInt16", "UInt16", "UInt16" }),
            ("DeactivateTrainerID", new List<string>{ "UInt16" }),
            ("96", new List<string>{ "UInt16" }),
            ("StoreActiveTrainerID", new List<string>{ "UInt16", "UInt16" }),
            ("ChangeMusic", new List<string>{ "UInt16" }),
            ("99", null),
            ("9A", null),
            ("9B", null),
            ("9C", null),
            ("9D", null),
            ("FadeToDefaultMusic", null),
            ("9F", new List<string>{ "UInt16" }),
            ("Nop", null),
            ("Nop", null),
            ("A2", new List<string>{ "UInt16", "UInt16" }),
            ("A3", new List<string>{ "UInt16" }),
            ("A4", new List<string>{ "UInt16" }),
            ("A5", new List<string>{ "UInt16", "UInt16" }),
            ("PlaySound", new List<string>{ "UInt16" }),
            ("WaitSoundA7", null),
            ("WaitSound", null),
            ("PlayFanfare", new List<string>{ "UInt16" }),
            ("WaitFanfare", null),
            ("Cry", new List<string>{ "UInt16", "UInt16" }),
            ("WaitCry", null),
            ("Nop", null),
            ("Nop", null),
            ("SetTextScriptMessage", new List<string>{ "UInt16", "UInt16", "UInt16" }),
            ("CloseMulti", null),
            ("B1", null),
            ("Multi2", new List<string>{"UInt8", "UInt8", "UInt8", "UInt8", "UInt16"}),
            ("FadeScreen", new List<string>{"UInt16", "UInt16", "UInt16", "UInt16"}),
            ("ResetScreen", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("ScreenB5", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("TakeItem", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("CheckItemBagSpace", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("CheckItemBagNumber", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("StoreItemCount", new List<string>{"UInt16", "UInt16"}),
            ("BA", new List<string>{"UInt16", "UInt16", "UInt16", "UInt16"}),
            ("BB", new List<string>{"UInt16", "UInt16"}),
            ("BC", new List<string>{"UInt16"}),
            ("Nop", null),
            ("Warp", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("TeleportWarp", new List<string>{"UInt16", "UInt16", "UInt16", "UInt16"}),
            ("Nop", null),
            ("FallWarp", new List<string>{"UInt16", "UInt16", "UInt16"}),
            ("FastWarp", new List<string>{"UInt16", "UInt16", "UInt16", "UInt16"}),
            ("UnionWarp", null),
        };

        public string GetCommand(int command)
        {
                return CommandList[command].name;
        }

        public List<string> GetParameters(int command)
        {
            return CommandList[command].parameters;
        }

        public int GetCommandID(string command)
        {
            int i = 0;
            while(true)
            {
                if (command != CommandList[i].name)
                    i++;
                else
                    break;
            }
            return i;
        }

        public string GetParamTypeStr(string command, int parameter)
        {
            var cmdid = GetCommandID(command);
            return CommandList[cmdid].parameters[parameter];
        }

        public string GetParamTypeInt(int command, int parameter)
        {
            return CommandList[command].parameters[parameter];
        }
    }
}
