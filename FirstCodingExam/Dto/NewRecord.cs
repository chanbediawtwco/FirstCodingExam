using System.ComponentModel.DataAnnotations;

namespace FirstCodingExam.Dto
{
    public class NewRecord
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }
        [Required]
        public double? Amount { get; set; }

        [Required]
        public int? LowerBoundInterestRate { get; set; }

        [Required]
        public int? UpperBoundInterestRate { get; set; }

        [Required]
        public int? IncrementalRate { get; set; }

        [Required]
        public int? MaturityYears { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
