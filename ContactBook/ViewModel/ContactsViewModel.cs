using ContactBook.Model;
using ContactBook.Services;
using ContactBook.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactBook.ViewModel
{
    public class ContactsViewModel : ObservableObject
    {
        private Contact _selectedContact;
        public Contact SelectedContact {
            get { return _selectedContact; }
            set { OnPropertyChanged(ref _selectedContact, value); }
        }

        private bool _isEditMode;
        public bool IsEditMode {
            get { return _isEditMode; }
            set {
                OnPropertyChanged(ref _isEditMode, value);
                OnPropertyChanged("IsDisplayMode");
            }
        }
        public bool IsDisplayMode {
            get { return !_isEditMode; }
        }

        public ObservableCollection<Contact> Contacts { get; set; }

        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        private IContactDataService _dataService;

        public ContactsViewModel(IContactDataService dataService) {
            _dataService = dataService;
                
            EditCommand = new RelayCommand(Edit, CanEdit);
            SaveCommand = new RelayCommand(Save, IsEdit);
            UpdateCommand = new RelayCommand(Update);

        }
        private void Update() {
            _dataService.Save(Contacts);
        }

        private void Save() {
            _dataService.Save(Contacts);
            IsEditMode = false;
            OnPropertyChanged("SelectedContact");
        }
        private bool IsEdit()
        {
            return IsEditMode;
        }
        private bool CanEdit() {
            if (SelectedContact == null)
                return false;

            return !IsEditMode;
        }
        private void Edit()
        {
            IsEditMode = true;
        }

        public void LoadContacts(IEnumerable<Contact> contacts) {
            Contacts = new ObservableCollection<Contact>(contacts);
            OnPropertyChanged("Contacts");
        }

    }
}
