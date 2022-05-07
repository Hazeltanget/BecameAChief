﻿using BecomeAChef.Core;
using BecomeAChef.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BecomeAChef.EF;
using System.Windows;

namespace BecomeAChef.MVVM.ViewModel
{
    class AddReceipeViewModel: ObservableObject
    {
        public RelayCommand ChangeImageCommand { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set 
            { 
                title = value;
                OnPropertyChanged();
            }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set 
            { 
                content = value;
                OnPropertyChanged();
            }
        }

        private short cookingTime;

        public short CookingTime
        {
            get { return cookingTime; }
            set 
            { 
                cookingTime = (short)value;
                OnPropertyChanged();
            }
        }

        private byte portions;

        public byte Portions
        {
            get { return portions; }
            set 
            { 
                portions = (byte)value;
                OnPropertyChanged();
            }
        }

        private BitmapImage image;

        public BitmapImage Image
        {
            get
            {
                if (image == null)
                {
                    Uri resourceUri = new Uri(System.AppContext.BaseDirectory + "../../Theme/Images/BackgroundImage.png", UriKind.Relative);
                    return new BitmapImage(resourceUri);
                }

                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }



        public AddReceipeViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            ChangeImageCommand = new RelayCommand(o =>
            {
                Image = new ImageConverter().GetImageFromFileDialog();
            });
        }

        public void AddRecipe()
        {
            Recipe recipe = new Recipe()
            {
                UserID = ((User)UserDataSaver.UserObject).ID,
                Contents = Content,
                Image = new ImageConverter().GetJPGFromImageControl(Image),
                Title = this.Title,
                CookingTimeMinutes = this.CookingTime,
                Portions = this.Portions, 
                PublishedTimestamp = DateTime.Now
            };

            using (RecipeBookDBEntities db = new RecipeBookDBEntities())
            {
                try
                {
                    db.Recipe.Add(recipe);
                    db.SaveChanges();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            MessageBox.Show("Рецепт успешно добавлен", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}