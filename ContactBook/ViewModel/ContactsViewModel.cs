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
            set {
                OnPropertyChanged(ref _selectedContact, value);
            }
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
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand BrowseImageCommand { get; private set; }
        public ICommand FavoriteCommand { get; private set; }

        private IContactDataService _dataService;
        private IDialogService _dialogService;

        public ContactsViewModel(IContactDataService dataService, IDialogService dialogService) {
            _dataService = dataService;
            _dialogService = dialogService;

            BrowseImageCommand = new RelayCommand(BrowseImage, IsEdit);
            SaveCommand = new RelayCommand(Save, IsEdit);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FavoriteCommand = new RelayCommand(Favorite);
        }

        private void Favorite() {
            SelectedContact.IsFavorite = !SelectedContact.IsFavorite;
            Save();
        }

        private void BrowseImage() {
            var filePath = _dialogService.OpenFile(string.Empty);
            SelectedContact.ImagePath = filePath;
        }

        private void Save() {
            _dataService.Save(Contacts);
            IsEditMode = false;
            OnPropertyChanged("SelectedContact");
        }

        private void Add() {
            var newContact = new Contact() {
                Name = "N/A",
                PhoneNumbers = new string[2],
                Emails = new string[2],
                Locations = new string[2]
            };
            Contacts.Add(newContact);
            SelectedContact = newContact;
        }

        private bool IsEdit() {
            return IsEditMode;
        }

        private void Delete() {
            Contacts.Remove(SelectedContact);
        }

        private bool CanDelete() {
            return SelectedContact == null ? false : true;
        }

        private void Edit() {
            IsEditMode = true;
        }

        private bool CanEdit() {
            if (SelectedContact == null)
                return false;

            return !IsEditMode;
        }

        public void LoadContacts(IEnumerable<Contact> contacts) {
            Contacts = new ObservableCollection<Contact>(contacts);
            OnPropertyChanged("Contacts");
        }

    }
}
