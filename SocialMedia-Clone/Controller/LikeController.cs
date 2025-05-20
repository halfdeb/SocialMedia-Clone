using AutoMapper;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Brainrot.Controller;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeRepository _repository;
    private readonly IMapper _mapper;

    public LikeController(ILikeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    //GET: api/likes
    [HttpGet]
    public async Task<IActionResult> GetAllLikes()
    {
        var DomainModel = await _repository.GetAllLikesAsync();
        if (DomainModel == null)
        {
            return NotFound("No Likes Found!");
        }

        var DomainModelDto = _mapper.Map<ICollection<Like>>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/like/{likeId}
    [HttpGet]
    [Route("{likeId}")]
    public async Task<IActionResult> GetLikeByLikeId(int likeId)
    {
        var DomainModel = await _repository.GetLikeByLikeIdAsync(likeId);
        if (DomainModel == null)
        {
            return NotFound("No Likes Found!");
        }

        var DomainModelDto = _mapper.Map<Like>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //POST: api/likes
    [HttpPost]
    public async Task<IActionResult> AddLike([FromForm] LikeRequestDto dto)
    {
        var domainModel = _mapper.Map<Like>(dto);
        bool isAdded = await _repository.AddLikeAsync(domainModel);

        if (!isAdded)
        {
            return Conflict("User has already liked this post");
        }

        var domainModeldto = _mapper.Map<LikeResponseDto>(domainModel);
        return Ok(domainModeldto);
    }
    
    //DELETE: api/likes
    [HttpDelete]
    [Route("{likeId}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int likeId)
    {
        var DomainModel = await _repository.GetLikeByLikeIdAsync(likeId);
        if (DomainModel == null)
        {
            return NotFound("No Likes Found!");
        }
        await _repository.DeleteLikeAsync(likeId);
        return Ok("Like Removed!");
    }
}