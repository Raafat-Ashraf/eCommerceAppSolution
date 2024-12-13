namespace eCommerceApp.Application.Dtos.Identity;

public class CreateUser : BaseModel
{
    public required string FullName { get; set; }
    public required string ConfirmPassword { get; set; }
}