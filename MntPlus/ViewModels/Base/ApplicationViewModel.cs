﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application 
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.ManageWork;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        private bool _sideMenuVisible { get; set; } = false;
        public bool SideMenuVisible 
        { 
            get => _sideMenuVisible;
            set 
            { 
                _sideMenuVisible = value; 
                if (value == false) 
                    SideBarStatus = "Afficher le Menu"; 
                else 
                    SideBarStatus = "Masquer le Menu";
                OnPropertyChanged(nameof(SideMenuVisible)); 
            } 
        }

       public ICommand SideMenuCommand { get; set; }
        public string SideBarStatus { get; set; } = "Masquer le Menu";

        public ApplicationViewModel()
        {
            SideMenuCommand = new RelayCommand(() => SideMenuVisible = !SideMenuVisible);
        }
        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
           
            // Set the view model
            CurrentPageViewModel = viewModel;

            

            // Set the current page
            CurrentPage = page;


            OnPropertyChanged(nameof(CurrentPage));


            // Show side menu or not?
            SideMenuVisible = true; /*page != ApplicationPage.Login*/ ;

        }

      
    }
}
