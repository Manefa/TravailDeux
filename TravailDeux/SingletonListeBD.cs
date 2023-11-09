using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravailDeux;

namespace ExerxiceNavigation
{
    internal class SingletonListeBD
    {
        ObservableCollection<Produit> liste;
        MySqlConnection con;
        static SingletonListeBD instance = null;

        // Constructeur de la classe
        public SingletonListeBD()
        {
            liste = new ObservableCollection<Produit>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420326_gr01_2204989-yousouf-esdras-manefa;Uid=2204989;Pwd=2204989;");
        }

        // Retourne l'instance du singleton
        public static SingletonListeBD GetInstance()
        {
            if (instance == null)
                instance = new SingletonListeBD();

            return instance;
        }

        // Retourne la liste des Produits
        public ObservableCollection<Produit> GetListeProduits()
        {
            liste.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("ObtenirTousLesProduits");
                commande.Connection = con;
                //commande.CommandText = "Select * from produits";
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                Produit produit;
                while (r.Read())
                {
                    produit = new Produit
                    {
                        Code = Convert.ToInt32(r["code"]),
                        Modele = r["modele"] as String,
                        Meuble = r["meuble"] as String,
                        Categorie = r["categorie"] as String,
                        Couleur = r["couleur"] as String,
                        Prix = Convert.ToDouble(r["prix"], CultureInfo.InvariantCulture)
                    };

                    liste.Add(produit);
                }

                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return liste;
        }

        // Retourne le code du dernier produit 
        public int GetCodeDernierProduit()
        {
            int code = 1;
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "Select * from produits order by code desc limit 1";
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                r.Read();
                code = Convert.ToInt32(r["code"]);
                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return code;
        }

        // Retourne un Produit à une position précise
        public Produit GetProduit(int code)
        {
            Produit produit = new Produit();

            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"Select * from produits where code = @id";
                commande.Parameters.AddWithValue("@id", code);
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                while (r.Read())
                {
                    produit = new Produit
                    {
                        Code =  Convert.ToInt32(r["code"] as String),
                        Modele = r["modele"] as String,
                        Meuble = r["meuble"] as String,
                        Categorie = r["categorie"] as String,
                        Couleur = r["couleur"] as String,
                        Prix = Convert.ToDouble(r["prix"], CultureInfo.InvariantCulture)
                    };
                }

                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return produit;
        }

        // Ajoute un Produit dans la liste
        public void Ajouter(Produit produit)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("AjouterProduit");
                commande.Connection = con;
                //commande.CommandText = $"insert into produits values (@code, @modele, @meuble, @categorie, @couleur, @prix)";

                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("@p_code", produit.Code);
                commande.Parameters.AddWithValue("@p_modele", produit.Modele);
                commande.Parameters.AddWithValue("@p_meuble", produit.Meuble);
                commande.Parameters.AddWithValue("@p_categorie", produit.Categorie);
                commande.Parameters.AddWithValue("@p_couleur", produit.Couleur);
                commande.Parameters.AddWithValue("@p_prix", produit.Prix.ToString(CultureInfo.InvariantCulture));

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
            }

            liste.Add(produit);
        }

        // Modifie un Produit à une position précise
        public void Modifier(Produit produit)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"update produits set modele = @modele, meuble = @meuble, " +
                                    $"categorie = @categorie, couleur = @couleur, prix = @prix where code = @code";

                commande.Parameters.AddWithValue("@modele", produit.Modele);
                commande.Parameters.AddWithValue("@meuble", produit.Meuble);
                commande.Parameters.AddWithValue("@categorie", produit.Categorie);
                commande.Parameters.AddWithValue("@couleur", produit.Couleur);
                commande.Parameters.AddWithValue("@prix", produit.Prix.ToString(CultureInfo.InvariantCulture));
                commande.Parameters.AddWithValue("@code", produit.Code);

                con.Open();
                commande.Prepare();
                int i = commande.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
            }
        }

        // Supprime un Produit à une position précise
        public void Supprimer(int code)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"delete from produits where code = @code";
                commande.Parameters.AddWithValue("@code", code);
                con.Open();
                commande.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
            }
        }
    }
}
