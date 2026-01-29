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

    private bool login = true;

    void Start()
    {
        loginSignUpButton.onClick.AddListener(onButtonClicked);
        switchButton.onClick.AddListener(onSwitch);
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
        Task<AccountResult> accountTask = DatabaseAccountManager.SignInAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(() => accountTask.IsCompleted);

        if (accountTask.Result.faulted)
        {
            Debug.Log("No account m8");
            yield break;
        }

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        MainMenuUIManager.instance.ReturnToMainPanel();
    }
    private IEnumerator SignUp()
    {
        Task<AccountResult> accountTask = DatabaseAccountManager.CreateAccountWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(()=> accountTask.IsCompleted);

        if (accountTask.Result.faulted)
        {
            Debug.Log("e");
            yield break;
        }

        Player player = new Player();
        Task createPlayerTask = player.CreatePlayer();
        yield return new WaitUntil(() => createPlayerTask.IsCompleted);
        MainMenuUIManager.instance.ReturnToMainPanel();
    }
}
