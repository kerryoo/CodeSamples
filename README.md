# CodeSamples
Please hire me c:

These samples are from my current project, a 20 person multiplayer battle royale game Gauntlet.  
Each player gets dropped in a neo-tokyo city and has to find characters to add to his or her character inventory. Each player can only have one character active at once, but can switch characters easily. Each character has a special move that can combo with other characters' special moves.

In the folder you will find the following:

Base Character Script: Inherited by all characters.

CharacterState: An abstract class that is inherited by all possible character states. Contains an abstract method "handleInput()", which can be ducktyped to handle player inputs based on which state the character is currently in.

WalkingState: Inherits CharacterState

CharacterStateLibrary: Holds all the states a character can be in. Has a pointer to the current character state. That character state will have its handleinput() method called. Each character has af field for this library.

GameManager: Contains libraries of data and methods that should be accessible by any class at any time.

GamePassiveItemLibrary: Deciphers JSON data of passive items so that many possible passive items can be efficiently coded and randomly distrubted around the map.

GameCharacterLibrary: Contains all the characters in the game, so a player can create a copy of the character when he or she gets that character. Deciphers JSON data for character stats, which is attached during the start of the game.

SwitchID: static class that contains the IDs for characters, passive items, character stats, and crowd control so classes can easily and clearly interact.




