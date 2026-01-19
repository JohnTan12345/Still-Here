using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public static class DatabaseManager
{
    static readonly FirebaseDatabase databaseInstance = FirebaseDatabase.DefaultInstance;

    // Player Data
    static readonly DatabaseReference userDataReference = databaseInstance.GetReference("PlayerData");
    public async static Task<DatabaseResult> GetUserDataAsync()
    {
        databaseInstance.SetPersistenceEnabled(false);
        DatabaseResult databaseResult = new DatabaseResult();

        try
        {
            if (DatabaseAccountManager.user == null)
            {
                databaseResult.faulted = true;
                databaseResult.errorMessage = "No user logged in";
            }
            else
            {
                Debug.Log("User Data Read started");
                string userID = DatabaseAccountManager.user.UserId;
                databaseResult.snapshot = await userDataReference.Child(userID).GetValueAsync();
            }
        }
        catch (DatabaseException databaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = databaseException.Message;

        }
        finally
        {
            Debug.Log("User Data Read finished");
        }

        return databaseResult;
    }

    public async static void SaveUserDataAsync(Player player)
    {
        try
        {
            if (DatabaseAccountManager.user == null)
            {
                Debug.Log("No user logged in");
            } 
            else
            {
                Debug.Log("User Data Write started");
                string userID = DatabaseAccountManager.user.UserId;
                await userDataReference.Child(userID).SetValueAsync(player.playerData);
            }
        }
        catch (DatabaseException databaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
        }
        finally
        {
            Debug.Log("User Data Write finished");
        }
    }

    public async static Task<DatabaseResult> GetValueFromPath(string path)
    {
        DatabaseResult databaseResult = new DatabaseResult();
        databaseInstance.SetPersistenceEnabled(false);
        DatabaseReference databaseReference = databaseInstance.GetReference(path);

        try
        {
            Debug.Log($"Read from path: {path} started");
            databaseResult.snapshot = await databaseReference.GetValueAsync();
        }
        catch (DatabaseException databaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = databaseException.Message;
        }
        finally
        {
            Debug.Log($"Read from path: {path} finished");
        }

        return databaseResult;
    }

    public async static void SetValueInPath(string path, string valueJSON)
    {
        databaseInstance.SetPersistenceEnabled(false);
        DatabaseReference databaseReference = databaseInstance.GetReference(path);

        try
        {
            await databaseReference.SetRawJsonValueAsync(valueJSON);
        }
        catch (DatabaseException databaseException)
        {
            Debug.Log($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
        }
        finally
        {
            Debug.Log("User Data Write finished");
        }
    }
}

public class DatabaseResult
{
    public DataSnapshot snapshot;
    public bool faulted = false;
    public string errorMessage;
}
