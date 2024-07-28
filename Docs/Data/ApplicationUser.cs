using Docs.Modules.Subjects.Models;

namespace Docs.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // NAV
    // public ICollection<Doc> Docs { get; set; } = (HashSet<Doc>) [];
    public ICollection<Subject>Subjects { get; set; }= (HashSet<Subject>) [];

}

