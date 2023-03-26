using Assets.PixelCrew.Model;
using Assets.PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Hero.Features
{
    public class HeroInvisibility : MonoBehaviour
    {

        private SpriteRenderer _renderer;
        private Color _color;

        private void Start()
        {
            _renderer = GetComponentInParent<SpriteRenderer>();
            _color = _renderer.color;
        }
        public void Use()
        {
            _color.a = .2f;
            _renderer.color = _color;
        }

        [ContextMenu("UseIt")]
        public void UseIt()
        {
            Use();
        }

        private void Update()
        {
            if (!GameSession.Instance.Perks.IsHeroInvisible)
            {
                _color.a = 1;
                _renderer.color = _color;
            }
        }
    }
}