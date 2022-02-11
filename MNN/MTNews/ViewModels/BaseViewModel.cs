using Acr.UserDialogs;
using MTNews.Models;
using MTNews.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MTNews
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        private string pageName;
        public string PageName
        {
            get { return pageName; }
            set { SetProperty(ref pageName, value); }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }
        private string role;
        public string Role
        {
            get { return role; }
            set { SetProperty(ref role, value); }
        }
        private int memberId;
        public int MemberId
        {
            get { return memberId; }
            set { SetProperty(ref memberId, value); }
        }
        private bool isSearch = false;
        public bool IsSearch
        {
            get { return isSearch; }
            set { SetProperty(ref isSearch, value); }
        }
        private bool isMenu = false;
        public bool IsMenu
        {
            get { return isMenu; }
            set { SetProperty(ref isMenu, value); }
        }
        private bool isSearchedText = false;
        public bool IsSearchedText
        {
            get { return isSearchedText; }
            set { SetProperty(ref isSearchedText, value); }
        }
        private string userProfilePic;
        public string UserProfilePic
        {
            get { return userProfilePic; }
            set { SetProperty(ref userProfilePic, value); }
        }
        private string shortUsername;
        public string ShortUsername
        {
            get { return shortUsername; }
            set { SetProperty(ref shortUsername, value); }
        }
        #endregion

        #region Commands
        public Command LogoutCommand { get; }
        public Command MenuCommand { get; }
        public Command HideCommand { get; }
        #endregion

        #region Constructor
        public BaseViewModel()
        {
            try
            {
                LogoutCommand = new Command(OnLogout);
                MenuCommand = new Command(OnMenuClicked);
                HideCommand = new Command(OnHideClicked);

                BindUserDetails();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion

        #region Methods
        private async void OnLogout(object obj)
        {
            try
            {
                var res = await UserDialogs.Instance.ConfirmAsync("Are you sure you want to logout?", "Logout", "Yes", "No");
                if (res)
                {
                    Preferences.Set(Constants.IsLoggedIn, false);
                    Preferences.Set(Constants.MemberID, string.Empty);
                    Preferences.Set(Constants.UserJson, string.Empty);

                    App.Current.MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void OnMenuClicked(object obj)
        {
            try
            {
                if (App.RootPage != null)
                {
                    App.MenuIsPresented = true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        private void OnHideClicked(object obj)
        {
            try
            {
                if (App.RootPage != null)
                {
                    App.MenuIsPresented = false;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void BindUserDetails()
        {
            try
            {
                var local = Preferences.Get(Constants.UserJson, string.Empty);

                if (!string.IsNullOrEmpty(local))
                {
                    var user = JsonConvert.DeserializeObject<LoginResponseData>(local);
                    if (user != null)
                    {
                        MemberId = user.MemberID;
                        Username = user.FirstName + " " + user.LastName;
                        Role = "Software Developer";
                        UserProfilePic = "";

                        ShortUsername = Utilities.GetShortName(Username);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion
    }
}