using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window
    {
        private DispatcherTimer timer;
        #region Private Members

        /// <summary>
        /// The view model for this window
        /// </summary>
        private NotificationWindowViewModel mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model for this window
        /// </summary>
        public NotificationWindowViewModel ViewModel
        {
            get => mViewModel;
            set
            {
                // Set new value
                mViewModel = value;

                // Update data context
                DataContext = mViewModel;
            }
        }

        #endregion  
        public NotificationsWindow()
        {
            InitializeComponent();
            

        }

        public void SetTopCenter()
        {
            // Get the screen's working area
        
            var workingArea = SystemParameters.WorkArea;

            // Calculate the horizontal center
            double centerX = (workingArea.Width - ActualWidth) / 2;

            
            // Set the window's position
            Left = centerX;
            Top = -ActualHeight ;

            // Position window above the screen
            //this.Left = SystemParameters.WorkArea.Width / 2 - this.Width / 2;
            //this.Top = 0;

            // Start timer to close the window after 3 seconds
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SetTopCenter();
            AddSlideFromTop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            // Add fade-out animation
            var fadeOutStoryboard = new Storyboard();
            fadeOutStoryboard.AddFadeOut(0.5f);
            fadeOutStoryboard.Completed += (s, _) => Close();
            fadeOutStoryboard.Begin(this);
        }
        private void AddSlideFromTop()
        {
            // Get the storyboard from resources
            Storyboard storyboard = (Storyboard)FindResource("SlideInFromTopAnimation");

            // Apply the animation to the window
            BeginStoryboard(storyboard);
        }
       
    }
   
}
