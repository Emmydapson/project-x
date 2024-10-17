using System.Linq;
using LearningStarter.Common;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using LearningStarter.Data;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/classrooms")]

public class ClassroomController : ControllerBase
{
    private readonly DataContext _dataContext;
    
    public ClassroomController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        
        var data = _dataContext
            .Set<Classroom>()
            .Select(classroom => new ClassroomGetDto
            {
                Id = classroom.Id,
                Name = classroom.Name,
                Description = classroom.Description,
                Student = classroom.Student,
                Channel = classroom.Channel
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
            .Set<Classroom>()
            .Select(classroom => new ClassroomGetDto
            {
                Id = classroom.Id,
                Name = classroom.Name,
                Description = classroom.Description,
                Student = classroom.Student,
                Channel = classroom.Channel
            })
            .FirstOrDefault(classroom => classroom.Id == id);
        
        response.Data = data;
        return Ok(response);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] ClassroomCreateDto createDto)
    {
        var response = new Response();
        
        if(string.IsNullOrEmpty(createDto.Name))
        {
             response.AddError(nameof(createDto.Name), "Class Name must not be empty.");   
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        var classroomToCreate = new Classroom
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Student = createDto.Student,
            Channel = createDto.Channel
        };
        
        _dataContext.Set<Classroom>().Add(classroomToCreate);
        _dataContext.SaveChanges();
        
        var classroomToReturn  = new ClassroomGetDto
        {
            Id = classroomToCreate.Id,
            Name = classroomToCreate.Name,
            Description = classroomToCreate.Description,
            Student = classroomToCreate.Student,
            Channel = classroomToCreate.Channel
        };
        
        response.Data = classroomToReturn;
        return Created("", response);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] ClassroomUpdateDto updateDto, int id)
    {
        var response = new Response();
        
        if(string.IsNullOrEmpty(updateDto.Name))
        {
             response.AddError(nameof(updateDto.Name), "Class Name must not be empty.");   
        }
        
        var classroomToUpdate = _dataContext.Set<Classroom>()
            .FirstOrDefault(classroom => classroom.Id == id);
        
        if(classroomToUpdate == null)
        {
            response.AddError("id", "Classroom not found.");
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        
        classroomToUpdate.Name = updateDto.Name;
        classroomToUpdate.Description = updateDto.Description;
        classroomToUpdate.Student = updateDto.Student;
        classroomToUpdate.Channel = updateDto.Channel;
        classroomToUpdate.Students = updateDto.Students;
        
        _dataContext.SaveChanges();
        
        var classroomToReturn = new ClassroomGetDto
        {
            Id = classroomToUpdate.Id,
            Name = classroomToUpdate.Name,
            Description = classroomToUpdate.Description,
            Student = classroomToUpdate.Student,
            Channel = classroomToUpdate.Channel
        };
        
        response.Data = classroomToReturn;
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();
        
        var classroomToDelete = _dataContext.Set<Classroom>()
            .FirstOrDefault(classroom => classroom.Id == id);
        
        if(classroomToDelete == null)
        {
            response.AddError("id", "Classroom not found.");
        }
        
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
            
        _dataContext.Set<Classroom>().Remove(classroomToDelete);
        _dataContext.SaveChanges();
        
        response.Data = true;
        return Ok(response);
    }
}