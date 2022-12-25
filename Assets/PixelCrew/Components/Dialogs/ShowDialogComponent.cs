﻿using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.UI.Hud.Dialogs;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;
        public void Show()
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(Data);
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject controllerGo = null;

            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                default:
                    throw new ArgumentException("Undefined Dialog Type");
            }

            return controllerGo.GetComponent<DialogBoxController>();
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}