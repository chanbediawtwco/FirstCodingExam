using System;
using System.Collections.Generic;

namespace FirstCodingExam.Models;

public partial class HistoryCalculatedRecord
{
    public int Id { get; set; }

    public int? HistoryRecordId { get; set; }

    public int Years { get; set; }

    public double CurrentAmount { get; set; }

    public int InterestRate { get; set; }

    public double FutureAmount { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual HistoryRecord? HistoryRecord { get; set; }
}
