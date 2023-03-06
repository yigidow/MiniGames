using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

namespace YY_Games_Scripts
{
    public class LoginCanvas : MonoBehaviour
    {
        #region References to Objects, Scripts and Components

        [Header("Main Menu Variables")]
        //Login button
        [SerializeField] private Button loginButton;
        //Register button
        [SerializeField] private Button registerButton;
        [SerializeField] private Button quitButton;

        [Header("Login Menu Variables")]
        [SerializeField] private Button loginMenuLoginButton;
        [SerializeField] private Button forgetPasswordButton;
        [SerializeField] private Button loginMenuBackButton;

        private string loginEmailAddress, loginPassword;

        [Header("Register Menu Variables")]
        [SerializeField] private Button registerMenuRegisterButton;
        [SerializeField] private Button registerMenuBackButton;

        private string registerEmailAddress, registerPassword, registerUsername;

        [Header("Account Recovery Menu Variables")]
        [SerializeField] private Button accountRecoveryButton;
        [SerializeField] private Button accountRecoveryMenuBackButton;

        private string accountRecoveryEmailAddress;

        [Header("References to Objects, Scripts and Components")]
        //main menu
        [SerializeField] private GameObject mainMenuRef;

        //Login
        [SerializeField] private GameObject loginRef;

        //Register
        [SerializeField] private GameObject registerRef;

        //Account Recovery
        [SerializeField] private GameObject accountRecoveryRef;

