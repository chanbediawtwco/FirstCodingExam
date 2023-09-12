using System;
using System.Collections.Generic;

namespace FirstCodingExam.Models;

public partial class CalculatedRecord
{
    public int Id { get; set; }

    public int RecordId { get; set; }

    public int Years { get; set; }

    public double CurrentAmount { get; set; }

    public int InterestRate { get; set; }

    public double FutureAmount { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Record Record { get; set; } = null!;
}
