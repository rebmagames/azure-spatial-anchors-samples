using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Microsoft.Azure.SpatialAnchors.Unity.Examples
{

    ///-------------------------------------------------------------------------------
    /// Author: Amber Voskamp
    /// <summary>
    /// Here comes the code for all the basic functionalities of ASA wich are now in AzurespatialAnchorsBasicDemoScript.cs and ASA find
    /// </summary>
    ///-------------------------------------------------------------------------------

    public class ASABasics : DemoScriptBase
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
            set { }
        }

        protected override Color GetStepColor()
        {
            return stateParams[currentAppState].StepColor;
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

        public async Task CreateSessionAsync()
        {
            Debug.Log("!!! DemoStepCreateSession");
            if (CloudManager.Session == null)
            {
                await CloudManager.CreateSessionAsync();
            }
            Debug.Log("!!! DemoStepCreateSession 2");
            currentAnchorId = "";
            currentCloudAnchor = null;
        }

        public async Task StartSession()
        {
            Debug.Log("!!! DemoStepCreateSession 3");
            ConfigureSession();
            
            Debug.Log("!!! DemoStepCreateSession 4");
            //StartSession2Async();
        }

        public async Task StartSession2Async()
        {
            roomManager.SwitchDonebutton(true);
            Debug.Log("!!! DemoStepCreateSession 4.5");
            await CloudManager.StartSessionAsync();
            Debug.Log("!!! DemoStepCreateSession 5");
        }

        public async Task CreateLocalAnchor()
        {
            if (spawnedObject != null)
            {
                Debug.Log("%%% 1");
                currentAppState = AppState.DemoStepSaveCloudAnchor;
                Debug.Log("%%% 2");
            }
            else
            {
                Debug.Log("%%% 3");
                currentAppState = AppState.DemoStepCreateLocalAnchor;
                Debug.Log("%%% 4");
            }
        }

        public async void SaveAnchor()
        {
            
            roomManager.SwitchDoneButton(false);
            roomManager.SwitchAddObject(true);
        }

        public async Task StopSessionAsync()
        {
            CloudManager.StopSession();
            //CleanupSpawnedObjects();
            await CloudManager.ResetSessionAsync();
            currentAppState = AppState.DemoStepCreateSessionForQuery;
            roomManager.SwitchAddObject(false);
            roomManager.SwitchSaveAnchorPanel(true);
        }

        public async override Task AdvanceDemoAsync()
        {
            switch (currentAppState)
            {
                
            }
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