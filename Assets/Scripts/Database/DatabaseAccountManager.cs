// Created by: John
// Description: Account Database manager and Auth handler

using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public static class DatabaseAccountManager
{
    private static readonly FirebaseAuth AuthenticationDatabase = FirebaseAuth.DefaultInstance; // Set instance to default
    public static FirebaseUser user {get; private set;}
    public static bool signedIn {get; private set;} = false;
    public static bool isAuthenticated() => user != null && !string.IsNullOrEmpty(user.Email);

    // Add listener to StageChanged event
    public static void Initialize()
    {
        AuthenticationDatabase.StateChanged += AuthStateChanged;
        AuthStateChanged(null, null);
    }

    // Update the current user
    private static void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (AuthenticationDatabase.CurrentUser != user)
        {
            user = AuthenticationDatabase.CurrentUser;
        }
    }    

    // Create a new account with given email and password
    public static async Task<AccountResult> CreateAccountWithEmailAndPasswordAsync(string email, string password)
    {
        AccountResult accountResult = new AccountResult();

        try
        {
            AuthResult result = await AuthenticationDatabase.CreateUserWithEmailAndPasswordAsync(email, password); // Attempt to create a new account
            accountResult.userID = result.User.UserId; // forward userID to return
        } catch (FirebaseException firebaseException)
        {
            Debug.Log($"Something went wrong.\nException Source: Firebase\nException: {firebaseException.Message}");
            accountResult.faulted = true;
            accountResult.errorMessage = FirebaseAccountErrorHandler(false, firebaseException); // Handles the error and gives error message for error logging later
            
        }

        return accountResult;
    }

    // Sign in to an existing account with given email and password
    public static async Task<AccountResult> SignInAccountWithEmailAndPasswordAsync(string email, string password)
    {
        AccountResult accountResult = new AccountResult();

        try
        {
            AuthResult result = await AuthenticationDatabase.SignInWithEmailAndPasswordAsync(email, password); // Attempt to sign in to an existing account
            accountResult.userID = result.User.UserId; // forward userID to return
        } catch (FirebaseException firebaseException)
        {
            Debug.Log($"Something went wrong.\nException Source: Firebase\nException: {firebaseException.Message}");
            accountResult.faulted = true;
            accountResult.errorMessage = FirebaseAccountErrorHandler(true, firebaseException); // Handles the error and gives error message for error logging later
        }

        return accountResult;
    }

    // Signs out of the current account
    public static void SignOutAccount()
    {
        AuthenticationDatabase.SignOut();
    }

    // Handler function for firebase related errors
    private static string FirebaseAccountErrorHandler(bool loginStep, FirebaseException firebaseException)
    {
        string errorMessage = ""; // Define error Message string
        AuthError authError = (AuthError)firebaseException.ErrorCode;
        
        switch (authError)
        {
            case AuthError.Failure: // When there is no account or failed login attempt
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
            case AuthError.InvalidCredential: // Using wrong credential
            {
                errorMessage = "Invalid login method, try another method";
                break;
            }
            case AuthError.UserDisabled: // Disabled user
            {
                errorMessage = "Your account is disabled";
                break;
            }
            case AuthError.AccountExistsWithDifferentCredentials: // Using different credential for an existing account
            {
                errorMessage = "Account exists with another login method, try that method instead";
                break;
            }
            case AuthError.OperationNotAllowed: // Operation not allowed
            {
                errorMessage = "Operation not allowed";
                break;
            }
            case AuthError.EmailAlreadyInUse: // Email in use
            {
                errorMessage = "Account exists, please login instead";
                break;
            }
            case AuthError.CredentialAlreadyInUse: // Credential in use
            {
                errorMessage = "Account exists, please login instead";
                break;
            }
            case AuthError.TooManyRequests: // Rate limiter
            {
                errorMessage = "Too many requests made, please try again later";
                break;
            }
            case AuthError.UserNotFound: // No such user
            {
                errorMessage = "Account does not exist";
                break;
            }
            case AuthError.WeakPassword: // Weak password
            {
                errorMessage = "Password must be at least 8 characters long";
                break;
            }
            case AuthError.MissingEmail: // No email
            {
                errorMessage = "Please enter an email";
                break;
            }
            case AuthError.MissingPassword: // No password
            {
                errorMessage = "Please enter a password";
                break;
            }
            default: // For any other errors not covered
            {
                errorMessage = $"Something went wrong. Please report this to the developers: Login Error Code: {firebaseException.ErrorCode}";
                break;
            }
        }
        
        return errorMessage;
    }
}

// Wrapper for result
public class AccountResult
{
    public string userID;
    public bool faulted = false;
    public string errorMessage;
}