﻿namespace MagicLand_System.PayLoad.Response.Schedules.ForLecturer
{
    public class LectureScheduleResponse : ScheduleWithoutLectureResponse
    {
        public required Guid ClassId { get; set; }
        public required Guid CourseId { get; set; }
        public required string ClassName { get; set; }
        public required string ClassSubject { get; set; }
        public required string ClassCode { get; set; }
        public required string Method { get; set; }
        public required string Address { get; set; }
    }
}
