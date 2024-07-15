namespace Docs.Config;

/*public enum ErrorEnums
{
    ObjectNotExist,
    ObjectNotFound,
    ObjectCannotBeSave,
    ObjectSaved,
}

public static class ErrorDescripions
{
    public static string GetErrorDescription<T>(this ErrorEnums errorEnum) =>
        errorEnum switch
        {
            ErrorEnums.ObjectNotExist => $"{typeof(T).Name.ToUpper()} nie istnieje",
            ErrorEnums.ObjectNotFound => $"{typeof(T).Name.ToUpper()} nie znaleziony",
            ErrorEnums.ObjectCannotBeSave => "",
            ErrorEnums.ObjectSaved => "",
            
            _ => "Nieznany błąd"
        };
}*/

public static class Errors
{
    public static string ObjectNotExist<T> () => $"{typeof(T).Name.ToUpper()} - nie istnieje";
    public static string ObjectNotFound<T> () => $"{typeof(T).Name.ToUpper()} - nie znaleziono";
    public static string ObjectCannotBeUpdate<T> () => $"{typeof(T).Name.ToUpper()} - nie mógł zostać zaktualizowany";
    public static string ObjectCannotBeSaved<T> () => $"{typeof(T).Name.ToUpper()} - nie można zapisać";
    public static string ObjectCannotBeDeleted<T> () => $"{typeof(T).Name.ToUpper()} - nie można usunąć";
    public static string ObjectCannotBeDeletedHasRelatedEntities<T>() =>
        $"{typeof(T).Name.ToUpper()} - obiekt nie może być usunięty, ponieważ posiada powiazane encje. ";
    public static string ObjectCreated<T> () => $"{typeof(T).Name.ToUpper()} - utworzono";
    public static string ObjectSaved<T> () => $"{typeof(T).Name.ToUpper()} - zapisano";
    public static string ObjectDeleted <T> () => $"{typeof(T).Name.ToUpper()} - usunieto";

}