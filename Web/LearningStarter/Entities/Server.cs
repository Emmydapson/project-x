using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningStarter.Entities;

public class Server
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Classroom { get; set; }
    public List<ServerClassroom> Classes { get; set; }
}

public class ServerGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Classroom { get; set; }
}

public class ServerCreateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Classroom { get; set; }
}

public class ServerUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Classroom { get; set; }
}

public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.ToTable("Server");
    }
}