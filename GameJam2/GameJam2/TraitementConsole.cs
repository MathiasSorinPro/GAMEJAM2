using System;
using System.IO;
using System.Collections.Generic;
using System.Media;
using System.Threading;

namespace GameJam2
{

	public static class TraitementConsole
	{


        /// <summary>
        /// Note de musique avec notation anglaise. Les # sont representes par sh.
        /// </summary>
        public enum Note {
			C, Csh , D , Dsh, 
			E, F, Fsh , G , 
			Gsh , A , Ash , B 
		}
		/// <summary>
		/// Frequences liees aux notes (tableau base sur la valeur de l'enum).  Pour utilisation interne seulement, pas important pour vous
		/// </summary>
		private static double[] frequences = {16.35,17.32,18.35, 19.45, 
		20.60, 21.83, 23.12, 24.5, 
			25.96, 27.5,  29.14, 30.87};
	
		/// <summary>
		/// Change les couleurs de surlignage et du texte dans la console.  Les deux variables sont de type ConsoleColor, 
		/// donc faites ConsoleColor.x (ou x est la couleur voulue).
		/// </summary>
		/// <param name="background">Background.</param>
		/// <param name="foreground">Foreground.</param>
		/// <param name="nettoieConsole">If set to <c>true</c> nettoie console.</param>
		public static void ChangeCouleur(ConsoleColor background, ConsoleColor foreground, bool nettoieConsole){
			if(nettoieConsole) NettoieConsole( );
			Console.BackgroundColor = background;
			Console.ForegroundColor = foreground;
		}

		/// <summary>
		/// Joue une note de  musique a travers le PC speaker.
		/// </summary>
		/// <param name="note">Note (Note.x).</param>
		/// <param name="octave">Octave.</param>
		/// <param name="dureeEnMilliSeconde">Duree en milli seconde.</param>
		public static void JoueNote(Note note, int octave, int dureeEnMilliSeconde){
			
			int frequence = (int) (frequences[(int)note] * octave);
            
			Console.Beep(frequence, dureeEnMilliSeconde );
		}


		public static void JoueSon(string fichier){
			string sonAJouer= "..\\..\\Fichiers\\Sons\\" + fichier;
			SoundPlayer sp = new SoundPlayer(sonAJouer);

			sp.Play();

		}
		/// <summary>
		/// Nettoie la console.
		/// </summary>
		public static void NettoieConsole( ){
			Console.Clear();
		}

		/// <summary>
		/// Recupere un entier entre au clavier. Si le resultat n'est pas un entier, il retourne -1.
		/// </summary>
		/// <returns>The entier entree.</returns>
		/// <param name="message">Message a afficher avant de recuperer l'entree.  Si "", n'affiche rien</param>
		public static int RecupereEntierEntree(string message){
			if(message != "") AfficheTexte(message);
			string temp = Console.ReadLine();
			int resultat;
			if(int.TryParse(temp,out resultat)) return resultat;
			else return -1;
		}

		/// <summary>
		/// Affiche le texte mis en argument
		/// </summary>
		/// <param name="message">Message.</param>
		public static void AfficheTexte(string message){
			Console.WriteLine(message);
		}


