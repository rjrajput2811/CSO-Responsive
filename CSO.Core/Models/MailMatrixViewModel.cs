﻿namespace CSO.Core.Models;

public class MailMatrixViewModel
{
    public int Id { get; set; }
    public int MailType { get; set; }
    public string? StakeHoldersEmailIds { get; set; }
    public string? RecipientUsers { get; set; }
    public string? Subject { get; set; }
    public string? MessageHeader { get; set; }
    public string? MailBody { get; set; }
    public string? MessageFooter { get; set; }
    public int AddedBy { get; set; }
    public DateTime? AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool Active { get; set; }
}
