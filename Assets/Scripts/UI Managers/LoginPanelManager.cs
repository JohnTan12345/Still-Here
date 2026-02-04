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
    private Button loginSignUpButton;
    [SerializeField]
    private TextMeshProUGUI loginSignUpText;
    [SerializeField]
    private Button switchButton;
    [SerializeField]
    private TextMeshProUGUI switchText;
    [SerializeField]
    private TextMeshProUGUI errorMessage;

    private bool login = true;

    void Start()
    {
        loginSignUpButton.onClick.AddListener(onButtonClicked);
        switchButton.onClick.AddListener(onSwitch);
        errorMessage.gameObject.SetActive(false);
    }

    private void onButtonClicked()
    {
        if (login)
        {
            StartCoroutine(Login()); 
        }
        else
        {
            StartCoroutine(SignUp());
        }
    }
    private void onSwitch()
    {
        login = !login;
        if (login)
        {
            loginSignUpText.text = "Login";
            switchText.text = "No Account?";
        }
        else
        {
            loginSignUpText.text = "Sign Up";
            switchText.text = "Have Account?";
        }
    }
    private IEnumerator Login()
    {
        bool valid = CheckInputs();

        if (!valid)
        {
            yield break;
        }

        Task<AccountResult> accountTask = DatabaseAccountManager.SignInAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(() => accountTask.IsCompleted);

        if (accountTask.Result.faulted)
        {
            AccountLogError(accountTask.Result.errorMessage);
            yield break;
        }

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        MainMenuUIManager.Instance.ReturnToMainPanel();

        password.text = "";
    }
    private IEnumerator SignUp()
    {
        bool valid = CheckInputs();

        if (!valid)
        {
            yield break;
        }
        Task<AccountResult> accountTask = DatabaseAccountManager.CreateAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(()=> accountTask.IsCompleted);

        if (accountTask.Result.faulted)
        {
            AccountLogError(accountTask.Result.errorMessage);
            yield break;
        }

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        MainMenuUIManager.Instance.ReturnToMainPanel();

        password.text = "";
    }

    private bool CheckInputs()
    {
        errorMessage.gameObject.SetActive(false);

        if (email.text.Length == 0)
        {
            AccountLogError("Please enter an email");
            return false;
        }
        else if (!(email.text.Contains("@") || email.text.Contains(".")))
        {
            AccountLogError("Invalid email");
            return false;
        }
        else if (password.text.Length == 0)
        {
            AccountLogError("Please enter a password");
            return false;
        }
        else if (password.text.Length < 8)
        {
            if (login)
            {
                AccountLogError("Invalid password");
            }
            else
            {
                AccountLogError("Password must be at least 8 characters long");
            }

            return false;
        }

        return true;
    }

    private void AccountLogError(string message)
    {
        Debug.LogError(message);
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }
}
