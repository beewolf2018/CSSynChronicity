using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace CSSynChronicity
{
    public class LanguageHandler
    {
        public static LanguageHandler Singleton;
        public static ConfigHandler ProgramConfig =  ConfigHandler.GetSingleton();
        public struct LangInfo
        {
            public List<string> CodeNames;
            public string NativeName;
        }

        //    'Renames : non-english file name -> english file name
        private static Dictionary<string, string> Renames = new Dictionary<string, string> { { "francais", "french" }, { "deutsch", "german" } };
        private static string NewFilename(string  OldLanguageName)
        {
            return Renames.ContainsKey(OldLanguageName) ? Renames[OldLanguageName]: OldLanguageName;
        }

        private static string GetLanguageFilePath(string  LanguageName )
        {
           // ConfigHandler ProgramConfig = new ConfigHandler();
            return ProgramConfig.LanguageRootDir + ProgramSetting.DirSep + NewFilename(LanguageName) + ".lng";
        }

        public LanguageHandler()
        {
            ProgramConfig.LoadProgramSettings();
            Directory.CreateDirectory(ProgramConfig.LanguageRootDir);
            Dictionary<string, string>  Strings = new Dictionary<string, string>();
            string DictFile = GetLanguageFilePath(ProgramConfig.GetProgramSetting(ProgramSetting.Language, ProgramSetting.DefaultLanguage));
            if(!File.Exists(DictFile))
            {
                DictFile = GetLanguageFilePath(ProgramSetting.DefaultLanguage);
                Interaction.ShowMsg("No language file found!");
            }
            else
            {
                using(StreamReader Reader = new StreamReader(DictFile, Encoding.UTF8))
                {
                    while(!Reader.EndOfStream)
                    {
                        string Line = Reader.ReadLine();
                        if (Line.StartsWith("#") || !(Line.Contains("="))) continue;
                        string[]  Pair= Line.Split("=".ToCharArray(), 2);
                        try
                        {
                            if (Pair[0].StartsWith("->")) { Pair[0] = Pair[0].Remove(0, 2); }
                             Strings.Add("\\" + Pair[0], Pair[1].Replace("\n" , System.Environment.NewLine));
                        }
                        catch(ArgumentException ex)
                        {
                            Interaction.ShowDebug("Duplicate line in translation: " + Line);
                        }
                    }
                }
            }

        }
       public static LanguageHandler GetSingleton(bool Reload  = false)
        {
            if( Reload || (Singleton is  null)) { Singleton = new LanguageHandler(); }
                return Singleton;
        }
        Dictionary<string, string> Strings = new Dictionary<string, string>();
        public string Translate(string  Code,string  DefaultValue = "")
        {
            if (Code is null || Code == "") return DefaultValue;
            return Strings.ContainsKey(Code) ? Strings[Code] : (DefaultValue == "" ? Code : DefaultValue);
        }


        //#region "TranslateFormat"
        //    'ParamArray requires building objects array, and adds binsize.
        public string TranslateFormat(string Code , Object Arg0 )
        {
            return String.Format(Translate(Code), Arg0);
        }
        public string TranslateFormat(string Code, Object Arg0,Object Arg1)
        {
            return String.Format(Translate(Code), Arg0,Arg1);
        }
        public string TranslateFormat(string Code, Object Arg0, Object Arg1, Object Arg2)
        {
            return String.Format(Translate(Code), Arg0, Arg1,Arg2);
        }
        //#end region

        public void TranslateControl(Control Ctrl)
        {
            if (Ctrl is null) return;
            //'Add ; in tags so as to avoid errors when tag properties are split.
            Ctrl.Text = Translate(Ctrl.Text);
            TranslateControl(Ctrl.ContextMenuStrip);

            if(Ctrl is ListView)
            {
                ListView List = (ListView)Ctrl;
                foreach(ListViewGroup Group in List.Groups)
                {
                    Group.Header = Translate(Group.Header);
                }
                foreach (ColumnHeader Column in List.Columns)
                {
                    Column.Text = Translate(Column.Text);
                }
                if (! List.VirtualMode)
                {
                    foreach(ListViewItem Item in List.Items)
                    {
                        foreach (ListViewItem.ListViewSubItem SubItem in Item.SubItems)
                        {
                            SubItem.Text = Translate(SubItem.Text);
                            SubItem.Tag = Translate((SubItem.Tag is null)?"":SubItem.Tag.ToString(), ";");
                        }
                    }
                }
            }

            if (Ctrl is ContextMenuStrip)
            {
                ContextMenuStrip ContextMenu = (ContextMenuStrip)Ctrl;
                foreach (ToolStripItem Item in ContextMenu.Items)
                {
                    Item.Text = Translate(Item.Text);
                    Item.Tag = Translate((Item.Tag is null) ? "" : Item.Tag.ToString(), ";");
                    //Item.Tag = Translate(Item.Tag.ToString(), ";");
                }                
            }
            //try
            //{ Ctrl.Tag = Translate(Ctrl.Tag.ToString(), ";"); }
            //catch (Exception ex) { }
            foreach (Control ChildCtrl in Ctrl.Controls)
            {
                TranslateControl(ChildCtrl);
            }
        }

        public static void FillLanguagesComboBox(ComboBox LanguagesComboBox)
        {
            Dictionary<string, LangInfo> LanguagesInfo = new Dictionary<string, LangInfo>();
            if(File.Exists(ProgramConfig.LocalNamesFile))
            {
                using (StreamReader PropsReader = new StreamReader(ProgramConfig.LocalNamesFile))
                {
                    while(!PropsReader.EndOfStream)
                    {
                        string[] CurLanguage = PropsReader.ReadLine().Split(";".ToCharArray());
                        if (CurLanguage.Length != 3) continue;
                        LanguagesInfo.Add(CurLanguage[0], new LangInfo() { NativeName = CurLanguage[1], CodeNames = new List<string>(CurLanguage[2].ToLower(Interaction.InvariantCulture).Split('/')) });
                    }
                }
            }
            string SystemLanguageItem = "";
            string ProgramLanguageItem = "";
            string DefaultLanguageItem = "";
            System.Globalization.CultureInfo CurrentCulture = System.Globalization.CultureInfo.InstalledUICulture;
            ProgramConfig.LoadProgramSettings();
            string CurProgramLanguage = NewFilename(ProgramConfig.GetProgramSetting(ProgramSetting.Language, ""));
            if (!CurrentCulture.IsNeutralCulture) CurrentCulture = CurrentCulture.Parent;

            LanguagesComboBox.Items.Clear();

            foreach(string File in Directory.GetFiles(ProgramConfig.LanguageRootDir, "*.lng"))
            {
                string EnglishName = Path.GetFileNameWithoutExtension(File);
                string NewItemText = EnglishName;
                if(LanguagesInfo.ContainsKey(EnglishName))
                {
                    LanguageHandler.LangInfo Info = LanguagesInfo[EnglishName];
                    NewItemText = String.Format("{0} - {1} ({2})", EnglishName, Info.NativeName, Info.CodeNames[0]);
                    if (Info.CodeNames.Contains(CurrentCulture.Name.ToLower(Interaction.InvariantCulture))) SystemLanguageItem = NewItemText;
                }
                LanguagesComboBox.Items.Add(NewItemText);

                if (string.Compare(EnglishName, CurProgramLanguage, true) == 0) ProgramLanguageItem = NewItemText;
                if (string.Compare(EnglishName, ProgramSetting.DefaultLanguage, true) == 0) DefaultLanguageItem = NewItemText;
            }
            LanguagesComboBox.Sorted = true;
            if (!(ProgramLanguageItem == "")) LanguagesComboBox.SelectedItem = ProgramLanguageItem;
            else if (!(SystemLanguageItem == "")) LanguagesComboBox.SelectedItem = SystemLanguageItem;
            else if (!(DefaultLanguageItem == "")) LanguagesComboBox.SelectedItem = DefaultLanguageItem;
            else if (LanguagesComboBox.Items.Count > 0) LanguagesComboBox.SelectedIndex = 0;



        }



        //#If Debug And 0 Then
        //    Public Shared Sub EnumerateCultures()
        //        Dim Builder As New Text.StringBuilder
        //        For Each Culture As Globalization.CultureInfo In Globalization.CultureInfo.GetCultures(Globalization.CultureTypes.AllCultures)
        //            Builder.AppendLine(String.Join(Microsoft.VisualBasic.ControlChars.Tab, New String() { Culture.Name, Culture.Parent.Name, Culture.IsNeutralCulture.ToString, Culture.DisplayName, Culture.NativeName, Culture.EnglishName, Culture.TwoLetterISOLanguageName, Culture.ThreeLetterISOLanguageName, Culture.ThreeLetterWindowsLanguageName})) ', LangInfo.LocalName, LangInfo.IsoLanguageName, LangInfo.WindowsCode
        //        Next

        //        MessageBox.Show(Builder.ToString)
        //    End Sub
        //#End If
        //End Class

    }
}
