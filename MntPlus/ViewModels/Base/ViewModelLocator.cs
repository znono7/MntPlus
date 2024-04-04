﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus
{
    public class ViewModelLocator
    {
        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoContainer.Application;
      //  public static StockHostViewModel StockHostViewModel => IoC.Stocks;

        /// <summary>
        /// The settings view model
        /// </summary>
       // public static SettingsViewModel SettingsViewModel => IoC.Settings;

        #endregion
    }
}
