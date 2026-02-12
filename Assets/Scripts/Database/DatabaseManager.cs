// Created by: John
// Description: Database manager for user data and game information or others

using System;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public static class DatabaseManager
{
    private static readonly FirebaseDatabase databaseInstance = FirebaseDatabase.DefaultInstance;

    // Player Data
    private static readonly DatabaseReference userDataReference = databaseInstance.GetReference("PlayerData");

    // Retrieve player data from database
    public async static Task<DatabaseResult> GetUserDataAsync()
    {
        DatabaseResult databaseResult = new DatabaseResult();

        try
        {
            if (DatabaseAccountManager.isAuthenticated()) // Check if there is a user logged in
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

                // Check if the database returns anything withing 5 seconds
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

    // Saves the user data to the database
    public async static void SaveUserDataAsync(Player player)
    {
        try
        {
            if (DatabaseAccountManager.isAuthenticated()) // Check if there is a user logged in
            {
                Debug.Log("User Data Write started");
                string userID = DatabaseAccountManager.user.UserId;
                await userDataReference.Child(userID).SetRawJsonValueAsync(JsonUtility.ToJson(player.playerData)); // Set data at the userID path
                
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

    // Get value from a specific path
    public async static Task<DatabaseResult> GetValueFromPath(string path)
    {
        DatabaseResult databaseResult = new DatabaseResult();
        DatabaseReference databaseReference = databaseInstance.GetReference(path); // Set reference to specified path

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
            
            // Check if the database returns anything withing 5 seconds
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

    // Set value in a specified path with the JSON
    public async static void SetValueInPath(string path, string valueJSON)
    {
        databaseInstance.SetPersistenceEnabled(false);
        DatabaseReference databaseReference = databaseInstance.GetReference(path); // Set reference to specified path

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
