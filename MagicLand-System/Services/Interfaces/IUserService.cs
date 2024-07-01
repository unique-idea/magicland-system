﻿using MagicLand_System.Domain.Models;
using MagicLand_System.Enums;
using MagicLand_System.PayLoad.Request;
using MagicLand_System.PayLoad.Request.Checkout;
using MagicLand_System.PayLoad.Request.Class;
using MagicLand_System.PayLoad.Request.User;
using MagicLand_System.PayLoad.Response;
using MagicLand_System.PayLoad.Response.Bills;
using MagicLand_System.PayLoad.Response.Lectures;
using MagicLand_System.PayLoad.Response.Schedules.ForLecturer;
using MagicLand_System.PayLoad.Response.Students;
using MagicLand_System.PayLoad.Response.Users;

namespace MagicLand_System.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<LecturerCareerResponse>> GetLecturerCareerAsync();
        Task<bool> AddUserAsync(UserAccountRequest request);
        Task<List<User>> GetUsers(string? keyWord, RoleEnum? role);
        Task<UserExistRespone> CheckUserExistByPhone(string phone);
        Task<LoginResponse> Authentication(LoginRequest loginRequest);
        Task<User> GetCurrentUser();
        Task<NewTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<bool> RegisterNewUser(RegisterRequest registerRequest);
        Task<List<LecturerResponse>> GetLecturers(FilterLecturerRequest? request);
        Task<UserResponse> UpdateUserAsync(UserRequest request);
        Task<List<LectureScheduleResponse>> GetLectureScheduleAsync(Guid? classId);
        Task<List<AdminLecturerResponse>> GetAdminLecturerResponses(DateTime? startDate , DateTime? EndDate , string? searchString,string? slotId);
        Task<UserResponse> GetUserFromPhone(string phone);
        Task<List<StudentResponse>> GetStudents(string classId, string phone);
        Task<List<UserResponse>> GetUserFromName(string name);  
        Task<List<StudentResultResponse>> GetFromNameAndBirthDate(string? name , DateTime? birthdate,string? id);
        Task<ClassResultResponse> GetClassOfStudent(string studentId,string? status,string? searchstring,DateTime? dateTime);
        Task<List<StudentScheduleResponse>> GetScheduleOfStudentInDate(string studentId, DateTime date);
        Task<StudentSessionResponse> GetStudentSession(string scheduleId);
        Task<List<ClassScheduleResponse>> GetStudentSessionAsync(string classId,string studentId,DateTime? date);

    }
}
