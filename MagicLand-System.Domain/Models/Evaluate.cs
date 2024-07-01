﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MagicLand_System.Domain.Models
{
    public class Evaluate
    {
        public Guid Id { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public bool? IsPublic { get; set; } = true;
        public Guid? MakeUpFromScheduleId { get; set; } = null;

        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Schedule")]
        public Guid ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
