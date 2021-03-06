﻿using CityApp.DataModel;
using CityApp.Services;
using CityApp.Services.Navigation;
using CityApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace CityApp.Views
{
    public sealed partial class CompanyDetails : Page, IPageWithViewModel<CompanyDetailsViewModel>
    {
        public CompanyDetailsViewModel ViewModel { get; set; }
        public bool CanSubscribe { get; set; }

        public CompanyDetails()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            CanSubscribe = StorageService.UserType == 1 ? true : false;
        }
        public void UpdateBindings()
        {
            Bindings.Update();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var @event = (Event)e.ClickedItem;
            var calendarService = new CalendarService();
            calendarService.CreateAppointmentAsync(@event);
        }
    }
}
