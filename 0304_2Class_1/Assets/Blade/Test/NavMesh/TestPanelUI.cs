using TMPro;
using UnityEngine;

namespace Blade.Test.NavMesh
{
    public class TestPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetConstrucion(bool isConstrucion) => text.text = isConstrucion? "Construcion" : "Nomal";
    }
}