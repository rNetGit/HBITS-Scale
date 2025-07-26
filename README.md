# HBITS Rate Explorer

A Blazor Server web application for searching and displaying HBITS (Health and Human Services Information Technology Services) contractor rates.

## Features

- **Search by Job Title**: Filter rates by specific job titles
- **Search by Skill Level**: Filter rates by skill levels
- **Ranked Results**: Results are automatically ranked by Hourly Bill Rate (lowest to highest)
- **Mobile-Friendly**: Responsive design that works on desktop and mobile devices
- **Real-time Search**: Fast search functionality with loading indicators

## Database Schema

The application connects to a SQLite database with the following Rates table structure:

```sql
CREATE TABLE Rates (
    Contractor_Name TEXT (100),
    Job_Title       TEXT (50),
    Skill_Level     TEXT (50),
    HourlyWageRate  REAL (5),
    HourlyBillRate  REAL (5),
    EffictiveDt     TEXT (4) 
);
```

## Search Results Display

The search results show the following information:
- **Rank**: Position based on Hourly Bill Rate (1 = lowest rate)
- **Contractor Name**: Name of the contractor
- **Job Title**: Specific job title
- **Skill Level**: Skill level classification
- **Hourly Bill Rate**: Billable hourly rate (formatted as currency)
- **Effective Date**: Date when the rate became effective

## Running the Application

1. Ensure the `HBITS.db` file is in the application root directory
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open your browser and navigate to `http://localhost:5000`

## Technology Stack

- **.NET 9**: Latest .NET framework
- **Blazor Server**: Interactive web UI framework
- **SQLite**: Lightweight database
- **Bootstrap 5**: Responsive CSS framework
- **Microsoft.Data.Sqlite**: SQLite data provider

## Project Structure

```
HBITSExplorer/
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor
│   └── Pages/
│       └── RateSearch.razor
├── Models/
│   └── Rate.cs
├── Services/
│   └── RateService.cs
├── wwwroot/
│   └── app.css
├── HBITS.db
└── Program.cs
```

## Mobile Responsiveness

The application is designed to be mobile-friendly with:
- Responsive grid layout
- Touch-friendly buttons and controls
- Optimized table display for small screens
- Appropriate font sizes for mobile devices
- Horizontal scrolling for wide tables 