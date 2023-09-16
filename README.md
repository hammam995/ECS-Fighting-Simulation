# ECS-Fighting-Simulation
Creating a fighting simulation with ECS design Pattern

Battle simulator is a game where two teams fight it out. It consists of two teams, Team Blue and 
Team Red. Team Blue is your team, and Team Red is the opponent team. Each team consists of 5 
units.

Technical Specs

- using ECS design pattern for optimization
- using Scriptable object



Unit in-game.
Each unit in-game has the following properties
1. HP: Health points, when they become zero unit dies.
2. Attack: Damage a unit does.
3. Attack Speed: Speed at which a unit attacks.
4. Attack Range: Range at which unit attacks.
5. Movement Speed: Speed at which a unit moves



AI Design:
1. All units can see every unit in the game, friendly and enemy both.
2. Each unit will choose a random target from the enemy and start the movement towards the 
acquired target
3. The unit will move toward that target until it comes into attack range.
4. The unit will start the attack and do damage based on the attack speed and attack.
5. It keeps on doing damage until the unit dies, and then it will acquire another random target 
until all the enemy unit dies.


- Should be able to change all the unit properties and the initial setup.
- The right side is the enemy team and is also read from the config file.
- You can also change the enemy team by clicking on Team 1, Team 2, team 3, etc.
o The list is also read from the config file, and we should be able to add new teams, and 
the list should adapt to that.
- The list should also support scrolling if the content is more than the viewport.
- The Start battle should hide the UI, and the battle will begin.
- Each unit has the HP number of top and should decrease as they receive damage.
- THE UNIT WILL DISAPPEAR when HP is zero;
- After the battle end, it should display a victor screen and the option to go to the main menu 
to start another battle
