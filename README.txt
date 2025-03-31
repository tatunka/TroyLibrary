environment setup

New Migration:
dotnet ef migrations add <migration name> --project TroyLibrary.Data --startup-project TroyLibrary.API --context TroyLibraryContext

Apply Migration:
dotnet ef database update <migration name> --project TroyLibrary.Data --startup-project TroyLibrary.API --context TroyLibraryContext