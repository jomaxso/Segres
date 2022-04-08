namespace ClassLibrary2;

public class Employee 
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public Job Job { get; init; }
}

public class Job
{
    public Guid Id { get; set; }
    public string Titel { get; set; }
}

