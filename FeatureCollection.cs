
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Anamorphing.Models
{
    //Class для хранения geojson в виде объекта 
    public class FeatureCollection
    {
        [JsonIgnore]
        public List<double> all_x = new List<double>();
        [JsonIgnore]
         public List<double> all_y = new List<double>();


        [JsonProperty("features")]
        public List<Feature> features;
        [JsonProperty("type")]
        public string type;
        [JsonProperty("properties")]
        public Properties properties;

        //Helper для получения координат
        ///<summary>
        ///Helper для получения коор-ты Х
        ///</summary>
        public double getX(int num_cell, int num_vertex)
        {
            //geomtries[0]-???? Coordinates[0][][]-???
            return features[num_cell].geo.geometries[0].Coordinates[0][num_vertex][0];
        }
        ///<summary>
        ///Helper для получения координаты У
        ///</summary>
        public double getY(int num_cell, int num_vertex)
        {
            //geomtries[0]-???? Coordinates[0][][]-???
            return features[num_cell].geo.geometries[0].Coordinates[0][num_vertex][1];
        }

        ///<summary>
        ///Hepler установка нового значение Х
        ///</summary>
        public void setX(int num_cell, int num_vertex, double newValue)
        {
            features[num_cell].geo.geometries[0].Coordinates[0][num_vertex][0] = newValue;
        }
        ///<summary>
        ///Helper установка нового значения У
        ///</summary>
        public void setY(int num_cell, int num_vertex, double newValue)
        {
            features[num_cell].geo.geometries[0].Coordinates[0][num_vertex][1] = newValue;
        }

        public void setX_ref(int num_cell, int num_vertex, int newValue)
        {
            features[num_cell].geo.geometries[0].x_ref = newValue;
        }
        public void setY_ref(int num_cell, int num_vertex, int newValue)
        {
            features[num_cell].geo.geometries[0].y_ref = newValue;
        }
            public int getX_ref(int num_cell, int num_vertex, int newValue)
        {
            return features[num_cell].geo.geometries[0].x_ref;
        }
        public int getY_ref(int num_cell, int num_vertex, int newValue)
        {
            return features[num_cell].geo.geometries[0].y_ref;
        }

        ///<summary>
        ///Helper для получения кол-ва ячеек
        ///</summary>
        public int cellCount()
        {
            return features.Count;
        }
        ///<summary>
        ///Количество вершин ячейки
        ///</summary>
        public int vertexCount(int num_cell)
        {
            return features[num_cell].geo.geometries[0].Coordinates[0].Count;
        }
        public void setColor(string color)
        {
            for (int i = 0; i < features.Count; i++)
                features[i].properties.fill_color = color;
        }
    }
    public class Feature
    {
        [JsonProperty("geometry")] //т.к. имя переменной не совпадает с именем в JSON, необходимо указывать жестко, с каким именем это работает
        public geomerty geo;
        [JsonProperty("type")]
        public string type;
        public Properties properties;
    }

    public class geomerty
    {
        [JsonProperty("geometries")]
        public List<AnamorphCell> geometries;
        public string type;
    }

    public class AnamorphCell
    {
        [JsonIgnore]
        public int x_ref { get; set; }
        [JsonIgnore]
        public int y_ref { get; set; }

        [JsonProperty("coordinates")]
        public List<List<List<double>>> Coordinates;
        public string type;

    }
    public class Properties
    {
        [JsonProperty("alpha3")]
        public string alpha3;
        [JsonProperty("name")]
        public string name;
        [JsonProperty("fill_color")]
        public string fill_color;
        [JsonProperty("type")]
        public string type;
        [JsonProperty("LineStyle")]
        public LineStyleProperty LineStyle;
    }

    public class LineStyleProperty
    {
        [JsonProperty("color")]
        public string color;
        [JsonProperty("width")]
        public float width;
    }
    public struct GeoPoint
    {


        double x;
        double y;
        public void setX(double _x)
        {
            x = _x;
        }
        public void setY(double _y)
        {
            y = _y;
        }
        public double getX()
        {
            return x;
        }
        public double getY()
        {
            return y;
        }
    }
}