﻿using Catalog.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Dal.Entities;
using Order.Dal.Repository.Abstraction;
using Order.Dal.Entities;

namespace Order.Business
{
    public class OrderService
    {
        private IFilmRepository _filmRepo;
        private ICartFilmRepository _cartFilmRepo;
        private IPlaceRepository _placeRepo;
        private ICartPlacesRepository _cartPlacesRepo;
        private IPaymentMethodRepository _paymentMethodRepo;
        private IDeliveryTypeRepository _deliveryTypeRepo;
        private INewOrderRepository _newOrderRepo;
        private IOrderFilmRepository _orderFilmRepo;
        private IPaymentRepository _paymentRepo;

        public OrderService(
            IFilmRepository filmRepo, 
            ICartFilmRepository cartFilmRepo, 
            IPlaceRepository placeRepo,
            ICartPlacesRepository cartPlacesRepo,
            IPaymentMethodRepository paymentMethodRepo,
            IDeliveryTypeRepository deliveryTypeRepo,
            INewOrderRepository newOrderRepo,
            IOrderFilmRepository orderFilmRepo,
            IPaymentRepository paymentRepo)
        {
            _filmRepo = filmRepo;
            _cartFilmRepo = cartFilmRepo;
            _placeRepo = placeRepo;
            _cartPlacesRepo = cartPlacesRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _deliveryTypeRepo = deliveryTypeRepo;
            _newOrderRepo = newOrderRepo;
            _orderFilmRepo = orderFilmRepo;
            _paymentRepo = paymentRepo;
        }

        /// <summary>
        /// Ulozi objednavku
        /// </summary>
        /// <param name="IdUser"></param>
        /// <param name="IdFilm"></param>
        /// <param name="IdTime"></param>
        /// <param name="IdDate"></param>
        public void Add(string IdUser, int IdFilm, DateTime IdTime, DateTime IdDate)
        {
            _cartFilmRepo.Add(IdUser, IdFilm, IdTime, IdDate);
        }

        /// <summary>
        /// Aktualizuje objednavku
        /// </summary>
        /// <param name="IdCartFilm"></param>
        /// <param name="Amount"></param>
        public void Update(int IdCartFilm, int Amount)
        {
            _cartFilmRepo.Update(IdCartFilm, Amount);
        }

        /// <summary>
        /// Slouzi k zobrazeni volnych mist
        /// </summary>
        /// <param name="IdDate"></param>
        /// <param name="IdFilm"></param>
        /// <returns></returns>
        public List<Places> GetSeats(DateTime IdDate, int IdFilm)
        {
            List<Places> plac = new List<Places>();
            int testNull = _cartFilmRepo.TestIfFirst(IdDate, IdFilm);
            if (testNull != 0)
            {
                plac = _placeRepo.CheckFreePlaces(plac, IdFilm, IdDate);
            } 
            else
            {
                plac = _placeRepo.ShowAllSeats(IdFilm, IdDate);
            }
            return plac;
        }
        
        /// <summary>
        /// Ulozeni vybranych mist do databaze 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="IdUser"></param>
        public int FindChooseSeats(List<Places> model, string IdUser, int IdFilm, DateTime IdDate)
        {
            CartFilm getLast = new CartFilm();
            int count = 0;
            foreach (var item in model)
            {
                if (item.checkboxAnswer == true)
                {
                    getLast = _cartFilmRepo.GetLastCart(IdUser, IdFilm, IdDate);
                    _cartPlacesRepo.Add(getLast.IdCartFilm, item.IdPlace);
                    count++;
                }
            }
            _cartFilmRepo.Update(getLast.IdCartFilm, count);
            return count;
        }

        /// <summary>
        /// Ziska polozky pro zobrazeni v Summary
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        public List<CartFilm> GetSummary(string IdUser)
        {
            return _cartFilmRepo.GetSummary(IdUser);
        }

        /// <summary>
        /// Ziska vsechny zpusoby platby
        /// </summary>
        /// <returns></returns>
        public List<PaymentMethod> GetAllMethod()
        {
            return _paymentMethodRepo.GetAllMethod();
        }

        /// <summary>
        /// Ziska vsechny zpusoby prevzeti
        /// </summary>
        /// <returns></returns>
        public List<DeliveryType> GetAllDelivery()
        {
            return _deliveryTypeRepo.GetAllMethod();
        }

        public NewOrder AddOrder(string IdUser, int IdPayment, int IdDeliveryType, DateTime date)
        {
            return _newOrderRepo.Add(IdUser, IdPayment, IdDeliveryType, date);
        }

        /// <summary>
        /// Ziska vsechny polozky uzivatele z kosiku, ktere maji vybrana mista
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        public List<CartFilm> GetUserCart(string IdUser)
        {
            return _cartFilmRepo.GetUserCart(IdUser);
        }

        /// <summary>
        /// Ziska vsechny polozky z kosiku uzivatele
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        public List<CartFilm> GetUserCartForShow(string IdUser)
        {
            return _cartFilmRepo.GetUserCartForShow(IdUser);
        }

        /// <summary>
        /// Prida film do orderFilm
        /// </summary>
        /// <param name="IdOrder">Cislo objednavky</param>
        /// <param name="IdFilm">Cislo filmu</param>
        /// <param name="Amount">Pocet mist</param>
        /// <param name="Time">Cas predstaven</param>
        /// <param name="Date">Datum predstaveni</param>
        /// <param name="IdCartFilm">Cislo "kosiku"</param>
        public void AddOrderFilm(int IdOrder, int IdFilm, int Amount, DateTime Time, DateTime Date, int IdCartFilm)
        {
            _orderFilmRepo.Add(IdOrder, IdFilm, Amount, Time, Date, IdCartFilm);
        }

        /// <summary>
        /// Vymaze film cartFilm
        /// </summary>
        /// <param name="IdCartFilm"></param>
        public void DeleteCartFilm(int IdCartFilm)
        {
            _cartFilmRepo.DeleteItem(IdCartFilm);
        }

        /// <summary>
        /// Prida payment do tabulky a vrati IdPayment
        /// </summary>
        /// <param name="IdMethod"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public Payment AddGetPayment(int IdMethod, decimal Price)
        {
            return _paymentRepo.Add(IdMethod, Price);
        }

        /// <summary>
        /// Vymaze film podle cisla "kosiku"
        /// </summary>
        /// <param name="IdCartFilm"></param>
        public void DeleteFilm(int IdCartFilm)
        {
            _cartFilmRepo.DeleteItem(IdCartFilm);
            _cartPlacesRepo.Delete(IdCartFilm);
        }

        /// <summary>
        /// Ziska vsechny objednvaky podle uzivatele
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        public List<NewOrder> GetAllOrder(string IdUser)
        {
            return _newOrderRepo.GetAllOrders(IdUser);
        }

        /// <summary>
        /// Ziska detailni informace objednavky
        /// </summary>
        /// <param name="IdOrder"></param>
        /// <returns></returns>
        public List<NewOrder> GetDetailOrder(int IdOrder)
        {
            return _newOrderRepo.GetHistoryOrder(IdOrder);
        }


    }
}
