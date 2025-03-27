using UnityEngine;

namespace Blade.Test
{
    public class Circle : Shape
    {
        public float radius;
        public override float GetArea()
        {
            return radius * radius * Mathf.PI;
        }
    }
}