using System;
using System.Collections.Generic;

namespace FirstCodingExam.Models;

public partial class User
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<HistoryRecord> HistoryRecords { get; set; } = new List<HistoryRecord>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
