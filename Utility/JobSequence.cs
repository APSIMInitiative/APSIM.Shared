﻿// -----------------------------------------------------------------------
// <copyright file="JobSequence.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
//-----------------------------------------------------------------------
namespace Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// A composite class for a sequence of jobs that will be run sequentially.
    /// If an error occurs in any job, then subsequent jobs won't be run.
    /// </summary>
    public class JobSequence : Utility.JobManager.IRunnable
    {
        /// <summary>Gets a value indicating whether this instance is computationally time consuming.</summary>
        public bool IsComputationallyTimeConsuming { get { return false; } }

        /// <summary>A list of jobs that will be run in sequence.
        public List<Utility.JobManager.IRunnable> Jobs { get; set; }
        
        /// <summary>Gets a value indicating whether this job is completed. Set by JobManager.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets the error message. Can be null if no error. Set by JobManager.</summary>
        public string ErrorMessage { get; set; }

        /// <summary>Called to start the job.</summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void Run(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Get a reference to the JobManager so that we can add jobs to it.
            Utility.JobManager jobManager = e.Argument as Utility.JobManager;

            for (int j = 0; j < Jobs.Count; j++)
            {
                // Add job to the queue
                jobManager.AddJob(Jobs[j]);

                // Wait for it to be completed.
                while (!Jobs[j].IsCompleted)
                    Thread.Sleep(200);

                if (Jobs[j].ErrorMessage != null)
                    throw new Exception(Jobs[j].ErrorMessage);
            }            
        }

        /// <summary>
        /// Gets a value indicating whether all jobs are completed.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        private bool AllCompleted
        {
            get
            {
                foreach (Utility.JobManager.IRunnable job in Jobs)
                    if (!job.IsCompleted)
                        return false;

                return true;
            }
        }

    }
}
