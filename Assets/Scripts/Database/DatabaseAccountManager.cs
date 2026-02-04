using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public static class DatabaseAccountManager
{
    private static readonly FirebaseAuth AuthenticationDatabase = FirebaseAuth.DefaultInstance;
    public static FirebaseUser user {get; private set;}
    public static bool signedIn {get; private set;} = false;
    public static bool isAuthenticated() => user != null && !string.IsNullOrEmpty(user.Email);

    public static void Initialize()
    {
        AuthenticationDatabase.StateChanged += AuthStateChanged;
        AuthStateChanged(null, null);
    }

    private static void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (AuthenticationDatabase.CurrentUser != user)
        {
            user = AuthenticationDatabase.CurrentUser;
        }
    }    
    public static async Task<AccountResult> CreateAccountWithEmailAndPasswordAsync(string email, string password)
    {
        AccountResult accountResult = new AccountResult();

        try
        {
            AuthResult result = await AuthenticationDatabase.CreateUserWithEmailAndPasswordAsync(email, password);
            accountResult.userID = result.User.UserId;
        } catch (FirebaseException firebaseException)
        {
            Debug.Log($"Something went wrong.\nException Source: Firebase\nException: {firebaseException.Message}");
            accountResult.faulted = true;
            accountResult.errorMessage = FirebaseAccountErrorHandler(false, firebaseException);
            
        }

        return accountResult;
    }

    public static async Task<AccountResult> SignInAccountWithEmailAndPasswordAsync(string email, string password)
    {
        AccountResult accountResult = new AccountResult();

        try
        {
            AuthResult result = await AuthenticationDatabase.SignInWithEmailAndPasswordAsync(email, password);
            accountResult.userID = result.User.UserId;
        } catch (FirebaseException firebaseException)
        {
            Debug.Log($"Something went wrong.\nException Source: Firebase\nException: {firebaseException.Message}");
            accountResult.faulted = true;
            accountResult.errorMessage = FirebaseAccountErrorHandler(true, firebaseException);
        }

        return accountResult;
    }

    public static void SignOutAccount()
    {
        AuthenticationDatabase.SignOut();
    }

    private static string FirebaseAccountErrorHandler(bool loginStep, FirebaseException firebaseException)
    {
        string errorMessage = "";
        AuthError authError = (AuthError)firebaseException.ErrorCode;
        
        switch (authError)
        {
            case AuthError.Failure:
            {
                if (loginStep)
                    {
                        errorMessage = "Email or password is incorrect";
                    }
                    else
                    {
                        errorMessage = "Account exists, please login instead";
                    }
                break;
            }
            case AuthError.InvalidCredential:
            {
                errorMessage = "Invalid login method, try another method";
                break;
            }
            case AuthError.UserDisabled:
            {
                errorMessage = "Your account is disabled";
                break;
            }
            case AuthError.AccountExistsWithDifferentCredentials:
            {
                errorMessage = "Account exists with another login method, try that method instead";
                break;
            }
            case AuthError.OperationNotAllowed:
            {
                errorMessage = "Operation not allowed";
                break;
            }
            case AuthError.EmailAlreadyInUse:
            {
                errorMessage = "Account exists, please login instead";
                break;
            }
            case AuthError.CredentialAlreadyInUse:
            {
                errorMessage = "Account exists, please login instead";
                break;
            }
            case AuthError.TooManyRequests:
            {
                errorMessage = "Too many requests made, please try again later";
                break;
            }
            case AuthError.UserNotFound:
            {
                errorMessage = "Account does not exist";
                break;
            }
            case AuthError.WeakPassword:
            {
                errorMessage = "Password must be at least 8 characters long";
                break;
            }
            case AuthError.MissingEmail:
            {
                errorMessage = "Please enter an email";
                break;
            }
            case AuthError.MissingPassword:
            {
                errorMessage = "Please enter a password";
                break;
            }
            default:
            {
                errorMessage = $"Something went wrong. Please report this to the developers: Login Error Code: {firebaseException.ErrorCode}";
                break;
            }
        }
        
        return errorMessage;
    }
}

public class AccountResult
{
    public string userID;
    public bool faulted = false;
    public string errorMessage;
}