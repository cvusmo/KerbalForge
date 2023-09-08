
[Toggle Notifications Bug 1 FIXED] 
    [Info   :Toggle Notifications] Loaded
    [Error :Toggle Notifications] HarmonyLib.HarmonyException: Patching exception in method null ---> System.ArgumentException: Undefined target method for patch method static bool ToggleNotifications.AssistantToTheAssistantPatchManager+ToggleNotificationPatch::Prefix(KSP.Game.NotificationEvents __instance, KSP.Game.GameStateMachine stateMachine, KSP.Game.NotificationUI OnGamePauseToggled)
  at HarmonyLib.PatchClassProcessor.PatchWithAttributes (System.Reflection.MethodBase& lastOriginal) [0x00047] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.PatchClassProcessor.Patch () [0x0006a] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
   --- End of inner exception stack trace ---
  at HarmonyLib.PatchClassProcessor.ReportException (System.Exception exception, System.Reflection.MethodBase original) [0x0006c] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.PatchClassProcessor.Patch () [0x00095] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.Harmony.<PatchAll>b__11_0 (System.Type type) [0x00007] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.CollectionExtensions.Do[T] (System.Collections.Generic.IEnumerable`1[T] sequence, System.Action`1[T] action) [0x00014] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.Harmony.PatchAll (System.Reflection.Assembly assembly) [0x00006] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.Harmony.CreateAndPatchAll (System.Reflection.Assembly assembly, System.String harmonyInstanceId) [0x0001e] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at ToggleNotifications.ToggleNotificationsPlugin.OnInitialized () [0x000a0] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:89 
  at SpaceWarp.Patching.LoadingActions.InitializeModAction.DoAction (System.Action resolve, System.Action`1[T] reject) [0x00000] in <903e43f16c5745be9d0911dfe1510a8f>:0 

^-- fixed with 0.2.1 release
x
x
[Toggle Notifications Bug 2 FIXED] 
    [Info   :Toggle Notifications] Loaded
    [Error  :Toggle Notifications] HarmonyLib.HarmonyException: Patching exception in method null ---> System.ArgumentException: Undefined target method for patch method static bool ToggleNotifications.AssistantToTheAssistantPatchManager+PauseStateChangedPatch::Prefix(KSP.Messages.PauseStateChangedMessage __instance)
  at HarmonyLib.PatchClassProcessor.PatchWithAttributes (System.Reflection.MethodBase& lastOriginal) [0x00047] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.PatchClassProcessor.Patch () [0x0006a] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
   --- End of inner exception stack trace ---
  at HarmonyLib.PatchClassProcessor.ReportException (System.Exception exception, System.Reflection.MethodBase original) [0x0006c] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.PatchClassProcessor.Patch () [0x00095] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.Harmony.<PatchAll>b__11_0 (System.Type type) [0x00007] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.CollectionExtensions.Do[T] (System.Collections.Generic.IEnumerable`1[T] sequence, System.Action`1[T] action) [0x00014] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at HarmonyLib.Harmony.PatchAll (System.Reflection.Assembly assembly) [0x00006] in <474744d65d8e460fa08cd5fd82b5d65f>:0 
  at ToggleNotifications.AssistantToTheAssistantPatchManager.ApplyPatches () [0x0000c] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\AssistantToTheAssistantPatchManager.cs:82 
  at ToggleNotifications.ToggleNotificationsPlugin.OnInitialized () [0x000a0] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:89 
  at SpaceWarp.Patching.LoadingActions.InitializeModAction.DoAction (System.Action resolve, System.Action`1[T] reject) [0x00000] in <903e43f16c5745be9d0911dfe1510a8f>:0 
   
    WORKS [HarmonyPatch(typeof(PauseStateChangedMessage))]

        public static class PauseStateChangedPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("PauseStateChangedMessage")]
            public static bool Prefix(PauseStateChangedMessage IsPaused)
            {
                Logger.LogInfo("Prefix Loaded for PauseStateChangedMessage");
                Logger.LogInfo("Paused: " + IsPaused);
                // Perform your logic here
                return false;
            }
        }

    WORKS BUT NO LOAD [HarmonyPatch(typeof(MessageCenter))]
        public static class MessageCenterPublishPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            public static bool Prefix(System.Type type, MessageCenterMessage message)
            {
                if (type == typeof(PauseStateChangedMessage))
                {
                    Logger.LogInfo("Prefix Loaded for PauseStateChangedMessage");
                    Logger.LogInfo("Paused: " + ((PauseStateChangedMessage)message).Paused);
                    return false; 
                }
                return true; 
            }
        }

    WORKS BUT NO LOAD
        [HarmonyPatch(typeof(MessageCenter))]
        public static class MessageCenterPublishPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            public static bool Prefix(System.Type type, MessageCenterMessage message)
            {
                if (type == typeof(PauseStateChangedMessage))
                {
                    PauseStateChangedMessage pauseMessage = (PauseStateChangedMessage)message;
                    if (pauseMessage.Paused)
                    {
                        Logger.LogInfo("Game is paused");
                        return false;
                    }
                    else
                    {
                        Logger.LogInfo("Game is unpaused");
                        return true;
                    }
                }
                return true;
            }
        }

     [LOG 01:10:42.834] [System] Loading UI Toolkit completed in 0.0129s.
    [EXC 01:10:42.854] NullReferenceException: Object reference not set to an instance of an object
	ToggleNotifications.AssistantToTheAssistantPatchManager+SetPauseVisiblePatch.Prefix (KSP.Game.UIManager __instance, System.Boolean isVisible) (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/AssistantToTheAssistantPatchManager.cs:59)
	(wrapper dynamic-method) KSP.Game.UIManager.DMD<KSP.Game.UIManager::SetPauseVisible>(KSP.Game.UIManager,bool)
	KSP.Game.UIManager.HidePause () (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
	KSP.Game.UIManager.InitializeSystems () (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
	KSP.Game.UIManager+<>c__DisplayClass100_0.<LoadUI>b__0 (System.Object sender, System.EventArgs args) (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
	KSP.Game.Flow.SequentialFlow.NextFlowAction () (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
	KSP.Game.Flow.SequentialFlow.Update () (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
    [WRN 01:10:42.970] [UI] Failed to add instrument to UIFlightHud's internal tracking - any management of the instrument through the UIFlightHud API will fail
    [EXC 01:10:43.884] NullReferenceException: Object reference not set to an instance of an object
	KSP.Sim.impl.KerbalBehavior.OnDestroy () (at <ef61a348eb874c99b6bdcbcf875cc384>:0)
    ERR 01:15:49.794] [System] Action (Loading UI Manager) timed out. (300s)

    [LOG 01:15:49.795] [Debug] [SaveLoadManager]: Loading failed [316.19s].
    [ERR 01:15:49.796] [Debug] SaveLoadManager.TriggerFailure(): Called with failure code = Error_SomeUnknownError!!!

    [LOG 01:19:13.612] [Lighting] Removing CelestialBody Flight: Kerbol
    [LOG 01:19:13.613] [Lighting] Removing CelestialBody Flight: Kerbin
    [ERR 01:19:13.614] Invalid memory pointer was detected in ThreadsafeLinearAllocator::Deallocate!

    [Info   :Toggle Notifications] Loaded
    [Info   :Toggle Notifications] Option 1: Enabled
        [Info   : Unity Log] [System] Initialization for plugin Toggle Notifications completed in 0.0099s.
        [Error  : Unity Log] ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
    Parameter name: index
    Stack trace:
    System.ThrowHelper.ThrowArgumentOutOfRangeException (System.ExceptionArgument argument, System.ExceptionResource resource) (at <695d1cc93cca45069c528c15c9fdd749>:0)
    System.ThrowHelper.ThrowArgumentOutOfRangeException () (at <695d1cc93cca45069c528c15c9fdd749>:0)
    ToggleNotifications.TNTools.UI.TabsUI.Init () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/TNTools/UI/TabsUI.cs:59)
    ToggleNotifications.ToggleNotificationsUI.Update () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsUI.cs:39)
    ToggleNotifications.ToggleNotificationsPlugin.Update () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsPlugin.cs:113) 
    
    NullReferenceException - This is usually thrown when you try to access a member of an object that is set to null. In this case, it seems like the methods SetPauseVisiblePatch.Prefix, HidePause, InitializeSystems, and KerbalBehavior.OnDestroy are trying to access a member of an object that is not initialized.

    Failed to add instrument to UIFlightHud's internal tracking - This warning indicates that an instrument couldn't be added to the UI for tracking. This could be due to an invalid reference or a failure in the underlying system.

    Action (Loading UI Manager) timed out - The system was unable to load the UI manager within the expected time frame. This could be due to an underlying issue that is blocking the operation.

    Loading failed - The system failed to load a component or resource, possibly due to one or more of the previous issues.

    Invalid memory pointer was detected in ThreadsafeLinearAllocator::Deallocate - This error usually signifies an attempt to deallocate a memory pointer that is invalid or has already been deallocated.



    FIXED THE ISSUE [HarmonyPatch(typeof(UIManager))]
        public static class SetPauseVisiblePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetPauseVisible")]
            public static bool Prefix(UIManager __instance, bool isVisible)
            {
                if (Logger != null)
                {
                    Logger.LogInfo("Prefix Loaded for SetPauseVisible");
                    Logger.LogInfo("IsVisible: " + isVisible);
                }

                if (isVisible)
                {
                    return false;
                }

                return true; 
            }
        }











