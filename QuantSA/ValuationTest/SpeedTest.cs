﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantSA.General;
using Accord.Math.Random;
using Accord.Math;
using QuantSA.Valuation;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace ValuationTest
{
    [TestClass]
    public class SpeedTest
    {
        private Product[] GetListOfSwaps()
        {
            int N = 50000;
            Date anchorDate = new Date(2016, 11, 21);
            double rate = 0.08;
            bool payFixed = true;
            double notional = 1000000;            
            
            double[,] swapDist = new double[,] { { 0.171 }, { 0.148 }, { 0.101 }, { 0.094 }, { 0.108 }, { 0.056 }, { 0.041 }, { 0.049 }, { 0.047 }, { 0.056 }, { 0.013 }, { 0.013 }, { 0.010 }, { 0.011 }, { 0.011 }, { 0.004 }, { 0.003 }, { 0.005 }, { 0.007 }, { 0.006 }, { 0.004 }, { 0.004 }, { 0.007 }, { 0.005 }, { 0.006 }, { 0.006 }, { 0.003 }, { 0.003 }, { 0.002 }, { 0.005 } };
            IRandomNumberGenerator<double> generator1 = new ZigguratUniformGenerator(0, 1);
            IRandomNumberGenerator<double> generator365 = new ZigguratUniformGenerator(1, 365);
            double[,] cumSum = swapDist.CumulativeSum(1);

            Product[] allSwaps = new Product[N];
            for (int swapNum = 0; swapNum< N; swapNum++){
                double x = generator1.Generate();
                int years = 0;
                while (years < cumSum.GetLength(0) && x > cumSum[years, 0]) years++;
                int days = (int)Math.Round(generator365.Generate());
                Date endDate = anchorDate.AddTenor(new Tenor(days, 0, 0, years));
                Date startDate = endDate.AddTenor(Tenor.Years(-years - 1));
                allSwaps[swapNum] = IRSwap.CreateZARSwap(rate, payFixed, notional, startDate, Tenor.Years(years+1));
            }
            
            return allSwaps;
        }



        [Ignore]
        [TestMethod]        
        public void TestManySwaps()
        {
            Debug.StartTimer();
            Product[] allSwaps = GetListOfSwaps();
            
            Debug.WriteLine("Create swaps took: " + Debug.ElapsedTime().ToString());

            // Set up the model
            Date valueDate = new Date(2016, 11, 21);
            Date[] dates = { new Date(2016, 11, 21), new Date(2047, 11, 21) };
            Date[] datesLong = { new Date(2016, 11, 21), new Date(2018, 11, 21), new Date(2020, 11, 21), new Date(2022, 11, 21), new Date(2024, 11, 21), new Date(2047, 11, 21) };
            double[] rates = { 0.07, 0.07 };
            double[] ratesLong = { 0.07, 0.071, 0.072, 0.073, 0.074, 0.08 };
            IDiscountingSource discountCurve = new DatesAndRates(Currency.ZAR, valueDate, datesLong, ratesLong);
            IFloatingRateSource forecastCurve = new ForecastCurveFromDiscount(discountCurve, FloatingIndex.JIBAR3M,
                    new FloatingRateFixingCurve1Rate(0.07, FloatingIndex.JIBAR3M));            
            DeterminsiticCurves curveSim = new DeterminsiticCurves(discountCurve);
            curveSim.AddRateForecast(forecastCurve);
            Coordinator coordinator = new Coordinator(curveSim, new List<Simulator>(), 1);

            // Run the valuation
            Debug.StartTimer();
            double value = coordinator.Value(allSwaps, valueDate);
            Debug.WriteLine("Value took: " + Debug.ElapsedTime().ToString());
        }
    }
}
