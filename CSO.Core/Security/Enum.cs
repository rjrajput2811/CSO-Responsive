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