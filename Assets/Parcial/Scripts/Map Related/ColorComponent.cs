namespace FlyEngine
{

    public class ColorComponent : ECSComponent
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public ColorComponent(ColorComponent color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public ColorComponent(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public ColorComponent(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }


    }
}