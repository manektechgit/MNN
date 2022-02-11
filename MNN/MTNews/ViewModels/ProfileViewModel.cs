using MTNews.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MTNews
{
    public class ProfileViewModel : BaseViewModel
    {
        #region Properties

        private bool isEdit = false;
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                SetProperty(ref isEdit, value);

                if (value)
                {
                    EditSource = "save";
                    CancelSource = "cancel";
                    PageName = "Edit Profile";
                }
                else
                {
                    EditSource = "edit";
                    CancelSource = "slider";
                    PageName = "My Profile";
                }
            }
        }
        private string editSource = "edit";
        public string EditSource
        {
            get { return editSource; }
            set { SetProperty(ref editSource, value); }
        }
        private string cancelSource = "slider";
        public string CancelSource
        {
            get { return cancelSource; }
            set { SetProperty(ref cancelSource, value); }
        }
        private string gender = "Male";
        public string Gender
        {
            get { return gender; }
            set { SetProperty(ref gender, value); }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        private string birthdate;
        public string Birthdate
        {
            get { return birthdate; }
            set { SetProperty(ref birthdate, value); }
        }
        private string mobileNo;
        public string MobileNo
        {
            get { return mobileNo; }
            set { SetProperty(ref mobileNo, value); }
        }
        #endregion

        #region Commands
        public Command EditAboutCommand { get; }
        public Command EditCommand { get; }
        public Command CancelCommand { get; }

        #endregion

        #region Constructor
        public ProfileViewModel()
        {
            try
            {
                PageName = "My Profile";

                EditAboutCommand = new Command(EditAboutClicked);
                EditCommand = new Command(EditClicked);
                CancelCommand = new Command(CancelClicked);

                BindLocalDetails();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion

        #region Methods
        public void EditAboutClicked(object obj)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void EditClicked(object obj)
        {
            try
            {
                IsEdit = !IsEdit;

                if(IsEdit)
                {
                    // Save user deatils Service API calling and then save data to local and display from local storage
                    BindLocalDetails(); //this method call after Save API response.
                }
                else
                {
                    BindLocalDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void CancelClicked(object obj)
        {
            try 
            {
                if(IsEdit)
                {
                    IsEdit = false;
                    BindLocalDetails();
                }
                else
                {
                    OnMenuClicked(obj);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void BindLocalDetails()
        {
            try
            {
                var local = Preferences.Get(Constants.UserJson, string.Empty);

                if (!string.IsNullOrEmpty(local))
                {
                    var user = JsonConvert.DeserializeObject<LoginResponseData>(local);
                    if (user != null)
                    {
                        Username = user.FirstName + " " + user.LastName;
                        Role = "Software Developer";

                        Birthdate = user.BirthDate.ToLocalTime().ToString("dd MMM, yyyy");
                        Email = user.UserName;
                        MobileNo = user.MobileNo;

                        if (!string.IsNullOrEmpty(user.Gender))
                        {
                            if (user.Gender.ToLower() == "m")
                                Gender = "Male";
                            else
                                Gender = "Female";
                        }
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