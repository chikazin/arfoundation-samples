using System;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class GPS2UnityCoord : MonoBehaviour
    {
        public Text result;

        private static double Radians(double degree)
        {
            return Math.PI / 180 * degree;
        }

        public UnityCoord CalcUnityCoord( LocationInfo origin,LocationInfo obj)
        {

            double p_la = origin.latitude;
            double p_lg = origin.longitude;
            double p_al = origin.altitude;

            double f_la = obj.latitude;
            double f_lg = obj.longitude;
            double f_al = obj.altitude;

            double lg_diff = f_lg - p_lg;
            double la_diff = f_la - p_la;

            // 参考https://www.wikiwand.com/en/Geographic_coordinate_system#Expressing_latitude_and_longitude_as_linear_units
            double lg2m_factor = 111412.84 * Math.Cos(Radians(p_la))
                - 93.5 * Math.Cos(3 * Radians(p_la))
                + 0.118 * Math.Cos(5 * Radians(p_la));

            double la2m_factor = 111132.92
                - 559.82 * Math.Cos(2 * Radians(p_la))
                - 1.175 * Math.Cos(4 * Radians(p_la))
                + 0.0023 * Math.Cos(6 * Radians(p_la));

            float x = (float)(lg_diff * lg2m_factor);
            float y = (float)(f_al - p_al);
            float z = (float)(la_diff * la2m_factor);



            result.text = $"x: {x}\n" +
                $"y: {y}\n" +
                $"z: {z}\n";
            return new UnityCoord { X = x, Y = y, Z = z };
        }


    }


    public struct UnityCoord
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

    }

    public struct LocationInfo
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }
    }
}
