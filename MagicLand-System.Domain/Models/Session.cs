﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MagicLand_System.Domain.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public int NoSession { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }
        public Topic? Topic { get; set; }

        public QuestionPackage? QuestionPackage { get; set; }

        public ICollection<SessionDescription>? SessionDescriptions { get; set; } = new List<SessionDescription>();

    }
}
