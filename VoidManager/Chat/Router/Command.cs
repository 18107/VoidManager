﻿
namespace VoidManager.Chat.Router
{
    /// <summary>
    /// Handles a command instance for use via in-game chat.
    /// </summary>
    public abstract class ChatCommand
    {
        /// <summary>
        /// Command aliases will fail silently if the alias is not unique
        /// </summary>
        /// <returns>An array containing names for the command that can be used by the player</returns>
        public abstract string[] CommandAliases();
        /// <returns>A short description of what the command does</returns>
        public abstract string Description();

        /// <returns>Examples of how to use the command including what arguments are valid</returns>
        public virtual string[] UsageExamples()
        {
            return new string[] { $"/{CommandAliases()[0]}" };
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="arguments">A string containing all of the text entered after the command</param>
        public abstract void Execute(string arguments);
    }

    /// <summary>
    /// Handles a command instance for use via in-game chat.
    /// </summary>
    public abstract class PublicCommand
    {
        /// <summary>
        /// Command aliases will fail silently if the alias is not unique
        /// </summary>
        /// <returns>An array containing names for the command that can be used by the player</returns>
        public abstract string[] CommandAliases();
        /// <returns>A short description of what the command does</returns>
        public abstract string Description();

        /// <returns>Examples of how to use the command including what arguments are valid</returns>
        public virtual string[] UsageExamples()
        {
            return new string[] { $"/{CommandAliases()[0]}" };
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="arguments">A string containing all of the text entered after the command</param>
        /// <param name="SenderID">Handler identifier for player</param>
        public abstract void Execute(string arguments, int Sender);
    }
}