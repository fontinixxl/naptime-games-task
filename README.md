# Unity mobile developer test task

This project presents my solution to a test task for a Unity mobile developer position.

## Gameplay Footage (Solution)
![alt text](Recordings/gameplay.gif)
 
## Task Description

### Guidelines:
- **Assets:** Utilize only the provided assets for the game. If additional gameplay elements are necessary, use standard Unity assets.
- **UI Development:** Construct the User Interface (UI) strictly using the provided elements.
- **Audio:** The game is not required to contain music or sound effects.
- **Object Lives:** Each object is allocated 3 lives.
- **Object Spawn:** Objects should spawn in a random (visible) location on the board, ensuring that initial spawn positions can vary from subsequent respawns after an object loses a life.
- **Object Interaction:** Objects should not come into contact with each other.
- **Gameplay Visual Effects:** While visual effects are not pivotal, optimizing the game's performance is crucial.
- **Visual Representation:** Ensure both the object and its projectile have a visual representation.

### Game Description:

**1. Start-Up Window:**
   - Upon launching, a window is displayed prompting the selection of the number of objects (options: 50, 100, 250, 500), as depicted in the provided mock-up.
   - Once an object number is selected, the "Start" button becomes active.

**2. Gameplay Mechanics:**
   - Upon clicking "Start", all selected objects spawn simultaneously.
   - Each object rotates randomly between 0 and 360 degrees at random intervals (0 - 1 second).
   - Every second, each object fires a projectile in its current facing direction.
   - A projectile may hit one of the spawned objects (1 out of 50/100/250/500). Once struck, the object loses a life and vanishes from the board.
   - If the object has remaining lives, it respawns on the board after a 2-second interval and resumes the above mechanics.
   - Gameplay continues until only one object remains on the board.

**3. Game Conclusion:**
   - Once the game concludes, a "Main Menu" button appears, offering the option to restart the game (as shown in the mockup) and redirects players to the initial object-number selection window.

## Implementation Details

- Game architecture is using a Event-driven approach to decouple systems as much as possible. However, for simplicity Game Manager and PoolManger use a Singleton Pattern.
- The objects are spawned randomly in a grid-based pattern, with a randomize offset for each spawned object, so it looks more "natural". Thanks to that, and the usage of an extra data structure to keep track of "used positions", the spawning algorithm is very efficient.
- Due to the large amount of objects on screen (specially bullets), an Object Pooling Pattern Solution has been implemented for both Projectiles and Objects (Shooters).
- For the sake of this task I opted out for a Single Scene pattern, but the Game Architecture is layed out to work in a multi-scene setup.
- Further optimizations could have been implemented (DOTS), however since the performance was already good in different devices I didn't go further.
- To avoid waiting a lot of time for the Game Over Screen, I changed the game over condition to **90% of objects destroyed**.
- In order to easily visualize the lifetime circle of an Object, I added a lives-colour-pattern `{green = 3 lives, yellow = 2 lives, read = 1 live}`
- I added a FPS visualizer on the top-left corner, to check performance easily.

Author: Gerard Cuello Adell
Email: gerard.cuello@proton.me
