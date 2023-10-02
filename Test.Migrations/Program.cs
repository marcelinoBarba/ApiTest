// See https://aka.ms/new-console-template for more information

using Test.Migrations;


string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Tenant2DB;Trusted_Connection=True;MultipleActiveResultSets=true;";
ExecuteMigration.RunMigrations(connectionString);
