using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.Interactions
{
    [RequireComponent(typeof(SpawnComponent))]
    public class RequireItemComponent : MonoBehaviour
    {

        [SerializeField] InventoryItemData[] _required;
        [SerializeField] bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSucsess;
        [SerializeField] private UnityEvent _onFail;

        public void Check()
        {
            var session = GameSession.Instance;
            var areAllrequirementsMet = true;

            foreach (var item in _required)
            {
                var numItems = session.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                    areAllrequirementsMet = false;
            }

            if (areAllrequirementsMet)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                        session.Data.Inventory.Remove(item.Id, item.Value);
                }

                _onSucsess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }


        }
    }
}