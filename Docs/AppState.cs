namespace Docs;

public class AppState
{
    public event Action? OnChange;

    // public Doc? DocToEdit { get; set; }
    public DocVM? DocToEdit { get; set; }

    // public Subject? RecentSubject { get; set; }
    public SubjectVM? SelectedSubject { get; set; }
    public CategoryVM? SelectedCategory { get; set; }

    // public void  SetRecentSubjectId(Subject recentSubjectId)
    public void SelectSubject(SubjectVM selectedSubject)
    {
        SelectedSubject = selectedSubject;
        OnChange?.Invoke();
    }

    public void SelectCategory(CategoryVM selectedCategory)
    {
        SelectedCategory = selectedCategory;
        OnChange?.Invoke();
    }
}