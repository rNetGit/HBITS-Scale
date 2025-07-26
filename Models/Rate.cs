namespace HBITSExplorer.Models;

public class Rate
{
    public string Contractor_Name { get; set; } = string.Empty;
    public string Job_Title { get; set; } = string.Empty;
    public string Skill_Level { get; set; } = string.Empty;
    public decimal HourlyWageRate { get; set; }
    public decimal HourlyBillRate { get; set; }
    public string EffictiveDt { get; set; } = string.Empty;
} 