x
x
[Toggle Notifications Bug 3 FIXED] 
 [Error 2 :Toggle Notifications] Parameter name: index
 [Error  : Unity Log] ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
 Parameter name: index
 Stack trace:
 System.ThrowHelper.ThrowArgumentOutOfRangeException (System.ExceptionArgument argument, System.ExceptionResource resource) (at <695d1cc93cca45069c528c15c9fdd749>:0)
 System.ThrowHelper.ThrowArgumentOutOfRangeException () (at <695d1cc93cca45069c528c15c9fdd749>:0)
 ToggleNotifications.TNTools.UI.TabsUI.Init () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/TNTools/UI/TabsUI.cs:59)
 ToggleNotifications.ToggleNotificationsUI.Update () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsUI.cs:39)
 ToggleNotifications.ToggleNotificationsPlugin.Update () (at C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsPlugin.cs:113)

  need to look at TabsUI. Right now I set Init to do nothing.
x
x
[Toggle Notifications Bug 4 FIXED] 
  [Error 3 :Toggle Notifications] NullReferenceException: Object reference not set to an instance of an object
  at ToggleNotifications.ToggleNotificationsUI.get_RefreshState () [0x00043] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:55 
  at ToggleNotifications.ToggleNotificationsUI.OnGUI () [0x00008] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:325 
  at ToggleNotifications.ToggleNotificationsPlugin.FillWindow (System.Int32 windowID) [0x0005c] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:163 
  at UnityEngine.GUILayout+LayoutedWindow.DoWindow (System.Int32 windowID) [0x00072] in <2b3e92e60405495d8f74d7db1fa137b9>:0 
  at UnityEngine.GUI.CallWindowDelegate (UnityEngine.GUI+WindowFunction func, System.Int32 id, System.Int32 instanceID, UnityEngine.GUISkin _skin, System.Int32 forceRect, System.Single width, System.Single height, UnityEngine.GUIStyle style) [0x0007d] in <2b3e92e60405495d8f74d7db1fa137b9>:0 

 Don't know how it got fixed

