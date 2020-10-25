using System;

namespace Services
{
    [AppOptions(Description = "Updates the files in the current directory.")]
    class ExampleOptions
    {
        [AppOptions(FullKeys = new[] { "help" }, ShortKeys = new[] { "h" })]
        public bool Help { get; set; }

        //[AppOptions(FullKeys = new[] { "merge-args" }, Description = "Merge over inner args")]
        //public bool MergeArgs { get; set; }

        [AppOptions(FullKeys = new[] { "wait-for-pid", "pid" }, Description = "Wait for end of process with specified PID before.")]
        public int[] WaitForPid { get; set; } = Array.Empty<int>();

        [AppOptions(FullKeys = new[] { "start-app" }, Description = "Start application.")]
        public bool StartApp { get; set; }

        [AppOptions(FullKeys = new[] { "default-exe-name" }, Description = "Default name for executable file.")]
        public string DefaultExeName { get; set; }

        [AppOptions(FullKeys = new[] { "check-for-updates" }, Description = "Only checks for available updates.")]
        public bool CheckForUpdates { get; set; }

        [AppOptions(FullKeys = new[] { "working-folder" }, Description = "Set working folder (the default folder is the one that contains updater.exe).")]
        public string WorkingFolder { get; set; }

        [AppOptions(FullKeys = new[] { "remote-manifest-url" }, Description = "Url to manifest.")]
        public string RemoteManifestUrl { get; set; }

        [AppOptions(FullKeys = new[] { "write-args" }, Description = "Write file with args inside.")]
        public bool WriteArgs { get; set; }

        [AppOptions(FullKeys = new[] { "examine-args" }, Description = "Examine inner args.")]
        public bool ExamineArgs { get; set; }

        [AppOptions(ShortKeys = new[] { "f" }, Description = "Force operation.")]
        public bool Force { get; set; }

        [AppOptions(FullKeys = new[] { "input" }, ShortKeys = new[] { "i" }, Description = "Input filename.")]
        public string InputFile { get; set; }

        [AppOptions(ShortKeys = new[] { "o" }, Description = "Output filename.")]
        public string OutputFile { get; set; }

    }
}
