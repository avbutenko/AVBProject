using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils;

public class ShowWindowComponent : MonoBehaviour
{
    [SerializeField] private string _path;
    public void Show()
    {
        WindowUtils.CreateWindow(_path);
    }
}