x
x
[Toggle Notifications Bug PENDING 5]
    [Info   :ToolbarBackend] Added appbar button: BTN-ToggleNotificationsFlight
    Uploading Crash Report
    NullReferenceException: Object reference not set to an instance of an object
  at KSP.Sim.impl.KerbalBehavior.OnDestroy () [0x0001b] in <ef61a348eb874c99b6bdcbcf875cc384>:0 

    object being accessed by KerbalBehavior.OnDestroy(). Examine code in method to identify object causing error.


    Initial isWindowOpen value: True
    No "HideBentoCanvasGroup" Action Bound on UIAction_Void_Toggle "BTN-Resource-Manager(Clone)"
    UnityEngine.GUISkin must be instantiated using the ScriptableObject.CreateInstance method instead of new GUISkin.

     [FIX]Skin = ScriptableObject.CreateInstance<GUISkin>();

    custom styles is null
    Attempting window
    Did it work?
        problem with the custom styles being used in code.



    Just make sure that your custom UI elements and logic in the FillWindow method are properly implemented and functioning as intended.


x
x
[Toggle Notifications Bug PENDING 6]
    Uploading Crash Report
    NullReferenceException: Object reference not set to an instance of an object
  at ToggleNotifications.ToggleNotificationsUI.get_RefreshState () [0x00043] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:55 
  at ToggleNotifications.ToggleNotificationsUI.OnGUI () [0x00008] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:314 
  at ToggleNotifications.ToggleNotificationsPlugin.FillWindow (System.Int32 windowID) [0x0005c] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:158 
  at UnityEngine.GUILayout+LayoutedWindow.DoWindow (System.Int32 windowID) [0x00072] in <2b3e92e60405495d8f74d7db1fa137b9>:0 
  at UnityEngine.GUI.CallWindowDelegate (UnityEngine.GUI+WindowFunction func, System.Int32 id, System.Int32 instanceID, UnityEngine.GUISkin _skin, System.Int32 forceRect, System.Single width, System.Single height, UnityEngine.GUIStyle style) [0x0007d] in <2b3e92e60405495d8f74d7db1fa137b9>:0 

    Object reference not set to an instance of an object in ToggleNotificationsUI.get_RefreshState() This error indicates that an object is being accessed before it is properly initialized. You need to check the code in the ToggleNotificationsUI class, specifically the get_RefreshState() method, and ensure that any required objects are instantiated or assigned before accessing them.


    Initial isWindowOpen value: False
    No "HideBentoCanvasGroup" Action Bound on UIAction_Void_Toggle "BTN-Resource-Manager(Clone)"

    Initial isWindowOpen value: False and No "HideBentoCanvasGroup" Action Bound on UIAction_Void_Toggle "BTN-Resource-Manager(Clone)": These messages may be unrelated to the previous errors. They suggest that the initial value of isWindowOpen is false, and there is an issue with the action binding for the "HideBentoCanvasGroup" action on the UI toggle named "BTN-Resource-Manager(Clone)". You should review the relevant code to ensure that the correct action is bound and the isWindowOpen value is properly set.
