using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Learn.Models
{
    public partial class Service
    {
        public Service()
        {
            Appointment = new HashSet<Appointment>();
            ServicePhoto = new HashSet<ServicePhoto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public int Duration { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<ServicePhoto> ServicePhoto { get; set; }

        public SolidColorBrush Background
        {
            get
            {
                return Discount > 0 ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.White;
            }
        }

        public string StrikethroughPrice
		{
            get
            {
                return Discount > 0 ? Math.Round(Price, 2).ToString() + " " : "";
            }
        }
        
        public string PriceTime
        {
            get
            {
                return Discount > 0 ? Math.Round((decimal)(Price - Price * Discount / 100), 2).ToString() + " рублей за " + Duration.ToString() + " минут" : Math.Round(Price, 2).ToString() + " рублей за " + Duration.ToString() + " минут";
            }
        }

        public string DiscountString
        {
            get
            {
                return Discount > 0 ? "* скидка " + Discount.ToString() + "%" : "";
            }
        }

        private Visibility editDeleteShow = Visibility.Hidden;

        [NotMapped]
        public Visibility EditDeleteShow
		{
            get
            {
                return editDeleteShow;
            }
            set 
            { 
                editDeleteShow = value;
            }
        }
    }
}
