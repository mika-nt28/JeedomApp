# JeedomApp
Application Windows Universelle non officielle permettant de gérer sa box domotique Jeedom.
Cette application n'est pas officielle et est un fork de Domojee.
Il s'agit juste d'un petit projet réalisé en attendant l'application officielle.

##Configuration de l'application
L'application se connecte a votre jeedom par son api REST.
Pour vous y conecter, il faut entrer l'adresse de connexion ainsi que sa clef.
Cette procédure un peufastidieuse sera remplacée par une lecture de QRcode de l'application officielle App Mobile (mobile).

##Mise a jours des informations en backend
Tous les échanges avec jeedom se font en arriere plan, ce qui libère des ressources et offre une meilleur expérience utilisateur.

##Dashboard

Cette application reprend le principe du dashboard de jeedom.
Elle affichera tous vous équipements et commandes dans des tuilles.

##Géolocalisation
Pour l'utilisation de cette fonction il est obligatoire d'avoir sur son jeedom, le plugin Geoloc d'installé avec une commande pour l'appareil dont vous souhaitait connaitre la position.
L'application va relever votre position toutes les 15 minutes et l'envoyer à jeedom.
Un système de GeoFensing prend le relais lorsque l'on entre dans un zone definie dans la configuration de l'application.

##Utilisation de Cortana
Il est impératif d'être sur une version de jeedom superieur à la 2.2.
L'application téléchargera toutes les interactions configurées sous jeedom pour les transmètre à Cortana.
Pour l'utiliser il suffira de demander à Cortana
Jeedom, "votre intéraction"
Exemple:
Jeedom, Quel est la température ambiante du salon
