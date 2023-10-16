using System.Collections.Generic;
using UnityEngine;

namespace _1v1.lol_cheat
{
    public class Utils : MonoBehaviour
    {
        public static List<PlayerController> GetTargets()
        {
            List<PlayerController> players = new List<PlayerController>();

            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (!player.IsMine())
                {
                    players.Add(player);
                }
            }

            return players;
        }
    }
}
