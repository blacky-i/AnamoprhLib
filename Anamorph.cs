using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Anamorphing.Models;
namespace Anamorphing.Math
{
    public class Anamorph
    {
        ///<summary>
        ///Проверяет выполнение алгоритма с точностью MorphError.
        ///Возвращает false, если точность выполнилась
        ///Возвращает true, если точность не выполнилась
        ///</summary>
        private static bool checkError(double[,] MorphSquares, double[] NewSquares, double MorphError)
        {
            if (NewSquares[0] == 0)
                return true;
            for (int i = 0; i < MorphSquares.GetLength(0); i++)
            {
                Console.WriteLine("{0:F2} - {1:F2}", MorphSquares[i, 0], NewSquares[i]);

                if (System.Math.Abs(MorphSquares[i, 0] - NewSquares[i]) > MorphError)
                    return true;
            }
            return false;
        }

        ///<summary>
        ///Сортировка по арктангенсу
        ///</summary>
        private static void sortPoints(ref FeatureCollection Country, List<GeoPoint> CenterCells)
        {
             double[][] Angles = new double[Country.cellCount()][];
            for (int i = 0; i < Country.cellCount(); i++)
            {
                double[] Angle_i = new double[Country.vertexCount(i)];
                for (int j = 0; j < Country.vertexCount(i); j++)
                {

                    double angle = 0;
                    if (CenterCells[i].getX() == Country.getX(i, j) && CenterCells[i].getY() == Country.getY(i, j))
                    {
                        angle = 0;

                    }
                    else if (CenterCells[i].getX() == Country.getX(i, j) && CenterCells[i].getY() < Country.getY(i, j))
                        angle = System.Math.PI / 2;
                    else if (CenterCells[i].getX() == Country.getX(i, j) && CenterCells[i].getY() > Country.getY(i, j))
                        angle = -1 * System.Math.PI / 2;
                    else if (CenterCells[i].getY() == Country.getY(i, j))
                        angle = 0;
                    else if (CenterCells[i].getX() > Country.getX(i, j) && CenterCells[i].getY() > Country.getY(i, j))
                        angle = System.Math.Atan(CenterCells[i].getY() - Country.getY(i, j) / CenterCells[i].getX() - Country.getX(i, j)) - System.Math.PI;
                    else if (CenterCells[i].getX() < Country.getX(i, j) && CenterCells[i].getY() >= Country.getY(i, j))
                        angle = System.Math.Atan(CenterCells[i].getY() - Country.getY(i, j) / CenterCells[i].getX() - Country.getX(i, j)) + System.Math.PI;
                    else
                        angle = System.Math.Atan(CenterCells[i].getY() - Country.getY(i, j) / CenterCells[i].getX() - Country.getX(i, j));
                    angle = System.Math.Atan2(CenterCells[i].getY() - Country.getY(i, j), CenterCells[i].getX() - Country.getX(i, j));
                    Angle_i[j] = angle;
                }
                Angles[i] = Angle_i;
            }

            for (int i = 0; i < Country.cellCount(); i++)
            {
                for (int j = 0; j < Country.vertexCount(i); j++)
                {
                    for (int k = i + 1; k < Country.vertexCount(i); k++)
                    {
                        if (Angles[i][k] > Angles[i][j])
                        {
                            double tempAngle = Angles[i][k];
                            Angles[i][k] = Angles[i][j];
                            Angles[i][j] = tempAngle;

                            double x = 0;
                            double y = 0;

                            x = Country.getX(i, k);
                            Country.setX(i, k, Country.getX(i, j));
                            Country.setX(i, j, x);

                            y = Country.getY(i, k);
                            Country.setY(i, k, Country.getY(i, j));
                            Country.setY(i, j, y);
                        }
                    }
                }
            }
        }

