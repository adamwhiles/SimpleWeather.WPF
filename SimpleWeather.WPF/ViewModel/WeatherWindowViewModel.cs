using SimpleWeather.WPF.Model;
using SimpleWeather.WPF.ViewModel.Commands;
using SimpleWeather.WPF.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWeather.WPF.ViewModel
{
    public class WeatherWindowViewModel : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                OnPropertyChanged("Query");
            }
        }

        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<DailyForecast> Forecasts { get; set; }

        private Forecast forecast;

        public Forecast Forecast
        {
            get { return forecast; }
            set 
            { 
                forecast = value;
                OnPropertyChanged("Forecast");
            }
        }


        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            { 
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public  City SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                selectedCity = value;
                if (selectedCity != null)
                {
                    OnPropertyChanged("SelectedCity");
                    GetCurrentConditions();
                    GetForecast();
                }

            }
        }

        public SearchCommand SearchCommand { get; set; }

        public WeatherWindowViewModel()
        {

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
            Forecasts = new ObservableCollection<DailyForecast>();
        }

        private async void GetForecast()
        {
            Query = String.Empty;
            Forecast = await AccuWeatherHelper.GetForecast(SelectedCity.Key);
            Forecasts.Clear();
            foreach (DailyForecast forecast in Forecast.DailyForecasts)
            {
                Forecasts.Add(forecast);
            }
        }

        private async void GetCurrentConditions()
        {
            Query = String.Empty;
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
            Cities.Clear();

        }
        public async void MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear();
            foreach (City city in cities)
            {
                Cities.Add(city);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
