namespace PersonalFinanceManager.Shared.Requests.Auth;

public class GoogleSignInRequest
{
    public required string IdToken { get; init; }
}
