using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;



namespace LibZepp
{
    /// <summary>
    /// Методы поля для работы со станком
    /// </summary>
    public class Stanok
    {
        #region Поля
         int _id;        
         string _name; 
         string _key;
         string _mats;        
         decimal _ploshad;        
         int _ispraven;
        static bool _isSets;                
        static SortedDictionary<string, Stanok> _stanokDic;
        static List<Stanok> _stanokList;
        #endregion

        #region Свойства
        /// <summary>
        /// Номер станка
        /// </summary>
        public int id { get { return _id; }  }

        /// <summary>
        /// Название станка
        /// </summary>
        public string name { get { return _name; }  }

        /// <summary>
        /// Индентификатор станка
        /// </summary>
        public string key { get { return _key; } }

        /// <summary>
        /// Список материалов подходящик для обработки на станке
        /// </summary>
        public string mats { get { return _mats; } }

        /// <summary>
        /// Площадь которую он может обработать в сутки
        /// </summary>
        public decimal ploshad { get { return _ploshad; } }

        /// <summary>
        /// Состояние станка
        /// </summary>
        public int ispraven { get { return _ispraven; } }
        /// <summary>
        /// Получен ли список станков из базы данных
        /// </summary>
        static public bool isSets { get { return _isSets; } }

        /// <summary>
        /// Набор возможных станков (содержится в базе)
        /// </summary>
        public static SortedDictionary<string, Stanok> stanokDic { get { return _stanokDic; } }

        /// <summary>
        /// Набор возможных станков (содержится в базе)
        /// </summary>
        public static List<Stanok> stanokList { get { return _stanokList; } }
        #endregion

        #region Методы
        public Stanok()
        {
            _name = "Пусто";
            _id = -1;
            _ploshad = 0;
            _ispraven = 0;
            _mats = "";
           // _isSets = false;
            getStanok();
        }
        public Stanok(string stName) : base()
        {
            getStanok();
            SetStanok(stName);
        }

