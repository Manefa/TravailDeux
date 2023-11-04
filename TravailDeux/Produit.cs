using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailDeux
{
    internal class Produit
    {
        int code;
        string modele;
        string meuble;
        string categorie;
        string couleur;
        double prix;

        public int Code { get => code; set => code = value; }
        public string Modele { get => modele; set => modele = value; }
        public string Meuble { get => meuble; set => meuble = value; }
        public string Categorie { get => categorie; set => categorie = value; }
        public string Couleur { get => couleur; set => couleur = value; }
        public double Prix { get => prix; set => prix = value; }

        public override string ToString()
        {
            return $"code: {Code} - modele: {Modele} - meuble: {Meuble} - categorie: {Categorie} - couleur: {Couleur} - prix: {Prix}";
        }
    }

}
