# Still Here
## What is the game about
-   Still Here is a game about the life of a 60 year old person with mid-stage dementia and how they would do their daily tasks

## Controls for the game
-   Left joystick for movement
-	Right joystick for camera rotation (every 45 degrees)
-	Right joystick upwards to teleport around the place
-	Grab button for grabbing stuff
-	Trigger button to act as a “push”

# Game Features
## Game Tasks
### Washing machine task
-	Take the clothes from the second bedroom
-	Put clothes in the washing machine
-	Start the washing machine by pressing the button
-	Take out clothes after finish washing

### Take medication
-	Grab pill bottle
-	Tilt and shake pill bottle to get pills
-	Eat 2 pills

###	Throw trash
-	Grab the trash bag
-	Go to the trash chute outside
-	Open the trash chute and throw the trash bag
-	Close the trash chute door

###	Watering plants
-	Grab the watering can
-	Tilt the watering can on the plant
-	Water until complete particles play

###	Fix circuit breaker
-	Open the circuit breaker
-	Flip a switch
-	Close circuit breaker

###	Feed fishes
-	Open the fish tank
-	Grab the fish feed
-	Tilt and shake the feed until bubble particles show up

###	Wash dishes
-   Turn on the tap by interacting it with trigger
-	Hold the dirty dishes under the water for 5 seconds
-	Rinse and repeat until completion

## Gameplay features
-	Some tasks will have their completion state reverted and the order shuffled after a while to simulate “forgetting”
-	Taking more than 2 pills ends the game
-	Interactable doors that do not bug out that much
-	A clock that tracks how long the run has gone for
-	A bed that ends the day
-	Objects can return to their starting position when thrown out
-	Some objects can “relocate” to other places

# Bugs/Limitations
-	Player can go higher than usual due to gravity not working apparently. There is a band aid fix of reducing the step offset to stop the player from climbing furniture
-	If you pull the clothes far enough from the washing machine after putting it in, it will go back to its starting position

# Credits/References
- Short circuit SFX: https://freesound.org/s/576590/ 
- Circuit breaker SFX: https://freesound.org/s/381447/ 
- Washing machine SFX: https://pixabay.com/sound-effects/search/washing%20machine%20tune/
- Rain SFX: https://pixabay.com/sound-effects/search/rain/
- Trees Unity Asset: https://assetstore.unity.com/packages/3d/vegetation/big-oak-tree-free-279431

Copilot and ChatGPT were used partially to write the following scripts:
- FishBehaviour.cs
- FishFeedingTracker.cs
- FoodTrigger.cs
- BreakerController.cs
- DishCleanable.cs
- TapWaterTrigger.cs
- WashingMachineController.cs
- WaterPlantTracker.cs
- WaterPourTilt.cs
- PourController.cs