x
x
[Toggle Notifications Bug 7 FIXED]
    NullReferenceException: Object reference not set to an instance of an object
  at ToggleNotifications.ToggleNotificationsUI.ShowGUI () [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:56 
  at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:118 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsUI.cs Line: 56)
    Changed MainUI.ShowGUI() call to MainUI.OnGUI() in plugin OnGUI();
x
x
[Toggle Notifications Bug 8 FIXED]
    Uploading Crash Report
    NullReferenceException: Object reference not set to an instance of an object
  at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:118 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsPlugin.cs Line: 118)
    The OnGUI() method in the ToggleNotificationsUI class has been renamed to ShowGUI().
    The OnGUI() method in the ToggleNotificationsPlugin class now calls MainUI.ShowGUI() instead of MainUI.OnGUI().
    The ToggleNotificationsUI class does not need to implement the OnGUI() method anymore.
x
x
[BUILD LOG 0.2.2-dev TEST 0.2.2.1]
    Build started...
        1>------ Build started: Project: toggle_notifications, Configuration: Debug Any CPU ------
    Restored C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\toggle_notifications.csproj (in 269 ms).
    1>C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\obj\Debug\net472\toggle_notifications.AssemblyInfo.cs(14,44,14,74): warning CS0436: The type 'IgnoresAccessChecksToAttribute' in 'C:\Users\nicho\.nuget\packages\bepinex.assemblypublicizer.msbuild\0.4.0\contentFiles\cs\any\IgnoresAccessChecksToAttribute.cs' conflicts with the imported type 'IgnoresAccessChecksToAttribute' in 'MonoMod.Utils, Version=22.3.23.4, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'C:\Users\nicho\.nuget\packages\bepinex.assemblypublicizer.msbuild\0.4.0\contentFiles\cs\any\IgnoresAccessChecksToAttribute.cs'.

    1>toggle_notifications -> C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\bin\Debug\net472\ToggleNotifications.dll
    1>'rm' is not recognized as an internal or external command,    
    1>operable program or batch file.
    1>Invalid path
    1>0 File(s) copied
    1>C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\bin\Debug\net472\ToggleNotifications.dll
    1>1 File(s) copied
    1>C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\bin\Debug\net472\ToggleNotifications.pdb
            1>1 File(s) copied
    1>C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\..\LICENSE
    1>1 File(s) copied
    1>C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\..\README.md
    1>1 File(s) copied
    1>Done building project "toggle_notifications.csproj".
    ========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
    ========== Build started at 1:12 AM and took 00.569 seconds ==========
