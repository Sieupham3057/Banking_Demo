using Microsoft.AspNetCore.Identity;

namespace Banking.Domain.Entities;

public class UserApplication : IdentityUser<Guid>
{
    public string FulName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
