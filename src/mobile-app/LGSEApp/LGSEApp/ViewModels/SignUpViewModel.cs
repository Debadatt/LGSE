using System;
using System.Windows.Input;
using Xamarin.Forms;
using LGSEApp.Services.Models;
using LGSEApp.ViewModels.Base;
using LGSEApp.Validations;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LGSEApp.Services.Services;
using System.ComponentModel;
using Xamarin.Essentials;

namespace LGSEApp.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        // const string EngineerId = "8FE7DBCB-DCC3-4AC1-803A-5336621C8359"; //Dev Id(Local)
        const string Engineer = "Engineer"; //QA (WWU)

        private ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);

            }
        }

        private ValidatableObject<string> _firstName;
        public ValidatableObject<string> FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }
        private ValidatableObject<string> _lastName;
        public ValidatableObject<string> LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private ValidatableObject<string> _employeeId;
        public ValidatableObject<string> EmployeeId
        {
            get { return _employeeId; }
            set
            {
                _employeeId = value;
                RaisePropertyChanged(() => EmployeeId);
            }
        }

        public ObservableCollection<RoleModel> RoleList { get; set; }
        private RoleModel _Roles;
        public RoleModel Roles
        {
            get
            {
                return _Roles;
            }
            set
            {
                _Roles = value;

                RaisePropertyChanged(() => Roles);
                RoleMessage = null;
                if (_Roles.RoleName == Engineer)
                    eusr = true;
                else
                    eusr = false;

            }
        }

        private ValidatableObject<string> _EUSR;
        public ValidatableObject<string> EUSR
        {
            get { return _EUSR; }
            set
            {
                _EUSR = value;
                RaisePropertyChanged(() => EUSR);
            }
        }
        private string _RoleMessage;
        public string RoleMessage
        {
            get { return _RoleMessage; }
            set
            {
                _RoleMessage = value;
                RaisePropertyChanged(() => RoleMessage);
            }
        }

        private bool _eusr;
        public bool eusr
        {
            get { return _eusr; }
            set
            {
                _eusr = value;
                RaisePropertyChanged(() => eusr);
            }
        }

        private ValidatableObject<string> _contactNo;
        public ValidatableObject<string> ContactNo
        {
            get { return _contactNo; }
            set
            {
                _contactNo = value;
                RaisePropertyChanged(() => ContactNo);
            }
        }
        private bool _IsAccept;
        public bool IsAccept
        {
            get { return _IsAccept; }
            set
            {
                if (_IsAccept != value)
                {
                    _IsAccept = value;
                    RaisePropertyChanged(() => IsAccept);
                    if (IsAccept)
                        Message = "";
                    else
                        Message = ResponceCode.applicationErrorHandler("ACEEPTSTERMS_REQUIRED");
                }
            }
        }
        public Command LoadItemsCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
      
        public ICommand SignInCommand { get; private set; }
        public ICommand ValidateEmailCommand { get; private set; }
        public ICommand ValidateFirstNameCommand { get; private set; }
        public ICommand ValidateLastNameCommand { get; private set; }
        public ICommand ValidateEUSRCommand { get; private set; }
        public ICommand ValidateContactCommand { get; private set; }
        public ICommand IsVisibleEUSRCommand { get; private set; }
        public ICommand IsVisibleRoleMessageCommand { get; private set; }

        public SignUpViewModel()
        {
            _email = new ValidatableObject<string>();
            _firstName = new ValidatableObject<string>();
            _lastName = new ValidatableObject<string>();
            _employeeId = new ValidatableObject<string>();
            _EUSR = new ValidatableObject<string>();
            _contactNo = new ValidatableObject<string>();
            RoleList = new ObservableCollection<RoleModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            SignInCommand = new Command(() => OnSignIn(), () => !IsBusy);
          
            ValidateEmailCommand = new Command(() => _email.Validate());
            ValidateFirstNameCommand = new Command(() => _firstName.Validate());
            ValidateLastNameCommand = new Command(() => _lastName.Validate());
            ValidateEUSRCommand = new Command(() => _EUSR.Validate());
            ValidateContactCommand = new Command(() => _contactNo.Validate());

            AddValidations();
            this.PropertyChanged += SignUpViewModel_PropertyChanged;
        }

        private void SignUpViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SignInCommand).ChangeCanExecute();
                ((Command)SubmitCommand).ChangeCanExecute();

            }
        }
        private bool IsVisibleEUSR()
        {
            bool isVisible = true;
            if (_Roles.RoleName == Engineer)
                isVisible = true;
            else
                isVisible = false;

            return isVisible;
        }



        public override void Validate()
        {
            base.Validate();
            _email.Validate();
            _firstName.Validate();
            _lastName.Validate();
            if (_Roles != null)
            {
                RoleMessage = null;
                if (_Roles.RoleName == Engineer)
                    _EUSR.Validate();
                else
                    _EUSR.IsValid = true;
            }
            else
            {
                RoleMessage = ResponceCode.applicationErrorHandler("ROLE_REQUIRED");
            }
            Message = (IsAccept == false ? ResponceCode.applicationErrorHandler("ACEEPTSTERMS_REQUIRED") : "");
            _contactNo.Validate();
            this.SetValid();
        }

        public override void SetValid()
        {
            IsValid = _email.IsValid && _firstName.IsValid && _lastName.IsValid && _EUSR.IsValid && _contactNo.IsValid && (_Roles == null ? false : true) && IsAccept;
        }
        private bool isValidateRole()
        {
            return (_Roles != null ? true : false);
        }

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_REQUIRED") });
            _email.Validations.Add(new EmailValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_VALID") });
            _firstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("FIRSTNAME_REQUIRED") });
            _firstName.Validations.Add(new NameValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("FIRSTNAME_VALID") });
            _lastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("LASTNAME_REQUIRED") });
            _lastName.Validations.Add(new NameValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("LASTNAME_VALID") });
            _EUSR.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EUSR_REQUIRED") });
            _EUSR.Validations.Add(new EUSRNumericValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EUSR_VALID") });
            _contactNo.Validations.Add(new ContactNumericValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("CONTACT_VALID") });

        }





        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                RoleService role = new RoleService();
                RoleList.Clear();
                var items = await role.GetSignUpRole();
                foreach (var item in items)
                {
                    if (item.RoleName == "Isolator" || item.RoleName == "Engineer")
                        RoleList.Add(new RoleModel() { Id = item.Id, RoleName = item.RoleName });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                // IsBusy = false;
            }
        }
        public void OnSubmit()
        {
            Validate();
            if (IsValid)
            {
                networkAccess = Connectivity.NetworkAccess;
                if (networkAccess == NetworkAccess.Internet)
                {
                    IsBusy = true;
                    this.OnSubmitExcute();
                }
                else
                {
                    Message = ResponceCode.customErrorFunction(651, null);
                }
            }

        }

        private async void OnSubmitExcute()
        {
            try
            {
                AccountService userService = new AccountService();
                UserModel userModel = new UserModel();
                userModel.Email = _email.Value;
                userModel.FirstName = _firstName.Value;
                userModel.LastName = _lastName.Value;
                userModel.EmployeeId = _employeeId.Value;
                userModel.RoleId = _Roles.Id;
                userModel.EUSR = _EUSR.Value;
                userModel.ContactNo = _contactNo.Value;
                var customeMessage = await userService.PostUser(userModel);
                if (customeMessage.message == "USER_REGISTERED")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new UserActivePage(_email.Value));
                }
                else
                {
                    Message = customeMessage.message;
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }




}
