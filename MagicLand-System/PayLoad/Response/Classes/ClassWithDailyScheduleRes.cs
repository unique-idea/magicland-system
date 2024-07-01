﻿using MagicLand_System.PayLoad.Response.Schedules;
using MagicLand_System.PayLoad.Response.Users;

namespace MagicLand_System.PayLoad.Response.Classes
{
    public class ClassWithDailyScheduleRes : ClassResponse
    {
        public int CurrentSession {  get; set; }    
        public string? CourseName { get; set; }
        public required UserResponse Lecture { get; set; }
        public List<DailySchedule>? Schedules { get; set; }
    }
}
