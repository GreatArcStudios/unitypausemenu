### Unrelated request: please check out my new asset [uDocumentGenerator](https://github.com/GreatArcStudios/uDocumentGenerator)

![alt text](http://i.imgur.com/QvaVqvN.png)
### Setup
Just drag the pause menu prefab from the prefab folder into your scene, and then bring it into view by editing the x value of the PREFAB to 0 , y to 0 and Z to 0. 
### Updating
Completely delete the pause menu **folder** and install the new version. 

If you have a custom prefab or manager you can always keep that version in a different folder and merge the changes from there.

### Things to note
1. Assign the main cam obj as the camera with your image effects like DOF and AO. 
2. The docs are located here: https://github.com/GreatArcStudios/UnityPauseMenuDocs
3. **Special note for 5.4 and above:** you may ignore the _Coroutine Utlities_ folder and or update the script due to Unity 5.4's (and above) addition of a [WaitForSecondsRealTime](https://docs.unity3d.com/ScriptReference/WaitForSecondsRealtime.html)  method.   

### Joystick/Controller support
Using a joystick/controller should work fine. Just remeber to change the highlighted dropdown to Joystick.
![alt text](http://i.imgur.com/Pf7poMk.png)
 
### Enabling pause blur
First, make sure you have the unity image effects package. Then follow the next few steps.

Uncomment the following variable declarations found starting at line ***270***
```csharp
 //Blur Variables
 //Blur Effect Script (using the standard image effects package) 
 public Blur blurEffect;
 //Blur Effect Shader (should be the one that came with the package)
 public Shader blurEffectShader;
 //Boolean for if the blur effect was originally enabled
 public Boolean blurBool;
 ```

 Find and uncomment the following code found starting at line ***349***
 ```csharp
  //set the blur boolean to false;
  blurBool = false;
  //Add the blur effect
  mainCamObj.AddComponent(typeof(Blur));
  blurEffect = (Blur)mainCamObj.GetComponent(typeof(Blur));
  blurEffect.blurShader = blurEffectShader;
  blurEffect.enabled = false;  
  ```
 
  Uncomment the following code found starting at line ***382***
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
   
  Uncomment the following code found starting at line ***465*** .
  ```csharp
  if (blurBool == false)
  {
   blurEffect.enabled = true;
  }  
  ```
And that's it. Just a few steps to enable a blury background on your pause menu!
