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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TravailDeux
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifieProduit : Page
    {
        public ModifieProduit()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {

                Produit produit = e.Parameter as Produit;
                tbCode.Text = produit.Code.ToString();
                tbModele.Text = produit.Modele;
                tbMeuble.Text = produit.Meuble;
                tbCategorie.Text = produit.Categorie;
                tbCouleur.Text = produit.Couleur;
                nbPrice.Text = produit.Prix.ToString();
            }
        }

        private async void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            Boolean formValid = true;

            if (tbModele.Text == "")
            {
                tbModeleError.Text = "Le modele est obligatoire";
                tbModeleError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbModeleError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }


            if (tbMeuble.Text == "")
            {
                tbMeubleError.Text = "Le meuble est obligatoire";
                tbMeubleError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {
                tbMeubleError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (tbCategorie.Text == "")
            {
                tbCategorieError.Visibility = Visibility.Visible;
                tbCategorieError.Text = "La categorie est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                tbCategorieError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (tbCouleur.Text == "")
            {
                tbCouleurError.Visibility = Visibility.Visible;
                tbCouleurError.Text = "La couleur est obligatoire";
                formValid = formValid & false;
            }
            else
            {
                tbCouleurError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (nbPrice.Value.ToString() == "" || nbPrice.Text == "")
            {

                nbPriceError.Text = "Le prix est obligatoire";
                nbPriceError.Visibility = Visibility.Visible;
                formValid = formValid & false;
            }
            else
            {

                nbPriceError.Visibility = Visibility.Collapsed;
                formValid = formValid & true;
            }

            if (formValid == true)
            {
                int code = SingletonListeBD.GetInstance().GetCodeDernierProduit();

                Produit produit = new Produit
                {
                    Code = Convert.ToInt32(tbCode.Text),
                    Modele = tbModele.Text,
                    Meuble = tbMeuble.Text,
                    Categorie = tbCategorie.Text,
                    Couleur = tbCouleur.Text,
                    Prix = Convert.ToInt32(nbPrice.Text)
                };

                SingletonListeBD.GetInstance().Modifier(produit);

                tbModele.Text = "";
                tbMeuble.Text = "";
                tbCategorie.Text = "";
                tbCouleur.Text = "";
                nbPrice.Text = "10";

                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = mainpanel.XamlRoot;
                dialog.Title = "Information";
                dialog.CloseButtonText = "OK";
                dialog.Content = "Produit ajouter avec success";

                var result = await dialog.ShowAsync();

                this.Frame.Navigate(typeof(AfficheProduit));

            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AfficheProduit));
        }
    }
}
