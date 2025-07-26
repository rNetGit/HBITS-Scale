using Microsoft.Data.Sqlite;
using HBITSExplorer.Models;

namespace HBITSExplorer.Services;

public class RateService
{
    private readonly string _connectionString;

    public RateService(IConfiguration configuration)
    {
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "HBITS.db");
        _connectionString = $"Data Source={dbPath}";
        Console.WriteLine($"RateService initialized with database path: {dbPath}");
        Console.WriteLine($"Database file exists: {File.Exists(dbPath)}");
    }

    private decimal ParseCurrencyValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
            
        // Remove dollar signs, commas, and any other currency formatting
        var cleanValue = value.Replace("$", "").Replace(",", "").Trim();
        
        if (decimal.TryParse(cleanValue, out var result))
            return result;
            
        return 0;
    }

    public async Task<List<string>> GetJobTitlesAsync()
    {
        var jobTitles = new List<string>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT DISTINCT Job_Title FROM Rates ORDER BY Job_Title";
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            jobTitles.Add(reader.GetString(0));
        }
        
        return jobTitles;
    }

    public async Task<List<string>> GetSkillLevelsAsync()
    {
        var skillLevels = new List<string>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT DISTINCT Skill_Level FROM Rates ORDER BY Skill_Level";
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            skillLevels.Add(reader.GetString(0));
        }
        
        return skillLevels;
    }

    public async Task<List<Rate>> SearchRatesAsync(string jobTitle, string skillLevel)
    {
        var rates = new List<Rate>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        var query = @"
            SELECT Contractor_Name, Job_Title, Skill_Level, HourlyWageRate, HourlyBillRate, EffictiveDt 
            FROM Rates 
            WHERE Job_Title = @JobTitle AND Skill_Level = @SkillLevel
            ORDER BY HourlyBillRate";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@JobTitle", jobTitle),
            new SqliteParameter("@SkillLevel", skillLevel)
        };
        
        // Debug: Log the query and parameters
        Console.WriteLine($"Search Query: {query}");
        Console.WriteLine($"Job Title Parameter: '{jobTitle}'");
        Console.WriteLine($"Skill Level Parameter: '{skillLevel}'");
        
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rates.Add(new Rate
            {
                Contractor_Name = reader.GetString(0),
                Job_Title = reader.GetString(1),
                Skill_Level = reader.GetString(2),
                HourlyWageRate = ParseCurrencyValue(reader.GetString(3)),
                HourlyBillRate = ParseCurrencyValue(reader.GetString(4)),
                EffictiveDt = reader.GetString(5)
            });
        }
        
        Console.WriteLine($"Found {rates.Count} results");
        return rates;
    }

    public async Task<List<Rate>> GetAllRatesAsync()
    {
        var rates = new List<Rate>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Contractor_Name, Job_Title, Skill_Level, HourlyWageRate, HourlyBillRate, EffictiveDt FROM Rates LIMIT 10";
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rates.Add(new Rate
            {
                Contractor_Name = reader.GetString(0),
                Job_Title = reader.GetString(1),
                Skill_Level = reader.GetString(2),
                HourlyWageRate = ParseCurrencyValue(reader.GetString(3)),
                HourlyBillRate = ParseCurrencyValue(reader.GetString(4)),
                EffictiveDt = reader.GetString(5)
            });
        }
        
        return rates;
    }

    public async Task<List<Rate>> GetRatesByJobTitleAsync(string jobTitle)
    {
        var rates = new List<Rate>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Contractor_Name, Job_Title, Skill_Level, HourlyWageRate, HourlyBillRate, EffictiveDt FROM Rates WHERE Job_Title = @JobTitle ORDER BY HourlyBillRate";
        command.Parameters.Add(new SqliteParameter("@JobTitle", jobTitle));
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rates.Add(new Rate
            {
                Contractor_Name = reader.GetString(0),
                Job_Title = reader.GetString(1),
                Skill_Level = reader.GetString(2),
                HourlyWageRate = ParseCurrencyValue(reader.GetString(3)),
                HourlyBillRate = ParseCurrencyValue(reader.GetString(4)),
                EffictiveDt = reader.GetString(5)
            });
        }
        
        return rates;
    }

    public async Task<List<Rate>> GetRatesBySkillLevelAsync(string skillLevel)
    {
        var rates = new List<Rate>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Contractor_Name, Job_Title, Skill_Level, HourlyWageRate, HourlyBillRate, EffictiveDt FROM Rates WHERE Skill_Level = @SkillLevel ORDER BY HourlyBillRate";
        command.Parameters.Add(new SqliteParameter("@SkillLevel", skillLevel));
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rates.Add(new Rate
            {
                Contractor_Name = reader.GetString(0),
                Job_Title = reader.GetString(1),
                Skill_Level = reader.GetString(2),
                HourlyWageRate = ParseCurrencyValue(reader.GetString(3)),
                HourlyBillRate = ParseCurrencyValue(reader.GetString(4)),
                EffictiveDt = reader.GetString(5)
            });
        }
        
        return rates;
    }

    public async Task<string> TestDatabaseConnectionAsync()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            
            // Test 1: Count total records
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Rates";
                var count = await command.ExecuteScalarAsync();
                Console.WriteLine($"Total records in Rates table: {count}");
            }
            
            // Test 2: Get sample data
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Contractor_Name, Job_Title, Skill_Level, HourlyBillRate FROM Rates LIMIT 3";
                using var reader = await command.ExecuteReaderAsync();
                var results = new List<string>();
                while (await reader.ReadAsync())
                {
                    var billRate = ParseCurrencyValue(reader.GetString(3));
                    results.Add($"{reader.GetString(0)} - {reader.GetString(1)} - {reader.GetString(2)} - ${billRate:F2}");
                }
                Console.WriteLine("Sample data:");
                foreach (var result in results)
                {
                    Console.WriteLine($"  {result}");
                }
            }
            
            // Test 3: Test the exact search
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*) 
                    FROM Rates 
                    WHERE Job_Title = @JobTitle AND Skill_Level = @SkillLevel";
                command.Parameters.Add(new SqliteParameter("@JobTitle", "Business Analyst"));
                command.Parameters.Add(new SqliteParameter("@SkillLevel", "Expert"));
                
                var count = await command.ExecuteScalarAsync();
                Console.WriteLine($"Business Analyst + Expert count: {count}");
            }
            
            return "Database connection test completed successfully";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
            return $"Database connection test failed: {ex.Message}";
        }
    }
} 