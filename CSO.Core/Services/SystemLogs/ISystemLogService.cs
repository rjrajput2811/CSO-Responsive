namespace CSO.Core.Services.SystemLogs;

public interface ISystemLogService
{
    bool WriteLog(string strMessage);
}
