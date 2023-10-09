namespace Authen.Cookie.Models.RequestModels
{
    public sealed record User
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
