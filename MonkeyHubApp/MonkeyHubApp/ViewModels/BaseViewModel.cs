using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonkeyHubApp.ViewModels
{
    /// <summary>
    /// Propagação das properties nessa classe
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {                
                SetProperty(ref _isBusy, value);
            }
        }

        public BaseViewModel()
        {
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //para poder sobreescrever com o virtual
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage">valor do backend field (os private _)</param>
        /// <param name="value">Valor da property mesmo</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            //se teve alteração no valor propaga, senão não propaga
            //EqualityComparer serve justamente caso algum parâmetro venha null
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// AS navegações serão se uma ViewModel para outra ViewModel
        /// Não usado em produção, somente para testes.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task PushAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);

            var viewModelTypeName = viewModelType.Name;
            var viewModelWordLength = "ViewModel".Length;
            var viewTypeName = $"MonkeyHubApp.{viewModelTypeName.Substring(0, viewModelTypeName.Length - viewModelWordLength)}Page";
            var viewType = Type.GetType(viewTypeName);

            var page = Activator.CreateInstance(viewType) as Page;

            //Activator cria tipos. Cria e define o binding context para não ter que ficar definindo o binding context direto
            var viewModel = Activator.CreateInstance(viewModelType, args);
            if (page != null)
            {
                page.BindingContext = viewModel;
            }

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
