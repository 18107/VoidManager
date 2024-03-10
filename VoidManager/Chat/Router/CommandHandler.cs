﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidManager.Chat.Router
{
    internal class CommandHandler
    {
        private static Dictionary<string, ChatCommand> chatCommands = new Dictionary<string, ChatCommand>();
        private static Dictionary<string, PublicCommand> publicCommands = new Dictionary<string, PublicCommand>();
        public static int chatCommandCount => chatCommands.Count;
        public static int publicCommandCount => publicCommands.Count;

        /// <summary>
        /// Executes chat command if found from alias with arguments.
        /// </summary>
        /// <param name="alias">Potential chat command alias</param>
        /// <param name="arguments">Arguments to use with command</param>
        internal static void ExecuteCommandFromAlias(string alias, string arguments, bool publicCommand = false, int playerId = -1)
        {
            alias = alias.ToLower();
            try
            {
                if (publicCommand) { if (publicCommands.ContainsKey(alias)) publicCommands[alias].Execute(arguments, playerId); }
                else if (chatCommands.ContainsKey(alias)) chatCommands[alias].Execute(arguments);
                else Plugin.Log.LogInfo($"'{(publicCommand ? "!" : "/")}{alias} {arguments}' cound not be found!");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"'{(publicCommand ? "!" : "/")}{alias} {arguments}' failed! \nCommand Exception: {ex.Message}!\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Gets chat command from alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>ChatCommand reference from alias</returns>
        public static ChatCommand GetCommand(string alias)
        {
            if (chatCommands.ContainsKey(alias)) return chatCommands[alias];
            else return null;
        }

        /// <summary>
        /// Gets ordered list of chat commands.
        /// </summary>
        /// <returns>Ordered list of chat commands</returns>
        public static IOrderedEnumerable<ChatCommand> GetCommands()
        {
            return new HashSet<ChatCommand>(chatCommands.Values).OrderBy(t => t.CommandAliases()[0]);
        }

        /// <summary>
        /// Gets public command from alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>PublicCommand reference from alias</returns>
        public static PublicCommand GetPublicCommand(string alias)
        {
            if (publicCommands.ContainsKey(alias)) return publicCommands[alias];
            else return null;
        }

        /// <summary>
        /// Gets ordered list of public chat commands.
        /// </summary>
        /// <returns>Ordered list of public chat commands</returns>
        public static IOrderedEnumerable<PublicCommand> GetPublicCommands()
        {
            return new HashSet<PublicCommand>(publicCommands.Values).OrderBy(t => t.CommandAliases()[0]);
        }

        /// <summary>
        /// Iterates through the current Plugin files and searches for commands.
        /// </summary>
        public static void DiscoverCommands(System.Reflection.Assembly assembly, string ModName = "")
        {
            var types = assembly.GetTypes();
            // Finds ChatCommand implementations from all the Assemblies in the same file location.
            var chatCommandInstances = types.Where(t => typeof(ChatCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            int commandCount = 0;
            foreach (var modType in chatCommandInstances)
            { // Iterates through each discovered ChatCommand
                ChatCommand modInstance = (ChatCommand)Activator.CreateInstance(modType);
                foreach (string commandAlias in Array.ConvertAll(modInstance.CommandAliases(), d => d.ToLower()))
                {
                    if (chatCommands.ContainsKey(commandAlias))
                    {
                        Plugin.Log.LogInfo($"[{ModName}] Found duplicate command alias {commandAlias}");
                        continue;
                    }
                    else
                    {
                        chatCommands.Add(commandAlias, modInstance);
                        commandCount++;
                    }
                }
            }
            Plugin.Log.LogInfo($"[{ModName}] Added {commandCount} chat commands");
        }
        /// <summary>
        /// Iterates through the current Plugin files and searches for public commands.
        /// </summary>
        public static void DiscoverPublicCommands(System.Reflection.Assembly assembly, string ModName = "")
        {
            var types = assembly.GetTypes();
            // Finds PublicCommand implementations from all the Assemblies in the same file location.
            var publicCommandInstances = types.Where(t => typeof(PublicCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            int commandCount = 0;
            foreach (var modType in publicCommandInstances)
            { // Iterates through each discovered PublicCommand
                PublicCommand modInstance = (PublicCommand)Activator.CreateInstance(modType);
                foreach (string commandAlias in Array.ConvertAll(modInstance.CommandAliases(), d => d.ToLower()))
                {
                    if (publicCommands.ContainsKey(commandAlias))
                    {
                        Plugin.Log.LogInfo($"[{ModName}] Found duplicate public command alias {commandAlias}");
                        continue;
                    }
                    else
                    {
                        publicCommands.Add(commandAlias, modInstance);
                        commandCount++;
                    }
                }
            }
            Plugin.Log.LogInfo($"[{ModName}] Added {commandCount} public commands");
        }
    }
}
