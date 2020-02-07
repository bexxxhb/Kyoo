﻿using Newtonsoft.Json;

namespace Kyoo.Models
{
    public class Genre
    {
        [JsonIgnore] public long ID { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public Genre(string slug, string name)
        {
            Slug = slug;
            Name = name;
        }

        public Genre(long id, string slug, string name)
        {
            ID = id;
            Slug = slug;
            Name = name;
        }

        public static Genre FromReader(System.Data.SQLite.SQLiteDataReader reader)
        {
            return new Genre((long)reader["id"],
                reader["slug"] as string,
                reader["name"] as string);
        }
    }
}