using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public static class DatabaseAccountManager
{
    private static readonly FirebaseAuth AuthenticationDatabase = FirebaseAuth.DefaultInstance;
    public static readonly FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
    public static bool isAuthenticated() => user != null;

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
            accountResult.errorMessage = FirebaseAccountErrorHandler(firebaseException);
            
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
            accountResult.errorMessage = FirebaseAccountErrorHandler(firebaseException);
        }

        return accountResult;
    }

    public static async void SignOutAccount()
    {
        AuthenticationDatabase.SignOut();
    }

    private static string FirebaseAccountErrorHandler(FirebaseException firebaseException)
    {
        string errorMessage = "";
        AuthError authError = (AuthError)firebaseException.ErrorCode;
        // Deal with auth error messages later
        return errorMessage;
    }
    
    
}

public class AccountResult
{
    public string userID;
    public bool faulted = false;
    public string errorMessage;
}