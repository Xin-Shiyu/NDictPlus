﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace NDictPlus.Compatibility
{
    // used for migrating from the old ndict
    public class VersionedDictionary : IDictionary<string, string>
    {
        private enum OperationType
        {
            Add,
            Set,
            Del,
        }

        private class Operation
        {
            public readonly OperationType Type;
            public readonly string Key;
            public readonly string Value;

            public Operation(OperationType type, string key, string value = null)
            {
                Type = type;
                Key = key;
                Value = value;
            }
        }

        private readonly Dictionary<string, string> cache =
            new Dictionary<string, string>();

        private readonly List<Operation> operations =
            new List<Operation>();

        public VersionedDictionary(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists) dir.Create();
            var files = dir
                .GetFiles("*.opg", SearchOption.TopDirectoryOnly)
                .OrderBy(file => 
                    DateTime.ParseExact(
                        Path.GetFileNameWithoutExtension(file.Name),
                        "yyyyMMddHHmmssffff",
                        CultureInfo.InvariantCulture));
            foreach (FileInfo file in files)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(file.FullName);
                XmlNode root = xml.SelectSingleNode("group");
                foreach (XmlElement node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "add":
                            cache.Add(node.GetAttribute("key"), node.GetAttribute("val"));
                            break;
                        case "set":
                            cache[node.GetAttribute("key")] = node.GetAttribute("val");
                            break;
                        case "del":
                            cache.Remove(node.GetAttribute("key"));
                            break;
                    }
                }
            }
        }

        public ICollection<string> Keys => cache.Keys;

        public ICollection<string> Values => cache.Values;

        public int Count => cache.Count;

        public bool IsReadOnly => false;

        public string this[string key]
        { 
            get => cache[key];
            set
            {
                cache[key] = value;
                operations.Add(new Operation(OperationType.Set, key, value));
            }
        }

        public void Add(string key, string value)
        {
            cache.Add(key, value);
            operations.Add(new Operation(OperationType.Add, key, value));
        }

        public bool ContainsKey(string key)
        {
            return cache.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            if (cache.Remove(key))
            {
                operations.Add(new Operation(OperationType.Del, key));
                return true;
            }
            return false;
        }

        public bool TryGetValue(string key, out string value)
        {
            return cache.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            cache.Add(item.Key, item.Value);
            operations.Add(new Operation(OperationType.Set, item.Key, item.Value));
        }

        public void Clear()
        {
            foreach (var key in cache.Keys) operations.Add(new Operation(OperationType.Del, key));
            cache.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)cache).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)cache).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            if(((ICollection<KeyValuePair<string, string>>)cache).Remove(item))
            {
                operations.Add(new Operation(OperationType.Del, item.Key));
                return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cache.GetEnumerator();
        }
    }
}
