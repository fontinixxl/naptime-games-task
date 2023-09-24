# Naptime.games Test Task

Author: Gerard Cuello Adell

## Disclaimer
- To avoid waiting a lot of time for the Game Over Screen, I changed the game over condition to **90% of objects destroyed**.
- In order to easily visualize the lifetime circle of an Object, I added a lives-colour-pattern `{green = 3 lives, yellow = 2 lives, read = 1 live}`
- I added a FPS visualizer on the top-left corner, to check performance easily.

## Implementation Details
- Game architecture is using a Event-driven approach to decouple systems as much as possible. However, for simplicity Game Manager and PoolManger use a Singleton Pattern.
- The objects are spawned randomly in a grid-based pattern, with a randomize offset for each spawned object, so it looks more "natural". Thanks to that, and the usage of an extra data structure to keep track of "used positions", the spawning algorithm is very efficient.
- Due to the large amount of objects on screen (specially bullets), an Object Pooling Pattern Solution has been implemented for both Projectiles and Objects (Shooters).
- For the sake of this task I opted out for a Single Scene pattern, but the Game Architecture is layed out to work in a multi-scene setup.
- Further optimizations could have been implemented (DOTS), however since the performance was already good in different devices I didn't go further.
 
## Gameplay Footage
![alt text](Recordings/gameplay.gif)
