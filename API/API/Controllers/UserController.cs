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

            var checkUserName = _userRepository.getByUserName(data.NewUserName);

            if (checkUserName != null)
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

            //Check if patient info already exist
            var patientUserId = await _userRepository.getPatientUserId(userId);

            if (patientUserId != null)
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
    }
}
