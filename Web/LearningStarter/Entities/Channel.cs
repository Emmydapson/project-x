using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LearningStarter.Entities;
public class Channel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Description { get; set; }
    
    public List<ChannelPosts> Posts { get; set; }
}
public class ChannelGetDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Description { get; set; }
}
public class ChannelCreateDto
{
    public string UserName { get; set; }
    public string Description { get; set; }
}
public class ChannelUpdateDto
{
    public string UserName { get; set; }
    public string Description { get; set; }
    public List<ChannelPosts> Posts { get; set; }
}
public class ChannelEntityTypeConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("Channel");
    }
}