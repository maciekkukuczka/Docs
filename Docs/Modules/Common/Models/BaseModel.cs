namespace Docs.Modules.Common.Models;

public class BaseModel
{
    public string Id { get; set; }=Guid.NewGuid().ToString();
    public bool IsActive { get; set; } = true;

    public DateTime CreateDate { get; set; }=DateTime.UtcNow;
    public DateTime ModifyDate { get; set; }
}