namespace CSO.Core.Models;

public class EmailOTPViewModel
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public int OTP { get; set; }
    public DateTime ExpiresAt { get; set; }
}
