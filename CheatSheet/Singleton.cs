namespace CheatSheet
{
    static class Singleton
    {
        static readonly object O;

        static Singleton()
        {
            O = new object();
        }
    }
}