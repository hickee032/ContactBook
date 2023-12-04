using ContactBook.Services;
using ContactBook.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactBook.ViewModel
{
    public class BookViewModel : ObservableObject
    {
        private IContactDataService _dataService;
        private IDialogService _dialogService;

        private ContactsViewModel _contactsVM;
        public ContactsViewModel ContactsVM {
            get { return _contactsVM; }
            set { OnPropertyChanged(ref _contactsVM, value); }
        }

        public ICommand LoadFavoritesCommand { get; private set; }
        public ICommand LoadContactsCommand { get; private set; }

        public BookViewModel(IContactDataService dataService, IDialogService dialogService) {
            _dialogService = dialogService;
            _dataService = dataService;

            ContactsVM = new ContactsViewModel(dataService, dialogService);
            LoadContacts();

            LoadFavoritesCommand = new RelayCommand(LoadFavorites);
            LoadContactsCommand = new RelayCommand(LoadContacts);
        }

        private void LoadContacts() {
            ContactsVM.LoadContacts(_dataService.GetContacts());
        }

        private void LoadFavorites() {
            var favorites = _dataService.GetContacts().Where(c => c.IsFavorite);
            ContactsVM.LoadContacts(favorites);
        }
    }
}
