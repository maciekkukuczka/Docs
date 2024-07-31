namespace Docs.Modules.Items.Pages;

// public partial class DocsPageBase : ComponentBase, IDisposable
public partial class DocsPage : IDisposable
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public DocsService DocsService { get; set; }
    [Inject] public CategoriesVMService CategoriesService { get; set; }
    [Inject] public PersistentComponentState ApplicationState { get; set; }
    [Inject] public AppState AppState { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public IDialogService DialogService { get; set; }


    PersistingComponentStateSubscription stateSubscription;
    MudDataGrid<DocVM>? dataGrid;
    HashSet<DocVM>? docs;
    HashSet<CategoryVM>? selectedCategories;
    SubjectVM? selectedSubject;
    CategoryVM? selectedCategory;
    string? searchString;


    protected override async Task OnInitializedAsync()
    {
        // throw new ArgumentException();
        // AppState.OnChange += StateHasChanged;
        stateSubscription = ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson("Docs", docs);
            return Task.CompletedTask;
        });
        await GetDocs();
    }


    async Task GetDocs(string? subjectId = "")
    {
        docs = !ApplicationState.TryTakeFromJson<HashSet<DocVM>?>("Docs", out var restored)
            ? (await DocsService.GetDocBySubject(subjectId)).Data
            : restored!;

        //By AppState
        /*selectedCategory = AppState.SelectedCategory;
        selectedSubject = AppState.SelectedSubject;*/

        HashSet<Expression<Func<Doc, bool>>> filters = [];
        if (selectedSubject is not null) filters.Add(x => x.Subjects.Any(x => x.Id == selectedSubject.Id));
        if (selectedCategory is not null) filters.Add(x => x.Categories.Any(y => y.Id == selectedCategory.Id));

        // _=   selectedSubject is not null? filters.Add(x=>x.Subjects.Any(x=>x.Id==selectedSubject.Id)): filters.Add(x => x.Subjects.Any());


        docs = (await DocsService.GetDocsByFilter(filters,
            (HashSet<Expression<Func<Doc, object>>>)
            [
                x => x.Categories, x => x.Subjects,x=>x.Links
            ]
        )).Data;

        // subjects = docs.SelectMany(x => x.Subjects).ToHashSet();
        // selectedCategories = docs.SelectMany(x => x.Categories).ToHashSet();
        /*        try
        {
        var sw = Stopwatch.StartNew();
            if (ApplicationState.TryTakeFromJson<HashSet<DocVM>>("Docs", out var restored))
            {
                docs = restored;
                Log.Error("REESTORED");
            }
            else
            {
                docs = (await DocsService.GetDocBySubject(subjectId)).data;
                Log.Error("DB");
            }
        }
        finally
        {
            sw.Stop();
            Log.Warning($" POMIAR DOCSÓW: {sw.ElapsedMilliseconds} ms");
        }*/
    }


    async Task OnSubjectSelected(SubjectVM subject)
    {
        // selectedSubject = AppState.RecentSubject; 
        selectedSubject = subject;
        await GetDocs();
    }


    async Task OnCategorySelected(CategoryVM category)
    {
        selectedCategory = category;
        await GetDocs();
    }


    void AddDoc() => NavigationManager.NavigateTo("/add-doc");


    void Edit(DocVM contextItem)
    {
        AppState.DocToEdit = contextItem;
        NavigationManager.NavigateTo($"/edit-doc/{contextItem.Id}");
    }


    async Task Remove(string contextItemId)
    {
        bool? dialogResult = await DialogService.ShowMessageBox(
            "uwaga",
            "Czy na pewno skasować dokument?",
            yesText: "Skasuj!", cancelText: "Anuluj");

        var state = dialogResult == null ? "Canceled" : "Deleted!";
        if (state.Equals("Canceled")) return;
        var result = await DocsService.DeleteDoc(contextItemId);
        var snackbar = result.Success
            ? Snackbar.Add(result.Message, Severity.Warning)
            : Snackbar.Add(result.Message, Severity.Error);
        await GetDocs();
    }


    Func<DocVM, bool> SearchFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(searchString)) return true;
        if (x.Title is not null && x.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)) return true;
        if (x.ShortDescription is not null &&
            x.ShortDescription.Contains(searchString, StringComparison.OrdinalIgnoreCase)) return true;
        if (x.Description is not null &&
            x.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase)) return true;
        if (x.Categories is not null &&
            x.Categories.Any(x=>x.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))) return true;
        return false;
    };


    public void Dispose()
    {
        // AppState.OnChange -= StateHasChanged;
        stateSubscription.Dispose();
    }
}