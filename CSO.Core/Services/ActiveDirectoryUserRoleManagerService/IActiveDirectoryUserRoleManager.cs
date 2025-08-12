using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Services.ActiveDirectoryUserRoleManagerService;

public interface IActiveDirectoryUserRoleManager
{
    string GetLoginUserName();
}
