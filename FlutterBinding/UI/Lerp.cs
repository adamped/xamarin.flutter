namespace FlutterBinding.UI
{
    public static class Lerp
    {
        public static double lerpDouble(double a, double b, double t)
        {
            return a + (b - a) * t;
        }
    }
}
