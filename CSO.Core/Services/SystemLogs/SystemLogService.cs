using Microsoft.AspNetCore.Hosting;

namespace CSO.Core.Services.SystemLogs;

public class SystemLogService : ISystemLogService
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public SystemLogService(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public bool WriteLog(string strMessage)
    {
        try
        {
            // Ensure Log folder path
            var logFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Log");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            // Log file name: Log_ddMMyyyy.txt
            string fileName = $"Log_{DateTime.Now:ddMMyyyy}.txt";
            string filePath = Path.Combine(logFolderPath, fileName);

            // Append log message with timestamp in dd/MM/yyyy format
            using (FileStream objFilestream = new(filePath, FileMode.Append, FileAccess.Write))
            using (StreamWriter objStreamWriter = new(objFilestream))
            {
                string timeStampedMessage = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {strMessage}";
                objStreamWriter.WriteLine(timeStampedMessage);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
