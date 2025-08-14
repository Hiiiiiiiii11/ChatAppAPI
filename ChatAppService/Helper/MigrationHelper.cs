using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Helper
{
    public static class MigrationHelper
    {
        public static bool IsRunningMigration()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName != null && a.FullName.Contains("EntityFrameworkCore.Design"));
        }
    }
}