        /// <summary>
        /// Наполняет словарь станоков
        /// </summary>
        /// <returns>Словарь станков из базы</returns>
        public static SortedDictionary<string, Stanok> getStanok()
        {

            //if (Stanok.stanok.Count > 0) return;
            if (stanokDic != null) return stanokDic;

            _stanokDic = new SortedDictionary<string, Stanok>();
            _stanokList = new List<Stanok>();
            _isSets = false;

            string queryString = @" SELECT `id` , `name` , `key` ,`mats` , `ploshad` , `ispraven` "
                                 + " FROM  jos_zepp_polnocvet_stanok "
                                 + " WHERE `set` = 1 AND `mats` = 1 "
            ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Stanok stanok = new Stanok();
                                string key = ((string)dr["key"]);
                                stanok._key = key;
                                stanok._name = ((string)dr["name"]);
                                stanok._id = (int)dr["id"];
                                stanok._ploshad = (decimal)dr["ploshad"];
                                stanok._ispraven = (int)dr["ispraven"];
                                stanok._mats = (string)dr["mats"];

                                _stanokDic.Add(key.ToUpper().Trim(), stanok);
                                _stanokList.Add(stanok);
                            }
                        }
                    }
                    _isSets = true;
                    return stanokDic;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Наполняет список станоков
        /// </summary>
        /// <returns>Список станков из базы</returns>
        public static List<Stanok> getListStanok()
        {
            if (stanokList != null) return stanokList;
            getStanok();
            if (stanokList != null) return stanokList;
            return null;
        }

        /// <summary>
        /// Влияетли материал на цену для этого станка
        /// </summary>
        public bool isMaterial
        {
            get
            {
                if (_mats == "1") return true;
                return false;
            }
        }

        /// <summary>
        /// Устанавливает станок
        /// </summary>
        /// <param name="stName">Имя станка</param>
        /// <returns>true - если в наличии</returns>
        public bool SetStanok(string stKey)
        {
            stKey = stKey.ToUpper().Trim();
            if (!Stanok.stanokDic.ContainsKey(stKey)) return false;
            _id = Stanok.stanokDic[stKey].id;
            _ispraven = Stanok.stanokDic[stKey].ispraven;
            _mats = Stanok.stanokDic[stKey].mats;
            _name = Stanok.stanokDic[stKey].name;
            _key = Stanok.stanokDic[stKey].key;
            _ploshad = Stanok.stanokDic[stKey].ploshad;
            return true;
        }
        public override string ToString()
        {
            if (_key == null) return null;
            return _key.ToString().ToUpper();
        }
        #endregion
    }
    public class materialProperties
    {  
        #region Свойства
        /// <summary>
        /// Номер материала
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Название материала
        /// </summary>
        public string name { get; set; }
        #endregion

        #region Методы  

        /// <summary>
        /// Словарь элементов
        /// </summary>
        /// <returns>true - Если загружен удачно из базы</returns>
      //  abstract public bool getDic();

        /// <summary>
        /// Название (поумолчанию)
        /// </summary>
        /// <returns>name</returns>
        public override string ToString()
        {
            return name.ToString();
        }
        #endregion

    }

    /// <summary>
    /// Описание материала, свойсва которыми материал может обладать, набор используемых материалов
    /// </summary>
    public class Osnova : materialProperties
    {
        #region Поля       
        int _plotnost;
        int _texture;
        int _color;
        static SortedDictionary<string, Osnova> _osnovs;
        #endregion

        #region Свойства
        /// <summary>
        /// 1- Плотность присутсвует 
        /// </summary>
        public int plotnost { get { return _plotnost; } }

        /// <summary>
        /// 1 - Текстура присутсвует
        /// </summary>       
        public int texture { get { return _texture; } }

        /// <summary>
        /// 1- Цвет  присутсвует
        /// </summary>
        public int color { get { return _color; } }

        /// <summary>
        /// Набор возможных материалов (содержится в базе)
        /// </summary>
        static public SortedDictionary<string, Osnova> osnovs { get { return _osnovs; } }
        #endregion

        #region Методы
        /// <summary>
        /// Заполняе набор существующих материалов из базы
        /// </summary>
        /// <returns></returns>
        public  Osnova()
        {
            if (Osnova.osnovs != null) return ;
            _osnovs = new SortedDictionary<string, Osnova>();
            string queryString = @" SELECT id , name , plotnost , texture , color
                                    FROM  jos_zepp_polnocvet_material 
                                    WHERE `set` = 1
                                 "
                        ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Osnova os = new Osnova();
                                String mat = ((string)dr["name"]);
                                os.name = mat;
                                os.id = (int)dr["id"];
                                os._plotnost = (int)dr["plotnost"];
                                os._texture = (int)dr["texture"];
                                os._color = (int)dr["color"];

                                _osnovs.Add(mat.Trim().ToUpper(), os);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return ;
                }
            }
            return ;
        }
        #endregion
    }

    /// <summary>
    /// Для описания плотности материала
    /// </summary>
    public class Plotnost : materialProperties
    {
        static SortedDictionary<string, Plotnost> _plotnostes;
        #region Свойства       

        /// <summary>
        /// Набор возможных (содержится в базе)
        /// </summary>
        static public SortedDictionary<string, Plotnost> plotnostes { get { return _plotnostes; } }
        #endregion

        #region Методы
        /// <summary>
        /// Читает набор используемых плотностей материалов
        /// </summary>
        public Plotnost()
        {

            if (Plotnost.plotnostes != null) return ;
            _plotnostes = new SortedDictionary<string, Plotnost>();

            string queryString = @" SELECT id , name 
                                    FROM  jos_zepp_polnocvet_plotnost 
                                    WHERE `set` = 1
                                 "
                        ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Plotnost pl = new Plotnost();
                                string mat = (string)dr["name"];
                                pl.name = mat;
                                pl.id = (int)dr["id"];

                                _plotnostes.Add(mat, pl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return ;
                }
                return ;
            }
        }
        #endregion
    }

    /// <summary>
    /// Для описания текстуры материала
    /// </summary>
    public class Texture : materialProperties
    {
        static SortedDictionary<string, Texture> _textures;
        #region Свойства
        /// <summary>
        /// Текстура - буква
        /// </summary>       
        public string simvol { get; set; }

        /// <summary>
        /// Набор возможных (содержится в базе)
        /// </summary>
        static public SortedDictionary<string, Texture> textures { get { return _textures; } }
        #endregion

        #region Методы
        /// <summary>
        /// Читает набор используемых текстур материалов
        /// </summary>
        public Texture()
        {
            if (Texture.textures != null) return;
            _textures = new SortedDictionary<string, Texture>();

            string queryString = @" SELECT id , name , simvol
                                    FROM  jos_zepp_polnocvet_texture 
                                    WHERE `set` = 1
                                 "
                        ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Texture tx = new Texture();
                                string mat = ((string)dr["simvol"]).Trim().ToUpper();
                                tx.name = (string)dr["name"];
                                tx.id = (int)dr["id"];
                                tx.simvol = mat;

                                _textures.Add(mat, tx);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return ;
                }
                return ;
            }
        }
        /// <summary>
        /// Символ обозночающий текстуру
        /// </summary>
        /// <returns>simvol</returns>
        public override string ToString()
        {
            return simvol.ToString().ToLower();
        }
        /// <summary>
        /// Название текстуры
        /// </summary>
        /// <param name="i"> любое число </param>
        /// <returns>name</returns>
        public string ToString(int i)
        {
            return name.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Для описания цвета материала
    /// </summary>
    public class ZColor : materialProperties
    {
        static SortedDictionary<string, ZColor> _colors;
        #region Свойства       
        /// <summary>
        /// Цвет материала - код
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// Набор возможных (содержится в базе)
        /// </summary>
        static public SortedDictionary<string, ZColor> colors { get { return _colors; } }
        #endregion

        #region Методы
        /// <summary>
        /// Читает набор используемых цветов материала
        /// </summary>
        public ZColor()
        {
            if (ZColor.colors != null) return;
            _colors = new SortedDictionary<string, ZColor>();
            string queryString = @" SELECT id , name , color
                                    FROM  jos_zepp_polnocvet_color 
                                    WHERE `set` = 1
                                 "
                        ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ZColor cl = new ZColor();
                                string mat = (string)dr["color"];
                                cl.name = (string)dr["name"];
                                cl.id = (int)dr["id"];
                                cl.color = mat;

                                _colors.Add(mat, cl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                return;
            }
        }

        /// <summary>
        /// Код цвета
        /// </summary>
        /// <returns>color</returns>
        public override string ToString()
        {
            return color.ToString();
        }
        /// <summary>
        /// Название цвета
        /// </summary>
        /// <param name="i">любое число</param>
        /// <returns>name</returns>
        public string ToString(int i)
        {
            return name.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Методы и свойства для работы с материалами
    /// </summary>
    public class Material
    {
        #region Поля
        Osnova _name = new Osnova();
        Plotnost _plotnost = new Plotnost();
        Texture _texture = new Texture();       
        ZColor _color = new ZColor();
        #endregion

        #region Свойства
        /// <summary>
        /// Название материала
        /// </summary>
        public string name {get {return _name.name;}}
        /// <summary>
        /// Плотность материала
        /// </summary>
        public string plotnost { get { return _plotnost.name; } }
        /// <summary>
        /// Текстура материала - буква
        /// </summary>       
        public string texture { get { return _texture.simvol; } }
        /// <summary>
        /// Текстура материала - название
        /// </summary>   
        public string textureName { get { return _texture.name; } }
        /// <summary>
        /// Цвет материала - код
        /// </summary>
        public string color { get { return _color.color; } }
        /// <summary>
        /// Цвет материала - название
        /// </summary>
        public string colorName { get { return _color.name; } }
        /// <summary>
        /// true - Если дополнительных свойств у материала не предусмотренно
        /// </summary>
        public bool isNull {

            get {
                if (this.name == null) return false;
                if (!Osnova.osnovs.ContainsKey( this.name.Trim().ToUpper() )) return false;

                if ((
                    Osnova.osnovs[this.name.Trim().ToUpper()].plotnost +
                    Osnova.osnovs[this.name.Trim().ToUpper()].texture +
                    Osnova.osnovs[this.name.Trim().ToUpper()].color
                 )

                 == 0)

                             return true;

                return false;
            } }
        /// <summary>
        /// true - Если есть плотность
        /// </summary>
        public bool isPlotnost
        {
            get
            {
                if (this.name == null) return false;
                if (!Osnova.osnovs.ContainsKey(this.name.Trim().ToUpper())) return false;
                if ( Osnova.osnovs[this.name.Trim().ToUpper()].plotnost == 1)   return true;
                return false;
            }
        }
        /// <summary>
        /// true - Если есть текстура
        /// </summary>
        public bool isTexture
        {
            get
            {
                if (this.name == null) return false;
                if (!Osnova.osnovs.ContainsKey(this.name.Trim().ToUpper())) return false;
                if (Osnova.osnovs[this.name.Trim().ToUpper()].texture == 1) return true;
                return false;
            }
        }
        /// <summary>
        /// true - Если есть цвет
        /// </summary>
        public bool isColor
        {
            get
            {
                if (this.name == null) return false;
                if (!Osnova.osnovs.ContainsKey(this.name.Trim().ToUpper())) return false;
                if (Osnova.osnovs[this.name.Trim().ToUpper()].color == 1) return true;
                return false;
            }
        }
        /// <summary>
        /// Проверяет установленные ли обязательные поля материала
        /// </summary>
        /// <returns>true если установлено</returns>
        public bool isSet() {
            int i = 0;
            if (this.name != null && this.name.Length > 0) i++;
            if (!this.isPlotnost) i++;
            else if ( this.plotnost!= null && this.plotnost.Length > 0) i++;
            if (!this.isColor) i++;
            else if( this.color != null && this.color.Length > 0) i++;
            if (!this.isTexture) i++;
            else if( this.texture != null && this.texture.Length > 0) i++;
            if (i > 3) return true;
            return false;
        }
        #endregion

        #region Методы
        /// <summary>
        /// Конструктор, заполняет поля из базы данных
        /// </summary>
        public Material()
        {
           /* getOsnovs();
            getPlotnost();
            getTexture();
            getColor();

            this._name = null;
            this._plotnost = null;
            this._texture = null;
            //this._textureName = "";
            this._color = null;
            //this._colorName = "";
            */
           
        }

        #region Установка значений по входящей строке
        /// <summary>
        /// Устанавлвает название материала
        /// </summary>
        /// <param name="name">название материала</param>
        /// <returns>Ложь если материал отсутвует в базе</returns>
        public bool setName(string name)
        {
           /* this._name = null;
            this._plotnost.name = null;
            this._texture.name = null; 
            this._texture.simvol = null;
            this._color.color = null;
            this._color.name = null;*/

            name = name.ToUpper().Trim();
            if (!Osnova.osnovs.ContainsKey(name)) return false;
            this._name = Osnova.osnovs[name];
            return true;
        }

        /// <summary>
        /// Устанавливает плотность материала
        /// </summary>
        /// <param name="pl">номер плотности</param>
        /// <returns>Ложь если для данного материала плотность не предусмотренна или указанный номер отсутсвуе в списке</returns>
        public bool setPlotnost(string pl)
        {
            this._plotnost = null;
            if (Osnova.osnovs[this.name.Trim().ToUpper()].plotnost != 1) return false;
            if (!Plotnost.plotnostes.ContainsKey(pl)) return false;
            this._plotnost = Plotnost.plotnostes[pl];
            return true;
        }

        /// <summary>
        /// Устанавливает текстуру материала
        /// </summary>
        /// <param name="name">Название текстуры</param>
        /// <returns>Ложь если для данного материала текстура не предусмотренна или указанное название отсутсвуе в списке</returns>
        public bool setTexture(string name)
        {
            this._texture = null;
            //this._textureName = "";
            name = name.ToUpper().Trim();
            if (Osnova.osnovs[this.name.Trim().ToUpper()].texture != 1) return false;

            foreach (Texture t in Texture.textures.Values)
            {
                if (t.name.ToUpper() == name)
                {
                    //this._textureName = t.name;
                    this._texture = t;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Устанавливает текстуру материала
        /// </summary>
        /// <param name="name">код текстуры</param>
        /// <returns>Ложь если для данного материала текстура не предусмотренна или указанное название отсутсвуе в списке</returns>
        public bool setTexture(char n)
        {
            this._texture = null;
           // this._textureName = "";
            if (Osnova.osnovs[this.name.Trim().ToUpper()].texture != 1) return false;

            foreach(Texture t in Texture.textures.Values)
            {
                if (t.simvol[0] == n)
                {
                    //this._textureName = t.name;
                    this._texture = t;
                    return true;
                }
            }
            return false;
           
        }

        /// <summary>
        /// Устанавливает Цвет материала
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Ложь если для данного материала цветность не предусмотренна или указанное название отсутсвуе в списке</returns>
        public bool setColor(string name)
        {
            this._color = null;
            //this._colorName = "";
            name = name.ToUpper().Trim();
            if (Osnova.osnovs[this.name.Trim().ToUpper()].color != 1) return false;
            foreach (ZColor c in ZColor.colors.Values)
            {
                if (c.name.ToUpper() == name)
                {
                    this._color = c;
                    //this._colorName = c.name;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Устанавливает Цвет материала
        /// </summary>
        /// <param name="name">Код цвета</param>
        /// <returns>Ложь если для данного материала цветность не предусмотренна или указанное название отсутсвуе в списке</returns>
        public bool setColor(int kod)
        {
            this._color = null;
            //this._colorName = "";
            if (Osnova.osnovs[this.name.Trim().ToUpper()].color != 1) return false;

            foreach(ZColor c in ZColor.colors.Values)
            {
                if (c.color == kod.ToString("000"))
                {
                    this._color = c;
                    //this._colorName = c.name;
                    return true;
                }
            }
           
            return false;
        }
        #endregion

       // #region Заполнение словарей

       // /// <summary>
       // /// Читает набор использыемых материалов
       // /// </summary>
       // private static void getOsnovs()
       // {
       //     if (Osnova.osnovs != null) return;
       //     _name.getDic();
       // }

       // /// <summary>
       // /// Читает набор используемых плотностей материалов
       // /// </summary>
       // private static void getPlotnost()
       // {

       //     if (Plotnost.plotnostes != null) return;
       //     Plotnost.plotnostes = new Dictionary<string, Plotnost>();

       //     string queryString = @" SELECT id , name 
       //                             FROM  jos_zepp_polnocvet_plotnost 
       //                             WHERE `set` = 1
       //                          "
       //                 ;

       //     using (MySqlConnection con = new MySqlConnection())
       //     {
       //         con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
       //         MySqlCommand com = new MySqlCommand(queryString, con);
       //         try
       //         {
       //             con.Open();
       //             using (MySqlDataReader dr = com.ExecuteReader())
       //             {
       //                 if (dr.HasRows)
       //                 {
       //                     while (dr.Read())
       //                     {
       //                         Plotnost pl = new Plotnost();
       //                         string mat = (string)dr["name"];
       //                         pl.name = mat;
       //                         pl.id = (int)dr["id"];

       //                         Plotnost.plotnostes.Add(mat, pl);                           
       //                     }
       //                 }
       //             }
       //         }
       //         catch (Exception ex)
       //         {
       //             MessageBox.Show(ex.Message);
       //         }
       //     }
       // }

       // /// <summary>
       // /// Читает набор используемых текстур материалов
       // /// </summary>
       // private static void getTexture()
       // {
       //     if (Texture.textures != null) return;
       //     Texture.textures = new Dictionary<string, Texture>();

       //     string queryString = @" SELECT id , name , simvol
       //                             FROM  jos_zepp_polnocvet_texture 
       //                             WHERE `set` = 1
       //                          "
       //                 ;

       //     using (MySqlConnection con = new MySqlConnection())
       //     {
       //         con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
       //         MySqlCommand com = new MySqlCommand(queryString, con);
       //         try
       //         {
       //             con.Open();
       //             using (MySqlDataReader dr = com.ExecuteReader())
       //             {
       //                 if (dr.HasRows)
       //                 {
       //                     while (dr.Read())
       //                     {
       //                         Texture tx = new Texture();
       //                         string mat = ((string)dr["simvol"]).Trim().ToUpper();
       //                         tx.name = (string)dr["name"];
       //                         tx.id = (int)dr["id"];
       //                         tx.simvol = mat;

       //                         Texture.textures.Add(mat, tx);
       //                     }
       //                 }
       //             }
       //         }
       //         catch (Exception ex)
       //         {
       //             MessageBox.Show(ex.Message);
       //         }
       //     }
       // }

       ///// <summary>
       ///// Читает набор используемых цветов материала
       ///// </summary>
       // private static void getColor()
       // {
       //     if (Color.colors != null) return;
       //     Color.colors = new Dictionary<string, Color>();
       //     string queryString = @" SELECT id , name , color
       //                             FROM  jos_zepp_polnocvet_color 
       //                             WHERE `set` = 1
       //                          "
       //                 ;

       //     using (MySqlConnection con = new MySqlConnection())
       //     {
       //         con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
       //         MySqlCommand com = new MySqlCommand(queryString, con);
       //         try
       //         {
       //             con.Open();
       //             using (MySqlDataReader dr = com.ExecuteReader())
       //             {
       //                 if (dr.HasRows)
       //                 {
       //                     while (dr.Read())
       //                     {
       //                         Color cl = new Color();
       //                         string mat = (string)dr["color"]; 
       //                         cl.name = (string)dr["name"];
       //                         cl.id = (int)dr["id"];
       //                         cl.color = mat;

       //                         Color.colors.Add(mat, cl);
       //                     }
       //                 }
       //             }
       //         }
       //         catch (Exception ex)
       //         {
       //             MessageBox.Show(ex.Message);
       //         }
       //     }
       // }
       // #endregion

        /// <summary>
        /// Возвращает форматировонную строку названия выбранного материала
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.name == null) return "";
            if (!Osnova.osnovs.ContainsKey(this.name.Trim().ToUpper())) return "";

            StringBuilder retr = new StringBuilder();
            string  g = "";

            if (this.isNull) { retr.Append("_" + this.name); g = "_"; } // Печатем название

            if (Osnova.osnovs[this.name.Trim().ToUpper()].plotnost == 1 )
            {
                if (this.plotnost != null && this.isPlotnost) retr.Append("_" + this.plotnost); // Печатем плотность
                else {  retr.Append("_" + this.name ); g = "_"; } // Если неустановлена плотность
            }

            if (Osnova.osnovs[this.name.Trim().ToUpper()].color == 1 )
            {
                if (this.name == "Материал-для-УФ-печати") { if (this.color == "010") retr.Append("_Б");g = "_"; }
                else if (this.color != null && this.isColor) retr.Append("_" + this.color.ToString()); // Печатаем цвет
                else { retr.Append("_" + this.name); g = "_"; }// Если неустановлена цветность
            }

            if (this.texture != null && this.isTexture) retr.Append(g + this.texture.ToString().ToLower()); // Печатаем глянец

            return retr.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Свойства файлов передаваемых для печати и методы для работы с ними
    /// </summary>
    public class PrintFile
    {
        #region Поля 
        public const string UF_STANOK = "У";
        public const string UF_COLOR = "Б";    
              
        int _padding;
        int _stick;
        int _cringle;
        int _sizeL;
        int _sizeM;
        int _copyes;
        string _laminaciy;
        static string _charId;
        string _number;
        string _coment;
        int _company;       

        Stanok _stanok;
        Material _material;
        FileInfo _startfile;
        FileInfo _sеndfile;        

        public static string[] lam;
        #endregion

        #region Конструкторы
        public PrintFile(Stanok _stanok, Material _material, int _padding, int _stick, int _cringle, int _siseL, int _siseM, int _copyes, string _laminaciy, string _charId, string _number, string _coment, int _company, FileInfo _startfile, FileInfo _sеndfile)
        {
            this.stanok = _stanok;
            this.material = _material;
            this.padding = _padding;
            this.stick = _stick;
            this.cringle = _cringle;
            this.sizeL = _siseL;
            this.sizeM = _siseM;
            this.copyes = _copyes;
            this.laminaciy = _laminaciy;
            this.charId = _charId;
            this.number = _number;
            this.coment = _coment;
            this.company = _company;
            this.startfile = _startfile;
            this.sеndfile = _sеndfile;            
        }
        public PrintFile() : this(null, null, 0, 0, 0, 0, 0, 0, null, null, null, null, -1,null, null)
        {}
        #endregion

        #region Свойсва
        /// <summary>
        /// Поля при печати
        /// </summary>                  
        public int padding
        {
            get
            {
                return _padding;
            }

            set
            {
                _padding = value;
            }
        }
        /// <summary>
        /// Склей
        /// </summary>
        public int stick
        {
            get
            {
                return _stick;
            }

            set
            {
                _stick = value;
            }
        }
        /// <summary>
        /// Люверсы
        /// </summary>
        public int cringle
        {
            get
            {
                return _cringle;
            }

            set
            {
                _cringle = value;
            }
        }
        /// <summary>
        /// Короткая сторона
        /// </summary>
        public int sizeL
        {
            get
            {
                return _sizeL;
            }

            set
            {
                _sizeL = value;
            }
        }
        /// <summary>
        /// Длинная сторона
        /// </summary>
        public int sizeM
        {
            get
            {
                return _sizeM;
            }

            set
            {
                _sizeM = value;
            }
        }
        /// <summary>
        /// Количество копий
        /// </summary>
        public int copyes
        {
            get
            {
                return _copyes;
            }

            set
            {
                _copyes = value;
            }
        }
        /// <summary>
        /// Материал ламинации если есть
        /// </summary>
        public string laminaciy
        {
            get
            {
                if (_laminaciy != null) return _laminaciy;
                else return "";
            }

            set
            {
                _laminaciy = value;
            }
        }
        /// <summary>
        /// Код менеджера
        /// </summary>
        public string charId
        {
            get
            {
                if (_charId != null) return  _charId;
                else return "";
                
            }

            set
            {
                _charId = value;
            }
        }
        /// <summary>
        /// Номер проекта
        /// </summary>
        public string number
        {
            get
            {
                if (_number != null) return  _number;
                else return "";
               
            }

            set
            {
                _number = value;
            }
        }
        /// <summary>
        /// Коментарий к печати
        /// </summary>
        public string coment
        {
            get
            {
                if (_coment != null) return _coment;
                else return "";                
            }

            set
            {
                _coment = value;
            }
        }
        /// <summary>
        /// Какой компании принадлежит менеджер
        /// </summary>
        public int company
        {
            get
            {
                return _company;
            }

            set
            {
                _company = value;
            }
        }
        /// <summary>
        /// На каком станке печатать
        /// </summary>
        public Stanok stanok
        {
            get
            {
                return _stanok;
            }

            set
            {
                _stanok = value;
            }
        }
        /// <summary>
        /// На каком метериале печатать
        /// </summary>
        public Material material
        {
            get
            {
                return _material;
            }

            set
            {
                _material = value;
            }
        }
        /// <summary>
        /// Файл выбранный для печати
        /// </summary>
        public FileInfo startfile
        {
            get
            {
                return _startfile;
            }

            set
            {
                _startfile = value;
            }
        }
        /// <summary>
        /// Файл сохраняемый на сервер для печати
        /// </summary>
        public FileInfo sеndfile
        {
            get
            {
                return _sеndfile;
            }

            set
            {
                _sеndfile = value;
            }
        }
        /// <summary>
        /// Былали ошибка при перезаписи файла локально с новым именем/(ошибка - false)
        /// </summary>
        public bool resaveFile = true;
        /// <summary>
        /// Сохранен ли файл на сервере
        /// </summary>
        public bool fileOnServer;
        /// <summary>
        /// id записи в базе
        /// </summary>
        public int id_file;
        #endregion

        #region Методы
        /// <summary>
        /// Проверяет установленныл важные поля отправляемых файлов
        /// </summary>
        /// <returns>true если установленно</returns>
        public bool IsSetPropertys() {
            int i = 0;
            if (this.charId.Length > 0) i++;
            if (this.stanok != null) i++;
            if (this.material != null && this.material.isSet()) i++;
            if (this.sizeL > 0) i++;
            if (this.sizeM > 0) i++;
            if (this.number.Length > 0) i++;
            if (i > 5) return true;
            return false;
        }

        #region Разбор строки
        /// <summary>
        /// Разбирает строку на поля объекта
        /// </summary>
        /// <param name="str"></param>
        /// <returns>поле на котором закончился разбор</returns>
        public int ParsingName(string str)
        {
            startfile = new FileInfo(str);
            //Раскладываем на массив
            string[] words = startfile.Name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            words[words.Length - 1] = words[words.Length - 1].Replace(startfile.Extension, "");
            // Проверяем строку в сроке должны быть: 1 - символ станка; 
            if (words.Length < 4) return -1;


            material = new Material();
            stanok = new Stanok();
            if (!Stanok.isSets) Stanok.getStanok();

/* /-+            
            ParsStanok(words);
            ParsMaterial(words);
*/ //--


            // Пытаемся разобрать строку
            int i = 0; //Указатель разбираемого поля
            // Первым должен идти всегда станок если его нет строка то строка не верна разбор прекращаем
            if ( (i = ParsStanok(words[i],i)) < 0 ) return i;            
            // Если уф печать, ищем признак белого цвета
            if (stanok.key != null && stanok.key[0] == UF_STANOK[0] && words[i].Length == 1 && words[i] == UF_COLOR) { material.setColor(010); i++; } 
            //- Ищем материал
            i = ParsMaterial(words, i);            
                       
            i = ParsPading(words, i);
            i = ParsSkley(words, i);
            i = ParsLuvers(words, i);

            {
                int j = i;
                i = ParsSize(words, i);
                if (i == j) return i * (-1);
            }

            i = ParsCol(words, i);
            i = ParsLam(words, i);
            i = ParsNumber(words, i);
            i = ParsComment(words, i);

            if (i < 5) return i * (-1); 
            sеndfile = new FileInfo(this.ToString());
            return i;
        }      

        private int ParsStanok(string s,int i)
        {
            if (s.Length > 3 || s.Length == 0) return -1; // индентификатор станка одна буква               
            SortedDictionary<string, Stanok> dicStanok = Stanok.getStanok();
            if (s.ToUpper()[0] == UF_STANOK[0])
            {
                stanok.SetStanok("У");
                return i+1;
            }

            foreach (string keys in dicStanok.Keys)
            {
                if (s.ToUpper()[0] == keys[0])
                {
                    stanok.SetStanok(keys);
                    return i+1;
                }
            }
            
            return -1;            
        }
        private int ParsStanok(string[] s) {
           int retr = -1;            
           for(int n = s.Length - 1; n >= 0; n--) {
              if(ParsStanok(s[n], -1) >= 0) retr = n;                
            }
            return retr;
        }

        private int ParsMaterial(string[] words, int i)
        {  
            if (words.Length - 1 >= i)
            {
                SearchWord(words[i], material);
                if (material.name == null) return i;
                if (words.Length - 1 >= i + 1)
                {
                    if (material.isColor && material.color == null)
                    {
                        SearchWord(words[i + 1], material);
                        if (material.color != null) i++;
                    }
                    if (material.isPlotnost && material.plotnost == null)
                    {
                        SearchWord(words[i + 1], material);
                        if (material.plotnost != null) i++;
                    }
                    if (material.isTexture && material.texture == null)
                    {
                        SearchWord(words[i + 1], material);
                        if (material.texture != null) i++;
                    }
                }
                return i + 1;
            }
            
            return i;
        }
        private int ParsMaterial(string[] words) {
            int retr = -1;
            Material mat = new Material();
            for (int n = words.Length - 1; n >= 0; n--) {
                SearchWord(words[n], mat);
                if (mat.name != null) retr = n;                
            }
            if (mat.name != null)  material.setName(mat.name);
            if (mat.color != null) material.setColor(mat.color);
            if (mat.plotnost != null) material.setPlotnost(mat.plotnost);
            if (mat.texture != null) material.setTexture(mat.texture);

            return retr;
        }

        private int ParsPading(string[] words, int i)
        {
            int p = 0;
            if (words.Length - 1 >= i)
                if (words[i].ToUpper().IndexOf('П') > -1)
                    if (int.TryParse(words[i].Trim('п', 'П'), out p)) {
                        padding = p; // int.Parse(words[i].Trim('п', 'П'));
                        i++;
                    }

            return i;
        }

        private int ParsLuvers(string[] words, int i)
        {
            //Люверсы
            int p = 0;
            if (words.Length - 1 >= i)
                if (words[i].ToUpper().IndexOf('Л') > -1)
                    if (int.TryParse(words[i].Trim('л', 'Л'), out p)) {
                        cringle = p; // int.Parse(words[i].Trim('л', 'Л'));
                        i++;
                    }                

            return i;
        }

        private int ParsSkley(string[] words, int i)
        {
            //Склей
            int p = 0;
            if (words.Length - 1 >= i)
                if (words[i].ToUpper().IndexOf('С') > -1)
                    if (int.TryParse(words[i].Trim('c', 'C', 'с', 'С'), out p)) {
                        stick = p; // int.Parse(words[i].Trim('c', 'C', 'с', 'С'));
                        i++;
                    }
            return i;
        }

        private int ParsSize(string[] words, int i)
        {
            if (words.Length - 1 >= i + 1)
            {
                int size;
                if (int.TryParse(words[i], out size)) _sizeM = size;
                if (int.TryParse(words[i + 1], out size)) _sizeL = size;
                if (_sizeL > _sizeM) { int j = _sizeM; _sizeM = _sizeL; _sizeL = j; }
                i += 2;
            }
            return i;
        }

        private int ParsCol(string[] words, int i)
        {
            if (words.Length - 1 >= i)
            {
                int col;
                string s = words[i];
                string v = s;
                if (v.EndsWith("шт")) s = v.Replace("шт", "");
                if (v.EndsWith("ШТ")) s = v.Replace("ШТ", "");
                if (v.EndsWith("шт.")) s = v.Replace("шт.", "");
                if (v.EndsWith("ШТ.")) s = v.Replace("ШТ.", "");
                if (int.TryParse(s, out col)) { copyes = col; return i++; }
            }
            return i;
        }

        private int ParsLam(string[] words, int i)
        {
            if (words.Length - 1 >= i)
            {
                string word = words[i];
                List<string> listLam = lam.ToList();
                _laminaciy = listLam.Find(x => x == word);
                if (_laminaciy != null) i++;
            }
            return i;
        }

         private int ParsNumber(string[] words, int i)
        {
            if (words.Length - 1 >= i)
                if (Helper.managers.FindIndex(x => x.project_user_id == words[i]) > 0) i++;

            int r;
            if (words.Length - 1 >= i)
                if (int.TryParse(words[i], out r)) {
                    number = r.ToString();
                    i++;
                }
            
            return i;
        }

        private int ParsComment(string[] words, int i)
        {
            if (words.Length - 1 >= i)
                for (int j = i; j < words.Length; j++)
                {
                    coment += "_" + words[j];
                    i++;
                }
            return i;
        }

        private static Material SearchWord(string word, Material mat)
        {
            //Material.Osnova.getOsnovs();
            int lastI = -1;

            if (Osnova.osnovs.Count > 0 && word.Length > 2 && word.Length < 5) // Ищем название материала
            {
                Osnova os = SearchIndex(word, ref lastI, Osnova.osnovs);
                if (lastI > -1) mat.setName(os.name);

                // ищем в строке плотность
                Plotnost pl = SearchIndex(word, ref lastI, Plotnost.plotnostes);
                if (lastI > -1)
                {
                    if (mat.name == null) mat.setName("Банер");
                    mat.setPlotnost(pl.name);
                }

                // Ищем цвет и плотность
                //SearchColor(word, mat);

                // ищем в строке цвет
                ZColor cl = SearchIndex(word, ref lastI, ZColor.colors);
                if (lastI > -1)
                {
                    if (mat.name == null) mat.setName("Пленка");
                    mat.setColor(cl.name);
                }

                // ищем в строке текстуру
                Texture tx = SearchIndex(word, ref lastI, Texture.textures);
                if (lastI == word.Length - 1)
                {
                    if (mat.name == null) mat.setName("Пленка");
                    if (mat.color == null) mat.setColor("000");
                    mat.setTexture(tx.name);
                }

            }

            return mat;
        }

        /// <summary>
        /// Ищет в строке строковые индификаторы установленных типов материала
        /// </summary>
        /// <typeparam name="T">тип индентификатора</typeparam>
        /// <typeparam name="S">Строковый индентификатор</typeparam>
        /// <param name="word">Строка для поиска</param>
        /// <param name="lastI">текущий индекс входящего масива строки названия файла в печать</param>
        /// <param name="dictionary">Словарь переданного типа</param>
        /// <returns>Найденный индентификатор типа в словаре</returns>
        private static T SearchIndex<T, S>(string word, ref int lastI, SortedDictionary<S, T> dictionary)
        {
           int copyI = -1;
           lastI = -1;
            T dic = dictionary.Values.First<T>();
            foreach (S Key in dictionary.Keys) {
               copyI = word.ToUpper().LastIndexOf(Key.ToString());               
               if (copyI > lastI) { lastI = copyI; dic = dictionary[Key]; }
               /* if (word.ToUpper().Trim() == Key.ToString()) {
                    dic = dictionary[Key];
                    lastI = 0;
                }*/

            }

            return dic;
        }
        private static void SearchColor(string word, Material mat) {

            foreach (string cl in ZColor.colors.Keys)
                foreach (string tx in Texture.textures.Keys) {
                    if (word.ToUpper().Trim() == cl + tx) {
                        mat.setColor(cl);
                        mat.setTexture(tx);
                    }
                }


        }
        #endregion
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(stanok);
            str.Append(material);
            if ( padding > 0 ) str.Append("_п"+padding);
            if ( stick > 0 ) str.Append("_с" + stick);
            if ( cringle > 0 ) str.Append("_л" + cringle);
            if (sizeL > 0) str.Append("_" + sizeL);
            if (sizeM > 0) str.Append("_" + sizeM);
            if (copyes > 0) str.Append("_" + copyes);
            if (laminaciy.Length > 0) str.Append("_" + laminaciy);
            str.Append("_" + charId);
            str.Append("_" + number);
            if (coment.Length > 0) str.Append("_" + coment);
            if (startfile != null) str.Append(startfile.Extension);

            return str.ToString();
        }
        
        public static bool errSave = false;
        public static User selectManager;
        #endregion
    }

}
