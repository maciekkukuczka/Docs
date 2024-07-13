namespace Docs.Modules.Common.Models;

public class BaseModel
{
    public string Id { get; set; }=  Guid.NewGuid().ToString();
    // TODO Check if work
    // public string Id { get; set; }=  Guid.CreateVersion7().ToString();
    public bool IsActive { get; set; } = true;

    public DateTime CreateDate { get; set; }=DateTime.Now;
    public DateTime? ModifyDate { get; set; }
}