		/// <summary>
		/// Permet d'afficher un menu et retourne la valeur entree.  
		/// </summary>
		/// <param name="message">Message d'acceuil du menu</param>
		/// <param name="choix">Choix, soit en tableau de string ou en strings individuels</param>
		public static int Menu (string message, params string[] choix){
			Console.WriteLine(message);
			Console.WriteLine("(Tapez le chiffre correspondant a votre choix)\n");
			int valeurEntree = -1;

			while(valeurEntree ==-1){
				for(int i = 0; i< choix.Length ; i++){
					Console.WriteLine("{0} - {1} \n", i + 1,choix[i]);
				}
				valeurEntree = RecupereEntierEntree("");
				if(valeurEntree==-1){
					Console.WriteLine("Ce choix est non-valide!");
					Console.Clear();
				}
			}

			return valeurEntree;
		}
		/// <summary>
		/// Similaire a la fonction Menu, mais sans les chiffres 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="choix"></param>
		/// <returns></returns>
        public static string MenuChoixTexte(string message, params string[] choix)
        {
            Console.WriteLine(message);
            
            string valeurEntree = "";

            while (valeurEntree == "")
            {
                for (int i = 0; i < choix.Length; i++)
                {
                    Console.WriteLine("{0}\n", choix[i]);
                }
                valeurEntree = Console.ReadLine().ToLower();
                if (valeurEntree == "") Console.WriteLine("Veuillez ecrire votre choix svp...");
            }

            return valeurEntree;
        }
		/// <summary>
		/// Affiche un fichier ascii art, il doit etre place dans le repertoire AsciiArt.
		/// </summary>
		/// <param name="fichier">Fichier (nom +extension , dans AsciiArt)</param>
		public static void AfficheAsciiArtFile(string fichier){
			string path = "..\\..\\Fichiers\\AsciiArt\\" +fichier;
			using (StreamReader reader = new StreamReader(path)){
				string ligne;
				while ((ligne = reader.ReadLine()) !=null){
					
					Console.WriteLine(ligne);
				}

			}

		}
        /// <summary>
        /// Compare deux string et return vrai si les deux string sont identiques
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CompareString (string a, string b)
        {
            if (a.Length == b.Length && a.ToLower().Contains(b.ToLower()))
            {
                return true;
            }
            return false;
        }
		/// <summary>
		/// Lire un fichier texte (dans Fichiers).
		/// </summary>
		/// <returns>The fichier.</returns>
		/// <param name="fichier">Fichier.</param>
		public static string[] LireFichier(string fichier){
			
			string path = "..\\..\\Fichiers\\Textes\\" +fichier;
			//Console.WriteLine(path);
			List<string> lignes = new List<string>();
			using (StreamReader reader = new StreamReader(path)){
				string ligne;
				while ((ligne = reader.ReadLine()) !=null){

					lignes.Add(ligne);
				}

			}
			return lignes.ToArray();
		}
		/// <summary>
		/// Afficher un tableau de strings mis en parametre.  Se combine bien avec les menus.
		/// </summary>
		/// <param name="aAfficher">A afficher.</param>
		public static void AfficheTableauString(string[] aAfficher){
			foreach(string a in aAfficher){
				Console.WriteLine(a);
			}
		}
        /// <summary>
        /// Attend un certain temps en marquant la progression en *
        /// </summary>
        /// <param name="duration">Duree en millisecondes</param>
        /// <param name="delayBetweenStars">Delai entre 2 etoiles en millisecondes</param>
        public static void AttendreEtAfficherEtoiles(int duree, int delaiEntreEtoiles) {

            /*int tempsDepart = 0;

            while(tempsDepart < duree) {
                Console.Write("*");
                Thread.Sleep(delaiEntreEtoiles);
                tempsDepart += delaiEntreEtoiles;
            }*/
            AttendreEtAfficherCaractere(duree, delaiEntreEtoiles, '*');
        }
        /// <summary>
        /// Attend un certain temps en marquant la progression en *
        /// </summary>
        /// <param name="duration">Duree en millisecondes</param>
        /// <param name="delayBetweenStars">Delai entre 2 etoiles en millisecondes</param>
        public static void AttendreEtAfficherCaractere(int duree, int delaiEntreEtoiles,char caractere)
        {

            int tempsDepart = 0;

            while (tempsDepart < duree)
            {
                Console.Write(caractere);
                Thread.Sleep(delaiEntreEtoiles);
                tempsDepart += delaiEntreEtoiles;
            }
        }
        /// <summary>
        /// Affiche 3 petits points over and over again
        /// </summary>
        /// <param name="duree">duree de l'affichage en millisecondes</param>
        public static void AttenteFBStyle(int duree) {
            int tempsDepart = 0;
            int qte = 0;
           /* int left = Console.CursorLeft;
            int top = Console.CursorTop;*/
            while (tempsDepart < duree) {
                Console.Write(".");
                
                tempsDepart += 300;
                
                if (qte >= 3) {
                    //Console.Write("Patate");
                    Console.Write("\b\b\b   \b\b\b");
                    /*Console.CursorTop = top;
                    Console.CursorLeft = left;*/
                    Thread.Sleep(100);
                    tempsDepart += 100;
                    qte = 0;
                }
                qte++;
                Thread.Sleep(300);
            }
            Console.Write("\b\b\b   \b\b\b");
        }
        /// <summary>
        /// Affiche un string avec un delai entre l'affichage de chaque lettre
        /// </summary>
        /// <param name="aAfficher">Texte a afficher</param>
        /// <param name="delaiEntreLettres">Delai en millisecondes entre chaque lettre</param>
        public static void AfficherStringAvecDelai(string aAfficher, int delaiEntreLettres) {
            for(int i = 0; i < aAfficher.Length; i++) {
                Console.Write(aAfficher[i]);
                Thread.Sleep(delaiEntreLettres);
            }
            Console.WriteLine("\n");
        }

		public static void ChangerGrosseurConsole(int largeur, int hauteur )
        {
			hauteur = hauteur > Console.LargestWindowHeight ? Console.LargestWindowHeight : hauteur;
			largeur = largeur > Console.LargestWindowWidth ? Console.LargestWindowWidth : largeur;
			Console.SetWindowSize(largeur, hauteur);

		}	

		public static void ChangeConsoleFullScreen()
        {

			ChangerGrosseurConsole(Console.LargestWindowHeight, Console.LargestWindowWidth);
        }
  

	}
}

