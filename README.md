# CodeSamples
I took some of the more interesting scripts from my projects and organized them in this repository.

In the Gauntlet2 folder you will find the following:

Base Character Script: Inherited by all characters.

CharacterState: An abstract class that is inherited by all possible character states. Contains an abstract method "handleInput()", which can be ducktyped to handle player inputs based on which state the character is currently in.

WalkingState: Inherits CharacterState

CharacterStateLibrary: Holds all the states a character can be in. Has a pointer to the current character state. That character state will have its handleinput() method called. Each character has af field for this library.

GameManager: Contains libraries of data and methods that should be accessible by any class at any time.

GamePassiveItemLibrary: Deciphers JSON data of passive items so that many possible passive items can be efficiently coded and randomly distrubted around the map.

GameCharacterLibrary: Contains all the characters in the game, so a player can create a copy of the character when he or she gets that character. Deciphers JSON data for character stats, which is attached during the start of the game.

SwitchID: static class that contains the IDs for characters, passive items, character stats, and crowd control so classes can easily and clearly interact.

CameraControl: Controls camera logic for the player

FerroMagneticPulse: Projectile that locks onto a player and nudges the projectile towards him or her.


In the ViViD AI folder you will find the following:

Zombie is an abstract class that has a basic AI finite state machine written into it. It will wander around aimlessly until it finds a player in certain radius of sight, then start following the player.

Basic Zombie inherits the Zombie class and has the added function of attacking when the player is in range.

Fast Zombie inherits the basic Zombie class and will jump onto a player if he or she is in range. The zombie explodes on collision with the player.

Boid and flockcontroller implement the boid algorithm to create a flocking mentality between AI so certain types of zombies form groups.


In the ViViD Data Processing Folder you will find the following:

UserData: Contains listeners for game start, level completed, death, and using special abilities event. When the level begins, the player's current position and timestamp relative to the level start time is recorded every ten seconds. On completion of the level, a file containing those positions, the amount of time the player took to complete the level, and the amount of times the player died and used his or her special ability is uploaded to a MySQL database. There is also a PHP file that contributes to this functionality called playerdata.

Register/Login: Sends a query through their respective php files to the MySQL database containing registered users  to either register or log in a player.

ProcessToExcel: Python program that accesses the playerdata MySQL database, deciphers the information, then writes the data into an organized Excel workbook 

