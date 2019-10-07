# CodeSamples
I took some of the more interesting scripts from my projects and organized them in this repository.

These samples are from my current project, a 20 person multiplayer battle royale game Gauntlet.  
Each player gets dropped in a neo-tokyo city and has to find characters to add to his or her character inventory. Each player can only have one character active at once, but can switch characters easily. Each character has a special move that can combo with other characters' special moves.

In the Gauntlet2 folder you will find the following:

Base Character Script: Inherited by all characters.

CharacterState: An abstract class that is inherited by all possible character states. Contains an abstract method "handleInput()", which can be ducktyped to handle player inputs based on which state the character is currently in.

WalkingState: Inherits CharacterState

CharacterStateLibrary: Holds all the states a character can be in. Has a pointer to the current character state. That character state will have its handleinput() method called. Each character has af field for this library.

GameManager: Contains libraries of data and methods that should be accessible by any class at any time.

GamePassiveItemLibrary: Deciphers JSON data of passive items so that many possible passive items can be efficiently coded and randomly distrubted around the map.

GameCharacterLibrary: Contains all the characters in the game, so a player can create a copy of the character when he or she gets that character. Deciphers JSON data for character stats, which is attached during the start of the game.

SwitchID: static class that contains the IDs for characters, passive items, character stats, and crowd control so classes can easily and clearly interact.


In the ViViD AI folder you will find the following:

Zombie is an abstract class that has a basic AI finite state machine written into it. It will wander around aimlessly until it finds a player in certain radius of sight, then start following the player.

Basic Zombie inherits the Zombie class and has the added function of attacking when the player is in range.

Fast Zombie inherits the basic Zombie class and will jump onto a player if he or she is in range. The zombie explodes on collision with the player.

Boid and flockcontroller implement the boid algorithm to create a flocking mentality between AI so certain types of zombies form groups.



