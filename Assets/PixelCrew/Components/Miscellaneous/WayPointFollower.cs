﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AVBProject.Components
{
    public class WayPointFollower : MonoBehaviour
    {
        [SerializeField] public List<Transform> waypoints;
        [SerializeField] public float moveSpeed;
        [SerializeField] public int target;
        [SerializeField] private UnityEvent _action;

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, moveSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (transform.position == waypoints[target].position)
            {
                if (target == waypoints.Count - 1)
                {
                    target = 0;
                }
                else
                {
                    target += 1;
                }

                _action?.Invoke();
            }
        }
    }
}
