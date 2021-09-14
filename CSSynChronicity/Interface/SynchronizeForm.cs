using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSSynChronicity;

namespace CSSynChronicity.Interface
{
    public partial class SynchronizeForm : Form
    {
        private LogHandler Log;
        private ProfileHandler Handler;

        private Dictionary<string, bool> ValidFiles = new Dictionary<string, bool>();
        private List<SyncingItem> SyncingList = new List<SyncingItem>();
        private List<FileNamePattern> IncludedPatterns = new List<FileNamePattern>();
        private List<FileNamePattern> ExcludedPatterns = new List<FileNamePattern>();
        private List<FileNamePattern> ExcludedDirPatterns = new List<FileNamePattern>();

        private string[] Labels = null;
        private string StatusLabel = "";
        private object Lock = new object();
        ConfigHandler ProgramConfig = ConfigHandler.GetSingleton();
        LanguageHandler Translation = LanguageHandler.GetSingleton();

        private bool Catchup; // Indicates whether this operation was started due to catchup rules.
        private bool Preview; // Should show a preview.

        private StatusData Status;
        private string TitleText;
        private SyncingListSorter Sorter = new SyncingListSorter(3);

        private System.Threading.Thread FullSyncThread;
        private System.Threading.Thread ScanThread;
        private System.Threading.Thread SyncThread;

        private bool AutoInclude;
        private DateTime MDate;
        private string LeftRootPath, RightRootPath; // Translated path to left and right folders

        private delegate void StepCompletedCall(StatusData.SyncStep Id);
        private delegate void SetIntCall(StatusData.SyncStep Id, int Max);

        internal event SyncFinishedEventHandler SyncFinished;

        internal delegate void SyncFinishedEventHandler(string Name, bool Completed);

        public SynchronizeForm(string ConfigName, bool DisplayPreview, bool _Catchup)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            Catchup = _Catchup;
            Preview = DisplayPreview;
            SyncBtn.Enabled = false;
            SyncBtn.Visible = Preview;

            Status = new StatusData();
            Status.CurrentStep = StatusData.SyncStep.Scan;
            Status.StartTime = DateTime.Now; // NOTE: This call should be useless; it however seems that when the messagebox.show method is called when a profile is not found, the syncingtimecounter starts ticking. This is not suitable, but until the cause is found there this call remains, for display consistency.

            Handler = new ProfileHandler(ConfigName);
            Log = new LogHandler(ConfigName, Handler.GetSetting<bool>(ProfileSetting.ErrorsLog, false));

            MDate = Handler.GetSetting<DateTime>(ProfileSetting.LastModified, System.IO.File.GetLastWriteTimeUtc(Handler.ConfigPath));
            AutoInclude = Handler.GetSetting<bool>(ProfileSetting.AutoIncludeNewFolders, false);
            LeftRootPath = ProfileHandler.TranslatePath(Handler.GetSetting<string>(ProfileSetting.Source));
            RightRootPath = ProfileHandler.TranslatePath(Handler.GetSetting<string>(ProfileSetting.Destination));

            FileNamePattern.LoadPatternsList(IncludedPatterns, Handler.GetSetting<string>(ProfileSetting.IncludedTypes, ""), false, ProgramSetting.ExcludedFolderPrefix);
            FileNamePattern.LoadPatternsList(ExcludedPatterns, Handler.GetSetting<string>(ProfileSetting.ExcludedTypes, ""), false, ProgramSetting.ExcludedFolderPrefix);
            FileNamePattern.LoadPatternsList(ExcludedDirPatterns, Handler.GetSetting<string>(ProfileSetting.ExcludedFolders, ""), true, "");
            FileNamePattern.LoadPatternsList(ExcludedDirPatterns, Handler.GetSetting<string>(ProfileSetting.ExcludedTypes, ""), true, ProgramSetting.ExcludedFolderPrefix);

            FullSyncThread = new System.Threading.Thread(FullSync);
            ScanThread = new System.Threading.Thread(Scan);
            SyncThread = new System.Threading.Thread(Sync);

            this.CreateHandle();
            Translation.TranslateControl(this);
            this.Icon = ProgramConfig.Icon;
            TitleText = string.Format(this.Text, Handler.ProfileName, LeftRootPath, RightRootPath);

            Labels = new string[] { "", Step1StatusLabel.Text, Step2StatusLabel.Text, Step3StatusLabel.Text };
        }

        public void StartSynchronization(bool CalledShowModal)
        {
            ProgramConfig.CanGoOn = false;

            if (CommandLine.Quiet)
            {
                this.Visible = false;

                Interaction.StatusIcon.ContextMenuStrip = null;
                Interaction.StatusIcon.Click += StatusIcon_Click;

                Interaction.StatusIcon.Text = Translation.Translate(@"\RUNNING");

                Interaction.ToggleStatusIcon(true);
                if (Catchup)
                    Interaction.ShowBalloonTip(Translation.TranslateFormat(@"\CATCHING_UP", Handler.ProfileName, Handler.FormatLastRun()));
                else
                    Interaction.ShowBalloonTip(Translation.TranslateFormat(@"\RUNNING_TASK", Handler.ProfileName));
            }
            else if (!CalledShowModal)
                this.Visible = true;

            Status.FailureMsg = "";

            bool IsValid = Handler.ValidateConfigFile(false, true, Status.FailureMsg);
            if (Handler.GetSetting<bool>(ProfileSetting.PreviewOnly, false) & (!Preview))
                IsValid = false;

            Status.Failed = !IsValid;

            if (IsValid)
            {
                ProgramConfig.IncrementSyncsCount();
                if (Preview)
                    ScanThread.Start();
                else
                    FullSyncThread.Start();
            }
            else
                EndAll();// Also saves the log file
        }

        private void StatusIcon_Click(object sender, System.EventArgs e) // Handler dynamically added
        {
            this.Visible = !this.Visible;
            this.WindowState = FormWindowState.Normal;
            if (this.Visible)
                this.Activate();
        }

        private void FullSync()
        {
            Scan();
            Sync();
        }

