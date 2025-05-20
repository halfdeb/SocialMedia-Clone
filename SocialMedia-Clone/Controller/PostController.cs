using AutoMapper;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Brainrot.Controller;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _repository;
    private readonly IMapper _mapper;

    public PostController(IPostRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    //GET: api/posts
    [HttpGet]
    public async Task<IActionResult> GetAllPost()
    {
        var DomainModel = await _repository.GetAllPostAsync();
        if (DomainModel == null)
        {
            return NotFound("Post Not Found");
        }

        var DomainModelDto = _mapper.Map<ICollection<PostResponseDto>>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/post/{postId}
    [HttpGet]
    [Route("{postId}")]
    public async Task<IActionResult> GetPostById(int postId)
    {
        var DomainModel = await _repository.GetPostByIdAsync(postId);
        if (DomainModel == null)
        {
            return NotFound("Post Not Found");
        }

        var DomainModelDto = _mapper.Map<PostResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/post/userId/{userId}
    [HttpGet]
    [Route("userid/{userId}")]
    public async Task<IActionResult> GetPostByUserId(int userId)
    {
        var DomainModel = await _repository.GetPostByUserIdAsync(userId);
        if (DomainModel == null)
        {
            return NotFound("Post Not Found");
        }

        var DomainModelDto = _mapper.Map<PostResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    
    //POST: api/post
    [HttpPost]
    public async Task<IActionResult> AddPost([FromForm] PostRequestDto postRequestDto)
    {
        var DomainModel = _mapper.Map<Post>(postRequestDto);
        await _repository.AddPostAsync(DomainModel);

        var DomainModelDto = _mapper.Map<PostResponseDto>(DomainModel);
        return CreatedAtAction(nameof(GetPostById), new { postId = DomainModelDto.PostId }, DomainModelDto);
    }
    
    //PUT: api/post/{postId}
    [HttpPut]
    [Route("{postId}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int postId, [FromBody] PostUpdateDto postUpdateDto)
    {
        var DomainModel = await _repository.GetPostByIdAsync(postId);
        if (DomainModel == null)
        {
            return NotFound("Post Not Found");
        }

        var post = _mapper.Map<Post>(postUpdateDto);
        await _repository.UpdatePostAsync(postId, post);

        var DomainModelDto = _mapper.Map<PostResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //DELETE: api/post/{postId}
    [HttpDelete]
    [Route("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var DomainModel = await _repository.GetPostByIdAsync(postId);
        if (DomainModel == null)
        {   
            return NotFound("Post Not Found!");
        }

        await _repository.DeletePostAsync(postId);
        return Ok("Post Deleted!");
    }
}