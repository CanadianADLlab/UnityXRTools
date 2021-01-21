
Unity Version : 2020.1.17f1

This project contains various tools for VR interactions using the Unity XR plugin.

Tools contained

- Locomotion character controller
- Teleport character controller
- Grabbing both 1 handed and 2 handed
- Interactions with physical objects such as levels, dials and buttons
- Vr interaction with standard unity ui
- Climbing 
- Gun shooting/reloading


Project Setup

I recommend using a unity version that is the same or newer than the unity version listed above. It will most likely work with older versions that unity xr is supported by, 
but just to be safe. Also before using the xr samples for your own project I recommend trying out the test scenes which I explain below.
Once you have the project open all of the sample interactions created will be located under the Project folder.
<br><br>
<img width="828" alt="In this folder" src="https://user-images.githubusercontent.com/46603511/105355759-92392880-5bc0-11eb-9de1-e5d685246e28.png">
<br><br>
The project folder has two main example scenes for you to play around with. Both are located in the scenes folder the first one is just called example scene. This scene
contains interactions for grabbing,shooting,climbing and ui. Also it contains the locomotion character controller to move around.
<br><br>
<img width="733" alt="Example scene" src="https://user-images.githubusercontent.com/46603511/105355587-5bfba900-5bc0-11eb-9715-993ff8186d8a.png">
<br><br>
The next scene is called GenericInteractions and this contains all the physical interactions you can do. 
The scene lets players control a drone in different ways using dials levers and buttons.
<br><br>
<img width="1056" alt="Scene overview" src="https://user-images.githubusercontent.com/46603511/105357868-6f5c4380-5bc3-11eb-99f1-3c0182fda28c.png">
<br><br>
<img width="759" alt="lever" src="https://user-images.githubusercontent.com/46603511/105358102-b9452980-5bc3-11eb-8dbd-fcdc8d45eb5c.png">
<br><br>

An Overview of the Scripts
I'm gonna break down the scripts here and how to set them up to work on your own assets.

<h1>Input</h1>

