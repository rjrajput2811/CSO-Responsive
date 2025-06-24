namespace CSO.Core.Security;

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