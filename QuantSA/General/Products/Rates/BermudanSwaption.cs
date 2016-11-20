﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantSA.General
{
    /// <summary>
    /// The cashflows on a product which exercises into another are of two type:
    /// <para/>
    /// 1: Cashflows that take place until exercise, we assume that cashflows that take 
    /// place before an exercise are independent of the time of the exercise
    /// <para/>
    /// 2: Cashflows that take place if exercise occurs at an exercise date.  For example 
    /// a penalty that must be paid at the point of exercise on a cancellable swap.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <seealso cref="QuantSA.General.ProductWithEarlyExercise" />
    [Serializable]
    public class BermudanSwaption : ProductWithEarlyExercise
    {
        List<Date> exDates;
        Product postExerciseSwap;
        Date valueDate;
        Currency ccy = Currency.ZAR;
        bool longOptionality;

        /// <summary>
        /// Initializes a new instance of the <see cref="BermudanSwaption" /> class.
        /// </summary>
        /// <param name="postExerciseSwap">The post exercise swap.</param>
        /// <param name="exDates">The ex dates.</param>
        /// <param name="longOptionality">if set to <c>true</c> then the holder of this owns the optionality.</param>
        public BermudanSwaption(Product postExerciseSwap, List<Date> exDates, bool longOptionality)
        {
            this.postExerciseSwap = postExerciseSwap;
            this.exDates = exDates;
            this.longOptionality = longOptionality;
        }

        /// <summary>
        /// Gets the post ex products.
        /// </summary>
        /// <remarks>
        /// It is a list in case the underlying product is different at each exercise date
        /// </remarks>
        /// <returns></returns>
        public override List<Product> GetPostExProducts()
        {
            return new List<Product> { postExerciseSwap };
        }

        public override List<Date> GetExerciseDates()
        {
            return exDates;
        }

        public override int GetPostExProductAtDate(Date exDate)
        {
            return 0;
        }


        public override bool IsLongOptionality(Date exDate)
        {
            return longOptionality;
        }


        public override void SetValueDate(Date valueDate)
        {
            this.valueDate = valueDate;
        }

        public override void Reset()
        {
            // nothing to reset
        }

        public override List<Currency> GetCashflowCurrencies()
        {
            return new List<Currency> { ccy };
        }

        public override List<MarketObservable> GetRequiredIndices()
        {
            return new List<MarketObservable>();
        }

        public override List<Date> GetRequiredIndexDates(MarketObservable index)
        {
            return new List<Date>();
        }

        public override List<Date> GetCashflowDates(Currency ccy)
        {
            return new List<Date>();
        }

        public override void SetIndexValues(MarketObservable index, double[] indexValues)
        {
            // Do nothing
        }

        public override List<Cashflow> GetCFs()
        {
            return new List<Cashflow>();
        }

    }
}
