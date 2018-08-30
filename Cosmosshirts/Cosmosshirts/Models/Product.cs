using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;
using Xamarin.Forms;

namespace Cosmosshirts.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ImageSource ProductImage { get; set; }

    }
}
