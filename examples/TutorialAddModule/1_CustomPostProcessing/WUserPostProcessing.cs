﻿using System;
using OpenPoseDotNet;

namespace CustomPostProcessing
{

    internal sealed class WUserPostProcessing : UserWorker<CustomDatum>
    {

        #region Fields

        private readonly UserPostProcessing _UserPostProcessing;

        #endregion

        #region Constructors

        public WUserPostProcessing(UserPostProcessing userPostProcessing)
        {
            if (userPostProcessing == null)
                throw new ArgumentNullException(nameof(userPostProcessing));

            this._UserPostProcessing = userPostProcessing;
        }

        #endregion

        #region Methods

        #region Overrides

        protected override void InitializationOnThread()
        {
        }

        protected override void Work(StdSharedPtr<CustomDatum>[] datums)
        {
            try
            {
                if (datums != null)
                {
                    // Debugging log
                    OpenPose.LogIfDebug("", Priority.Low, -1, nameof(this.Work), "");
                    // Profiling speed
                    var profilerKey = Profiler.TimerInit(-1,nameof(this.Work),"");

                    foreach (var datum in datums)
                    {
                        // THESE 2 ARE THE ONLY LINES THAT THE USER MUST MODIFY ON THIS HPP FILE, by using the proper
                        // function and datum elements
                        using (var cvOutputData = OpenPose.OP_OP2CVMAT(datum.Get().CvOutputData))
                            this._UserPostProcessing.DoSomething(cvOutputData, cvOutputData);
                        // Profiling speed
                        Profiler.TimerEnd(profilerKey);
                        Profiler.PrintAveragedTimeMsOnIterationX(profilerKey, -1, nameof(this.Work), "");
                        // Debugging log
                        OpenPose.LogIfDebug("", Priority.Low, -1, nameof(this.Work), "");
                    }
                }
            }
            catch (Exception e)
            {
                this.Stop();
                datums = null;
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #endregion

    }

}