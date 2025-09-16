using CSO.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSO.Core.DatabaseContext;

[Table("EmailOTPs")]
public class EmailOTP : SqlTable
{
    public required string Email { get; set; }
    public int OTP { get; set; }
    public DateTime ExpiresAt { get; set; }
}
