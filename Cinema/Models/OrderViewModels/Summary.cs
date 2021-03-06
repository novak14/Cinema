﻿using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.OrderViewModels
{
    public class Summary
    {
        public Summary(decimal Price, DateTime Time, DateTime Date, string FilmName, List<Places> Seat, int IdCartFilm)
        {
            this.Price = Price;
            this.Time = Time;
            this.Date = Date;
            this.FilmName = FilmName;
            this.Seat = Seat;
            this.IdCartFilm = IdCartFilm;
        }

        public Summary(int IdOrder, DateTime Date, DateTime Time, int Amount, DateTime CreateDate, string FilmName,
            List<Places> Seat, string ChooseTransport, string PaymentMethod)
        {
            this.IdOrder = IdOrder;
            this.Date = Date;
            this.Time = Time;
            this.Amount = Amount;
            this.CreateDate = CreateDate;
            this.FilmName = FilmName;
            this.Seat = Seat;
            this.ChooseTransport = ChooseTransport;
            this.PaymentMethod = PaymentMethod;
        }
        public int Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public int IdOrder { get; set; }

        public int IdCartFilm { get; set; }
        /// <summary>
        /// Cena jednoho filmu
        /// </summary>
        public decimal Price { get; set; }

        public decimal OverallPrice { get; set; } = 0;

        /// <summary>
        /// Cas zahajeni predstaveni
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Datum predstaveni
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Jmeno filmu
        /// </summary>
        public string FilmName { get; set; }

        /// <summary>
        /// Zpusob platby
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Cislo sedadla
        /// </summary>
        public List<Places> Seat { get; set; }

        /// <summary>
        /// Zpusob vyzvednuti
        /// </summary>
        public string ChooseTransport { get; set; }

        /// <summary>
        /// Pro zobrazeni ve View payment zpusobu
        /// </summary>
        public List<PaymentMethod> ChoosePaymentMethod { get; set; }

        /// <summary>
        /// Zobrazeni zpusobu pro ziskani vstupenky
        /// </summary>
        public List<DeliveryType> ChooseDeliveryType { get; set; }
    }
}