        private void LaunchTimer()
        {
            Status.BytesCopied = 0;
            Status.StartTime = DateTime.Now;
            SyncingTimer.Start();
        }

        private void AddValidFile(string File)
        {
            if (!IsValidFile(File))
                ValidFiles.Add(File.ToLower(Interaction.InvariantCulture), true/* TODO Change to default(_) if this is not a reference type */);
        }
        private void AddValidAncestors(string Folder)
        {
            Log.LogInfo(string.Format("AddValidAncestors: Folder \"{0}\" is a top level folder, adding it's ancestors.", Folder));
            System.Text.StringBuilder CurrentAncestor = new System.Text.StringBuilder();
            List<string> Ancestors = new List<string>(Folder.Split(new char[] { ProgramSetting.DirSep }, StringSplitOptions.RemoveEmptyEntries));

            for (int Depth = 0; Depth <= (Ancestors.Count - 1) - 1; Depth++) // The last ancestor is the folder itself, and will be added in SearchForChanges.
            {
                CurrentAncestor.Append(ProgramSetting.DirSep).Append(Ancestors[Depth]);
                AddValidFile(CurrentAncestor.ToString());
                Log.LogInfo(string.Format("AddValidAncestors: [Valid folder] \"{0}\"", CurrentAncestor.ToString()));
            }
        }

        private void RemoveValidFile(string File)
        {
            if (IsValidFile(File))
                ValidFiles.Remove(File.ToLower(Interaction.InvariantCulture));
        }

        private bool IsValidFile(string File)
        {
            return ValidFiles.ContainsKey(File.ToLower(Interaction.InvariantCulture));
        }

        private void PopSyncingList(SideOfSource Side)
        {
            ValidFiles.Remove(SyncingList[SyncingList.Count - 1].Path);
            SyncingList.RemoveAt(SyncingList.Count - 1);

            Status.TotalActionsCount -= 1;
            switch (Side)
            {
                case SideOfSource.Left:
                    {
                        Status.LeftActionsCount -= 1;
                        break;
                    }

                case  SideOfSource.Right:
                    {
                        Status.RightActionsCount -= 1;
                        break;
                    }
            }
        }


        private bool IsExcludedSinceHidden(string Path)
        {
            // File.GetAttributes works for folders ; see http://stackoverflow.com/questions/8110646/
            return Handler.GetSetting<bool>(ProfileSetting.ExcludeHidden, false) && (System.IO.File.GetAttributes(Path) & System.IO.FileAttributes.Hidden) != 0;
        }

        private bool IsTooOld(string Path)
        {
            int Days = Handler.GetSetting<int>(ProfileSetting.DiscardAfter, 0);
            return ((Days > 0) && (DateTime.UtcNow - System.IO.File.GetLastWriteTimeUtc(Path)).TotalDays > Days);
        }

        internal static string GetFileOrFolderName(string Path)
        {
            return Path.Substring(Path.LastIndexOf(ProgramSetting.DirSep) + 1); // IO.Path.* -> Bad because of separate file/folder handling.
        }

        private bool IsIncludedInSync(string FullPath)
        {
            if (IsExcludedSinceHidden(FullPath) || IsTooOld(FullPath))
                return false;

            try
            {
                switch (Handler.GetSetting<int>(ProfileSetting.Restrictions))
                {
                    case 1:
                        {
                            return FileNamePattern.MatchesPattern(GetFileOrFolderName(FullPath), IncludedPatterns);
                        }

                    case 2:
                        {
                            return !FileNamePattern.MatchesPattern(GetFileOrFolderName(FullPath), ExcludedPatterns);
                        }
                }
            }
            catch (Exception Ex)
            {
                Log.HandleSilentError(Ex);
            }

            return true;
        }

        private bool HasAcceptedDirname(string Path)
        {
            return !FileNamePattern.MatchesPattern(Path, ExcludedDirPatterns);
        }

        private string GetCompressedName(string OriginalName)
        {

            string Extension = Handler.GetSetting(ProfileSetting.CompressionExt, "");


            if (Extension != "" && Handler.GetSetting<bool>(ProfileSetting.Decompress, false))
                return OriginalName.EndsWith(Extension) ? OriginalName.Substring(0, OriginalName.LastIndexOf(Extension)) : OriginalName;
            else
                return OriginalName + Extension;
        }

