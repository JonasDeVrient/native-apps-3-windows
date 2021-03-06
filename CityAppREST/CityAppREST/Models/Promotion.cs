﻿namespace CityAppREST.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Discount Discount { get; set; }

        public Promotion(string desription, Discount discount = null)
        {
            Description = desription;
            Discount = discount;
        }

        protected Promotion()
        {

        }

        public override string ToString()
        {
            return string.Format("{0}", Description);
        }
    }
}
