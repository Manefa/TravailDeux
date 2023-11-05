using ExerxiceNavigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TravailDeux
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AfficheProduit : Page
    {
        ObservableCollection<Produit> listeProduits = SingletonListeBD.GetInstance().GetListeProduits();
        int index = 0;
        private GridViewItem _clickedItem;
        public AfficheProduit()
        {
            this.InitializeComponent();

            produitsGridView.ItemsSource = listeProduits;
        }

        private void retour_Click(object sender, RoutedEventArgs e)
        {
            afficheFrame.Navigate(typeof(AjouteProduit));

        }

        private void produitsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listeProduits.Count > 0)
                afficheFrame.Navigate(typeof(ModifieProduit), listeProduits[produitsGridView.SelectedIndex]);

            index += produitsGridView.SelectedIndex;

        }

        private async void supprimer_Click(object sender, RoutedEventArgs e)
        {

            int selectedIndex = 0;

            Button button = (Button)sender;

            DependencyObject parent = VisualTreeHelper.GetParent(button);

            while (parent != null && !(parent is GridViewItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is GridViewItem gridViewItem)
            {
                selectedIndex = produitsGridView.IndexFromContainer(gridViewItem);

            }

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = mainpanel.XamlRoot;
            dialog.Title = "Attention";
            dialog.PrimaryButtonText = "Oui";
            dialog.SecondaryButtonText = "Non";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Secondary;
            dialog.Content = "Voulez vous vraiment supprimer ce produit ?";

            ContentDialogResult resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary)
            {
                SingletonListeBD.GetInstance().Supprimer(listeProduits[selectedIndex].Code);
                listeProduits.Clear();
                listeProduits = SingletonListeBD.GetInstance().GetListeProduits();
            }
            else if (resultat == ContentDialogResult.Secondary)
            {
                
            }
            else
            {
               
            }

            
        }

        private void produitsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //_clickedItem = (GridViewItem)produitsGridView.ContainerFromItem(e.ClickedItem);

        }

        private async void Savedata_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(SingletonFenetre.GetInstance().Fenetre);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

                picker.SuggestedFileName = "listeproduits";
                picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

                // Demandez à l'utilisateur de choisir un emplacement pour enregistrer le fichier
                Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

                if (monFichier != null)
                {
                    List<Produit> liste = new List<Produit>();
                    foreach (Produit produit in SingletonListeBD.GetInstance().GetListeProduits())
                    {
                        liste.Add(produit);
                    }

                    // La fonction ToString de la classe Client retourne: nom + ";" + prenom
                    await Windows.Storage.FileIO.WriteLinesAsync(monFichier, liste.ConvertAll(x => x.ToStringCSV()), Windows.Storage.Streams.UnicodeEncoding.Utf8);

                    ContentDialog dialog = new ContentDialog();
                    dialog.XamlRoot = mainpanel.XamlRoot;
                    dialog.Title = "Information";
                    dialog.CloseButtonText = "OK";
                    dialog.Content = "Fichier sauvegarder avec success";

                    var result = await dialog.ShowAsync();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }


        }
    }
}
