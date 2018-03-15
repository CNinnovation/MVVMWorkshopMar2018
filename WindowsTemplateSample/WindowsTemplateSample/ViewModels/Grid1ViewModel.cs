﻿using System;
using System.Collections.ObjectModel;

using WindowsTemplateSample.Helpers;
using WindowsTemplateSample.Models;
using WindowsTemplateSample.Services;

namespace WindowsTemplateSample.ViewModels
{
    public class Grid1ViewModel : Observable
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
