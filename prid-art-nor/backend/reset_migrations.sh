dotnet ef database drop --force
rm -rf Migrations
dotnet ef migrations add init
dotnet ef database update
