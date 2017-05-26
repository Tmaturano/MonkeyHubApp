using MonkeyHubApp.Models;
using MonkeyHubApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonkeyHubApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        /*
        private string _searchTerm;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (SetProperty(ref _searchTerm, value))
                    SearchCommand.ChangeCanExecute(); //Importante pois assim invoca o CanExecuteSearchCommand
            }
        } */

        public ObservableCollection<Tag> Tags { get; }

        //Sem Set, para ter somente uma instância
        public Command SearchCommand { get; }

        public Command AboutCommand { get; }

        //Esse comando aceita um parâmetro do tipo Tag
        public Command<Tag> ShowCategoriaCommand { get; }

        private readonly IMonkeyHubApiService _monkeyHubApiService;
        public MainViewModel(IMonkeyHubApiService monkeyHubApiService)
        {
            SearchCommand = new Command(ExecuteSearchCommand);
            AboutCommand = new Command(ExecuteAboutCommand);
            ShowCategoriaCommand = new Command<Tag>(ExecuteShowCategoriaCommand);

            Tags = new ObservableCollection<Tag>();

            _monkeyHubApiService = monkeyHubApiService;
        }

        async void ExecuteShowCategoriaCommand(Tag tag)
        {
            //O PushAsync funciona porque as viewmodels herdam de baseviewmodel que tem um PushAsync que faz esse papel
            //de acordo com o tipo informado, nesse caso, CategoriaViewModel
            await PushAsync<CategoriaViewModel>(_monkeyHubApiService, tag);
        }

        async void ExecuteSearchCommand()
        {
            await PushAsync<SearchViewModel>(_monkeyHubApiService);

            /*bool result = await App.Current.MainPage.DisplayAlert("MonkeyHubApp", 
                        $"Você pesquisou por '{SearchTerm}'.", "Sim" , "Não");

        if (result)
        {
            await App.Current.MainPage.DisplayAlert("MonkeyHubApp",
                  "Obrigado", "Ok");


            var tags = await _monkeyHubApiService.GetTagsAsync();

            Resultados.Clear();

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    Resultados.Add(tag);
                }
            }

        }
        else
        {
            await App.Current.MainPage.DisplayAlert("MonkeyHubApp",
                   "De nada", "Ok");

        } */
        }

        async void ExecuteAboutCommand()
        {
            await PushAsync<AboutViewModel>();
        }

        //Pode colocar regra para habilitar / desabilitar o botão
        //Executado somente uma vez (ao construir)
        //Não deve alterar nenhum valor, para não ter risco de entrar em loop infinito
        /*bool CanExecuteSearchCommand()
        {
            return !string.IsNullOrWhiteSpace(SearchTerm);
        } */

        public async Task LoadAsync()
        {
            IsBusy = true;

            var tags = await _monkeyHubApiService.GetTagsAsync();

            Tags.Clear();
            foreach (var content in tags)
            {
                Tags.Add(content);
            }

            IsBusy = false;
        }

        /*
        public MainViewModel()
        {
             Teste
            Descricao = "Olá mundo, eu estou aqui!";

            //Após os 3 segundos, vai executar
            Task.Delay(3000).ContinueWith(async t =>
            {                
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    Descricao = $"Meu texto mudou {i}";
                }                      
            }); 
    } */

        /*
         Na navegação modal, PushModalAsync, a barra superior de navegação some e não tem um botão de voltar
         Podemos misturar navegações modais com não modais.
         No IOS, não existe o botão de back, então para este caso temos que fazer um PopModalAsync em um botão
         Tem que tomar cuidado para não chamar o PopModalAsync sem antes de verificar a pilha de navegação, senão
         crasha o app.
         
         */

    }
}