        private static void enableEquality(FeatureCollection comparer, ref FeatureCollection target)
        {
            for (int i = 0; i < comparer.cellCount(); i++)
            {
                for (int j = 0; j < comparer.vertexCount(i); j++)
                {
                    double x_1 = comparer.getX(i, j);
                    double y_1 = comparer.getY(i, j);

                    for (int i2 = 0; i2 < comparer.cellCount(); i2++)
                    {
                        for (int j2 = 0; j2 < comparer.vertexCount(i2); j2++)
                        {
                            double x_2 = comparer.getX(i2, j2);
                            double y_2 = comparer.getY(i2, j2);

                            if (x_1 == x_2 && y_1 == y_2)
                            {
                                target.setX(i2, j2, target.getX(i, j));
                                target.setY(i2, j2, target.getY(i, j));
                            }
                        }
                    }
                }
            }
        }
        private static GeoPoint moveCenter(List<GeoPoint> CenterCells, GeoPoint Center, double[,] Squares, double[,] MorphSquares)
        {
            double x = 0;
            double y = 0;
            for (int k = 0; k < CenterCells.Count; k++)
            {
                //11. Найти расстояние от вершины №1, ячейки №1 до ЦТ ячейки №1
                double r = System.Math.Sqrt(System.Math.Pow((CenterCells[k].getX() - Center.getX()), 2) + System.Math.Pow((CenterCells[k].getY() - Center.getY()), 2));
                //12. Для вершины №1 ячейки №1 рассчитать сдвиг, вызванный воздействием на нее ЦТ ячейки №1 (сдвиг по прямой соединяющей точку с ЦТ ячейки)
                double l = 0;

                if (r <= Squares[k, 1])
                    l = r * (MorphSquares[k, 1] / Squares[k, 1] - 1);
                else
                    l = System.Math.Sqrt(System.Math.Pow(r, 2) + (System.Math.Pow(MorphSquares[k, 1], 2) - System.Math.Pow(Squares[k, 1], 2))) - r;
                //13. Вычислить угол между прямой соединяющей вершину №1 с ЦТ ячеек  и осью ОХ 
                double angle = 0;
                if (CenterCells[k].getX() == Center.getX() && CenterCells[k].getY() == Center.getY())
                    angle = 0;
                else if (CenterCells[k].getX() == Center.getX())
                    angle = System.Math.PI / 2;
                else if (CenterCells[k].getY() == Center.getY())
                    angle = 0;
                else
                    angle = System.Math.Atan(System.Math.Abs(CenterCells[k].getY() - Center.getY()) / System.Math.Abs(CenterCells[k].getX() - Center.getX()));

                //                     angle =System.Math.Atan2(CenterCells[k].getY() - Center.getY() , CenterCells[k].getX() - Center.getX());
                //14. Рассчитать координаты сдвигов вершины №1 от воздействия ЦТ ячеек : , ;


                //15. С учетом расположения вершин в разных квадрантах, смещения равны:

                if (CenterCells[k].getX() > Center.getX())
                {
                    x += (l * System.Math.Cos(angle)) * (-1);
                }
                else
                    x += l * System.Math.Cos(angle);
                if (CenterCells[k].getY() > Center.getY())
                    y += (l * System.Math.Sin(angle)) * (-1);
                else
                    y += l * System.Math.Sin(angle);
            }
            Center.setX(Center.getX() + x);
            Center.setY(Center.getY() + y);
            return Center;
        }




