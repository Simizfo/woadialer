﻿using Dialer.Systems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Calls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Dialer.UI.Controls
{
    public sealed partial class ContactControl : UserControl
    {
        private CallSystem CallSystem => App.Current.CallSystem;
        private DisplayableLine CurrentPhoneLine;
        private ObservableCollection<DisplayableLine> DisplayableLines = new ObservableCollection<DisplayableLine>(App.Current.CallSystem.Lines.Select(x => new DisplayableLine(x)));

        public string ContactName
        {
            get => ContactNameTB.Text;
            set => ContactNameTB.Text = value;
        }

        public string ContactMainPhone
        {
            get => ContactMainPhoneTB.Text;
            set => ContactMainPhoneTB.Text = value;
        }

        public IRandomAccessStreamReference ContactPicture
        {
            set => ContactImage.Source = new BitmapImage(new Uri(((StorageFile) value).Path));
        }

        public ContactControl()
        {
            InitializeComponent();

            //CallSystem.Lines.CollectionChanged += Lines_CollectionChanged;

            if (CallSystem.DefaultLine != null && DisplayableLines.Any(x => x.Line.Id == CallSystem.DefaultLine.Id))
                CurrentPhoneLine = DisplayableLines.First(x => x.Line.Id == CallSystem.DefaultLine.Id);
            //PhoneLineSelector.SelectedItem = CurrentPhoneLine;
        }

        private void ToggleMoreDataButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MoreDataPanel.Visibility == Visibility.Collapsed)
            {
                ToggleMoreDataButtonIcon.Glyph = "\uE010";
                MoreDataPanel.Visibility = Visibility.Visible;
                //MoreDataPanel.Margin = new Thickness(0, 80, 0, 0);
                HeightAnimationOpen.To = 80 + MoreDataPanel.ActualHeight;
                ShowPaneAnimation.Begin();
                PrimaryPanel.CornerRadius = new CornerRadius(4, 4, 0, 0);
            } else
            {
                ToggleMoreDataButtonIcon.Glyph = "\uE011";
                //MoreDataPanel.Margin = new Thickness(0, 0, 0, 0);
                HeightAnimationClose.From = 80 + MoreDataPanel.ActualHeight;
                HidePaneAnimation.Completed += (object a, object b) => MoreDataPanel.Visibility = Visibility.Collapsed;
                HidePaneAnimation.Begin();
                PrimaryPanel.CornerRadius = new CornerRadius(4, 4, 4, 4);
            }
        }

        private void MainCallButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //TODO: Check for missing phone lines. If no phone lines, show an alert
            try
            {
                CurrentPhoneLine?.Line?.DialWithOptions(new PhoneDialOptions() { Number = ContactMainPhone.ToString() });
            }
            catch { }
        }
    }
}