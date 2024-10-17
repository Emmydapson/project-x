using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Mvc;
namespace LearningStarter.Controllers;
[Route("api/channels")]
public class ChannelController: ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IAuthenticationService _authenticationService;
    public ChannelController(DataContext dataContext, IAuthenticationService authenticationService)
    {
        _dataContext = dataContext;
        _authenticationService = authenticationService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();

        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }

        var userName = user.UserName;
        var data = _dataContext
            .Set<Channel>()
            .Select(channel=> new ChannelGetDto
            {
                Id = channel.Id,
                UserName = userName,
                Description = channel.Description,
            })
            .ToList();
        response.Data = data;
        return Ok(response);
    }
    [HttpGet("{id}")]
    public IActionResult GetbyId(int id)
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();

        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }
        var userName = user.UserName;
        var data = _dataContext
            .Set<Channel>()
            .Select(channel=> new ChannelGetDto
            {
                Id = channel.Id,
                UserName = userName,
                Description = channel.Description,
            })
            .FirstOrDefault(channel => channel.Id == id );
        response.Data = data;
        return Ok(response);
    }
    [HttpPost]
    public IActionResult Create([FromBody] ChannelCreateDto createDto)
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();
        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }
        var userName = user.UserName;
        if(string.IsNullOrEmpty(createDto.Description))
        {
            response.AddError("Description", "Description must not be empty.");
        }
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        var channelToCreate = new Channel 
        {
            UserName = userName,
            Description = createDto.Description
        };
        _dataContext.Set<Channel>().Add(channelToCreate);
        _dataContext.SaveChanges();
        var channelToReturn = new ChannelGetDto
        {
            Id = channelToCreate.Id,
            UserName = userName,
            Description = channelToCreate.Description
        };
        response.Data = channelToReturn;
        return Created("", response);
    }
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] ChannelUpdateDto updateDto, int id)
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();
        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }
        var userName = user.UserName;
        var channelToUpdate = _dataContext
            .Set<Channel>()
            .FirstOrDefault(channel => channel.Id == id);
        if(channelToUpdate == null)
        {
            response.AddError("id", "Post not found." );
        }
        if(string.IsNullOrEmpty(updateDto.Description))
        {
            response.AddError("Description", "Description must not be empty.");
        }
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        channelToUpdate.UserName = userName;
        channelToUpdate.Description = updateDto.Description;
        _dataContext.SaveChanges();
        var channelToReturn = new ChannelGetDto
        {
            Id = channelToUpdate.Id,
            UserName = userName,
            Description = channelToUpdate.Description,
        };
        response.Data = channelToReturn;
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();
        var channelToDelete = _dataContext
            .Set<Channel>()
            .FirstOrDefault(channel => channel.Id == id);
        if(channelToDelete == null)
        {
            response.AddError("id", "Channel not found." );
        }
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<Channel>().Remove(channelToDelete);
        _dataContext.SaveChanges();
        response.Data = true;
        return Ok(response);
    }
}