using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LearningStarter.Entities;
public class Post
{
    public int Id { get; set; }
    public string UserName  { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Time { get; set; }
   
}
public class PostGetDto
{
    public int Id { get; set; }
    public string UserName  { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Time { get; set; }
}
public class PostCreateDto
{
    public string UserName  { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Time { get; set; }
}
public class PostUpdateDto
{
    public string UserName  { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Time { get; set; }
}
public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
    }
}