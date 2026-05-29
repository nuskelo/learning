using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

List<Runner> runners = new List<Runner>();

runners.Add(new Runner("Vlad", 200));
runners.Add(new Runner("Dima", 150));
runners.Add(new Runner("Savchuk", 100));
runners.Add(new Runner("Yarik", 250));
runners.Add(new Runner("Tihon", 50));

List<RunRecord> runRecords = new List<RunRecord>();

runRecords.Add(new RunRecord(10, 55, new DateTime(2026, 05, 01), 0));
runRecords.Add(new RunRecord(12.5, 65, new DateTime(2026, 05, 05), 0));
runRecords.Add(new RunRecord(8, 42, new DateTime(2026, 05, 10), 0));
runRecords.Add(new RunRecord(21.1, 115, new DateTime(2026, 05, 15), 0));

runRecords.Add(new RunRecord(5, 25, new DateTime(2026, 05, 02), 1));
runRecords.Add(new RunRecord(5, 24, new DateTime(2026, 05, 06), 1));
runRecords.Add(new RunRecord(7, 35, new DateTime(2026, 05, 12), 1));
runRecords.Add(new RunRecord(10, 52, new DateTime(2026, 05, 18), 1));

runRecords.Add(new RunRecord(3, 15, new DateTime(2026, 05, 03), 2));
runRecords.Add(new RunRecord(4, 22, new DateTime(2026, 05, 08), 2));
runRecords.Add(new RunRecord(3.5, 18, new DateTime(2026, 05, 14), 2));

runRecords.Add(new RunRecord(15, 75, new DateTime(2026, 05, 04), 3));
runRecords.Add(new RunRecord(18, 95, new DateTime(2026, 05, 09), 3));
runRecords.Add(new RunRecord(15, 72, new DateTime(2026, 05, 16), 3));
runRecords.Add(new RunRecord(20, 105, new DateTime(2026, 05, 22), 3));

runRecords.Add(new RunRecord(2, 8, new DateTime(2026, 05, 07), 4));
runRecords.Add(new RunRecord(2.5, 11, new DateTime(2026, 05, 11), 4));
runRecords.Add(new RunRecord(3, 13, new DateTime(2026, 05, 20), 4));

app.MapGet("/runners", () =>
{
    CalculateAllTotalKm();
    if (runners.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(runners);

});

app.MapGet("/runrecords", () =>
{
    return runRecords;
});

app.MapGet("/runners/{id}", (int id) =>
{
    int ID = runners.FindIndex(i => i.Id == id);
    if (ID == -1)
    {
        return Results.NotFound();
    }
    runners[ID].ResetTotalKm();
    foreach (var runRecord in runRecords)
    {
        if (runRecord.RunnerId == runners[ID].Id)
        {
            runners[ID].AddKm(runRecord.Distance);
        }
    }
    return Results.Ok(runners[ID]);
});

app.MapGet("/runrecords/{id}", (int id) =>
{
    foreach (var runRecord in runRecords)
    {
        if (runRecord.Id == id) return Results.Ok(runRecord);
    }
    return Results.NotFound();
});

app.MapPost("/runners/add", (Runner newRunner) =>
{
    runners.Add(newRunner);
    return Results.Ok(newRunner);
});

app.MapPost("/runrecords/add", (RunRecord newRunRecord) =>
{
    runRecords.Add(newRunRecord);
    return Results.Ok(newRunRecord);
});

app.MapPut("runners/{id}", (int id, Runner newRunner) =>
{
    int ID = runners.FindIndex(i => i.Id == id);

    if (ID == -1)
    {
        return Results.NotFound();
    }
    else
    {
        runners[ID].Name = newRunner.Name;
        runners[ID].Goal = newRunner.Goal;
        return Results.Ok(runners[ID]);
    }
});

app.MapPut("runrecords/{id}", (int id, RunRecord newRunRecord) =>
{
    int ID = runRecords.FindIndex(i => i.Id == id);
    if (ID == -1)
    {
        return Results.NotFound();
    }
    else
    {
        runRecords[ID].Distance = newRunRecord.Distance;
        runRecords[ID].RunnerId = newRunRecord.RunnerId;
        runRecords[ID].TimeMin = newRunRecord.TimeMin;

        return Results.Ok(runRecords[ID]);
    }
});

app.MapDelete("/runners/{id}", (int id) =>
{
    int ID = runners.FindIndex(i => i.Id == id);
    if (ID == -1)
    {
        return Results.NotFound();
    }
    else
    {
        var deletedRunner = runners[ID];
        runners.RemoveAt(ID);
        return Results.Ok(deletedRunner);
    }
});

app.MapDelete("/runrecords/{id}", (int id) =>
{
    int ID = runRecords.FindIndex(i => i.Id == id);
    if (ID == -1)
    {
        return Results.NotFound();
    }
    else
    {
        var deletedRunRecord = runRecords[ID];
        runRecords.RemoveAt(ID);
        return Results.Ok(deletedRunRecord);
    }
});

app.MapGet("/runners/{id}/runs", (int id) =>
{
    return runRecords.Where(r => r.RunnerId == id);
});

app.MapGet("runners/top", () =>
{
    CalculateAllTotalKm();
    var topRunners = runners.OrderByDescending(r => r.TotalKm);
    if (topRunners.Count() == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(topRunners);
});

void CalculateAllTotalKm()
{
    for (int i = 0; i < runners.Count(); i++)
    {
        runners[i].ResetTotalKm();
        foreach (var runRecord in runRecords)
        {
            if (runRecord.RunnerId == runners[i].Id)
            {
                runners[i].AddKm(runRecord.Distance);
            }
        }
    }
}

app.Run();

public class Runner
{
    private static int _id = 0;
    public string? Name { get; set; }
    public int Id { get; private set; }
    public double Goal { get; set; }
    public double TotalKm { get; private set; }

    public double GoalPercent
    {
        get
        {
            if (Goal == 0) return 0;
            return TotalKm * 100 / Goal;
        }
    }

    public Runner()
    {
        Id = _id++;
    }
    public Runner(string name)
    {
        Name = name;
        Id = _id++;
    }

    public Runner(string name, double goal)
    {
        Name = name;
        Goal = goal;
        Id = _id++;
    }

    public void ResetTotalKm()
    {
        TotalKm = 0;
    }

    public void AddKm(double distance)
    {
        TotalKm += distance;
    }

}

public class RunRecord
{
    private static int _id = 0;
    private double _paceMin;
    public int RunnerId { get; set; }
    public int Id { get; private set; }
    public double Distance { get; set; }
    public double TimeMin { get; set; }
    public DateTime Date { get; set; }
    public double PaceMin
    {
        get
        {
            if (Distance == 0) return 0;
            return TimeMin / Distance;
        }
        private set
        {
            if (value < 0)
            {
                _paceMin = 0;
            }
            else
            {
                _paceMin = value;
            }
        }
    }

    public RunRecord()
    {
        Id = _id++;
    }
    public RunRecord(double distance, double timeMin, DateTime date, int runnerId)
    {
        Distance = distance;
        TimeMin = timeMin;
        Date = date;
        RunnerId = runnerId;
        Id = _id++;
    }
}