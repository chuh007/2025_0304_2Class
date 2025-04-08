using UnityEngine;

namespace Blade.Test.Drags
{
    public class TestUnit : MonoBehaviour
    {
        [SerializeField] private GameObject selectedIcon;
        
        public void SetSelected(bool isSelected)
        {
            selectedIcon.SetActive(isSelected);
        }
    }
}