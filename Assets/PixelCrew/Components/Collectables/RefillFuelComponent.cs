﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Model;

namespace Assets.PixelCrew.Components.Collectables
{
    public class RefillFuelComponent : MonoBehaviour
    {
        private GameSession _session;
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void Refill()
        {
            _session.Data.Fuel.Value = 100;
        }
    }
}

