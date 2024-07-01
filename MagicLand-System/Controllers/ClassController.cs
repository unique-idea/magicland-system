﻿using MagicLand_System.Constants;
using MagicLand_System.Enums;
using MagicLand_System.PayLoad.Request.Class;
using MagicLand_System.PayLoad.Response;
using MagicLand_System.PayLoad.Response.Classes;
using MagicLand_System.PayLoad.Response.Students;
using MagicLand_System.PayLoad.Response.Topics;
using MagicLand_System.Services.Interfaces;
using MagicLand_System.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MagicLand_System.Controllers
{
    [ApiController]
    public class ClassController : BaseController<ClassController>
    {
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        private readonly IWalletTransactionService _walletTransactionService;
        public ClassController(ILogger<ClassController> logger, IClassService classService, IWalletTransactionService walletTransactionService, IStudentService studentService) : base(logger)
        {
            _classService = classService;
            _walletTransactionService = walletTransactionService;
            _studentService = studentService;
        }

        #region document API Get Classes
        /// <summary>
        ///  Truy Suất Toàn Bộ Lớp Học
        /// </summary>
        /// <param name="time">Nếu Có Giá Trị Khác [Default] Truy Suất Toàn Bộ Lớp Có Thời Gian Bắt Đầu Trong 1 Tuần/Tháng Tới</param>
        /// <response code="200">Trả Về Danh Sách Lớp Học</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetClasses([FromQuery] PeriodTimeEnum time)
        {

            var classes = await _classService.GetClassesAsync(time);
            return Ok(classes);
        }

        #region document API Get Class By Course Id
        /// <summary>
        ///  Truy Suất Toàn Bộ Lớp Của Một Khóa
        /// </summary>
        /// <param name="id">Id Của Khóa Học</param>
        /// <param name="classStatus">Trạng Thái Của Lớp Học</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": "fded66d4-c3e7-4721-b509-e71feab6723a",
        ///        "classStatus":"UPCOMING",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.ClassByCourseId)]
        [ProducesResponseType(typeof(ClassWithSlotShorten), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetClassByCourseId(Guid id, [FromQuery] ClassStatusEnum classStatus)
        {
            var classes = await _classService.GetClassesByCourseIdAsync(id, classStatus);
            return Ok(classes);
        }

        #region document API Get Class Not In Cart
        /// <summary>
        ///  Truy Suất Các Lớp Học Không Thuộc Giỏ Hàng Của Người Dùng Hiện Tại
        /// </summary>
        /// <param name="courseId">Id Của Khóa Học (Option)</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "courseId": "fded66d4-c3e7-4721-b509-e71feab6723a"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetAllClassNotInCart)]
        [ProducesResponseType(typeof(ClassWithSlotShorten), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> GetClassNotInCart([FromQuery] Guid? courseId)
        {
            var classes = await _classService.GetClassesNotInCartAsync(courseId);
            return Ok(classes);
        }

        #region document API Get Topic Learning  
        /// <summary>
        ///  Truy Suất Nội Dung Chủ Đề Dựa Vào Thử Tự Chủ Đề
        /// </summary>
        /// <param name="classId">Id Của Lớp Học</param>
        /// <param name="orderTopic">Thứ Tự Của Chủ Đề</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "classId": "fded66d4-c3e7-4721-b509-e71feab6723a",
        ///        "orderTopic": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Nội Dung Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetTopicLearning)]
        [ProducesResponseType(typeof(TopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "PARENT")]
        public async Task<IActionResult> GetTopicLearning([FromQuery] Guid classId, [FromQuery] int orderTopic)
        {
            if (orderTopic == 0 || orderTopic < 0 || orderTopic > 30)
            {
                return BadRequest("Thứ Tự Chủ Đề Không Hợp Lệ");
            }
            var response = await _classService.GetTopicLearningAsync(classId, orderTopic);
            return Ok(response);
        }


        #region document API Checking Valid Student For Class Of Current User
        /// <summary>
        ///  Kiểm Tra Các Học Sinh Của Người Dùng Hiện Tại Thỏa Mãn Điều Kiện Để Học Một Lớp Dựa Vào Id Của Lớp
        /// </summary>
        /// <param name="classId">Id Của Lớp Học</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "classId": "fded66d4-c3e7-4721-b509-e71feab6723a"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetStudentValid)]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "PARENT")]
        public async Task<IActionResult> GetValidStudent([FromQuery] Guid classId)
        {
            if (classId == default)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "Yêu Cầu Không Hợp Lệ",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            var studentIds = (await _studentService.GetStudentsOfCurrentParent()).Select(stu => stu.Id).ToList();
            if (studentIds == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "Phụ Huynh Chưa Thêm Bé Vào Hệ Thống",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            var responses = await _classService.GetValidStudentForClassAsync(classId, studentIds);
            return Ok(responses);
        }

        #region document API Get Valid Class
        /// <summary>
        ///  Truy Suất Các Lớp Kèm Lịch Học Của Một Khóa Học Sinh Có Thể Đăng Ký Dựa Vào Id Của Khóa Học Và Học Sinh
        /// </summary>
        /// <param name="courseId">Id Của Khóa Học</param>
        /// <param name="studentId">Id Của Học Sinh</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "courseId": "fded66d4-c3e7-4721-b509-e71feab6723a"
        ///       "studentId": "1a3abb54-d1e8-6ab2-g50a-b22vbab6799b"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetClassValid)]
        [ProducesResponseType(typeof(ClassWithSlotShorten), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetValidClass([FromQuery] Guid courseId, [FromQuery] Guid studentId)
        {
            if (courseId == default || studentId == default)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "Yêu Cầu Không Hợp Lệ",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            var classes = await _classService.GetValidClassForStudentAsync(courseId, studentId);
            return Ok(classes);
        }

        #region document API Get Class By Id
        /// <summary>
        ///  Truy Suất Thông Tin Chi Tiết Lớp Thông Qua Id Của Lớp
        /// </summary>
        /// <remarks>
        /// <param name="id">Id Của Lớp Học</param>
        /// Sample request:
        ///
        ///     {
        ///        "id": "c6d70a5f-56ae-4de0-b441-c080da024524"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.ClassById)]
        [AllowAnonymous]
        public async Task<IActionResult> GetClassById(Guid id)
        {
            var courses = await _classService.GetClassByIdAsync(id);
            if (courses == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = $"Id [{id}] Của Lớp Không Tồn Tại",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            return Ok(courses);
        }

        #region document API Checking Class For Students
        /// <summary>
        ///  Cho Phép Kiểm Tra Các Học Sinh Có Thỏa Mãn Điều Kiện Để Học Một Lớp Dựa Vào Id của Lớp Và Id Của Các Học Sinh
        /// </summary>
        /// <param name="classId">Id Của Lớp Học Cần Kiểm Tra</param>
        /// <param name="studentIdList">Id Của Các Học Sinh Cần Kiểm Tra</param>
        /// <response code="200">Các Học Sinh Đã Thõa Mãn Điều Kiện</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.CheckingClassForStudents)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequest))]
        [AllowAnonymous]
        public async Task<IActionResult> CheckingClassForStudents([FromQuery] Guid classId, [FromQuery] List<Guid> studentIdList)
        {
            if (classId == default || studentIdList == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "Yêu Cầu Không Hợp Lệ",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }

            var allStudentSchedules = new List<StudentScheduleResponse>();
            foreach (var task in studentIdList.Select(async stu => await _studentService
            .GetScheduleOfStudent(stu.ToString(), null, null)))
            {
                var schedules = await task;
                allStudentSchedules.AddRange(schedules);
            }

            await _walletTransactionService.ValidRegisterAsync(allStudentSchedules, classId, studentIdList);

            return Ok("Các Học Thỏa Mãn Điều Kiện Để Đăng Ký Vào Lớp");
        }

        #region document API Filter Class
        /// <summary>
        ///  Tìm Kiếm Hoặc Lọc Lớp Theo Tiêu Chí
        /// </summary>
        /// <param name="keyWords">Cho Các Lớp Thỏa Mãn Các Từ Khóa</param>
        /// <param name="leastNumberStudent">Cho Lớp Có Giới Hạn Tối Thiểu Nhiều Hơn Hoặc Bằng Gía Trị Này</param>
        /// <param name="limitStudent">Cho Lớp Có Giới Hạn Tối Đa Thấp Hơn Hoặc Bằng Gía Trị Này</param>
        /// <param name="time">Cho Các Lớp Có Thời Gian Bắt Đầu Trong Tuần/Tháng Tới</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "keyWords": "Basic", "online", "11/25/2023", "ho chi minh", "prn231"
        ///        "leastNumberStudent": 10
        ///        "limitStudent": 30
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Danh Sách Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.FilterClass)]
        [AllowAnonymous]
        public async Task<IActionResult> FilterClass(
            [FromQuery] List<string>? keyWords,
            [FromQuery] int? leastNumberStudent,
            [FromQuery] int? limitStudent,
            [FromQuery] PeriodTimeEnum time)
        {
            var classes = await _classService.FilterClassAsync(keyWords, leastNumberStudent, limitStudent, time);
            return Ok(classes);
        }

        #region document API Find Suitable Class
        /// <summary>
        ///  Tìm Kiếm Các Lớp Học Phù Hợp Với Id Lớp Học Hiện Tại Và Id Các Học Sinh Trong Lớp Cần Chuyển
        /// </summary>
        /// <param name="classId">Id Của Lớp Học Cần Kiểm Tra</param>
        /// <param name="studentId">Id Của Học Sinh Trong Lớp Cần Chuyển</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "classId": "c6d70a5f-56ae-7e0-b441-c080da024524"
        ///        "studentId":"1a2ff-afgf-h6ae-4890-b9441-a80sa034aa4"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Trả Về Các Lớp Học Thỏa Mãn</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetStuitableClass)]
        [ProducesResponseType(typeof(ClassResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequest))]
        //[Authorize(Roles = "STAFF")]
        public async Task<IActionResult> FindSuitableClass([FromQuery] string classId, [FromQuery] string studentId)
        {
            var classes = await _classService.GetClassWithDailyScheduleRes(classId, studentId);
            return Ok(classes);
        }

        #region document API Change Class
        /// <summary>
        ///  Cho Phép Chuyển Lớp Các Học Sinh Dựa Vào Id Lớp Và Id Của Các Học Sinh Cần Chuyển
        /// </summary>
        /// <param name="fromClassId">Id Của Lớp Cần Phải Chuyển</param>
        /// <param name="toClassId">Id Của Lớp Sẽ Chuyển Học Sinh Qua</param>
        /// <param name="studentId">Id Của Các Học Sinh Sẽ Chuyển Lớp</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "classId": "c6d70a5f-56ae-7e0-b441-c080da024524"
        ///        "studentIdList": "1a2ff-afgf-h6ae-4890-b9441-a80sa034aa4"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Chuyển Lớp Thành Công</response>
        /// <response code="400">Yêu Cầu Không Hợp Lệ</response>
        /// <response code="403">Chức Vụ Không Hợp Lệ</response>
        /// <response code="500">Lỗi Hệ Thống Phát Sinh</response>
        #endregion
        [HttpGet(ApiEndpointConstant.ClassEnpoint.ChangeClass)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequest))]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> ChangeStudentClass([FromQuery] string fromClassId, [FromQuery] string toClassId, [FromQuery] string studentId)
        {
            var classes = await _classService.ChangeStaffStudentClassAsync(fromClassId, toClassId, studentId);
            return Ok(classes);
        }

        [HttpPost(ApiEndpointConstant.ClassEnpoint.AddClass)]
        [CustomAuthorize(Enums.RoleEnum.STAFF, RoleEnum.DEVELOPER)]
        public async Task<IActionResult> AddClass([FromBody] CreateClassRequest request)
        {
            var isSuccess = await _classService.CreateNewClass(request);
            if (!isSuccess.Success)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "Add Class is wrong",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.Now,
                });
            }
            return Ok(isSuccess);
        }

        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetAllV2)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStaffClass([FromQuery] string? searchString, [FromQuery] string? status)
        {
            var courses = await _classService.GetAllClass(searchString, status);
            return Ok(courses);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.ClassByIdV2)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailByStaff(string id)
        {
            var matchclass = await _classService.GetClassDetail(id);
            return Ok(matchclass);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.StudentInClass)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentInClass(string id)
        {
            var matchClass = await _classService.GetAllStudentInClass(id);
            if (matchClass == null)
            {
                return NotFound(new ErrorResponse
                {
                    TimeStamp = DateTime.Now,
                    Error = "Class not has any student",
                    StatusCode = StatusCodes.Status404NotFound,
                });
            }
            return Ok(matchClass);
        }

        [HttpGet(ApiEndpointConstant.ClassEnpoint.AutoCreateClassEndPoint)]
        [AllowAnonymous]
        public async Task<IActionResult> AutoCreate(string courseId)
        {
            var result = await _classService.AutoCreateClassCode(courseId);
            return Ok(new { ClassCode = result });
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.UpdateClass)]
        [CustomAuthorize(Enums.RoleEnum.STAFF)]
        public async Task<IActionResult> UpdateClass([FromRoute] string id, [FromBody] UpdateClassRequest request)
        {
            var isSuccess = await _classService.UpdateClass(id, request);
            if (!isSuccess)
            {
                return BadRequest(new ErrorResponse
                {
                    TimeStamp = DateTime.Now,
                    Error = "không thể insert",
                    StatusCode = StatusCodes.Status400BadRequest,
                });
            }
            return Ok("success");
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.SessionLoad)]
        public async Task<IActionResult> LoadSession(string classId)
        {
            var result = await _classService.GetClassProgressResponsesAsync(classId);
            return Ok(result);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.LoadClassForAttandance)]
        public async Task<IActionResult> LoadClasForAttandance(string? searchString, DateTime dateTime, string? attendanceStatus)
        {
            var result = await _classService.GetAllClassForAttandance(searchString, dateTime, attendanceStatus);
            return Ok(result);
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.CancelClass)]
        public async Task<IActionResult> CancelClass([FromRoute] string classId)
        {
            var result = await _classService.CancelClass(classId);
            if (!result)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "update lỗi",
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.UtcNow,
                });
            }
            return Ok("successfully");
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.UpdateSession)]
        [CustomAuthorize(Enums.RoleEnum.STAFF)]
        public async Task<IActionResult> UpdateSession([FromRoute] string id, [FromBody] UpdateSessionRequest request)
        {
            var isSuccess = await _classService.UpdateSession(id, request);
            if (!isSuccess)
            {
                return BadRequest(new ErrorResponse
                {
                    TimeStamp = DateTime.Now,
                    Error = "không thể insert",
                    StatusCode = StatusCodes.Status400BadRequest,
                });
            }
            return Ok("success");
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.MakeUpClass)]
        [CustomAuthorize(Enums.RoleEnum.STAFF)]
        public async Task<IActionResult> MakeUpClass([FromRoute] string studentId, [FromRoute] string scheduleId, [FromBody] MakeupClassRequest request)
        {
            var isSuccess = await _classService.MakeUpClass(studentId, scheduleId, request);
            if (!isSuccess)
            {
                return BadRequest(new ErrorResponse
                {
                    TimeStamp = DateTime.Now,
                    Error = "không thể update",
                    StatusCode = StatusCodes.Status400BadRequest,
                });
            }
            return Ok("success");
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetMakeUpClass)]
        public async Task<IActionResult> GetMakeUpClass(string scheduleId, string studentId, DateTime? dateTime, string? keyword, string? slotId)
        {
            var isSuccess = await _classService.GetScheduleCanMakeUp(scheduleId, studentId, dateTime, keyword, slotId);
            return Ok(isSuccess);
        }
        [HttpPost(ApiEndpointConstant.ClassEnpoint.InsertClasses)]
        public async Task<IActionResult> InsertClass([FromBody] List<CreateClassesRequest> requests)
        {
            var isSuccess = await _classService.InsertClasses(requests);
            return Ok(isSuccess);
        }
        [HttpPost(ApiEndpointConstant.ClassEnpoint.InsertClassesV2)]
        public async Task<IActionResult> InsertClassV2([FromBody] List<CreateClassesRequest> requests)
        {
            var isSuccess = await _classService.InsertClassesV2(requests);
            return Ok(isSuccess);
        }
        [HttpPost(ApiEndpointConstant.ClassEnpoint.InsertClassesV3)]
        public async Task<IActionResult> InsertClassV3([FromBody] InsertClassesResponse request)
        {
            var isSuccess = await _classService.InsertClassesSave(request);
            return Ok(isSuccess);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.RoomForUpdateClass)]
        public async Task<IActionResult> UpdateClassRoom(string classId)
        {
            var isSuccess = await _classService.GetRoomsForUpdate(classId);
            return Ok(isSuccess);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.LecturerForUpdateClass)]
        public async Task<IActionResult> UpdateClassLecturer(string classId)
        {
            var isSuccess = await _classService.GetLecturerForUpdate(classId);
            return Ok(isSuccess);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.RoomForUpdateSession)]
        public async Task<IActionResult> UpdateSessionRoom(string classId, string slotId, DateTime date)
        {
            var isSuccess = await _classService.GetRoomForUpdateSession(classId, slotId, date);
            return Ok(isSuccess);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.LecturerForUpdateSession)]
        public async Task<IActionResult> UpdateSesionLecturer(string classId, string slotId, DateTime date)
        {
            var isSuccess = await _classService.GetLecturerResponseForUpdateSession(classId, slotId, date);
            return Ok(isSuccess);
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.SetNotCanMakeUp)]
        public async Task<IActionResult> SetNotCanMakeUp(string scheduleId, string studentId)
        {
            var isSuccess = await _classService.SetNotCanMakeUp(scheduleId, studentId);
            if (!isSuccess)
            {
                return Ok("Không thể cập nhật trạng thái");
            }
            return Ok("Cập nhật trạng thái thành công");
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetListCanNotMakeUp)]
        public async Task<IActionResult> GetListCanNotMakeRes(string? search, DateTime? dateOfBirth)
        {
            var isSuccess = await _classService.GetCanNotMakeUpResponses(search, dateOfBirth);
            return Ok(isSuccess);
        }
        [HttpPut(ApiEndpointConstant.ClassEnpoint.SaveCourse)]
        public async Task<IActionResult> SaveCourse(string classId, string studentId)
        {
            var isSuccess = await _classService.SaveCourse(classId, studentId);
            if (!isSuccess)
            {
                return Ok("Không thể bảo lưu");
            }
            return Ok("Bảo lưu thành công");
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetListSavedCourse)]
        public async Task<IActionResult> GetSavedCourse(string? search, DateTime? dateOfBirth)
        {
            var isSuccess = await _classService.GetSaveCourseResponse(search, dateOfBirth);
            return Ok(isSuccess);
        }
        [HttpGet(ApiEndpointConstant.ClassEnpoint.GetClassToRegister)]
        public async Task<IActionResult> GetCourseToRegister(string studentId, string courseId)
        {
            var isSuccess = await _classService.GetClassToRegister(studentId, courseId);
            return Ok(isSuccess);
        }
    }
}
