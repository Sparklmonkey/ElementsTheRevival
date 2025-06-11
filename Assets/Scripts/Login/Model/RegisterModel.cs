namespace Login.Model
{
    public class RegisterModel
    {
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsProcessing { get; set; }
    }
}