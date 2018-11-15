#if UNITY_EDITOR
using System.IO;
using FK.JSON;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace FK.Saving
{
    /// <summary>
    /// <para>A Window to edit the config of the Save Manager</para>
    ///
    /// v1.0 11/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class SaveManagerEditor : EditorWindow
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The Config data
        /// </summary>
        private static JSONObject Config
        {
            get
            {
                if (_config == null || _config.IsNull)
                    InitConfig();

                return _config;
            }
        }

        /// <summary>
        /// Path to the config data
        /// </summary>
        private static string ConfigPath => Path.Combine(Application.streamingAssetsPath, SaveManager.CONFIG_NAME);

        
        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Backing for Config
        /// </summary>
        private static JSONObject _config;

        // ######################## UNITY EVENT FUNCTIONS ######################## //
        private void OnGUI()
        {
            GUILayout.Space(50);

            SaveManager.SaveModes saveMode = (SaveManager.SaveModes) EditorGUILayout.EnumPopup(new GUIContent("Save Location", "The parent location of the save folder"),
                (SaveManager.SaveModes) Config[SaveManager.CONFIG_SAVEMODE_KEY].IntValue);
            string folderPath = EditorGUILayout.TextField(new GUIContent("Path", "Path of the folder for the save files from the selected parent (if using CUSTOM_PATH this needs to be a full path)"),
                Config[SaveManager.CONFIG_FOLDER_PATH_KEY].StringValue);

            EditorGUILayout.Space();

            bool autoNumber = EditorGUILayout.ToggleLeft(new GUIContent("Auto Number", "Makes the save manager automatically number the saves so each save has a unique name"),
                Config[SaveManager.CONFIG_AUTONUMBER_KEY].BoolValue);
            if (autoNumber)
            {
                string numberFormat = EditorGUILayout.TextField(new GUIContent("Numberformat"), Config[SaveManager.CONFIG_NUMBERFORMAT_KEY].StringValue);

                Config.SetField(SaveManager.CONFIG_NUMBERFORMAT_KEY, numberFormat);
            }

            // set data
            Config.SetField(SaveManager.CONFIG_SAVEMODE_KEY, (int) saveMode);
            Config.SetField(SaveManager.CONFIG_FOLDER_PATH_KEY, folderPath);
            Config.SetField(SaveManager.CONFIG_AUTONUMBER_KEY, autoNumber);


            GUILayout.Space(50);
            if (GUILayout.Button("Apply"))
            {
                CheckoutConfig();
                Config.SaveToFile(ConfigPath);
            }
        }


        // ######################## INITS ######################## //
        [MenuItem("Window/Save Manager", false, 70)]
        public static void OpenSaveManagerEditor()
        {
            SaveManagerEditor window = (SaveManagerEditor) GetWindow(typeof(SaveManagerEditor));
            window.titleContent = new GUIContent("Save Manager");
            window.Show();
            InitConfig();
        }


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Loads and initializes the config
        /// </summary>
        private static void InitConfig()
        {
            // make sure streaming assets exists
            if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets"))
            {
                AssetDatabase.CreateFolder("Assets", "StreamingAssets");

                AssetDatabase.Refresh();
            }

            // loads the config or creates a new one if none exists
            try
            {
                _config = JSONObject.LoadFromFile(ConfigPath);
            }
            catch (FileNotFoundException)
            {
                _config = new JSONObject(JSONObject.Type.OBJECT);
                
                _config.AddField(SaveManager.CONFIG_SAVEMODE_KEY, (int) SaveManager.SaveModes.DOCUMENTS);
                _config.AddField(SaveManager.CONFIG_FOLDER_PATH_KEY, $"My Games/{Application.productName}");
                _config.AddField(SaveManager.CONFIG_AUTONUMBER_KEY, true);
                _config.AddField(SaveManager.CONFIG_NUMBERFORMAT_KEY, "00000");

                _config.SaveToFile(ConfigPath);
            }
        }


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Checks out the config from version control
        /// </summary>
        private void CheckoutConfig()
        {
            // if version Control is active, check out the file automatically if it is not yet checked out
            if (Provider.isActive)
            {
                // Get the version control asset of the file
                Asset configFile = Provider.GetAssetByPath($"Assets/StreamingAssets/{SaveManager.CONFIG_NAME}");

                // checkout the file if it is not checked out yet and wait until it is checked out
                if (!Provider.IsOpenForEdit(configFile))
                    Provider.Checkout(configFile, CheckoutMode.Asset).Wait();
            }
        }
    }
}
#endif