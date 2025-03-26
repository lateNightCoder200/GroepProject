using API.DataModel.DTO;
using API.DataModel.Models;
using API.Repository.UserRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository ;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ;
        }

        [HttpPost]
        public async Task<IActionResult> SetUserName([FromBody] UserNameDTO data)
        {

            if (data == null)
                return BadRequest("Username cannot be empty.");

            var checkUserName = await _userRepository.getByUserName(data.NewUserName);

            if (!string.IsNullOrEmpty(checkUserName))
                return BadRequest("Username already exists!");

            bool IsUserUpdated = await _userRepository.updateUserName( data.email , data.NewUserName);

            if (!IsUserUpdated)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("/PatientInfo")]
        public async Task<IActionResult> SetPatientInfo([FromBody] PatientInfoDTO data)
        {

            if (data == null)
                return BadRequest("Inputs are empty.");

            //Get user Id
            var userId = await _userRepository.getUserId(data.email);

            if (userId == null)
                return NotFound("User does not exist!");

           // Check if patient info already exist
            var patientUserId = await _userRepository.getPatientId(userId);

            if ( patientUserId != Guid.Empty)
                return BadRequest("User already has patient information");

            // From patientInfoDTO to PatientInfor
            PatientInfo patientInfo = new PatientInfo()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                City = data.City,
                Hospital = data.Hospital,
                UserId = userId,
                BirthDate = data.BirthDate
            };

            var insertPatient = await _userRepository.setPatientInfo(patientInfo);

            // Check if is not inserted
            if (!insertPatient)
                return BadRequest();

            return Ok();
        }

        [HttpGet("/GetUserInfo")]

        public async Task<ActionResult<PatientInfoDTO>> getUserInfo(string userEmail)
        {
            if (userEmail == null)
                return BadRequest("Email can't be null!");

            //Validate user id
            var getUserId = await _userRepository.getUserId(userEmail);

            if (getUserId == null)
                return NotFound("Invalid email!");

            //Get patient info from the database
            var getPatientInfo = await _userRepository.getPatientInfoByUserId(getUserId);

            //Transfer data from PatientInfo object into PatientInfoDTO for security

            PatientInfoDTO patinetData = new PatientInfoDTO()
            {
                email = userEmail,
                FirstName = getPatientInfo.FirstName,
                LastName = getPatientInfo.LastName,
                City = getPatientInfo.City,
                BirthDate = getPatientInfo.BirthDate,
                Hospital = getPatientInfo.Hospital
            };

            return Ok(patinetData);
        }

        [HttpPut("UpdatePatientInfo")]

        public async Task<IActionResult> updatePatientData(PatientInfoDTO patientInfoDTO)
        {
            if (patientInfoDTO == null)
                return BadRequest("patient data can't be null!");

            //get userID 
            var getUserId = await _userRepository.getUserId(patientInfoDTO.email);

            if (getUserId == null)
                return BadRequest("This email is not registerd!");

            //get patient id by userId
            var getPatientId = await _userRepository.getPatientId(getUserId);

            if (getPatientId == null)
                return BadRequest("This email does not have a registerd patient!");

            PatientInfo patientInfo = new PatientInfo()
            {
                FirstName = patientInfoDTO.FirstName,
                LastName = patientInfoDTO.LastName,
                City = patientInfoDTO.City,
                Hospital = patientInfoDTO.Hospital,
                UserId = getUserId,
                Id = getPatientId,
                BirthDate = patientInfoDTO.BirthDate,
            };

            //update patientInfo
            var updatePatientInfo = await _userRepository.updatePatientInfo(patientInfo);

            if (!updatePatientInfo)
                return BadRequest("Invalid data!");

            return Ok(patientInfo);

        }

        [HttpGet("/CheckPatientInfo/{userEmail}")]

        public async Task<IActionResult> checkPatientInfo(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User name must not be null");

            //get userID 
            var getUserId = await _userRepository.getUserId(userEmail);

            if (getUserId == null)
                return BadRequest("This email is not registerd!");

            var getPatientInfo = await _userRepository.getPatientInfoByUserId(getUserId);

            if (getPatientInfo == null)
                return BadRequest();

            return Ok();
        }

    }

    
}
