![alt text](http://i.imgur.com/QvaVqvN.png)
##Setup
Just drag the pause menu prefab from the prefab folder into your scene, and then bring it into view by editing the x value of the PREFAB to 0 , y to 0 and Z to 0. 
##Updating
Delete the pause menu **folder** and install the new version. 
##Things to note
Assign the main cam obj as the camera with your image effects like DOF and AO. 

The docs are located here: https://github.com/GreatArcStudios/UnityPauseMenuDocs
### Joystick/Controller support
Using a joystick/controller should work fine. Just remeber to change the highlighted dropdown to Joystick.
![alt text](http://i.imgur.com/Pf7poMk.png)
 
### Enbaling pause blur
First, make sure you have the unity image effects package. Then follow the next few steps.
<hr>
Uncomment the following variable declarations found starting at line <b>270</b>
```csharp
 //Blur Variables
 //Blur Effect Script (using the standard image effects package) 
 public Blur blurEffect;
 //Blur Effect Shader (should be the one that came with the package)
 public Shader blurEffectShader;
 //Boolean for if the blur effect was originally enabled
 public Boolean blurBool;
 ```
 <br>
 Find and uncomment the following code found starting at line <b>349</b>
 ```csharp
  //set the blur boolean to false;
  blurBool = false;
  //Add the blur effect
  mainCamObj.AddComponent(typeof(Blur));
  blurEffect = (Blur)mainCamObj.GetComponent(typeof(Blur));
  blurEffect.blurShader = blurEffectShader;
  blurEffect.enabled = false;  
  ```
  <br>
  Uncomment the following code found starting at line <b>382</b> .
  ```csharp
 if (blurBool == false)
  {
  blurEffect.enabled = false;
  }
  else
   {
   //if you want to add in your own stuff do so here
   return;
   } 
   ```
   <br>
  Uncomment the following code found starting at line <b>465</b> .
  ```csharp
  if (blurBool == false)
  {
   blurEffect.enabled = true;
  }  
  ```
And that's it. Just a few steps to enable a blury background on your pause menu!
