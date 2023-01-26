﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {

        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var localeItem in _localeItems)
            {
                dictionary.Add(localeItem.Key, localeItem.Value);
            }

            return dictionary;
        }
#if UNITY_EDITOR
        [ContextMenu("Update Locale")]
        public void UpdateLocale()
        {
            if (_request != null) return;
            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        [ContextMenu("Update Locale from File")]
        public void UpdateLocaleFromFile()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Choose locale file", "", "tsv");
            if (path.Length != 0)
            {
                var data = File.ReadAllText(path);
                ParseData(data);
            }
        }
#endif
        private void OnDataLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                var data = _request.downloadHandler.text;
                ParseData(data);
            }
        }

        private void ParseData(string data)
        {
            var rows = data.Split('\n');
            _localeItems.Clear();
            foreach (var row in rows)
            {
                AddLocaleItem(row);
            }
        }

        private void AddLocaleItem(string row)
        {
            try
            {
                var parts = row.Split('\t');
                _localeItems.Add(new LocaleItem { Key = parts[0], Value = parts[1] });
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't parse row: {row}. \n {e}");
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}