using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ETdoFreshPublishNPM.Editor
{
    public static class NPMProcessRunner
    {
        public static bool Run(string workingDirectory, string registryPath)
        {
            var commands = new[] {"cmd", "npm", "./npm",};
            var arguments = $"publish --registry {registryPath}";
            foreach (var command in commands)
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo =
                        {
                            FileName = command,
                            Arguments = command == "cmd" ? "/c npm " + arguments : arguments,
                            WorkingDirectory = workingDirectory,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Debug.LogError(error);
                        return false;
                    }
                    else
                    {
                        Debug.Log(output);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return false;
        }
    }
}
