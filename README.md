# CandyQuest-Scripts

Candy Quest is a limitlessly expanding action adventure game. It's an original pitch of mine. The game puts you on a conquest for ultimate happiness: the more candy you eat, you transcend towards the heavens. You are haunted by vegetables that try to impede your transcendence. There is no concept of health or damage - just don't fall off the world!

The scripts you see here are those I've mainly worked on throughout our development process. 

The InteractionController deals with the player interacting with their environment such as breaking blocks and collecting candy, as well as placing traps using the candy collected. 

The Candy_GameManager handles the game win/fail states and checks to see if the player has won the level. Each time the player reaches a 100% happiness on their meter, they transcend towards the heavens. In that case, a new level is generated that is bigger and has less candy than the previous one. This one handles happiness levels based on the collected candy type. If the player falls off the level (fail state), the game resets.

In the cubes folder you see three scripts: 
  CubeHealth is pretty self-explanatory. Like Minecraft, each cube has a certain life-span that is reduced each time the player "hits" the block. They represent non-edible cubes, i.e. those that do not provide any candy upon breaking.
  EdibleCubeHealth is same to CubeHealth, except these provide candy to the player if broken, in which case they increase the player's happiness based on the type of cube it is.
  MineExplosion is a simple script related to mines as traps in the game. The mine explosion is meant to push the enemies off on a radius off the location the mine was placed. 

