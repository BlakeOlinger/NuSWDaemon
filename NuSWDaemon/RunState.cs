using System;
using System.IO;

namespace NuSWDaemon
{
    class RunState
    {
        private bool runState;
        private bool actionState;
        private bool closeBlob;
        private string blobName;

        internal bool GetCloseBlob()
        {
            return closeBlob;
        }

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
            string blobName, bool closeBlob)
        {
            this.runState = runState;
            this.actionState = actionState;
            this.blobName = blobName;
            this.closeBlob = closeBlob;
        }

        internal static RunState GetProgramState(string path)
        {
            try
            {
                var rawString = File.ReadAllText(path).Split("!");

                var runState = rawString[0].Substring(0, 1).CompareTo("0") == 0;
                var actionState = rawString[0].Substring(1, 1).CompareTo("0") == 0;
                var closeBlob = rawString[0].Substring(2, 1).CompareTo("0") == 0;

                var blobName = rawString.Length > 1 ? rawString[1] : null;

                return new RunState(
                    runState,
                    actionState,
                    blobName,
                    closeBlob
                    );

            } catch (Exception exception)
            {
                Console.WriteLine(exception);

                return null;
            }
        }
    }
}
