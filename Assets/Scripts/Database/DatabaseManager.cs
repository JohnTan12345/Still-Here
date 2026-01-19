using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public static class DatabaseManager
{
    private static readonly FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
    private static bool isAuthenticated() => user.UserId != null || user.UserId != "";
    static readonly FirebaseDatabase databaseInstance = FirebaseDatabase.DefaultInstance;

    // Player Data
    static readonly DatabaseReference userDataReference = databaseInstance.GetReference("PlayerData");
    public async static Task<DatabaseResult> GetUserDataAsync()
    {
        databaseInstance.SetPersistenceEnabled(false);
        DatabaseResult databaseResult = new DatabaseResult();

        try
        {
            if (!isAuthenticated())
            {
                databaseResult.faulted = true;
                databaseResult.errorMessage = "No user logged in";
            }
            else
            {
                string userID = user.UserId;
                DataSnapshot userDataSnapshot = await userDataReference.Child(userID).GetValueAsync();
                databaseResult.value = userDataSnapshot.GetRawJsonValue();
            }
        }
        catch (FirebaseException firebaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{firebaseException.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = firebaseException.Message;

        }

        return databaseResult;
    }

    public async static void SaveUserDataAsync(Player player)
    {
        try
        {
            if (!isAuthenticated())
            {
                Debug.Log("No user logged in");
            } 
            else
            {
                
            }
        }
        catch (DatabaseException DatabaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{DatabaseException.Message}");
        }
        
    }
}

public class DatabaseResult
{
    public string value;
    public bool faulted = false;
    public string errorMessage;
}
