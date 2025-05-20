using AutoMapper;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Brainrot.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    public UserController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    //GET: api/user
    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        var DomainModel = await _repository.GetAllUserAsync();

        if (DomainModel == null)
        {
            return NotFound("No User Found!");
        }
        
        var DomainModelDto = _mapper.Map<ICollection<UserResponseDto>>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/user/{userId}
    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var DomainModel = await _repository.GetUserByIdAsync(userId);
        if (DomainModel == null)
        {
            return NotFound("User Not Found!");
        }

        var DomainModelDto = _mapper.Map<UserResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //POST: api/user
    [HttpPost]
    public async Task<IActionResult> AddUser([FromForm] UserRequestDto userRequestDto)
    {
        var DomainModel = _mapper.Map<User>(userRequestDto);
        var success = await _repository.AddUserAsync(DomainModel);
        if (success == false)
        {
            return BadRequest("Can't add Users!");
        }
        var DomainModelDto = _mapper.Map<UserResponseDto>(DomainModel);
        return CreatedAtAction(nameof(GetUserById), new{userId = DomainModelDto.UserId}, DomainModelDto);
    }
    
    //PUT: api/user/{userId}
    [HttpPut]
    [Route("{userId}")]
    public async Task<IActionResult> UpdateUser([FromRoute]int userId, [FromForm] UserRequestDto userRequestDto)
    {
        var DomainModel = await _repository.GetUserByIdAsync(userId);
        if (DomainModel == null)
        {
            return NotFound("User Not Found!");
        }
        
        var user = _mapper.Map<User>(userRequestDto);
        var success = await _repository.UpdateUserAsync(userId, user);

        if (success == false)
        {
            return BadRequest("Can't update Users!");
        }
        var DomainModelDto = _mapper.Map<UserResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //DELETE: api/user
    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var success = await _repository.DeleteUserAsync(userId);
        if (success == false)
        {
            return BadRequest("Can't delete User!");
        }
        
        return Ok("User Deleted");
    }
}