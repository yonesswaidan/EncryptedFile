
public class RegistrationAttempt
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public DateTime AttemptTime { get; set; }
    public bool Success { get; set; }
    public string? UserName { get; set; }
}
