using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace Problème
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage myImage = null;
            Console.WriteLine("Bonjour !" + "\nQuelle image voulez-vous modifier ?" + "\n1) Lac en montagne (Taper 1)" + "\n2) Coco (Taper 2)" + "\n3) Lena (Taper 3)" + "\n4) ImprovedLogo (Taper 4)"+"\n5) Fractale (Taper 5)");
            int choiximage = Convert.ToInt32(Console.ReadLine());

            switch (choiximage)
            {
                case 1:
                    myImage = new MyImage("lac_en_montagne.bmp");
                    break;
                case 2:
                    myImage = new MyImage("coco.bmp");
                    break;
                case 3:
                    myImage = new MyImage("lena.bmp");
                    break;
                case 4:
                    myImage = new MyImage("ImprovedLogo.bmp");
                    break;
                case 5:
                    MyImage fractale = null;
                    int largeurFractale = 100;
                    int hauteurFractale = 100;
                    fractale = new MyImage(largeurFractale, hauteurFractale, 24, 255, 255, 255);
                    fractale.Fractale(largeurFractale, hauteurFractale);
                    fractale.From_Image_To_File("Fractale.bmp");
                    myImage = new MyImage("Fractale.bmp");
                    break;
            }

            Console.Clear();

            int choix = 0;
            Console.WriteLine("Quelle modification voulez-vous effectuer ?" + "\n1) Traitement d'image (agrandissement, rotations...) (Taper 1)" + "\n2) Application d'un filtre (Taper 2)" + "\n3) Coder ou décoder une image dans mon image (Taper 3)"+"\n4) Afficher l'histogramme (Taper 4)");
            int choix2 = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            if (choix2==1)
            {
                Console.WriteLine("Quel traitement d'image voulez-vous effectuer ?"+"\n1) Agrandissement (Taper 1)"+"\n2) Rétrécissement (Taper 2)"+"\n3) Rotation à 90° (Taper 3)"+ "\n4) Rotation à 180° (Taper 4)"+ "\n5) Rotation à 270° (Taper 5)"+"\n6) Effet miroir (Taper 6)"+"\n7) Passage à une photo en nuances de gris (Taper 7)"+"\n8) Passage à une photo en noir ou blanc (Taper 8)"+"\n9) Inversion des couleurs (Taper 9)");
                int choix3 = Convert.ToInt32(Console.ReadLine());
                choix = choix3;
            }
            if (choix2==2)
            {
                Console.WriteLine("Quel filtre voulez-vous appliquer ?"+"\n1) Détection de contours (Taper 1)"+"\n2) Renforcement des bords (Taper 2)"+"\n3) Flou (Taper 3)"+"\n4) Repoussage (Taper 4)");
                int choix3 = Convert.ToInt32(Console.ReadLine());
                choix = choix3 + 9;
            }
            if (choix2==3)
            {
                Console.WriteLine("Voulez vous cacher une image dans votre image (Taper 1) ou retrouver une image dans votre image (Taper 2) ?");
                int choixcodage = Convert.ToInt32(Console.ReadLine());
                if (choixcodage==1) choix = 14;
                if (choixcodage == 2) choix = 15;
            }
            if (choix2==4)
            {
                choix = 16;
            }

            switch (choix)
            {
                case 1:
                    Console.WriteLine("Donner le coefficient d'agrandissement : ");
                    int coef = Convert.ToInt32(Console.ReadLine());
                    myImage.Agrandir_Image(coef);
                    break;
                case 2:
                    Console.WriteLine("Donner le coefficient de rétrécissement : ");
                    int coeff = Convert.ToInt32(Console.ReadLine());
                    myImage.Retrecir_Image(coeff);
                    break;
                case 3:
                    myImage.Rotation_90();
                    break;
                case 4:
                    myImage.Rotation_180();
                    break;
                case 5:
                    myImage.Rotation_270();
                    break;
                case 6:
                    myImage.Miroir();
                    break;
                case 7:
                    myImage.Transforme_Image_En_Gris();
                    break;
                case 8:
                    myImage.Transforme_Image_En_Noir_Ou_Blanc();
                    break;
                case 9:
                    myImage.Couleurs_Inverses();
                    break;
                case 10:
                    myImage.Contours();
                    break;
                case 11:
                    myImage.Renforcement_Bords();
                    break;
                case 12:
                    myImage.Flou();
                    break;
                case 13:
                    myImage.Repoussage();
                    break;
                case 14:
                    Console.WriteLine("Quelle image voulez-vous cacher ?" + "\n1) Lac en montagne (Taper 1)" + "\n2) Coco (Taper 2)" + "\n3) Lena (Taper 3)" + "\n4) ImprovedLogo (Taper 4)" + "\n5) Fractale (Taper 5)");
                    int choiximageacacher = Convert.ToInt32(Console.ReadLine());
                    MyImage imageacacher = null;
                    switch (choiximageacacher)
                    {
                        case 1:
                            imageacacher = new MyImage("lac_en_montagne.bmp");
                            break;
                        case 2:
                            imageacacher = new MyImage("coco.bmp");
                            break;
                        case 3:
                            imageacacher = new MyImage("lena.bmp");
                            break;
                        case 4:
                            imageacacher = new MyImage("ImprovedLogo.bmp");
                            break;
                        case 5:
                            MyImage fractale = null;
                            int largeurFractale = 100;
                            int hauteurFractale = 100;
                            fractale = new MyImage(largeurFractale, hauteurFractale, 24, 255, 255, 255);
                            fractale.Fractale(largeurFractale, hauteurFractale);
                            fractale.From_Image_To_File("Fractale.bmp");
                            imageacacher = new MyImage("Fractale.bmp");
                            break;
                    }

                    myImage.DissimulerImage(imageacacher);
                    break;
                case 15:
                    MyImage imageadecoder = new MyImage("lac+logo.bmp");
                    myImage.TrouverImage(imageadecoder);
                    imageadecoder.From_Image_To_File("imageretrouvee.bmp");
                    break;
                case 16:
                    int[,] histo = myImage.Histogramme("red");
                    Console.Write("Valeur du pixel rouge (entre 0 et 255)" + " | ");
                    Console.WriteLine("Nombre de pixels rouges prenant cette valeur");
                    for (int i = 0; i < 256; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Console.Write("\t \t \t " + histo[i, j] + " \t \t ");
                        }
                        Console.WriteLine();
                    }
                    break;
            }

            myImage.From_Image_To_File("imagedesortie.bmp");
            Process.Start("imagedesortie.bmp");
            
            /*
            Process.Start("Test.bmp");
            Process.Start("Sortie.bmp");
            */
            Console.ReadLine();
        }

        static void DisplayFileImage(string file)
        {
            //Console.WriteLine("ENTREE DisplayFileImage() file="+file);
            byte[] myfile = File.ReadAllBytes(file);
             Console.WriteLine("Header \n");
             for (int i = 0; i < 14; i++)
                 Console.Write(myfile[i] + " ");
             Console.WriteLine("\n HEADER INFO \n\n");
             for (int i = 14; i < 54; i++)
                 Console.Write(myfile[i] + " ");

            Console.WriteLine("\nTaille image 34=" + myfile[34] + " 35 = " + myfile[35] + " 36 = " + myfile[36] + " 37 = " + myfile[37]);

            Console.ReadLine();
            Console.WriteLine("\n\n IMAGE \n");
             for (int i = 54; i < myfile.Length; i++)
             {
                 Console.Write("i="+i + " " + myfile[i] + "\t");
             }

            //Console.WriteLine("SORTIE DisplayFileImage()");
        }
    }
}