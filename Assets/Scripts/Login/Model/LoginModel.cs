public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public string VersionNote { get; set; }
    public string Version { get; set; }
    public bool IsProcessing { get; set; }
}