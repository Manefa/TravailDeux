using ExerxiceNavigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        ObservableCollection<Produit> listeProduits = SingletonListeBD.GetInstance().GetListeProduits();
        public MainWindow()
        {
            this.InitializeComponent();

            SingletonFenetre.GetInstance().Fenetre = this;

            if(listeProduits.Count > 0)
            {
                mainFrame.Navigate(typeof(AfficheProduit));
            }
            
        }

        //private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        //{
        //    var item = (NavigationViewItem)args.SelectedItem;

        //    switch (item.Name)
        //    {
        //        case "afficheProduit":
        //            mainFrame.Navigate(typeof(AjouteProduit));
        //            break;
        //        case "ajouterProduit":
        //            mainFrame.Navigate(typeof(AfficheProduit));
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private async void chargedata_Click(object sender, RoutedEventArgs e)
        {

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".csv");

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(SingletonFenetre.GetInstance().Fenetre);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            //sélectionne le fichier à lire
            Windows.Storage.StorageFile monFichier = null;

            try
            {
                // Demande à l'utilisateur de choisir un fichier à lire
                monFichier = await picker.PickSingleFileAsync();
            }
            catch (Exception ex)
            {
                // gestion d'exception ici
            }

            if (monFichier != null)
            {
                //ouvre le fichier et lit le contenu
                var lignes = await Windows.Storage.FileIO.ReadLinesAsync(monFichier);

                List<Produit> liste = new List<Produit>();

                /*boucle permettant de lire chacune des lignes du fichier
                * et de remplir une liste d'objets de type CLient
                */
                foreach (var ligne in lignes)
                {
                    var v = ligne.Split(";");
                    SingletonListeBD.GetInstance().Ajouter(new Produit
                    {
                        Code = Convert.ToInt32(v[0] as string),
                        Meuble = v[1],
                        Categorie = v[2],
                        Couleur = v[4],
                        Modele = v[3],
                        Prix = Convert.ToDouble((v[5] as string).Substring(0, (v[5] as string).Length - 1), CultureInfo.InvariantCulture)
                    });
                    liste.Add(new Produit
                    {
                        Code = Convert.ToInt32(v[0] as string),
                        Meuble = v[1],
                        Categorie = v[2],
                        Couleur = v[4],
                        Modele = v[3],
                        Prix = Convert.ToDouble(v[5] as string, CultureInfo.InvariantCulture)
                    });

                }

                //on peut mettre la liste de Clients comme source d'une listView
                //produitsGridView.ItemsSource = liste;

                mainFrame.Navigate(typeof(AfficheProduit));
            }
            else
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Aucun fichier n'a ete selectionner";

                var result = await dialog.ShowAsync();

            }



        }
    }
}
