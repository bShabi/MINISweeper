using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINISwepper2
{
    public class ScoreManager
    {
        private const string c_FileName = "ScoreTable.txt";

        public ScoreManager()
        {
            if(!File.Exists(c_FileName))
               File.Create(c_FileName);
        }

        public void SetScore(PlayerScore playerScore, bool isWin)
        {
            if (!File.Exists(c_FileName))
                return;
            if (IsPlayerInFile(playerScore.Name))
            {
                UpdatePlayerScore(playerScore,isWin);
                return;
            }

             File.WriteAllText(c_FileName, playerScore.ToString());
        }
        /// <summary>
        /// Get Score Game and update
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="isWin"></param>
        private void UpdatePlayerScore(PlayerScore ps,bool isWin)
        {
            string[] arr = File.ReadAllLines(c_FileName);
            int nameChangeFileIndex = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                var spliteLine=arr[i].Split(',');
                if(ps.Name == spliteLine[(int)ePlayerScoreIndex.Name] && 
                    ps.Mode.ToString() == spliteLine[(int)ePlayerScoreIndex.Mode])
                {
                    ps.Score = int.Parse(spliteLine[(int)ePlayerScoreIndex.Score]);
                    ps.TotalGames = int.Parse(spliteLine[(int)ePlayerScoreIndex.Games]);
                    if (isWin)
                        ps.Score++;
                    ps.TotalGames++;
                    nameChangeFileIndex = i;
                    break;
                }
          
            }
            if (nameChangeFileIndex > 0)
            {
                arr[nameChangeFileIndex] = ps.ToString();
                    File.WriteAllLines(c_FileName, arr);
            }

        }

        public bool IsPlayerInFile(string name)
        {
            string[] arr = File.ReadAllLines(c_FileName);
            foreach (var line in arr)
            {
                if (line.Contains(name))
                    return true;
            }
            return false;
        }


    }

    public class PlayerScore
    {
        public string Name { get; set; }
        public eGameMode Mode { get; set; }
        public int Score { get; set; }
        public int TotalGames { get; set; }

        public override string ToString()
        {
            return $"{Name},{TotalGames},{Mode},{Score}\n";
        }
    }
    public enum ePlayerScoreIndex
    {
        Name = 0,
        Games = 1,
        Mode = 2,
        Score = 3
    }
}