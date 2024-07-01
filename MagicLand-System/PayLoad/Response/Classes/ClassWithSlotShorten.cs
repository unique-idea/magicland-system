﻿using MagicLand_System.PayLoad.Response.Schedules;
using MagicLand_System.PayLoad.Response.Users;

namespace MagicLand_System.PayLoad.Response.Classes
{
    public class ClassWithSlotShorten : ClassResponse
    {
        public string? CourseName { get; set; } = "Undefine";
        public double RateScore { get; set; } = 0;
        public UserResponse? Lecture { get; set; } = default!;
        //public ScheduleShortenResponse? Schedules { get; set; } = new ScheduleShortenResponse();
        public List<ScheduleShortenResponse> Schedules { get; set; } = new List<ScheduleShortenResponse>();
    }
}
