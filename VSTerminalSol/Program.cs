using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

/* This is a windows .net console application which allows you to open visual studio from the command line.
 To use this you have to build this solution in visual studio or download the studio.exe file in the repo.
 After choosing one of the above options you will have to copy the file path for the folder containing studio.exe
 If you downloaded the repo and built the solution the studio.exe file will be in the VSTerminalSol/bin/debug.
 After copying your file path go to start => type Edit System Environment Variables => click Environment Variables on the bottom =>
    *double click* on the Path in the area called User variables => click New and paste the file path
 After this you can go into any terminal and cd into the visual studio folder contining a *.sln file, 
    then enter => studio LAST_TWO_OF_YOUR_VS_VERSION(ex 22)
 */

namespace VSTerminal {

    public struct Data {
        public string currentPath { get { return Directory.GetCurrentDirectory(); } }
        public string slnFile { get; set; }
        public string[] versionOptions => new string[] { "2017", "2019", "2022" };  // Visual studio options to choose from
        public string[] paths => new string[] { @"C:\Program Files (x86)\", @"C:\Program Files\" };
        public string devEnvPath { get; set; }
        public string consoleOptions { get; set; }
        public Dictionary<string, string> devEnvOptions { get; set; }
    }

    internal class Program {
        static void Main( string[] args ) {

            Data data = new Data {
                devEnvOptions = new Dictionary<string, string>()
            };
        
            // gets the first sln file from the folder breaks if no sln found
            getSLN(ref data);
            if (data.slnFile == null) return; 

            // checks available versions on users pc breaks if no version found
            checkVersions(ref data);
            if(data.devEnvOptions.Count == 0 || data.consoleOptions == null) return;

            // checks which option of visual studio does the user want to use
            if (args.Length != 1 || !data.devEnvOptions.ContainsKey(args[0])) {
                Console.WriteLine($"Please enter the version of visual studio you would like to use. Available options {data.consoleOptions} ");
                return;
            }                
            else {
                data.devEnvPath = data.devEnvOptions[args[0]]; 
                getVSType(ref data);
                startProcess(ref data);
            }
                        
        }

        public static void checkVersions(ref Data data) {
            try {

                // checks for all versions of visual studio on the machine in program files and program files (x86)
                foreach (string path in data.paths) {

                    string pathWithVS = path + @"Microsoft Visual Studio\"; // adds Microsoft Visual Studio in front to search for it

                    var directoryVersion = new DirectoryInfo(pathWithVS).GetDirectories();


                    if (directoryVersion.Length <= 0) {
                        throw new Exception(@"No version of visual studio found in C:\Program Files or C:\Program Files (x86)\.");
                    }
                    else {
                        foreach (string versionOption in data.versionOptions) {
                            if (directoryVersion.Any(x => x.Name == versionOption)) {
                                string fulldevEnvPath = pathWithVS + versionOption + @"\";

                                data.devEnvOptions.Add(versionOption.Substring(2), fulldevEnvPath);

                                // add to console options to show user
                                data.consoleOptions += versionOption.Substring(2) + " ";
                            }
                        }
                    }

                }
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }

        }

        public static void getSLN(ref Data data ) {

            try {
                var slnFile = new DirectoryInfo(data.currentPath).GetFiles()
                    .Where(x => x.Extension == ".sln")
                    .OrderBy(x => x.LastAccessTimeUtc)
                    .FirstOrDefault(); // picks first sln found or null
                if (slnFile == null) {
                    throw new Exception("No sln file found in current directory");
                }
                else {
                    data.slnFile = slnFile.Name;
                    Console.WriteLine($"Found solution file => {slnFile.Name}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static void getVSType(ref Data data) {
            try {
                var vsDirectory = new DirectoryInfo(data.devEnvPath).GetDirectories();
                // Where is VS - Community or Enterprise?
                if (vsDirectory.Any(x => x.Name == "Community"))
                    data.devEnvPath += @"Community\Common7\IDE\";
                else if (vsDirectory.Any(x => x.Name == "Professional"))
                    data.devEnvPath += @"Professional\Common7\IDE\";
                else if (vsDirectory.Any(x => x.Name == "Enterprise"))
                    data.devEnvPath += @"Enterprise\Common7\IDE\";
                else {
                    throw new Exception($"Neither Visual Studio Community, Professional nor Enterprise can be found in {data.devEnvPath}");
                }
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }
            
        }

        public static void startProcess(ref Data data) {
            // Call VS in a new process and return to the shell
            Console.WriteLine($"Opening solution => {data.slnFile,10} ");
            var proc = new Process();
            proc.StartInfo.FileName = data.devEnvPath + "devenv";

            // Enclose single argument in "" if file path or sln name includes a space
            var arguments = "\"" + data.currentPath + @"\" + data.slnFile + "\"";

            proc.StartInfo.Arguments = arguments;
            proc.Start();
        }
    }
}



