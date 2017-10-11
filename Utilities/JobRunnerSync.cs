// -----------------------------------------------------------------------
// <copyright file="JobRunnerSync.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
//-----------------------------------------------------------------------
namespace APSIM.Shared.Utilities
{
    using System;
    using System.Threading;

    /// <summary>A class for running jobs synchronously.</summary>
    public class JobRunnerSync : IJobRunner
    {
        /// <summary>Occurs when all jobs completed.</summary>
        public event EventHandler<AllCompletedArgs> AllJobsCompleted;

        /// <summary>Invoked when a job is completed.</summary>
        public event EventHandler<JobCompleteArgs> JobCompleted;

        /// <summary>Run the specified jobs</summary>
        /// <param name="jobs">An instance of a class that manages all jobs.</param>
        /// <param name="wait">Wait until all jobs finished before returning?</param>
        /// <param name="numberOfProcessors">The maximum number of cores to use.</param>
        public void Run(IJobManager jobs, bool wait = false, int numberOfProcessors = -1)
        {
            // No - get another job to run.
            IRunnable job = jobs.GetNextJobToRun();

            CancellationTokenSource cancelToken = new CancellationTokenSource();

            // Iterate through all jobs and run them.
            while (job != null)
            {
                JobCompleteArgs jobCompleteArguments = new JobCompleteArgs();
                jobCompleteArguments.job = job;
                try
                {
                    job.Run(cancelToken);
                }
                catch (Exception err)
                {
                    jobCompleteArguments.exceptionThrowByJob = err;
                }
                if (JobCompleted != null)
                    JobCompleted.Invoke(this, jobCompleteArguments);

                job = jobs.GetNextJobToRun();
            }

            Exception exceptionThrown = null;

            try
            {
                jobs.Completed();
            }
            catch (Exception err)
            {
                exceptionThrown = err;
            }
            finally
            {
                if (AllJobsCompleted != null)
                    AllJobsCompleted.Invoke(this, new AllCompletedArgs() { exceptionThrown = exceptionThrown });
            }
        }

        /// <summary>Stop all jobs currently running</summary>
        public void Stop()
        {
        }
        
    }
}
