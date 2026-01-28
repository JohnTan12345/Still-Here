using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField email;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button signupButton;

    void Start()
    {
        loginButton.onClick.AddListener(onLoginButtonClicked);
        signupButton.onClick.AddListener(onSignUpButtonClicked);
    }

    private void onLoginButtonClicked()
    {
       StartCoroutine(Login()); 
    }
    private void onSignUpButtonClicked()
    {
       StartCoroutine(SignUp()); 
    }
    private IEnumerator Login()
    {
        Task<AccountResult> accountTask = DatabaseAccountManager.SignInAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(()=> accountTask.IsCompleted);

        if (accountTask.Result.faulted)
        {
            Debug.Log("No account m8");
            yield break;
        }

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        Debug.Log("Player Created!");
    }
    private IEnumerator SignUp()
    {
        Task<AccountResult> accountResult = DatabaseAccountManager.CreateAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(()=> accountResult.IsCompleted);

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        Debug.Log("Player Created!");
    }
}
