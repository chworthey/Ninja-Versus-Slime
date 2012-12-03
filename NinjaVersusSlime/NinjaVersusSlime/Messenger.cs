using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NinjaVersusSlime
{
    /// <summary>
    /// A messenge system much like the console- except more artistic. Displays a message.
    /// </summary>
    static class Messenger
    {
        static SpriteFont font;
        static float timeStamp = 0.0f;
        static bool messenging = false;
        static float fade = 0.0f;
        static string msg = "";
        static Vector2 center = Vector2.Zero;

        /// <summary>
        /// Loads content. Call this before you call anything else.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="center">The center of where you want the text to scale around.</param>
        public static void Load(SpriteFont font, Vector2 center)
        {
            Messenger.font = font;
            Messenger.center = center;
        }

        /// <summary>
        /// Displays a message.
        /// </summary>
        /// <param name="s">String to display.</param>
        public static void DisplayMessage(string s)
        {
            messenging = true;
            msg = s;
            timeStamp = -1;
            fade = 1.0f;
        }

        /// <summary>
        /// Updates the message display. Important to call tihs every frame.
        /// </summary>
        /// <param name="time">A snap shot of game time.</param>
        public static void Update(GameTime time)
        {
            if (messenging)
            {
                if (timeStamp == -1)
                    timeStamp = (float)time.TotalGameTime.TotalSeconds;
                else
                {
                    float t = (float)time.TotalGameTime.TotalSeconds - timeStamp;
                    if (t > 5.5)
                        messenging = false;
                    else
                    {
                        fade = 1.5f * (float)Math.Cos((MathHelper.TwoPi / 10.0f) * (t - 0.5f)) + 1.5f;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the message. Call this every frame!!!
        /// </summary>
        /// <param name="spriteBatch">The texture spriteBatch.</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (messenging && timeStamp != -1)
                spriteBatch.DrawString(font, msg, center, new Color(200, 200, 200), 0.0f, font.MeasureString(msg) / 2f,
                    fade, SpriteEffects.None, 0.0f);
        }
    }
}
