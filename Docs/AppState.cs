namespace Docs;

public class AppState
{
    
    public event Action? OnChange;
    public Doc? DocToEdit { get; set; }
    public Subject? RecentSubject { get; set; }

    public void  SetRecentSubjectId(Subject recentSubjectId)
    {
        this.RecentSubject= recentSubjectId;
            OnChange?.Invoke();
    }
  
}