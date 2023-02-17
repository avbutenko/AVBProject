using Assets.PixelCrew.Model.Definitions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Repositories.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]

    public class ItemsRepository : DefRepository<ItemDef>
    {
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif

        public ItemDef[] GetAllByTags(params ItemTag[] tags)
        {
            var retValue = new List<ItemDef>();

            foreach (var item in _collection)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));

                if (isAllRequirementsMet)
                    retValue.Add(item);
            }
            return retValue.ToArray();
        }

    }

    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;
        [SerializeField] private string _info;
        [SerializeField] private ItemWithCount _price;

        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id);
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }

    }
}