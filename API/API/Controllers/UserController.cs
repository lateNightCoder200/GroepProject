using API.DataModel.DTO;
using API.DataModel.Models;
using API.Repository.UserRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        //Repository
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SetUserName([FromBody] UserNameDTO data)
        {

            if (data == null)
                return BadRequest("Username cannot be empty.");

            var checkUserName = await _userRepository.getUserNameByUserName(data.NewUserName);

            if (!string.IsNullOrEmpty(checkUserName))
                return BadRequest("Username already exists!");

            bool IsUserUpdated = await _userRepository.updateUserName(data.email, data.NewUserName);

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

            if (patientUserId != Guid.Empty)
                return BadRequest("User already has patient information");



            // From patientInfoDTO to PatientInfor
            PatientInfo patientInfo = new PatientInfo()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                City = data.City,
                Hospital = data.Hospital,
                UserId = userId,
                BirthDate = data.BirthDate,
                treatmentDate = data.treatmentDate,
                treatmentPlan = data.treatmentPlan,
                DoctorName = data.DoctorName


            };

            var insertPatient = await _userRepository.setPatientInfo(patientInfo);

            // Check if is not inserted
            if (!insertPatient)
                return BadRequest();

            return Ok();
        }

        [HttpGet("/GetUserInfo")]

        public async Task<ActionResult<GetPatientInfoDTO>> getUserInfo(string userEmail)
        {
            if (userEmail == null)
                return BadRequest("Email can't be null!");

            //Validate user id
            var getUserId = await _userRepository.getUserId(userEmail);

            if (getUserId == null)
                return NotFound("Invalid email!");

            //Get patient info from the database
            var getPatientInfo = await _userRepository.getPatientInfoByUserId(getUserId);

            //Get user name
            var getUserName = await _userRepository.getUserNameByEmail(userEmail);
                
            if(getPatientInfo == null)
                return NotFound("No user data!");

            //Transfer data from PatientInfo object into PatientInfoDTO for security

            GetPatientInfoDTO patinetData = new GetPatientInfoDTO()
            {
                userName = getUserName,
                FirstName = getPatientInfo.FirstName,
                LastName = getPatientInfo.LastName,
                City = getPatientInfo.City,
                BirthDate = getPatientInfo.BirthDate,
                Hospital = getPatientInfo.Hospital,
                treatmentDate = getPatientInfo.treatmentDate,
                treatmentPlan = getPatientInfo.treatmentPlan,
                DoctorName = getPatientInfo.DoctorName
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
                treatmentDate = patientInfoDTO.treatmentDate,
                treatmentPlan = patientInfoDTO.treatmentPlan,
                DoctorName = patientInfoDTO.DoctorName
            };

            //update patientInfo
            var updatePatientInfo = await _userRepository.updatePatientInfo(patientInfo);

            if (!updatePatientInfo)
                return BadRequest("Invalid data!");

            return Ok(patientInfo);

        }

        [HttpGet("/CheckPatientInfo/{userEmail}")]

        public async Task<ActionResult<TrajectDataDTO>> checkPatientInfo(string userEmail)
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

            TrajectDataDTO trajectData = new TrajectDataDTO()
            {

                name = getPatientInfo.FirstName,
                age = getPatientInfo.BirthDate,
                plan = getPatientInfo.treatmentPlan
            };

            return Ok(trajectData);
        }

        [HttpPost("/AddNote")]

        public async Task<IActionResult> AddNote(NoteDTO note)
        {
            if (note == null)
                return BadRequest("Note is null");

            var getUserId = await _userRepository.getUserId(note.userEmail);

            if (getUserId == null)
                return NotFound("User does not exist!");

            var getNote = await _userRepository.getNoteByName(note.Name , getUserId);

            if ( getNote != null)
                return BadRequest("Note Name is already used");


            Notes newNote = new Notes()
            {
                Id = Guid.NewGuid(),
                UserId = getUserId,
                Name = note.Name,
                Note  = note.Note
            };

            var addNote = await _userRepository.addNote(newNote);

            if (!addNote)
                return BadRequest("Notes cant added");

            return Ok();
        }

        [HttpGet("/GetNotes/{email}")]

        public async Task<ActionResult<List<Notes>>> getNotes(string email)
        {
            if(string.IsNullOrEmpty(email))
                return BadRequest("Email is null!");

            var getUserId = await _userRepository.getUserId(email);

            if (getUserId == null)
                return NotFound("User does not exist!");



            var getNotes = await _userRepository.getNotesByEmail(getUserId);

            if (getNotes == null)
                return BadRequest();
            return Ok(getNotes);
        }


        [HttpPost("/GetNote")]

        public async Task<ActionResult<Notes>> getANote([FromBody] GetNoteDTO note)
        {
            if (note == null)
                return BadRequest("Note is null!");

            var getUserId = await _userRepository.getUserId(note.email);

            if (getUserId == null)
                return NotFound("User does not exist!");



            var getNotes = await _userRepository.getNoteByName(note.name , getUserId);

            if (getNotes == null)
                return BadRequest();
            return Ok(getNotes);
        }
    }

   

    
}
