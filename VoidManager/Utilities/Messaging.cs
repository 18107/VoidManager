﻿using ExitGames.Client.Photon;
using Gameplay.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using VoidManager.Callbacks;

namespace VoidManager.Utilities
{
    public class Messaging
    {
        /// <summary>
        /// Inserts a line to text chat with reference to the executing assembly.
        /// </summary>
        /// <param name="message"></param>
        public static void Notification(string message)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            TextChat.Instance.AddLog(new Log($"{assembly.FullName.Split(',')[0]}", message));
        }

        /// <summary>
        /// Inserts a line to text chat.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="local"></param>
        public static void Echo(string message, bool local = true)
        {
            if (local) TextChat.Instance.AddLog(new Log($"", message));//fixme
            else
            {
                VoipService.Instance.SendTextMessage($"[Mod Manager]: {message}");
            }
        }

        public static void KickMessage(string title, string body, Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                BepinPlugin.Log.LogInfo($"Sending kick message to {player.NickName}: {title}::{body}");
                PhotonNetwork.RaiseEvent(InRoomCallbacks.KickMessageEventCode, new object[] { title, body }, new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendUnreliable);
            }
            else
            {
                BepinPlugin.Log.LogWarning($"Cannot send kick message while not master client.");
            }
        }
    }
}
