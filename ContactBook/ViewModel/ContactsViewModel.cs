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
        private List<Contact> _contacts;
        public ObservableCollection<Contact> Contacts { get; set; }

        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BrowseImageCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private IContactDataService _dataService;
        private IDialogService _dialogService;
        public ContactsViewModel(IContactDataService dataService, IDialogService dialogService) {

            _dataService = dataService;
            _dialogService = dialogService;
            _contacts = dataService.GetContacts().ToList();
                
            EditCommand = new RelayCommand(Edit, CanEdit);
            SaveCommand = new RelayCommand(Save, IsEdit);
            UpdateCommand = new RelayCommand(Update);
            BrowseImageCommand = new RelayCommand(BrowseImage, CanEdit);
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

        }


        private void Delete() {
            Contacts.Remove(SelectedContact);
            _contacts.Remove(SelectedContact);
            Save();
        }
        private bool CanDelete() {
            return SelectedContact == null ? false : true;
        }

        private void Add() {
            var newContact = new Contact {
                Name = "N/A",
                PhoneNumbers = new string[2],
                Emails= new string[2],
                Locations= new string[2]
            };
            Contacts.Add(newContact);
            _contacts.Add(newContact);
            SelectedContact= newContact;
        }

        private void BrowseImage() {
            var filePath = _dialogService.OpenFile("Image files|*.bmp;*.jpg;*.jpeg;*.png|All files");
            SelectedContact.ImagePath = filePath;
        }
        private void Update() {
            _dataService.Save(_contacts);
        }

        private void Save() {
            _dataService.Save(_contacts);
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
