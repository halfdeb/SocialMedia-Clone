using AutoMapper;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Brainrot.Controller;

[Route("api/[controller]")]
[ApiController]
public class CommentController :ControllerBase
{
    private readonly ICommentRepository _repository;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //GET: api/comments
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var DomainModel = await _repository.GetAllCommentsAsync();
        if (DomainModel == null)
        {
            return NotFound("Comments not Found!");
        }

        var DomainModelDto = _mapper.Map<ICollection<CommentResponseDto>>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/comments/{commentId}
    [HttpGet]
    [Route("{commentId}")]
    public async Task<IActionResult> GetCommentsByCommentId([FromRoute]int commentId)
    {
        var DomainModel = await _repository.GetCommentByCommentIdAsync(commentId);
        if (DomainModel == null)
        {
            return NotFound("Comment not found!");
        }

        var DomainModelDto = _mapper.Map<CommentResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //GET: api/comments/postId/{postId}
    [HttpGet]
    [Route("postId/{postId}")]
    public async Task<IActionResult> GetCommentsByPostId([FromRoute]int postId)
    {
        var DomainModel = await _repository.GetCommentByPostIdAsync(postId);
        if (DomainModel == null)
        {
            return NotFound("Comment not found!");
        }

        var DomainModelDto = _mapper.Map<ICollection<CommentResponseDto>>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //POST: api/comments
    [HttpPost]
    public async Task<IActionResult> AddComment([FromForm]CommentRequestDto dto)
    {
        var DomainModel = _mapper.Map<Comment>(dto);
        await _repository.AddCommentAsync(DomainModel);

        var DomainModelDto = _mapper.Map<CommentResponseDto>(DomainModel);
        return CreatedAtAction(nameof(GetCommentsByCommentId), new{commentId = DomainModelDto.CommentId}, DomainModelDto);
    }
    
    //PUT: api/comments/{commentId}
    [HttpPut]
    [Route("{commentId}")]
    public async Task<IActionResult> UpdateComment([FromForm]CommentUpdateDto dto, [FromRoute]int commentId)
    {
        var DomainModel = await _repository.GetCommentByCommentIdAsync(commentId);
        if (DomainModel == null)
        {
            return NotFound("Comment not Found!");
        }
        var comment = _mapper.Map<Comment>(dto);
        await _repository.UpdateCommentAsync(comment, commentId);
        
        var DomainModelDto = _mapper.Map<CommentResponseDto>(DomainModel);
        return Ok(DomainModelDto);
    }
    
    //DELETE: api/comments/{commentId}
    [HttpDelete]
    [Route("{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute]int commentId)
    {
        var DomainModel = await _repository.GetCommentByCommentIdAsync(commentId);
        if (DomainModel == null)
        {
            return NotFound("Comment not found!");
        }
        await _repository.DeleteCommentAsync(commentId);
        return Ok("Comment deleted!");
    }
}