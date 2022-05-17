// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Microsoft.Azure.SpatialAnchors.Unity.Examples
{
    public class AzureSpatialAnchorsFindAnchor : DemoScriptBase
    {
        public ObjectManager ObjectManager;
        internal enum AppState
        {
            DemoStepCreateSession = 0,
            DemoStepConfigSession,
            DemoStepStartSession,
            DemoStepCreateLocalAnchor,
            DemoStepSaveCloudAnchor,
            DemoStepSavingCloudAnchor,
            DemoStepStopSession,
            DemoStepDestroySession,
            DemoStepCreateSessionForQuery,
            DemoStepStartSessionForQuery,
            DemoStepLookForAnchor,
            DemoStepLookingForAnchor,
            DemoStepDeleteFoundAnchor,
            DemoStepStopSessionForQuery,
            DemoStepComplete,
            DemoStepBusy
        }

        private readonly Dictionary<AppState, DemoStepParams> stateParams = new Dictionary<AppState, DemoStepParams>
        {
            { AppState.DemoStepCreateSession,new DemoStepParams() { StepMessage = "Next: Create Azure Spatial Anchors Session", StepColor = Color.clear }},
            { AppState.DemoStepConfigSession,new DemoStepParams() { StepMessage = "Next: Configure Azure Spatial Anchors Session", StepColor = Color.clear }},
            { AppState.DemoStepStartSession,new DemoStepParams() { StepMessage = "Next: Start Azure Spatial Anchors Session", StepColor = Color.clear }},
            { AppState.DemoStepCreateLocalAnchor,new DemoStepParams() { StepMessage = "Tap a surface to add the Local Anchor.", StepColor = Color.blue }},
            { AppState.DemoStepSaveCloudAnchor,new DemoStepParams() { StepMessage = "Next: Save Local Anchor to cloud", StepColor = Color.yellow }},
            { AppState.DemoStepSavingCloudAnchor,new DemoStepParams() { StepMessage = "Saving local Anchor to cloud...", StepColor = Color.yellow }},
            { AppState.DemoStepStopSession,new DemoStepParams() { StepMessage = "Next: Stop Azure Spatial Anchors Session", StepColor = Color.green }},
            { AppState.DemoStepCreateSessionForQuery,new DemoStepParams() { StepMessage = "Next: Create Azure Spatial Anchors Session for query", StepColor = Color.clear }},
            { AppState.DemoStepStartSessionForQuery,new DemoStepParams() { StepMessage = "1", StepColor = Color.clear }},
            { AppState.DemoStepLookForAnchor,new DemoStepParams() { StepMessage = "2", StepColor = Color.clear }},
            { AppState.DemoStepLookingForAnchor,new DemoStepParams() { StepMessage = "Looking for Anchor...", StepColor = Color.clear }},
            { AppState.DemoStepDeleteFoundAnchor,new DemoStepParams() { StepMessage = "", StepColor = Color.yellow }},
            { AppState.DemoStepStopSessionForQuery,new DemoStepParams() { StepMessage = "Next: Stop Azure Spatial Anchors Session for query", StepColor = Color.grey }},
            { AppState.DemoStepComplete,new DemoStepParams() { StepMessage = "Next: Restart demo", StepColor = Color.clear }},
            { AppState.DemoStepBusy,new DemoStepParams() { StepMessage = "Processing...", StepColor = Color.clear }}
        };

        private AppState _currentAppState = AppState.DemoStepCreateSession;

        AppState currentAppState
        {
            get
            {
                return _currentAppState;
            }
            set
            {
                if (_currentAppState != value)
                {
                    Debug.LogFormat("State from {0} to {1}", _currentAppState, value);
                    _currentAppState = value;
                    if (spawnedObjectMat != null)
                    {
                        spawnedObjectMat.color = stateParams[_currentAppState].StepColor;
                    }

                    if (!isErrorActive)
                    {
                        feedbackBox.text = stateParams[_currentAppState].StepMessage;
                    }
                }
            }
        }

        private string currentAnchorId = "";
        public string _currentAnchorId
        {
            get { return currentAnchorId; }
            set { Debug.Log("nope"); }
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any
        /// of the Update methods are called the first time.
        /// </summary>
        public override void Start()
        {
            Debug.Log(">>Azure Spatial Anchors Demo Script Start");

            base.Start();

            if (!SanityCheckAccessConfiguration())
            {
                return;
            }
            feedbackBox.text = stateParams[currentAppState].StepMessage;

            Debug.Log("Azure Spatial Anchors Demo script started");
        }

        protected override void OnCloudAnchorLocated(AnchorLocatedEventArgs args)
        {
            base.OnCloudAnchorLocated(args);

            if (args.Status == LocateAnchorStatus.Located)
            {
                currentCloudAnchor = args.Anchor;

                UnityDispatcher.InvokeOnAppThread(() =>
                {
                    Pose anchorPose = currentCloudAnchor.GetPose();
                    SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);
                    currentAppState = AppState.DemoStepDeleteFoundAnchor;
                });
            }
        }

        protected override bool IsPlacingObject()
        {
            return currentAppState == AppState.DemoStepCreateLocalAnchor;
        }

        protected override Color GetStepColor()
        {
            return stateParams[currentAppState].StepColor;
        }

        protected override async Task OnSaveCloudAnchorSuccessfulAsync()
        {
            await base.OnSaveCloudAnchorSuccessfulAsync();

            Debug.Log("Anchor created, yay!");

            currentAnchorId = currentCloudAnchor.Identifier;

            currentAppState = AppState.DemoStepStopSession;
        }

        protected override void OnSaveCloudAnchorFailed(Exception exception)
        {
            base.OnSaveCloudAnchorFailed(exception);

            currentAnchorId = string.Empty;
        }

        public void test()
        {
            ObjectManager.GetObjects();
        }

        public async override Task AdvanceDemoAsync()
        {
            switch (currentAppState)
            {
                case AppState.DemoStepCreateSession:
                    currentAppState = AppState.DemoStepBusy;
                    if (CloudManager.Session == null)
                    {
                        await CloudManager.CreateSessionAsync();
                    }
                    currentAnchorId = "";
                    currentCloudAnchor = null;
                    currentAppState = AppState.DemoStepCreateSessionForQuery;
                    AdvanceDemoAsync(); //test if I can do this without a button
                    break;


                case AppState.DemoStepCreateSessionForQuery:
                    ConfigureSession();
                    currentAppState = AppState.DemoStepStartSessionForQuery;
                   AdvanceDemoAsync();
                    break;
                case AppState.DemoStepStartSessionForQuery:
                    currentAppState = AppState.DemoStepBusy;
                    await CloudManager.StartSessionAsync();
                    currentAppState = AppState.DemoStepLookForAnchor;
                   AdvanceDemoAsync();
                    break;
                case AppState.DemoStepLookForAnchor:
                    currentAppState = AppState.DemoStepLookingForAnchor;
                    if (currentWatcher != null)
                    {
                        currentWatcher.Stop();
                        currentWatcher = null;
                    }
                    currentWatcher = CreateWatcher();
                    if (currentWatcher == null)
                    {
                        Debug.Log("Either cloudmanager or session is null, should not be here!");
                        feedbackBox.text = "YIKES - couldn't create watcher!";
                        currentAppState = AppState.DemoStepLookForAnchor;
                    }
                    //ObjectManager.GetObjects();
                    //DebugTXT.text = "HEre?";
                    AdvanceDemoAsync();
                    break;
                case AppState.DemoStepLookingForAnchor:
                    test();

                    break;
                case AppState.DemoStepDeleteFoundAnchor:
                    DebugTXT.text = "HEre?3";
                    currentAppState = AppState.DemoStepBusy;
                    //DebugTXT.text = "HEre?4";
                    currentAppState = AppState.DemoStepStopSessionForQuery;
                    AdvanceDemoAsync();
                    break;
                case AppState.DemoStepStopSessionForQuery:
                    currentAppState = AppState.DemoStepBusy;
                    CloudManager.StopSession();
                    currentWatcher = null;
                    DebugTXT.text = "HEre?4";
                    currentAppState = AppState.DemoStepComplete;
                    AdvanceDemoAsync();
                    break;
                case AppState.DemoStepComplete:
                    currentAppState = AppState.DemoStepBusy;
                    currentCloudAnchor = null;
                    DebugTXT.text = "HEre?5";
                    CleanupSpawnedObjects();
                    currentAppState = AppState.DemoStepCreateSession;
                    break;
                case AppState.DemoStepBusy:
                    break;
                default:
                    Debug.Log("Shouldn't get here for app state " + currentAppState.ToString());
                    break;

            }
        }

        public async void test2()
        {
            await StopSessionAsync();
        }

        public async Task StopSessionAsync()
        {
            Debug.Log("!!! BEFORE STOP");
            CloudManager.StopSession();
            await CloudManager.ResetSessionAsync();
            Debug.Log("!!! STOP");
        }

        public void GetAnchorKey(string key)
        {
            AnchorKey = key;
            DebugTXT.text = AnchorKey + " GetAnchorKey";
        }

        public string AnchorKey;
        public Text DebugTXT;
        private void ConfigureSession()
        {
            DebugTXT.text = "ConfigureSession";
            List<string> anchorsToFind = new List<string>();
            if (currentAppState == AppState.DemoStepCreateSessionForQuery)
            {
                anchorsToFind.Add(AnchorKey);
                DebugTXT.text = AnchorKey;
            }

            SetAnchorIdsToLocate(anchorsToFind);
        }

    }
}

