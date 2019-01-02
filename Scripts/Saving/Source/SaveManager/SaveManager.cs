using System;
using System.IO;
using System.Text.RegularExpressions;
using FK.JSON;
using UnityEngine;

namespace FK.Saving
{
    /// <summary>
    /// <para>A static save manager that can load and save json formatted files from a specfied location.</para>
    /// <para>Possible Locations are:</para>
    /// <para>- The Streaming Assets Folder</para>
    /// <para>- The Documents Folder</para>
    /// <para>- The persistent data path</para>
    /// <para>- A custom path</para>
    ///
    /// v1.2 11/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class SaveManager
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// All available save modes
        /// </summary>
        public enum SaveModes
        {
            /// <summary>
            /// The parent folder will be the Streaming Assets
            /// </summary>
            STREAMING_ASSETS = 0,

            /// <summary>
            /// The parent folder will be My Documents
            /// </summary>
            DOCUMENTS = 1,

            /// <summary>
            /// The parent folder will be the persistent data path of the application
            /// </summary>
            PERSISTENT_DATA_PATH = 2,

            /// <summary>
            /// The path provided is a custom absolute path
            /// </summary>
            CUSTOM_PATH = 3,
        }

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// Is any Data loaded?
        /// </summary>
        public static bool HasData => !_data?.IsNull ?? false;

        /// <summary>
        /// The Data
        /// </summary>
        public static JSONObject Data => _data;

        // ######################## PUBLIC VARS ######################## //

        #region CONFIG_CONSTANTS

        public const string CONFIG_NAME = "SaveManagerConfig.json";
        public const string CONFIG_SAVEMODE_KEY = "SaveMode";
        public const string CONFIG_FOLDER_PATH_KEY = "FolderPath";
        public const string CONFIG_AUTONUMBER_KEY = "AutoNumber";
        public const string CONFIG_NUMBERFORMAT_KEY = "Numberformat";
        public const string CONFIG_FILEENDING_KEY = "FileEnding";

        #endregion

        /// <summary>
        /// Regular expression to match all characters invalid for file names
        /// </summary>
        public static readonly Regex INVALID_FILENAME_CHARS = new Regex("[\\\\\\/:*?\"<>|]");

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The save data
        /// </summary>
        private static JSONObject _data;

        /// <summary>
        /// The Config
        /// </summary>
        private static JSONObject _config;

        /// <summary>
        /// The parent save path defined by the Save Mode
        /// </summary>
        private static string _savePath;

        /// <summary>
        /// Number of the latest save
        /// </summary>
        private static int _lastNumber;
        
        /// <summary>
        /// All save file names
        /// </summary>
        private static string[] _files;

        // ######################## INITS ######################## //
        /// <summary>
        /// Initializes the Manager on Application load
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            // get the config
            string configPath = Path.Combine(Application.streamingAssetsPath, CONFIG_NAME);
            _config = JSONObject.LoadFromFile(configPath);

            // get the parent path dependin on the save mode
            string folderPath = _config[CONFIG_FOLDER_PATH_KEY].StringValue;
            switch ((SaveModes) _config[CONFIG_SAVEMODE_KEY].IntValue)
            {
                case SaveModes.STREAMING_ASSETS:
                    _savePath = Path.Combine(Application.streamingAssetsPath, folderPath);
                    break;
                case SaveModes.DOCUMENTS:
                    _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderPath);
                    break;
                case SaveModes.PERSISTENT_DATA_PATH:
                    _savePath = Path.Combine(Application.persistentDataPath, folderPath);
                    break;
                case SaveModes.CUSTOM_PATH:
                    _savePath = folderPath;
                    break;
                default:
                    Debug.LogError($"Invalid Save Mode \"{_config[CONFIG_SAVEMODE_KEY].IntValue}\" in SaveManagerConfig");
                    break;
            }

            // check if the save directory exists and create it if it doesn't
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);

            // get the number of the latest file
            GetSaveFileNames();
            if (_files.Length == 0)
                _lastNumber = -1;
            else
                _lastNumber = int.Parse(_files[0].Substring(0, _files[0].IndexOf('_')));
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Loads the latest save file async or creates an empty data object if there is no file
        /// </summary>
        /// <returns></returns>
        public static Coroutine LoadLatest()
        {
            GetSaveFileNames();

            // if there are no files, create a new data object
            if (_files.Length == 0)
            {
                _data = new JSONObject(JSONObject.Type.OBJECT);
                Debug.Log("There are no Files to load!");
                return null;
            }

            Debug.Log($"Loading Save File {_files[0]}");

            return LoadFile(_files[0]);
        }

        /// <summary>
        /// Loades the save file with the provided name async
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Coroutine LoadFile(string name)
        {
            _data = new JSONObject(JSONObject.Type.NULL);
            return JSONObject.LoadFromFileAsync(Path.Combine(_savePath, _files[0]), _data);
        }

        /// <summary>
        /// Saves the data to a file with the provided name. If autonumber is true the filename gets a unique number as a prefix
        /// </summary>
        /// <param name="fileName">Save File Name</param>
        /// <param name="overwrite">If autonumber is true and this is true, the number is not increased and the latest file is overwritten</param>
        public static void Save(string fileName, bool overwrite = false)
        {
            if (_config[CONFIG_AUTONUMBER_KEY].BoolValue)
            {
                fileName = $"{(overwrite ? _lastNumber : ++_lastNumber).ToString(_config[CONFIG_NUMBERFORMAT_KEY].StringValue)}_{fileName}";
            }

            fileName = $"{fileName}.{_config[CONFIG_FILEENDING_KEY].StringValue}";
            Debug.Log($"Saving to File {fileName}");

            _data.SaveToFileAsync(Path.Combine(_savePath, fileName));
        }

        /// <summary>
        /// Returns the names of all save files in the folder sorted from newest to oldest
        /// </summary>
        /// <returns></returns>
        public static string[] GetSaveFileNames()
        {
            // get all .json files
            DirectoryInfo info = new DirectoryInfo(_savePath);
            FileInfo[] files = info.GetFiles($"*.{_config[CONFIG_FILEENDING_KEY].StringValue}");

            // sort them from newest to oldest
            Array.Sort(files, (f1, f2) => f2.CreationTime.CompareTo(f1.CreationTime));

            // get the names
            _files = new string[files.Length];
            for (int i = 0; i < files.Length; ++i)
            {
                _files[i] = files[i].Name;
            }

            return _files;
        }

        // ######################## UTILITY ######################## //
        /// <summary>
        /// Removes all characters invalid for file names from a string and replaces spaces with _
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MakeFileNameSafe(this string str)
        {
            return INVALID_FILENAME_CHARS.Replace(str, "").Replace(' ', '_');
        }
    }
}