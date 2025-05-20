using AutoMapper;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Brainrot.Controller;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public ImageController(IImageRepository imageRepository, IMapper mapper)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
    }
    
    //POST: api/Images
    [HttpPost]
    public async Task<IActionResult> AddImage([FromForm] ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);

        if (ModelState.IsValid)
        {
            //Convert DTO to Domain Model
            var imageDomainModel = new Image
            {
                UserId = request.UserId,
                PostId = request.PostId,
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.File.FileName,
                FileDescription = request.FileDescription
            };
            
            //User Repository to Upload Image
            await _imageRepository.AddImageAsync(imageDomainModel);
            return Ok(imageDomainModel);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        var allowedExtension = new string[] { ".jpg", ".jpeg", ".png"};

        if (!allowedExtension.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("file", "Unsupported file extension");
        }

        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("file", "File Moret than 10MB, please Upload a smaller size file");
        }
    }
    
    //GET: api/images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
        var imageDomainModels = await _imageRepository.GetAllImagesAsync();
        if (imageDomainModels == null)
        {
            return NotFound("Image not Found!");
        }

        var imageDto = _mapper.Map<ICollection<ImageResponseDto>>(imageDomainModels);
        return Ok(imageDto);
    }
    
    //GET: api/image/{imageId}
    [HttpGet]
    [Route("{imageId}")]
    public async Task<IActionResult> GetImagesById([FromRoute]int imageId)
    {
        var imageDomainModel = await _imageRepository.GetImageByIdAsync(imageId);
        if (imageDomainModel == null)
        {
            return NotFound("Image not Found!");
        }

        var imageDto = _mapper.Map<ImageResponseDto>(imageDomainModel);
        return Ok(imageDto);
    }
    
    //DELETE: api/image
    [HttpDelete]
    [Route("{imageId}")]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var imageDomainModel = await _imageRepository.GetImageByIdAsync(imageId);
        if (imageDomainModel == null)
        {
            return BadRequest("Image not Found!");
        }
        
        await _imageRepository.DeleteImageAsync(imageId);
        return Ok("Image Deleted!");
    }
}