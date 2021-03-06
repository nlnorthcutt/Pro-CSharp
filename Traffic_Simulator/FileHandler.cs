﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    public class FileHandler
    {
        /// <summary>
        /// If the project has data which is yet to be saved.
        /// </summary>
        private bool _hasUnsavedData;
        //public bool HasUnsavedData
        //{
        //    get { return _hasUnsavedData; }
        //    set { _hasUnsavedData = value; }
        //}
        
        /// <summary>
        /// Location to which data is being saved.
        /// </summary>
        private string _path = "";
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Saves data to file.
        /// </summary>
        /// <param name="grid">Grid object to serialize.</param>
        /// <returns>If save was succesful.</returns>
        public bool saveToFile(Grid grid) {

            FileStream fileStream = new FileStream(_path, FileMode.Append);
            try
            {
                BinaryFormatter binaryFormater = new BinaryFormatter();
                binaryFormater.Serialize(fileStream, grid);
                _hasUnsavedData = false;
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                fileStream.Close();
            }
            //TO DO: save the object grid onto file
            return true;
        }



        /// <summary>
        /// Loads data from file.
        /// </summary>
        /// <returns>Deserialized Grid object.</returns>
        public Grid loadFromFile() { 
            
            if (string.IsNullOrEmpty(_path)) { return default(Grid); }

            Grid objectToLoad = default(Grid);

            try
            {
                Stream stream = File.Open(_path, FileMode.Open);
                BinaryFormatter binaryFormater = new BinaryFormatter();
                objectToLoad = (Grid)binaryFormater.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                throw new FileLoadException();
            }
            return objectToLoad;
            
        }
        
        public bool hasUnsavedData() 
        {
            return _hasUnsavedData;
        }

        public void setUnsavedData()
        {
            _hasUnsavedData = true;
        }
       
    }
}