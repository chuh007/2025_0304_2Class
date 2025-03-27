namespace Blade.Test
{
    public class Rectangle : Shape
    {
        public float width;
        public float height;
        public override float GetArea()
        {
            return width * height;
        }
    }
}