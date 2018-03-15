﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WindowsTemplateSample.Helpers;
using WindowsTemplateSample.Services;
using WindowsTemplateSample.Views;

namespace WindowsTemplateSample.ViewModels
{
    public class ShellViewModel : Observable
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;

        private bool _isPaneOpen;

        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private object _selected;

        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;

        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }

        private object _lastSelectedItem;

        private ObservableCollection<ShellNavigationItem> _primaryItems = new ObservableCollection<ShellNavigationItem>();

        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get { return _primaryItems; }
            set { Set(ref _primaryItems, value); }
        }

        private ObservableCollection<ShellNavigationItem> _secondaryItems = new ObservableCollection<ShellNavigationItem>();

        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get { return _secondaryItems; }
            set { Set(ref _secondaryItems, value); }
        }

        private ICommand _openPaneCommand;

        public ICommand OpenPaneCommand
        {
            get
            {
                if (_openPaneCommand == null)
                {
                    _openPaneCommand = new RelayCommand(() => IsPaneOpen = !_isPaneOpen);
                }

                return _openPaneCommand;
            }
        }

        private ICommand _itemSelected;

        public ICommand ItemSelectedCommand
        {
            get
            {
                if (_itemSelected == null)
                {
                    _itemSelected = new RelayCommand<HamburgetMenuItemInvokedEventArgs>(ItemSelected);
                }

                return _itemSelected;
            }
        }

        private ICommand _stateChangedCommand;

        public ICommand StateChangedCommand
        {
            get
            {
                if (_stateChangedCommand == null)
                {
                    _stateChangedCommand = new RelayCommand<Windows.UI.Xaml.VisualStateChangedEventArgs>(args => GoToState(args.NewState.Name));
                }

                return _stateChangedCommand;
            }
        }

        private void InitializeState(double windowWith)
        {
            if (windowWith < WideStateMinWindowWidth)
            {
                GoToState(NarrowStateName);
            }
            else if (windowWith < PanoramicStateMinWindowWidth)
            {
                GoToState(WideStateName);
            }
            else
            {
                GoToState(PanoramicStateName);
            }
        }

        private void GoToState(string stateName)
        {
            switch (stateName)
            {
                case PanoramicStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    break;
                case WideStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    IsPaneOpen = false;
                    break;
                case NarrowStateName:
                    DisplayMode = SplitViewDisplayMode.Overlay;
                    IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }

        public void Initialize(Frame frame)
        {
            NavigationService.Frame = frame;
            NavigationService.Navigated += Frame_Navigated;
            PopulateNavItems();

            InitializeState(Window.Current.Bounds.Width);
        }

        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            // TODO WTS: Change the symbols for each item as appropriate for your app
            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            _primaryItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<WebView1Page>("Shell_WebView1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<MediaPlayer1Page>("Shell_MediaPlayer1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<MasterDetail1Page>("Shell_MasterDetail1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Grid1Page>("Shell_Grid1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Chart1Page>("Shell_Chart1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Tabbed1Page>("Shell_Tabbed1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Map1Page>("Shell_Map1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Camera1Page>("Shell_Camera1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<ImageGallery1Page>("Shell_ImageGallery1".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<Tabbed2Page>("Shell_Tabbed2".GetLocalized(), Symbol.Document));
            _secondaryItems.Add(ShellNavigationItem.FromType<Settings1Page>("Shell_Settings1".GetLocalized(), Symbol.Setting));
        }

        private void ItemSelected(HamburgetMenuItemInvokedEventArgs args)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }

            Navigate(args.InvokedItem);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var navigationItem = PrimaryItems?.FirstOrDefault(i => i.PageType == e?.SourcePageType);
            if (navigationItem == null)
            {
                navigationItem = SecondaryItems?.FirstOrDefault(i => i.PageType == e?.SourcePageType);
            }

            if (navigationItem != null)
            {
                ChangeSelected(_lastSelectedItem, navigationItem);
                _lastSelectedItem = navigationItem;
            }
        }

        private void ChangeSelected(object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as ShellNavigationItem).IsSelected = false;
            }

            if (newValue != null)
            {
                (newValue as ShellNavigationItem).IsSelected = true;
                Selected = newValue;
            }
        }

        private void Navigate(object item)
        {
            var navigationItem = item as ShellNavigationItem;
            if (navigationItem != null)
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }
    }
}
