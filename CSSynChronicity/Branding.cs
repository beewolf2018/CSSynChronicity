using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSynChronicity
{
    class Branding
    {
        public const string Brand = "Beewolf Studio";
        public const string Name = "CSSynchronicity";
        public const string Web = "http://synchronicity.sourceforge.net/";
        public const string CompanyWeb = "http://createsoftware.users.sourceforge.net/";
        public const string Support = "mailto:createsoftware@users.sourceforge.net";
        public const string License = "http://www.gnu.org/licenses/gpl.html";
        public const string BugReport = "http://sourceforge.net/tracker/?group_id=264348&atid=1130882";

        public const string UpdatesTarget = Branding.Web + "update.html";
        public const string UpdatesUrl = Branding.Web + "code/version.txt";
        public const string UpdatesSchedulerUrl = Branding.Web + "code/scheduler-version.txt";
        public const string UpdatesFallbackUrl = Branding.CompanyWeb + "code/synchronicity-version.txt";

        public const string Help = Branding.Web + "help.html";
        public const string SettingsHelp = Branding.Web + "settings-help.html";
        public const string Contribute = Branding.Web + "contribute.html";
    }
}
