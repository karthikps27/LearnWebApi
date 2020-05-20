﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using BookDataPump.Models;

namespace BookDataPump.Framework
{
    public class JsonInputReader
    {
        private StreamReader _streamReader;
        public JsonInputReader(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        public List<BookItem> GetAllDataFromJsonFile()
        {
            string jsonData = _streamReader.ReadToEnd();
            BookApiResponse bookApiResponse = JsonConvert.DeserializeObject<BookApiResponse>(jsonData);
            return bookApiResponse.Items;
        }
    }
}