<h3>XRCrossPlatformInputManager</h3>
Singleton class that contains all the input states for xr. If you need to get input use this script. The functions that will give input are.
<br>
GetInputByButton(ButtonTypes inputButton, ControllerHand hand, bool hold)
<br>
This function takes the ButtonTypes inputbutton which is the input you are listening for (ButtonTypes.Grip would return a grip pressed state).
The ControllerHand is the hand you are looking for if ControllerHand.Left it will return true when left grip is pressed.
And last is the bool hold, if hold is true it will keep returning true while the user is holding the grip, if false it will only return true on button down.
<br>
GetInputUp(ButtonTypes inputButton, ControllerHand hand)
<br>
This behavces the same as the first function but only return input up, I found I rarely use this so it got its own function
<br>
<h3>XRPositionManager</h3>
Singleton script just keeps track of all the vr specific objects like hands,camera and playspace. Useful for checking states an example use would be seeing if the leftcontroller was grabbing. XRPositionManager.Instance.LeftController.GetComponent<Controller>().IsGrabbing
<h3>Controller<h3>
Controller just handles states of the controller. This script is attatched to both the left and right controllers on the character prefabs.
  <br>
  public vars
  <br>
  GameObject ControllerMesh -> the mesh of the controller
  ControllerHand Hand -> the hand the controller is
  bool IsGrabbing -> True if the controller is being used for grabbing
  
  <h1>Movement Scripts</h1>
  <h3>XRCharacterController</h3>
  This handles the locomotion movement of the player.
  <br>
  Public vars
  <br>
  float speed -> The movement speed of the character
  <br>
  float maxVelocityChange -> the max increase in speed in one frame
  <br>
  <br>
   LayerMask JumpRaycastLayers -> Ground layer (Bad var name :( )
  <br>
   float JumpHeight -> How high you jump
   <br>
   FallMultiplier -> how quickly you're fall speed up
   <br>
   float StickToGroundForce -> The amount of force keeping the user on the ground, the lower it is the easily the user can catch air
   <br>
   Camera VRCamera -> The vr vamera
   <br>
   Transform PlayerCollider -> the transform that contains the player collider
   <br>
   
  <h3>XRCharacterTeleport</h3>
  Attatch this script to a controller for teleport controls.
  <br>
  Public vars
  <br>
  ButtonTypes TeleportButton -> The button that needs to be pressed toi enable teleporting
  <br>
  Material TeleportArrowMat -> The material of the object that is used to display the teleport location
  <br>
  LineRender LineRend -> The line renderer that is used for the teleport line
  <br>
  Color ValidColor -> the color of the linerender and the arrowmat when the user is pointing to a valid teleport spot
  <br>
  Color InvalidColor -> the color of the linerender and the arrowmat when the user is pointing to a bad teleport spot
  <br>
  float YOffset -> The height of the arch of the teleport line
  <br>
  float MaxTeleportDistance  -> The max distance a player can point and teleport
  <br>
  LayerMask TeleportRaycastLayers -> The valid teleport layers
  <br>
  

<h1>Grabbing Scripts</h1>

<h3>InteractableObject</h3>

This script enables objects to be grabbed. Attatch to any object with a collider and a rigidbody to enable grabbing.
<br>
Public var descriptions
<br>
int ControllerLayer -> the layer the controller is on, used to distinguish the controller from any other triggers while checking for input.
<br>
bool IsGrabEnabled -> True by default, turn to false to disable the object from being grabbed
<br>
bool HideControllerOnGrab -> if true hides controller or hands from being seen while grabbing
<br>
bool HoldToGrab -> If true you have to hold down the input button to keep grabbing
<br>
ButtonTypes GrabButton -> An enum of the button options to set by default it is grip.
<br>
bool SnapToController -> Snaps center point of object to controller on grab
<br>
bool SnapTonController2Hand -> does the same thing but for the second hand
<br>
float ThrowVelocityMultiplier -> the multiplier of the velocity when you release by default its set to 1 if you wan't throwing to feel more powerful increase the number.
<br>
float ThrowTorqueMultiplier ->  the same thing but for torque

<h3>InteractableSecondaryGrab</h3>
This scripts makes the object look at secondary hand when grabbed. To use attatch to any object that has InteractableObject attatched.
<br>
Public var descriptions
<br>
bool ReverseGrabLook -> just makes the object look in the other direction if true.

<h3>XRInteractableHands</h3>
This scripts enables a hand on a object when grabbed, used to give the illusion that the users hands are holding something. Attatch to a object with InteractableObject.
<br>
Public var descriptions
<br>
GameObejct LeftHandMesh -> the mesh of the left hand that will be turned on. In the example scenes the hand is set up to be holding the object in the scene.
<br>
GameObejct Right -> the mesh of the right hand that will be turned on. In the example scenes the hand is set up to be holding the object in the scene.
<br>
bool IsTwoHands -> if theres a secondary grab enable this lets you attatch to more hands for the secondary attatch
<br>
GameObject SecondaryLeftHandMesh -> secondary mesh that gets enabled upon grabbing left
<br>
GameObject SecondaryRightHandMesh -> secondary mesh that gets enabled upon grabbing right
<br>
<h3>XRMagnetGrab</h3>
Lets users grab things from a distance. Attach this script to the controller that you want to be able to do long distance grabbing.
<br>
Public var descriptions
<br>
ButtonTypes MagneticGrabInput -> The button input to turn on the grabbing, when input is pressed and held it turns on a beam the users can than use to point and grab.
<br>
GameObject GrabIconPrefab -> A UI element that will be displayed when an object is pointed at to be grabbed
<br>
float GrabDistance -> the max distance a user can grab from
<br>
LayerMask GrabLayer -> The layer of interactable objects
<br>
Vector3 RaycastDirection -> The direction of the raycast, by default it's just transform forward
<br>
GameObject TractorBeamPrefab -> The prefab of the pointer when a raycast finds something it stretches the z of the object to point to the item.
<br>
float MinTractorBeamZ -> the min length of the z scale of the pointer/beam
<br>


   


<h1>UI</h1>

<h1>Climbing</h1>


<h1>Physical Interactions</h1>








