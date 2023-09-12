using System;
using System.Collections.Generic;

namespace FirstCodingExam.Models;

public partial class Record
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Amount { get; set; }

    public int LowerBoundInterestRate { get; set; }

    public int UpperBoundInterestRate { get; set; }

    public int IncrementalRate { get; set; }

    public int MaturityYears { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CalculatedRecord> CalculatedRecords { get; set; } = new List<CalculatedRecord>();

    public virtual ICollection<HistoryRecord> HistoryRecords { get; set; } = new List<HistoryRecord>();

    public virtual User User { get; set; } = null!;
}
