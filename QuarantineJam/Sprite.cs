using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MaskGame
{
    /// <summary>
    /// Un sprite est une image, animée ou non. 
    /// </summary>
    public class Sprite
    {
        public Rectangle Source; // Rectangle permettant de définir la zone de la texture à afficher (la zone de la frame de notre spritesheet)
        public float time, opacity = 1f; // Durée depuis laquelle la frame est à l'écran (on la remettra à zéro à chaque nouvelle frame, sert à mesurer depuis quand on a affiché la frame actuelle)
        public int frameIndex; // Indice de l'image en cours, de 0 à (nombre de frames - 1)
        public int frame_event;
        public float angle;
        public bool isAnimated, reverse;
        public bool isOver, firstFrame, lastFrame;
        public int totalFrames { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public int timeBetweenFrames { get; private set; }
        public Texture2D Texture { get; private set; }
        public float scale { get; set; }
        public SpriteEffects effects;
        public bool loopAnimation { get; private set; }
        public List<Vector2> MaskRelativePosition;
        public int direction;

        public Sprite()
        {

        }
        public Sprite(Texture2D texutre, Rectangle source)
        {
            this.Source = source;
            this.Texture = texutre;
            this.frameWidth = source.Width;
            this.frameHeight = source.Height;
            this.scale = 1;
            this.opacity = 1f;
        }
        public Sprite(Sprite sprite) // In order to copy sprites
        {
            // If crash here, tried to copy a sprite that doesn't exist
            totalFrames = sprite.totalFrames;
            this.angle = 0f;
            this.frameWidth = sprite.frameWidth;
            this.frameHeight = sprite.frameHeight;
            this.timeBetweenFrames = sprite.timeBetweenFrames;
            this.Texture = sprite.Texture;
            this.scale = sprite.scale;
            this.loopAnimation = sprite.loopAnimation;
            this.direction = sprite.direction;
            this.Source = sprite.Source;
            this.reverse = sprite.reverse;
            this.isAnimated = sprite.isAnimated;
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
        }

        public Sprite(Texture2D Texture, float scale = 1f, int direction = 1) // Constructeur d'un sprite non animé, et donc appelé avec uniquement une texture et de manière facultative une échelle de taille.
        {
            this.angle = 0f;
            this.Texture = Texture;
            this.scale = scale;
            this.direction = direction;
            isAnimated = false;
            frameHeight = Convert.ToInt32(scale * Texture.Height);
            frameWidth = Convert.ToInt32(scale * Texture.Width);
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
        }
        public Sprite(int totalAnimationFrames, int frameWidth, int frameHeight, int timeBetweenFrames, Texture2D Texture, float scale = 1f, bool loopAnimation = true, bool reverse = false, int direction = 1, int normalFrameWidth = 0) // Autre constructeur pour quand un gameObject est créé avec les paramètres ci-contre
        {
            this.angle = 0f;
            this.totalFrames = totalAnimationFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.timeBetweenFrames = timeBetweenFrames;
            this.Texture = Texture;
            this.scale = scale;
            this.loopAnimation = loopAnimation;
            this.direction = direction;
            //Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Source = new Rectangle(0, 0, frameWidth, frameHeight);
            this.reverse = reverse;
            isAnimated = true;
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position) // Dessin du sprite depuis son centre
        {

            spriteBatch.Draw(Texture, Position, Source, Color.White, angle, new Vector2(Texture.Width / 2, Texture.Height / 2), scale, effects, 1f); // On remarque l'ajoute de l'argument Source qui corresponds à la zone de la spritesheet qu'on dessine (voir comment elle est sélectionnée plus bas)
        }

        public void ScreenDraw(SpriteBatch spriteBatch, Vector2 Position) // Dessin du sprite avec la position sur l'écran
        {

            spriteBatch.Draw(Texture, Position, Source, Color.White * opacity, 0f, new Vector2(0, 0), scale, effects, 1f); // On remarque l'ajoute de l'argument Source qui corresponds à la zone de la spritesheet qu'on dessine (voir comment elle est sélectionnée plus bas)
        }

        public void ScreenCenterDraw(SpriteBatch spriteBatch, Vector2 Position) // Dessin du sprite avec la position sur l'écran
        {
            spriteBatch.Draw(Texture, Position, Source, Color.White * opacity, 0f, new Vector2(0.5f * Texture.Width, 0.5f * Texture.Height), this.scale, effects, 1f); // On remarque l'ajoute de l'argument Source qui corresponds à la zone de la spritesheet qu'on dessine (voir comment elle est sélectionnée plus bas)
        }

        public void DrawFromFeet(SpriteBatch spriteBatch, Vector2 FeetPosition, float Angle = 0f, float Opacity = 1f) // Dessin du sprite à partir de la position des pieds (pied = milieu de l'image en bas) s'il y en a une
        {
            float angle2;
            if (Angle == 0f) angle2 = angle;
            else angle2 = Angle;
            Vector2 FeetRelativePosition = new Vector2(-0.5f * frameWidth, -frameHeight);
            Vector2 display_position = FeetPosition + scale * FeetRelativePosition;
            spriteBatch.Draw(Texture, display_position, Source, Color.White * Opacity * opacity, angle2, Vector2.Zero, scale, effects, 1f); // on obtient le point en haut à gauche à partir de la position des pieds
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, Rectangle Source) // Pour dessiner une partie d'une image fixe. La position = la position du centre de l'image
        {
            Vector2 display_position = Position + scale * new Vector2(frameWidth / 2, frameHeight / 2);
            spriteBatch.Draw(Texture, display_position, Source, Color.White, 0f, new Vector2(0, 0), scale, effects, 1f);
        }

        public void TopLeftDraw(SpriteBatch spriteBatch, Vector2 Position, float Opacity = 1f) // Pour dessiner une partie d'une image, en donnant sa position d'affichage en haut à droite
        {
            Vector2 display_position = Position;
           spriteBatch.Draw(Texture, display_position, Source, Color.White * Opacity, 0f, new Vector2(0, 0), scale, effects, 1f);
        }

        public void UpdateFrame(GameTime gameTime) // Pour décider quelle frame on affiche
        {
            isOver = false;
            lastFrame = false;
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
            if (isAnimated)
            {
                if (time == 0 && frameIndex == 0) firstFrame = true;
                else firstFrame = false;

                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Le temps en millisecondes depuis le dernier changement de frame

                frame_event = new int();
                if (time > timeBetweenFrames)
                {
                    if (reverse) frameIndex--; else frameIndex++; // Frame suivante
                    frame_event = frameIndex;
                    time = 0f; // On remet le temps à 0
                }
                if (time + (float)gameTime.ElapsedGameTime.TotalMilliseconds > timeBetweenFrames && (frameIndex == totalFrames - 1))
                {
                    lastFrame = true;
                }
                if ((frameIndex >= totalFrames || frameIndex < 0)) // Si le numéro de la frame dépasse le nombre de frames
                {
                    if (loopAnimation)
                    {
                        if (reverse) frameIndex = totalFrames;
                        else frameIndex = 0; // On retourne à la première frame
                    }
                    else
                    {
                        if (reverse) frameIndex++;
                        else frameIndex--; // retour à la frame précédente
                    }

                    isOver = true;
                }

                Source = new Rectangle(( // Le rectangle de la frame
                        frameIndex % (Texture.Width / frameWidth)) * frameWidth,
                        ((frameIndex - (frameIndex % (Texture.Width / frameWidth))) / (Texture.Width / frameWidth)) * frameHeight,
                        frameWidth,
                        frameHeight);
            }
            else
            {
                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Texture != null) Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            }
        }

        public void ResetAnimation()
        {
            isOver = false;
            if (!reverse) frameIndex = 0;
            else frameIndex = totalFrames - 1;
            time = 0;
        }
        public void Tint(Color color)
        {
            Color[] colorData = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(colorData);

            for (int i = 0; i< Texture.Width; i++) for (int j = 0; j < Texture.Height; j++)
                {
                    Color pixel = colorData[i * Texture.Height + j];
                    pixel = new Color((pixel.R + color.R) / 2f, (pixel.G + color.G) / 2f, (pixel.B + color.B) / 2f);
                    colorData[i * Texture.Height + j] = pixel;
                }

            Texture.SetData<Color>(colorData);
        }
    }
}