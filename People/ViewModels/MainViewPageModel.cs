using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels
{
    public class MainPageViewModels : BindableObject
    {
        private string _newPerson;
        private string _statusMessage;

        public string NewPerson
        {
            get => _newPerson;
            set
            {
                _newPerson = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }

        public MainPageViewModels()
        {
            AddPersonCommand = new Command(async () => await AddPerson());
            GetPeopleCommand = new Command(async () => await GetPeople());
        }

        private async Task AddPerson()
        {
            if (!string.IsNullOrWhiteSpace(NewPerson))
            {
                StatusMessage = "";
                await App.PersonRepo.AddNewPerson(NewPerson);
                StatusMessage = App.PersonRepo.StatusMessage;
            }
            else
            {
                StatusMessage = "Ingresar un nombre válido ";
            }
        }

        private async Task GetPeople()
        {
            StatusMessage = "";
            var people = await App.PersonRepo.GetAllPeople();
            People.Clear();

            foreach (var person in people)
            {
                People.Add(person);
            }
        }
    }
}