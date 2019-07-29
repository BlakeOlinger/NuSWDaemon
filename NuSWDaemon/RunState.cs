using System;
using System.IO;

namespace NuSWDaemon
{
    class RunState
    {
        private bool runState;
        private bool actionState;
        private string blobName;

        internal bool GetRunState()
        {
            return runState;
        }

        internal bool GetActionState()
        {
            return actionState;
        }

        internal string GetBlobName()
        {
            return blobName;
        }

        private RunState(bool runState, bool actionState,
            string blobName)
        {
            this.runState = runState;
            this.actionState = actionState;
            this.blobName = blobName;
        }

        internal static RunState GetProgramState(string path)
        {
            try
            {
                var rawString = File.ReadAllText(path).Split("!");

                var runState = rawString[0].Substring(0, 1).CompareTo("0") == 0;
                var actionState = rawString[0].Substring(1, 1).CompareTo("0") == 0;

                var blobName = rawString.Length > 1 ? rawString[1] : null;

                return new RunState(
                    runState,
                    actionState,
                    blobName
                    );

            } catch (Exception exception)
            {
                Console.WriteLine(exception);

                return null;
            }
        }
    }
}
