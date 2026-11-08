using DbUp;
using System.Reflection;

namespace Infrastructure.Postgres
{
    public static class DatabaseMigrator
    {
        public static void Execute(string connectrionStrign)
        {
            // Garante que o banco de dados existe no PostgreSQL
            EnsureDatabase.For.PostgresqlDatabase(connectrionStrign);

            // Procura e executa todos os scripts .sql embutidos no assembly
            var upgrader = DeployChanges.To
                                .PostgresqlDatabase(connectrionStrign)
                                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                .LogToConsole()
                                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                throw new Exception("Falha na migração do banco de dados.", result.Error);
            }
        }
    }
}
