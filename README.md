# Optimisation d’heuristiques - A*

ENSC - École Nationale Supérieure de Cognitique
E-mails:
● thomas.chimbault@ensc.fr
● tristan.goncalves@ensc.fr
30 novembre 2022
I. Introduction
Parmi les algorithmes de résolution générale de problèmes, certains peuvent
être utilisés dans le cadre de la recherche du plus court chemin. C'est par exemple le
cas de l'algorithme de Dijkstra, qui sert à trouver le chemin le plus court entre deux
noeuds d'un graphe, en construisant (dans le cadre du Dijkstra dynamique) un arbre
au fur et à mesure, en plaçant des noeuds dans l'ensemble des Ouverts puis des
Fermés.
Dans le but d'accélérer cet algorithme, une méthode d'optimisation a été développée :
l’A* (A star). Cette méthode se base sur Dijkstra en y ajoutant des heuristiques afin de
minimiser le nombre de noeuds ouverts et fermés. En effet, le Dijkstra est
