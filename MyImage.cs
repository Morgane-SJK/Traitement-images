using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace Problème
{

    public class MyImage
    {
        //Attributs
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int tailleHeader;
        private int tailleImage;
        private int largeur;
        private int hauteur;
        private int nbBitsParCouleur;
        private Pixel[,] image;

        //Constructeur n°1
        //Création d'une image à partir du fichier passé en paramètre
        public MyImage(string myfile)
        {
            byte[] tabImage;
            tabImage = File.ReadAllBytes(myfile); //lire le fichier 

            // Type d'image
            if ((tabImage[0] == 66) && (tabImage[1] == 77))
            {
                this.typeImage = "BM"; //on ne traite que les images en format Bitmap
            }
            else
            {
                this.typeImage = "Autre que BM";
            }
            //Console.WriteLine("typeImage="+typeImage);

            // Taille du fichier
            byte[] tabBytesTailleFichier = new Byte[4];
            tabBytesTailleFichier[0] = tabImage[2];
            tabBytesTailleFichier[1] = tabImage[3];
            tabBytesTailleFichier[2] = tabImage[4];
            tabBytesTailleFichier[3] = tabImage[5];
            this.tailleFichier = Convertir_Endian_To_Int(tabBytesTailleFichier);
            //Console.WriteLine("tailleFichier="+ tailleFichier);

            // Taille Offset
            byte[] tabBytesTailleOffset = new Byte[4];
            tabBytesTailleOffset[0] = tabImage[10];
            tabBytesTailleOffset[1] = tabImage[11];
            tabBytesTailleOffset[2] = tabImage[12];
            tabBytesTailleOffset[3] = tabImage[13];
            this.tailleOffset = Convertir_Endian_To_Int(tabBytesTailleOffset);
            //Console.WriteLine("tailleOffset=" + tailleOffset);

            // Taille Header
            byte[] tabBytesTailleHeader = new Byte[4];
            tabBytesTailleHeader[0] = tabImage[14];
            tabBytesTailleHeader[1] = tabImage[15];
            tabBytesTailleHeader[2] = tabImage[16];
            tabBytesTailleHeader[3] = tabImage[17];
            this.tailleHeader = Convertir_Endian_To_Int(tabBytesTailleHeader);
            //Console.WriteLine("tailleHeader=" + tailleHeader);

            //Largeur de l'image
            byte[] tabBytesLargeurImage = new Byte[4];
            tabBytesLargeurImage[0] = tabImage[18];
            tabBytesLargeurImage[1] = tabImage[19];
            tabBytesLargeurImage[2] = tabImage[20];
            tabBytesLargeurImage[3] = tabImage[21];
            this.largeur = Convertir_Endian_To_Int(tabBytesLargeurImage);
            //Console.WriteLine("largeur=" + largeur);

            //Hauteur de l'image
            byte[] tabBytesHauteurImage = new Byte[4];
            tabBytesHauteurImage[0] = tabImage[22];
            tabBytesHauteurImage[1] = tabImage[23];
            tabBytesHauteurImage[2] = tabImage[24];
            tabBytesHauteurImage[3] = tabImage[25];
            this.hauteur = Convertir_Endian_To_Int(tabBytesHauteurImage);
            //Console.WriteLine("hauteur=" + hauteur);

            //Nombre de bits par couleur
            byte[] tabBytesNbBitsParCouleur = new Byte[2];
            tabBytesNbBitsParCouleur[0] = tabImage[28];
            tabBytesNbBitsParCouleur[1] = tabImage[29];
            this.nbBitsParCouleur = Convertir_Endian_To_Int(tabBytesNbBitsParCouleur);
            //Console.WriteLine("Nombre de bits par couleur=" + nbBitsParCouleur);

            //Taille de l'image
            byte[] tabBytesTailleImage = new Byte[4];
            tabBytesTailleImage[0] = tabImage[34];
            tabBytesTailleImage[1] = tabImage[35];
            tabBytesTailleImage[2] = tabImage[36];
            tabBytesTailleImage[3] = tabImage[37];
            this.tailleImage = Convertir_Endian_To_Int(tabBytesTailleImage);
            //Console.WriteLine("*** tailleImage=" + tailleImage);

            //Créer une matrice de pixels = une image
            // nombre de lignes = hauteur
            // nombre de colonnes = largeur
            this.image = new Pixel[hauteur, largeur];
            int i = tailleOffset;
            do
            {
                for (int j = 0; j < hauteur; j++)
                {
                    for (int k = 0; k < largeur; k++)
                    {
                        // Récupérer dans le fichier les 3 pixels Red puis Green puis Blue
                        image[j, k] = new Pixel(tabImage[i], tabImage[i + 1], tabImage[i + 2]);
                        i = i + 3;
                    }
                }
            } while (i < myfile.Length);
        }

        //Constructeur n°2
        //Création d'une image vide aux dimensions et couleurs indiquées en paramètre
        public MyImage(int largeur, int hauteur, int nbBitsParCouleur, byte red, byte green, byte blue)
         {
            // Valorisation des attributs
            this.typeImage = "BM";                      // on ne traite que les images en format Bitmap
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.nbBitsParCouleur = nbBitsParCouleur;
            this.tailleHeader = 40;                     // Taille Header d'info = 40
            this.tailleOffset = 54;                     // Taille Offset = 54 (14+40);
            this.tailleImage = largeur * hauteur * 3;   // 3 octets par pixel
            this.tailleFichier = this.TailleOffset+this.tailleImage;
            this.image = new Pixel[hauteur, largeur];

            // Chaque pixel est écrit dans l'ordre Red, Green, Blue
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(red, green, blue);
                }
            }
        }

        //Propriétés
        public string TypeImage
        {
            get { return typeImage; }
            set { value = typeImage; }
        }
        public int TailleFichier
        {
            get { return tailleFichier; }
            set { value = tailleFichier; }
        }
        public int TailleOffset
        {
            get { return tailleOffset; }
            set { value = tailleOffset; }
        }
        public int TailleHeader
        {
            get { return tailleHeader; }
            set { value = tailleHeader; }
        }
        public int Largeur
        {
            get { return largeur; }
            set { value = largeur; }
        }
        public int Hauteur
        {
            get { return hauteur; }
            set { value = hauteur; }
        }
        public int NbBitsParCouleur
        {
            get { return nbBitsParCouleur; }
            set { value = nbBitsParCouleur; }
        }
        public Pixel[,] Image
        {
            get { return image; }
            set { value = image; }
        }

        //Méthodes
        public void From_Image_To_File(string file)
        {
            byte[] outputFile = new byte[tailleFichier];

            //initialisation de toutes les lignes
            for (int value = 0; value < outputFile.Length; value++)
            {
                outputFile[value] = 0;
            }

            // HEADER

            // Type image
            if (typeImage == "BM")
            {
                outputFile[0] = 66;
                outputFile[1] = 77;
            }

            // Taille du fichier
            byte[] tabBytesTailleFichier = new Byte[4];
            tabBytesTailleFichier = Convertir_Int_To_Endian(tailleFichier, 4);
            outputFile[2] = tabBytesTailleFichier[0];
            outputFile[3] = tabBytesTailleFichier[1];
            outputFile[4] = tabBytesTailleFichier[2];
            outputFile[5] = tabBytesTailleFichier[3];

            // Taille Offset
            byte[] tabBytesTailleOffset = new Byte[4];
            tabBytesTailleOffset = Convertir_Int_To_Endian(tailleOffset, 4);
            outputFile[10] = tabBytesTailleOffset[0];
            outputFile[11] = tabBytesTailleOffset[1];
            outputFile[12] = tabBytesTailleOffset[2];
            outputFile[13] = tabBytesTailleOffset[3];

            // HEADER INFO

            // Taille du Header
            byte[] tabBytesHeader = new Byte[4];
            tabBytesHeader = Convertir_Int_To_Endian(tailleHeader, 4);
            outputFile[14] = tabBytesHeader[0];
            outputFile[15] = tabBytesHeader[1];
            outputFile[16] = tabBytesHeader[2];
            outputFile[17] = tabBytesHeader[3];

            // Largeur de l'image
            byte[] tabBytesLargeurImage = new Byte[4];
            tabBytesLargeurImage = Convertir_Int_To_Endian(largeur, 4);
            outputFile[18] = tabBytesLargeurImage[0];
            outputFile[19] = tabBytesLargeurImage[1];
            outputFile[20] = tabBytesLargeurImage[2];
            outputFile[21] = tabBytesLargeurImage[3];

            // Hauteur de l'image
            byte[] tabBytesHauteurImage = new Byte[4];
            tabBytesHauteurImage = Convertir_Int_To_Endian(hauteur, 4);
            outputFile[22] = tabBytesHauteurImage[0];
            outputFile[23] = tabBytesHauteurImage[1];
            outputFile[24] = tabBytesHauteurImage[2];
            outputFile[25] = tabBytesHauteurImage[3];

            // Nombre de Bits par couleur
            byte[] tabBytesNbBitsParCouleur = new Byte[2];
            tabBytesNbBitsParCouleur = Convertir_Int_To_Endian(nbBitsParCouleur, 2);
            outputFile[28] = tabBytesNbBitsParCouleur[0];
            outputFile[29] = tabBytesNbBitsParCouleur[1];

            // Taille de l'image
            byte[] tabBytesTailleImage = new Byte[4];
            tabBytesTailleImage = Convertir_Int_To_Endian(tailleImage, 4);
            outputFile[34] = tabBytesTailleImage[0];
            outputFile[35] = tabBytesTailleImage[1];
            outputFile[36] = tabBytesTailleImage[2];
            outputFile[37] = tabBytesTailleImage[3];

            // Tableau de l'image
            int i = tailleOffset;
            // Chaque pixel est écrit dans l'ordre Red, Green, Blue
            // On a donc une image de largeur multipliée par 3 au niveau du fichier
            for (int j = 0; j < hauteur; j++)
            {
                for (int k = 0; k < largeur; k++)
                {
                    //Console.WriteLine("From_Image_To_File() i=" + i + " j = " + j + " k= " + k + " Red = " + image[j, k].Red + " Green = " + image[j, k].Green + " Blue = " + image[j, k].Blue);
                    outputFile[i++] = image[j, k].Red;
                    outputFile[i++] = image[j, k].Green;
                    outputFile[i++] = image[j, k].Blue;
                }
            }
            File.WriteAllBytes(file, outputFile);
        }

        // (1er octet x 1) + (2ème octet x 16²) + (3ème octet x 16^4) + (4ème octet x 16^8)
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int entier = 0;
            double somme = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (i == 0)
                {
                    somme += tab[i];
                } else
                {
                    somme += tab[i] * Math.Pow(16, Math.Pow(2, i));
                }
            }
            entier = Convert.ToInt32(somme);

            return entier;
        }
        public byte[] Convertir_Int_To_Endian(int val, int nbOctets)
        {
            //Console.WriteLine("Entrée dans Convertir_Int_To_Endian() val="+ val + " nbOctets="+ nbOctets);
            double valdecimal = Convert.ToDouble(val);
            byte[] endian = null;
            double total = 0;

            if (nbOctets == 2)
            {
                endian = new byte[2];
                endian[1] = Convert.ToByte(Math.Truncate((valdecimal / (Math.Pow(16, 2)))));
                endian[0] = Convert.ToByte(val - (Math.Pow(16, 2)) * endian[1]);
                //Console.WriteLine("Sortie de Convertir_Int_To_Endian() endian[0]=" + endian[0] + " endian[1]=" + endian[1]);
            } else if (nbOctets == 4)
            {
                endian = new byte[4];
                endian[3] = Convert.ToByte(Math.Truncate((valdecimal / (Math.Pow(16, 8)))));
                total += endian[3] * Math.Pow(16, 8);

                endian[2] = Convert.ToByte(Math.Truncate((valdecimal - total) / Math.Pow(16, 4)));
                total += endian[2] * Math.Pow(16, 4);

                endian[1] = Convert.ToByte(Math.Truncate((valdecimal - total) / (Math.Pow(16, 2))));
                total += endian[1] * Math.Pow(16, 2);

                endian[0] = Convert.ToByte(val - total);
                //Console.WriteLine("Sortie de Convertir_Int_To_Endian() endian[0]=" + endian[0] + " endian[1]=" + endian[1] + " endian[2]=" + endian[2] + " endian[3]=" + endian[3]);
            }

            return endian;
        }

        public void Transforme_Image_En_Gris()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].Transforme_En_Gris(); // on applique la méthode de la classe pixel à tous les pixels de la matrice de pixels correspondant à l'image
                }
            }
        }

        public void Transforme_Image_En_Noir_Ou_Blanc()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].Transforme_En_Noir_Ou_Blanc();
                }
            }
        }

        // Agrandir une image
        // Ne pas oublier que l'on a 3 octects à écrire dans le fichier pour chaque pixel (Red + Green + Blue)
        public void Agrandir_Image(int agrandissement)
        {
            int hauteur_agrandie = hauteur * agrandissement;
            int largeur_agrandie = largeur * agrandissement;
            // Une image agrandie x 2 devient de taille 2 x 2
            // Une image agrandie x 3 devient de taille 3 x 3
            int tailleImage_agrandie = tailleImage * (agrandissement * agrandissement);
            Pixel[,] image_agrandie = new Pixel[hauteur_agrandie, largeur_agrandie];

            // i et j sont les coordonnées du tableau initial
            // i2 et j2 sont les coordonnées du tableau agrandi
            for (int i = (hauteur - 1); i >= 0; i--)
            {
                for (int j = 0; j < largeur; j++)
                {
                    for (int i2 = (i * agrandissement); i2 < ((i * agrandissement) + agrandissement); i2++)
                    {
                        for (int j2 = (j * agrandissement); j2 < ((j * agrandissement) + agrandissement); j2++)
                        {
                            image_agrandie[i2, j2] = new Pixel(image[i, j].Red, image[i, j].Green, image[i, j].Blue);
                        }
                    }
                }
            }

            image = new Pixel[hauteur_agrandie, largeur_agrandie];
            // Recopie de l'image agrandie dans l'ancienne image
            for (int i = 0; i < hauteur_agrandie; i++)
            {
                for (int j = 0; j < largeur_agrandie; j++)
                {
                    image[i, j] = new Pixel(image_agrandie[i, j].Red, image_agrandie[i, j].Green, image_agrandie[i, j].Blue);
                }
            }

            tailleFichier = tailleOffset + tailleImage_agrandie;
            tailleImage = tailleImage_agrandie;
            hauteur = hauteur_agrandie;
            largeur = largeur_agrandie;
        }

        // Retrecir une image
        // Ne garder que les pixels en haut à gauche de la zone n x n à rétrécir
        // Ne pas oublier que l'on a 3 octects à écrire dans le fichier pour chaque pixel (Red + Green + Blue)
        public void Retrecir_Image(int retrecissement)
        {
            int hauteur_retrecie = (int) (Math.Floor((decimal) (hauteur / retrecissement)));
            int largeur_retrecie = (int) (Math.Floor((decimal) (largeur / retrecissement)));
            // Une image rétrécie x 2 devient de taille 1/2 x 1/2
            // Une image rétrécie x 3 devient de taille 1/3 x 1/3
            int tailleImage_retrecie = tailleImage / (retrecissement * retrecissement);
            Pixel[,] image_retrecie = new Pixel[hauteur_retrecie, largeur_retrecie];

            // i et j sont les coordonnées du tableau initial
            // i2 et j2 sont les coordonnées du tableau rétréci
            for (int i = (hauteur - 1); i >= 0; i = i - retrecissement)
            {
                for (int j = 0; j < largeur; j = j + retrecissement)
                {
                    int i2 = (int)(Math.Floor((decimal)(i / retrecissement)));
                    int j2 = (int)(Math.Floor((decimal)(j / retrecissement)));
                    image_retrecie[i2, j2] = new Pixel(image[i, j].Red, image[i, j].Green, image[i, j].Blue);
                }
            }

            image = new Pixel[hauteur_retrecie, largeur_retrecie];
            // Recopie de l'image retrecie dans l'ancienne image
            for (int i = 0; i < hauteur_retrecie; i++)
            {
                for (int j = 0; j < largeur_retrecie; j++)
                {
                    image[i, j] = new Pixel(image_retrecie[i, j].Red, image_retrecie[i, j].Green, image_retrecie[i, j].Blue);
                }
            }

            tailleFichier = tailleOffset + tailleImage_retrecie;
            tailleImage = tailleImage_retrecie;
            hauteur = hauteur_retrecie;
            largeur = largeur_retrecie;
        }

        // Clone une image
        public Pixel[,] Clone_Image()
        {
            Pixel[,] clone_image = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    clone_image[i, j] = image[i, j];
                }
            }
            return clone_image;
        }

        // Fait pivoter une image de 180 degrés
        public void Rotation_180()
        {
            Pixel[,] clone_image = Clone_Image();
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = clone_image[hauteur - i - 1, largeur - j - 1];
                }
            }
        }

        // Fait pivoter une image de 90 degrés
        public void Rotation_90()
        {
            Pixel[,] clone_image = Clone_Image();
            image = new Pixel[largeur, hauteur];
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    image[i, j] = clone_image[j, largeur - i - 1];
                }
            }
            int ancienne_hauteur = hauteur;
            hauteur = largeur;
            largeur = ancienne_hauteur;
        }

        // Fait pivoter une image de 270 degrés
        public void Rotation_270()
        {
            Pixel[,] clone_image = Clone_Image();
            image = new Pixel[largeur, hauteur];
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    image[i, j] = clone_image[hauteur - j - 1, i];
                }
            }
            int ancienne_hauteur = hauteur;
            hauteur = largeur;
            largeur = ancienne_hauteur;
        }

        // Effet miroir
        public void Miroir()
        {
            Pixel[,] clone_image = Clone_Image();
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = clone_image[i, largeur - j - 1];
                }
            }
        }

        // Matrice de convolution
        public int[,] Matrice_De_Convolution(int nombre, int[,] noyau)
        {
            int[,] mat_convolution = new int[hauteur, largeur];
            int[,] pixelRGB = new int[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (nombre == 1)
                        pixelRGB[i, j] = image[i, j].Red; //on crée la matrice des pixels rouges
                    if (nombre == 2)
                        pixelRGB[i, j] = image[i, j].Green; //on crée la matrice des pixels verts
                    if (nombre == 3)
                        pixelRGB[i, j] = image[i, j].Blue; //on crée la matrice des pixels bleus
                }
            }
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (i - 1 < 0)  //si on est sur les lignes avant la première ligne
                    {
                        int it = hauteur - 1;
                        if (j - 1 < 0) //si on est sur les colonnes avant la première colonne
                        {
                            int jt = largeur - 1;
                            mat_convolution[i, j] = pixelRGB[it, jt] * noyau[0, 0]
                                                     + pixelRGB[it, j] * noyau[0, 1]
                                                     + pixelRGB[it, j + 1] * noyau[0, 2]
                                                      + pixelRGB[i, jt] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, j + 1] * noyau[1, 2]
                                                      + pixelRGB[i + 1, jt] * noyau[2, 0]
                                                      + pixelRGB[i + 1, j] * noyau[2, 1]
                                                      + pixelRGB[i + 1, j + 1] * noyau[2, 2];

                        }
                        if (j + 1 >= largeur) //si on est sur les colonnes après la dernière colonne
                        {
                            int jt = 0;
                            mat_convolution[i, j] = pixelRGB[it, j - 1] * noyau[0, 0]
                                                     + pixelRGB[it, j] * noyau[0, 1]
                                                     + pixelRGB[it, jt] * noyau[0, 2]
                                                      + pixelRGB[i, j - 1] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, jt] * noyau[1, 2]
                                                      + pixelRGB[i + 1, j - 1] * noyau[2, 0]
                                                      + pixelRGB[i + 1, j] * noyau[2, 1]
                                                      + pixelRGB[i + 1, jt] * noyau[2, 2];

                        }
                        if (j-1>=0 && j+1<largeur)
                        {
                            mat_convolution[i, j] = pixelRGB[it, j - 1] * noyau[0, 0]
                                                     + pixelRGB[it, j] * noyau[0, 1]
                                                     + pixelRGB[it, j + 1] * noyau[0, 2]
                                                      + pixelRGB[i, j - 1] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, j + 1] * noyau[1, 2]
                                                      + pixelRGB[i + 1, j - 1] * noyau[2, 0]
                                                      + pixelRGB[i + 1, j] * noyau[2, 1]
                                                      + pixelRGB[i + 1, j + 1] * noyau[2, 2];
                        }
                    }
                    if (i + 1 >= hauteur)  //si on est sur les lignes après la dernière ligne
                    {
                        int it = 0;
                        if (j - 1 < 0) //si on est sur les colonnes avant la première colonne
                        {
                            int jt = largeur - 1;
                            mat_convolution[i, j] = pixelRGB[i - 1, jt] * noyau[0, 0]
                                                     + pixelRGB[i - 1, j] * noyau[0, 1]
                                                     + pixelRGB[i - 1, j + 1] * noyau[0, 2]
                                                      + pixelRGB[i, jt] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, j + 1] * noyau[1, 2]
                                                      + pixelRGB[it, jt] * noyau[2, 0]
                                                      + pixelRGB[it, j] * noyau[2, 1]
                                                      + pixelRGB[it, j + 1] * noyau[2, 2];

                        }
                        if (j + 1 >= largeur) //si on est sur les colonnes après la dernière colonne
                        {
                            int jt = 0;
                            mat_convolution[i, j] = pixelRGB[i - 1, j - 1] * noyau[0, 0]
                                                     + pixelRGB[i - 1, j] * noyau[0, 1]
                                                     + pixelRGB[i - 1, jt] * noyau[0, 2]
                                                      + pixelRGB[i, j - 1] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, jt] * noyau[1, 2]
                                                      + pixelRGB[it, j - 1] * noyau[2, 0]
                                                      + pixelRGB[it, j] * noyau[2, 1]
                                                      + pixelRGB[it, jt] * noyau[2, 2];

                        }
                        if (j-1 >=0 && j+1<largeur)
                        {
                            mat_convolution[i, j] = pixelRGB[i - 1, j - 1] * noyau[0, 0]
                                                     + pixelRGB[i - 1, j] * noyau[0, 1]
                                                     + pixelRGB[i - 1, j + 1] * noyau[0, 2]
                                                      + pixelRGB[i, j - 1] * noyau[1, 0]
                                                      + pixelRGB[i, j] * noyau[1, 1]
                                                      + pixelRGB[i, j + 1] * noyau[1, 2]
                                                      + pixelRGB[it, j - 1] * noyau[2, 0]
                                                      + pixelRGB[it, j] * noyau[2, 1]
                                                      + pixelRGB[it, j + 1] * noyau[2, 2];
                        }
                    }
                    if (j - 1 < 0 && i-1 >=0 && i+1 < hauteur) //si on est sur les colonnes avant la première colonne
                    {
                        int jt = largeur - 1;
                        mat_convolution[i, j] = pixelRGB[i - 1, jt] * noyau[0, 0]
                                                 + pixelRGB[i - 1, j] * noyau[0, 1]
                                                 + pixelRGB[i - 1, j + 1] * noyau[0, 2]
                                                  + pixelRGB[i, jt] * noyau[1, 0]
                                                  + pixelRGB[i, j] * noyau[1, 1]
                                                  + pixelRGB[i, j + 1] * noyau[1, 2]
                                                  + pixelRGB[i + 1, jt] * noyau[2, 0]
                                                  + pixelRGB[i + 1, j] * noyau[2, 1]
                                                  + pixelRGB[i + 1, j + 1] * noyau[2, 2];
                    }
                    if (j + 1 >= largeur && i-1 >= 0 && i+1 < hauteur) //si on est sur les colonnes après la dernière colonne
                    {
                        int jt = 0;
                        mat_convolution[i, j] = pixelRGB[i - 1, j - 1] * noyau[0, 0]
                                                 + pixelRGB[i - 1, j] * noyau[0, 1]
                                                 + pixelRGB[i - 1, jt] * noyau[0, 2]
                                                  + pixelRGB[i, j - 1] * noyau[1, 0]
                                                  + pixelRGB[i, j] * noyau[1, 1]
                                                  + pixelRGB[i, jt] * noyau[1, 2]
                                                  + pixelRGB[i + 1, j - 1] * noyau[2, 0]
                                                  + pixelRGB[i + 1, j] * noyau[2, 1]
                                                  + pixelRGB[i + 1, jt] * noyau[2, 2];
                    }
                    if (i-1>=0 && j-1>=0 && i+1<hauteur && j+1<largeur) 
                    {
                        mat_convolution[i, j] = pixelRGB[i - 1, j - 1] * noyau[0, 0]
                                                 + pixelRGB[i - 1, j] * noyau[0, 1]
                                                 + pixelRGB[i - 1, j + 1] * noyau[0, 2]
                                                  + pixelRGB[i, j - 1] * noyau[1, 0]
                                                  + pixelRGB[i, j] * noyau[1, 1]
                                                  + pixelRGB[i, j + 1] * noyau[1, 2]
                                                  + pixelRGB[i + 1, j - 1] * noyau[2, 0]
                                                  + pixelRGB[i + 1, j] * noyau[2, 1]
                                                  + pixelRGB[i + 1, j + 1] * noyau[2, 2];
                    }
                    //Console.WriteLine("ligne : "+i+" colonne : " +j+ " résulat du filtre: "+  mat_convolution[i, j]);
                }
            }
            return mat_convolution;
        }

        // Détection des contours
        public void Contours()
        {
            int[,] noyau = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            int[,] mat_convolution_red = Matrice_De_Convolution(1, noyau);
            int[,] mat_convolution_green = Matrice_De_Convolution(2, noyau);
            int[,] mat_convolution_blue = Matrice_De_Convolution(3, noyau);
            for (int i=0; i<hauteur;i++)
            {
                for (int j=0; j<largeur;j++)
                {
                    image[i, j].Red = (byte)mat_convolution_red[i, j];
                    image[i, j].Green = (byte)mat_convolution_green[i, j];
                    image[i, j].Blue = (byte)mat_convolution_blue[i, j];
                }
            }
        }

        // Renforcement des bords
        public void Renforcement_Bords()
        {
            int[,] noyau = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            int[,] mat_convolution_red = Matrice_De_Convolution(1, noyau);
            int[,] mat_convolution_green = Matrice_De_Convolution(2, noyau);
            int[,] mat_convolution_blue = Matrice_De_Convolution(3, noyau);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j].Red = (byte)mat_convolution_red[i, j];
                    image[i, j].Green = (byte)mat_convolution_green[i, j];
                    image[i, j].Blue = (byte)mat_convolution_blue[i, j];
                }
            }
        }

        // Flou
        public void Flou()
        {
            int[,] noyau = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] mat_convolution_red = Matrice_De_Convolution(1, noyau);
            int[,] mat_convolution_green = Matrice_De_Convolution(2, noyau);
            int[,] mat_convolution_blue = Matrice_De_Convolution(3, noyau);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                     image[i, j].Red = (byte)(mat_convolution_red[i, j]/9);
                     image[i, j].Green = (byte)(mat_convolution_green[i, j]/9);
                     image[i, j].Blue = (byte)(mat_convolution_blue[i, j]/9);
                }
            }
        }

        // Repoussage
        public void Repoussage()
        {
            int[,] noyau = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            int[,] mat_convolution_red = Matrice_De_Convolution(1, noyau);
            int[,] mat_convolution_green = Matrice_De_Convolution(2, noyau);
            int[,] mat_convolution_blue = Matrice_De_Convolution(3, noyau);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j].Red = (byte)mat_convolution_red[i, j];
                    image[i, j].Green = (byte)mat_convolution_green[i, j];
                    image[i, j].Blue = (byte)mat_convolution_blue[i, j];
                }
            }
        }

        //Fractale
        public void Fractale(int largeur, int hauteur)
        {
            double x1= -2.1; //axe des abscisses compris entre -2,1 et 0,6
            double x2 = 0.6;
            double y1 = -1.2; //axe des ordonnées compris entre -1,2 et 1,2 
            double y2 = 1.2;
            int iteration_max = 50;

            double zoom_x = largeur / (x2 - x1);
            double zoom_y = hauteur / (y2 - y1);

            for (int i=0; i<largeur; i++)
            {
                for (int j=0; j<hauteur;j++)
                {
                    double c_r = i / zoom_x + x1;
                    double c_i = j / zoom_y + y1;
                    double z_r = 0;
                    double z_i = 0;
                    int k = 0;

                    while(z_r*z_r + z_i*z_i <4 && k<iteration_max) //le module de z ne dépasse pas 2 
                    {
                        double tmp = z_r;
                        z_r = z_r * z_r - z_i * z_i + c_r;
                        z_i = 2 * z_i * tmp + c_i;
                        k = k + 1;
                    }
                    if (k==iteration_max) //z fait partie de la fractale car son module n'a pas dépassé 2 après les 50 itérations
                    {
                        image[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        image[i, j] = new Pixel(255, 255, 255);
                    }


                }
            }
        }

        //Histogrammes d'une image 
        //Je considère l'histogramme comme une matrice dont la 1ère colonne correspond à l'intensité du pixel et la 2ème colonne au nombre de pixels atteignant chaque intensité
        public int[,] Histogramme(string couleur)
        {
            int[,] histogramme = new int[256, 2];
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0) histogramme[i, j] = i;
                    else
                    {
                        histogramme[i, j] = 0;
                    }
                }
            }
            for (int k = 0; k < 256; k++)
            {
                for (int a = 0; a < hauteur; a++)
                {
                    for (int b = 0; b < largeur; b++)
                    {
                        if (couleur == "red")
                        {
                            if (image[a, b].Red == k) histogramme[k, 1] += 1;
                        }
                        if (couleur == "green")
                        {
                            if (image[a, b].Green == k) histogramme[k, 1] += 1;
                        }
                        if (couleur == "blue")
                        {
                            if (image[a, b].Blue == k) histogramme[k, 1] += 1;
                        }
                    }
                }
            }
            return histogramme;
        }

        //Méthode pour "cacher" une image dans une image: faire apparaître 1 pixel sur 2
        public void CacherImage(MyImage image2)
        {
            //On demande à l'utilisateur les coordonnées de l'image à cacher dans la grande image
            int lignepossible = hauteur - image2.hauteur;
            int colonnepossible = largeur - image2.largeur;
            Console.WriteLine("Donner un numéro de ligne compris entre 0 et " + lignepossible);
            int lignedecommencement = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Donner un numéro de colonne compris entre 0 et " + colonnepossible);
            int colonnedecommencement = Convert.ToInt32(Console.ReadLine());
            for (int i=lignedecommencement; i<(image2.hauteur+lignedecommencement-1);i+=2)
            {
                for (int j=colonnedecommencement; j<(image2.largeur+colonnedecommencement-1);j+=2)
                {
                    image[i, j] = image2.image[i-lignedecommencement,j-colonnedecommencement];
                }
            }
        }

        //Méthode pour cacher une image dans une image en utilisant la stéganographie
        //Cette méthode permet de dissimuler beaucoup mieux l'image que la méthode CacherImage(MyImage image2)
        public void DissimulerImage(MyImage image2)
        {
            //On demande à l'utilisateur les coordonnées de l'image à cacher dans la grande image
            int lignepossible = hauteur - image2.hauteur;
            int colonnepossible = largeur - image2.largeur;
            Console.WriteLine("Donner un numéro de ligne compris entre 0 et " + lignepossible);
            int lignedecommencement = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Donner un numéro de colonne compris entre 0 et " + colonnepossible);
            int colonnedecommencement = Convert.ToInt32(Console.ReadLine());
            for (int i = lignedecommencement; i < (image2.hauteur+lignedecommencement-1); i++)
            {
                for (int j = colonnedecommencement; j < (image2.largeur+colonnedecommencement-1); j++)
                {
                    image[i,j] = image[i,j].Mixer2Pixels(image2.image[i-lignedecommencement,j-colonnedecommencement]);
                }
            }
        }

        //Méthode pour retrouver une image cachée dans une autre image
        //Cette méthode ne fonctionne pas...
        public MyImage TrouverImage(MyImage imageadecoder)
        {
            MyImage imagecachee = new MyImage(0,0,4,0,0,0);
            int lignedecommencement = 0;
            int colonnedecommencement = 0;
            for (int i=0; i<hauteur;i++)
            {
                for (int j=0;j<largeur;j++)
                {
                    if (image[i,j]!=imageadecoder.image[i,j])
                    {
                        lignedecommencement = i;
                        colonnedecommencement = j;
                        break;
                    }
                }
            }
            for (int i=lignedecommencement;i<hauteur+lignedecommencement;i++)
            {
                for (int j=colonnedecommencement; j<largeur+colonnedecommencement;j++)
                {
                    image[i,j]=image[i, j].RetrouverPixelReference();
                    imagecachee.image[i - lignedecommencement, j - colonnedecommencement]=imagecachee.image[i - lignedecommencement, j - colonnedecommencement].RetrouverPixelImageCachee();
                }

            }
            return imagecachee;
        }

        //Ma création : inverser les couleurs d'une image
        public void Couleurs_Inverses()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j].Pixel_inverse();
                }
            }
        }

    }



}
