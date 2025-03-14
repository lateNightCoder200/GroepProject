using API.DataModel.DTO;
using API.Repository.UserRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return BadRequest("Username cannot be empty");

            bool IsUserUpdated = await _userRepository.updateUserName( data.UserName , data.NewUserName);

            if (!IsUserUpdated)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
