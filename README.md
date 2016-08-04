# JeedomApp
Application Windows Universelle non officielle permettant de gérer sa box domotique Jeedom.
Cette application n'est pas officielle et est un frok de Domojee.
Il s'agit juste d'un petit projet réalisé en attendant l'application officielle.

##Configuration de l'application
L'application se connecte a votre jeedom par son api REST.
Pour vous y conecter, il faut entree l'adresse de connexion ainsi que sa clef.
Cette procédure un peut fastidieuse sera remplacé par une lecture de QRcode de l'application officiel App Mobile (mobile).

##Mise a jours des informations en backend
Tous les echanges avec jeedom se font en arriere plan, ce qui libere des ressources et offre une meilleur experience utilisateur

##Dashboard

Cette application reprend le principe du dashboard de jeedom.
Elle affichera tous vous equipements et commande dans des tuilles

##Géolocalisation
Pour l'utilisation de cette fonction il est obligatoire d'avoir sur son jeedom, le plugin Geoloc d'installé avec une commande pour l'appareil que vous souhaité connaitre sa position.
L'application va relever votre position toutes les 15 minutes et l'envoyer a jeedom
Un systeme de GeoFending prend le relais lorsque l'on entre dans un zone defini dans votre configuration de l'application

##Utilisation de Cortana
Il est impératif d'etre sur une version de jeedom superieur a la 2.2.
L'application téléchargera toutes les interactions configurer sous jeedom pour les transmetre a Cortana.
Pour l'utilise il suffira de demandé a Cortana
Jeedom, "votre interaction"
Exemple:
Jeedom, Quel est la temperature ambiante du salon
