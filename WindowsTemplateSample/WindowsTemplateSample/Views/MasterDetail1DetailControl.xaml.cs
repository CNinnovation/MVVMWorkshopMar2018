﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WindowsTemplateSample.Models;

namespace WindowsTemplateSample.Views
{
    public sealed partial class MasterDetail1DetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(MasterDetail1DetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public MasterDetail1DetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MasterDetail1DetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
