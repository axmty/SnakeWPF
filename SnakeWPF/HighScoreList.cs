using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SnakeWPF
{
    public class HighScoreList : ObservableCollection<HighScore>
    {
        private static readonly string FileName = "snake_highscorelist.xml";
        private static readonly int MaxCount = 5;

        public void Load()
        {
            if (File.Exists(FileName))
            {
                var serializer = new XmlSerializer(typeof(List<HighScore>));
                using var reader = new FileStream(FileName, FileMode.Open);
                var tempList = (List<HighScore>)serializer.Deserialize(reader);

                this.Clear();
                foreach (var item in tempList.OrderByDescending(x => x.Score))
                {
                    this.Add(item);
                }

            }
        }

        public void Save()
        {
            var serializer = new XmlSerializer(this.GetType());
            using var writer = new FileStream(FileName, FileMode.Create);

            serializer.Serialize(writer, this);
        }

        public void Add(string playerName, int score)
        {
            var firstSmaller = this
                .OrderByDescending(x => x.Score)
                .FirstOrDefault(x => x.Score <= score);

            if (firstSmaller == null)
            {
                this.Add(new HighScore { PlayerName = playerName, Score = score });
            }
            else
            {
                this.Insert(
                    this.IndexOf(firstSmaller), 
                    new HighScore { PlayerName = playerName, Score = score });
            }

            while (this.Count > MaxCount)
            {
                this.RemoveAt(MaxCount);
            }
        }

        public bool CanAdd(int score)
        {
            var lowest = this.Count > 0 ? this.Min(x => x.Score) : 0;

            return score > lowest || this.Count < MaxCount;
        }
    }
}
