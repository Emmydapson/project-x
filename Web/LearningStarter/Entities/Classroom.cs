using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningStarter.Entities;

public class Classroom
{
    public int Id { get; set;  }
    public string Name { get; set;  }
    public string Description { get; set;  }
    public string Student { get; set; }
    public string Channel { get; set; }
    
    public List<ClassroomStudents> Students { get; set; }
}

public class ClassroomGetDto
{
    public int Id { get; set;  }
    public string Name { get; set;  }
    public string Description { get; set;  }
    public string Student { get; set; }
    public string Channel { get; set; }
}

public class ClassroomCreateDto
{
    public string Name { get; set;  }
    public string Description { get; set;  }
    public string Student { get; set; }
    public string Channel { get; set; }
}

public class ClassroomUpdateDto
{
    public string Name { get; set;  }
    public string Description { get; set;  }
    public string Student { get; set; }
    public string Channel { get; set; }
    public List<ClassroomStudents> Students { get; set; }
}

public class ClassroomEntityTypeConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.ToTable("Classroom");
    }
}