using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AVBProject.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;
        public int Swords;

        public PlayerData Clone()
        {
            return new PlayerData
            {
                Hp = Hp,
                Coins = Coins,
                IsArmed = IsArmed
            };

            /*var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);*/
        }
    }
}

