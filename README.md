# SignAR
HoloLens App for detecting, reading and displaying text in the environment

Projects using this code should acknowledge:

An Augmented Reality Sign-reading Assistant for Users with Reduced Vision.
J. Huang, M. Kinateder, M.J. Dunn, W. Jarosz, X. Yang and E.A. Cooper
PLOS One, 14(1), e0210630, 2019

## Requirements
This application is based of the MS HoloToolkit and in particular it uses the spatial mapping abilities. Basic familiarity with developing and deploying for MS HoloLens is assumed. 

To get started with HoloLens, follow tutorials on the [HoloAcademy website](https://developer.microsoft.com/en-us/windows/mixed-reality/academy) (Holograms 101, 212, and 230 in particular).

For ease of use, the necessary assets from the HoloToolkit are included.

This application uses the Google Vision API. The application does not run as is and you need an account for the Google Vision API. For more information see [Google Vision](https://cloud.google.com/vision/).

### Software
* See [MS installation checklist](https://developer.microsoft.com/en-us/windows/mixed-reality/install_the_tools)
* [Google Vision API](https://cloud.google.com/vision/)
* Visual Studio 2015 Update 3
* Unity 5.4.0f3
* Microsoft ASP. NET 4.5
* [SimpleJSON] A SimpleJSON C sharp script that will work with this project can be downloaded from (https://blogs.sap.com/2016/06/09/annotating-the-world-using-microsoft-hololens/): The whole SAP project source code is [here](https://s3.eu-central-1.amazonaws.com/hololens-samples/hololens-annotate-world-master.zip). Only the `SimpleJson.cs` file is needed, and should be placed in the `/Assets/Scripts` folder.


make sure project is set up to work with [Github](http://www.studica.com/blog/how-to-setup-github-with-unity-step-by-step-instructions)


### Hardware
- MS HoloLens enabled for [developer mode](https://developer.microsoft.com/en-us/windows/mixed-reality/Using_Visual_Studio.html#enabling_developer_mode)
- Bluetooth clicker (toggles augmentation on and off)

## Deploying the app
1. Clone repo to your computer 
2. Open project in Unity (make sure that the SignAR scene is loaded)
3. Open `ApiManager.cs` and add your Google vision api account information into line 26
4. Build and Deploy (see HoloAcademy for tutorials)

## Using the app
### General App description

The SignAR application will detect text in the direction the user is looking at and place spherical icons wherever text exists. The number of icons to display is restricted to 5 (can be adjusted in Unity).  

Every icon represents a sign containing text in the real world. These icons are color-coded (green for confident, orange for semi-confident, and red for doubtful) and can be selected. 
Once selected, the application will read and display the text stored at that icon, and the icon will disappear. 
See below for a list of modes and voice commands.

The clicker can be used with the application. If the user is gazing at an object, a click will select the object. 
If the user is not gazing at an object and the user previously selected an icon, then the click will deselect the previous icon. Otherwise, a click will tell the HoloLens to detect text if it is manual mode. 



## Modes
* Audio Only Mode (default): when the application detects text, the application will read all of the text in front of the user without showing any icons 
* Icon Mode: when the application detects text, the application will first show icons in the scene. The user can tap on any of the icons, and the application will read/display the text to the user




### Voice Commands

* What's here: Tell the application to search for text in the scene
* Icon mode: Switch to mode to show icons
* Audio only mode: Switch to audio-only mode
* Clear Icons: Delete all icons in the scene (Icon mode only)
* Show me: Reads and displays the text of the icon that is currently gazed at (Icon mode only)
* Read all here: Reads text of all icons that are currently in front of the user
* Hide words: Hides the text of the icon that is currently selected (Icon mode only)


