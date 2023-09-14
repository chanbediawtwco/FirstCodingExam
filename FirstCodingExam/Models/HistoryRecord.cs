using System;
using System.Collections.Generic;

namespace FirstCodingExam.Models;

public partial class HistoryRecord
{
    public int Id { get; set; }

    public int RecordId { get; set; }

    public int UserId { get; set; }

    public double Amount { get; set; }

    public int LowerBoundInterestRate { get; set; }

    public int UpperBoundInterestRate { get; set; }

    public int IncrementalRate { get; set; }

    public int MaturityYears { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<HistoryCalculatedRecord> HistoryCalculatedRecords { get; set; } = new List<HistoryCalculatedRecord>();

    public virtual Record Record { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
