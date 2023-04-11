using Assets.PixelCrew.Model;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.UI.Hud
{
    public class RightStickController : MonoBehaviour
    {

        [SerializeField] private GameObject _attackButton;
        [SerializeField] private GameObject _stickButton;
        [SerializeField] private GameObject _useButton;
        [SerializeField] private RectTransform _useButtonDefaultRect;
        [SerializeField] private RectTransform _useButtonTargetRect;
        [SerializeField] private GameObject _flashLightButton;
        [SerializeField] private RectTransform _flashLightDefaultRect;
        [SerializeField] private RectTransform _flashLightTargetRect;

        private string SelectedItemId => GameSession.Instance.QuickInventory.SelectedItem?.Id;

        private const string SwordId = "Sword";
        private const string PistolId = "pistol";

        private RectTransform _useButtonRect;
        private RectTransform _flashLightRect;

        private void Start()
        {
            _useButtonRect = _useButton.GetComponent<RectTransform>();
            _flashLightRect = _flashLightButton.GetComponent<RectTransform>();
        }
        void Update()
        {
            switch (SelectedItemId)
            {
                case null:
                    _attackButton.SetActive(false);
                    _stickButton.SetActive(false);
                    _useButton.SetActive(false);
                    _flashLightRect.transform.localPosition = _flashLightTargetRect.transform.localPosition;
                    break;
                case SwordId:
                    _attackButton.SetActive(true);
                    _stickButton.SetActive(false);
                    _flashLightRect.transform.localPosition = _flashLightDefaultRect.transform.localPosition;
                    _useButtonRect.transform.localPosition = _useButtonDefaultRect.transform.localPosition;
                    _useButton.SetActive(true);
                    break;
                case PistolId:
                    _attackButton.SetActive(false);
                    _stickButton.SetActive(true);
                    _flashLightRect.transform.localPosition = _flashLightDefaultRect.transform.localPosition;
                    _useButtonRect.transform.localPosition = _useButtonDefaultRect.transform.localPosition;
                    _useButton.SetActive(true);
                    break;
                default:
                    _attackButton.SetActive(false);
                    _stickButton.SetActive(false);
                    _flashLightRect.transform.localPosition = _flashLightDefaultRect.transform.localPosition;
                    _useButtonRect.transform.localPosition = _useButtonTargetRect.transform.localPosition;
                    _useButton.SetActive(true);
                    break;
            }
        }
    }
}