using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibZepp
{
    /// <summary>
    /// Поля для названия файла
    /// </summary>
    public class Polnocvet
    {
        public const string UF_STANOK = "УФ";
        public const string UF_COLOR = "Б";

        public bool[] onOff = new bool[10];
        public string[] onOffn = { "number_0", "charId_1", "laminaciy_2", "sizeM_3", "sizeL_4", "мaterial_5", "bensity_6", "texture_7", "transparency_8", "stanok_9" };
        /// <summary>
        /// Коментарий
        /// </summary>
        public string coment { get; set; }
        /// <summary>
        /// цифры в номере
        /// </summary>
        public string number
        {
            get { return _number; }
            set
            {
                _number = value;
                if (_number != "") onOff[0] = true;
                else onOff[0] = false;
            }
        }
        string _number;
        /// <summary>
        /// Буквы в номере
        /// </summary>
        public string charId
        {
            get { return _charId; }
            set
            {
                _charId = value;
                if (_charId != null && _charId != "Б/К") onOff[1] = true;
                else onOff[1] = false;
            }
        }
        string _charId;
        public int manager_id;
        public int company;
        /// <summary>
        /// Номер проекта
        /// </summary>
        public string projectNumber { get { return charId + number; } }
        /// <summary>
        /// Ламинация (перечисление)
        /// </summary>
        public Basis laminaciy
        {
            get { return _laminaciy; }
            set
            {
                _laminaciy.transparency = value.transparency;
                _laminaciy.texture = value.texture;
                if (_laminaciy.transparency != Тransparency.Нет && _laminaciy.texture == Тexture.Нет) onOff[2] = false;
                else if (_laminaciy.transparency == Тransparency.Нет && _laminaciy.texture != Тexture.Нет) onOff[2] = false;
                else onOff[2] = true;
            }
        }

        Basis _laminaciy;
        /// <summary>
        /// Ламинация 
        /// </summary>
        public string laminaciyS
        {
            get
            {
                string str1 = "";
                if (laminaciy.transparency == Тransparency.Прозрачный_000) str1 = "000";
                if (laminaciy.transparency == Тransparency.Белый_010) str1 = "010";
                if (str1 == "") return "";

                string str2 = "";
                if (laminaciy.texture == Тexture.Глянец) str2 = "г";
                if (laminaciy.texture == Тexture.Матовый) str2 = "м";
                if (str2 == "") return "";

                return "_" + str1 + str2;
            }
        }
        /// <summary>
        /// Количество копий
        /// </summary>
        public int copyes;
        /// <summary>
        /// Размер больший
        /// </summary>
        public int sizeM
        {
            get { return _sizeM; }
            set
            {
                if (_sizeL > value)
                {
                    _sizeM = sizeL;
                    _sizeL = value;
                }
                else _sizeM = value;

                if (_sizeL <= 0) onOff[4] = false; else onOff[4] = true;
                if (_sizeM <= 0) onOff[3] = false; else onOff[3] = true;
            }
        }
        int _sizeM;
        /// <summary>
        /// Размер меньший
        /// </summary>
        public int sizeL
        {
            get { return _sizeL; }
            set
            {
                if (_sizeM < value)
                {
                    _sizeL = sizeM;
                    _sizeM = value;
                }
                else _sizeL = value;

                if (_sizeL <= 0) onOff[4] = false; else onOff[4] = true;
                if (_sizeM <= 0) onOff[3] = false; else onOff[3] = true;
            }
        }
        int _sizeL;
        /// <summary>
        /// Размеры, сначало больший затем меньший
        /// </summary>
        public string size { get { return sizeL.ToString() + "_" + sizeM.ToString(); } }
        /// <summary>
        /// Люверсы
        /// </summary>
        public int cringle
        {
            get
            {
                if (_cringle == 0)
                {
                    int col = 0;
                    string storona = "";
                    if (cringleOnL1) { col += (int)(sizeL / 300); storona += "М1"; }
                    if (cringleOnL2) { col += (int)(sizeL / 300); storona += "М2"; }
                    if (cringleOnM1) { col += (int)(sizeM / 300); storona += "Б1"; }
                    if (cringleOnM2) { col += (int)(sizeM / 300); storona += "Б2"; }
                    cringleS = col + storona;
                    return col;
                }
                else return _cringle;
            }

            set
            {
                cringleS = value + "М1М2Б1Б1";
                _cringle = value;
            }
        } //Люверсы
        int _cringle;
        /// <summary>
        /// Люверсы с указанием сторон
        /// </summary>
        public string cringleS;
        /// <summary>
        /// Имеются люверсы
        /// </summary>
        public bool cringleOnL1;
        /// <summary>
        /// Имеются люверсы
        /// </summary>
        public bool cringleOnL2;
        /// <summary>
        /// Имеются люверсы
        /// </summary>
        public bool cringleOnM1;
        /// <summary>
        /// Имеются люверсы
        /// </summary>
        public bool cringleOnM2;
        /// <summary>
        /// Склей
        /// </summary>
        public int stick
        {
            get
            {
                //if (stickOnL && stickOnM) return (sizeL + sizeM) * 2;
                //else if (stickOnL) return (sizeL);
                //else if (stickOnM) return (sizeM);
                return _stick;
            }
            set { _stick = value; }
        }
        int _stick;
        /// <summary>
        /// Имеется склей по меньшей стороне(если и та и там то stick вернет периметр)
        /// </summary>
        public bool stickOnL;
        /// <summary>
        /// Имеется склей по большей стороне (если и та и там то stick вернет периметр)
        /// </summary>
        public bool stickOnM;
        /// <summary>
        /// Поля
        /// </summary>
        public int padding;
        /// <summary>
        /// Материал (перечисление)
        /// </summary>
        public Material мaterial
        {
            get
            {
                return _мaterial;
            }
            set
            {
                _мaterial = value;
                if (_мaterial != Material.Нет || (_мaterial == Material.Нет && stanok == Stanok.Ламинация)) onOff[5] = true;
                else onOff[5] = false;
            }
        }
        Material _мaterial;
        /// <summary>
        /// Плотность банера
        /// </summary>
        public Вensity bensity
        {
            get { return _bensity; }
            set
            {
                _bensity = value;
                if (_мaterial == Material.Банер && _bensity == Вensity.Нет) onOff[6] = false;
                else onOff[6] = true;
                if (stanok == Stanok.УФ) onOff[6] = true;
            }
        }
        Вensity _bensity;
        /// <summary>
        /// Глянец, матовый
        /// </summary>
        public Тexture texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                if (transparency != Тransparency.Нет && _texture == Тexture.Нет) onOff[7] = false;
                else onOff[7] = true;
            }
        }
        Тexture _texture;
        /// <summary>
        /// Белый, прозрачный
        /// </summary>
        public Тransparency transparency
        {
            get { return _transparency; }
            set
            {
                _transparency = value;
                if (_мaterial == Material.Пленка && _transparency == Тransparency.Нет) onOff[8] = false;
                else onOff[8] = true;
                if (stanok == Stanok.УФ) onOff[8] = true;
            }
        }
        Тransparency _transparency;
        /// <summary>
        /// Материал для задания его в ручную, необходим для феникса
        /// </summary>
        public string materialS;
        /// <summary>
        /// Возвращает отформатированую строку материала
        /// </summary>
        public string GetMaterial
        {
            get
            {
                StringBuilder str = new StringBuilder();
                string tr = "";
                if ((int)this.transparency == -1) tr = "";
                if (this.transparency == Тransparency.Белый_010) tr = "010";
                if (this.transparency == Тransparency.Прозрачный_000) tr = "000";
                if (this.transparency == Тransparency.Золото_091) tr = "091";
                if (this.transparency == Тransparency.Серебро_090) tr = "090";


                string te = "";
                if ((int)this.texture != -1) te = (this.texture == Тexture.Глянец) ? "г" : "м";
                string g = tr + te;

                switch (this.stanok)
                {
                    case Stanok.Ламинация:
                        if (g.Length > 0) return str.Append("_" + g).ToString();
                        return "";
                    case Stanok.УФ:
                        return str.Append("_" + this.мaterial.ToString() + "_" + te).ToString();
                    default:
                        if (this.мaterial == Material.Банер && (int)this.bensity != -1) str.Append(this.bensity);
                        else if (this.мaterial == Material.Пленка && g.Length > 0) str.Append("_" + g);
                        else str.Append("_" + мaterial.ToString());
                        return str.ToString();

                }
            }
        }
        /// <summary>
        /// Признак наличия белого цвета, используется для УФ печати
        /// </summary>
        public bool white;
        /// <summary>
        /// Станок (перечисление)
        /// </summary>
        public Stanok stanok { get { return _stanok; } set { _stanok = value; if (_stanok != Stanok.Нет) onOff[9] = true; else onOff[9] = false; } }
        Stanok _stanok;

        /// <summary>
        /// Файл предоставленный
        /// </summary>
        public FileInfo Startfile;

        /// <summary>
        /// Отправлен ли файл на сервер
        /// </summary>
        public bool FileOnServer;

        /// <summary>
        /// Файл отправленный
        /// </summary>
        public FileInfo Sеndfile;

        /// <summary>
        /// Произошлоли форматирование и разбор имени выбранных файлов
        /// </summary>
        public bool SendFileFormat;

        /// <summary>
        /// id записи в базе
        /// </summary>
        public int id_file;

        #region Открытые методы
        /// <summary>
        /// Выдает строку для названия файла, без расширения
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            switch (stanok)
            {
                case Stanok.Ламинация:
                    str.Append("Л");
                    break;
                case Stanok.Роланд:
                    str.Append("Р");
                    break;
                case Stanok.УФ:
                    str.Append("УФ");
                    break;
                case Stanok.Феникс:
                    str.Append("Ф");
                    break;
                case Stanok.Нет:

                    break;
            }


            if (stanok == Stanok.УФ && this.transparency == Тransparency.Белый_010) str.Append("_б");
            str.Append(GetMaterial);
            if (stanok == Stanok.Роланд || stanok == Stanok.Феникс)
            {
                if (padding >= 10)  str.Append("_п" + padding); // < str.Append("_п" + "10"); else
                if (this.stick > 0) str.Append("_с" + this.stick);
                if (this.cringle > 0) str.Append("_л" + this.cringle);
            }
            str.Append("_" + this.size);

            if (this.copyes > 0) str.Append("_" + copyes+"шт.");

            str.Append(this.laminaciyS
                + "_" + this.charId
                + "_" + this.number
                + "_" + this.coment);
            if (Startfile != null) str.Append(Startfile.Extension);

            return str.ToString();
        }

        /// <summary>
        /// Разбирает строку на поля объекта
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool ParsingName(string str)
        {            

            Stanok st;
            this.Startfile = new FileInfo(str);
            //Раскладываем на массив
            string[] words = this.Startfile.Name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            words[words.Length - 1] = words[words.Length - 1].Replace(Startfile.Extension, "");

            int i = 0; // ParsMaterialTT(words, 1); - Лучше , но...

            st = Polnocvet.CompareStanok(words[0]);
            if ((int)st != -1)
            { 
                //Разбираем имя
                this.stanok = st;
                switch (this.stanok)
                {
                    case Stanok.Ламинация:
                        //Материал                        
                        if (words.Length - 1 >= 1) ParsMaterialTT(words[1]);
                        //Размеры
                        if (words.Length - 1 >= 3) ParsSize(words[2], words[3]);
                        i = 4;
                        // Количество
                        if (words.Length - 1 >= i) if (ParsCol(words[i])) i++;
                        //Номер
                        if (words.Length - 1 >= i + 1) ParsNumber(words[i], words[i + 1]); i++; i++;
                        //Коментарий
                        if (words.Length - 1 >= i) ParsComment(words, i);
                        break;

                    case Stanok.УФ:
                        i = 1;
                        // Есть ли белый цвет
                        if (words.Length - 1 >= i) if (words[1].ToUpper() == "Б") { this.white = true; i = 2; }
                        //глянец
                        if (words.Length - 1 >= i) if (words[i].ToUpper().IndexOf('Г') == words[i].Length - 1) { this.texture = Тexture.Глянец; words[i].Trim('Г', 'г'); }
                            else if (words[i].ToUpper().IndexOf('М') == words[i].Length - 1) { this.texture = Тexture.Матовый; words[i].Trim('М', 'м'); }
                            else this.texture = Тexture.Нет;
                        if (words.Length - 1 >= i) this.materialS = words[i]; i++;
                        //Размеры
                        if (words.Length - 1 >= i + 1) ParsSize(words[i], words[i + 1]); i++; i++;
                        // Количество
                        if (words.Length - 1 >= i) if (ParsCol(words[i])) i++;
                        //Номер
                        if (words.Length - 1 >= i + 1) ParsNumber(words[i], words[i + 1]); i++; i++;
                        //Коментарий
                        if (words.Length - 1 >= i) ParsComment(words, i);
                        break;

                    case Stanok.Роланд:
                    case Stanok.Феникс:
                        //Материал
                        if (words.Length - 1 >= 1) ParsMaterialTT(words[1]);
                        i = 2;
                        //Поля
                        if (words.Length - 1 >= i) if (words[i].ToUpper().IndexOf('П') > -1) { this.padding = int.Parse(words[i].Trim('п', 'П')); i++; }
                        //Склей
                        if (words.Length - 1 >= i) if (words[i].ToUpper().IndexOf('С') > -1) { this.stick = int.Parse(words[i].Trim('c', 'C', 'с', 'С')); i++; }
                        //Люверсы
                        if (words.Length - 1 >= i) if (words[i].ToUpper().IndexOf('Л') > -1) { this.cringle = int.Parse(words[i].Trim('л', 'Л')); i++; }
                        //Размеры
                        if (words.Length - 1 >= i + 1) ParsSize(words[i], words[i + 1]); i++; i++;
                        // Количество
                        if (words.Length - 1 >= i) if (ParsCol(words[i])) i++;
                        // Ламинация
                        if (words.Length - 1 >= i) if (ParsLam(words[i])) i++;
                        //Номер
                        if (words.Length - 1 >= i + 1) ParsNumber(words[i], words[i + 1]); i++; i++;
                        //Коментарий
                        if (words.Length - 1 >= i) ParsComment(words, i);
                        break;
                }

            }


            return false;
        }

        /// <summary>
        /// Констуктор инициатор пустого объекта
        /// </summary>
        public Polnocvet()
        {
            FileOnServer = false;
            this.charId = "Б/К";
            this.coment = "";
            this.materialS = "";
            this.number = "";
            this.padding = 0;
            this._laminaciy = new Basis();
            this.laminaciy = new Basis();
            this.stanok = Stanok.Нет;
            this.мaterial = Material.Нет;
            this.bensity = Вensity.Нет;
            this.transparency = Тransparency.Нет;
            this.texture = Тexture.Нет;
            this.stickOnL =
            this.stickOnM = false;
            this.manager_id = 114;
            this.company = 0;
        }

        /// <summary>
        /// Конструктор наполняет объект
        /// </summary>
        /// <param name="str"></param>
        public Polnocvet(string str) : this()
        {
            ParsingName(str);
        }

        public bool getTrue()
        {
            foreach (bool f in this.onOff) if (!f) return false;
            return true;
        }
        #endregion

        #region Закрытые функции

        /// <summary>
        /// Получает ламинацию
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool ParsLam(string word)
        {
            bool retr = false;
            if (word.ToUpper().IndexOf('Г') > -1) { this.laminaciy.texture = Тexture.Глянец; word = word.Trim('г', 'Г'); }
            else if (word.ToUpper().IndexOf('М') > -1) { this.laminaciy.texture = Тexture.Матовый; word = word.Trim('М', 'м'); }
            else this.laminaciy.texture = Тexture.Нет;

            if (word == "000") { this.laminaciy.transparency = Тransparency.Прозрачный_000; }
            else if (word == "010") { this.laminaciy.transparency = Тransparency.Белый_010; }
            else this.laminaciy.transparency = Тransparency.Нет;

            if (this.laminaciy.transparency != Тransparency.Нет && this.laminaciy.texture != Тexture.Нет) retr = true;

            return retr;
        }

        /// <summary>
        /// Получает количество копий
        /// </summary>
        /// <param name="v">соответствующий эл.</param>
        /// <returns>если удалось выделить число</returns>
        private bool ParsCol(string v)
        {
            int col;
            string s = v;
            if (v.EndsWith("шт")) s = v.Replace("шт", "");
            if (v.EndsWith("ШТ")) s = v.Replace("ШТ", "");
            if (v.EndsWith("шт.")) s = v.Replace("шт.", "");
            if (v.EndsWith("ШТ.")) s = v.Replace("ШТ.", "");
            if (Int32.TryParse(s, out col)) { this.copyes = col; return true; }
            return false;
        }

        /// <summary>
        /// Формирует комментария
        /// </summary>
        /// <param name="words">Весь разобранный масив</param>
        /// <param name="pos">Позиция начала коментария</param>
        private void ParsComment(string[] words, int pos)
        {
            for (int i = pos; i < words.Length; i++)
            {
                this.coment += "_" + words[i];
            }
        }

        /// <summary>
        /// Извлекает номер
        /// </summary>
        /// <param name="v1">Буквы</param>
        /// <param name="v2">Цифры</param>
        private void ParsNumber(string v1, string v2)
        {
            foreach (User m in Helper.managers)
            {
                if (m.project_user_id == v1) { this.charId = v1; this.manager_id = m.id; }               
            }
            this.number = v2;
        }

        /// <summary>
        /// Извлекает в поля объекта размеры изделия
        /// </summary>
        /// <param name="v">Размер1</param>
        /// <param name="v">Размер2</param>
        private void ParsSize(string wordL, string wordM)
        {
            int size;
            if (Int32.TryParse(wordM, out size)) this.sizeM = size;
            if (Int32.TryParse(wordL, out size)) this.sizeL = size;

        }
        
        /// <summary>
        /// Извелекает в поля объекта из строки свойства материала
        /// </summary>
        /// <param name="word">обозночение материала</param>
        private void ParsMaterialTT(string word)
        {
            string w = word.Trim('Г', 'г', 'М', 'м'); ;

            if (w == "000") { this.transparency = Тransparency.Прозрачный_000; this.мaterial = Material.Пленка; }
            else if (w == "010") { this.transparency = Тransparency.Белый_010; this.мaterial = Material.Пленка; }
            else if (w == "090") { this.transparency = Тransparency.Серебро_090; this.мaterial = Material.Пленка; }
            else if (w == "091") { this.transparency = Тransparency.Золото_091; this.мaterial = Material.Пленка; }
            else this.transparency = Тransparency.Нет;


            if ((word.ToUpper())[word.Length - 1] == 'Г') { this.texture = Тexture.Глянец; }
            else if ((word.ToUpper())[word.Length - 1] == 'М') { this.texture = Тexture.Матовый; }
            else this.texture = Тexture.Нет;


            if (w == "300") { this.bensity = Вensity._300; this.мaterial = Material.Банер; }
            else if (w == "440") { this.bensity = Вensity._440; this.мaterial = Material.Банер; }
            else if (w == "510") { this.bensity = Вensity._510; this.мaterial = Material.Банер; }
            else this.bensity = Вensity.Нет;
            if (this.мaterial == Material.Нет)
            {
                Material mat;
                if (Enum.TryParse(word, true, out mat)) this.мaterial = mat;
            }
        }

        #region задел
        //private int ParsMaterialTT(string[] words, int i)
        //{
        //    LibZepp.Material mat = new LibZepp.Material();

        //    if (words[0].ToUpper()[0] == UF_STANOK[0])
        //    {
        //        mat.setName("Материал-для-УФ-печати");
        //        if (words[1].ToUpper()[0] == UF_COLOR[0]) mat.setColor(010);

        //    }

        //    SearchWord(words[i], mat);
        //    if (mat.name == "") return -1;
        //    if (mat.isColor && mat.color == "")
        //    {
        //        SearchWord(words[i + 1], mat);
        //        if (mat.color != "") i++;
        //    }
        //    if (mat.isPlotnost && mat.plotnost == "")
        //    {
        //        SearchWord(words[i + 1], mat);
        //        if (mat.color != "") i++;
        //    }
        //    if (mat.isTexture && mat.texture == "")
        //    {
        //        SearchWord(words[i + 1], mat);
        //        if (mat.texture != "") i++;
        //    }
        //    //string ttt = mat.ToString();
        //    return i+1;
        //}

      /*  private static LibZepp.Material SearchWord(string word, LibZepp.Material mat)
        {
            LibZepp.Material.Osnova.getOsnovs();
            int lastI = -1;
            
            if (LibZepp.Material.Osnova.osnovs.Count > 0) // Ищем название материала
            {
                LibZepp.Material.Osnova os = SearchIndex<LibZepp.Material.Osnova, string>(word, ref lastI, LibZepp.Material.Osnova.osnovs);
                if (lastI > -1) mat.setName(os.name);

                // ищем в строке плотность
                LibZepp.Material.Plotnost pl = SearchIndex<LibZepp.Material.Plotnost, string>(word, ref lastI, LibZepp.Material.Plotnost.plotnostes);
                if (lastI > -1)
                {
                    if (mat.name == "") mat.setName("Банер");
                    mat.setPlotnost(pl.name);
                }

                // ищем в строке цвет
                LibZepp.Material.Color cl = SearchIndex<LibZepp.Material.Color, string>(word, ref lastI, LibZepp.Material.Color.colors);
                if (lastI > -1)
                {
                    if (mat.name == "") mat.setName("Пленка");
                    mat.setColor(cl.name);
                }

                // ищем в строке текстуру
                LibZepp.Material.Texture tx = SearchIndex<LibZepp.Material.Texture, string>(word, ref lastI, LibZepp.Material.Texture.textures);
                if (lastI == word.Length-1)
                {
                    if (mat.name == "") mat.setName("Пленка");
                    if (mat.color == "") mat.setColor("000");
                    mat.setTexture(tx.name);
                }

            }

            return mat;
        }*/

        /// <summary>
        /// Ищет в строке строковые индификаторы установленных типов материала
        /// </summary>
        /// <typeparam name="T">тип индентификатора</typeparam>
        /// <typeparam name="S">Строковый индентификатор</typeparam>
        /// <param name="word">Строка для поиска</param>
        /// <param name="lastI">текущий индекс входящего масива строки названия файла в печать</param>
        /// <param name="dictionary">Словарь переданного типа</param>
        /// <returns>Найденный индентификатор типа в словаре</returns>
        private static T SearchIndex< T,S >(string word, ref int lastI, Dictionary < S,T > dictionary)
        {
            int copyI = -1;
            lastI = -1;
            T dic = dictionary.Values.First<T>();
            foreach (S Key in dictionary.Keys)
            {   
                copyI = word.ToUpper().LastIndexOf(Key.ToString());
                if (copyI > lastI) { lastI = copyI; dic = dictionary[Key]; }
            }

            return dic;
        }

        static Stanok CompareStanok(string str)
        {
            Stanok[] st = { Stanok.Ламинация, Stanok.Роланд, Stanok.Феникс, Stanok.УФ };

            foreach (Stanok s in st)
            {
                if (s.ToString().ToUpper()[0] == str.ToUpper()[0])
                {
                    for (int i = 1; i < str.Length; i++)
                        if (s.ToString().ToUpper()[i] != str.ToUpper()[i]) return Stanok.Нет;

                    return s;
                }
            }

            return Stanok.Нет;
        }
        #endregion
        #endregion

        #region Внутрении Типы переменных

        /// <summary>
        /// Прозрачность материала
        /// </summary>
        public enum Тransparency { Нет = -1, Прозрачный_000 = 000, Белый_010 = 010 , Серебро_090 = 090 , Золото_091 = 091};
        /// <summary>
        /// Поверхность материала
        /// </summary>
        public enum Тexture { Нет = -1, Глянец, Матовый };

        /// <summary>
        /// плотность банера
        /// </summary>
        public enum Вensity { Нет = -1, _300 = 300, _440 = 440, _510 = 510 }
        /// <summary>
        /// Материала (перечисление)
        /// </summary>
        public enum Material
        {
            Нет = -1,
            Пленка,
            Банер,
            Банер_сетка,
            Полистер,
            Перфорированная_пленка,
            Транслюцентная_пленка,
            Обои,
            Пентопринт_молочный,
            Пентопринт_прозрачный,
            Пентопринт_пластик,
            Фотобумага,
            Постерная_бумага,
            Бэклит,
            Материал_заказчика,
        }
        /// <summary>
        /// Тип материала
        /// </summary>
        public class Basis
        {
            public Basis()
            {
                this.transparency = Тransparency.Нет;
                this.texture = Тexture.Нет;

            }
            public Тransparency transparency;
            public Тexture texture;
            //override
        }
        

        /// <summary>
        /// Пречисление станков
        /// </summary>
        public enum Stanok { Нет = -1, Роланд, Феникс, УФ, Ламинация }
        #endregion

    }
}