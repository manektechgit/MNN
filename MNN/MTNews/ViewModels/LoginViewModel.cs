using Acr.UserDialogs;
using MTNews.Models;
using MTNews.Services;
using MTNews.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MTNews
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        IBackendConnection loginService = DependencyService.Get<IBackendConnection>();

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        private bool isPassword = true;
        public bool IsPassword
        {
            get { return isPassword; }
            set { SetProperty(ref isPassword, value); }
        }
        #endregion

        #region Commands
        public Command IsPasswordCommand { get; }
        public Command LoginCommand { get; }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            try
            {
                IsPasswordCommand = new Command(OnIsPasswordClicked);
                LoginCommand = new Command(OnLoginClicked);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion

        #region Methods
        private void OnIsPasswordClicked(object obj)
        {
            try
            {
                IsPassword = !IsPassword;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    UserDialogs.Instance.Alert("Please enter email");
                }
                else if (!Constants.CheckValidEmail.IsMatch($"{Email}"))
                {
                    UserDialogs.Instance.Alert("Please enter vaild email");
                }
                else if (string.IsNullOrEmpty(Password))
                {
                    UserDialogs.Instance.Alert("Please enter password");
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    LoginRequest request = new LoginRequest();
                    request.userName = Email;
                    request.password = Password;

                    var res = await loginService.Login(request);
                    if (res != null)
                    {
                        UserDialogs.Instance.HideLoading();

                        if (res.Data != null && res.StatusCode == Constants.StatusCode)
                        {
                            Preferences.Set(Constants.IsLoggedIn, true);
                            Preferences.Set(Constants.MemberID, res.Data.MemberID);
                            Preferences.Set(Constants.UserJson, JsonConvert.SerializeObject(res.Data));

                            var ress = await loginService.UpdateUser(res.Data);

                            App.CallMainPage();
                        }
                        else
                        {
                            UserDialogs.Instance.Toast(res.Message);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Toast(Constants.ServerError);
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                Logger.SendErrorLog(ex);
            }
        }
        #endregion
    }
}