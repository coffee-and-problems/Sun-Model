using System;
using System.IO;
using System.Linq;
using nom.tam.fits;

namespace ModelCalculations
{
    public class BifrostModel
    {
        public Array data { get; }
        public float[] hData { get; }
        public float hMax { get; }
        public float maxImpactParameter { get; }
        public int StartIndex { get; }
        public string lable { get; }

        public BifrostModel(string file)
        {
            hData = FitsHandler.GetData(file, 1) as float[];
            data = FitsHandler.GetData(file, 0) as Array;
            hMax = hData.Max() * 1000;
            maxImpactParameter = 1 + hMax / Constants.RSun;
            StartIndex = GetStartIndex(hData);
            var timestep = Path.GetFileNameWithoutExtension(Constants.LgT).Split('_').Last();
            lable = "BIFROST" + timestep;
        }

        /// <summary>
        ///     Переводит данные из логарифмического масштаба в линейный, а заодно и в массив float[][]
        /// </summary>
        /// <param name="model">Объект модели</param>
        /// <param name="sliceNumber">Количество срезов исходных данных (по умолчанию 1)</param>
        public static float[][] TransformToLinearData(BifrostModel model, float logBase = 10, int sliceNumber = 1)
        {
            var hData = model.hData;
            var logData = model.data;
            var linearData = new float[hData.Length - model.StartIndex][];

            for (var h = 0; h < hData.Length - model.StartIndex; h++)
            {
                var linearSlice = new float[hData.Length * sliceNumber];

                for (var z = 0; z < sliceNumber; z++)
                {
                    for (var i = 0; i < hData.Length; i++)
                    {
                        var _slice0 = logData.GetValue(h + model.StartIndex) as Array;
                        var _slice1 = _slice0.GetValue(475 + z) as float[];
                        linearSlice[i + hData.Length * z] = Convert.ToSingle(Math.Pow(logBase, _slice1[i]));
                    }
                }

                linearData[h] = linearSlice;
            }

            return linearData;
        }

        private static int GetStartIndex(float[] hData)
        {
            var index = 0;

            for (; hData[index] < 0; index++) {} //да, я знаю о бинарном поиске, но тут и так сойдет

            return index;
        }
    }
}