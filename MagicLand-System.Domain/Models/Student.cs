﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace MagicLand_System.Domain.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? AvatarImage { get; set; }
        public DateTime AddedTime { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; } = true;

        [ForeignKey("User")]
        public Guid ParentId { get; set; }
        public required User Parent { get; set; }


        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();
    }
}
