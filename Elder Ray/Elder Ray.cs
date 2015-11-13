using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ElderRay : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 13, MinValue = 2)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Output("Bear Power", Color = Colors.Red, PlotType = PlotType.Histogram)]
        public IndicatorDataSeries BearPowerBar { get; set; }

        [Output("Bear Power Line", Color = Colors.Red, PlotType = PlotType.Line)]
        public IndicatorDataSeries BearPowerLine { get; set; }

        [Output("Bear Power EMA", Color = Colors.Yellow, PlotType = PlotType.Line)]
        public IndicatorDataSeries BearPowerEMA { get; set; }

        [Output("Bull Power", Color = Colors.Green, PlotType = PlotType.Histogram)]
        public IndicatorDataSeries BullPowerBar { get; set; }

        [Output("Bull Power Line", Color = Colors.Green, PlotType = PlotType.Line)]
        public IndicatorDataSeries BullPowerLine { get; set; }

        [Output("Bull Power EMA", Color = Colors.Blue, PlotType = PlotType.Line)]
        public IndicatorDataSeries BullPowerEMA { get; set; }

        [Output("Green Bar", Color = Colors.Green, PlotType = PlotType.Histogram)]
        public IndicatorDataSeries GreenBar { get; set; }

        [Output("Red Bar", Color = Colors.Red, PlotType = PlotType.Histogram)]
        public IndicatorDataSeries RedBar { get; set; }

        private MovingAverage movingAverage;

        private IndicatorDataSeries atrBull;
        private IndicatorDataSeries atrBear;
        private ExponentialMovingAverage emaBull;
        private ExponentialMovingAverage emaBear;


        protected override void Initialize()
        {
            atrBull = CreateDataSeries();
            atrBear = CreateDataSeries();
            emaBull = Indicators.ExponentialMovingAverage(atrBull, 12);
            emaBear = Indicators.ExponentialMovingAverage(atrBear, 12);
            movingAverage = Indicators.MovingAverage(Source, Periods, MAType);

        }

        public override void Calculate(int index)
        {
            BearPowerBar[index] = MarketSeries.Low[index] - movingAverage.Result[index];
            BearPowerLine[index] = MarketSeries.Low[index] - movingAverage.Result[index];
            BullPowerBar[index] = MarketSeries.High[index] - movingAverage.Result[index];
            BullPowerLine[index] = MarketSeries.High[index] - movingAverage.Result[index];

            atrBull[index] = BullPowerBar[index];
            BullPowerEMA[index] = emaBull.Result[index];

            atrBear[index] = BearPowerBar[index];
            BearPowerEMA[index] = emaBear.Result[index];


            if (BullPowerLine[index] < 0)
                GreenBar[index] = BullPowerLine[index];

            if (BearPowerLine[index] > 0)
                RedBar[index] = BearPowerLine[index];


        }
    }
}
