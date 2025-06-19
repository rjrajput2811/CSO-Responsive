namespace CSO.Core.Security;

public enum UserType
{
    Manager = 0,
    General = 1
}

public enum Permissions
{
    LogCSOOnly = 1,
    ViewAllCSO = 2,
    EditCSO = 3,
    DeleteCSO = 4,
    Rootcause = 5,
    Monitor = 6,
    Approve = 7,
    Closure = 8,
    Reports = 9,
    UserDetails = 10,
    AdminRights = 11,
    MailMatrixConfiguration = 12
}

public enum Status
{
    Open = 1,
    RootCause = 2,
    Monitor = 3,
    Approve = 4,
    Reject = 5,
    Close = 6
}

public enum RejectRevertStatus
{
    RootCause = 1,
    Monitor = 2
}

public enum Lavel4Status
{
    Approve = 3,
    Reject = 4
}

public enum Lavel5Status
{
    Approve = 3,
    Reject = 4,
    Close = 5
}

public enum CSOLogFileType
{
    CSOLOg = 0,
    RootCauseAnalysisFile = 1,
    CorrectiveActionFile = 2,
    PreventiveActionFile = 3,
    MonitoringofCorrectiveActionFile = 4
}