x
x
[Toggle Notificaitons Bug 9 FIXED]
    Uploading Crash Report
    NullReferenceException: Object reference not set to an instance of an object
    at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:118 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsPlugin.cs Line: 118)

    118:         MainUI.OnGUI(); 
    MainUI object is null.
    [1]Verify that the MainUI object is instantiated and assigned a value before line 118 is executed in the ToggleNotificationsPlugin class. Check the constructor or any initialization methods of the ToggleNotificationsPlugin class to ensure the MainUI object is properly created.

    Removed MainUI.ONGUI() call

    NullReferenceException: Object reference not set to an instance of an object
    at ToggleNotifications.ToggleNotificationsUI.OnGUI () [0x00008] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsUI.cs:251 
    at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:118 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/ToggleNotificationsUI.cs Line: 251)

    251: TNUtility.Instance.RefreshNotifications(); TNUtility.Instance object is null
    [1]Ensure that the TNUtility class has a static property or method named Instance that returns an instance of the TNUtility class. Make sure this property or method is properly implemented and returns a non-null value.

    [2]Check if there are any initialization steps or dependencies required for the TNUtility class. Ensure that these steps are completed before line 251 is executed in the ToggleNotificationsUI class. 
    removed MainUI.OnGUI() call

x
x
[Toggle Notifications Bug 10]
    [Info   :Toggle Notifications] Loaded
    MainUI instantiated
    [Error  :Toggle Notifications] System.NullReferenceException: Object reference not set to an instance of an object
    at ToggleNotifications.ToggleNotificationsPlugin.OnInitialized () [0x000c1] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:83 
    at SpaceWarp.Patching.LoadingActions.InitializeModAction.DoAction (System.Action resolve, System.Action`1[T] reject) [0x00000] in <903e43f16c5745be9d0911dfe1510a8f>:0 

    [Info   :ToolbarBackend] Added appbar button: BTN-ToggleNotificationsFlight
    Uploading Crash Report
    NullReferenceException: Object reference not set to an instance of an object
    at KSP.Sim.impl.KerbalBehavior.OnDestroy () [0x0001b] in <ef61a348eb874c99b6bdcbcf875cc384>:0 

    Initial isWindowOpen value: True
    No "HideBentoCanvasGroup" Action Bound on UIAction_Void_Toggle "BTN-Resource-Manager(Clone)" - no idea
    [Simulation] [UniverseTime] Pause State changed to: True
    Simulation] [UniverseTime] Time Scale Changed to physics:0.000, multiplier:1.000, universe:0.000
    [Simulation] [UniverseTime] Pause State changed to: False
    [Simulation] [UniverseTime] Time Scale Changed to physics:1.000, multiplier:1.000, universe:1.000

x
x
[Toggle Notifications Bug 11]
    Attempting window
    Uploading Crash Report
    IndexOutOfRangeException: Unable to find asset at path "togglenotifications/images/window.png"
    at SpaceWarp.API.Assets.AssetManager.GetAsset[T] (System.String path) [0x0004d] in <903e43f16c5745be9d0911dfe1510a8f>:0 
    at ToggleNotifications.TNTools.UI.AssetsLoader.LoadIcon (System.String path) [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\AssetsLoader.cs:10 
    at ToggleNotifications.TNTools.UI.TNBaseStyle.BuildFrames () [0x0007a] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:146 
    at ToggleNotifications.TNTools.UI.TNBaseStyle.BuildStyles () [0x00018] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:57 
    at ToggleNotifications.TNTools.UI.TNBaseStyle.Init () [0x00000] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:50 
    at ToggleNotifications.TNStyles.Init () [0x00011] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNStyles.cs:21 
    at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00074] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:128 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/TNTools/UI/AssetsLoader.cs Line: 10)



x
x
[Toggle Notifcations 12]
        Attempting window
        Did it work?
    NullReferenceException: Object reference not set to an instance of an object
     at ToggleNotifications.TNTools.UI.TNBaseStyle.SetAllFromNormal (UnityEngine.GUIStyle style) [0x00001] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:359 
     at ToggleNotifications.TNTools.UI.TNBaseStyle.BuildSliders () [0x000c6] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:196 
      at ToggleNotifications.TNTools.UI.TNBaseStyle.BuildStyles () [0x0001e] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:58 
     at ToggleNotifications.TNTools.UI.TNBaseStyle.Init () [0x00000] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNTools\UI\TNBaseStyle.cs:50 
     at ToggleNotifications.TNStyles.Init () [0x00011] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\TNStyles.cs:21 
     at ToggleNotifications.ToggleNotificationsPlugin.OnGUI () [0x00074] in C:\Users\nicho\Source\Repos\cvusmo\Toggle-Notifications\Toggle NotificationsProject\ToggleNotificationsPlugin.cs:128 

    (Filename: C:/Users/nicho/Source/Repos/cvusmo/Toggle-Notifications/Toggle NotificationsProject/TNTools/UI/TNBaseStyle.cs Line: 196)
x
x