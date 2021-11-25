using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème
{
    public class Pixel
    {
        //Attributs
        private byte red;
        private byte green;
        private byte blue;

        //Constructeur
        public Pixel(byte red, byte green, byte blue)
        {
            //Console.WriteLine("ENTREE de Pixel() red=" + red + " green=" + green + " blue=" + blue);
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        //Propriétés
        public byte Red
        {
            get { return red; }
            set { red = value; }
        }
        public byte Green
        {
            get { return green; }
            set { green = value; }
        }
        public byte Blue
        {
            get { return blue; }
            set { blue = value; }
        }

        //Méthodes
        public void Transforme_En_Gris()
        {
            int moyenne = ((int)red + (int)green + (int)blue) / 3;
            red = (byte)moyenne;
            green =(byte)moyenne;
            blue = (byte)moyenne;
        }

        public void Transforme_En_Noir_Ou_Blanc()
        {
            int somme = red + green + blue;
            if (somme<=382) //La somme RGB est plus proche de la couleur noire (0) que blanche (255)
            {
                red = 0;
                green = 0;
                blue = 0;
            }
            else //La somme RGB est plus proche de la couleur blanche (255) que noire (0)
            {
                red = 255;
                green = 255;
                blue = 255;
            }
        }

        public Pixel Clone_Pixel()
        {
            Pixel clone_pixel = new Pixel(red, green, blue);
            return clone_pixel;
        }

        //Chaque pixel est remplacé par son inverse (le symétrique par rapport à 128)
        public void Pixel_inverse()
        {
            red = (byte)(255 - red);
            green = (byte)(255 - green);
            blue = (byte)(255 - blue);
        }


        //Méthodes pour cacher une image stéganographie
        public int[] ConvertToBase2(byte couleur)
        {
            int couleurint = (int)couleur;
            int[] couleurbase2 = new int[8];
            for (int i=7;i>=0;i--)
            {
                if (couleurint > 0)
                {
                    couleurbase2[i] = couleurint % 2;
                    couleurint = (int)(couleurint / 2);
                }
                else couleurbase2[i] = 0;
            }
            return couleurbase2;          
        }

        public int[,] ConvertPixelToBase2()
        {
            int[,] pixelbase2 = new int[3, 8];
            int[] redbase2 = ConvertToBase2(red);
            int[] greenbase2 = ConvertToBase2(green);
            int[] bluebase2 = ConvertToBase2(blue);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0) pixelbase2[i, j] = redbase2[j];
                    if (i == 1) pixelbase2[i, j] = greenbase2[j];
                    if (i == 2) pixelbase2[i, j] = bluebase2[j];
                }
            }
            return pixelbase2;
        }

        public byte ConvertToByte(int [] couleur)
        {
            byte couleurbyte = 0;
            int a = 7;
            for (int i=0; i<8;i++)
            {
                couleurbyte = (byte)(couleurbyte+couleur[i] * Math.Pow(2, a));
                a--;
            }
            return couleurbyte;
        }

        public Pixel ConvertBase2ToPixel(int [,] pixelbase2)
        {
            Pixel pixelbyte = new Pixel(0,0,0);
            int[] red = new int[8];
            for (int i=0; i<8; i++)
            {
                red[i] = pixelbase2[0, i];
            }
            byte redbyte = ConvertToByte(red);
            int[] green = new int[8];
            for (int i = 0; i < 8; i++)
            {
                green[i] = pixelbase2[1, i];
            }
            byte greenbyte = ConvertToByte(green);
            int[] blue = new int[8];
            for (int i = 0; i < 8; i++)
            {
                blue[i] = pixelbase2[2, i];
            }
            byte bluebyte = ConvertToByte(blue);
            pixelbyte.red = redbyte;
            pixelbyte.green = greenbyte;
            pixelbyte.blue = bluebyte;
            return pixelbyte;
        }

        public Pixel Mixer2Pixels(Pixel p2)
        {
            int[,] pixel1 = ConvertPixelToBase2();
            int[,] pixel2 = p2.ConvertPixelToBase2();
            for (int i=0;i<3;i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (j >= 4) pixel1[i, j] = pixel2[i, j-4];
                }
            }
            return ConvertBase2ToPixel(pixel1);
        }


        //Méthodes pour retrouver une image 
        public byte RetrouverCouleurReference(byte couleur)
        {
            int[] couleurbase2 = ConvertToBase2(couleur);
            int[] couleurretrouvee = null;
            for (int i=0; i<8;i++)
            {
                if (i<4) couleurretrouvee[i] = couleurbase2[i];
                if (i >= 4) couleurretrouvee[i] = 0;
            }
            return (ConvertToByte(couleurretrouvee));
        }

        public Pixel RetrouverPixelReference()
        {
            Pixel pixelreference=new Pixel(RetrouverCouleurReference(red), RetrouverCouleurReference(green), RetrouverCouleurReference(blue));
            return pixelreference;
        }

        public byte RetrouverCouleurImageCachee(byte couleur)
        {
            int[] couleurbase2 = ConvertToBase2(couleur);
            int[] couleurretrouvee = null;
            for (int i = 0; i < 8; i++)
            {
                if (i < 4) couleurretrouvee[i] = 0;
                if (i >= 4) couleurretrouvee[i] = couleurbase2[i-4];
            }
            return ConvertToByte(couleurretrouvee);
        }

        public Pixel RetrouverPixelImageCachee()
        {
            Pixel pixelimagecachee = new Pixel(RetrouverCouleurImageCachee(red), RetrouverCouleurImageCachee(green), RetrouverCouleurImageCachee(blue));
            return pixelimagecachee;
        }
    }
}
