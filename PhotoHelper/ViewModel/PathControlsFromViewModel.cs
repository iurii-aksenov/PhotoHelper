﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using PhotoHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PhotoHelper.ViewModel
{
    public class PathControlsFromViewModel:DependencyObject
    {
        public RenameInterfaceViewModel RenameInterfaceViewModel { get; set; }

        public ICommand OpenFolderDialogCommand { get; set; }
               
        public PathControlsFromViewModel(RenameInterfaceViewModel renameInterfaceViewModel)
        {
            RenameInterfaceViewModel = renameInterfaceViewModel;
            OpenFolderDialogCommand = new RelayCommand(this.OpenFolderDialog);
        }


        public ForCollectionItems SelectedFile
        {
            get { return (Model.ForCollectionItems)GetValue(SelectedFileProperty); }
            set { SetValue(SelectedFileProperty, value); }
        }

        public static readonly DependencyProperty SelectedFileProperty =
            DependencyProperty.Register("SelectedFile", typeof(Model.ForCollectionItems), typeof(PathControlsFromViewModel), new PropertyMetadata(null, SelectedFile_Changed));

        private static void SelectedFile_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var t = d as PathControlsFromViewModel;
            if (t != null &&  t.SelectedFile!=null)
            {
                //t.RenameInterfaceViewModel.FileInfoComponents = null;
                FileInfoComponents fileInfoComponents = new FileInfoComponents();
                fileInfoComponents.Parsing(t.SelectedFile.FileName);
                t.RenameInterfaceViewModel.FileInfoComponents = fileInfoComponents;

                t.RenameInterfaceViewModel.MessageNotice = "Выбран новый файл " + t.SelectedFile.FileOnlyId;
                
                //SelectedItem.ItemPath = Path.Combine(t.SelectedFile.oldPath,t.SelectedFile.fileName);
                //MessageBox.Show(SelectedItem.ItemPath);

                //MessageBox.Show(string.IsNullOrWhiteSpace(t.SelectedFile.PathFrom) ? "Не выбран" : t.SelectedFile.PathFrom + "  " + (string.IsNullOrWhiteSpace(t.SelectedFile.FileName)? "Не выбран" : t.SelectedFile.FileName));
            }
        }

       
        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set { SetValue(FolderPathProperty, value); }
        }

        public static readonly DependencyProperty FolderPathProperty =
            DependencyProperty.Register("FolderPath", typeof(string), typeof(PathControlsFromViewModel), new PropertyMetadata("", FolderPath_Changed));

        private static void FolderPath_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as PathControlsFromViewModel;
            if (current != null)
            {
                if (Directory.Exists(current.FolderPath))
                {
                    current.Items = null;
                    current.Items = CollectionViewSource.GetDefaultView(ForCollectionItems.GetItems(current.FolderPath,true));
                }
            }
        }

        public void OpenFolderDialog()
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (result.ToString() != "Cancel" && result.ToString() != null)
                {
                    FolderPath = dialog.FileName;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Возникла ошибка в выборе папки. " + e.Message);
            }
        }


        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(PathControlsFromViewModel), new PropertyMetadata(null));

        




    }
}
