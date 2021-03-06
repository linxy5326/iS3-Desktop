﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS3.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class IS3MainWindow : Window
    {
        public IS3MainWindow()
        {
            InitializeComponent();

            Closing += MainWindow_Closing;

            //UserPreferences userPrefs = new UserPreferences();
            //this.Height = userPrefs.WindowHeight;
            //this.Width = userPrefs.WindowWidth;
            //this.Top = userPrefs.WindowTop;
            //this.Left = userPrefs.WindowLeft;
            //this.WindowState = userPrefs.WindowState;

            this.Height = 750;
            this.Width = 1200;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            UserLoginPage userLoginPage = new UserLoginPage();
            pageTransitionControl.ShowPage(userLoginPage);

            //ProjectListPage projectListPage = new ProjectListPage();
            //pageTransitionControl.ShowPage(projectListPage);

            //SwitchToMainFrame("NJ_MetroL10.xml");
        }

        void MainWindow_Closing(object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            UserPreferences userPrefs = new UserPreferences();

            userPrefs.WindowHeight = this.Height;
            userPrefs.WindowWidth = this.Width;
            userPrefs.WindowTop = this.Top;
            userPrefs.WindowLeft = this.Left;
            userPrefs.WindowState = this.WindowState;

            userPrefs.Save();
        }

        public void SwitchToMainFrame(string projectID)
        {
            pageTransitionControl.TransitionType =
                WpfPageTransitions.PageTransitionType.SlideLeft;
            MainFrame mainFrame = new MainFrame(projectID);
            pageTransitionControl.ShowPage(mainFrame);

            App app = App.Current as App;
            app.MainFrame = mainFrame;
        }

        public void SwitchToProjectListPage()
        {
            ProjectListPage projectListPage = new ProjectListPage();
            pageTransitionControl.ShowPage(projectListPage);
        }

    }
}
