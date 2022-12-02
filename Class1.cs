using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ProjetIA2022
{
    public class Node2 : GenericNode 
    {
        public int x;
        public int y;

        // Méthodes abstrates, donc à surcharger obligatoirement avec override dans une classe fille
        public override bool IsEqual(GenericNode N2)
        {
            Node2 N2bis = (Node2)N2;

            return (x == N2bis.x) && (y == N2bis.y);
        }

        public override double GetArcCost(GenericNode N2)
        {
            // Ici, N2 ne peut être qu'1 des 8 voisins, inutile de le vérifier
            Node2 N2bis = (Node2)N2;
            double dist = Math.Sqrt((N2bis.x-x)*(N2bis.x-x)+(N2bis.y-y)*(N2bis.y-y));
            if (Form1.matrice[x, y] == -1)
                // On triple le coût car on est dans un marécage
                dist = dist*3;
            return dist;
        }

        public override bool EndState()
        {
            return (x == Form1.xfinal) && (y == Form1.yfinal);
        }

        public override List<GenericNode> GetListSucc()
        {
            List<GenericNode> lsucc = new List<GenericNode>();

            for (int dx=-1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if ((x + dx >= 0) && (x + dx < Form1.nbcolonnes)
                                      && (y + dy >= 0) && (y + dy < Form1.nblignes) && ((dx != 0) || (dy != 0)))
                        if (Form1.matrice[x + dx, y + dy] > -2)
                        {
                            Node2 newnode2 = new Node2();
                            newnode2.x = x + dx;
                            newnode2.y = y + dy;
                            lsucc.Add(newnode2);
                        }
                }

            }
            return lsucc;
        }

        private double CalculDistance(int xI, int yI,int xF, int yF)
        {
            double distanceEucli = 0;
            int diffX = Math.Abs(xF - xI);
            int diffY = Math.Abs(yF - yI);
            int ajoutCases = Math.Abs(diffX - diffY);
            if (diffX > diffY)
            {
                distanceEucli = diffY * Math.Sqrt(2) + ajoutCases;
            }
            else
            {
                distanceEucli = diffX * Math.Sqrt(2) + ajoutCases;
            }
            return distanceEucli;
        }

        private static bool EstDansCercle(int x, int y)
        {
            return x > 2 && x <= 9 && y >= 4 && y <= 9;
        }
        /*public bool EstDansZone1(int x, int y)
        {
            return x > 2 && x <= 9 && y >= 4 && y <= 9;
        }*/
        private static bool EstDansZone2(int x, int y) // Zone du bas à gauche
        {
            return (x >= 0 && x <=10 && y >= 11 && y <= 19) || (x >= 8 && x <=9 && y >= 9 && y <= 10);
        }
        private static bool EstDansZone3(int x, int y)
        {
            return (x >= 0 && x <=10 && y >= 0 && y <= 2) || (x >= 8 && x <=10 && y >= 3 && y <= 4) ;
        }
        private static bool EstDansMarecage(int x, int y)
        {
            return x >= 11 && y <= 9 ;
        }
        private static bool EstDansZone5(int x, int y)
        {
            return x >= 11 && y >= 10 ;
        }
        private static bool EstDansZone6(int x, int y)
        {
            return x >=0 && x<= 3 && y>=3 && y <= 10;
        }
        public override double CalculeHCost()
        {
            int xF = Form1.xfinal;
            int yF = Form1.yfinal;
            double distanceEucli = 0;

            // Form1.matrice[x,y]
            if (Form1.matrice[10, 0] == -2) // on est dans Env2
            {
                if (x <= 10) //  noeud dans la zone gauche ou au centre du mur 
                {
                    if (xF <= 10) // pt d'arrivée dans la même zone que le pt de départ
                    {
                        distanceEucli = CalculDistance(x, y, xF, yF);
                    }
                    else // pt de départ à gauche et pt d'arrivée à droite
                    {
                        int xFIntermediaire = 10;
                        int yFIntermediaire = 8;
                        distanceEucli = CalculDistance(x,y, xFIntermediaire, yFIntermediaire) + CalculDistance(xFIntermediaire, yFIntermediaire, xF, yF); 
                    }
                }
                else if (x > 10)
                {
                    if (Form1.xfinal > 10) // pt d'arrivée dans la même zone que le pt de départ
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else // pt de départ à gauche et pt d'arrivée à droite
                    {

                        // calcul du trajet jusqu'au centre du plateau, dans le goulot
                        
                        int xFIntermediaire = 10;
                        int yFIntermediaire = 8;
                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + CalculDistance(xFIntermediaire,yFIntermediaire, xF,yF);
                    }
                }
            }
            else if (Form1.matrice[10, 0] == 0 && Form1.matrice[10, 1] == -2) // on est dans Env3
            {
                if (EstDansCercle(x, y)) // Zone 1 : dans le petit cercle
                {
                    int xFIntermediaireSortieCercle = 2;
                    int yFIntermediaireSortieCercle = 6;

                    if (EstDansCercle(xF, yF)) //deux points dans le petit cercle
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else if(EstDansZone2(xF,yF)) //point d'arrivée en bas à gauche
                    {
                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaireSortieCercle, yFIntermediaireSortieCercle) 
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xF, yF);
                    }
                    else if(EstDansZone3(xF,yF)) //point d'arrivée en haut à gauche
                    {
                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaireSortieCercle, yFIntermediaireSortieCercle) 
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle, xF, yF);
                    }
                    else if(EstDansMarecage(xF,yF)) //point d'arrivée dans le marécage
                    {
                        int xFIntermediaire = 11;
                        int yFIntermediaire = 0 ;

                        distanceEucli = 
                            CalculDistance(x,y, xFIntermediaireSortieCercle, yFIntermediaireSortieCercle) 
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle, xFIntermediaire, yFIntermediaire) 
                            + 3*CalculDistance(xFIntermediaire, yFIntermediaire, xF, yF);
                    }
                    else if(EstDansZone5(xF,yF)) //point d'arrivée en bas à droite
                    {
                        int xFIntermediaire = 11;
                        int yFIntermediaire = 0 ;

                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 10 ;

                        distanceEucli = CalculDistance(x, y, xFIntermediaireSortieCercle, yFIntermediaireSortieCercle)
                                        + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xFIntermediaire, yFIntermediaire) 
                                        + 3*CalculDistance(xFIntermediaire, yFIntermediaire, xFSortieMarecage,  yFSortieMarecage) 
                                        + CalculDistance(xFSortieMarecage, yFSortieMarecage, xF, yF);
                    }
                    else if (EstDansZone6(xF, yF)) //point d'arrivée au connecteur 
                    {
                        distanceEucli =
                            CalculDistance(x, y, xFIntermediaireSortieCercle,
                                yFIntermediaireSortieCercle)
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle, xF, yF);
                    }
                }
                else if (EstDansZone2(x,y)) // Zone 2 :  en bas à gauche 
                {
                    if (EstDansZone2(xF,yF)) // deux points dans la zone 2 || petit triangle
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else if(EstDansCercle(xF,yF)) // point d'arrivée dans le cercle
                    {
                        int xFIntermediaireSortieCercle = 2;
                        int yFIntermediaireSortieCercle = 6;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaireSortieCercle, yFIntermediaireSortieCercle) 
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xF, yF);
                    }
                    else if(EstDansZone3(xF,yF)) // point d'arrivée en haut à gauche 
                    {
                        int xFIntermediaire = 1;
                        int yFIntermediaire = 9;

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 4;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + CalculDistance(xFIntermediaire, yFIntermediaire,xFIntermediaire2, yFIntermediaire2) 
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xF, yF);
                    }
                    else if(EstDansMarecage(xF,yF)) // point d'arrivée dans le marécage 
                    {
                        int xFIntermediaire = 1;
                        int yFIntermediaire = 9;

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 4;

                        int xFIntermediaire3 = 11;
                        int yFIntermediaire3 = 0;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + CalculDistance(xFIntermediaire, yFIntermediaire,xFIntermediaire2, yFIntermediaire2) 
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xFIntermediaire3, yFIntermediaire3) 
                            +3* CalculDistance(xFIntermediaire3, yFIntermediaire3,xF, yF);
                    }
                    else if(EstDansZone5(xF,yF)) //point d'arrivée en bas à droite 
                    {
                        int xFIntermediaire = 1;
                        int yFIntermediaire = 9;

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 4;

                        int xFIntermediaire3 = 11;
                        int yFIntermediaire3 = 0;

                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 10 ;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + CalculDistance(xFIntermediaire, yFIntermediaire,xFIntermediaire2, yFIntermediaire2) 
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xFIntermediaire3, yFIntermediaire3) 
                            + 3*CalculDistance(xFIntermediaire3, yFIntermediaire3,xFSortieMarecage, yFSortieMarecage) 
                            + CalculDistance(xFSortieMarecage, yFSortieMarecage,xF, yF);
                    }
                    else if (EstDansZone6(xF, yF)) //point d'arrivée au connecteur 
                    {
                        const int xFIntermediaire = 1;
                        const int yFIntermediaire = 9;
                        
                        distanceEucli =
                            CalculDistance(x, y, xFIntermediaire,
                                yFIntermediaire)
                            + CalculDistance(xFIntermediaire, yFIntermediaire, xF, yF);
                    }
                }
                else if (EstDansZone3(x,y)) // Zone 3 : en haut à gauche
                {
                    if (EstDansZone3(xF,yF) ) // deux points dans la zone 3 || petit triangle
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else if(EstDansCercle(xF,yF)) // point d'arrivée dans le cercle
                    {
                        int xFIntermediaireSortieCercle = 2;
                        int yFIntermediaireSortieCercle = 6;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaireSortieCercle, yFIntermediaireSortieCercle) 
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle, xF, yF);
                    }
                    else if(EstDansZone2(xF,yF)) // point d'arrivée en bas à gauche 
                    {
                        int xFIntermediaire = 1;
                        int yFIntermediaire = 4;
                        
                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 9;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + CalculDistance(xFIntermediaire, yFIntermediaire,xFIntermediaire2, yFIntermediaire2) 
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xF, yF);
                    }
                    else if(EstDansMarecage(xF,yF)) // point d'arrivée dans le marécage 
                    {
                        int xFIntermediaire = 11;
                        int yFIntermediaire = 0;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire) 
                            + 3*CalculDistance(xFIntermediaire, yFIntermediaire,xF, yF);
                    }
                    else if(EstDansZone5(xF,yF)) //point d'arrivée en bas à droite 
                    {
                        int xFIntermediaire = 11;
                        int yFIntermediaire = 0;

                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 10 ;

                        distanceEucli = 
                            CalculDistance(x,y,xFIntermediaire, yFIntermediaire)  
                            + 3*CalculDistance(xFIntermediaire, yFIntermediaire,xFSortieMarecage, yFSortieMarecage) 
                            + CalculDistance(xFSortieMarecage, yFSortieMarecage,xF, yF);
                    }
                    else if (EstDansZone6(xF, yF)) //point d'arrivée au connecteur 
                    {
                        const int xFIntermediaireZone3 = 1;
                        const int yFIntermediaireZone3 = 4;
                        
                        distanceEucli =
                            CalculDistance(x, y, xFIntermediaireZone3,
                                yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xF, yF);
                    }
                }
                else if (EstDansMarecage(x,y)) // Zone 4 : zone marécage
                {
                    if (EstDansMarecage(xF,yF)) // deux points dans la zone 4 
                    {
                        distanceEucli = 3*CalculDistance(x,y,xF, yF);
                    }
                    else if (EstDansCercle(xF,yF)) // point d'arrivée dans le cercle
                    {
                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaireSortieCercle = 2;
                        int yFIntermediaireSortieCercle = 6;
                            
                        distanceEucli = 
                            3* CalculDistance(x,y,xFIntermediaireGoulot, yFIntermediaireGoulot) 
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xFIntermediaireSortieCercle, yFIntermediaireSortieCercle)
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xF, yF);
                    }
                    else if (EstDansZone2(xF,yF)) // point d'arrivée en bas à gauche 
                    {
                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 9;

                        distanceEucli = 
                            3*CalculDistance(x,y,xFIntermediaireGoulot, yFIntermediaireGoulot) 
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xFIntermediaireZone3, yFIntermediaireZone3) 
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3,xFIntermediaire2, yFIntermediaire2)  
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xF, yF);
                    }
                    else if (EstDansZone3(xF,yF)) // point d'arrivée en haut à gauche
                    {
                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        distanceEucli = 
                            3*CalculDistance(y,y,xFIntermediaireGoulot, yFIntermediaireGoulot) 
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xF, yF);
                    }
                    else if (EstDansZone5(xF,yF)) //point d'arrivée en bas à droite 
                    {
                        int xFSortieMarecage = x;
                        int yFSortieMarecage = 10;

                        distanceEucli =
                            3*CalculDistance(x,y,xFSortieMarecage, yFSortieMarecage) 
                            + CalculDistance(xFSortieMarecage, yFSortieMarecage,xF, yF);
                    }
                    else if (EstDansZone6(xF, yF)) //point d'arrivée au connecteur 
                    {
                        const int xFIntermediaireGoulot = 10;
                        const int yFIntermediaireGoulot = 0;

                        const int xFIntermediaireZone3 = 1;
                        const int yFIntermediaireZone3 = 4;
                        
                        distanceEucli =
                            3 * CalculDistance(x, y, xFIntermediaireGoulot,
                                yFIntermediaireGoulot)
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot, xFIntermediaireZone3,
                                yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xF, yF);
                    }
                }
                else if(EstDansZone5(x,y)) //Zone 5 : en dessous de la zone marécage
                {
                    if (EstDansZone5(xF,yF)) // deux points dans la zone 5 
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else if (EstDansCercle(xF,yF)) // point d'arrivée dans le cercle
                    {
                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 9;

                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaireSortieCercle = 2;
                        int yFIntermediaireSortieCercle = 6;

                        distanceEucli = 
                            CalculDistance(x,y,xFSortieMarecage, yFSortieMarecage) 
                            + 3*CalculDistance(xFSortieMarecage, yFSortieMarecage,xFIntermediaireGoulot, yFIntermediaireGoulot) 
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xFIntermediaireSortieCercle, yFIntermediaireSortieCercle)
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xF, yF);
                    }
                    else if (EstDansZone2(xF,yF)) // point d'arrivée en bas à gauche 
                    {
                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 9;
                            
                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 9;

                        distanceEucli = 
                            CalculDistance(x,y,xFSortieMarecage, yFSortieMarecage) 
                            + 3*CalculDistance(xFSortieMarecage, yFSortieMarecage,xFIntermediaireGoulot, yFIntermediaireGoulot)
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3,xFIntermediaire2, yFIntermediaire2)
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xF, yF);
                    }
                    else if (EstDansZone3(xF,yF)) // point d'arrivée en haut à gauche
                    {
                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 9;

                        int xFIntermediaireGoulot = 10;
                        int yFIntermediaireGoulot = 0;

                        distanceEucli = 
                            CalculDistance(x,y,xFSortieMarecage, yFSortieMarecage) 
                            + 3*CalculDistance(xFSortieMarecage, yFSortieMarecage,xFIntermediaireGoulot, yFIntermediaireGoulot) 
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xF, yF);
                    }
                    else if (EstDansMarecage(xF,yF)) //point d'arrivée au marécage
                    {
                        int xFSortieMarecage = xF;
                        int yFSortieMarecage = 9;

                        distanceEucli =
                            CalculDistance(x,y,xFSortieMarecage, yFSortieMarecage) 
                            + 3*CalculDistance(xFSortieMarecage, yFSortieMarecage,xF, yF);
                    }    
                    else if (EstDansZone6(xF, yF)) //point d'arrivée au connecteur 
                    {
                        const int xFSortieMarecage = 11;
                        const int yFSortieMarecage = 9;
                            
                        const int xFIntermediaireGoulot = 10;
                        const int yFIntermediaireGoulot = 0;

                        const int xFIntermediaireZone3 = 1;
                        const int yFIntermediaireZone3 = 4;
                        
                        distanceEucli =
                            CalculDistance(x, y, xFSortieMarecage, yFSortieMarecage)
                            + 3 * CalculDistance(xFSortieMarecage, yFSortieMarecage, xFIntermediaireGoulot,
                                yFIntermediaireGoulot)
                            + CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot, xFIntermediaireZone3,
                                yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xF, yF);
                    }
                }
                else if (EstDansZone6(x,y)) //Zone 6 : partie voisine de la HG et BG
                {
                    if (EstDansZone6(xF,yF)) // point arrivée dans le connector
                    {
                        distanceEucli = CalculDistance(x,y,xF, yF);
                    }
                    else if (EstDansCercle(xF,yF)) // point d'arrivée dans le cercle
                    {
                        int xFIntermediaireSortieCercle = 2;
                        int yFIntermediaireSortieCercle = 6;

                        distanceEucli =
                            CalculDistance(x,y,xFIntermediaireSortieCercle, yFIntermediaireSortieCercle)
                            + CalculDistance(xFIntermediaireSortieCercle, yFIntermediaireSortieCercle,xF, yF);
                    }
                    else if (EstDansZone2(xF,yF)) // point d'arrivée en bas à gauche 
                    { 

                        int xFIntermediaire2 = 1;
                        int yFIntermediaire2 = 9;

                        distanceEucli =
                            CalculDistance(x,y,xFIntermediaire2, yFIntermediaire2)
                            + CalculDistance(xFIntermediaire2, yFIntermediaire2,xF, yF);
                    }
                    else if (EstDansZone3(xF,yF)) // point d'arrivée en haut à gauche
                    {
                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        distanceEucli =
                            CalculDistance(x,y,xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3,xF, yF);
                    }
                    else if (EstDansMarecage(xF,yF)) //point d'arrivée au marécage
                    {
                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaireGoulot = 11;
                        int yFIntermediaireGoulot = 0;

                        distanceEucli =
                            CalculDistance(x, y, xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3, xFIntermediaireGoulot, yFIntermediaireGoulot)
                            +3* CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot, xF, yF);
                    }
                    else if (EstDansZone5(xF,yF)) //point d'arrivée en bas à droite 
                    {
                        int xFIntermediaireZone3 = 1;
                        int yFIntermediaireZone3 = 4;

                        int xFIntermediaireGoulot = 11;
                        int yFIntermediaireGoulot = 0;

                        int xFSortieMarecage = 11;
                        int yFSortieMarecage = 10;

                        distanceEucli =
                            CalculDistance(x,y,xFIntermediaireZone3, yFIntermediaireZone3)
                            + CalculDistance(xFIntermediaireZone3, yFIntermediaireZone3,xFIntermediaireGoulot, yFIntermediaireGoulot)
                            + 3*CalculDistance(xFIntermediaireGoulot, yFIntermediaireGoulot,xFSortieMarecage, yFSortieMarecage)
                            + CalculDistance(xFSortieMarecage, yFSortieMarecage,xF, yF);
                    }
                }
            }
            else // Env1
            {
                distanceEucli = CalculDistance(x,y,xF, yF);
                
            }
            
            // x et y du noeud examiné
            // Form1.xinitial & Form1.yinitial 
            // Form1.xfinal   & Form1.yfinal 
            // matrice[x,y] donne type de case (-2 : obstacle ; -1 : marécage ; 0 : rien)
            // tenter d'estimer la distance qu'il reste entre la position actuelle et la position finale
            return ( distanceEucli );           
        }

        public override string ToString()
        {
            return Convert.ToString(x)+","+ Convert.ToString(y);
        }
    }
}