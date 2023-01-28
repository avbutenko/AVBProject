using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Model;

public class RestoreStateComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    public string Id => _id;
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        var isDestroyed = _session.RestoreState(_id);
        if (isDestroyed)
            Destroy(gameObject);
    }
}
