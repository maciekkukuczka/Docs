namespace Docs.Modules.Items.Pages;

public partial class AddEditDocPage : IDisposable
{
    [CascadingParameter] Task<AuthenticationState>? AuthStateTask { get; set; }
    [Inject] public DocsVMService DocsService { get; set; }
    [Inject] public CategoriesVMService CategoriesService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AppState AppState { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public UserManager<ApplicationUser> UserManager { get; set; }

    [Parameter] public string? Id { get; set; }
    [Parameter] public bool IsEdited { get; set; }

    // Doc? Doc { get; set; }
    DocVM? Doc { get; set; }
    string? userId;
    HashSet<CategoryVM>? categories;
    CategoryVM? selectedCategory;


    protected override async Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += OnLocationChanged;

        var user = (await AuthStateTask).User.Identity is { IsAuthenticated: true }
            ? (await AuthStateTask).User.Identity.Name
            : string.Empty;
        userId = (await UserManager.Users.FirstOrDefaultAsync(x => x.UserName == user))?.Id;

        await GetCategories();
        // Doc = AppState.DocToEdit ?? new Doc
        Doc = AppState.DocToEdit ?? new DocVM
        {
            // Subjects =new HashSet<Subject>{ AppState.RecentSubject}
            /*Path = new DocPath(),
            Categories = new List<Category>(),
            Links = new List<Link>(),
            Images = new List<Image>(),
            Notes = new List<Note>(),
            Tags = new List<Tag>()*/
        };
    }


    async Task OnValidSubmit(EditContext context)
    {
        // var submittedDoc = context.Model as Doc;
        var submittedDoc = context.Model as DocVM;
        // submittedDoc.Subjects.Add(AppState.RecentSubject??=new Subject
        submittedDoc?.Subjects.Add(AppState.SelectedSubject ??= new SubjectVM
        {
            Name = "Default",
            UserId = userId
        });
        // submittedDoc.Categories = Doc.Categories;
        Result? result = null;
        if (string.IsNullOrWhiteSpace(Id))
        {
            if (submittedDoc != null) result = await DocsService.AddDoc(submittedDoc);
        }
        else
        {
            if (submittedDoc != null) result = await DocsService.UpdateDoc(submittedDoc);
            AppState.DocToEdit = null;
        }


        if (result.Success)
        {
            Snackbar.Add(result.Message, Severity.Success);
            IsEdited = false;
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }


    async Task GetCategories()
    {
        categories = (await CategoriesService.GetCategories(userId)).Data;

    }
    async Task Edit()
    {
        IsEdited = !IsEdited;
    }

    Task OnCategoryChanged(CategoryVM category)
    {
        Doc.Categories.Clear();
        Doc.Categories.Add(category);
        return Task.CompletedTask;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        AppState.DocToEdit = null;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        AuthStateTask?.Dispose();
    }
}