# Traitement-images

*Image processing*

**Date de réalisation :** Avril 2019

**Cadre du projet :** Cours "Problème scientifique informatique" en 2ème année de prépa intégrée à l'ESILV

**Langage utilisé :** C#

Il s’agit d’un projet informatique réalisé en C# sur le sujet du traitement d’images, au format Bitmap 24 bits.
J’ai créé un outil permettant de lire une image, de la traiter et de la sauvegarder dans un fichier de sortie différent de celui donné en entrée.

Les différentes fonctions disponibles permettent de :

-	traiter une image : passage d’une photo couleur à une photo en nuances de gris et en noir et blanc, agrandir/rétrécir une image, rotation (90, 180 ou 270°), effet miroir

<p align="center">
  <img src="./Images/lac_en_montagne_En_Noir_Ou_Blanc.bmp" height="160">
  <img src="./Images/lac_en_montagne_En_Gris.bmp" height="160">
  <img src="./Images/lac_en_montagne_Miroir.bmp" height="160">
  <img src="./Images/lac_en_montagne_Rotation_180.bmp" height="160">
</p>

-	appliquer un filtre (à l’aide d’une matrice de convolution) : détection de contour, renforcement des bords, flou, repoussage

<p align="center">
  <img src="./Images/lac_en_montagne_Contours.bmp" height="150">
  <img src="./Images/lac_en_montagne_Renforcement_Bords.bmp" height="150">
  <img src="./Images/lac_en_montagne_Flou.bmp" height="150">
  <img src="./Images/lac_en_montagne_Repoussage.bmp" height="150">
</p>

-	créer une nouvelle image : fractale, histogramme se rapportant à l’image

<p align="center">
  <img src="./Images/Fractale.bmp" height="160">
</p>

-	coder et décoder une image dans une image

<p align="center">
  <img src="./Images/ImprovedLogo.bmp" height="30"> <b>+</b>
  <img src="./Images/lac_en_montagne.bmp" height="160"> <b>=</b>
  <img src="./Images/lac+logo.bmp" height="160">
</p>

-	inverser les couleurs de l’image

<p align="center">
  <img src="./Images/coco.bmp" height="160">
  <img src="./Images/coco_couleurs_inverses.bmp" height="160">
</p>
