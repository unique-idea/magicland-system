﻿namespace MagicLand_System.PayLoad.Response.Syllabuses
{
    public class QuestionPackageResponse
    {
        public Guid QuestionPackageId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? TypeName { get; set; } = string.Empty;
        public int NoOfSession { get; set; } = 1;
    }
}
