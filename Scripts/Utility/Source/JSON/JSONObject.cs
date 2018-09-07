using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace FK.JSON
{
    /// <summary>
    /// <para>This class allows the use of JSON Objects.</para>
    /// <para>It can load a JSON Object from a file via a static function and parse it into a usable form. You can then work with that object, access fields, change them and add new fields.</para>
    /// <para>Furthermore it can create a json string from an existing JSONObject and save that string to a file</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class JSONObject : IEnumerable
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// All possible types of a JSON Object
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// This JSONObject is a proper Object with key-value-pairs
            /// </summary>
            OBJECT,

            /// <summary>
            /// This JSONObject is an Array containing other JSONObjects
            /// </summary>
            ARRAY,

            /// <summary>
            /// This JSONObject is a value containing a string
            /// </summary>
            STRING,

            /// <summary>
            /// This JSONObject is a value containing a number (might be an integer or a floating point number)
            /// </summary>
            NUMBER,

            /// <summary>
            /// This JSONObject is a value containing a bool
            /// </summary>
            BOOL,

            /// <summary>
            /// This JSONObject is a null value
            /// </summary>
            NULL
        }

        // ######################## PROPERTIES ######################## //
        public Type ObjectType { get; private set; }

        /// <summary>
        /// If true, this JSONObject is a proper Object with key-value-pairs
        /// </summary>
        public bool IsObject => ObjectType == Type.OBJECT;

        /// <summary>
        /// If true, this JSONObject is an Array containing other JSONObjects
        /// </summary>
        public bool IsArray => ObjectType == Type.ARRAY;

        /// <summary>
        /// If true, this JSONObject is a value containing a string
        /// </summary>
        public bool IsString => ObjectType == Type.STRING;

        /// <summary>
        /// If true, this JSONObject is a value containing a number (might be an integer or a floating point number)
        /// </summary>
        public bool IsNumber => ObjectType == Type.NUMBER;

        /// <summary>
        /// If true, this JSONObject is a value containing an integer number (int or long)
        /// </summary>
        public bool IsInteger => ObjectType == Type.NUMBER && _useInt;

        /// <summary>
        /// If true, this JSONObject is a value containing a bool
        /// </summary>
        public bool IsBool => ObjectType == Type.BOOL;

        /// <summary>
        /// If true, this JSONObject is a null value
        /// </summary>
        public bool IsNull => ObjectType == Type.NULL;

        public string StringValue => _string;
        public float FloatValue => (float) _number;
        public double DoubleValue => _number;
        public int IntValue => (int) _integerNumber;
        public long LongValue => _integerNumber;
        public bool BoolValue => _bool;

        public JSONObject this[string key]
        {
            get { return GetField(key); }
            set { SetField(key, value); }
        }

        public JSONObject this[int index]
        {
            get
            {
                // if there is a list and it contains this index, return the value
                if (_list != null && _list.Count > index)
                    return _list[index];

                throw new IndexOutOfRangeException();
            }
            set
            {
                // dont add null values
                if (value == null)
                    return;

                // make sure the index exitst
                if (index > Count - 1)
                    throw new IndexOutOfRangeException();

                // if the list exists, set the value
                if (_list != null)
                    _list[index] = value;
            }
        }

        /// <summary>
        /// The amount of elements in this Object
        /// </summary>
        public int Count
        {
            get
            {
                if (_list != null)
                    return _list.Count;

                return -1;
            }
        }

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// All whitespace characters
        /// </summary>
        private static readonly char[] WHITESPACE = {' ', '\r', '\n', '\t', '\uFEFF', '\u0009'};

        /// <summary>
        /// The keys if we are a proper Object
        /// </summary>
        private List<string> _keys;

        /// <summary>
        /// The values if we are a proper Object or an Array
        /// </summary>
        private List<JSONObject> _list;


        /// <summary>
        /// The string value of this Object
        /// </summary>
        private string _string;

        /// <summary>
        /// The float value of this Object
        /// </summary>
        private double _number;

        /// <summary>
        /// If we are a number object, are we an integer?
        /// </summary>
        private bool _useInt;

        /// <summary>
        /// The integer value of this Object
        /// </summary>
        private long _integerNumber;

        /// <summary>
        /// The bool value of this Object
        /// </summary>
        private bool _bool;


        // ######################## FILE MANAGEMENT ######################## //

        #region FILE_MANAGEMENT

        /// <summary>
        /// Loads the file at the specified path and converts it to a JSONObject, which is returned
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="maxDepth">Set this if you want to stop parsing at a specific depth (0 would only parse the outmost object)</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static JSONObject LoadFromFile(string path, int maxDepth = -2)
        {
            // make sure the file exists
            if (File.Exists(path))
            {
                // Read the json from the file into a string
                string dataString = File.ReadAllText(path);
                return CreateFromString(dataString, maxDepth);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// Saves the JSONObject as a JSON formatted string into a file at the specified path
        /// </summary>
        /// <param name="path">The path at which the file should be written (inclusive file name with file ending)</param>
        /// <param name="maxDepth">Set this if you want to stop parsing at a specific depth (0 would only parse the outmost object)</param>
        public void SaveToFile(string path, int maxDepth = -2)
        {
            StringBuilder sb = new StringBuilder();

            Stringify(sb, 0, true, maxDepth);
            File.WriteAllText(path, sb.ToString());
        }

        #endregion


        // ######################## INITS ######################## //
        /// <summary>
        /// Parses the passed string as a JSON formatted string and creates an Object from it
        /// </summary>
        /// <param name="dataString">The JSON formatted string to parse</param>
        /// <param name="maxDepth">Set this if you want to stop parsing at a specific depth (0 would only parse the outmost object)</param>
        public static JSONObject CreateFromString(string dataString, int maxDepth = -2)
        {
            JSONObject obj = new JSONObject();
            obj.Parse(dataString, maxDepth);
            return obj;
        }

        #region CONSTRUCTOR

        /// <summary>
        /// Creates a NULL Object
        /// </summary>
        public JSONObject()
        {
            ObjectType = Type.NULL;
        }

        /// <summary>
        /// Creates an empty JSON Object of the given type
        /// </summary>
        /// <param name="type"></param>
        public JSONObject(Type type)
        {
            ObjectType = type;
            switch (ObjectType)
            {
                case Type.OBJECT:
                    _keys = new List<string>();
                    _list = new List<JSONObject>();
                    break;
                case Type.ARRAY:
                    _list = new List<JSONObject>();
                    break;
                case Type.STRING:
                    _string = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Creates a bool Object
        /// </summary>
        /// <param name="boolValue"></param>
        public JSONObject(bool boolValue)
        {
            ObjectType = Type.BOOL;
            _bool = boolValue;
        }

        /// <summary>
        /// Creates an integer Object
        /// </summary>
        /// <param name="intValue"></param>
        public JSONObject(int intValue)
        {
            ObjectType = Type.NUMBER;
            _useInt = true;
            _integerNumber = intValue;
        }

        /// <summary>
        /// Creates a integer Object
        /// </summary>
        /// <param name="longValue"></param>
        public JSONObject(long longValue)
        {
            ObjectType = Type.NUMBER;
            _useInt = true;
            _integerNumber = longValue;
        }

        /// <summary>
        /// Creates a floating point Object
        /// </summary>
        /// <param name="floatValue"></param>
        public JSONObject(float floatValue)
        {
            ObjectType = Type.NUMBER;
            _number = floatValue;
        }

        /// <summary>
        /// Creates a floating point Object
        /// </summary>
        /// <param name="doubleValue"></param>
        public JSONObject(double doubleValue)
        {
            ObjectType = Type.NUMBER;
            _number = doubleValue;
        }

        /// <summary>
        /// Creates a string Object
        /// </summary>
        /// <param name="stringValue"></param>
        public JSONObject(string stringValue)
        {
            ObjectType = Type.STRING;
            _string = stringValue;
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="jsonObjs"></param>
        public JSONObject(JSONObject[] jsonObjs)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < jsonObjs.Length; ++i)
            {
                _list.Add(jsonObjs[i]);
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="boolArray"></param>
        public JSONObject(bool[] boolArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < boolArray.Length; ++i)
            {
                _list.Add(new JSONObject(boolArray[i]));
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="intArray"></param>
        public JSONObject(int[] intArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < intArray.Length; ++i)
            {
                _list.Add(new JSONObject(intArray[i]));
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="longArray"></param>
        public JSONObject(long[] longArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < longArray.Length; ++i)
            {
                _list.Add(new JSONObject(longArray[i]));
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="floatArray"></param>
        public JSONObject(float[] floatArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < floatArray.Length; ++i)
            {
                _list.Add(new JSONObject(floatArray[i]));
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="doubleArray"></param>
        public JSONObject(double[] doubleArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < doubleArray.Length; ++i)
            {
                _list.Add(new JSONObject(doubleArray[i]));
            }
        }

        /// <summary>
        /// Creates an Array Object
        /// </summary>
        /// <param name="stringArray"></param>
        public JSONObject(string[] stringArray)
        {
            ObjectType = Type.ARRAY;
            _list = new List<JSONObject>();
            for (int i = 0; i < stringArray.Length; ++i)
            {
                _list.Add(new JSONObject(stringArray[i]));
            }
        }

        #endregion

        // ######################## FUNCTIONALITY ######################## //

        #region JSON_STRING_MANAGEMENT

        /// <summary>
        /// Fills the JSONObject by parsing a JSON formatted string
        /// </summary>
        /// <param name="dataString">The JSON formatted string to parse</param>
        /// <param name="maxDepth">Set this if you want to stop parsing at a specific depth (0 would only parse the outmost object)</param>
        public void Parse(string dataString, int maxDepth = -2)
        {
            // if the provided string is null or empty, this is a NULL Object
            if (string.IsNullOrEmpty(dataString))
            {
                ObjectType = Type.NULL;
                return;
            }

            // remove white spaces from the start and the end
            dataString = dataString.Trim(WHITESPACE);

            // if the first char is a quotation mark, we must be a string Object!
            if (dataString[0] == '"')
            {
                ObjectType = Type.STRING;
                // save only the contents of the string without the quotation marks
                _string = dataString.Substring(1, dataString.Length - 2);
            }
            // okay, we know that we are not a string, but if we equal the string "true" non case sensitive, we must be a bool Object!
            else if (String.Compare(dataString, "true", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ObjectType = Type.BOOL;
                _bool = true;
            }
            // okay, we know that we are not a "true" bool, but if we equal the string "false" non case sensitive, we must be a bool Object anyway!
            else if (String.Compare(dataString, "false", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ObjectType = Type.BOOL;
                _bool = false;
            }
            // so, we are not a string and not a bool, but if we equal "null" non case sensitive, we are a NULL Object!
            else if (String.Compare(dataString, "null", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ObjectType = Type.NULL;
            }
            // The only things we can be if we reach this are an Object, an Array or a Number
            else
            {
                // the start of the thing we are currently looking at
                int currentTokenStart = 1;
                // current position in the string
                int offset = 0;

                // let's look at the first character of the string
                switch (dataString[offset])
                {
                    // if the first character is a curly bracket, we are a proper Object, so we need to initialize our lists
                    case '{':
                        ObjectType = Type.OBJECT;
                        _keys = new List<string>();
                        _list = new List<JSONObject>();
                        break;
                    // if the first characzer is a square bracket, we are an Array and need to intialize our list
                    case '[':
                        ObjectType = Type.ARRAY;
                        _list = new List<JSONObject>();
                        break;
                    // okay, if we are nothing from the above, we must be a number, so lets try parsing
                    default:
                        try
                        {
                            // parse to double first
                            _number = Convert.ToDouble(dataString, CultureInfo.InvariantCulture);

                            // if the string does not contain a point, we can safely cast as an integer and mark that we are an integer
                            if (!dataString.Contains("."))
                            {
                                _integerNumber = Convert.ToInt64(dataString);
                                _useInt = true;
                            }

                            ObjectType = Type.NUMBER;
                        }
                        catch (FormatException)
                        {
                            // if we reach this something is very wrong, we have no clue what type we are so lets just be NULL
                            ObjectType = Type.NULL;
                            Debug.LogWarning($"Improper JSON Formatting: {dataString}");
                        }

                        return;
                }

                // if we reach this we are either an object or an array, so we need to get our contents
                // this is the key of the property we are currently looking at
                string propertyKey = "";
                // do we have an unresolved open quotation mark?
                bool openQuote = false;
                // are we currently looking at a property value?
                bool inProperty = false;
                // how deep inside the object are we? 0 is top level (of this particular object, note that this could be a child Object of another Object)
                int depth = 0;

                // go through the string as long as there are chars left
                while (++offset < dataString.Length)
                {
                    // continue if this char is a whitespace
                    if (Array.IndexOf(WHITESPACE, dataString[offset]) > -1)
                        continue;
                    // continue if this char is an escape char
                    if (dataString[offset] == '\\')
                    {
                        offset += 1;
                        continue;
                    }

                    // if this char is a quotation mark, we need to check if we started or ended a property key
                    if (dataString[offset] == '"')
                    {
                        // if we have an unresolved open Quote, we need to close it and maybe save the contents
                        if (openQuote)
                        {
                            // if we are an Object, not in a property and at top level, we now finished a property key, lets save it
                            if (ObjectType == Type.OBJECT && !inProperty && depth == 0)
                                propertyKey = dataString.Substring(currentTokenStart + 1, offset - currentTokenStart - 1);
                            openQuote = false;
                        }
                        // if we don't have an unresolved open Quote, we need to open one and maybe save our current position
                        else
                        {
                            // if we are an Object, and at top level, we need to save our current position as the start of the thing we are now looking at
                            if (ObjectType == Type.OBJECT && depth == 0)
                                currentTokenStart = offset;
                            openQuote = true;
                        }
                    }

                    // if there is an unresolved open quote, don't execute the code after this, go to the next position
                    if (openQuote)
                        continue;

                    // if we are an Object and at top level, lets check for a ":" which marks the start of a property value
                    if (ObjectType == Type.OBJECT && depth == 0)
                    {
                        if (dataString[offset] == ':')
                        {
                            // we are in a property, save this!
                            inProperty = true;
                            currentTokenStart = offset + 1;
                        }
                    }

                    // if this character is a open curly or square bracket, we enter a deeper level
                    if (dataString[offset] == '[' || dataString[offset] == '{')
                        ++depth;
                    // if this character is a closed curly or square bracket, we exit a deeper level
                    else if (dataString[offset] == ']' || dataString[offset] == '}')
                        --depth;

                    // if this character is a "," (which marks the end of a property) and we are at toplevel (or even farther up which would mean this object is finished),
                    // we need to deal with the property we just finished
                    if ((dataString[offset] == ',' && depth == 0) || depth < 0)
                    {
                        // we are not in a property anymore
                        inProperty = false;

                        // get the string of the property value and remove all whitespaces from it
                        string propertyString = dataString.Substring(currentTokenStart, offset - currentTokenStart).Trim(WHITESPACE);

                        // if the property string is not empty, let's deal with it
                        if (propertyString.Length > 0)
                        {
                            // if we are an Object, we need to save the property key now
                            if (ObjectType == Type.OBJECT)
                                _keys.Add(propertyKey);
                            // if we did not reach the maximum depth (this is the case if maxDepth == -1), we need to parse the property string as the JSONObject of this Property
                            if (maxDepth != -1)
                                _list.Add(CreateFromString(propertyString, maxDepth < -1 ? -2 : maxDepth - 1));
                        }

                        // now we start the next token
                        currentTokenStart = offset + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a JSON formatted string from the JSONObject
        /// </summary>
        /// <param name="stringBuilder">The String Builder to use (passed in for less Garbage)</param>
        /// <param name="depth">The depth of the current Object</param>
        /// <param name="beautify">If true this function will take a bit longer, but the resulting file will have nice indents</param>
        /// <param name="maxDepth">Set this if you want to stop parsing at a specific depth (0 would only parse the outmost object)</param>
        public void Stringify(StringBuilder stringBuilder, int depth, bool beautify, int maxDepth = -2)
        {
            switch (ObjectType)
            {
                case Type.OBJECT:
                    // start with an opening bracket
                    stringBuilder.AppendLine("{");
                    // if we are at a valid depth, continue
                    if (maxDepth != -1)
                    {
                        // go through all the contents of the object
                        for (int i = 0; i < _list.Count; ++i)
                        {
                            // add indents if desired
                            if (beautify)
                            {
                                for (int j = 0; j < depth + 1; ++j)
                                {
                                    stringBuilder.Append('\t');
                                }
                            }

                            // add the key value pair
                            stringBuilder.Append($"\"{_keys[i]}\": ");
                            _list[i].Stringify(stringBuilder, depth + 1, beautify, maxDepth < -1 ? -2 : maxDepth - 1);

                            // add a comma if we are not at the last value
                            if (i < _list.Count - 1)
                                stringBuilder.AppendLine(",");
                        }
                    }

                    // add an empty line
                    stringBuilder.AppendLine();
                    // add indents if desired
                    if (beautify)
                    {
                        for (int j = 0; j < depth; ++j)
                        {
                            stringBuilder.Append('\t');
                        }
                    }

                    // stop with a closing bracket
                    stringBuilder.Append("}");
                    break;
                case Type.ARRAY:
                    // start with an opening bracket
                    stringBuilder.AppendLine("[");
                    if (maxDepth != -1)
                    {
                        // go through all the contents of the array
                        for (int i = 0; i < _list.Count; ++i)
                        {
                            // add indents if desired
                            if (beautify)
                            {
                                for (int j = 0; j < depth + 1; ++j)
                                {
                                    stringBuilder.Append('\t');
                                }
                            }

                            // add the value
                            _list[i].Stringify(stringBuilder, depth + 1, beautify, maxDepth < -1 ? -2 : maxDepth - 1);

                            // add a comma if we are not at the last value
                            if (i < _list.Count - 1)
                                stringBuilder.AppendLine(",");
                        }
                    }

                    // add an empty line
                    stringBuilder.AppendLine();
                    // add indents if desired
                    if (beautify)
                    {
                        for (int j = 0; j < depth; ++j)
                        {
                            stringBuilder.Append('\t');
                        }
                    }

                    // stop with a closing bracket
                    stringBuilder.Append("]");
                    break;
                case Type.STRING:
                    stringBuilder.Append($"\"{_string}\"");
                    break;
                case Type.NUMBER:
                    if (_useInt)
                    {
                        stringBuilder.Append(_integerNumber);
                        break;
                    }

                    stringBuilder.Append(_number.ToString(CultureInfo.InvariantCulture));
                    break;
                case Type.BOOL:
                    stringBuilder.Append(_bool.ToString().ToLower());
                    break;
                case Type.NULL:
                    stringBuilder.Append("null");
                    break;
                default:
                    stringBuilder.Append("null");
                    break;
            }
        }

        #endregion

        #region GET_FIELD

        /// <summary>
        /// Returns the requested Field as a JSONObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JSONObject GetField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return null;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
                return _list[i];


            Debug.LogWarning($"Trying to access non existent field {key}");
            return null;
        }

        /// <summary>
        /// Returns the requested Field as a string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return null;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsString)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as a String!");
                return _list[i].StringValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return null;
        }

        /// <summary>
        /// Returns the requested Field as a float
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetFloatField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return 0;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsNumber)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as a Float!");
                return _list[i].FloatValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return 0;
        }

        /// <summary>
        /// Returns the requested Field as a Double
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double GetDoubleField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return 0;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsNumber)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as a Double!");
                return _list[i].DoubleValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return 0;
        }

        /// <summary>
        /// Returns the requested Field as a int
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIntField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return 0;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsInteger)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as an Int!");
                return _list[i].IntValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return 0;
        }

        /// <summary>
        /// Returns the requested Field as a long
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetLongField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return 0;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsInteger)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as an Int!");
                return _list[i].LongValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return 0;
        }

        /// <summary>
        /// Returns the requested Field as a bool
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBoolField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to get a field from an Object that is of type {ObjectType.ToString()}");
                return false;
            }

            int i = _keys.IndexOf(key);
            if (i >= 0)
            {
                if (!_list[i].IsBool)
                    Debug.LogWarning($"Accessing field \"{key}\" of type {_list[i].ObjectType.ToString()} as bool!");
                return _list[i].BoolValue;
            }

            Debug.LogWarning($"Trying to access non existent field \"{key}\"");
            return false;
        }

        #endregion

        #region ADD

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(JSONObject value)
        {
            if (value == null)
                return;

            if (ObjectType == Type.OBJECT)
            {
                Debug.LogError("To Add to an Object use \"AddField()\"");
                return;
            }

            if (ObjectType != Type.ARRAY)
            {
                if (_list == null)
                    _list = new List<JSONObject>();

                _keys = null;
                ObjectType = Type.ARRAY;
            }

            _list.Add(value);
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(Type value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(bool value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(int value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(long value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(float value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(double value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(JSONObject[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(bool[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(int[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(long[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(float[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(double[] value)
        {
            Add(new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided value to the object if the Object is an Array
        /// </summary>
        /// <param name="value"></param>
        public void Add(string[] value)
        {
            Add(new JSONObject(value));
        }

        #endregion

        #region ADD_FIELD

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, JSONObject value)
        {
            if (value == null)
                return;

            if (ObjectType != Type.OBJECT)
            {
                _keys = new List<string>();

                // convert to Object
                switch (ObjectType)
                {
                    case Type.ARRAY:
                        for (int i = 0; i < _list.Count; ++i)
                        {
                            _keys.Add("" + i);
                        }

                        break;
                    case Type.STRING:
                        _keys.Add("string");
                        _list = new List<JSONObject>();
                        _list.Add(new JSONObject(StringValue));
                        break;
                    case Type.NUMBER:
                        _keys.Add("number");
                        _list = new List<JSONObject>();
                        _list.Add(new JSONObject(IsInteger ? LongValue : DoubleValue));
                        break;
                    case Type.BOOL:
                        _keys.Add("bool");
                        _list = new List<JSONObject>();
                        _list.Add(new JSONObject(BoolValue));
                        break;
                }

                ObjectType = Type.OBJECT;
            }

            // add the new pair
            _keys.Add(key);
            _list.Add(value);
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, Type value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, bool value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, int value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, long value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, float value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, double value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, string value)
        {
            AddField(key, new JSONObject(value));
        }

        public void AddField(string key, JSONObject[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, bool[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, int[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, long[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, float[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, double[] value)
        {
            AddField(key, new JSONObject(value));
        }

        /// <summary>
        /// Adds the provided key value pair to the object. If the Object is not a proper JSON Object yet, this will make it one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddField(string key, string[] value)
        {
            AddField(key, new JSONObject(value));
        }

        #endregion

        #region SET_FIELD

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, JSONObject value)
        {
            if (value == null)
                return;

            if (HasField(key))
            {
                _list[_keys.IndexOf(key)] = value;
            }
            else
            {
                AddField(key, value);
            }
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, Type value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, bool value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, int value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, long value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, float value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, double value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, string value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, JSONObject[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, bool[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, int[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, long[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, float[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, double[] value)
        {
            SetField(key, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value of the provided key value pair. If the pair does not exist yet, it will be added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetField(string key, string[] value)
        {
            SetField(key, new JSONObject(value));
        }

        #endregion

        #region SET

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, JSONObject value)
        {
            this[index] = value;
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, Type value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, bool value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, int value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, long value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, float value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, double value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, string value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, JSONObject[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, bool[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, int[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, long[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, float[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, double[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        /// <summary>
        /// Sets the value at the provided Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, string[] value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            Set(index, new JSONObject(value));
        }

        #endregion

        #region REMOVE
        /// <summary>
        /// REmoves the field with the provided key from the Object
        /// </summary>
        /// <param name="key"></param>
        public void RemoveField(string key)
        {
            if (ObjectType != Type.OBJECT)
            {
                Debug.LogWarning($"Trying to remove a field from an Object that is of type {ObjectType.ToString()}");
                return;
            }

            if (HasField(key))
            {
                _keys.Remove(key);
                _list.Remove(this[key]);
                return;
            }

            Debug.LogWarning($"Cannot remove field {key} as it does not exist!");
        }

        /// <summary>
        /// Removes the field at the provided index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (ObjectType != Type.OBJECT && ObjectType != Type.ARRAY)
            {
                Debug.LogWarning($"Trying to remove from an Object that is of type {ObjectType.ToString()}");
                return;
            }

            if (index < _list.Count)
            {
                _keys?.RemoveAt(index);
                _list.RemoveAt(index);
                return;
            }

            Debug.LogWarning($"Cannot remove at index {index} as it does not exist!");
        }

        #endregion

        public bool HasField(string key)
        {
            if (ObjectType != Type.OBJECT)
                return false;

            return _keys.Contains(key);
        }


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Converts the Object to a string
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Stringify(sb, 0, false);
            return sb.ToString();
        }

        public IEnumerator GetEnumerator()
        {
            return new JSONObjectEnumerator(this);
        }
    }

    /// <summary>
    /// <para>An Enumerator for a JSONObject</para>
    ///
    /// v1.0 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class JSONObjectEnumerator : IEnumerator
    {
        // ######################## PROPERTIES ######################## //
        object IEnumerator.Current => Current;
        public JSONObject Current => _jsonObj[_position];
        
        // ######################## PRIVATE VARS ######################## //
        private JSONObject _jsonObj;

        private int _position = -1;

        
        // ######################## INITS ######################## //
        public JSONObjectEnumerator(JSONObject obj)
        {
            Debug.Assert(obj.ObjectType == JSONObject.Type.ARRAY || obj.ObjectType == JSONObject.Type.OBJECT);
            _jsonObj = obj;
        }

        // ######################## FUNCTIONALITY ######################## //
        public bool MoveNext()
        {
            ++_position;
            return (_position < _jsonObj.Count);
        }

        public void Reset()
        {
            _position = 0;
        }

        
    }
}
