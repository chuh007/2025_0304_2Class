using TMPro;
using UnityEngine;

namespace Blade.Test.NavMesh
{
    public class TestPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetConstruction(bool isConstruction) 
            => text.text = isConstruction ? "Construction" : "Normal";
    }
}