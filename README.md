# RollyVortexRemake
 Prototype for a hyper casual game (enhenced) - Test Voodoo

The goal of this test was to improve the retention of the game and make it more appealing.
I had one week to do this test and I spent 25 hours of work (work environment set up, developement, testing, debuging and build).

# DEMO VIDEO
Here is a short demo clip


# Gameplay
In the main menu, the player can enter the skin section to select a color.
The player can also check the missions he has completed (not implemented yet).
The player can start a run by taping anywhere on the screen (except buttons).

The goal in a run is to make the best score and gain soft currency.

The player will control a sphere by sliding his finger to the left or to the right.
The sphere will move around a circle and the player moves the sphere to avoid obstacles and collect bonuses.
The player will gain points to increase his score for each obstacles avoided.
For exemple, an obstacle can be made of multiple blocks. If the player avoid this obstacle, he will gain a point for each blocks.
An obstacle can be made of a maximum of 13 blocks.

The difficulty increases as time passes, more obstacles are spawned and they are faster.
There are 2 types of bonuses :
-one will give currency
-one will trigger a power

There are 2 ways to trigger a power :
-gain 100 score points
-collect the power bonus
The effect of the power destroys all the visible obstacles in the level and increase the score.

If the player is hit by an obstacle, the player will be given the choice to revive the player (by watching an ad). The player has 5 seconds to make this choice.
If the timer reaches 0, the player will be lead to the game over screen. The player can revive only once per run.

At the end of the game the current score will be shown along side with the currency he gain during the run.
If the player has beaten his high score, the high score is updated.

The game currently saves the currency, the high score and the vibration/sound options.

# Visual aspect

I wanted to give the project a real identity. I chose a 'neon theme' that we can find in Tron with flashy colored lines and a specific selction of colors.
I chose a neon blue for the main color and picked some colors that could get along with it such as night blue, pink/purple, cyan and emerald green.
The neon theme is highlighted with the rings in which the player moves and dodges obstacles. There is also a plane with a grid pattern in the background with cyan stripes.
The skybox is also cyan and project a light blue color on the bottom side of the player.
I also chose a nice font which goes well with this theme.
And lastly, I added a particle effect on the player so when the player moves, we can see a lightning effect left behind the player. This partcile effect takes the same color than the player's skin.

# Sound
I picked sounds that matches the neon theme. The background music is a soft techno sound and there are sound effects when collecting the bonuses, when destroying obstacles with power and when the player dies.
I edited all sound effects in Audacity so they go well with the current background music.

# Retention
I added a daily reward system which is a classic game element in the hyper casual field.
For now, this daily reward system gives soft currency.

# Futur possible improvements
The game can be improved a lot. I only had one week to make it.

-Give a use to the soft currency.
-Add different obstacles.
-Add different bonuses.
-Implement the mission system (which is just an achivement system).
-Extend the current skin system (for now, only the color changes).

# Sources
I used neon assets from the Unity asset store :
https://assetstore.unity.com/packages/templates/packs/neonsphere-starter-pack-free-158198