        //Error Text
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Main Menu Functions
        private void OnRegisterButtonClick()
        {
            mainMenuRef.SetActive(false);
            registerRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        private void OnLoginButtonClick()
        {
            mainMenuRef.SetActive(false);
            loginRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        private void SetUpMainMenuButtons()
        {
            loginButton.onClick.AddListener(OnLoginButtonClick);
            registerButton.onClick.AddListener(OnRegisterButtonClick);
            quitButton.onClick.AddListener(Application.Quit);
        }
        private void DeactivateOtherScreensExceptMainMenu()
        {
            registerRef.SetActive(false);
            loginRef.SetActive(false);
            accountRecoveryRef.SetActive(false);
        }
        #endregion

        #region Login Menu Functions

        private void OnLoginMenuBackButtonClick()
        {
            mainMenuRef.SetActive(true);
            loginRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        public void OnLoginMenuForgetPasswordButtonClick()
        {
            loginRef.SetActive(false);
            accountRecoveryRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        private void OnLoginMenuLoginButtonClick()
        {
            if (string.IsNullOrEmpty(loginEmailAddress))
            {
                StartCoroutine(DisplayMessages("Please Enter an Email"));
                return;
            }
            if (string.IsNullOrEmpty(loginPassword))
            {
                StartCoroutine(DisplayMessages("Please Enter a Password"));
                return;
            }

            if (loginEmailAddress.Contains("@") == false)
            {
                StartCoroutine(DisplayMessages("Please Enter a Valid Email Address"));
                return;
            }

            var loginRequest = new LoginWithEmailAddressRequest
            {
                Email = loginEmailAddress,
                Password = loginPassword
            };

            loginMenuLoginButton.interactable = false;
            loginMenuBackButton.interactable = false;
            forgetPasswordButton.gameObject.SetActive(false);

            PlayFabClientAPI.LoginWithEmailAddress(
                loginRequest,
                resultCallback:result =>
                {
                    print(message: "Login was succesfull");
                    StartCoroutine(DisplayMessages("Login was succesfull"));
                    StartCoroutine(Routine_RegistrationSuccesful());
                },
                errorCallback:error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    loginMenuLoginButton.interactable = true;
                    loginMenuBackButton.interactable = true;
                    forgetPasswordButton.gameObject.SetActive(true);
                }
            );

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        public void OnLoginEmailInputValueChanged(string valueChanged)
        {
            loginEmailAddress = valueChanged;
        }
        public void OnLoginPasswordInputValueChanged(string valueChanged)
        {
            loginPassword = valueChanged;
        }
        private void SetUpLoginMenuButtons()
        {
            loginMenuBackButton.onClick.AddListener(OnLoginMenuBackButtonClick);
            loginMenuLoginButton.onClick.AddListener(OnLoginMenuLoginButtonClick);
            forgetPasswordButton.onClick.AddListener(OnLoginMenuForgetPasswordButtonClick);
        }

        #endregion

        #region Register Menu Functions

        private void OnRegisterMenuBackButtonClick()
        {
            mainMenuRef.SetActive(true);
            registerRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void SetupWinLossDataOnRegistration()
        {
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "setupWinLossData"
            };

            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
                resultCallback: result =>
                {
                    // Getting Result From Server
                    var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

                    print(serilizedResult["message"]);

                    StartCoroutine(DisplayMessages("Registration was succesfull"));
                    StartCoroutine(Routine_RegistrationSuccesful());
                },
                errorCallback: error =>
                {
                    print(error.ErrorMessage);
                }
            );
        }

        private void OnRegisterMenuRegisterButtonClick()
        {
            if (string.IsNullOrEmpty(registerUsername))
            {
                StartCoroutine(DisplayMessages("Please Enter a UserName"));
                return;
            }

            if (string.IsNullOrEmpty(registerEmailAddress))
            {
                StartCoroutine(DisplayMessages("Please Enter a Email"));
                return;
            }
            if (string.IsNullOrEmpty(registerPassword))
            {
                StartCoroutine(DisplayMessages("Please Enter a Password"));
                return;
            }

            if(registerPassword.Length < 6)
            {
                StartCoroutine(DisplayMessages("Please Enter a Password with more than six characters"));
                return;
            }

            if (registerUsername.Length < 6)
            {
                StartCoroutine(DisplayMessages("Please Enter a Username with more than six characters"));
                return;
            }

            if (registerEmailAddress.Contains("@") == false)
            {
                StartCoroutine(DisplayMessages("Please Enter a Valid Email Address"));
                return;
            }

            //Buttons stop interacting 
            registerMenuRegisterButton.interactable = false;
            registerMenuBackButton.interactable = false;

            //Request for register
            var registerRequest = new RegisterPlayFabUserRequest
            {
                Username = registerUsername,
                DisplayName = registerUsername,
                Email = registerEmailAddress,
                Password = registerPassword,
            };

            // Sending registration request
            PlayFabClientAPI.RegisterPlayFabUser(
                registerRequest,
                resultCallback: result =>
                {
                    print(message: "Registration result succeeded");
                    SetupWinLossDataOnRegistration();
                    StartCoroutine(DisplayMessages("Registration was succesfull"));
                    StartCoroutine(Routine_RegistrationSuccesful());

                    UpdateContactEmail();
                },
                errorCallback: error =>
                {
                    print(message: "Failed to register");
                    StartCoroutine(DisplayMessages(error.ErrorMessage));

                    registerMenuRegisterButton.interactable = true;
                    registerMenuBackButton.interactable = true;
                }
            );

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        public void OnRegisterEmailInputValueChanged(string valueChanged)
        {
            registerEmailAddress = valueChanged;
        }
        public void OnRegisterPasswordInputValueChanged(string valueChanged)
        {
            registerPassword = valueChanged;
        }
        public void OnRegisterUsernameInputValueChanged(string valueChanged)
        {
            registerUsername = valueChanged;
        }

        private void SetUpRegisterMenuButtons()
        {
            registerMenuBackButton.onClick.AddListener(OnRegisterMenuBackButtonClick);
            registerMenuRegisterButton.onClick.AddListener(OnRegisterMenuRegisterButtonClick);
        }
        private IEnumerator Routine_RegistrationSuccesful()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        #endregion

        #region Account Recovery Menu Functions

        public void OnAccountRecoveryInputValueChanged(string valueChanged)
        {
            accountRecoveryEmailAddress = valueChanged;
        }
        private void OnAccountRecoveryrMenuBackButtonClick()
        {
            mainMenuRef.SetActive(true);
            accountRecoveryRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnAccountRecoveryButtonClick()
        {
            if (string.IsNullOrEmpty(accountRecoveryEmailAddress))
            {
                StartCoroutine(DisplayMessages("Please Enter an Email"));
                return;
            }

            if (accountRecoveryEmailAddress.Contains("@") == false)
            {
                StartCoroutine(DisplayMessages("Please Enter a Valid Email Address"));
                return;
            }
            accountRecoveryButton.interactable = false;
            accountRecoveryMenuBackButton.interactable = false;

            //Sending Account Recovery Email
            var recoveryRequest = new SendAccountRecoveryEmailRequest
            {
                Email = accountRecoveryEmailAddress,
                TitleId = "E4C04"
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(
                recoveryRequest,
                resultCallback: result =>
                {
                    StartCoroutine(DisplayMessages("Email to Reset Password Has Been Sent"));
                    StartCoroutine(Routine_SendRecoveryEmail());
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    accountRecoveryButton.interactable = true;
                    accountRecoveryMenuBackButton.interactable = true;
                }
            );

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void SetUpAccountRecoveryMenuButtons()
        {
            accountRecoveryMenuBackButton.onClick.AddListener(OnAccountRecoveryrMenuBackButtonClick);
            accountRecoveryButton.onClick.AddListener(OnAccountRecoveryButtonClick);
        }

        private IEnumerator Routine_SendRecoveryEmail()
        {
            yield return new WaitForSeconds(2f);
            accountRecoveryButton.interactable = true;
            accountRecoveryMenuBackButton.interactable = true;
            OnAccountRecoveryrMenuBackButtonClick();
        }

        #endregion

        #region Update Contact Email Functions

        private void UpdateContactEmail()
        {
            var updateContactEmailRequest = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = registerEmailAddress,
            };

            PlayFabClientAPI.AddOrUpdateContactEmail(
                updateContactEmailRequest,
                resultCallback: result =>
                {
                    print(message: "Contact email updates successfully");
                },
                errorCallback: error =>
                {
                    print(error.ErrorMessage);
                }
                );
        }

        #endregion

        #region Message Functions
        //Message To show when login or register is failed or succeeds  
        private IEnumerator DisplayMessages(string messageToDisplay)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = messageToDisplay;
            yield return new WaitForSeconds(3f);
            messageText.gameObject.SetActive(false);
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            DeactivateOtherScreensExceptMainMenu();
            SetUpMainMenuButtons();
            SetUpLoginMenuButtons();
            SetUpRegisterMenuButtons();
            SetUpAccountRecoveryMenuButtons();

            //Setting PlayFab ID 
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "E4C04";
            }
        }
        #endregion
    }
}
