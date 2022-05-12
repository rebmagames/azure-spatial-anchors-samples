// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Microsoft.Azure.SpatialAnchors.Unity.Examples
{
    public class AzureSpatialAnchorsBasicDemoScript : DemoScriptBase
    {

        //public Text DebugTXT;
        public RoomManager roomManager;
        public bool isdone;
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
            { AppState.DemoStepCreateSession,new DemoStepParams() { StepMessage = "", StepColor = Color.clear }},
            { AppState.DemoStepConfigSession,new DemoStepParams() { StepMessage = "", StepColor = Color.clear }},
            { AppState.DemoStepStartSession,new DemoStepParams() { StepMessage = "", StepColor = Color.clear }},
            { AppState.DemoStepCreateLocalAnchor,new DemoStepParams() { StepMessage = "Tap a surface to add the Local Anchor.", StepColor = Color.blue }},
            { AppState.DemoStepSaveCloudAnchor,new DemoStepParams() { StepMessage = "", StepColor = Color.yellow }},
            { AppState.DemoStepSavingCloudAnchor,new DemoStepParams() { StepMessage = "Saving local Anchor to cloud...", StepColor = Color.yellow }},
            { AppState.DemoStepStopSession,new DemoStepParams() { StepMessage = "", StepColor = Color.green }},
            { AppState.DemoStepCreateSessionForQuery,new DemoStepParams() { StepMessage = "Anchor has been saved", StepColor = Color.clear }},
            { AppState.DemoStepStartSessionForQuery,new DemoStepParams() { StepMessage = "Next: Start Azure Spatial Anchors Session for query", StepColor = Color.clear }},
            { AppState.DemoStepLookForAnchor,new DemoStepParams() { StepMessage = "Next: Look for Anchor", StepColor = Color.clear }},
            { AppState.DemoStepLookingForAnchor,new DemoStepParams() { StepMessage = "Looking for Anchor...", StepColor = Color.clear }},
            { AppState.DemoStepDeleteFoundAnchor,new DemoStepParams() { StepMessage = "Next: Delete Anchor", StepColor = Color.yellow }},
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
            set {}
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

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (spawnedObjectMat != null)
            {
                float rat = 0.1f;
                float createProgress = 0f;
                if (CloudManager.SessionStatus != null)
                {
                    createProgress = CloudManager.SessionStatus.RecommendedForCreateProgress;
                }
                rat += (Mathf.Min(createProgress, 1) * 0.9f);
                spawnedObjectMat.color = GetStepColor() * rat;
                //local anchor placed
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

        public async override Task AdvanceDemoAsync()
        {
            switch (currentAppState)
            {
                case AppState.DemoStepCreateSession:
                    currentAppState = AppState.DemoStepBusy;
                    Debug.Log("!!! DemoStepCreateSession");
                    if (CloudManager.Session == null)
                    {
                        await CloudManager.CreateSessionAsync();
                    }
                    Debug.Log("!!! DemoStepCreateSession 2");
                    currentAnchorId = "";
                    currentCloudAnchor = null;
                    currentAppState = AppState.DemoStepConfigSession;
                    //AdvanceDemoAsync(); //test if I can do this without a button
                    break;
                case AppState.DemoStepConfigSession:
                    currentAppState = AppState.DemoStepBusy;
                    Debug.Log("!!! DemoStepCreateSession 3");
                    ConfigureSession();
                    currentAppState = AppState.DemoStepStartSession;
                    Debug.Log("!!! DemoStepCreateSession 4");
                    //AdvanceDemoAsync();
                    break;
                case AppState.DemoStepStartSession:
                    currentAppState = AppState.DemoStepBusy;
                    roomManager.SwitchDonebutton(true);
                    await CloudManager.StartSessionAsync();
                    currentAppState = AppState.DemoStepCreateLocalAnchor;
                    break;
                case AppState.DemoStepCreateLocalAnchor:
                    currentAppState = AppState.DemoStepBusy;
                    if (spawnedObject != null)
                    {
                        currentAppState = AppState.DemoStepSaveCloudAnchor;
                    }
                    else
                    {
                        currentAppState = AppState.DemoStepCreateLocalAnchor;
                    }
                    AdvanceDemoAsync();
                    break;
                case AppState.DemoStepSaveCloudAnchor:
                    currentAppState = AppState.DemoStepSavingCloudAnchor;
                    await SaveCurrentObjectAnchorToCloudAsync();
                    roomManager.SwitchDoneButton(false);
                    roomManager.SwitchAddObject(true);
                    break;
                case AppState.DemoStepStopSession:
                    currentAppState = AppState.DemoStepBusy;
                    CloudManager.StopSession();
                    //CleanupSpawnedObjects();
                    await CloudManager.ResetSessionAsync();
                    currentAppState = AppState.DemoStepCreateSessionForQuery;
                    roomManager.SwitchAddObject(false);
                    roomManager.SwitchSaveAnchorPanel(true);
                    break;
             
                default:
                    Debug.Log("Shouldn't get here for app state " + currentAppState.ToString());
                    break;
                  
            }
        }

        public async Task StopSessionAsync()
        {
            CloudManager.StopSession();
            await CloudManager.ResetSessionAsync();
        }

        private void ConfigureSession()
        {
            List<string> anchorsToFind = new List<string>();
            if (currentAppState == AppState.DemoStepCreateSessionForQuery)
            {
                anchorsToFind.Add(currentAnchorId);
            }

            SetAnchorIdsToLocate(anchorsToFind);
        }

    }
}
