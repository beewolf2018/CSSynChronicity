using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSSynChronicity
{
    struct StatusData
    {
        public  enum SyncStep
        {
            Done,
            Scan,
            LR,
            RL
        }

        public DateTime StartTime;

        public long BytesCopied;
        public long BytesToCopy;

        public long FilesScanned;

        public long CreatedFiles;
        public long CreatedFolders;
        public long FilesToCreate;
        public long FoldersToCreate;

        public long DeletedFiles;
        public long DeletedFolders;
        public long FilesToDelete;
        public long FoldersToDelete;

        public long ActionsDone;
        public long TotalActionsCount;// ' == SyncingList.Count
        public int LeftActionsCount;// ' Used to set a ProgressBar's maximum => Integer
        public int RightActionsCount;

        public SyncStep CurrentStep;
        public TimeSpan TimeElapsed;
        public double Speed;

        public bool Cancel;//'= False
        public bool Failed;// '= False
        public bool ShowingErrors;// '= False

        public string FailureMsg;
    }

    public enum  TypeOfItem:int
    {
        File = 0,
        Folder = 1
    }


    public enum TypeOfAction : int
    {
        Delete = -1,
        None = 0,
        Copy = 1
    }

    public enum SideOfSource : int
    {
        Left = 0,
        Right = 1
    }


    struct SyncingContext
    {
        public SideOfSource Source;
        public string SourceRootPath;
        public string DestinationRootPath;
    }

    class SyncingItem
    {
        public string Path;
        public TypeOfItem Type;
        public SideOfSource Side;

        LanguageHandler Translation =  LanguageHandler.GetSingleton();

        public bool IsUpdate;
        public TypeOfAction Action;

        public int RealId;//' Keeps track of the order in which items where inserted in the syncing list, hence making it possible to recover this insertion order even after sorting the list on other criterias

        public string FormatType() {
            switch (Type)
            {
               
                case TypeOfItem.File:
                    return Translation.Translate("\\FILE");
                default:
                    return Translation.Translate("\\FOLDER");
            }
         }
 
        public string FormatAction()
        {
            switch (Action)
            {

                case TypeOfAction.Copy:
                    return IsUpdate? Translation.Translate("\\UPDATE"):Translation.Translate("\\CREATE");
                case TypeOfAction.Delete:
                    return Translation.Translate("\\DELETE");
                default:
                    return Translation.Translate("\\NONE");
            }
        }


        public string FormatDirection()
        {
            switch (Side)
            {

                case SideOfSource.Left:
                    return Action == TypeOfAction.Copy? Translation.Translate("\\LR"):Translation.Translate("\\LEFT");
                case SideOfSource.Right:
                    return Action == TypeOfAction.Copy? Translation.Translate("\\RL"):Translation.Translate("\\RIGHT");
                default:
                    return "";
            }
        }
        public ListViewItem ToListViewItem()
        {
            ListViewItem ListItem = new ListViewItem(new string[] { FormatType(), FormatAction(), FormatDirection(), Path });
            int Delta = IsUpdate? 1: 0;
            switch(Action)
            {
                case TypeOfAction.Copy:
                    if(Type == TypeOfItem.Folder)
                    {

                    }
                    else if(Type == TypeOfItem.File)
                    {
                        switch(Side)
                        {
                            case SideOfSource.Left:
                                ListItem.ImageIndex = 0 + Delta;
                                break;
                            case SideOfSource.Right:
                                ListItem.ImageIndex = 2 + Delta;
                                break;
                        }
                    }
                    break;
                case TypeOfAction.Delete:
                    if(Type == TypeOfItem.Folder) { ListItem.ImageIndex = 7; }
                    else if (Type == TypeOfItem.File) { ListItem.ImageIndex = 4; }
                    break;
            }
            return ListItem;

        }
    }

    public class FileNamePattern
    {
        public enum PatternType
        {
            FileExt,
            FileName,
            FolderName,
            Regex
        }

        public PatternType Type;
        public string Pattern;
        public static ConfigHandler ProgramConfig =  ConfigHandler.GetSingleton();

        public FileNamePattern(PatternType _Type, string _Pattern)
        {
            Type = _Type;
            Pattern = _Pattern;
         }


        private static bool IsBoxed(char Frame, string Str)
        {
            return (Str.StartsWith(Frame.ToString()) && Str.EndsWith(Frame.ToString()) && Str.Length > 2);
        }


        private static string Unbox(string Str)
        {
            return Str.Substring(1, Str.Length - 2).ToLower(System.Globalization.CultureInfo.InvariantCulture);// ' ToLower: Careful on linux ; No need to check that length > 2 here: IsBoxed already has.
        }


        public static FileNamePattern GetPattern(string Str , bool IsFolder = false)
        {
            if (IsBoxed(' ', Str))// 'Filename
                return new FileNamePattern(IsFolder ? PatternType.FolderName : PatternType.FileName, Unbox(Str));
            else if (IsBoxed('/', Str)) // 'Regex
                return new FileNamePattern(PatternType.Regex, Unbox(Str));
            else
                return new FileNamePattern(PatternType.FileExt, Str.ToLower(System.Globalization.CultureInfo.InvariantCulture));
            
         }


        private static string SharpInclude(string FileName)
        {
            string Path = ProgramConfig.ConfigRootDir + ProgramSetting.DirSep + FileName;
            return File.Exists(Path) ? File.ReadAllText(Path) : FileName;
         }


        public static void LoadPatternsList(List<FileNamePattern> PatternsList, string PatternsStr, bool IsFolder, string FolderPrefix  = "")
        {
            List<string> Patterns = new List<string>(PatternsStr.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
    
            while( Patterns.Count > 0 && Patterns.Count < 1024)// 'Prevent circular references
            {
                string CurPattern = Patterns[0];
                //IsFolder = CurPattern.StartsWith(FolderPrefix);

                if (IsFolder = CurPattern.StartsWith(FolderPrefix))
                {
                    if (IsFolder) CurPattern = CurPattern.Substring(FolderPrefix.Length);



                    if (IsBoxed(':', CurPattern)) //'Load patterns from file
                    {
                           string SubPatterns = SharpInclude(Unbox(CurPattern));
                        Patterns.AddRange(SubPatterns.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    }
                    else
                    {
                        PatternsList.Add(GetPattern(CurPattern, IsFolder));
                    }
                }

                Patterns.RemoveAt(0);
            }
        }

        private static string GetExtension(string File)
        {
            return File.Substring(File.LastIndexOf('.') + 1);//Not used when dealing with a folder.
        }

        public static bool MatchesPattern( string PathOrFileName, List<FileNamePattern> Patterns)
        {

            string Extension = GetExtension(PathOrFileName);


            foreach (FileNamePattern Pattern in Patterns)// 'LINUX: Problem with IgnoreCase
            {
                switch (Pattern.Type)
                {
                    case FileNamePattern.PatternType.FileExt:
                        if (String.Compare(Extension, Pattern.Pattern, true) == 0) return true;
                        break;
                    case FileNamePattern.PatternType.FileName:
                    if (String.Compare(PathOrFileName, Pattern.Pattern, true) == 0 ) return true;
                        break;
                    case FileNamePattern.PatternType.FolderName:
                    if( PathOrFileName.EndsWith(Pattern.Pattern, StringComparison.CurrentCultureIgnoreCase))  return true;
                        break;
                    case FileNamePattern.PatternType.Regex:
                        if (System.Text.RegularExpressions.Regex.IsMatch(PathOrFileName, Pattern.Pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)) return true;
                        break;
                 }
            }

            return false;
        }
    }
 

    public class FileHandling
    {
        public string GetFileOrFolderName(string Path)
        {
            return Path.Substring(Path.LastIndexOf(ProgramSetting.DirSep) + 1);// 'IO.Path.* -> Bad because of separate file/folder handling.
        }
    }
}
