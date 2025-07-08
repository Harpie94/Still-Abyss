# Technical Design Document (TDD)

## Table of Contents
1. [Introduction](#1-introduction)  
2. [Gameplay](#2-gameplay)  
3. [Interface Utilisateur (UI/UX)](#3-interface-utilisateur-uiux)  
4. [Architecture Technique](#4-architecture-technique)  
5. [Plan de Développement](#5-plan-de-développement)  
6. [Ressources et Références](#6-ressources-et-références)  
7. [Nomenclature](#7-nomenclature)  
8. [UML](#8-uml)  
9. [Optionnel](#9-optionnel)

## 1. Introduction

### 1.1 Nom du projet
**Nom du jeu** : _Still Abyss_  
**Équipe** : _Romain Ponsignon_
**Date de création** : _08-07-2025_  
**Version du document** : _1.0.0_  

### 1.2 Objectif du projet
Résumé du concept du jeu : _Un defend the office inspiré de FNAF et Subnautica en 3D où le joueur joue un technicien dans une base sous marine. Le joueur de jour, doit réparer des dégâts infligés à la base / améliorer des éléments en se déplaçant dans un environnement en 3D, pendant la nuit il doit détecter des anomalies depuis son poste._

### 1.3 Plateformes et technologies
- **Plateformes cibles** : _PC_  
- **Moteur de jeu** : Unity  
- **Langages de programmation** : C#
- **Gestion de version** : GitHub  

---

## 2. Gameplay

### 2.1 Mécaniques de base
- **Type de jeu** : Defend the office endless
- **Déplacement** : 3 dimensions le jour
- **Contrôles** : _ZQSD le jour, interaction de avec l'environnement à l'aide d'un système de caméra la nuit_  

### 2.2 Anomalies
- **Fréquence d’apparition** : _Aléatoire mais plus la nuit dure, plus la fréquence d'apparition est élevée, le nombre de nuit passée augmente aussi la fréquence_  
- **Effets visuels et sonores associés** : _bruits d'ambiances / indications visuels_  
- **Conséquences sur le gameplay** : _Impact sur la difficulté dut aux dégâts que les entités peuvent causer à la base que le joueur doit réparer la journée_
  
### 2.3 Défense
- **Types de défenses** : 
  - Salle du joueur:
    - Portes blindées que le joueur peut activer à différents endroits
    - Volets blindés que le joueur peut fermer si il détecte une entité / infiltration d'eau

- **Cout d'activation** : _Demande de l'énergie à la batterie_
- **Durée d’activation** : _En fonction du temps en seconde_  
- **Méthode d’activation** : _Utilisant le clavier / clic sur des boutons_  

### 2.4 Système de caméras
- **Inspiration** : FNAF  
- **Navigation** :  
  - Écran de sélection de caméras  
  - Clic sur une salle pour afficher le flux local  
  - Les caméras peuvent présenter des bugs, bruit statique ou devenir inaccessibles si inondées  

- **Fonctionnalités** :  
  - Affichage des statuts :  
    - **Grisé** : Caméra inaccessible (salle inondée)  
    - **Rouge** : Dégâts majeurs détectés  
    - **Orange** : Dégâts légers  
  - Interaction directe :  
    - Fermer une porte ou volet de la salle visionnée  
  - Améliorations possibles :  
    - Résistance à l’eau  
    - Vision nocturne  
    - Détéction d’anomalies
    - Flash  

### 2.5 Activités diurnes

Le jour, le joueur peut se déplacer dans la base et effectuer diverses tâches critiques pour la survie et la préparation à la nuit suivante :

- **Réparer les dégâts** causés par les anomalies
- **Installer ou améliorer des systèmes** :
  - Ajout de **nouvelles sources d’énergie** (ex : hydroliennes)
  - Renforcement des structures (portes, murs...)
- **Explorer des zones optionnelles** pour trouver des ressources ou journaux audio/vidéo
- **Réparer des zones** (zones fermées, couloirs inondés, pièces scellées)

---

## 3. Interface Utilisateur (UI/UX)

### 3.1 Écrans principaux

- **Interface jour** :  
  - Pas de barre d’état affichée en permanence.  
  - Le joueur peut ouvrir une **tablette** pour consulter :  
    - L’intégrité des différentes pièces de la base (affichage similaire aux caméras, avec une sorte de "barre de vie" ou indicateur de santé des pièces).  
    - Les types de dégâts détectés dans chaque zone.  
    - La position actuelle du joueur dans la base (carte simplifiée ou plan).  

- **Interface nuit** :  
  - Trois écrans principaux à disposition :  
    - **Écran principal** : affichage de la caméra sélectionnée (flux vidéo en direct).  
    - **Écran secondaire** : affichage des actions possibles sur la caméra sélectionnée (exemple : activation de la vision nocturne, flash, fermeture des portes/volets).  
    - **Troisième écran** : sélection rapide des caméras disponibles.  
  - Un écran secondaire périphérique (sur ou autour de l’écran principal) indique :  
    - Le niveau actuel de la batterie restante.  
    - L’utilisation instantanée de l’énergie (consommation en temps réel).  

- **Menus** :  
  - **Menu principal**  
  - **Menu options** (paramètres audio, graphismes, contrôles)  
  - **Menu pause** (accessible pendant le jeu)  
  - **Menu amélioration** accessible via la tablette (gestion des upgrades des caméras, portes, systèmes de détection)

---

### 3.2 Navigation et ergonomie

- Contrôles intuitifs clavier/souris adaptés à chaque phase (jour/nuit).  
- Retour visuel clair pour chaque interaction (ex : clic sur porte ou caméra).  
- Sons d’ambiance et alertes sonores différenciées selon les actions et alertes détectées.  
- Notifications contextuelles discrètes mais informatives (ex : dégâts détectés, batterie faible).

---

### 3.3 Design graphique

- Style visuel low poly, ambiance sous-marine et sci-fi, tons froids et sombres.  
- Icônes claires et lisibles pour les états des pièces et équipements.  
- Animations douces pour transitions entre caméras et interactions (ex : ouverture tablette, changement caméra).  
- Effets visuels pour signifier les alertes critiques (clignotements, couleurs chaudes).

---

## 4. Architecture Technique

### 4.1 Organisation Unity

- **Une seule scène persistante**  
- Cycle jour/nuit intégré dynamiquement (éclairage, ambiance, gameplay)  
- Dossiers :  
  - `Scripts/`, `Prefabs/`, `UI/`, `Scenes/`, `Models/`

### 4.2 Systèmes principaux

#### 4.2.1 Game Loop
- Timer global 10 minutes
- Déclenche jour/nuit  
- Nuit = 7 minutes, le reste est jour  
- Passage déclenche scripts spécifiques (activation/désactivation de systèmes)

#### 4.2.2 Caméras
- `CameraNode` + `CameraController`  
- UI dynamique basée sur `RenderTexture`  
- Actions intégrées dans l’UI

#### 4.2.3 Énergie & Oxygène

- **Production d’énergie** :
  - **Jour** : panneaux solaires rechargent la batterie principale.
  - **Nuit** : aucune production d’énergie par défaut.
  - **Upgrade** : possibilité de débloquer des **hydroliennes** pour générer une quantité limitée d’énergie pendant la nuit.
  
- **Consommation** :
  - Caméras, portes, volets et systèmes de défense consomment de l’énergie selon leur usage.
  - Interface de nuit affiche en temps réel :
    - Batterie restante
    - Consommation instantanée (ex. fermeture d’une porte = pic)

- **Défaillance énergétique** :
  - Si la batterie atteint 0 :
    - Les systèmes se coupent.
    - Un **compteur d’oxygène** s’active (temps limité pour survivre sans énergie).
    - Si l’oxygène atteint 0 → **Game Over**.

- **Recharge** :
  - Uniquement possible le **jour** (panneaux solaires ou réparation du réseau).
  - Plus lente s’il y a des dégâts dans la pièce contenant les générateurs.
  - Les **hydroliennes** (si débloquées) fournissent un **flux constant** mais limité la nuit (pas suffisant pour alimenter tout en continu).

- **UI liée** :
  - Niveau de batterie visible en nuit sur un écran dédié.
  - Icône ou jauge de charge visible pendant la phase de jour (représente production en cours).

#### 4.2.4 Anomalies
- `AnomalySpawner` (basé sur intensité/nuit actuelle)  
- Les entités n'inondent pas, mais causent des **dégâts structurels**  
- Entité + porte non fermée = mort  
- Alerte visuelle + sonore pour réaction

#### 4.2.5 Pression des portes
- Chaque porte a une pression entre 0 et 100  
- Si < seuil → ouverture automatique  
- Maintenance de jour pour restaurer

### 4.3 Système d’améliorations

Le joueur pourra, via la tablette (menu amélioration), débloquer et installer diverses améliorations pour mieux survivre aux nuits successives :

- **Systèmes de production d’énergie** :
  - **Hydroliennes** : génèrent de l’énergie en continu pendant la nuit (débloquées via amélioration)
  - **Amélioration de panneaux solaires** : rendement accru, recharge plus rapide
  - **Amélioration de la batterie** : Meilleure capacité

- **Systèmes de surveillance** :
  - **Caméras renforcées** : résistantes à l’eau, résistance aux coupures,...
  - **Vision nocturne** 
  - **Détecteurs d’anomalies** 
  - **Flash de zone**

- **Défenses** :
  - **Blindage amélioré** pour les portes/volets
  - **Automatisation partielle** : certaines actions peuvent être déléguées à une IA de bord (ex : fermeture automatique)

- **Réparation/maintenance** :
  - **Outils de réparation avancés** (plus rapide)
  - **Meilleur mobilité** combinaison de réparation plus rapide dans les pièces submergées
  - **Plus de capacitée** O2 max augmenté
  
### 4.4 Sauvegarde
- Pas de sauvegarde auto entre chaque nuit  
- Le joueur peut sauvegarder manuellement ses **parties infinies**  
- Format JSON ou système interne Unity  
- Slots multiples

### 4.5 Architecture et pratiques
- Système orienté événements (Event Bus)  
- `GameManager` central  
- Entrées gérées via `InputManager`  
- Machines à états pour entités et gameplay général  

---

## 5. Plan de Développement

### 5.1 Roadmap
| Semaines         | Objectifs principaux                                                                                                                                                                                                                                                      |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Semaine 1-2**  |  **Prototype de base** : <br> - Implémentation du cycle **jour/nuit** <br> - Déplacements du joueur (jour) <br> - Contrôles caméra (nuit) <br> - Portes et volets <br> - Mécanique de **réparation** <br> - Système d'**amélioration** de base                          |
| **Semaine 3-4**  |  **Ajout de contenu gameplay** : <br> - Système d'**anomalies** basique <br> - Interface **tablette** pour le jour <br> - Ajout d’**upgrades bonus** : caméras, énergie, IA                                                                                             |
| **Semaine 5-6**  |  **Défenses et systèmes caméra** : <br> - Implémentation complète du système de **défense** (portes, volets, flash) <br> - Finalisation des **caméras** (UI complète, bugs, interaction) <br> - Système de **cycle jour/nuit** fonctionnel avec transitions dynamiques |
| **Semaine 7-8**  |  **Gestion des menaces** : <br> - Dégâts **structurels dynamiques** sur les pièces <br> - Système de **pression des portes** <br> - Intégration du **système d’oxygène** et conditions de **game over**                                                                 |
| **Semaine 9-10** |  **Polish gameplay + contenu** : <br> - Amélioration de l’**intelligence des anomalies** <br> - Ajout d’**événements aléatoires** (pannes, bruits, leurres) <br> - Équilibrage des **ressources énergétiques** (hydroliennes, recharge lente, etc.)                     |
| **Semaine 11**   |  **Playtests internes** : <br> - Retours d'expérience <br> - Ajustements des mécaniques trop punitives ou confuses <br> - Corrections de bugs bloquants                                                                                                                 |
| **Semaine 12**   |  **Préparation livrable final** : <br> - Stabilisation du projet <br> - Optimisations (fps, chargements, assets inutilisés) <br> - Nettoyage du code et du projet Unity <br> - Création d’une **build jouable** propre avec tous les systèmes fonctionnels               |
 

### 5.2 Répartition des tâches
- **Développeurs** : _Romain Ponsignon_  
- **Graphistes/UI** : __  
- **Sound Design** : __  
- **Testeurs** : __  

---

## 6. Ressources et Références


---

## 7. Nomenclature

### 7.1 Généralités
- Utiliser **PascalCase** pour tous les noms.
- Préfixer chaque asset en fonction de son type pour faciliter l’organisation.
- Éviter les abréviations trop courtes ou non standardisées.
- Utiliser des noms explicites et descriptifs.

### 7.2 Préfixes des assets

| Type d’Asset           | Préfixe     | Exemple                       |
| ---------------------- | ----------- | ----------------------------- |
| Script C#              | `Script_`   | `Script_PlayerController`     |
| ScriptableObject       | `SO_`       | `SO_UpgradeData_Hydroturbine` |
| Prefab                 | `Prefab_`   | `Prefab_EnemyBot`             |
| Material               | `Mat_`      | `Mat_Glass_Base`              |
| Material Variant       | `MatVar_`   | `MatVar_Glass_Cracked`        |
| Texture (2D/UI)        | `Tex_`      | `Tex_UI_HealthIcon`           |
| Mesh (3D modèle)       | `Mesh_`     | `Mesh_DoorPanel_Large`        |
| Animation Controller   | `AC_`       | `AC_PlayerHumanoid`           |
| Animation Clip         | `Anim_`     | `Anim_RunLoop`                |
| Audio Clip (son SFX)   | `SFX_`      | `SFX_DoorClose`               |
| Audio Clip (musique)   | `Music_`    | `Music_MainTheme`             |
| Particle System (VFX)  | `VFX_`      | `VFX_WaterLeak_Small`         |
| UI Canvas/Panel        | `UI_`       | `UI_TabletScreen`             |
| UI Sprite/Icon         | `Icon_`     | `Icon_OxygenWarning`          |
| Scene/Level            | `Scene_`    | `Scene_MainBase`              |
| Shader (graph/code)    | `Shader_`   | `Shader_WaterDistortion`      |
| Timeline (Cinemachine) | `Timeline_` | `Timeline_IntroSequence`      |


### 7.3 Blueprints spécifiques
- Les **Blueprints d’acteurs** doivent suivre la structure : `BP_[Nom]`
- Les **Blueprints composants** : `BPC_[Nom]`
- Les **Blueprints de contrôleur** : `BP_Controller_[Nom]`
- Les **Blueprints de GameMode** : `BP_GM_[Nom]`
- Les **Blueprints de HUD** : `BP_HUD_[Nom]`

### 7.4 Variables
- **Booleans** : Préfixe `b` (ex: `bIsOpen`)
- **Integers** : `i` (ex: `iCurrentTime`, `iPlayerO2`)
- **Floats** : `f` (ex: `fSpeed`, `fEnergyLevel`)
- **Vectors** : `v` (ex: `vPlayerPosition`)
- **Textures** : `Tex_` (ex: `Tex_IconCamera`)
- **Audio** : `Aud_` (ex: `Aud_ClosingDoor1`)

### 7.5 Fonctions et événements
- Les fonctions commencent par un verbe pour indiquer l’action : `OpenDoor`, `SpawnEnemy`, `PlaySoundEffect`
- Les événements commencent par `On` : `OnPlayerDeath`, `OnCameraClick`
- Les interfaces sont préfixées par `I_` : `I_Interactable`

### 7.6 Dossiers du projet Unity
La structure des dossiers doit être claire pour organiser les assets.

### 7.7 Commit

Description de l'action réalisée (Add:, Fix: ...) + description de ce qui a été ajouté ou modifié (EX: Add: class Player). Ajouter en description du commit quels fichiers on été Modifier/Supprimer/Déplacer/Ajouter.

---

## 8 UML

![]()

![]()


---

## 9. Optionnel 

Cette section regroupe les améliorations et ajouts envisageables si le projet est terminé en avance ou repris ultérieurement.  
