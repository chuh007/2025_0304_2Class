using System;
using Blade.Combat;
using TMPro;
using UnityEngine;

namespace Blade.Test
{
    public class HPText : MonoBehaviour
    {
        [SerializeField] private EntityHealth entityHealth;
        
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _text.text = entityHealth.CurrentHealth.ToString();
        }
    }
}