﻿namespace MagicLand_System.PayLoad.Response.Courses
{
    public class RelatedCourseResponse
    {
        public Guid? CourseRelatedId { get; set; } = default;
        public string? Name { get; set; } = "Unkow";
        public string? Subject { get; set; } = "Unkow";
        public string? Image { get; set; } = "Unkow";
        public double? Price { get; set; } = 0.0;
        public int? MinAgeStudent { get; set; } = 0;
        public int? MaxAgeStudent { get; set; } = 120;
    }
}
