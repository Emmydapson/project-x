using System.Linq;
using LearningStarter.Common;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using LearningStarter.Data;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/server")]

public class ServerController : ControllerBase
{
    private readonly DataContext _dataContext;
    
    public ServerController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        
        var data = _dataContext
            .Set<Server>()
            .Select(server => new ServerGetDto
            {
                Id = server.Id,
                Name = server.Name,
                Description = server.Description,
                Classroom = server.Classroom
            })
            .ToList();
        
        response.Data = data;
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var response = new Response();
                
        var data = _dataContext
            .Set<Server>()
            .Select(server => new ServerGetDto
            {
                Id = server.Id,
                Name = server.Name,
                Description = server.Description,
                Classroom = server.Classroom
            })
            .FirstOrDefault(server => server.Id == id);
        
        response.Data = data;
        return Ok(response);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] ServerCreateDto createDto)
    {
        var response = new Response();
        
        if(string.IsNullOrEmpty(createDto.Name))
        {
             response.AddError(nameof(createDto.Name), "Server Name must not be empty.");   
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        var serverToCreate = new Server
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Classroom = createDto.Classroom
        };
        
        _dataContext.Set<Server>().Add(serverToCreate);
        _dataContext.SaveChanges();
        
        var serverToReturn  = new ServerGetDto
        {
            Id = serverToCreate.Id,
            Name = serverToCreate.Name,
            Description = serverToCreate.Description,
            Classroom = serverToCreate.Classroom
        };
        
        response.Data = serverToReturn;
        return Created("", response);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] ServerUpdateDto updateDto, int id)
    {
        var response = new Response();
        
        if(string.IsNullOrEmpty(updateDto.Name))
        {
             response.AddError(nameof(updateDto.Name), "Server Name must not be empty.");   
        }
        
        var serverToUpdate = _dataContext.Set<Server>()
            .FirstOrDefault(server => server.Id == id);
        
        if(serverToUpdate == null)
        {
            response.AddError("id", "Server not found.");
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        
        serverToUpdate.Name = updateDto.Name;
        serverToUpdate.Description = updateDto.Description;
        serverToUpdate.Classroom = updateDto.Classroom;
        serverToUpdate.Classes = updateDto.Classes;
        
        _dataContext.SaveChanges();
        
        var serverToReturn = new ServerGetDto
        {
            Id = serverToUpdate.Id,
            Name = serverToUpdate.Name,
            Description = serverToUpdate.Description,
            Classroom = serverToUpdate.Classroom
        };
        
        response.Data = serverToReturn;
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();
        
        var serverToDelete = _dataContext.Set<Server>()
            .FirstOrDefault(server => server.Id == id);
        
        if(serverToDelete == null)
        {
            response.AddError("id", "Server not found.");
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
            
        _dataContext.Set<Server>().Remove(serverToDelete);
        _dataContext.SaveChanges();
        
        response.Data = true;
        return Ok(response);
    }
}