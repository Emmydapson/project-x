using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Mvc;
namespace LearningStarter.Controllers;
[Route("api/posts")]
public class PostController: ControllerBase
{   
    private readonly DataContext _dataContext;
    private readonly IAuthenticationService _authenticationService;
    public PostController(DataContext dataContext, IAuthenticationService authenticationService)
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
            .Set<Post>()
            .Select(post=> new PostGetDto
            {
                Id = post.Id,
                UserName = userName,
                Text = post.Text,
                Time = post.Time
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
              .Set<Post>()
              .Select(post=> new PostGetDto
              {
                   Id = post.Id,
                   UserName = userName,
                   Text = post.Text,
                   Time = post.Time
              })
              .FirstOrDefault(post => post.Id == id );
        response.Data = data;
        return Ok(response);
    }
    [HttpPost]
    public IActionResult Create([FromBody] PostCreateDto createDto)
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();
        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }
        var userName = user.UserName;
        if(string.IsNullOrEmpty(createDto.Text))
        {
            response.AddError("Text", "Text must not be empty.");
        }
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        var postToCreate = new Post 
        {
            UserName = userName,
            Text = createDto.Text,
            Time = createDto.Time
        };
        _dataContext.Set<Post>().Add(postToCreate);
        _dataContext.SaveChanges();
        var postToReturn = new PostGetDto
        {
            Id = postToCreate.Id,
            UserName = userName,
            Text = postToCreate.Text,
            Time = postToCreate.Time
        };
        response.Data = postToReturn;
        return Created("", response);
    }
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] PostUpdateDto updateDto, int id)
    {
        var response = new Response();
        var user = _authenticationService.GetLoggedInUser();
        if (user == null)
        {
            response.AddError("", "No logged in user found");
            return BadRequest(response);
        }
        var userName = user.UserName;
        var postToUpdate = _dataContext
            .Set<Post>()
            .FirstOrDefault(post => post.Id == id);
        if(postToUpdate == null)
        {
             response.AddError("id", "Post not found." );
        }
        if(string.IsNullOrEmpty(updateDto.Text))
        {
             response.AddError("Text", "Text must not be empty.");
        }
        if(response.HasErrors)
        {
             return BadRequest(response);
        }
        postToUpdate.UserName = userName;
        postToUpdate.Text = updateDto.Text;
        postToUpdate.Time = updateDto.Time;
        _dataContext.SaveChanges();
        var postToReturn = new PostGetDto
        {
            Id = postToUpdate.Id,
            UserName = userName,
            Text = postToUpdate.Text,
            Time = postToUpdate.Time
        };
        response.Data = postToReturn;
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();
        var postToDelete = _dataContext
            .Set<Post>()
            .FirstOrDefault(post => post.Id == id);
        if(postToDelete == null)
        {
            response.AddError("id", "Post not found." );
        }
        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<Post>().Remove(postToDelete);
        _dataContext.SaveChanges();
        response.Data = true;
        return Ok(response);
    }
}