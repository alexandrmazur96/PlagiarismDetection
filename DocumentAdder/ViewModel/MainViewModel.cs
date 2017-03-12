﻿using DocumentAdder.Helpers;
using DocumentAdder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using DocumentAdder.Types;
using DocumentAdder.Actions;
using System.Threading;

namespace DocumentAdder.ViewModel
{
    public class MainViewModel
    {
        public MainModel DocumentAdderModel { get; private set; }
        public static MainSettingModel SettingModel { get; set; }

        #region Commands
        //main programm commands
        public ICommand StartProgrammCommand { get; private set; }
        public ICommand StopProgrammCommand { get; private set; }
        public ICommand RestartProgrammCommand { get; private set; }                
        #endregion

        #region Methods
        //main programm methods        

        private void StartProgramm()
        {
            DocumentAdderModel.IsStartBtnEnabled = false;
            DocumentAdderModel.IsStopBtnEnabled = true;
            DocumentAdderModel.IsRestartBtnEnabled = true;
        }

        //settings methods
       

        //other methods
        private int ThreadCount(int fileCount)
        {
            if (fileCount <= 50 && fileCount > 1)
            {
                return 1;
            }
            else if (fileCount > 50 && fileCount <= 100)
            {
                return 2;
            }
            else if (fileCount > 100 && fileCount <= 150)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        #endregion

        static MainViewModel()
        {
            if (SettingModel == null)
            {
                var single = new MainSettingModel();
                SettingModel = single;
            }
        }

        public MainViewModel()
        {
            DocumentAdderModel = new MainModel();
            
            var t = new Types.DataBase.DataBase();
            t.ChooseDatabase("test2"); 
            
        }
    }
}