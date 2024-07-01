﻿using MagicLand_System.Domain.Models;
using MagicLand_System.Domain.Models.TempEntity.Class;
using MagicLand_System.Domain.Models.TempEntity.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace MagicLand_System.Domain
{
    public class MagicLandContext : DbContext
    {
        public MagicLandContext() { }
        public MagicLandContext(DbContextOptions<MagicLandContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PersonalWallet> PersonalWallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<StudentInCart> StudentIncarts { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Schedule> Sessions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SyllabusCategory> SyllabusCategories { get; set; }
        public DbSet<Syllabus> Syllabuses { get; set; }
        public DbSet<SubDescriptionTitle> SubDescriptionTitles { get; set; }
        public DbSet<SubDescriptionContent> SubDescriptionContents { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SessionDescription> SessionDescriptions { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ExamSyllabus> ExamSyllabuses { get; set; }
        public DbSet<QuestionPackage> QuestionPackages { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoice> MultipleChoice { get; set; }
        public DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<SideFlashCard> SideFlashCards { get; set; }
        public DbSet<LecturerField> LecturerFields { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        public DbSet<FlashCardAnswer> FlashCardAnswers { get; set; }
        public DbSet<CoursePrice> CoursePrices { get; set; }
        public DbSet<Rate> Rates { get; set; }
        /// <summary>
        /// Temp Entity
        /// </summary>
        public DbSet<TempQuiz> tempQuizzes { get; set; }
        public DbSet<TempQuestion> TempQuestions { get; set; }
        public DbSet<TempMCAnswer> TempMCAnswers { get; set; }
        public DbSet<TempFCAnswer> TempFCAnswers { get; set; }
        public DbSet<TempQuizTime> TempQuizTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:DefaultDB"]!;
            return strConn;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Phone, "UX_User_Phone");
                entity.Property(entity => entity.FullName).HasMaxLength(255);
                entity.Property(e => e.DateOfBirth).HasDefaultValueSql("getutcdate()");
                entity.HasOne(e => e.Role).WithMany(r => r.Accounts).HasForeignKey(e => e.RoleId).HasConstraintName("FK_USER_ROLE");
                entity.HasOne(e => e.LecturerField).WithMany(e => e.Users).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User).WithOne(e => e.Cart).HasForeignKey<User>(e => e.CartId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItem");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cart).WithMany(e => e.CartItems).HasForeignKey(e => e.CartId);
            });
            modelBuilder.Entity<StudentInCart>(entity =>
            {
                entity.ToTable("StudentInCart");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CartItem).WithMany(e => e.StudentInCarts).HasForeignKey(e => e.CartItemId);
            });
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Lecture).WithMany(e => e.Classes).HasForeignKey(entity => entity.LecturerId);
                entity.HasOne(e => e.Course).WithMany(r => r.Classes).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.StartDate).HasDefaultValueSql("getutcdate()");
                entity.Property(e => e.EndDate).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.ToTable("StudentClass");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Class).WithMany(e => e.StudentClasses).HasForeignKey(e => e.ClassId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Student).WithMany(e => e.StudentClasses).HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");
                entity.HasKey(e => e.Id);
                //entity.HasOne(e => e.Syllabus).WithOne(e => e.Course).HasForeignKey<Syllabus>(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Syllabus).WithOne(e => e.Course).HasForeignKey<Course>(e => e.SyllabusId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PersonalWallet>(entity =>
            {
                entity.ToTable("PersonalWallet");
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Balance).HasDefaultValue(0);

                entity.HasOne(e => e.User).WithOne(e => e.PersonalWallet).HasForeignKey<User>(e => e.PersonalWalletId);
            });
            modelBuilder.Entity<WalletTransaction>(entity =>
            {
                entity.ToTable("WalletTransaction");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.PersonalWallet).WithMany(e => e.WalletTransactions).HasForeignKey(e => e.PersonalWalletId);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");
                entity.HasKey(entity => entity.Id);
            });
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Class).WithMany(e => e.Schedules).HasForeignKey(e => e.ClassId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Room).WithMany(e => e.Schedules).HasForeignKey(e => e.RoomId);
                entity.HasOne(e => e.Slot).WithMany(e => e.Schedules).HasForeignKey(e => e.SlotId);

            });
            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");
                entity.HasKey(entity => entity.Id);
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Parent).WithMany(e => e.Students).HasForeignKey(e => e.ParentId);

            });
            modelBuilder.Entity<SyllabusCategory>(entity =>
            {
                entity.ToTable("SyllabusCategory");
                entity.HasKey(entity => entity.Id);
            });
            modelBuilder.Entity<SubDescriptionTitle>(entity =>
            {
                entity.ToTable("SubDescriptionTitle");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Course).WithMany(e => e.SubDescriptionTitles).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<SubDescriptionContent>(entity =>
            {
                entity.ToTable("SubDescriptionContent");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.SubDescriptionTitle).WithMany(e => e.SubDescriptionContents).HasForeignKey(e => e.SubDescriptionTitleId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Syllabus>(entity =>
            {
                entity.ToTable("Syllabus");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(entity => entity.SyllabusCategory).WithMany(e => e.Syllabuses).HasForeignKey(e => e.SyllabusCategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(entity => entity.Course).WithOne(e => e.Syllabus).HasForeignKey<Course>(e => e.SyllabusId).IsRequired(false);
            });
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Syllabus).WithMany(e => e.Topics).HasForeignKey(e => e.SyllabusId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Topic).WithMany(e => e.Sessions).HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.QuestionPackage).WithOne(e => e.Session).HasForeignKey<QuestionPackage>(e => e.SessionId).IsRequired(false);
            });
            modelBuilder.Entity<QuestionPackage>(entity =>
            {
                entity.ToTable("QuestionPackage");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Session).WithOne(e => e.QuestionPackage).HasForeignKey<QuestionPackage>(e => e.SessionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Schedule).WithMany(e => e.Attendances).HasForeignKey(e => e.ScheduleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Student).WithMany(e => e.Attendances).HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Evaluate>(entity =>
            {
                entity.ToTable("Evaluate");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.Schedule).WithMany(e => e.Evaluates).HasForeignKey(e => e.ScheduleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Student).WithMany(e => e.Evaluates).HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(entity => entity.Id);
                entity.HasOne(e => e.TargetUser).WithMany(e => e.Notifications).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<SessionDescription>(entity =>
            {
                entity.ToTable("SessionDescription");
                entity.HasKey(e => e.Id);
                entity.HasOne(x => x.Session).WithMany(e => e.SessionDescriptions).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("Material");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Syllabus).WithMany(e => e.Materials).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ExamSyllabus>(entity =>
            {
                entity.ToTable("ExamSyllabus");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Syllabus).WithMany(e => e.ExamSyllabuses).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.QuestionPackage).WithMany(e => e.Questions).HasForeignKey(e => e.QuestionPacketId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MultipleChoice>(entity =>
            {
                entity.ToTable("MultipleChoice");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Question).WithMany(e => e.MutipleChoices).HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<FlashCard>(entity =>
            {
                entity.ToTable("FlashCard");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Question).WithMany(e => e.FlashCards).HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<SideFlashCard>(entity =>
            {
                entity.ToTable($"{nameof(SideFlashCard)}");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.FlashCard).WithMany(e => e.SideFlashCards).HasForeignKey(e => e.FlashCardId).OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<LecturerField>(entity =>
            {
                entity.ToTable("LecturerField");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ExamResult>(entity =>
            {
                entity.ToTable("ExamResult");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.StudentClass).WithMany(e => e.ExamResults).HasForeignKey(e => e.StudentClassId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.ToTable("ExamQuestion");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ExamResult).WithMany(e => e.ExamQuestions).HasForeignKey(e => e.ExamResultResultId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(entity => entity.MultipleChoiceAnswer).WithOne(e => e.ExamQuestion).HasForeignKey<MultipleChoiceAnswer>(e => e.ExamQuestionId).IsRequired(false);

            });
            modelBuilder.Entity<MultipleChoiceAnswer>(entity =>
            {
                entity.ToTable("MultipleChoiceAnswer");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ExamQuestion).WithOne(e => e.MultipleChoiceAnswer).HasForeignKey<MultipleChoiceAnswer>(e => e.ExamQuestionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<FlashCardAnswer>(entity =>
            {
                entity.ToTable("FlashCardAnswer");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ExamQuestion).WithMany(e => e.FlashCardAnswers).HasForeignKey(e => e.ExamQuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TempQuiz>(entity =>
            {
                entity.ToTable("TempQuiz");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<TempQuestion>(entity =>
            {
                entity.ToTable("TempQuestion");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.TempQuiz).WithMany(e => e.Questions).HasForeignKey(e => e.TempQuizId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TempMCAnswer>(entity =>
            {
                entity.ToTable("TempMCAnswer");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.TempQuestion).WithMany(e => e.MCAnswers).HasForeignKey(e => e.TempQuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TempFCAnswer>(entity =>
            {
                entity.ToTable("TempFCAnswer");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.TempQuestion).WithMany(e => e.FCAnswers).HasForeignKey(e => e.TempQuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<CoursePrice>(entity =>
            {
                entity.ToTable("CoursePrice");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Course).WithMany(e => e.CoursePrices).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TempQuizTime>(entity =>
            {
                entity.ToTable("TempQuizTime");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("Rate");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Course).WithMany(e => e.Rates).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
