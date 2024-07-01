﻿using MagicLand_System.Constants;
using MagicLand_System.Enums;
using MagicLand_System.PayLoad.Request.Student;
using MagicLand_System.PayLoad.Response;
using MagicLand_System.PayLoad.Response.Classes;
using MagicLand_System.PayLoad.Response.Courses;
using MagicLand_System.PayLoad.Response.Schedules.ForStudent;
using MagicLand_System.PayLoad.Response.Students;
using MagicLand_System.PayLoad.Response.Users;
using MagicLand_System.Services.Interfaces;
using MagicLand_System.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagicLand_System.Controllers
{
    [ApiController]
    public class StudentController : BaseController<StudentController>
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        public StudentController(ILogger<StudentController> logger, IStudentService studentService, ICourseService courseService) : base(logger)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        #region document API Add New Student
        /// <summary>
        ///  Cho Phép Phụ Huynh Thêm Bé Vào Hệ Thống
        /// </summary>
        /// <param name="studentRequest">Chứa Các Thông Tin Của Bé</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "fullName": "Nguyễn Văn A",
        ///        "dateOfBirth": "3/8/2018",
        ///        "gender": "Nam",
        ///        "avatarImage":"url",
        ///        "email":"avannguyen@gmail.com",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Thông Tin Tài Khoản Của Bé</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpPost(ApiEndpointConstant.StudentEndpoint.StudentEnpointCreate)]
        [CustomAuthorize(Enums.RoleEnum.PARENT)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> AddStudent(CreateStudentRequest studentRequest)
        {
            if (studentRequest == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = $"Yêu Cầu Không Hợp Lệ",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            var response = await _studentService.AddStudent(studentRequest);
            return Ok(response);
        }

        #region document API Get Student Account Infor
        /// <summary>
        ///  Cho Phép Phụ Huynh Truy Suất Thông Tin Tài Khoản Của Các Bé
        /// </summary>
        /// <param name="studentId">Id Của Bé (Option)</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": "3c1849af-400c-43ca-979e-58c71ce9301d"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Thông Tin Tài Khoản Của Bé</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpPost(ApiEndpointConstant.StudentEndpoint.GetStudentAccount)]
        [CustomAuthorize(Enums.RoleEnum.PARENT)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetStudentAccountIfor([FromQuery] Guid? studentId)
        {
            var result = await _studentService.GetStudentAccountAsync(studentId);
            return Ok(result);
        }

        #region document API Get Current Student Class
        /// <summary>
        ///  Truy Suất Các Lớp Học Của Học Sinh
        /// </summary>
        /// <param name="studentId">Id Của Học Sinh</param>
        /// <param name="status">Trạng Thái Của Lớp Học (Option)</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "studentId": "3c1849af-400c-43ca-979e-58c71ce9301d",
        ///        "status": "Upcoming"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Thông Tin Lớp Của Bé</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.StudentEndpoint.StudentEndpointGetClass)]
        [ProducesResponseType(typeof(ClassWithSlotShorten), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [CustomAuthorize(Enums.RoleEnum.PARENT)]
        public async Task<IActionResult> GetClassFromStudent([FromQuery] string studentId, [FromQuery] string status = null)
        {
            var response = await _studentService.GetClassOfStudent(studentId, status);
            if (response == null || response.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "Not found any class",
                    StatusCode = StatusCodes.Status404NotFound,
                    TimeStamp = DateTime.Now,
                });
            }
            return Ok(response);
        }
        [HttpGet(ApiEndpointConstant.StudentEndpoint.StudentGetSchedule)]
        [ProducesResponseType(typeof(StudentScheduleResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [CustomAuthorize(Enums.RoleEnum.PARENT)]
        public async Task<IActionResult> GetScheduleFromStudent([FromQuery] string studentId, [FromQuery] Guid? classId, [FromQuery] DateTime? date)
        {
            var response = await _studentService.GetScheduleOfStudent(studentId, classId, date);
            if (response == null || response.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "Not found any schedule",
                    StatusCode = StatusCodes.Status404NotFound,
                    TimeStamp = DateTime.Now,
                });
            }
            return Ok(response);
        }
        [HttpGet(ApiEndpointConstant.StudentEndpoint.GetStudentsOfCurrentUser)]
        [ProducesResponseType(typeof(StudentWithAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [CustomAuthorize(Enums.RoleEnum.PARENT)]
        public async Task<IActionResult> GetStudentFromCurentUser()
        {
            var response = await _studentService.GetStudentsOfCurrentParent();
            if (response == null || response.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "Not found any children",
                    StatusCode = StatusCodes.Status404NotFound,
                    TimeStamp = DateTime.Now,
                });
            }
            response.OrderByDescending(r => r.AddedTime);
            return Ok(response);
        }


        #region document API update student
        /// <summary>
        ///  Cho Phép Phụ Huynh Cập Nhập Học Sinh
        /// </summary>
        /// <param name="request">Chứa Thông Tin Của Học Sinh</param>
        /// <remarks>
        /// Sample request:
        ///{     
        ///    "StudentId":"3c1849af-400c-43ca-979e-58c71ce9301d",
        ///    "FullName":"Nguyen Van A",
        ///    "DateOfBirth":"2024/1/19",
        ///    "Gender":"Name",
        ///    "AvatarImage":"url",
        ///    "Email":"avannguyen@gmail.com"
        ///    
        ///}
        /// </remarks>
        /// <response code="200">Trả Về Thông Tin Học Sinh Sau Khi Cập Nhập</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpPut(ApiEndpointConstant.StudentEndpoint.UpdateStudent)]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentRequest request)
        {
            var response = await _studentService.UpdateStudentAsync(request);

            return Ok(response);
        }


        #region document API delete student
        /// <summary>
        ///  Chó Phép Phụ Huynh Xóa Thông Tin Học Sinh Khỏi Hệ Thống
        /// </summary>
        /// <param name="id">Id Của Học Sinh </param>
        /// <remarks>
        /// Sample request:
        ///{     
        ///    "Id":"3c1849af-400c-43ca-979e-58c71ce9301d",
        ///    
        ///}
        /// </remarks>
        /// <response code="200">Trả Về Thông Báo Sau Khi Xóa</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpDelete(ApiEndpointConstant.StudentEndpoint.DeleteStudent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {

            var response = await _studentService.DeleteStudentAsync(id);

            return Ok(response);
        }

        #region document API Get Student Course Registered By Id
        /// <summary>
        ///  Truy Suất Các Khóa Học Đã Đăng Ký Của Học Sinh Thông Qua Id Của Học Sinh
        /// </summary>
        /// <param name="id">Id Của Học Sinh</param>
        /// <remarks>
        /// Sample request:
        ///{     
        ///    "id":"3c1849af-400c-43ca-979e-58c71ce9301d"
        ///}
        /// </remarks>
        /// <response code="200">Trả Danh Sách Các Lớp Đã Đăng Ký</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.StudentEndpoint.GetStudentCourseRegistered)]
        [ProducesResponseType(typeof(CourseWithScheduleShorten), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentCourseRegistered([FromRoute] Guid id)
        {
            var response = await _courseService.GetCoursesOfStudentByIdAsync(id);
            return Ok(response);
        }

        #region document API Get Student By Id
        /// <summary>
        ///  Truy Suất Thông Tin Của Học Sinh Thông Qua Id Của Học Sinh
        /// </summary>
        /// <param name="id">Id Của Học Sinh</param>
        /// <remarks>
        /// Sample request:
        ///{     
        ///    "id":"3c1849af-400c-43ca-979e-58c71ce9301d"
        ///}
        /// </remarks>
        /// <response code="200">Trả Về Thông Tin Của Học Sinh</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.StudentEndpoint.StudentEndpointGet)]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadHttpRequestException))]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentById([FromRoute] Guid id)
        {
            var response = await _studentService.GetStudentById(id);

            return Ok(response);
        }


        #region document API Get Student Register Statistic
        /// <summary>
        ///  Truy Suất Danh Sách Học Sinh Mới Đã Thêm Vào Hệ Thống Theo Thời Gian Hiện Tại
        /// </summary>
        /// <param name="time">Truy Suất Doanh Sách Học Sinh Mới Thêm Vào Hệ Thống Theo Thời Gian Đã Chọn, Mặc Định Là Theo Tuần Hiện Tại</param>
        /// <response code="200">Trả Về Danh Sách Học Sinh Theo Thời Gian | Trả Về Rỗng Khi Không Có Học Sinh Mới Nào Được Thêm Trong Thời Gian Đã Chọn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.StudentEndpoint.GetStatisticRegisterStudent)]
        [ProducesResponseType(typeof(StudentStatisticResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadHttpRequestException))]
        [AllowAnonymous]
        public async Task<IActionResult> GetRevenueTransactionByTime([FromQuery] PeriodTimeEnum time)
        {
            var response = await _studentService.GetStatisticNewStudentRegisterAsync(time);
            return Ok(response);
        }

        #region document API Get Student All Student Learning Progress
        /// <summary>
        ///  Cho Phép Phụ Huynh Truy Suất Phần Trăm Các Tiến Độ Của Bé Trong Một Lớp
        /// </summary>
        /// <param name="studentId">Id Của Học Sinh</param>
        /// <param name="classId">Id Của Lớp Học</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "studentId": "3c1849af-400c-43ca-979e-58c71ce9301d",
        ///        "classId":"5229E1A5-79F9-48A5-B8ED-0A53F963CB0a"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Phần Trăm Tiến Độ Của Học Sinh</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpPost(ApiEndpointConstant.StudentEndpoint.GetStudentLearningProgress)]
        [ProducesResponseType(typeof(StudentLearningProgress), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> GetStudentLearningProgress([FromQuery] Guid studentId, [FromQuery] Guid classId)
        {
            var result = await _studentService.GetStudentLearningProgressAsync(studentId, classId);
            return Ok(result);
        }

        #region document API Get Day Valid Off
        /// <summary>
        ///  Cho Phép Phụ Huynh Truy Suất Các Ngày Phù Hợp Cho Bé Học Bù
        /// </summary>
        /// <param name="studentId">Id Của Học Sinh</param>
        /// <param name="classId">Id Của Lớp Học</param>
        /// <param name="dayOffs">Các Ngày Muốn Nghĩ</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "studentId": "3c1849af-400c-43ca-979e-58c71ce9301d",
        ///        "classId":"5229E1A5-79F9-48A5-B8ED-0A53F963CB0a",
        ///        "dayOffs": 
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Các Lịch Học Phù Hợp</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpPost(ApiEndpointConstant.StudentEndpoint.FindValidDayReLearning)]
        [ProducesResponseType(typeof(ScheduleReLearn), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> FindValidDayReLearning([FromQuery] Guid studentId, [FromQuery] Guid classId, [FromQuery] List<DateOnly> dayOffs)
        {
            if (dayOffs.Count == 0 || dayOffs.Count > 4)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = $"Số Lượng Ngày Nghĩ Không Hợp Lệ, Khi Bằng [0] Hoặc Vượt Quá Số Ngày Nghĩ Cho Phép [4]",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            var result = await _studentService.FindValidDayReLearningAsync(studentId, classId, dayOffs);
            return Ok(result);
        }
    }
}
