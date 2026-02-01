using System;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public static class DatabaseManager
{
    private static readonly FirebaseDatabase databaseInstance = FirebaseDatabase.DefaultInstance;

    // Player Data
    private static readonly DatabaseReference userDataReference = databaseInstance.GetReference("PlayerData");

    public async static Task<DatabaseResult> GetUserDataAsync()
    {
        DatabaseResult databaseResult = new DatabaseResult();

        try
        {
            if (DatabaseAccountManager.isAuthenticated())
            {
                Debug.Log("User Data Read started");
                
                // A more complicated way to make sure the data returned is fresh (too many times data return was old)
                TaskCompletionSource<DataSnapshot> userDataTCS = new TaskCompletionSource<DataSnapshot>();
                EventHandler<ValueChangedEventArgs> ValueChangedHandler = null;

                ValueChangedHandler = (obj, eventArgs) => // local function that completes the task then removes listener
                {
                    userDataTCS.SetResult(eventArgs.Snapshot);
                    userDataReference.Child(DatabaseAccountManager.user.UserId).ValueChanged -= ValueChangedHandler;
                };

                userDataReference.Child(DatabaseAccountManager.user.UserId).ValueChanged += ValueChangedHandler; // run ValueChangedHandler since ValueChanged event runs when a listener is attached to it.
                
                await Task.WhenAny(userDataTCS.Task, Task.Delay(5000)); // Wait for either DataSnapshot or timeout

                if (userDataTCS.Task.IsCompleted == true)
                {
                    if (userDataTCS.Task.IsFaulted) {
                        throw userDataTCS.Task.Exception;
                    } else
                    {
                        databaseResult.snapshot = userDataTCS.Task.Result;
                    }
                }
                else
                {
                    throw new Exception("Database connection timeout");
                }
            }
            else
            {
                databaseResult.faulted = true;
                databaseResult.errorMessage = "No user logged in";
            }
        }
        catch (DatabaseException databaseException)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = databaseException.Message;

        }
        catch (Exception exception)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{exception.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = exception.Message;
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
            if (DatabaseAccountManager.isAuthenticated())
            {
                Debug.Log("User Data Write started");
                string userID = DatabaseAccountManager.user.UserId;
                await userDataReference.Child(DatabaseAccountManager.user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(player.playerData));
                
            } 
            else
            {
                Debug.LogError("No user logged in");
            }
        }
        catch (DatabaseException databaseException)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{exception.Message}");
        }
        finally
        {
            Debug.Log("User Data Write finished");
        }
    }

    public async static Task<DatabaseResult> GetValueFromPath(string path)
    {
        DatabaseResult databaseResult = new DatabaseResult();
        DatabaseReference databaseReference = databaseInstance.GetReference(path);

        try
        {
            Debug.Log($"Read from path: {path} started");
            
            // A more complicated way to make sure the data returned is fresh (too many times data return was old)
            TaskCompletionSource<DataSnapshot> pathValueTCS = new TaskCompletionSource<DataSnapshot>();
            EventHandler<ValueChangedEventArgs> ValueChangedHandler = null;

            ValueChangedHandler = (obj, eventArgs) => // local function that completes the task then removes listener
            {
                pathValueTCS.SetResult(eventArgs.Snapshot);
                databaseReference.ValueChanged -= ValueChangedHandler;
            };

            databaseReference.ValueChanged += ValueChangedHandler; // run ValueChangedHandler since ValueChanged event runs when a listener is attached to it.
                
            await Task.WhenAny(pathValueTCS.Task, Task.Delay(5000)); // Wait for either DataSnapshot or timeout

            if (pathValueTCS.Task.IsCompleted == true)
            {
                if (pathValueTCS.Task.IsFaulted) {
                    throw pathValueTCS.Task.Exception;
                } else
                {
                    databaseResult.snapshot = pathValueTCS.Task.Result;
                }
            }
            else
            {
                throw new Exception("Database connection timeout");
            }
            
        }
        catch (DatabaseException databaseException)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = databaseException.Message;
        }
        catch (Exception exception)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{exception.Message}");
            databaseResult.faulted = true;
            databaseResult.errorMessage = exception.Message;
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
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{databaseException.Message}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"An error occured when fetching data.\nSource: Firebase\nException:{exception.Message}");
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