        private bool AttributesChanged(string AbsSource, string AbsDest)
        {
            const System.IO.FileAttributes AttributesMask = System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System | System.IO.FileAttributes.Encrypted;

            // Disabled by default, and in two-ways mode.
            // TODO: Enable by default. It's currently disabled because some network drives do not update attributes correctly.
            if (!Handler.GetSetting<bool>(ProfileSetting.SyncFolderAttributes, false))
                return false;
            if ((Handler.GetSetting(ProfileSetting.Method,ProfileSetting.DefaultMethod)) ==(int)ProfileSetting.SyncMethod.BiIncremental)
                return false;

            try
            {
                return ((System.IO.File.GetAttributes(AbsSource) & AttributesMask) != (System.IO.File.GetAttributes(AbsDest) & AttributesMask));
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private void AddToSyncingList(string Path, TypeOfItem Type, SideOfSource Side, TypeOfAction Action, bool IsUpdate)
        {
            SyncingItem Entry = new SyncingItem() { Path = Path, Type = Type, Side = Side, Action = Action, IsUpdate = IsUpdate, RealId = SyncingList.Count };

            SyncingList.Add(Entry);
            if (Entry.Action != TypeOfAction.Delete)
                AddValidFile(Type == TypeOfItem.Folder ? Entry.Path : GetCompressedName(Entry.Path));

            switch (Entry.Action)
            {
                case  TypeOfAction.Copy:
                    {
                        if (Entry.Type == TypeOfItem.Folder)
                            Status.FoldersToCreate += 1;
                        else if (Entry.Type == TypeOfItem.File)
                            Status.FilesToCreate += 1;
                        break;
                    }

                case  TypeOfAction.Delete:
                    {
                        if (Entry.Type == TypeOfItem.Folder)
                            Status.FoldersToDelete += 1;
                        else if (Entry.Type == TypeOfItem.File)
                            Status.FilesToDelete += 1;
                        break;
                    }
            }
            switch (Entry.Side)
            {
                case  SideOfSource.Left:
                    {
                        Status.LeftActionsCount += 1;
                        break;
                    }

                case  SideOfSource.Right:
                    {
                        Status.RightActionsCount += 1;
                        break;
                    }
            }
            Status.TotalActionsCount += 1;
        }

        // Error catching for this function is done in the calling section
        private bool SourceIsMoreRecent(string AbsSource, string AbsDest) // Assumes Source and Destination exist.
        {
            if ((!Handler.GetSetting<bool>(ProfileSetting.PropagateUpdates, true)))
                return false; // LATER: Require expert mode?

            Log.LogInfo(string.Format("SourceIsMoreRecent: {0}, {1}", AbsSource, AbsDest));

            DateTime SourceFATTime = NTFSToFATTime(System.IO.File.GetLastWriteTimeUtc(AbsSource)).AddHours(Handler.GetSetting<int>(ProfileSetting.TimeOffset, 0));
            DateTime DestFATTime = NTFSToFATTime(System.IO.File.GetLastWriteTimeUtc(AbsDest));
            Log.LogInfo(string.Format("SourceIsMoreRecent: S:({0}, {1}); D:({2}, {3})", Interaction.FormatDate(System.IO.File.GetLastWriteTimeUtc(AbsSource)), Interaction.FormatDate(SourceFATTime), Interaction.FormatDate(System.IO.File.GetLastWriteTimeUtc(AbsDest)), Interaction.FormatDate(DestFATTime)));

            if (Handler.GetSetting<bool>(ProfileSetting.FuzzyDstCompensation, false))
            {
                int HoursDiff = System.Convert.ToInt32((SourceFATTime - DestFATTime).TotalHours);
                if (Math.Abs(HoursDiff) == 1)
                    DestFATTime = DestFATTime.AddHours(HoursDiff);
            }

            // StrictMirror is disabled in constructor if Method != LRMirror
            if (SourceFATTime < DestFATTime && (!Handler.GetSetting<bool>(ProfileSetting.StrictMirror, false)))
                return false;

            // User-enabled checks
            if (Handler.GetSetting<bool>(ProfileSetting.Checksum, false) && Md5(AbsSource) != Md5(AbsDest))
                return true;
            if (Handler.GetSetting<bool>(ProfileSetting.CheckFileSize, false) && Utilities.GetSize(AbsSource) != Utilities.GetSize(AbsDest))
                return true;

            if (Handler.GetSetting<bool>(ProfileSetting.StrictDateComparison, true))
            {
                if (SourceFATTime == DestFATTime)
                    return false;
            }
            else if (Math.Abs((SourceFATTime - DestFATTime).TotalSeconds) <= 4)
                return false;// Note: NTFSToFATTime introduces additional fuzziness (justifies the <= ('=')).
            Log.LogInfo("SourceIsMoreRecent: Filetimes differ");

            return true;
        }


        // This procedure searches for changes in the source directory.
        private void SearchForChanges(string Folder, bool Recursive, SyncingContext Context)
        {
            string SourceFolder = CombinePathes(Context.SourceRootPath, Folder);
            string DestinationFolder = CombinePathes(Context.DestinationRootPath, Folder);

            // Exit on excluded folders (and optionally on hidden ones).
            if (!HasAcceptedDirname(Folder) || IsExcludedSinceHidden(SourceFolder) || IsSymLink(SourceFolder))
                return;

            UpdateLabel(StatusData.SyncStep.Scan, SourceFolder);
            Log.LogInfo(string.Format("=> Scanning folder \"{0}\" for new or updated files.", Folder));

            // LATER: Factor out.
            bool IsNewFolder = !System.IO.Directory.Exists(DestinationFolder);
            bool ShouldUpdateFolder = IsNewFolder || AttributesChanged(SourceFolder, DestinationFolder);
            if (ShouldUpdateFolder)
            {
                AddToSyncingList(Folder, TypeOfItem.Folder, Context.Source, TypeOfAction.Copy, !IsNewFolder);
                Log.LogInfo(string.Format("SearchForChanges: {0} \"{1}\" \"{2}\" ({3})", IsNewFolder ? "[New folder]" : "[Updated folder]", SourceFolder, DestinationFolder, Folder));
            }
            else
            {
                AddValidFile(Folder);
                Log.LogInfo(string.Format("SearchForChanges: [Valid folder] \"{0}\" \"{1}\" ({2})", SourceFolder, DestinationFolder, Folder));
            }

            int InitialValidFilesCount = ValidFiles.Count;
            try
            {
                foreach (string SourceFile in System.IO.Directory.GetFiles(SourceFolder))
                {
                    Log.LogInfo("Scanning " + SourceFile);
                    string DestinationFile = GetCompressedName(CombinePathes(DestinationFolder, System.IO.Path.GetFileName(SourceFile)));

                    try
                    {
                        if (IsIncludedInSync(SourceFile))
                        {
                            bool IsNewFile = !System.IO.File.Exists(DestinationFile);
                            string RelativeFilePath = SourceFile.Substring(Context.SourceRootPath.Length);

                            if (IsNewFile || SourceIsMoreRecent(SourceFile, DestinationFile))
                            {
                                AddToSyncingList(RelativeFilePath, TypeOfItem.File, Context.Source, TypeOfAction.Copy, !IsNewFile);
                                Log.LogInfo(string.Format("SearchForChanges: {0} \"{1}\" \"{2}\" ({3}).", IsNewFile ? "[New File]" : "[Updated file]", SourceFile, DestinationFile, RelativeFilePath));

                                if (ProgramConfig.GetProgramSetting<bool>(ProgramSetting.Forecast, false))
                                    Status.BytesToCopy += Utilities.GetSize(SourceFile); // Degrades performance.
                            }
                            else
                            {
                                // Adds an entry to not delete this when cleaning up the other side.
                                AddValidFile(GetCompressedName(RelativeFilePath));
                                Log.LogInfo(string.Format("SearchForChanges: [Valid] \"{0}\" \"{1}\" ({2})", SourceFile, DestinationFile, RelativeFilePath));
                            }
                        }
                        else
                            Log.LogInfo(string.Format("SearchForChanges: [Excluded file] \"{0}\"", SourceFile));
                    }
                    catch (Exception Ex)
                    {
                        Log.HandleError(Ex, SourceFile);
                    }

                    Status.FilesScanned += 1;
                }
            }
            catch (Exception Ex)
            {
                Log.HandleSilentError(Ex);
            }

            if (Recursive | AutoInclude)
            {
                try
                {
                    foreach (string SubFolder in System.IO.Directory.GetDirectories(SourceFolder))
                    {
                        if (Recursive || (AutoInclude && System.IO.Directory.GetCreationTimeUtc(SubFolder) > MDate))
                            SearchForChanges(SubFolder.Substring(Context.SourceRootPath.Length), true, Context);
                    }
                }
                catch (Exception Ex)
                {
                    Log.HandleSilentError(Ex);
                }
            }

            if (InitialValidFilesCount == ValidFiles.Count)
            {
                if (!Handler.GetSetting<bool>(ProfileSetting.ReplicateEmptyDirectories, true))
                {
                    if (ShouldUpdateFolder)
                    {
                        // Don't create/update this folder.
                        Status.FoldersToCreate -= 1;
                        PopSyncingList(Context.Source);
                    }

                    RemoveValidFile(Folder); // Folders added for creation are marked as valid in AddToSyncingList. Removing this mark is vital to ensuring that the ReplicateEmptyDirectories setting works correctly (otherwise the count increases.
                }
            }
        }

        private bool IsSymLink(string SubFolder)
        {
            if ((System.IO.File.GetAttributes(SubFolder) & System.IO.FileAttributes.ReparsePoint) != 0)
            {
                Log.LogInfo(string.Format("Symlink detected: {0}; not following.", SubFolder));
                return true;
            }

            return false;
        }

        private void SearchForCrap(string Folder, bool Recursive, SyncingContext Context)
        {
            // Here, Source is set to be the right folder, and dest to be the left folder
            string SourceFolder = CombinePathes(Context.SourceRootPath, Folder);
            string DestinationFolder = CombinePathes(Context.DestinationRootPath, Folder);

            // Folder exclusion doesn't work exactly the same as file exclusion: if "Source\a" is excluded, "Dest\a" doesn't get deleted. That way one can safely exclude "Source\System Volume Information" and the like.
            if (!HasAcceptedDirname(Folder) || IsExcludedSinceHidden(SourceFolder) || IsSymLink(SourceFolder))
                return;

            UpdateLabel(StatusData.SyncStep.Scan, SourceFolder);
            Log.LogInfo(string.Format("=> Scanning folder \"{0}\" for files to delete.", Folder));
            try
            {
                foreach (string File in System.IO.Directory.GetFiles(SourceFolder))
                {
                    string RelativeFName = File.Substring(Context.SourceRootPath.Length);

                    try
                    {
                        if (!IsValidFile(RelativeFName))
                        {
                            AddToSyncingList(RelativeFName, TypeOfItem.File, Context.Source, TypeOfAction.Delete, false);
                            Log.LogInfo(string.Format("Cleanup: [Delete] \"{0}\" ({1})", File, RelativeFName));
                        }
                        else
                            Log.LogInfo(string.Format("Cleanup: [Keep] \"{0}\" ({1})", File, RelativeFName));
                    }
                    catch (Exception Ex)
                    {
                        Log.HandleError(Ex);
                    }

                    Status.FilesScanned += 1;
                }
            }
            catch (Exception Ex)
            {
                Log.HandleSilentError(Ex);
            }

            if (Recursive | AutoInclude)
            {
                try
                {
                    foreach (string SubFolder in System.IO.Directory.GetDirectories(SourceFolder))
                    {
                        if (Recursive || (AutoInclude && System.IO.Directory.GetCreationTimeUtc(SubFolder) > MDate))
                            SearchForCrap(SubFolder.Substring(Context.SourceRootPath.Length), true, Context);
                    }
                }
                catch (Exception Ex)
                {
                    Log.HandleSilentError(Ex);
                }
            }

            // Folder.Length = 0 <=> This is the root folder, not to be deleted.
            if (Folder.Length != 0 && !IsValidFile(Folder))
            {
                Log.LogInfo(string.Format("Cleanup: [Delete folder] \"{0}\" ({1}).", DestinationFolder, Folder));
                AddToSyncingList(Folder, TypeOfItem.Folder, Context.Source, TypeOfAction.Delete, false);
            }
        }

        private void Init_Synchronization(ref Dictionary<string, bool> FoldersList, SyncingContext Context, TypeOfAction Action)
        {
            foreach (string Folder in FoldersList.Keys)
            {
                Log.LogInfo(string.Format("=> Scanning \"{0}\" top level folders: \"{1}\"", Context.SourceRootPath, Folder));
                if (System.IO.Directory.Exists(CombinePathes(Context.SourceRootPath, Folder)))
                {
                    if (Action == TypeOfAction.Copy)
                    {
                        // FIXED-BUG: Every ancestor of this folder should be added too.
                        // Careful with this, for it's a performance issue. Ancestors should only be added /once/.
                        // How to do that? Well, if ancestors of a folder have not been scanned, it means that this folder wasn't reached by a recursive call, but by a initial call.
                        // Therefore, only the folders in the sync config file should be added.
                        AddValidAncestors(Folder);
                        SearchForChanges(Folder, FoldersList[Folder], Context);
                    }
                    else if (Action == TypeOfAction.Delete)
                        SearchForCrap(Folder, FoldersList[Folder], Context);
                }
            }
        }

        private static string CombinePathes(string Dir, string File) // LATER: Should be optimized; IO.Path?
        {
            return Dir.TrimEnd(ProgramSetting.DirSep) + ProgramSetting.DirSep + File.TrimStart(ProgramSetting.DirSep);
        }

        //private static Compressor LoadCompressionDll()
        //{
        //    System.Reflection.Assembly DLL = System.Reflection.Assembly.LoadFrom(ProgramConfig.CompressionDll);

        //    foreach (Type SubType in DLL.GetTypes())
        //    {
        //        if (typeof(Compressor).IsAssignableFrom(SubType))
        //            return (Compressor)Activator.CreateInstance(SubType);
        //    }

        //    throw new ArgumentException("Invalid DLL: " + ProgramConfig.CompressionDll);
        //}

        private static string Md5(string Path)
        {
            using (System.IO.StreamReader DataStream = new System.IO.StreamReader(Path))
            using (System.Security.Cryptography.MD5CryptoServiceProvider CryptObject = new System.Security.Cryptography.MD5CryptoServiceProvider()
    )
            {
                return Convert.ToBase64String(CryptObject.ComputeHash(DataStream.BaseStream));
            }
        }

        private static DateTime NTFSToFATTime(DateTime NTFSTime)
        {
            return (new DateTime(NTFSTime.Year, NTFSTime.Month, NTFSTime.Day, NTFSTime.Hour, NTFSTime.Minute, NTFSTime.Second).AddSeconds(NTFSTime.Millisecond == 0 ? NTFSTime.Second % 2 : 2 - (NTFSTime.Second % 2)));
        }

        private void Scan()
        {
            SyncingContext Context = new SyncingContext();
            StepCompletedCall StepCompletedCallback = new StepCompletedCall(StepCompleted);

            // Pass 1: Create actions L->R for files/folder copy, and mark dest files that should be kept
            // Pass 2: Create actions R->L for files/folder copy/deletion, based on what was marked as Valid.

            SyncingList.Clear();
            ValidFiles.Clear();

            this.Invoke(new Action(LaunchTimer));
            Context.Source = SideOfSource.Left;
            Context.SourceRootPath = LeftRootPath;
            Context.DestinationRootPath = RightRootPath;
            Init_Synchronization(ref Handler.LeftCheckedNodes, Context, TypeOfAction.Copy);

            Context.Source = SideOfSource.Right;
            Context.SourceRootPath = RightRootPath;
            Context.DestinationRootPath = LeftRootPath;
            switch (Handler.GetSetting<int>(ProfileSetting.Method, ProfileSetting.DefaultMethod)) // Important: (Of Integer)
            {
                case (int)ProfileSetting.SyncMethod.LRMirror:
                    {
                        Init_Synchronization(ref Handler.RightCheckedNodes, Context, TypeOfAction.Delete);
                        break;
                    }

                case (int)ProfileSetting.SyncMethod.LRIncremental:
                    {
                        break;
                    }

                case (int)ProfileSetting.SyncMethod.BiIncremental:
                    {
                        Init_Synchronization(ref Handler.RightCheckedNodes, Context, TypeOfAction.Copy);
                        break;
                    }
            }
            this.Invoke(StepCompletedCallback, StatusData.SyncStep.Scan);
        }

        private void Increment(StatusData.SyncStep Id, int Progress)
        {
            ProgressBar CurBar = GetProgressBar(Id);
            if (CurBar.Value + Progress < CurBar.Maximum)
                CurBar.Value += Progress;
        }

        private void CopyFile(string SourceFile, string DestFile)
        {
            string CompressedFile = GetCompressedName(DestFile);

            Log.LogInfo(string.Format("CopyFile: Source: {0}, Destination: {1}", SourceFile, DestFile));

            if (System.IO.File.Exists(DestFile))
                System.IO.File.SetAttributes(DestFile, System.IO.FileAttributes.Normal);

            bool Compression = CompressedFile != DestFile;
            DestFile = CompressedFile;

            if (Compression)
            {

                //GZipCompressor As Compressor = LoadCompressionDll()

 
            //Static Decompress As Boolean = Handler.GetSetting(Of Boolean)(ProfileSetting.Decompress, False)

 
            //GZipCompressor.CompressFile(SourceFile, CompressedFile, Decompress, Sub(Progress As Long) Status.BytesCopied += Progress) ', ByRef ContinueRunning As Boolean) 'ContinueRunning = Not [STOP]


            }
            else if (System.IO.File.Exists(DestFile))
            {
                try
                {
                    using (System.IO.FileStream TestForAccess = new System.IO.FileStream(SourceFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                    {
                    } // Checks whether the file can be accessed before trying to copy it. This line was added because if the file is only partially locked, CopyFileEx starts copying it, then fails on the way, and deletes the destination.
                    System.IO.File.Copy(SourceFile, DestFile, true);
                }
                catch ( System.IO.IOException Ex)
                {
                    Log.LogInfo(string.Format("Copy failed with message \"{0}\": Retrying in safe mode", Ex.Message));
                    SafeCopy(SourceFile, DestFile);
                }
            }
            else
                System.IO.File.Copy(SourceFile, DestFile);

            try
            {
                SyncFileAttributes(SourceFile, DestFile);
            }
            catch (UnauthorizedAccessException Ex)
            {
                // This section addresses a subtle bug on NAS drives: if you reconfigure your NAS and change user settings, some files may cause access denied exceptions when trying to update their attributes. Resetting the file is the only solution I've found.
                Log.LogInfo("CopyFile: Syncing file attributes failed. Retrying");
                SafeCopy(SourceFile, DestFile);
                SyncFileAttributes(SourceFile, DestFile);
            }

            Status.CreatedFiles += 1;
            if (!Compression)
                Status.BytesCopied += Utilities.GetSize(SourceFile);
            if (Handler.GetSetting<bool>(ProfileSetting.Checksum, false) && Md5(SourceFile) != Md5(DestFile))
                throw new System.Security.Cryptography.CryptographicException("MD5 validation: failed.");
        }

        private void SyncFileAttributes(string SourceFile, string DestFile)
        {
            if (Handler.GetSetting<int>(ProfileSetting.TimeOffset, 0) != 0)
            {
                Log.LogInfo("SyncFileAttributes: DST: Setting attributes to normal; current attributes: " + System.IO.File.GetAttributes(DestFile));
                System.IO.File.SetAttributes(DestFile, System.IO.FileAttributes.Normal); // Tracker #2999436
                Log.LogInfo("SyncFileAttributes: DST: Setting last write time");
                // Must use IO.File.GetLastWriteTimeUtc(**DestFile**), because it might differ from IO.File.GetLastWriteTimeUtc(**SourceFile**) (rounding, DST settings, ...)
                System.IO.File.SetLastWriteTimeUtc(DestFile, System.IO.File.GetLastWriteTimeUtc(DestFile).AddHours(Handler.GetSetting<int>(ProfileSetting.TimeOffset, 0)));
                Log.LogInfo("SyncFileAttributes: DST: Last write time set to " + System.IO.File.GetLastWriteTimeUtc(DestFile));
            }

            Log.LogInfo("SyncFileAttributes: Setting attributes to " + System.IO.File.GetAttributes(SourceFile));
            System.IO.File.SetAttributes(DestFile, System.IO.File.GetAttributes(SourceFile));
            Log.LogInfo("SyncFileAttributes: Attributes set to " + System.IO.File.GetAttributes(DestFile));
        }

        private static void SafeCopy(string SourceFile, string DestFile)
        {
            string TempDest, DestBack;
            do
            {
                TempDest = DestFile + "-" + System.IO.Path.GetRandomFileName();
                DestBack = DestFile + "-" + System.IO.Path.GetRandomFileName();
            }
            while (System.IO.File.Exists(TempDest) | System.IO.File.Exists(DestBack));

            System.IO.File.Copy(SourceFile, TempDest, false);
            System.IO.File.Move(DestFile, DestBack);
            System.IO.File.Move(TempDest, DestFile);
            System.IO.File.Delete(DestBack);
        }

        // "Source" is "current side", with the corresponding side stored in "Side"
        private void Do_Tasks(SideOfSource Side, StatusData.SyncStep CurrentStep)
        {
            SetIntCall IncrementCallback = new SetIntCall(Increment);

            string Source = Side == SideOfSource.Left ? LeftRootPath : RightRootPath;
            string Destination = Side == SideOfSource.Left ? RightRootPath : LeftRootPath;

            foreach (SyncingItem Entry in SyncingList)
            {
                if (Entry.Side != Side)
                    continue;

                string SourcePath = Source + Entry.Path;
                string DestPath = Destination + Entry.Path;

                try
                {
                    UpdateLabel(CurrentStep, Entry.Action == TypeOfAction.Delete ? SourcePath : Entry.Path);

                    switch (Entry.Type)
                    {
                        case  TypeOfItem.File:
                            {
                                switch (Entry.Action)
                                {
                                    case  TypeOfAction.Copy  :
                                        {
                                            CopyFile(SourcePath, DestPath);
                                            break;
                                        }

                                    case  TypeOfAction.Delete:
                                        {
                                            System.IO.File.SetAttributes(SourcePath, System.IO.FileAttributes.Normal);
                                            System.IO.File.Delete(SourcePath);
                                            Status.DeletedFiles += 1;
                                            break;
                                        }
                                }

                                break;
                            }

                        case  TypeOfItem.Folder:
                            {
                                switch (Entry.Action)
                                {
                                    case  TypeOfAction.Copy:
                                        {
                                            System.IO.Directory.CreateDirectory(DestPath);

                                            // FIXME: Folder attributes sometimes don't apply well.
                                            System.IO.File.SetAttributes(DestPath, System.IO.File.GetAttributes(SourcePath));

                                            // When a file is updated, so is its parent folder's last-write time.
                                            // LATER: Remove this line: manual copying doesn't preserve creation time.
                                            System.IO.Directory.SetCreationTimeUtc(DestPath, System.IO.Directory.GetCreationTimeUtc(SourcePath).AddHours(Handler.GetSetting<int>(ProfileSetting.TimeOffset, 0)));

                                            Status.CreatedFolders += 1;
                                            break;
                                        }

                                    case  TypeOfAction.Delete:
                                        {
                                            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                                            try
                                            {
                                                System.IO.Directory.Delete(SourcePath, true);
                                            }
                                            catch (Exception ex)
                                            {
                                                System.IO.DirectoryInfo DirInfo = new System.IO.DirectoryInfo(SourcePath);
                                                DirInfo.Attributes = System.IO.FileAttributes.Directory; // Using "DirInfo.Attributes = IO.FileAttributes.Normal" does just the same, and actually sets DirInfo.Attributes to "IO.FileAttributes.Directory"
                                                DirInfo.Delete();
                                            }
                                            Status.DeletedFolders += 1;
                                            break;
                                        }
                                }

                                break;
                            }
                    }
                    Status.ActionsDone += 1;
                    Log.LogAction(Entry, Side, true);
                }
                catch (System.Threading.ThreadAbortException StopEx)
                {
                    return;
                }

                catch (Exception ex)
                {
                    Log.HandleError(ex, SourcePath);
                    Log.LogAction(Entry, Side, false);
                }// Side parameter is only used for logging purposes.

                if (!Status.Cancel)
                    this.Invoke(IncrementCallback, new object[] { CurrentStep, 1 });
            }
        }



        private void Sync()
        {
            StepCompletedCall StepCompletedCallback = new StepCompletedCall(StepCompleted);
            SetIntCall SetMaxCallback = new SetIntCall(SetMax);

            // Restore original order before syncing.
            Sorter.SortColumn = -1; // Sorts according to initial index.
            Sorter.Order = SortOrder.Ascending;
            SyncingList.Sort(Sorter);

            this.Invoke(new Action(LaunchTimer));
            this.Invoke(SetMaxCallback, new object[] { StatusData.SyncStep.LR, Status.LeftActionsCount });
            Do_Tasks(SideOfSource.Left, StatusData.SyncStep.LR);
            this.Invoke(StepCompletedCallback, StatusData.SyncStep.LR);

            this.Invoke(SetMaxCallback, new object[] { StatusData.SyncStep.RL, Status.RightActionsCount });
            Do_Tasks(SideOfSource.Right, StatusData.SyncStep.RL);
            this.Invoke(StepCompletedCallback, StatusData.SyncStep.RL);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {

            switch (StopBtn.Text)
            {
                case "CANCEL":
                    {
                        EndAll();
                        break;
                    }

                case "CLOSE":
                    {
                        this.Close();
                        break;
                    }
                default:
                    break;
            }
        }

        private void EndAll()
        {
            Status.Cancel = Status.Cancel | (Status.CurrentStep != StatusData.SyncStep.Done);
            FullSyncThread.Abort();
            ScanThread.Abort(); SyncThread.Abort();
            StepCompleted(StatusData.SyncStep.Scan);
            StepCompleted(StatusData.SyncStep.LR);
            StepCompleted(StatusData.SyncStep.RL); // This call will sleep for 5s after displaying its failure message if the backup failed.
        }

        private ProgressBar GetProgressBar(StatusData.SyncStep Id)
        {
            switch (Id)
            {
                case StatusData.SyncStep.Scan:
                    {
                        return Step1ProgressBar;
                    }

                case StatusData.SyncStep.LR:
                    {
                        return Step2ProgressBar;
                    }

                default:
                    {
                        return Step3ProgressBar;
                    }
            }
        }

        private void SetMax(StatusData.SyncStep Id, int MaxValue) // Careful: MaxValue is an Integer.
        {
            bool Finished = false;
            ProgressBar CurBar = GetProgressBar(Id);

            CurBar.Style = ProgressBarStyle.Blocks;
            CurBar.Maximum = Math.Max(0, MaxValue);
            CurBar.Value = Finished ? MaxValue : 0;
        }

        private void SetMax(StatusData.SyncStep Id, int MaxValue, bool Finished = false) // Careful: MaxValue is an Integer.
        {
            ProgressBar CurBar = GetProgressBar(Id);

            CurBar.Style = ProgressBarStyle.Blocks;
            CurBar.Maximum = Math.Max(0, MaxValue);
            CurBar.Value = Finished ? MaxValue : 0;
        }

        private void UpdateLabel(StatusData.SyncStep Id, string Text)
        {
            string StatusText = Text;
            if (Text.Length > 30)
                StatusText = "..." + Text.Substring(Text.Length - 30, 30);

            switch (Id)
            {
                case StatusData.SyncStep.Scan:
                    {
                        StatusText = Translation.TranslateFormat(@"\STEP_1_STATUS", StatusText);
                        break;
                    }

                case  StatusData.SyncStep.LR:
                    {
                        StatusText = Translation.TranslateFormat(@"\STEP_2_STATUS", Step2ProgressBar.Value, Step2ProgressBar.Maximum, StatusText);
                        break;
                    }

                case  StatusData.SyncStep.RL:
                    {
                        StatusText = Translation.TranslateFormat(@"\STEP_3_STATUS", Step3ProgressBar.Value, Step3ProgressBar.Maximum, StatusText);
                        break;
                    }
            }

            lock (Lock)
            {
                Labels[(int)Id] = Text;
                StatusLabel = StatusText;
            }
        }

        private void UpdateStatuses()
        {
            long PreviousActionsDone = -1;
            bool CanDelete = (Handler.GetSetting<int>(ProfileSetting.Method, (int)ProfileSetting.DefaultMethod) == (int)ProfileSetting.SyncMethod.LRMirror);
            Status.TimeElapsed = (DateTime.Now - Status.StartTime) + new TimeSpan(1000000); // ie +0.1s

            string EstimateString = "";
            bool Copying = Status.CurrentStep == StatusData.SyncStep.LR | (!CanDelete & Status.CurrentStep == StatusData.SyncStep.RL);

            if (Status.CurrentStep == StatusData.SyncStep.Scan)
                Speed.Text = Math.Round(Status.FilesScanned / (double)Status.TimeElapsed.TotalSeconds).ToString() + " files/s";
            else if (CanDelete & Status.CurrentStep == StatusData.SyncStep.RL)
                Speed.Text = Math.Round(Status.DeletedFiles / (double)Status.TimeElapsed.TotalSeconds).ToString() + " files/s";
            else if (Copying && PreviousActionsDone != Status.ActionsDone)
            {
                PreviousActionsDone = Status.ActionsDone;

                Status.Speed = Status.BytesCopied / (double)Status.TimeElapsed.TotalSeconds;
                Speed.Text = Utilities.FormatSize(Status.Speed) + "/s";
            }


            if (Copying && Status.Speed > (1 << 10) && Status.TimeElapsed.TotalSeconds > ProgramSetting.ForecastDelay && ProgramConfig.GetProgramSetting<bool>(ProgramSetting.Forecast, false))
            {
                int TotalTime = System.Convert.ToInt32(Math.Min(int.MaxValue, Status.BytesToCopy / (double)Status.Speed));
                EstimateString = string.Format(" / ~{0}", Utilities.FormatTimespan(new TimeSpan(0, 0, TotalTime)));
            }

            ElapsedTime.Text = Utilities.FormatTimespan(Status.TimeElapsed) + EstimateString;

            Done.Text = Status.ActionsDone + "/" + Status.TotalActionsCount;
            FilesDeleted.Text = Status.DeletedFiles + "/" + Status.FilesToDelete;
            FilesCreated.Text = Status.CreatedFiles + "/" + Status.FilesToCreate + " (" + Utilities.FormatSize(Status.BytesCopied) + ")";
            FoldersDeleted.Text = Status.DeletedFolders + "/" + Status.FoldersToDelete;
            FoldersCreated.Text = Status.CreatedFolders + "/" + Status.FoldersToCreate;

            lock (Lock)
            {
                if (Labels != null)
                {
                    Step1StatusLabel.Text = Labels[1];
                    Step2StatusLabel.Text = Labels[2];
                    Step3StatusLabel.Text = Labels[3];
                }
                Interaction.StatusIcon.Text = StatusLabel;
            }

            int PercentProgress;
            if (Status.CurrentStep == StatusData.SyncStep.Scan)
                PercentProgress = 0;
            else if (Status.CurrentStep == StatusData.SyncStep.Done || Status.TotalActionsCount == 0)
                PercentProgress = 100;
            else
                PercentProgress = System.Convert.ToInt32(100 * Status.ActionsDone / (double)Status.TotalActionsCount);

            // Later: No need to update every time when CurrentStep \in {Scan, Done}
            this.Text = string.Format("({0}%) ", PercentProgress) + TitleText; // Feature requests #3037548, #3055740
        }

        private void ShowPreviewList()
        {
            // This part computes acceptable defaut values for column widths, since using VirtualMode prevents from resizing based on actual values.
            // This part requires that VirtualMode be set to False.
            SyncingItem i1 = new SyncingItem() { Action = TypeOfAction.Copy, Side = SideOfSource.Left, Type = TypeOfItem.File, Path = "".PadLeft(260) };
            SyncingItem i2 = new SyncingItem() { Action = TypeOfAction.Copy, Side = SideOfSource.Right, Type = TypeOfItem.File, IsUpdate = true };
            SyncingItem i3 = new SyncingItem() { Action = TypeOfAction.Delete, Side = SideOfSource.Right, Type = TypeOfItem.Folder };

            PreviewList.Items.Add(i1.ToListViewItem());
            PreviewList.Items.Add(i2.ToListViewItem());
            PreviewList.Items.Add(i3.ToListViewItem());

            PreviewList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            PreviewList.Items.Clear();

            PreviewList.VirtualMode = true;
            PreviewList.Visible = true;
            PreviewList.VirtualListSize = SyncingList.Count;

            if (!(Status.Cancel | Handler.GetSetting<bool>(ProfileSetting.PreviewOnly, false)))
                SyncBtn.Enabled = true;
        }

        public void RunPostSync()
        {
            // Search for a post-sync action, requiring that Expert mode be enabled.
            string PostSyncAction = Handler.GetSetting<string>(ProfileSetting.PostSyncAction);

            if (ProgramConfig.GetProgramSetting<bool>(ProgramSetting.ExpertMode, false) && PostSyncAction != null)
            {
                try
                {
                    Environment.CurrentDirectory = Application.StartupPath;
                    Interaction.ShowBalloonTip(string.Format(Translation.Translate(@"\POST_SYNC"), PostSyncAction));
                    System.Diagnostics.Process.Start(PostSyncAction, string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", Handler.ProfileName, !(Status.Cancel | Status.Failed), Log.Errors.Count, LeftRootPath, RightRootPath, Handler.ErrorsLogPath));
                }
                catch (Exception Ex)
                {
                    string Err = Translation.Translate(@"\POSTSYNC_FAILED") + Environment.NewLine + Ex.Message;
                    Interaction.ShowBalloonTip(Err);
                    ProgramConfig.LogAppEvent(Err);
                }
            }
        }

        private void StepCompleted(StatusData.SyncStep StepId)
        {
            if (!(Status.CurrentStep == StepId))
                return; // Prevents a potentially infinite exit loop.

            SetMax(StepId, 100, true);
            UpdateLabel(StepId, Translation.Translate(@"\FINISHED"));
            UpdateStatuses();

            switch (StepId)
            {
                case StatusData.SyncStep.Scan:
                    {
                        SyncingTimer.Stop();
                        Status.CurrentStep = StatusData.SyncStep.LR;
                        if (Preview)
                        {
                            ShowPreviewList();
                            StopBtn.Text = StopBtn.Tag.ToString().Split(';')[1];
                        }

                        break;
                    }

                case  StatusData.SyncStep.LR:
                    {
                        Status.CurrentStep = StatusData.SyncStep.RL;
                        break;
                    }

                case  StatusData.SyncStep.RL:
                    {
                        SyncingTimer.Stop();
                        Status.CurrentStep = StatusData.SyncStep.Done;

                        UpdateStatuses(); // Last update, to remove forecasts.

                        if (Status.Failed)
                            Interaction.ShowBalloonTip(Status.FailureMsg);
                        else if (Log.Errors.Count > 0)
                        {
                            PreviewList.Visible = true;
                            Status.ShowingErrors = true;

                            PreviewList.VirtualMode = true; // In case it hadn't been enabled (ie. if there was no preview)
                            PreviewList.VirtualListSize = Log.Errors.Count;

                            PreviewList.Columns.Clear();
                            PreviewList.Columns.Add(Translation.Translate(@"\ERROR"));
                            PreviewList.Columns.Add(Translation.Translate(@"\ERROR_DETAIL"));
                            PreviewList.Columns.Add(Translation.Translate(@"\PATH"));

                            PreviewList.Refresh();
                            PreviewList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

                            if (!Status.Cancel)
                                Interaction.ShowBalloonTip(Translation.TranslateFormat(@"\SYNCED_W_ERRORS", Handler.ProfileName), Handler.LogPath);
                        }
                        else if (!Status.Cancel)
                            Interaction.ShowBalloonTip(Translation.TranslateFormat(@"\SYNCED_OK", Handler.ProfileName), Handler.LogPath);

                        Log.SaveAndDispose(LeftRootPath, RightRootPath, Status);
                        if (!(Status.Failed | Status.Cancel))
                            Handler.SetLastRun(); // Required to implement catching up

                        RunPostSync();

                        if ((CommandLine.Quiet & !this.Visible) | CommandLine.NoStop)
                            this.Close();
                        else
                        {
                            //这儿有点问题！！！
                            StopBtn.Text = "So Bad!";
                            //StopBtn.Text = StopBtn.Tag.ToString().Split(';')[1];
                        }
                        break;
                    }
            }
        }



        private void SyncBtn_Click(object sender, EventArgs e)
        {
            PreviewList.Visible = false;
            SyncBtn.Visible = false;
            StopBtn.Text = StopBtn.Tag.ToString().Split(';')[0];

            SyncThread.Start();
        }
    }
}
