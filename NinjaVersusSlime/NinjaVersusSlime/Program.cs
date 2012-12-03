using System;

namespace NinjaVersusSlime
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameControl game = new GameControl())
            {
                game.Run();
            }
        }
    }
#endif
}