        ///<summary>
        ///geojson - ввод geojson в виде строки
        ///PokazMorph - показатели анамаорфирования (население,ВВП итд.)
        ///ErrorMorph - заданная точность
        ///возвращает geojson в виде строки
        ///</summary>
        public static string MorphVBA(string geojson, double[] PokazMorph, double ErrorMorph)
        {
            Console.WriteLine("I'm in Morph!!!!!!!!!!");

            JsonSerializerSettings sett = new JsonSerializerSettings();
            sett.MissingMemberHandling = MissingMemberHandling.Ignore;

            FeatureCollection Object = JsonConvert.DeserializeObject<FeatureCollection>(geojson, sett);
            FeatureCollection TempObject = JsonConvert.DeserializeObject<FeatureCollection>(geojson, sett);

            //Собираем все точки
                    for (int i = 0; i < Object.cellCount(); i++)
                    {
                        for (int j = 0; j < Object.vertexCount(i); j++)
                        {
                            if(!Object.all_x.Contains(Object.getX_First(i, j)))
                                Object.all_x.Add(Object.getX_First(i, j));
                            Object.setX_ref(i,j,Object.all_x.LastIndexOf(Object.getX_First(i, j)));

                            if(!Object.all_y.Contains(Object.getY_First(i, j)))
                                Object.all_y.Add(Object.getY_First(i, j));
                            Object.setY_ref(i,j,Object.all_y.LastIndexOf(Object.getY_First(i, j)));
                        }
                    }
         //Тест 1 квадрата
          for (int j = 0; j < Object.vertexCount(0); j++)
                        {
                            System.Console.WriteLine(Object.getX(0,j)+"|"+Object.getY(0,j)+" ref "+Object.getX_ref(0,j)+"|"+Object.getY_ref(0,j));
                        }
                           for (int j = 0; j < Object.vertexCount(1); j++)
                        {
                            System.Console.WriteLine(Object.getX(1,j)+"|"+Object.getY(1,j) +" ref "+Object.getX_ref(1,j)+"|"+Object.getY_ref(1,j));
                        }

            Console.WriteLine("Ячеек = " + Object.features.Count);

            if (PokazMorph.Length != Object.cellCount())
            {
                Console.WriteLine(@"Количество ячеек для анаморфирования не совпадает 
            с количеством показателей анаморфирования. Они должны быть равны");
            }
            else
            {
                //1. Определить количество анаморфируемых ячеек ;
                //2. Определить координаты вершин границы анаморфируемых ячеек , где  – количество вершин границы ячейки;
                //3. Задать погрешность анаморфирования ε;
                //4. Выбрать центр тяжести (ЦТ) каждой анаморфируемой ячейки
                List<GeoPoint> CenterCells = new List<GeoPoint>();
                try
                {
                    for (int i = 0; i < Object.cellCount(); i++)
                    {
                        double x = 0;
                        double y = 0;
                        for (int j = 0; j < Object.vertexCount(i); j++)
                        {
                            x += Object.getX(i, j);
                            y += Object.getY(i, j);
                        }
                        GeoPoint item = new GeoPoint();

                        item.setX(x / Object.vertexCount(i));
                        item.setY(y / Object.vertexCount(i));

                        CenterCells.Add(item);
                    }
                }
                catch (Exception e)
                { Console.WriteLine("4. punkt  - " + e.ToString()); }


                try
                {
                    //       sortPoints(ref Object, CenterCells);
                }
                catch (Exception e)
                { Console.WriteLine("4. Sort  - " + e.ToString()); }

                // 5. Назначить показатель анаморфирования каждой ячейке ;
                // 6. Рассчитать средний показатель анаморфирования для всех ячеек
                double AverageMorph = 0;
                for (int i = 0; i < PokazMorph.Length; i++)
                    AverageMorph += PokazMorph[i];
                AverageMorph /= PokazMorph.Length;
                Console.WriteLine("AverageMorph =" + AverageMorph);
                //        7. Вычислить площадь каждой ячейки без учета ее показателя анаморфирования (обход ячейки против часовой стрелки – для получения положительного значения S(t))
                double[,] Squares = new double[Object.cellCount(), 2];
                double[,] MorphSquares = new double[Object.cellCount(), 2];
                for (int i = 0; i < Object.cellCount(); i++)
                {
                    double Square = 0;
                    for (int j = Object.vertexCount(i) - 1; j >= 0; j--)
                    {
                        if (j == Object.vertexCount(i) - 1)
                        {
                            Square += (Object.getX(i, j) - Object.getX(i, 0)) * (Object.getY(i, j) + Object.getY(i, 0));
                        }
                        else
                        {
                            Square += (Object.getX(i, j + 1) - Object.getX(i, j)) * (Object.getY(i, j + 1) + Object.getY(i, j));
                        }

                    }
                    //          8. Приравнять площадь каждой ячейки площади круга равной площади с радиусом

                    //        9. Вычислить площадь каждой ячейки с учетом ее показателя анаморфирования
                    //        10. Подсчитать радиус каждой ячейки с учетом ее показателя анаморфирования
                    Squares[i, 0] = Square / 2;
                    Squares[i, 1] = System.Math.Sqrt(Squares[i, 0] / System.Math.PI);
                    MorphSquares[i, 0] = Squares[i, 0] * PokazMorph[i] / AverageMorph;
                    MorphSquares[i, 1] = System.Math.Sqrt(MorphSquares[i, 0] / System.Math.PI);
                    //        Console.WriteLine("{0:F2} - {1:F2}", MorphSquares[i, 0], Squares[i, 0]);
                    Squares[i, 0] = 0;
                }


                double[] NewSquares = new double[Object.cellCount()];
                List<GeoPoint> TempCenter = CenterCells;
                System.Console.WriteLine("Dvigaem");

                int c = 0;

                do
                {
                    for (int i = 0; i < Object.cellCount(); i++)
                    {
                        if (c == 0)
                        {
                            TempCenter[i] = moveCenter(CenterCells, CenterCells[i], Squares, MorphSquares);

                        }
                        else
                        {
                            TempCenter[i] = moveCenter(TempCenter, TempCenter[i], Squares, MorphSquares);
                        }

                        for (int j = 0; j < Object.vertexCount(i); j++)
                        {
                            double xx_sum = 0;
                            double yy_sum = 0;

                            double x_prev = Object.getX(i, j);
                            double y_prev = Object.getY(i, j);

                            for (int k = 0; k < Object.cellCount(); k++)
                            {
                                double x_ct = 0;
                                double y_ct = 0;
                                if (c == 0)
                                {
                                    x_ct = CenterCells[k].getX();
                                    y_ct = CenterCells[k].getY();
                                }
                                else
                                {
                                    x_ct = TempCenter[k].getX();
                                    y_ct = TempCenter[k].getY();
                                }
                                double l = System.Math.Sqrt((x_ct - x_prev) * (x_ct - x_prev) + (y_ct - y_prev) * (y_ct - y_prev));
                                double l_smesh = 0;
                                if (l < Squares[k, 1])
                                    l_smesh = l * (MorphSquares[k, 1] / Squares[k, 1] - 1);
                                else
                                    l_smesh = System.Math.Sqrt(l * l + (MorphSquares[k, 1] * MorphSquares[k, 1] - Squares[k, 1] * Squares[k, 1])) - l;
                                //13. Вычислить угол между прямой соединяющей вершину №1 с ЦТ ячеек  и осью ОХ 
                                double angle = 0;
                                if (x_ct == x_prev && y_ct == y_prev)
                                {
                                    angle = 0;
                                }
                                else if (x_ct == x_prev)
                                    angle = System.Math.PI / 2;
                                else if (y_ct == y_prev)
                                    angle = 0;
                                else
                                    angle = System.Math.Atan(System.Math.Abs(y_ct - y_prev) / System.Math.Abs(x_ct - x_prev));
                                double xx = 0;
                                double yy = 0;

                                xx = l_smesh * System.Math.Cos(angle);
                                yy = l_smesh * System.Math.Sin(angle);

                                if (x_ct > x_prev)
                                    xx *= -1;
                                if (y_ct > y_prev)
                                    yy *= -1;

                                xx_sum += xx;
                                yy_sum += yy;
                            }
                            Object.setX(i, j, x_prev + xx_sum);
                            Object.setY(i, j, y_prev + yy_sum);
                        }

                    }
                    for (int i = 0; i < Object.cellCount(); i++)
                    {
                        double Square = 0;
                        for (int j = 0; j < Object.vertexCount(i); j++)
                        {
                            if (j == Object.vertexCount(i) - 1)
                                Square += (Object.getX(i, j) - Object.getX(i, 0)) * (Object.getY(i, j) + Object.getY(i, 0));
                            else
                                Square += (Object.getX(i, j + 1) - Object.getX(i, j)) * (Object.getY(i, j + 1) + Object.getY(i, j));

                        }
                        Squares[i, 0] = Square / 2;
                        NewSquares[i] = Square / 2;
                        Squares[i, 1] = System.Math.Sqrt(Squares[i, 0] / System.Math.PI);
                    }
                    c++;

                } while (checkError(MorphSquares, NewSquares, ErrorMorph) && c != 10);
                for (int i = 0; i < TempObject.cellCount(); i++)
                {
                    for (int j = 0; j < TempObject.vertexCount(i); j++)
                    {
                        double x_1 = TempObject.getX(i, j);
                        double y_1 = TempObject.getY(i, j);

                        for (int i2 = 0; i2 < TempObject.cellCount(); i2++)
                        {
                            for (int j2 = 0; j2 < TempObject.vertexCount(i2); j2++)
                            {
                                double x_2 = TempObject.getX(i2, j2);
                                double y_2 = TempObject.getY(i2, j2);

                                if (x_1 == x_2 && y_1 == y_2)
                                {
                                    Object.setX(i2, j2, Object.getX(i, j));
                                    Object.setY(i2, j2, Object.getY(i, j));
                                }
                            }
                        }
                    }
                }
            }
            System.Console.WriteLine("Preparing");
            //Замыкаем GEOJSON
            for (int i = 0; i < Object.cellCount(); i++)
            {
                Object.setX(i, 0, Object.getX(i, Object.vertexCount(i) - 1));
                Object.setY(i, 0, Object.getY(i, Object.vertexCount(i) - 1));

            }
            //18. Если все  - закончить расчет, если нет - вернуться к шагу №11.*/
            return JsonConvert.SerializeObject(Object);
        }
    }



}