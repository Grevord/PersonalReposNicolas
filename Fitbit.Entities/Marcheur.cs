using System;
using System.Collections.Generic;

namespace Fitbit.Entities
{
    public class Marcheur
    {
        #region champs
        private int _id;
        private string _nom;
        private DateTime _dateDeNaissance;
        private DateTime _dateDerniereMarche;
        private List<BadgeObtenu> _lesBadgesObtenus;
        private List<Parcours> _lesParcours;
        private List<Badge> _lesBadges;
        #endregion
        #region accesseurs
        public int Id { get => _id; }
        public string Nom { get => _nom; set => _nom = value; }
        public DateTime DateDeNaissance { get => _dateDeNaissance; }
        public DateTime DateDerniereMarche { get => _dateDerniereMarche; }



        public List<Parcours> LesParcours { get => _lesParcours; }
        public List<BadgeObtenu> LesBadgesObtenus { get => _lesBadgesObtenus; }
        public List<Badge> LesBadges { get => _lesBadges; }


        #endregion

        #region constructeurs

        /// <summary>
        /// 
        /// </summary>
        public Marcheur(List<Badge> badges)
        {
            _lesBadgesObtenus = new List<BadgeObtenu>();
            _lesParcours = new List<Parcours>();
            _lesBadges = badges;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <example> Marcheur m = new Marcheur( 1, "  fds dsf")</example>
        public Marcheur(int id, string name, List<Badge> badges) : this(badges)
        {
            this._id = id;
            this._nom = name;

        }
        /// <summary>
        /// sdfesdsfdsfsdfdsfdsfdsdsfsdfsd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nom"></param>
        /// <param name="dateNaissance"></param>
        public Marcheur(int id, string nom, DateTime dateNaissance, List<Badge> badges) : this(id, nom, badges)
        {
            this._dateDeNaissance = dateNaissance;
        }

        #endregion

        #region methodes
        public int AddParcours(DateTime date, int nombreDePas)
        {
            Parcours parcour = new Parcours(_lesParcours.Count + 1, nombreDePas, this);
            parcour.Date = date;
            _lesParcours.Add(parcour);
            List<Badge> mesbadges = VerifierBadges(parcour);
            return AjouterNewBadges(mesbadges, date);
        }

        private List<Badge> VerifierBadges(Parcours parcours)
        {   
            // mise en place d'une variable permettant de calculer la distance totale parcourue
            int distancetotale = 0;
            foreach(Parcours unparcours in _lesParcours)
            {
                distancetotale = distancetotale + unparcours.NombreDePas;
            }
            List<Badge> correspondingBadges = new List<Badge>();
            foreach (Badge item in _lesBadges)
            {
                if (item.DistanceJournaliere <= parcours.NombreDePas || item.DistanceCumulee <= distancetotale)
                {
                    correspondingBadges.Add(item);
                }
            }
            return correspondingBadges;

        }
        private int AjouterNewBadges(List<Badge> badges, DateTime date)
        {
            // Pour chaque badges déja obtenus 
            foreach (BadgeObtenu item in _lesBadgesObtenus)
            {
                if (badges.Contains(item.Badge) && item.Marcheur == this)
                {
                    // On ajoute la date à la liste des dates du badge déja obtenu
                    item.Dates.Add(date);
                    //ensuite on retire l'élément de la liste de badges
                    badges.Remove(item.Badge);
                }
            }
            //pour  chaque badge restants dans la liste
            foreach (Badge badge in badges)
            {
                BadgeObtenu obtenu = new BadgeObtenu()
                {
                    Badge = badge,
                    Marcheur = this
                };
                obtenu.Dates.Add(date);
                _lesBadgesObtenus.Add(obtenu);
            }            
            return badges.Count;
        }        
    }
    #endregion



}
